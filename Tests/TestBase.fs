namespace Tests

open Ascension
open Microsoft.AspNetCore.Mvc.Testing

type AscensionFactory() =
    inherit WebApplicationFactory<Startup>()
    