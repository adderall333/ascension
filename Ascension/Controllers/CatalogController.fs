namespace Ascension

open System
open System.Collections.Generic
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Routing
open ProductFilter
open System.Diagnostics
open System.Linq
open Microsoft.AspNetCore.Mvc
open Microsoft.EntityFrameworkCore
open Models
open CartService

type CatalogController() =
    inherit Controller()
    
    let search (context : ApplicationContext) (searchString : string) =
        if searchString <> null && searchString <> ""
        then
            context
                .Product
                .Where(fun p -> p.SearchVector.Matches(searchString))
                .ToList()
        else
            null
        
    let errorHandling (this : Controller) =
        this.Response.StatusCode <- 404
        let reqId = 
            if isNull Activity.Current then
                this.HttpContext.TraceIdentifier
            else
                Activity.Current.Id
        this.View("Error", ErrorViewModel(reqId))
    
    member this.Index(searchString : string) =
        use context = new ApplicationContext()
        let model = context
                            .SuperCategory
                            .Include(fun sc -> sc.Categories)
                            .ToList()
        if (searchString <> null) && (searchString <> "")
        then
            this.ViewData.Add("searchString", searchString)
            let searchResult = context
                                   .Product
                                   .Where(fun p -> p.SearchVector.Matches(searchString))
                                   .Select(fun p -> p.Category)
                                   .Distinct()
                                   .ToList()
            this.ViewData.Add("searchResult", searchResult)
        this.View(model)
        
    
    
    member this.Category(name) =
        use context = new ApplicationContext()
        let category = context
                           .Category
                           .FirstOrDefault(fun c -> c.Name = name)
        if category = null
        then
            errorHandling this
        else
            category.Specifications <- context
                                     .Specification
                                     .Where(fun s -> s.CategoryId = category.Id)
                                     .ToList()
            for specification in category.Specifications do
                specification.SpecificationOptions <- context
                                                          .SpecificationOption
                                                          .Where(fun s -> s.SpecificationId = specification.Id)
                                                          .ToList()
            this.View(category)
        
    member this.GetProducts(category : string, sortOption : string,  ids : string, searchString : string) =
        use context = new ApplicationContext()
        if category = null
        then
            null
        else
            let products = searchString
                           |> search context  
                           |> selectByCategory context category
                           |> filter context ids
                           |> sortProducts sortOption
                           |> loadImages context
                           |> loadRating context
                           |> loadIsInCart context this.HttpContext
            this.PartialView("ProductsPartial", products)
            
    member this.Product(id : int) =
        use context = new ApplicationContext()
        let products = context
                           .Product
                           .Where(fun p -> p.Id = id)
                           .Include(fun p -> p.Images)
                           .Include(fun p -> p.SpecificationOptions)
                           .ThenInclude(fun (sOp:SpecificationOption) -> sOp.Specification)
                           .Include(fun p -> p.Category)
                           .Include(fun p -> p.Reviews)
                           .ThenInclude(fun (r:Review) -> r.User)
                           .Include(fun p -> p.Rating)
                           .AsSplitQuery()
                           .ToList()               
                           
        if products.Count = 0
        then
            errorHandling this
        else
            let product = products.First()
            product.Purchases <- context
                                    .Purchase
                                    .Where(fun p -> p.FirstProductId = id)
                                    .Include(fun p -> p.SecondProduct)
                                    .ThenInclude(fun p -> p.Images)
                                    .OrderByDescending(fun p -> p.Count)
                                    .Take(5)
                                    .ToList()
                                    
            product.IsInCart <- isInCart product this.HttpContext context
            
            for secondProduct in product.Purchases.Select(fun p -> p.SecondProduct) do
                secondProduct.IsInCart <- isInCart secondProduct this.HttpContext context
            
            this.View(product)

    [<HttpPost>]        
    member this.AddReview(review : ReviewToAdd) =
        use context = new ApplicationContext()
        
        let userId = this.HttpContext.Session.GetInt32("id")
        let mutable userIdValue = 0
        if userId.HasValue then userIdValue <- userId.Value 
        
        let newReview = new Review()
        newReview.Comment <- review.Text
        newReview.Rating <- review.Rating
        newReview.PublicationDate <- DateTime.Now
        newReview.User <- context
                              .User
                              .Where(fun u -> u.Id = userIdValue)
                              .First()
        newReview.Product <- context
                                 .Product
                                 .Where(fun p -> p.Id = review.ProdId)
                                 .First()                   
        context.Review.Add(newReview) |> ignore
        
        let prodRating = context.ProductRating.Where(fun p -> p.ProductId = review.ProdId).FirstOrDefault()
        if isNull prodRating
        then
            let newProdRating = new ProductRating()
            newProdRating.Sum <- review.Rating
            newProdRating.Count <- 1
            newProdRating.ProductId <- review.ProdId
            context.ProductRating.Add(newProdRating) |> ignore
        else
            prodRating.Sum <- prodRating.Sum + review.Rating
            prodRating.Count <- prodRating.Count + 1 
           
        context.SaveChanges() 
    
    [<HttpPost>]    
    member this.EditReview(review : ReviewToAdd) =
        use context = new ApplicationContext()
        
        let userId = this.HttpContext.Session.GetInt32("id")
        let mutable userIdValue = 0
        if userId.HasValue then userIdValue <- userId.Value
        
        let reviewToEdit = context
                               .Review
                               .Where(fun r -> r.Product.Id = review.ProdId)
                               .Where(fun r -> r.User.Id = userIdValue)
                               .FirstOrDefault()
        let prevRating = reviewToEdit.Rating
        reviewToEdit.Comment <- review.Text
        reviewToEdit.Rating <- review.Rating
        reviewToEdit.PublicationDate <- DateTime.Now
        
        let prodRatingToEdit = context
                                   .ProductRating
                                   .Where(fun p -> p.ProductId = review.ProdId)
                                   .FirstOrDefault()
        prodRatingToEdit.Sum <- prodRatingToEdit.Sum - prevRating + review.Rating
        
        context.SaveChanges()
       
    [<HttpPost>]    
    member this.DeleteReview(productId : int, reviewId : int) =
        use context = new ApplicationContext()
       
        let reviewToDelete = context
                                 .Review
                                 .Where(fun r -> r.Id = reviewId)
                                 .FirstOrDefault()
        let prevRating = reviewToDelete.Rating
        context.Review.Remove(reviewToDelete) |> ignore
        
        let prodRatingToEdit = context
                                   .ProductRating
                                   .Where(fun p -> p.ProductId = productId)
                                   .FirstOrDefault()
        prodRatingToEdit.Sum <- prodRatingToEdit.Sum - prevRating
        prodRatingToEdit.Count <- prodRatingToEdit.Count - 1
                                   
        context.SaveChanges() |> ignore
        
        this.Redirect($"/Catalog/Product/{productId}")