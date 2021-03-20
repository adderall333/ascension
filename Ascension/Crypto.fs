namespace Ascension

open System
open System.Text
open System.Security.Cryptography

module Crypto =
        
    let GetHashPassword (password : string) =
        if password = null
        then
            null
        else
            let shaM = new SHA512Managed()
            password
            |> Encoding.ASCII.GetBytes
            |> shaM.ComputeHash
            |> Convert.ToBase64String
        
    let VerifyHashedPassword inputPas hashedPas =
        if inputPas = null || hashedPas = null
        then
            false
        else
            GetHashPassword inputPas = hashedPas