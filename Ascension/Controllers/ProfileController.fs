namespace Ascension

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

    let isAuth (context : HttpContext) =
        if context.Session.Keys.Contains("isAuth")
            then
                true
            else
                false
    member this.Personal()  =
        if isAuth this.HttpContext then      
            let id = this.HttpContext.Session.GetInt32("id") |> int
            use dbContext = new ApplicationContext()
            let dbUser = dbContext
                             .User
                             .Where(fun e -> e.Id = id)
                             .ToList()
                             .First()    
            this.View(dbUser) :> ActionResult
        else
            this.Redirect("../Authentication/Signin") :> ActionResult
        
    member this.EditPersonal(editUser :User) =
        let id = this.HttpContext.Session.GetInt32("id") |> int
        use dbContext = new ApplicationContext()
        let dbUser = dbContext
                         .User
                         .Where(fun e -> e.Id = id)
                         .ToList()
                         .First()
        if(editUser <> null)
        then
            let hashedPassword = Crypto.GetHashPassword editUser.HashedPassword
            if(editUser.Name <> null)
            then
                dbUser.Name <- editUser.Name
            if(editUser.Surname <> null)
            then
                dbUser.Surname <- editUser.Surname
            if(editUser.Email <> null)
            then
                dbUser.Email <- editUser.Email
            if(hashedPassword <> null)
            then
                dbUser.HashedPassword <- hashedPassword
            this.HttpContext.Session.SetString("email", dbUser.Email)
            dbContext.SaveChanges() |> ignore
        this.Redirect("Personal")
        //fix to redirect to personal
       
        
    member this.Order() =
        if isAuth this.HttpContext then    
            use context = new ApplicationContext()
            let userId = this.HttpContext.Session.GetInt32("id") |> int
            let orders = context.Order.Where(fun c -> c.UserId = userId).ToList()
            
            this.View(orders) :> ActionResult
        else
            this.Redirect("../Authentication/Signin") :> ActionResult
            

    member this.Cart() = this.View()
