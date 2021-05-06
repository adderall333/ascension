namespace Ascension.Controller

open System.IO
open Ascension
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Models
open System.Linq
open Models.Attributes
open Selector
open Checks

type AdminController() =
    inherit Controller()
    
    let isAdmin (context : HttpContext) =
        if context.Session.Keys.Contains("id")
            then
                use dbContext = new ApplicationContext()
                let id = context.Session.GetInt32("id") |> int
                let user = dbContext.User.First(fun u -> u.Id = id)
                user.IsAdmin
            else
                false
    
    [<HttpGet>]
    member this.Index() =
        if isAdmin this.HttpContext
        then
            use context = new ApplicationContext()
            let model = context
                          .GetType()
                          .GetProperties()
                          .Where(fun p -> p.GetCustomAttributes(typedefof<DisplayedInAdminPanelAttribute>, false).Any())
                          .Select(fun p -> p.Name)
            this.View(model) :> ActionResult
        else
            this.StatusCode(403) :> ActionResult
        
    [<HttpGet>]
    member this.Models(name : string) =
        if isAdmin this.HttpContext
        then
            use context = new ApplicationContext()
            this.View(getModelsWithoutRelations name) :> ActionResult
        else
            this.StatusCode(403) :> ActionResult
    
    
    //Create
    [<HttpGet>]     
    member this.Create(name : string) =
        if isAdmin this.HttpContext
        then
             this.View(getModelType name) :> ActionResult
        else
            this.StatusCode(403) :> ActionResult
        
    [<HttpPost>]
    member this.CreateSuperCategory(formModel : SuperCategoryModel) =
        if isAdmin this.HttpContext
        then
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
        else
            this.StatusCode(403) :> ActionResult
        
    [<HttpPost>]
    member this.CreateCategory(formModel : CategoryModel) =
        if isAdmin this.HttpContext
        then
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
        else
            this.StatusCode(403) :> ActionResult
            
    [<HttpPost>]
    member this.CreateSpecification(formModel : SpecificationModel) =
        if isAdmin this.HttpContext
        then
            let checkResult = checkSpecification formModel
            let createSpecification (model : SpecificationModel) =
                use context = new ApplicationContext()
                context.Specification.Add(Specification(model.Name, model.Category, model.SpecificationOptions, context)) |> ignore
                context.SaveChanges() |> ignore
                this.Response.StatusCode = 200 |> ignore
                this.Redirect("/admin/models?name=Specification") :> ActionResult
            
            match checkResult with
            | Ok(checkedModel) -> createSpecification checkedModel
            | Bad(message) -> this.BadRequest(message) :> ActionResult
        else
            this.StatusCode(403) :> ActionResult
    
    [<HttpPost>]
    member this.CreateSpecificationOption(formModel : SpecificationOptionModel) =
        if isAdmin this.HttpContext
        then
            let checkResult = checkSpecificationOption formModel
            let createSpecificationOption (model : SpecificationOptionModel) =
                use context = new ApplicationContext()
                context.SpecificationOption.Add(SpecificationOption(model.Name, model.Specification, model.Products, context)) |> ignore
                context.SaveChanges() |> ignore
                this.Response.StatusCode = 200 |> ignore
                this.Redirect("/admin/models?name=SpecificationOption") :> ActionResult
                
            match checkResult with
            | Ok(checkedModel) -> createSpecificationOption checkedModel
            | Bad(message) -> this.BadRequest(message) :> ActionResult
        else
            this.StatusCode(403) :> ActionResult
    
    [<HttpPost>]
    member this.CreateProduct(formModel : ProductModel) =
        if isAdmin this.HttpContext
        then
            let checkResult = checkProduct formModel
            let createProduct (model : ProductModel) =
                use context = new ApplicationContext()
                context.Product.Add(Product(model.Name, model.Cost, model.Description, model.Category, model.SpecificationOptions, model.Images)) |> ignore
                context.SaveChanges() |> ignore
                this.Response.StatusCode = 200 |> ignore
                this.Redirect("/admin/models?name=Product") :> ActionResult
                
            match checkResult with
            | Ok(checkedModel) -> createProduct checkedModel
            | Bad(message) -> this.BadRequest(message) :> ActionResult
        else
            this.StatusCode(403) :> ActionResult
    
    [<HttpPost>]
    member this.CreateImage(formModel : ImageModel, file : IFormFile) =
        if isAdmin this.HttpContext
        then
            use context = new ApplicationContext()
            
            let path = "wwwroot/img/" + file.FileName
            use fileStream = new FileStream(path, FileMode.Create)
            file.CopyTo(fileStream)
                        
            context.Image.Add(Image(file.FileName, formModel.Product)) |> ignore
            context.SaveChanges() |> ignore
            this.Response.StatusCode = 200 |> ignore
            this.Redirect("/admin/models?name=Image") :> ActionResult
        else
            this.StatusCode(403) :> ActionResult
    
        
    //Read    
    [<HttpGet>]
    member this.Read(name : string, id : int) =
        if isAdmin this.HttpContext
        then
            let model = getModelWithRelations name id
            this.View(model) :> ActionResult
        else
            this.StatusCode(403) :> ActionResult
    
    
    //Update
    [<HttpGet>]    
    member this.Update(name : string, id : int) =
        if isAdmin this.HttpContext
        then
            let model = getModelWithRelations name id
            this.View(model) :> ActionResult
        else
            this.StatusCode(403) :> ActionResult
        
    [<HttpPost>]    
    member this.UpdateSuperCategory(formModel : SuperCategoryModel) =
        if isAdmin this.HttpContext
        then
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
        else
            this.StatusCode(403) :> ActionResult
            
    //todo Category
    
    //todo Specification
    
    //todo Specification option
    
    //todo Product
    
    //todo Image
    
    //todo User
    
    
    //Delete
    [<HttpPost>]    
    member this.DeleteSuperCategory(id : int) =
        if isAdmin this.HttpContext
        then
            use context = new ApplicationContext()
            let modelToDelete = SuperCategory.GetModel(id) :?> SuperCategory
            context.SuperCategory.Remove(modelToDelete) |> ignore
            context.SaveChanges() |> ignore
            
    //todo Category
        
    //todo Specification
    
    //todo Specification option
    
    //todo Product
    
    //todo Image
    
    //todo User