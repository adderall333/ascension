namespace Ascension

open System
open System.ComponentModel
open Dadata
open Dadata.Model
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc
open Newtonsoft.Json

type GeoResponse =
    {
        city: string
    }

type GeoApiController () =
    inherit Controller()
            
    member this.Get(lat: float, lot: float)  =
        let api = SuggestClientAsync("ef2db2da426469acd403d525ff8241bcb5487ef6")
        let response = api.Geolocate(lat, lot) |> Async.AwaitTask |> Async.RunSynchronously
        let dataCity = response.suggestions.[0].data.city
        { city = dataCity }
        