module SimpleTests

open System.Net
open NUnit.Framework
open Tests

let factory = new AscensionFactory()

let client = ref (factory.CreateClient())

[<TestCase("", TestName = "Index")>]

[<TestCase("/catalog", TestName = "CatalogIndex")>]
[<TestCase("/catalog/category?name=Laptops", TestName = "CatalogCategory")>]
[<TestCase("/catalog/product/1", TestName = "CatalogProduct")>]

[<TestCase("/catalog?searchString=apple", TestName = "Search_apple")>]
[<TestCase("/catalog?searchString=lenovo", TestName = "Search_lenovo")>]

[<TestCase("/authentication/signin", TestName = "AuthSignin")>]
[<TestCase("/authentication/signup", TestName = "AuthSignup")>]

[<TestCase("/cart", TestName = "Cart")>]

let statusCode200AndContentIsHtml (page : string) =
    let response = client.Value.GetAsync(page) |> Async.AwaitTask |> Async.RunSynchronously
    Assert.That(response.Content.Headers.ContentType.MediaType = "text/html")
    Assert.That(response.StatusCode = HttpStatusCode.OK)