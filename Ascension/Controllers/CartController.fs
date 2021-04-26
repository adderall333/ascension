namespace Ascension.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open System.Diagnostics

open Microsoft.AspNetCore.Mvc
open Models

type CartController () =
    inherit Controller()

    [<HttpGet>]
    member this.Cart(userId : int) =
        use context = new ApplicationContext()
        let productLines = context.ProductLine.Where(fun p -> p.ClientId = userId).ToList()
        this.View(productLines)
        
    [<HttpPost>]  
    member this.AddProductLine(productId : int, userId : int) =
        use context = new ApplicationContext()
        context.ProductLine.Add(ProductLine (userId, productId)) |> ignore
        context.SaveChanges |> ignore
        
    [<HttpPost>]
    member this.RemoveProductLine(productId : int, userId : int) =
        None
    
    [<HttpPost>]
    member this.AddProduct(productId : int, userId : int) =
        None
        //проверку на существование продуктлайна не забыть, если его нету ничего не делать
        
    [<HttpPost>]
    member this.RemoveProduct(productId : int, userId : int) =
        None
        //и тут проверку
            
        
        
        
   
    
    
        