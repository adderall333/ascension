namespace Tests

open System.Collections.Generic
open System.Net
open System.Net.Http
open System.Net.Http.Json;
open Ascension
open Models
open NUnit.Framework
open System.Linq
    
[<TestFixture>]
type IntegrationTests() =
    let url = "http://localhost:5000"

    let client = new HttpClient()

    let isEqual (collection1 : IEnumerable<int>) (collection2 : IEnumerable<int>) =
        let mutable result = true
        for el in collection1 do
            if not (collection2.Contains(el))
            then
                result <- false
        result
    
    [<Test>]
    member this.CreateSuperCategorySuccessfully() =
        let model = SuperCategoryModel()
        model.Name <- "New super category"
        model.Categories <- List<int>()
        let response = client.PostAsJsonAsync(url + "/admin/createsupercategory", model) |> Async.AwaitTask |> Async.RunSynchronously
        Assert.That(response.StatusCode = HttpStatusCode.OK)
        use context = new ApplicationContext()
        let createdModel = context.SuperCategory.First(fun sc -> sc.Id = model.Id)
        context.SuperCategory.Remove(createdModel) |> ignore
        Assert.That(createdModel.Name = model.Name &&
                    isEqual (createdModel.Categories.Select(fun c -> c.Id)) model.Categories)