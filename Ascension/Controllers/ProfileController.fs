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

type ProfileController() =
    inherit Controller()
    member this.Personal()  =      
        let id = this.HttpContext.Session.GetInt32("id") |> int
        use dbContext = new ApplicationContext()
        let dbUser = dbContext
                         .User
                         .Where(fun e -> e.Id = id)
                         .ToList()
                         .First()    
        this.View(dbUser)
        
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
            if(hashedPassword <> null)
            then
                dbUser.Name <- editUser.Name
            if(hashedPassword <> null)
            then
                dbUser.Surname <- editUser.Surname
            if(hashedPassword <> null)
            then
                dbUser.Email <- editUser.Email
            if(hashedPassword <> null)
            then
                dbUser.HashedPassword <- hashedPassword
            this.HttpContext.Session.SetString("email", dbUser.Email)
            dbContext.SaveChanges() |> ignore
        this.View(dbUser)
        //fix to redirect to personal
    member this.Order() = this.View()

    member this.Cart() = this.View()
