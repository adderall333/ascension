namespace Ascension

open System
open System.Diagnostics
open System.Linq
open Microsoft.AspNetCore.Mvc
open Microsoft.EntityFrameworkCore
open Models

type AuthenticationController() =
    inherit Controller()
    
    member this.Signin() =
        this.View()
        
    member this.Signup() =
        this.View()