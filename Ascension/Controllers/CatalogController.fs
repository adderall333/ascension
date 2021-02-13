namespace Ascension

open System.Collections.Generic
open System.Linq
open Microsoft.AspNetCore.Mvc
open Microsoft.EntityFrameworkCore
open Models

type CatalogController() =
    inherit Controller()
    
    let getProductsQuery (context : ApplicationContext) (category : string) =
        context
            .Product
            .Where(fun p -> p.Category.Name = category)
            .Include(fun p -> p.Images)
            .Include(fun p -> p.SpecificationOptions)
            .Include(fun p -> p.Category)
    
    
    let getProducts (context : ApplicationContext) (category : string) (options : List<SpecificationOption>) =
        let productsQuery = getProductsQuery context category
        if options = null
        then
            productsQuery
                .ToList()
        else
            productsQuery
                .Where(fun p -> p
                                    .SpecificationOptions
                                    .All(fun sOp -> options.Contains(sOp)))
                .ToList()
      
    let getRequiredOptions (context : ApplicationContext) (category : string) (ids : List<int>) =
        if ids = null
        then
            null
        else
            let specifications = context
                                     .Specification
                                     .Where(fun s -> s.Category.Name = category)
                                     .Include(fun s -> s.SpecificationOptions)
                                     .ToList()
            let checkedOptions = context
                                     .SpecificationOption
                                     .Where(fun sOp -> ids.Contains(sOp.Id))
                                     .ToList()
            specifications
                .Where(fun s -> not (s
                                    .SpecificationOptions
                                    .Any(fun sOp -> ids.Contains(sOp.Id))))
                .SelectMany(fun s -> s.SpecificationOptions)
                .Concat(checkedOptions)
                .ToList()
            
    let sortProducts (sortOption : string) (products : List<Product>) =
        if sortOption = null
        then
            products
        else
            match sortOption with
            | "cheapFirst" -> products.OrderBy(fun p -> p.Cost).ToList()
            | "expensiveFirst" -> products.OrderByDescending(fun p -> p.Cost).ToList()
            | "alphabet" -> products.OrderBy(fun p -> p.Name).ToList()
            | _ -> failwith "There is no such sort option"
        
    let parseIds (ids : string) =
        if ids = null
        then
            null
        else
            ids
                .Split(",")
                .Select(fun id -> id |> int)
                .ToList()

    member this.Index() =
        use context = new ApplicationContext()
        let model = context
                        .SuperCategory
                        .Include(fun sc -> sc.Categories)
                        .ToList()
        this.View(model)
    
    member this.Category(name) =
        use context = new ApplicationContext()
        let model = context
                        .Category
                        .Where(fun c -> c.Name = name)
                        .Include(fun c -> c.Products)
                        .ThenInclude(fun (p:Product) -> p.Images)
                        .Include(fun c -> c.Specifications)
                        .ThenInclude(fun (s:Specification) -> s.SpecificationOptions)
                        .AsSplitQuery()
                        .First()
        this.View(model)
        
    member this.GetProducts(sortOption : string, category : string, ids : string) =
        use context = new ApplicationContext()
        let products = ids
                            |> parseIds
                            |> getRequiredOptions context category
                            |> getProducts context category
                            |> sortProducts sortOption
        this.PartialView("ProductsPartial", products)
            
        
    member this.Product() =
        this.View()