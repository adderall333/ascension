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
        use context = new ApplicationContext()
        let alreadyRegistered = context
                                    .User
                                    .Where(fun e -> e.Email = user.Email)
                                    .ToList()
                                    .Any()
        if alreadyRegistered
        then
            this.Response.Headers.Add("registration_result", StringValues("failed"))
        else
            let hashedPassword = Crypto.GetHashPassword user.Pass
            let newUser = new User()
            newUser.Name <- user.Name
            newUser.Surname <- user.Surname
            newUser.Email <- user.Email
            newUser.HashedPassword <- hashedPassword
            context.User.Add(newUser) |> ignore
            context.SaveChanges() |> ignore
            this.Response.Headers.Add("registration_result", StringValues("ok"))