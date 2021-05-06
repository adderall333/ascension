namespace Ascension

module CartService =

    open Microsoft.AspNetCore.Http
    open Models
    open System.Linq

    let getCartId (httpContext: HttpContext) =
            use context = new ApplicationContext()

            if httpContext.Session.Keys.Contains("isAuth") then
                let userId =
                    httpContext.Session.GetInt32("id") |> int

                let cartIds =
                    context
                        .Cart
                        .Where(fun c -> c.AuthorizedUserId = userId)
                        .Select(fun c -> c.Id)
                        .ToList()

                if cartIds.Count > 0 then   
                    cartIds.First()
                else
                    let newCart = Cart(userId)
                    context.Cart.Add(newCart) |> ignore
                    context.SaveChanges() |> ignore
                    newCart.Id
            else if httpContext.Session.Keys.Contains("cartId") then
                httpContext.Session.GetInt32("cartId") |> int
            else
                let newCart = Cart()
                context.Cart.Add(newCart) |> ignore
                context.SaveChanges() |> ignore
                httpContext.Session.SetInt32("cartId", newCart.Id)
                newCart.Id
                
    let isInCart (product : Product) (httpContext : HttpContext) (context : ApplicationContext) =
        let cartId = getCartId httpContext                  
        context
            .ProductLine
            .Where(fun p -> p.ProductId = product.Id && p.CartId = cartId)
            .Any()

