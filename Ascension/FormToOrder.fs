namespace Ascension

type FormToOrder() =
    let mutable name = null : string
    let mutable surname = null : string
    let mutable email = null : string
    let mutable address = null : string
    
    member this.Name
        with get() = name
        and set(value) = name <- value
    member this.Surname
        with get() = surname
        and set(value) = surname <- value
    member this.Email
        with get() = email
        and set(value) = email <- value
        
    member this.Address
        with get() = address
        and set(value) = address <- value