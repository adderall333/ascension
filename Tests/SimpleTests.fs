module Tests

open System.Net
open System.Net.Http
open NUnit.Framework

let url = "http://localhost:5000"

let client = new HttpClient()

//main
[<TestCase("", TestName = "Index")>]

//catalog
[<TestCase("/catalog", TestName = "CatalogIndex")>]
[<TestCase("/catalog/category?name=Laptops", TestName = "CatalogCategory")>]
[<TestCase("/catalog/product/1", TestName = "CatalogProduct")>]

//admin
[<TestCase("/admin", TestName = "AdminIndex")>]

[<TestCase("/admin/models?name=product", TestName = "AdminModels_Product")>]
[<TestCase("/admin/models?name=specificationoption", TestName = "AdminModels_SpecificationOption")>]
[<TestCase("/admin/models?name=specification", TestName = "AdminModels_Specification")>]
[<TestCase("/admin/models?name=category", TestName = "AdminModels_Category")>]
[<TestCase("/admin/models?name=supercategory", TestName = "AdminModels_SuperCaetgory")>]

[<TestCase("/admin/create?name=product", TestName = "AdminCreate_Product")>]
[<TestCase("/admin/create?name=specificationoption", TestName = "AdminCreate_SpecificationOption")>]
[<TestCase("/admin/create?name=specification", TestName = "AdminCreate_Specification")>]
[<TestCase("/admin/create?name=category", TestName = "AdminCreate_Category")>]
[<TestCase("/admin/create?name=supercategory", TestName = "AdminCreate_SuperCaetgory")>]

[<TestCase("/admin/read/1?name=product", TestName = "AdminRead_Product")>]
[<TestCase("/admin/read/1?name=specificationoption", TestName = "AdminRead_SpecificationOption")>]
[<TestCase("/admin/read/1?name=specification", TestName = "AdminRead_Specification")>]
[<TestCase("/admin/read/1?name=category", TestName = "AdminRead_Category")>]
[<TestCase("/admin/read/1?name=supercategory", TestName = "AdminRead_SuperCaetgory")>]

[<TestCase("/admin/update/1?name=product", TestName = "AdminUpdate_Product")>]
[<TestCase("/admin/update/1?name=specificationoption", TestName = "AdminUpdate_SpecificationOption")>]
[<TestCase("/admin/update/1?name=specification", TestName = "AdminUpdate_Specification")>]
[<TestCase("/admin/update/1?name=category", TestName = "AdminUpdate_Category")>]
[<TestCase("/admin/update/1?name=supercategory", TestName = "AdminUpdate_SuperCaetgory")>]

//auth
[<TestCase("/authentication/signin", TestName = "AuthSignin")>]
[<TestCase("/authentication/signup", TestName = "AuthSignup")>]

let StatusCode200 (page : string) =
    let response = client.GetAsync(url + page) |> Async.AwaitTask |> Async.RunSynchronously
    Assert.That(response.StatusCode = HttpStatusCode.OK)