namespace Ascension

open System.Collections.Generic
open Models

type SuperCategoryModel() = 
    let mutable id = 0 : int
    let mutable name = null : string
    let mutable imagePath = null : string
    let mutable categories = List<int>() : List<int>
    member this.Id
            with get() = id
            and set value = id <- value
    member this.Name
        with get() = name
        and set value = name <- value
    member this.ImagePath
        with get() = imagePath
        and set value = imagePath <- value
    member this.Categories
        with get() = categories
        and set value = categories <- value
    interface IModel with
        member this.Id
            with get() = id
            and set value = id <- value
        
type CategoryModel() =
    let mutable id = 0 : int
    let mutable name = null : string
    let mutable imagePath = null : string
    let mutable superCategory = 0 : int
    let mutable products = List<int>() : List<int>
    let mutable specifications = List<int>() : List<int>
    member this.Id
        with get() = id
        and set value = id <- value
    member this.Name
        with get() = name
        and set value = name <- value
    member this.ImagePath
        with get() = imagePath
        and set value = imagePath <- value
    member this.SuperCategory
        with get() = superCategory
        and set value = superCategory <- value
    member this.Products
        with get() = products
        and set value = products <- value
    member this.Specifications
        with get() = specifications
        and set value = specifications <- value
    interface IModel with
        member this.Id
            with get() = id
            and set value = id <- value
        
type SpecificationModel() =
    let mutable id = 0 : int
    let mutable name = null : string
    let mutable category = 0 : int
    let mutable specificationOptions = List<int>() : List<int>
    member this.Id
        with get() = id
        and set value = id <- value
    member this.Name
        with get() = name
        and set value = name <- value
    member this.Category
        with get() = category
        and set value = category <- value
    member this.SpecificationOptions
        with get() = specificationOptions
        and set value = specificationOptions <- value
    interface IModel with
        member this.Id
            with get() = id
            and set value = id <- value
        
type SpecificationOptionModel() =
    let mutable id = 0 : int
    let mutable name = null : string
    let mutable specification = 0 : int
    let mutable products = List<int>() : List<int>
    member this.Id
        with get() = id
        and set value = id <- value
    member this.Name
        with get() = name
        and set value = name <- value
    member this.Specification
        with get() = specification
        and set value = specification <- value
    member this.Products
        with get() = products
        and set value = products <- value
    interface IModel with
        member this.Id
            with get() = id
            and set value = id <- value
        
type ProductModel() =
    let mutable id = 0 : int
    let mutable name = null : string
    let mutable cost = 0 : int
    let mutable description = null : string
    let mutable isAvailable = null : string
    let mutable category = 0 : int
    let mutable specificationOptions = List<int>() : List<int>
    let mutable images = List<int>() : List<int>
    member this.Id
        with get() = id
        and set value = id <- value
    member this.Name
        with get() = name
        and set value = name <- value
    member this.Cost
        with get() = cost
        and set value = cost <- value
    member this.Description
        with get() = description
        and set value = description <- value
    member this.IsAvailable
        with get() = isAvailable
        and set value = isAvailable <- value
    member this.Category
        with get() = category
        and set value = category <- value
    member this.SpecificationOptions
        with get() = specificationOptions
        and set value = specificationOptions <- value
    member this.Images
        with get() = images
        and set value = images <- value
    interface IModel with
        member this.Id
            with get() = id
            and set value = id <- value
        
type ImageModel() =
    let mutable id = 0 : int
    let mutable path = null : string
    let mutable product = 0 : int
    member this.Id
        with get() = id
        and set value = id <- value
    member this.Path
        with get() = path
        and set value = path <- value
    member this.Product
        with get() = product
        and set value = product <- value
    interface IModel with
        member this.Id
            with get() = id
            and set value = id <- value
        
type UserModel() =
    let mutable id = 0 : int
    let mutable name = null : string
    let mutable surname = null : string
    let mutable email = null : string
    let mutable isAdmin = null : string
    member this.Id
        with get() = id
        and set value = id <- value
    member this.Name
        with get() = name
        and set value = name <- value
    member this.Surname
        with get() = surname
        and set value = surname <- value
    member this.Email
        with get() = email
        and set value = email <- value
    member this.IsAdmin
        with get() = isAdmin
        and set value = isAdmin <- value
    interface IModel with
        member this.Id
            with get() = id
            and set value = id <- value