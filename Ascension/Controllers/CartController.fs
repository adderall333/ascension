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
    member this.Cart () =
        this.View()
        
    [<HttpPost>]  
    member this.AddProduct(id:int, user:int) =
        use context = new ApplicationContext()
        context.ProductLine.Add(ProductLine (user, id)) |> ignore
        context.SaveChanges |> ignore
        
        
        
        
   
    
    
        