namespace Ascension

open System
open System.Collections.Generic
open ProductFilter
open System.Diagnostics
open System.Linq
open Microsoft.AspNetCore.Mvc
open Microsoft.EntityFrameworkCore
open Models
open System
open System.Collections.Generic
open System.Net
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Identity
open ProductFilter
open System.Diagnostics
open System.Linq
open Microsoft.AspNetCore.Mvc
open Microsoft.EntityFrameworkCore

type ProfileController() =
    inherit Controller()


    member this.Personal() = this.View()

    member this.Order() =
        use context = new ApplicationContext()
        let userId = this.HttpContext.Session.GetInt32("id") |> int
        let orders = context.Order.Where(fun c -> c.UserId = userId).ToList()
        
        this.View(orders)

    member this.Cart() = this.View()
