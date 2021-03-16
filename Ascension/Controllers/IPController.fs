namespace Ascension

open System.ComponentModel
open Dadata
open Dadata.Model
open Microsoft.AspNetCore.Mvc
open Newtonsoft.Json

type IPController() =
    inherit Controller()

    
    let api = new SuggestClientAsync("ef2db2da426469acd403d525ff8241bcb5487ef6")
    let response = api.Geolocate(11231.1, 11111.1) |> Async.AwaitTask |> Async.RunSynchronously
    let address = response.suggestions.[0].data.city 
    