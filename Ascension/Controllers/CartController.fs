namespace Ascension.Controllers

open System.Linq

open Ascension
open Microsoft.AspNetCore.Mvc
open Models
open Microsoft.EntityFrameworkCore
open CartService

type CartController() =
    inherit Controller()

    [<HttpGet>]
    member this.Cart() =
        use context = new ApplicationContext()
        let cartId = getCartId this.HttpContext
        let productLines = context.ProductLine.Where(fun p -> p.CartId = cartId).ToList()
        
        for productLine in productLines do
            productLine.Product <- context
                                      .Product
                                      .Where(fun p -> p.Id = productLine.ProductId)
                                      .Include(fun p -> p.Images)
                                      .First() 
        
        this.View(productLines)
        

    [<HttpPost>]
    member this.AddProductLine(productId: int) =
        use context = new ApplicationContext()
        let cartId = getCartId this.HttpContext

        context.ProductLine.Add(ProductLine(cartId, productId))
        |> ignore

        context.SaveChanges() |> ignore

    [<HttpPost>]
    member this.RemoveProductLine(productLineId: int) =
        use context = new ApplicationContext()
        let productLineRemove = context.ProductLine.First(fun x -> x.Id = productLineId)
        context.ProductLine.Remove(productLineRemove)|> ignore
        context.SaveChanges() |> ignore

    [<HttpPost>]
    member this.AddProduct(productId: int) =
        use context = new ApplicationContext()
        let productCountAdd = context.ProductLine.Where(fun x -> x.Id = productId).Select(fun y -> y.ProductCount+1)
        context.SaveChanges() |> ignore

    [<HttpPost>]
    member this.RemoveProduct(productId: int) = 
     use context = new ApplicationContext()
     let productCountAdd = context.ProductLine.Where(fun x -> x.Id = productId).Select(fun y -> y.ProductCount-1)
     context.SaveChanges() |> ignore
