namespace Ascension.Controller

open Dadata
open Dadata.Model
open Microsoft.AspNetCore.Mvc

type GeoResponse =
    {
        city: string    
    }

type GeoApiController () =
    inherit Controller()
            
    member this.Get(lat: float, lot: float)  = 
        let api = SuggestClientAsync("ef2db2da426469acd403d525ff8241bcb5487ef6")
        let request = GeolocateRequest(lat,lot)
        request.language <- "en"
        let response = api.Geolocate(request)  |> Async.AwaitTask |> Async.RunSynchronously
        let dataCity = response.suggestions.[0].data.city 
        { city = dataCity }
        
        