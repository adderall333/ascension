namespace Ascension.Controllers

open System
open System.Text.RegularExpressions
open Ascension
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Primitives
open System.Linq
open Microsoft.EntityFrameworkCore
open Models
open Microsoft.AspNetCore.Http

type CheckoutController() =
    inherit Controller()

    let ValidateForm(form : FormToOrder) =
        let nameRegex = new Regex("^[A-Z][a-zA-Z]+$")
        let nameCheck = nameRegex.IsMatch form.Name
        
        let surnameRegex = new Regex("^[A-Z][a-zA-Z]+$")
        let surnameCheck = surnameRegex.IsMatch form.Surname
        
        let emailPattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                           @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$"
        let emailCheck = Regex.IsMatch(form.Email, emailPattern, RegexOptions.IgnoreCase)
        
        nameCheck && surnameCheck && emailCheck
    let isAuth (context : HttpContext) =
        if context.Session.Keys.Contains("isAuth")
            then
                true
            else
                false
          
    let matchIds (context : ApplicationContext, id : int, httpContext : HttpContext) =
        let userId = httpContext.Session.GetInt32("id") |> int
        if context.Order.Where(fun c -> c.UserId = userId).Where(fun i -> i.Id = id).Any() then
            true 
        else
            false
    member this.Checkout() =
        use context = new ApplicationContext()
        let mutable id = 0
        if (isAuth this.HttpContext) then
            id <- (this.HttpContext.Session.GetInt32("id") |> int)
        let user = context.User.Where(fun i -> i.Id = id).FirstOrDefault()
        if (isAuth this.HttpContext && context.Cart.Where(fun c -> c.AuthorizedUserId = id).Any()) then
            this.View(user) :> ActionResult
        elif isAuth this.HttpContext then
            this.Redirect("../../Catalog") :> ActionResult
        else
            this.Redirect("../../Authentication/Signin") :> ActionResult

    [<HttpPost>]
    member this.TrySendForm(form : FormToOrder) =
        
        let valid = ValidateForm form
        if not valid then this.Response.Headers.Add("order_result", StringValues("error"))
        else
            use context = new ApplicationContext()

            let userId =
                this.HttpContext.Session.GetInt32("id") |> int

            let cartId =
                context
                    .Cart
                    .First(fun c -> c.AuthorizedUserId = userId)
                    .Id

            let productLines =
                context
                    .ProductLine
                    .Where(fun p -> p.CartId = cartId)
                    .Include(fun p -> p.Product)
                    .ToList()
            //updating db
            let order = Order()
            order.Status <- Status.Paid
            
            let moscowZone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time")
            let utcTime = DateTime.Now.ToUniversalTime()
            let moscowTime = TimeZoneInfo.ConvertTime(utcTime, moscowZone)
            order.OrderTime <- moscowTime
            
            order.ProductLines <- productLines
            order.UserId <- userId
            order.RecipientName <- form.Name
            order.RecipientSurname <- form.Surname
            order.DeliveryType <- DeliveryType.Delivery
            order.RecipientEmail <- form.Email
            order.DeliveryAddress <- form.Address

            let amount =
                productLines
                    .Select(fun p -> p.Product.Cost * p.ProductCount)
                    .Sum()

            order.Amount <- amount
           
            //removing cart   
            let oldCart = context.Cart.FirstOrDefault(fun c -> c.AuthorizedUserId = userId)
            context.Cart.Remove(oldCart) |> ignore
            this.HttpContext.Session.Remove("cartId")
            
            context.Order.Add(order) |> ignore
            //removes product lines from db
            for productLine in productLines do
                productLine.CartId <- 0
                context.ProductLine.Update(productLine) |> ignore
                
            context.SaveChanges() |> ignore
            
            //update purchases statistics
            Purchase.UpdatePurchases(order, context)
            
            this.Response.Headers.Add("order_result", StringValues("ok"))
            
            //email
            let message = EmailService.GetMessageForOrder(order)
            EmailService.SendEmail(order.RecipientEmail, "The new order has been successfully created", message)
        
    
    member this.Card() = //name: string, surname: string, email: string, address: string
        this.View()

    member this.Orders(id: int) =
        let context = new ApplicationContext()
        if isAuth this.HttpContext && matchIds(context, id, this.HttpContext) then
            //order to Order list
            let order =
                context
                    .Order
                    .Single(fun i -> i.Id = id)
            context.Entry(order).Collection("ProductLines").Load()
            for product in order.ProductLines do
                context.Entry(product).Reference("Product").Load()
                
            for image in order.ProductLines.Select(fun i -> i.Product).ToList() do
                context.Entry(image).Collection("Images").Load()
            
            this.View(order) :> ActionResult
        elif isAuth this.HttpContext then
            this.Redirect("../../Profile/Order") :> ActionResult
        else
            this.Redirect("../../Authentication/Signin") :> ActionResult
            
            
            


    member this.UpdateStatus(orderId: int, newStatus: Status) =
        use context = new ApplicationContext()

        let order =
            context.Order.First(fun o -> o.Id = orderId)

        order.Status <- newStatus
        context.SaveChanges() |> ignore
