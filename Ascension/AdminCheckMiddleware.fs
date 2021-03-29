namespace Ascension

open System.Linq
open Microsoft.AspNetCore.Http
open Models

type AdminCheckMiddleware(next : RequestDelegate) =
    member this.Invoke(context : HttpContext) =
        if context.Request.Path.ToString().ToLower().StartsWith("/admin")
        then
            if context.Session.Keys.Contains("id")
            then
                use dbContext = new ApplicationContext()
                let id = context.Session.GetInt32("id").Value
                let user = dbContext.User.First(fun u -> u.Id = id)
                if not user.IsAdmin
                then
                    context.Response.StatusCode = StatusCodes.Status403Forbidden |> ignore
            else
                context.Response.StatusCode = StatusCodes.Status403Forbidden |> ignore
        let task = next.Invoke(context)
        task

