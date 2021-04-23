namespace Ascension

open Ascension
open Microsoft.AspNetCore.Mvc
open Models
open System.Linq
open Models.Attributes
open Selector
open Checker

type AdminController() =
    inherit Controller()
    
    [<HttpGet>]
    member this.Index() =
        use context = new ApplicationContext()
        let model = context
                      .GetType()
                      .GetProperties()
                      .Where(fun p -> p.GetCustomAttributes(typedefof<DisplayedInAdminPanelAttribute>, false).Any())
                      .Select(fun p -> p.Name)
        this.View(model)
        
    [<HttpGet>]
    member this.Models(name : string) =
        use context = new ApplicationContext()
        this.View(getModelsWithoutRelations name)
   
    
    //Create
    [<HttpGet>]     
    member this.Create(name : string) =
        this.View(getModelType name)
        
    [<HttpPost>]
    member this.CreateSuperCategory(formModel : SuperCategoryModel) =
        let checkResult = checkSuperCategory formModel
        let createSuperCategory (model : SuperCategoryModel) =
            use context = new ApplicationContext()
            context.SuperCategory.Add(SuperCategory(model.Name, model.Categories, context)) |> ignore
            context.SaveChanges() |> ignore
            this.Response.StatusCode = 200 |> ignore
            this.Redirect("/admin/models?name=SuperCategory") :> ActionResult
        
        match checkResult with
        | Ok(checkedModel) -> createSuperCategory checkedModel
        | Bad(message) -> this.BadRequest(message) :> ActionResult
        
    member this.CreateCategory(formModel : CategoryModel) =
        let checkResult = checkCategory formModel
        let createCategory (model : CategoryModel) =
            use context = new ApplicationContext()
            context.Category.Add(Category(model.Name, model.SuperCategory, model.Products, model.Specifications, context)) |> ignore
            context.SaveChanges() |> ignore
            this.Response.StatusCode = 200 |> ignore
            this.Redirect("/admin/models?name=Category") :> ActionResult
            
        match checkResult with
        | Ok(checkedModel) -> createCategory checkedModel
        | Bad(message) -> this.BadRequest(message) :> ActionResult
        
    //Read    
    [<HttpGet>]
    member this.Read(name : string, id : int) =
        let model = getModelWithRelations name id
        this.View(model)
    
    
    //Update
    [<HttpGet>]    
    member this.Update(name : string, id : int) =
        let model = getModelWithRelations name id
        this.View(model)
        
    [<HttpPost>]    
    member this.UpdateSuperCategory(formModel : SuperCategoryModel) =
        let checkResult = checkSuperCategory formModel
        let updateSuperCategory (model : SuperCategoryModel) = 
            use context = new ApplicationContext()
            context
                .SuperCategory
                .First(fun sc -> sc.Id = model.Id)
                .Update(model.Name, model.Categories, context)
            context.SaveChanges() |> ignore
            this.Response.StatusCode = 200 |> ignore
            this.Redirect($"/admin/read?name=SuperCategory&id={model.Id}") :> ActionResult
            
        match checkResult with
        | Ok(checkedModel) -> updateSuperCategory checkedModel
        | Bad(message) -> this.BadRequest(message) :> ActionResult
    
    
    //Delete
    [<HttpPost>]    
    member this.DeleteSuperCategory(id : int) =
        use context = new ApplicationContext()
        let modelToDelete = SuperCategory.GetModel(id) :?> SuperCategory
        context.SuperCategory.Remove(modelToDelete) |> ignore
        context.SaveChanges() |> ignore