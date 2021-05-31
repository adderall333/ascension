namespace Ascension

open System
open MimeKit;
open MailKit.Net.Smtp
open Models

module EmailService =
    
    let GetMessageForOrder(order:Order) =
        "<div class=\"col-md-9 order-content\">
            <div class=\"form_main col-md-4 col-sm-5 col-xs-7\">
                <h4 class=\"heading\"><strong>Order №" + order.Id.ToString() + "</strong></h4>
            </div>
            <div>
                <p>Time: " + order.OrderTime.ToString() + "</p>
                <p>Status: " + order.Status.ToString() + "</p>
                <p>Amount: " + order.Amount.ToString() + " $</p>
                <p>Delivery Address: " + order.DeliveryAddress.ToString() + "</p>
                <p>Recipient Name: " + order.RecipientName.ToString() + "</p>
                <p>Recipient Surname: " + order.RecipientSurname.ToString() + "</p>
                <p>Recipient Email: " + order.RecipientEmail.ToString() + "</p>
            </div>
        </div>"
    
    let SendEmail(email:string, subject:string, message:string) =
        
        let emailMessage = MimeMessage()
        emailMessage.From.Add(MailboxAddress("Ascension site administration", "ascensiongroupshop@gmail.com"));
        emailMessage.To.Add(MailboxAddress("", email));
        emailMessage.Headers.Add("Precedence", "bulk");
        emailMessage.Subject <- subject;
        emailMessage.Date <- DateTimeOffset.Now
        
        let htmlMessage = TextPart(MimeKit.Text.TextFormat.Html)
        htmlMessage.Text <- message
        emailMessage.Body <- htmlMessage
        
        use client = new SmtpClient()
        client.Connect("smtp.gmail.com", 465, true)
        client.Authenticate("ascensiongroupshop@gmail.com", "Vjacheslavovich098123MMM")
        client.Send(emailMessage)
        client.Disconnect(true)