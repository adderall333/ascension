namespace Ascension.Controller

open Ascension.ProductFilter
open System.Diagnostics
open System.Linq
open Microsoft.AspNetCore.Mvc
open Microsoft.EntityFrameworkCore
open Models

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
                           .AsSplitQuery()
                           .ToList()
                           
        if products.Count = 0
        then
            errorHandling this
        else    
            this.View(products.First())