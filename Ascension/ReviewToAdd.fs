namespace Ascension

type ReviewToAdd() =
    let mutable text = null : string
    let mutable rating = 0 : int
    let mutable prodId = 0 : int
    member this.Text
        with get() = text
        and set(value) = text <- value
    member this.Rating
        with get() = rating
        and set(value) = rating <- value
    member this.ProdId
        with get() = prodId
        and set(value) = prodId <- value