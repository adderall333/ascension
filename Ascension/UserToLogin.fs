namespace Ascension

type UserToLogin() =
    let mutable email = null : string
    let mutable pass = null : string
    let mutable remember = false : bool
    member this.Email
        with get() = email
        and set(value) = email <- value
    member this.Pass
        with get() = pass
        and set(value) = pass <- value
    member this.Remember
        with get() = remember
        and set(value) = remember <- value