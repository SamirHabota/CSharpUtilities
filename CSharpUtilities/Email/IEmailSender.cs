using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpUtilities.Email
{

    #region StartupConfiguration
    /*
    services.AddScoped<IEmailSender, EmailSender>();
    var emailConfig = Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
    services.AddSingleton(emailConfig);
    */
    #endregion

    #region AppsettingJsonConfiguration
    /*
    "EmailConfiguration": {
        "From": "mail.bluemoon.info@gmail.com",
        "SmtpServer": "smtp.gmail.com",
        "Port": 465,
        "Username": "YOURMAIL",
        "Password": "YOUR MAIL PASSWORD"
    },
    */
    #endregion

    #region GmailConfiguration
    //Allow less secure apps for your Google account
    #endregion

    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
