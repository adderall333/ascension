namespace Ascension

open System
open BCrypt.Net

module Crypto =
        
    let GetHashPassword (password : string) =
        if password = null
        then
            null
        else
            BCrypt.HashPassword(password)
        
    let VerifyHashedPassword inputPas hashedPas =
        if inputPas = null || hashedPas = null
        then
            false
        else
            BCrypt.Verify(inputPas, hashedPas)