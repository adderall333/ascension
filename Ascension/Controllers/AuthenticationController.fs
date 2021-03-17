namespace Ascension

open System
open System.Diagnostics
open System.Linq
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.EntityFrameworkCore
open Microsoft.Extensions.Primitives
open Models

type AuthenticationController() =
    inherit Controller()
    
    member this.Signin() =
        this.View()
      
    [<HttpGet>]  
    member this.Signup() =
        this.View()
    
    [<HttpPost>]  
    member this.TryRegister(user : UserToRegister) =
        if user.Name <> "Kirill"
        then
            this.Response.Headers.Add("result", StringValues("ok"))
        else
            this.Response.Headers.Add("result", StringValues("error"))