namespace Ascension

open System.Collections.Generic

type SuperCategoryModel() =
    let mutable id = 0 : int
    let mutable name = null : string
    let mutable image = 0 : int
    let mutable categories = List<int>() : List<int>
    member this.Id
        with get() = id
        and set(value) = id <- value
    member this.Name
        with get() = name
        and set(value) = name <- value
    member this.Image
        with get() = image
        and set(value) = image <- value
    member this.Categories
        with get() = categories
        and set(value) = categories <- value
        
type CategoryModel() =
    let mutable id = 0 : int
    let mutable name = null : string
    let mutable superCategory = 0 : int
    let mutable products = List<int>() : List<int>
    let mutable specifications = List<int>() : List<int>
    member this.Id
        with get() = id
        and set(value) = id <- value
    member this.Name
        with get() = name
        and set(value) = name <- value
    member this.SuperCategory
        with get() = superCategory
        and set(value) = superCategory <- value
    member this.Products
        with get() = products
        and set(value) = products <- value
    member this.Specifications
        with get() = specifications
        and set(value) = specifications <- value
        
type SpecificationModel() =
    let mutable id = 0 : int
    let mutable name = null : string
    let mutable category = 0 : int
    let mutable specificationOptions = List<int>() : List<int>
    member this.Id
        with get() = id
        and set(value) = id <- value
    member this.Name
        with get() = name
        and set(value) = name <- value
    member this.Category
        with get() = category
        and set(value) = category <- value
    member this.SpecificationOptions
        with get() = specificationOptions
        and set(value) = specificationOptions <- value
        
type SpecificationOptionModel() =
    let mutable id = 0 : int
    let mutable name = null : string
    let mutable specification = 0 : int
    let mutable products = List<int>() : List<int>
    member this.Id
        with get() = id
        and set(value) = id <- value
    member this.Name
        with get() = name
        and set(value) = name <- value
    member this.Specification
        with get() = specification
        and set(value) = specification <- value
    member this.Products
        with get() = products
        and set(value) = products <- value
        
type ProductModel() =
    let mutable id = 0 : int
    let mutable name = null : string
    let mutable cost = 0 : int
    let mutable description = null : string
    let mutable category = 0 : int
    let mutable specificationOptions = List<int>() : List<int>
    let mutable images = List<int>() : List<int>
    member this.Id
        with get() = id
        and set(value) = id <- value
    member this.Name
        with get() = name
        and set(value) = name <- value
    member this.Cost
        with get() = cost
        and set(value) = cost <- value
    member this.Description
        with get() = description
        and set(value) = description <- value
    member this.Category
        with get() = category
        and set(value) = category <- value
    member this.SpecificationOptions
        with get() = specificationOptions
        and set(value) = specificationOptions <- value
    member this.Images
        with get() = images
        and set(value) = images <- value
        
type ImageModel() =
    let mutable id = 0 : int
    let mutable path = null : string
    let mutable product = 0 : int
    member this.Id
        with get() = id
        and set(value) = id <- value
    member this.Path
        with get() = path
        and set(value) = path <- value
    member this.Product
        with get() = product
        and set(value) = product <- value