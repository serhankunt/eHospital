

using GenericEmailService;

namespace eHospitalServer.Business.Services;
public static class EmailHelper
{
    public static async Task<string> SendEmailAsync(string email, string subject, string body )
    {
       
        EmailConfigurations emailConfigurations = new(
            "smtp.gmail.com",
            "fxjo zimn owlm mzeq",
            587,
            false,
            true);

        EmailModel<Stream> emailModel = new(
            emailConfigurations,
        "hserhan006@gmail.com",
            new List<string> { email },
            subject,
            body,
            null);


        string emailResponse = await EmailService.SendEmailWithMailKitAsync(emailModel);

        return emailResponse;
    }
}
