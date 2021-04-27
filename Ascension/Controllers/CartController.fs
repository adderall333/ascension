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
    member this.RemoveProductLine(productId: int) =
        use context = new ApplicationContext()
        let cartId = getCartId this.HttpContext

        context.ProductLine.Remove(ProductLine(cartId, productId))
        |> ignore

        context.SaveChanges() |> ignore

    [<HttpPost>]
    member this.AddProduct(productId: int) = None
    //проверку на существование продуктлайна не забыть, если его нету ничего не делать

    [<HttpPost>]
    member this.RemoveProduct(productId: int) = None
//и тут проверку такую же
