namespace Ascension

open Microsoft.AspNetCore.Http
open System.Linq
open Models

type AuthorizationByCookiesMiddleware(next : RequestDelegate) =
    member this.Invoke(context : HttpContext) =
        if not <| context.Session.Keys.Contains("isAuth") // Если пользователь не авторизован (проверяем по сессии)
        then // тогда проверим его куки, если в куках есть логин и пароль,
            if context.Request.Cookies.ContainsKey("email") && context.Request.Cookies.ContainsKey("hashedPass")
            then  // то проверим их в базе данных.
                let email = context.Request.Cookies.Item("email")
                let hashedPass = context.Request.Cookies.Item("hashedPass")
                use dbContext = new ApplicationContext()
                let dbUser = dbContext
                                 .User
                                 .Where(fun e -> e.Email = email)
                                 .Where(fun e -> e.HashedPassword = hashedPass)
                                 .ToList()
                                 .First()
                if not <| isNull dbUser // если логин и пароль совпадают,
                then // авторизуем пользователя в сессии 
                    context.Session.SetString("isAuth", "true")
                    context.Session.SetInt32("id", dbUser.Id) 
                    context.Session.SetString("email", dbUser.Email)
        // Если пользователь авторизован - ничего не делаем. 
        let task = next.Invoke(context)
        task