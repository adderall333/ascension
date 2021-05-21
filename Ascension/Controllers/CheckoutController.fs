namespace Ascension

open System
open System.Collections.Generic
open ProductFilter
open System.Diagnostics
open System.Linq
open Microsoft.AspNetCore.Mvc
open Microsoft.EntityFrameworkCore
open Microsoft.EntityFrameworkCore
open Models
open System
open System.Collections.Generic
open System.Net
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Identity
open ProductFilter
open System.Diagnostics
open System.Linq
open Microsoft.AspNetCore.Mvc
open Microsoft.EntityFrameworkCore
open Models

type CheckoutController() =
    inherit Controller()


    member this.Checkout() = this.View()


    member this.Card(name: string, surname: string, email: string, address: string) =
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
        order.Status <- Status.NotPaid
        order.OrderTime <- DateTime.Now
        order.ProductLines <- productLines
        order.UserId <- userId
        order.RecipientName <- name
        order.RecipientSurname <- surname
        order.DeliveryType <- DeliveryType.Delivery
        order.RecipientEmail <- email
        order.DeliveryAddress <- address

        let amount =
            productLines
                .Select(fun p -> p.Product.Cost * p.ProductCount)
                .Sum()

        order.Amount <- amount

        context.Order.Add(order) |> ignore
        //removes product lines from db
        for productLine in productLines do
            productLine.CartId <- 0
            context.ProductLine.Update(productLine) |> ignore
            
        context.SaveChanges() |> ignore
        //fixed double SaveChanges
        this.View(order)

    member this.Orders(id: int) =
        let context = new ApplicationContext()

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
        
        this.View(order)


    member this.UpdateStatus(orderId: int, newStatus: Status) =
        use context = new ApplicationContext()

        let order =
            context.Order.First(fun o -> o.Id = orderId)

        order.Status <- newStatus
        context.SaveChanges() |> ignore
