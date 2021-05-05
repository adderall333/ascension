namespace Ascension

open System
open System.Collections.Generic
open ProductFilter
open System.Diagnostics
open System.Linq
open Microsoft.AspNetCore.Mvc
open Microsoft.EntityFrameworkCore
open Models

type ProfileController() =
    inherit Controller()


    member this.Personal () =
        this.View()
        
    member this.Order () =
        this.View()
        
    member this.Cart () =
        this.View()

