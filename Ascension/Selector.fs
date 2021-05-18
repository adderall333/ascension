namespace Ascension

open System.Linq
open Models
open Microsoft.EntityFrameworkCore

module Selector =
    let getModelsWithoutRelations (name : string) =
        use context = new ApplicationContext()
        let toList (set : DbSet<_>) = set.Select(fun e -> e :> IModel).ToList()
        match name.ToLower() with
        | "supercategory" -> context.SuperCategory |> toList
        | "category" -> context.Category |> toList
        | "specification" -> context.Specification |> toList
        | "specificationoption" -> context.SpecificationOption |> toList
        | "product" -> context.Product |> toList
        | "image" -> context.Image |> toList
        | "user" -> context.User |> toList
        | _ -> failwith "There is no such model type"
        
    let getModelWithRelations (name : string) id =
        match name.ToLower() with
        | "supercategory" -> SuperCategory.GetModel(id)
        | "category" -> Category.GetModel(id)
        | "specification" -> Specification.GetModel(id)
        | "specificationoption" -> SpecificationOption.GetModel(id)
        | "product" -> Product.GetModel(id)
        | "image" -> Image.GetModel(id)
        | "user" -> User.GetModel(id)
        | _ -> failwith "There is no such model type"
        
    let getModelType (name : string) =
        match name.ToLower() with
        | "supercategory" -> typeof<SuperCategory>
        | "category" -> typeof<Category>
        | "specification" -> typeof<Specification>
        | "specificationoption" -> typeof<SpecificationOption>
        | "product" -> typeof<Product>
        | "image" -> typeof<Image>
        | "user" -> typeof<User>
        | _ -> failwith "There is no such model type"
        
    
