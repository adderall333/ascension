namespace Ascension

open System
open System.Collections.Generic
open System.Linq
open System.Text
open Models
open Models.Attributes
open Selector

module Tools =
    let getInputType propertyType =
        match propertyType with
        | t when t = typeof<int> -> "number"
        | t when t = typeof<string> -> "text"
        | t when t = typeof<bool> -> "checkbox"
        | _ -> failwith "There is no such input type"

module ReadHelper = 
    let processSingleProperty propertyName value =
        "<p>" + propertyName + "  :  " + value + "</p>"
        
    let processEnumerableProperty propertyName values =
        let sb = StringBuilder()
        sb.Append("<p>" + propertyName) |> ignore
        for value in values do
            sb.Append("<br>" + value.ToString()) |> ignore
        sb.Append("</p>") |> ignore
        sb.ToString()
        
    let getReadHtml model =
        let sb = StringBuilder()
        for property in model.GetType().GetProperties() do
            if property.GetCustomAttributes(typedefof<OneToManyAttribute>, false).Any() ||
               property.GetCustomAttributes(typedefof<ManyToManyAttribute>, false).Any()
            then
                sb.Append(processEnumerableProperty property.Name (property.GetValue(model) :?> IEnumerable<_>)) |> ignore
            elif property.GetCustomAttributes(typedefof<OneToOneAttribute>, false).Any() ||
                 property.GetCustomAttributes(typedefof<ManyToOneAttribute>, false).Any() ||
                 property.GetCustomAttributes(typedefof<SimplePropertyAttribute>, false).Any()
            then
                sb.Append(processSingleProperty property.Name (property.GetValue(model).ToString())) |> ignore
        sb.ToString()
        
module CreateHelper =
    let processSimpleProperty propertyType propertyName =
        let sb = StringBuilder()
        sb.Append("<p><label>" + propertyName + "<label>") |> ignore
        sb.Append("<input type=\"" + Tools.getInputType propertyType + "\" name=\"" + propertyName + "\"></p>") |> ignore
        sb.ToString()
        
    let processComplexProperty (options : IEnumerable<IModel>) propertyName isEnumerable =
        let sb = StringBuilder()
        sb.Append("<p>" + propertyName) |> ignore
        for option in options do
            sb.Append("<br><input type=\"" + (if isEnumerable then "checkbox" else "radio") + "\"name=\"" +
                      propertyName + "\"value=\"" + option.Id.ToString() + "\">" + option.ToString()) |> ignore
        sb.ToString()
        
    let getCreateHtml (modelType : Type) =
        let sb = StringBuilder()
        for property in modelType.GetProperties() do
            if property.GetCustomAttributes(typedefof<SimplePropertyAttribute>, false).Any()
            then
                sb.Append(processSimpleProperty property.PropertyType property.Name) |> ignore
            elif property.GetCustomAttributes(typedefof<OneToManyAttribute>, false).Any() ||
                 property.GetCustomAttributes(typedefof<ManyToManyAttribute>, false).Any()
            then
                let options = getModelsWithoutRelations (property.PropertyType.GetGenericArguments().First()).Name
                sb.Append(processComplexProperty options property.Name true) |> ignore
            elif property.GetCustomAttributes(typedefof<OneToOneAttribute>, false).Any() ||
                 property.GetCustomAttributes(typedefof<ManyToOneAttribute>, false).Any()
            then
                let options = getModelsWithoutRelations property.Name
                sb.Append(processComplexProperty options property.Name false) |> ignore
        sb.ToString()
        
module UpdateHelper =
    let getHtmlAttribute propertyType (propertyValue : Object) propertyName =
        if propertyType <> typedefof<bool>
        then
            "value=\"" + propertyValue.ToString() + "\""
        elif (propertyValue :?> bool)
        then
            "value=\"" + propertyName + "\" checked"
        else
            "value=\"" + propertyName + "\""
    
    let processSimpleProperty propertyType propertyValue propertyName =
        let sb = StringBuilder()
        sb.Append("<p><label>" + propertyName + "<label>") |> ignore
        sb.Append("<input type=\"" + Tools.getInputType propertyType + "\"") |> ignore
        sb.Append(getHtmlAttribute propertyType propertyValue propertyName + "\">" + "</p>") |> ignore
        sb.ToString()
        
    let processComplexProperty (options : IEnumerable<IModel>) propertyName (checkedIds : IEnumerable<int>) isEnumerable  =
        let sb = StringBuilder()
        sb.Append("<p>" + propertyName) |> ignore
        for option in options do
            sb.Append("<br><input type=\"" + (if isEnumerable then "checkbox" else "radio") + "\"name=\"" +
                      propertyName + "\"value=\"" + option.Id.ToString() + "\" " +
                      (if checkedIds.Contains(option.Id) then "checked" else "") + ">" + option.ToString()) |> ignore
        sb.ToString()
        
    let getUpdateHtml model =
        let sb = StringBuilder()
        for property in model.GetType().GetProperties() do
            if property.GetCustomAttributes(typedefof<SimplePropertyAttribute>, false).Any()
            then
                sb.Append(processSimpleProperty property.PropertyType (property.GetValue(model)) property.Name) |> ignore
            elif property.GetCustomAttributes(typedefof<OneToManyAttribute>, false).Any() ||
                 property.GetCustomAttributes(typedefof<ManyToManyAttribute>, false).Any()
            then
                let options = getModelsWithoutRelations (property.PropertyType.GetGenericArguments().First()).Name
                let checkedIds = (property.GetValue(model) :?> IEnumerable<IModel>).Select(fun e -> e.Id)
                sb.Append(processComplexProperty options property.Name checkedIds true) |> ignore
            elif property.GetCustomAttributes(typedefof<OneToOneAttribute>, false).Any() ||
                 property.GetCustomAttributes(typedefof<ManyToOneAttribute>, false).Any()
            then
                let options = getModelsWithoutRelations property.Name
                let checkedIds = [(property.GetValue(model) :?> IModel).Id]
                sb.Append(processComplexProperty options property.Name checkedIds true) |> ignore
        sb.ToString()
        
module Editor =
    let createSuperCategory (model : SuperCategoryModel) =
        use context = new ApplicationContext()
        context.SuperCategory.Add(SuperCategory(model.Name, model.Categories)) |> ignore
        context.SaveChanges() |> ignore
        
    let createCategory (model : CategoryModel) =
        use context = new ApplicationContext()
        context.Category.Add(Category(model.Name, model.SuperCategory, model.Products, model.Specifications)) |> ignore
        context.SaveChanges() |> ignore
        
    let createSpecification (model : SpecificationModel) =
        use context = new ApplicationContext()
        context.Specification.Add(Specification(model.Name, model.Category, model.SpecificationOptions)) |> ignore
        context.SaveChanges() |> ignore
        
    let createSpecificationOption (model : SpecificationOptionModel) =
        use context = new ApplicationContext()
        context.SpecificationOption.Add(SpecificationOption(model.Name, model.Specification, model.Products)) |> ignore
        context.SaveChanges() |> ignore
        
    let createProduct (model : ProductModel) =
        use context = new ApplicationContext()
        let product = Product(model.Name, model.Cost, model.Description, model.Category, model.SpecificationOptions, model.Images)
        context.Product.Add(product) |> ignore
        context.SaveChanges() |> ignore
        
    let createImage (model : ImageModel) =
        use context = new ApplicationContext()
        context.Image.Add(Image(model.Path, model.Products)) |> ignore
        context.SaveChanges() |> ignore