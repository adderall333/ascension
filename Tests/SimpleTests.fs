module SimpleTests

open System.Net
open NUnit.Framework
open Tests

let factory = new AscensionFactory()

let client = ref (factory.CreateClient())

//main
[<TestCase("", TestName = "Index")>]

//catalog
[<TestCase("/catalog", TestName = "CatalogIndex")>]
[<TestCase("/catalog/category?name=Laptops", TestName = "CatalogCategory")>]
[<TestCase("/catalog/product/1", TestName = "CatalogProduct")>]

//search
[<TestCase("/catalog?searchString=apple", TestName = "Search_apple")>]
[<TestCase("/catalog?searchString=lenovo", TestName = "Search_lenovo")>]

//auth
[<TestCase("/authentication/signin", TestName = "AuthSignin")>]
[<TestCase("/authentication/signup", TestName = "AuthSignup")>]

//cart
[<TestCase("/cart", TestName = "Cart")>]

let statusCode200 (page : string) =
    let response = client.Value.GetAsync(page) |> Async.AwaitTask |> Async.RunSynchronously
    Assert.That(response.StatusCode = HttpStatusCode.OK)
    
