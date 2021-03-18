namespace Ascension

open System
open System.ComponentModel
open Dadata
open Dadata.Model
open Microsoft.AspNetCore.Mvc
open Newtonsoft.Json

type IPController() =
    inherit Controller()

    member this.Index(lat: float, lot: float) =
        let typ = 1
        if lat.GetType() <> typ.GetType() && lot.GetType() <> typ.GetType()
          then
              null
        else
              let api = new SuggestClientAsync("ef2db2da426469acd403d525ff8241bcb5487ef6")
              let response = api.Geolocate(lat, lot) |> Async.AwaitTask |> Async.RunSynchronously
              let dataCity = response.suggestions.[0].data.city
              this.View(dataCity)
                