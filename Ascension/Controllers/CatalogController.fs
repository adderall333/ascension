namespace Ascension

open System
open System.Collections.Generic
open System.Diagnostics
open System.Linq
open Microsoft.AspNetCore.Mvc
open Microsoft.EntityFrameworkCore
open Models

type CatalogController() =
    inherit Controller()
    
    let getProducts (context : ApplicationContext) (category : string) (options : List<SpecificationOption>) =
        let productsQuery = context
                                .Product
                                .Where(fun p -> p.Category.Name = category)
                                .Include(fun p -> p.Images)
                                .Include(fun p -> p.SpecificationOptions)
                                .Include(fun p -> p.Category)
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
                        .AsSplitQuery()
                        .ToList()
        this.View(model)
    
    member this.Category(name) =
        use context = new ApplicationContext()
        let model = context
                        .Category
                        .Where(fun c -> c.Name = name)
                        .Include(fun c -> c.Specifications)
                        .ThenInclude(fun (s:Specification) -> s.SpecificationOptions)
                        .AsSplitQuery()
                        .ToList()
        if not (model.Any())
        then
            this.Response.StatusCode <- 404
            let reqId = 
                if isNull Activity.Current then
                    this.HttpContext.TraceIdentifier
                else
                    Activity.Current.Id
            this.View("Error", ErrorViewModel(reqId))
        else
            this.View(model.First())
        
    member this.GetProducts(category : string, sortOption : string,  ids : string) =
        use context = new ApplicationContext()
        if category = null
        then
            let products = context
                               .Product
                               .Include(fun p -> p.Images)
                               .AsSplitQuery()
                               .ToList()
            this.PartialView("ProductsPartial", products)
        else
            let products = ids
                                |> parseIds
                                |> getRequiredOptions context category
                                |> getProducts context category
                                |> sortProducts sortOption
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
            this.Response.StatusCode <- 404
            let reqId = 
                if isNull Activity.Current then
                    this.HttpContext.TraceIdentifier
                else
                    Activity.Current.Id
            this.View("Error", ErrorViewModel(reqId))
        else    
            this.View(products.First())