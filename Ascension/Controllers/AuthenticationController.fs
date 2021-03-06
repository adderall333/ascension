﻿namespace Ascension.Controllers

open Ascension
open System.Linq
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Primitives
open System.Text.RegularExpressions;
open Models

type AuthenticationController() =
    inherit Controller()
    
    let ValidateUser(user : UserToRegister) =
        let nameRegex = new Regex("^[A-Z][a-zA-Z]+$")
        let nameCheck = nameRegex.IsMatch user.Name
        
        let surnameRegex = new Regex("^[A-Z][a-zA-Z]+$")
        let surnameCheck = surnameRegex.IsMatch user.Surname
        
        let emailPattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                           @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$"
        let emailCheck = Regex.IsMatch(user.Email, emailPattern, RegexOptions.IgnoreCase)
        
        let passRegex = new Regex("^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$")
        let passCheck = passRegex.IsMatch user.Pass
        
        nameCheck && surnameCheck && emailCheck && passCheck
        
    let FindUser email =
        use context = new ApplicationContext()
        context
            .User
            .Where(fun e -> e.Email = email)
            .ToList()
            .Any()
    
    [<HttpGet>]
    member this.Signin() =
        if this.HttpContext.Session.Keys.Contains("isAuth")
        then this.Redirect("/Account") :> ActionResult
        else this.View() :> ActionResult
      
    [<HttpGet>]  
    member this.Signup() =
        if this.HttpContext.Session.Keys.Contains("isAuth")
        then this.Redirect("/Account") :> ActionResult
        else this.View() :> ActionResult
    
    [<HttpPost>]  
    member this.TryRegister(user : UserToRegister) =
        let valid = ValidateUser user
        if not valid then this.Response.Headers.Add("registration_result", StringValues("error"))
        elif FindUser user.Email then this.Response.Headers.Add("registration_result", StringValues("failed"))
        else
            use context = new ApplicationContext()
            let hashedPassword = Crypto.GetHashPassword user.Pass
            let newUser = new User()
            newUser.Name <- user.Name
            newUser.Surname <- user.Surname
            newUser.Email <- user.Email
            newUser.HashedPassword <- hashedPassword
            context.User.Add(newUser) |> ignore
            context.SaveChanges() |> ignore
            
            if this.HttpContext.Session.Keys.Contains("cartId")
            then
                let cartId = this.HttpContext.Session.GetInt32("cartId") |> int
                context.Cart.First(fun c -> c.Id = cartId).AuthorizedUserId <- newUser.Id
            context.SaveChanges() |> ignore
            
            this.Response.Headers.Add("registration_result", StringValues("ok"))
       
    [<HttpPost>]
    member this.TryLogin(user : UserToLogin) =
        use context = new ApplicationContext()
        let dbUser = context
                         .User
                         .FirstOrDefault(fun e -> e.Email = user.Email)
        
        if this.HttpContext.Session.Keys.Contains("cartId")
            then
                let cartId = this.HttpContext.Session.GetInt32("cartId") |> int
                let oldCart = context.Cart.FirstOrDefault(fun c -> c.AuthorizedUserId = dbUser.Id)
                if (oldCart <> null && cartId <> oldCart.Id) //fixed empty cart on new User
                then
                    context.Cart.Remove(oldCart) |> ignore
                context.Cart.First(fun c -> c.Id = cartId).AuthorizedUserId <- dbUser.Id
        context.SaveChanges() |> ignore
        
        if dbUser = null || not <| Crypto.VerifyHashedPassword user.Pass dbUser.HashedPassword
        then
            this.Response.Headers.Add("registration_result", StringValues("failed"))
        else
            this.Response.Headers.Add("login_result", StringValues("ok"))
            this.HttpContext.Session.SetString("isAuth", "true")
            this.HttpContext.Session.SetInt32("id", dbUser.Id) 
            this.HttpContext.Session.SetString("email", dbUser.Email)
            if user.Remember
            then
                this.HttpContext.Response.Cookies.Append("email", dbUser.Email)
                this.HttpContext.Response.Cookies.Append("hashedPass", dbUser.HashedPassword)
    
    [<HttpGet>]            
    member this.Logout() =
        this.HttpContext.Session.Remove("isAuth")
        this.HttpContext.Session.Remove("id")
        this.HttpContext.Session.Remove("email")
        this.HttpContext.Session.Remove("cartId")
        if this.HttpContext.Request.Cookies.ContainsKey("email")
        then this.HttpContext.Response.Cookies.Delete("email")
        if this.HttpContext.Request.Cookies.ContainsKey("hashedPass")
        then this.HttpContext.Response.Cookies.Delete("hashedPass")
        this.Redirect("/Authentication/Signin")