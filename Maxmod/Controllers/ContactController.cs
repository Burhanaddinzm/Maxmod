using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

namespace Maxmod.Controllers;

public class ContactController : Controller
{
    private readonly IConfiguration _configuration;

    public ContactController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SendEmail(string name, string email, string phone, string message)
    {
        SmtpClient smtpClient = new SmtpClient(_configuration["EmailSettings:Host"], int.Parse(_configuration["EmailSettings:Port"]))
        {
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_configuration["EmailSettings:Email"], _configuration["EmailSettings:Password"])
        };
        MailMessage mailMessage = new MailMessage()
        {
            From = new MailAddress(_configuration["EmailSettings:Email"]),
            Subject = "Contact Form Submission",
            Body = $"Name: {name}\nEmail: {email}\nPhone: {phone}\n\nMessage:\n{message}",
            IsBodyHtml = false,
        };

        mailMessage.To.Add(_configuration["EmailSettings:Email"]);
        mailMessage.ReplyToList.Add(new MailAddress(email));  // Set the user's email as Reply-To

        try
        {
            await smtpClient.SendMailAsync(mailMessage);
            ViewData["Notification"] = "Email sent successfully!";
            return RedirectToAction("Index", "Notification");
        }
        catch (Exception ex)
        {
            ViewData["Notification"] = $"Error sending email: {ex.Message}";
            return RedirectToAction("Index", "Notification");
        }
    }
}
