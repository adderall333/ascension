namespace Ascension.Controllers

open System.Linq

open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Models

type CartController () =
    inherit Controller()
    
    let getCartId (httpContext : HttpContext) =
        use context = new ApplicationContext()
        if httpContext.Session.Keys.Contains("isAuth")
        then
            let userId = httpContext.Session.GetInt32("id") |> int
            let cartIds = context
                            .Cart
                            .Where(fun c -> c.AuthorizedUserId = userId)
                            .Select(fun c -> c.Id)
                            .ToList()
            if cartIds.Count > 0
            then
                cartIds.First()
            else
                let newCart = Cart(userId)
                context.Cart.Add(newCart) |> ignore
                context.SaveChanges() |> ignore
                newCart.Id
        else
            if httpContext.Session.Keys.Contains("cartId")
            then
                httpContext.Session.GetInt32("cartId") |> int
            else
                let newCart = Cart()
                context.Cart.Add(newCart) |> ignore
                context.SaveChanges() |> ignore
                httpContext.Session.SetInt32("cartId", newCart.Id)
                newCart.Id                
            
    [<HttpGet>]
    member this.Index() =
        use context = new ApplicationContext()
        let cartId = getCartId this.HttpContext
        let productLines = context.ProductLine.Where(fun p -> p.CartId = cartId).ToList()
        this.View(productLines)
        
    [<HttpPost>]  
    member this.AddProductLine(productId : int) =
        use context = new ApplicationContext()
        let cartId = getCartId this.HttpContext
        context.ProductLine.Add(ProductLine (cartId, productId)) |> ignore
        context.SaveChanges |> ignore
        
    [<HttpPost>]
    member this.RemoveProductLine(productId : int) =
        None
    
    [<HttpPost>]
    member this.AddProduct(productId : int) =
        None
        //проверку на существование продуктлайна не забыть, если его нету ничего не делать
        
    [<HttpPost>]
    member this.RemoveProduct(productId : int) =
        None
        //и тут проверку такую же
            
        
        
        
   
    
    
        