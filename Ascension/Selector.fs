namespace Ascension

open System.Linq
open Models
open Microsoft.EntityFrameworkCore

module Selector =
    let getModelsWithoutRelations (name : string) =
        use context = new ApplicationContext()
        let toList (set : DbSet<_>) = set.Select(fun e -> e :> IModel).OrderBy(fun e -> e.Id).ToList()
        match name.ToLower() with
        | "supercategory" -> context.SuperCategory |> toList
        | "category" -> context.Category |> toList
        | "specification" -> context.Specification |> toList
        | "specificationoption" -> context.SpecificationOption |> toList
        | "product" -> context.Product |> toList
        | "image" -> context.Image |> toList
        | _ -> failwith "There is no such model type"
        
    let getModelsWithRelations (name : string) =
        use context = new ApplicationContext()
        let toList (set : DbSet<_>) = set.Select(fun e -> e :> IModel).OrderBy(fun e -> e.Id).ToList()
        match name.ToLower() with
        | "specificationoptions" -> (context.SpecificationOption |> toList).Select(fun e -> SpecificationOption.GetModel(e.Id))
        | "products" -> (context.Product |> toList).Select(fun e -> Product.GetModel(e.Id))
        | _ -> failwith "There is no such model type"
        
    let getModelWithRelations (name : string) id =
        match name.ToLower() with
        | "supercategory" -> SuperCategory.GetModel(id)
        | "category" -> Category.GetModel(id)
        | "specification" -> Specification.GetModel(id)
        | "specificationoption" -> SpecificationOption.GetModel(id)
        | "product" -> Product.GetModel(id)
        | "image" -> Image.GetModel(id)
        | _ -> failwith "There is no such model type"
        
    let getModelType (name : string) =
        match name.ToLower() with
        | "supercategory" -> typeof<SuperCategory>
        | "category" -> typeof<Category>
        | "specification" -> typeof<Specification>
        | "specificationoption" -> typeof<SpecificationOption>
        | "product" -> typeof<Product>
        | "image" -> typeof<Image>
        | _ -> failwith "There is no such model type"
        
    let getEmptyModel (name : string) =
        match name.ToLower() with
        | "supercategory" -> SuperCategory() :> IModel
        | "category" -> Category() :> IModel
        | "specification" -> Specification() :> IModel
        | "specificationoption" -> SpecificationOption() :> IModel
        | "product" -> Product() :> IModel
        | "image" -> Image() :> IModel
        | _ -> failwith "There is no such model type"
    
