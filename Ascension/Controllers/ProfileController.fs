namespace Ascension

open Microsoft.AspNetCore.Http
open System.Linq
open Microsoft.AspNetCore.Mvc
open Models
open Ascension
open Microsoft.Extensions.Primitives
open System.Text.RegularExpressions;

type ProfileController() =
    inherit Controller()

    let FindUser (email : string, old_email : string) =
        use context = new ApplicationContext()
        if(old_email <> email) then
            context.User.Where(fun e -> e.Email = email).ToList().Any()
        else
            false
    let ValidateUser(user : UserToRegister) =
        let nameRegex = new Regex("^[A-Z][a-zA-Z]+$")
        let nameCheck = nameRegex.IsMatch user.Name
        
        let surnameRegex = new Regex("^[A-Z][a-zA-Z]+$")
        let surnameCheck = surnameRegex.IsMatch user.Surname
        
        let emailPattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                           @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$"
        let emailCheck = Regex.IsMatch(user.Email, emailPattern, RegexOptions.IgnoreCase)
        
        nameCheck && surnameCheck && emailCheck
    let isAuth (context : HttpContext) =
        if context.Session.Keys.Contains("isAuth")
            then
                true
            else
                false
    member this.Personal()  =
        if isAuth this.HttpContext then      
            let id = this.HttpContext.Session.GetInt32("id") |> int
            use dbContext = new ApplicationContext()
            let dbUser = dbContext
                             .User
                             .Where(fun e -> e.Id = id)
                             .ToList()
                             .First()    
            this.View(dbUser) :> ActionResult
        else
            this.Redirect("../Authentication/Signin") :> ActionResult
          
    member this.Order() =
        if isAuth this.HttpContext then    
            use context = new ApplicationContext()
            let userId = this.HttpContext.Session.GetInt32("id") |> int
            let orders = context.Order.Where(fun c -> c.UserId = userId).ToList()
            
            this.View(orders) :> ActionResult
        else
            this.Redirect("../Authentication/Signin") :> ActionResult
            
    [<HttpPost>]  
    member this.TryEdit(user : UserToRegister) =
        let id = this.HttpContext.Session.GetInt32("id") |> int
        use dbContext = new ApplicationContext()
        let dbUser = dbContext
                      .User
                      .Where(fun e -> e.Id = id)
                      .ToList()
                      .First()
        let valid = ValidateUser user
        if not valid then this.Response.Headers.Add("edit_result", StringValues("error"))
        elif FindUser (user.Email, dbUser.Email) then this.Response.Headers.Add("edit_result", StringValues("failed"))
        else
            let hashedPassword = Crypto.GetHashPassword user.Pass
            dbUser.Name <- user.Name
            dbUser.Surname <- user.Surname
            this.HttpContext.Session.SetString("email", user.Email)
            dbUser.Email <- user.Email
            if(hashedPassword <> null)
            then
                dbUser.HashedPassword <- hashedPassword
            dbContext.SaveChanges() |> ignore       
            this.Response.Headers.Add("edit_result", StringValues("ok"))
//    member this.Cart() = this.View()
