namespace Ascension

open Ascension
open Microsoft.AspNetCore.Mvc
open Models
open System.Linq
open Models.Attributes
open Selector
open Editor

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
   
    [<HttpGet>]     
    member this.Create(name : string) =
        this.View(getModelType name)
        
    [<HttpPost>]
    member this.CreateSuperCategory(model : SuperCategoryModel) =
        createSuperCategory model
        this.Redirect("/admin")
        
    [<HttpPost>]
    member this.CreateCategory(model : CategoryModel) =
        createCategory model
        this.Redirect("/admin") 
    
    [<HttpPost>]
    member this.CreateSpecification(model : SpecificationModel) =
        createSpecification model
        this.Redirect("/admin")
        
    [<HttpPost>]
    member this.CreateSpecificationOption(model : SpecificationOptionModel) =
        createSpecificationOption model
        this.Redirect("/admin")
        
    [<HttpPost>]
    member this.CreateProduct(model : ProductModel) =
        createProduct model
        this.Redirect("/admin")
        
    [<HttpPost>]
    member this.CreateImage(model : ImageModel) =
        createImage model
        this.Redirect("/admin")
        
    [<HttpGet>]
    member this.Read(name : string, id : int) =
        let model = getModelWithRelations name id
        this.View(model)
    
    [<HttpGet>]    
    member this.Update(name : string, id : int) =
        let model = getModelWithRelations name id
        this.View(model)
    
    [<HttpPost>]    
    member this.Delete(name : string, id : int) =
        //todo
        this.Redirect("/admin")