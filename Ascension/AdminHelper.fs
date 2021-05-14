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
    let processImageProperty value = $"<img src=\"{value}\" style=\"width: 300px; height: 300px;\">"
    
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
            elif property.GetCustomAttributes(typedefof<ImagePropertyAttribute>, false).Any()
            then
                sb.Append(processImageProperty (property.GetValue(model))) |> ignore
        sb.ToString()
        
module CreateHelper =
    let processSimpleProperty propertyType propertyName =
        let sb = StringBuilder()
        sb.Append("<p><label>" + propertyName + "<label>") |> ignore
        sb.Append("<input type=\"" + Tools.getInputType propertyType + "\" name=\"" + propertyName + "\"></p>") |> ignore
        sb.ToString()
        
    let processImageProperty = "<p><label>Image<lebel><input type=\"file\" name=\"file\" accept=\"image/jpeg,image/png\"></p>"
        
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
            elif property.GetCustomAttributes(typedefof<ImagePropertyAttribute>, false).Any()
            then
                sb.Append(processImageProperty) |> ignore
            elif (*property.GetCustomAttributes(typedefof<OneToManyAttribute>, false).Any() ||*)
                 property.GetCustomAttributes(typedefof<ManyToManyAttribute>, false).Any()
            then
                let options = getModelsWithoutRelations (property.PropertyType.GetGenericArguments().First()).Name
                sb.Append(processComplexProperty options property.Name true) |> ignore
            elif (*property.GetCustomAttributes(typedefof<OneToOneAttribute>, false).Any() ||*)
                 property.GetCustomAttributes(typedefof<ManyToOneAttribute>, false).Any()
            then
                let options = getModelsWithoutRelations property.Name
                sb.Append(processComplexProperty options property.Name false) |> ignore
        sb.ToString()
        
module UpdateHelper =
    let processImageProperty = "<p><label>Image<lebel><input type=\"file\" name=\"file\" accept=\"image/jpeg,image/png\"></p>"
    
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
        sb.Append("<input type=\"" + Tools.getInputType propertyType + "\" name=\"" + propertyName + "\"") |> ignore
        sb.Append(getHtmlAttribute propertyType propertyValue propertyName + "></p>") |> ignore
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
            elif property.GetCustomAttributes(typedefof<ImagePropertyAttribute>, false).Any()
            then
                sb.Append(processImageProperty) |> ignore
            elif (*property.GetCustomAttributes(typedefof<OneToManyAttribute>, false).Any() ||*)
                 property.GetCustomAttributes(typedefof<ManyToManyAttribute>, false).Any()
            then
                let options = getModelsWithoutRelations (property.PropertyType.GetGenericArguments().First()).Name
                let checkedIds = (property.GetValue(model) :?> IEnumerable<IModel>).Select(fun e -> e.Id)
                sb.Append(processComplexProperty options property.Name checkedIds true) |> ignore
            elif (*property.GetCustomAttributes(typedefof<OneToOneAttribute>, false).Any() ||*)
                 property.GetCustomAttributes(typedefof<ManyToOneAttribute>, false).Any()
            then
                let options = getModelsWithoutRelations property.Name
                let checkedIds = [(property.GetValue(model) :?> IModel).Id]
                sb.Append(processComplexProperty options property.Name checkedIds false) |> ignore
        sb.ToString()
        
