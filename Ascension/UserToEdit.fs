namespace Ascension

type UserToEdit() =
    let mutable name = null : string
    let mutable surname = null : string
    let mutable email = null : string
    let mutable pass = null : string
    let mutable old_pass = null : string
    member this.Name
        with get() = name
        and set(value) = name <- value
    member this.Surname
        with get() = surname
        and set(value) = surname <- value
    member this.Email
        with get() = email
        and set(value) = email <- value
    member this.Pass
        with get() = pass
        and set(value) = pass <- value
    member this.Old_Pass
        with get() = old_pass
        and set(value) = old_pass <- value