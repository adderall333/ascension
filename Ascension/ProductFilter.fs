namespace Ascension

open Models
open System.Linq
open System.Collections.Generic
open Microsoft.EntityFrameworkCore

module ProductFilter =
    
    let parse (ids : string) =
        if ids = null
        then
            null
        else
            ids
                .Split(",")
                .Select(fun id -> id |> int)
                .ToList()
    
    let getRequiredOptionsIds (context : ApplicationContext) (categoryId : int) (checkedOptionsIds : List<int>) =
        let specificationsIds = context
                                        .Specification
                                        .Where(fun s -> s.CategoryId = categoryId)
                                        .Select(fun s -> s.Id)
                                        .ToList()
        let requiredOptions = context
                                     .SpecificationOption
                                     .Where(fun s -> specificationsIds.Contains(s.SpecificationId))
                                     .ToList()
        if checkedOptionsIds = null
        then
            requiredOptions
                .Select(fun s -> s.Id)
                .ToList()
        else
            requiredOptions
                .GroupBy(fun s -> s.SpecificationId)
                .Where(fun g -> not (g.Any(fun s -> checkedOptionsIds.Contains(s.Id))))
                .SelectMany(fun g -> g.AsEnumerable())
                .Select(fun s -> s.Id)
                .Concat(checkedOptionsIds)
                .ToList()
                
    let filterProducts (context : ApplicationContext) (products : List<Product>) (requiredOptionsIds : List<int>) =
        for product in products do
            product.SpecificationOptions <- context
                                                .SpecificationOption
                                                .Where(fun s -> s.Products.Contains(product))
                                                .AsSplitQuery()
        products
            .Where(fun p -> p
                                .SpecificationOptions
                                .All(fun s -> requiredOptionsIds.Contains(s.Id)))
            .ToList()
    
    let selectByCategory (context : ApplicationContext) (categoryName : string) =
        let category = context
                           .Category
                           .FirstOrDefault(fun c -> c.Name = categoryName)
        if category = null
        then
            null
        else
            context
                   .Product
                   .Where(fun p -> p.CategoryId = category.Id)
                   .ToList()
            
    let filter (context : ApplicationContext) (checkedOptions : string) (products : List<Product>) =
        if products = null
        then
            null
        else
            let categoryId = products.First().CategoryId
            checkedOptions
            |> parse
            |> getRequiredOptionsIds context categoryId
            |> filterProducts context products
            
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
        
    let loadImages (context : ApplicationContext) (products : List<Product>) =
        for product in products do
            let images = context
                             .Image
                             .Where(fun i -> i.ProductId = product.Id)
                             .OrderBy(fun i -> i.Id)
                             .Take(1)
                             .ToList()
            product.Images <- images
        products