module Checks =
    type Result<'TModel, 'TMessage> = 
        | Ok of 'TModel
        | Bad of 'TMessage
    
    let check condition failMessage previousResult =
        match previousResult with
        | Ok(model) -> if condition model then Ok(model) else Bad(failMessage)
        | Bad(message) -> Bad(message)
    
    let checkSuperCategory (model : SuperCategoryModel) =
        use context = new ApplicationContext()
        let emptyNameCheck (superCategory : SuperCategoryModel) = not (String.IsNullOrEmpty(superCategory.Name))
        let nonUniqueNameCheck (superCategory : SuperCategoryModel) = not (context
                                                                            .SuperCategory
                                                                            .Where(fun sc -> sc.Name = superCategory.Name && sc.Id <> superCategory.Id)
                                                                            .Any())
        let noImageCheck (superCategory : SuperCategoryModel) = not (String.IsNullOrEmpty(superCategory.ImagePath))
        let notImageCheck (superCategory : SuperCategoryModel) = ["jpg"; "png"].Contains(superCategory.ImagePath.Split(".").Last())
        (Ok(model)) |> check emptyNameCheck "Super category name was empty"
                    |> check nonUniqueNameCheck "Super category with same name already exists"
                    |> check noImageCheck "Image was not specified"
                    |> check notImageCheck "File type is not supported"
                    
    let checkCategory (model : CategoryModel) =
        use context = new ApplicationContext()
        let emptyNameCheck (category : CategoryModel) = not (String.IsNullOrEmpty(category.Name))
        let nonUniqueNameCheck (category : CategoryModel) = not (context
                                                                    .Category
                                                                    .Where(fun c -> c.Name = category.Name && c.Id <> category.Id)
                                                                    .Any())
        let noSuperCategoryCheck (category : CategoryModel) = category.SuperCategory > 0
        let notImageCheck (category : CategoryModel) = ["jpg"; "png"].Contains(category.ImagePath.Split(".").Last())
        (Ok(model)) |> check emptyNameCheck "Category name was empty"
                    |> check nonUniqueNameCheck "Category with same name already exists"
                    |> check noSuperCategoryCheck "Super category was not specified"
                    |> check notImageCheck "File type is not supported"
                    
    let checkSpecification (model : SpecificationModel) =
        use context = new ApplicationContext()
        let emptyNameCheck (specification : SpecificationModel) = not (String.IsNullOrEmpty(specification.Name))
        let noCategoryCheck (specification : SpecificationModel) = specification.Category > 0
        (Ok(model)) |> check emptyNameCheck "Specification name was empty"
                    |> check noCategoryCheck "Category was not specified"
    
    let checkSpecificationOption (model : SpecificationOptionModel) =
        use context = new ApplicationContext()
        let emptyNameCheck (specificationOption : SpecificationOptionModel) = not (String.IsNullOrEmpty(specificationOption.Name))
        let noSpecificationCheck (specificationOption : SpecificationOptionModel) = specificationOption.Specification > 0
        (Ok(model)) |> check emptyNameCheck "Specification option name was empty"
                    |> check noSpecificationCheck "Specification was not specified"
        
    let checkProduct (model : ProductModel) =
        use context = new ApplicationContext()
        let emptyNameCheck (product : ProductModel) = not (String.IsNullOrEmpty(product.Name))
        let nonUniqueNameCheck (product : ProductModel) = not (context
                                                                    .Product
                                                                    .Where(fun p -> p.Name = product.Name && p.Id <> product.Id)
                                                                    .Any())
        let emptyDescriptionCheck (product : ProductModel) = not (String.IsNullOrEmpty(product.Description))
        let substantialCostCheck (product : ProductModel) = product.Cost > 0 && product.Cost < 1000000
        let noCategoryCheck (product : ProductModel) = product.Category > 0
        (Ok(model)) |> check emptyNameCheck "Product name was empty"
                    |> check nonUniqueNameCheck "Product with same name already exists"
                    |> check emptyDescriptionCheck "Product description was empty"
                    |> check substantialCostCheck "Product cost was less than 0 or bigger than 1000000"
                    |> check noCategoryCheck "Category was not specified"
                    
    let checkImage (model : ImageModel) =
        use context = new ApplicationContext()
        let emptyPathCheck (image : ImageModel) = not (String.IsNullOrEmpty(image.Path))
        let notImageCheck (image : ImageModel) = ["jpg"; "png"].Contains(image.Path.Split(".").Last())
        let noProductCheck (image : ImageModel) = image.Product > 0
        (Ok(model)) |> check emptyPathCheck "Image path was empty"
                    |> check notImageCheck "File type is not supported"
                    |> check noProductCheck "Product was not specified"
    
    let checkUser (model : UserModel) =
        use context = new ApplicationContext()
        let emptyNameCheck (user : UserModel) = not (String.IsNullOrEmpty(user.Name))
        let emptySurnameCheck (user : UserModel) = not (String.IsNullOrEmpty(user.Surname))
        let emptyEmailCheck (user : UserModel) = not (String.IsNullOrEmpty(user.Email))
        (Ok(model)) |> check emptyNameCheck "User name was empty"
                    |> check emptySurnameCheck "User surname was empty"
                    |> check emptyEmailCheck "User email was Empty"
        
        