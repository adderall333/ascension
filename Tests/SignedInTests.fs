module SignedInTests

open System
open System.Net
open Microsoft.AspNetCore.Mvc.Testing.Handlers
open Models
open NUnit.Framework
open NUnit.Framework
open Tests
open System.Linq

let user = (new ApplicationContext()).User.First(fun u -> u.IsAdmin && u.Email = "sagadeev.artem2001@gmail.com")

let baseAddress = Uri("http://localhost")

let cookieContainer = CookieContainer()
cookieContainer.Add(baseAddress, Cookie("email", user.Email))
cookieContainer.Add(baseAddress, Cookie("hashedPass", user.HashedPassword))

let factory = new AscensionFactory()

let cookieContainerHandler = new CookieContainerHandler(cookieContainer)

let client = ref (factory.CreateDefaultClient(cookieContainerHandler))

[<TestCase("/profile/personal", TestName = "Profile_Personal")>]
[<TestCase("/profile/order", TestName = "Profile_Orders")>]

[<TestCase("/checkout/card", TestName = "Checkout_Card")>]
[<TestCase("/checkout/orders/152", TestName = "Checkout_Order")>]

[<TestCase("/admin", TestName = "AdminIndex")>]

[<TestCase("/admin/models?name=product", TestName = "AdminModels_Product")>]
[<TestCase("/admin/models?name=specificationoption", TestName = "AdminModels_SpecificationOption")>]
[<TestCase("/admin/models?name=specification", TestName = "AdminModels_Specification")>]
[<TestCase("/admin/models?name=category", TestName = "AdminModels_Category")>]
[<TestCase("/admin/models?name=supercategory", TestName = "AdminModels_SuperCaetgory")>]
[<TestCase("/admin/models?name=image", TestName = "AdminModels_Image")>]

[<TestCase("/admin/create?name=product", TestName = "AdminCreate_Product")>]
[<TestCase("/admin/create?name=specificationoption", TestName = "AdminCreate_SpecificationOption")>]
[<TestCase("/admin/create?name=specification", TestName = "AdminCreate_Specification")>]
[<TestCase("/admin/create?name=category", TestName = "AdminCreate_Category")>]
[<TestCase("/admin/create?name=supercategory", TestName = "AdminCreate_SuperCaetgory")>]
[<TestCase("/admin/create?name=image", TestName = "AdminCreate_Image")>]

[<TestCase("/admin/read/1?name=product", TestName = "AdminRead_Product")>]
[<TestCase("/admin/read/1?name=specificationoption", TestName = "AdminRead_SpecificationOption")>]
[<TestCase("/admin/read/1?name=specification", TestName = "AdminRead_Specification")>]
[<TestCase("/admin/read/1?name=category", TestName = "AdminRead_Category")>]
[<TestCase("/admin/read/1?name=supercategory", TestName = "AdminRead_SuperCaetgory")>]
[<TestCase("/admin/read/1?name=image", TestName = "AdminRead_Image")>]

[<TestCase("/admin/update/1?name=product", TestName = "AdminUpdate_Product")>]
[<TestCase("/admin/update/1?name=specificationoption", TestName = "AdminUpdate_SpecificationOption")>]
[<TestCase("/admin/update/1?name=specification", TestName = "AdminUpdate_Specification")>]
[<TestCase("/admin/update/1?name=category", TestName = "AdminUpdate_Category")>]
[<TestCase("/admin/update/1?name=supercategory", TestName = "AdminUpdate_SuperCaetgory")>]
[<TestCase("/admin/update/1?name=image", TestName = "AdminUpdate_Image")>]

[<TestCase("/admin/orders", TestName = "Admin_Orders")>]

[<TestCase("/admin/admins", TestName = "Admin_Admins")>]

let statusCode200AndContentIsHtml (page : string) =
    let response = client.Value.GetAsync(page) |> Async.AwaitTask |> Async.RunSynchronously
    Assert.That(response.Content.Headers.ContentType.MediaType = "text/html")
    Assert.That(response.StatusCode = HttpStatusCode.OK)