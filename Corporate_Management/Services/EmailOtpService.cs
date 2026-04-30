using System.Net;
using System.Net.Mail;

namespace Corporate_Management.Services
{
    public class EmailOtpService
    {
        private readonly IConfiguration _config;
        public EmailOtpService(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateOTP(int length = 6)
        {
            var random = new Random();
            var otp = "";

            for (int i = 0; i < length; i++)
                otp += random.Next(0, 10); // 0-9

            return otp;
        }
        public async Task<bool> sendOtpEmail(string EmailId, string otp, string username)
        {
            try
            {
                var settings = _config.GetSection("EmailServiceSettings");
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(settings["SenderEmail"]);
                mailMessage.To.Add(EmailId);
                mailMessage.Subject = "OTP Verification";
                mailMessage.Body = SendOtpEmailBody(otp, username);
                mailMessage.IsBodyHtml = true;
                var smtp = new SmtpClient(settings["smtpServer"])
                {
                    Port = int.Parse(settings["Port"]),
                    Credentials = new NetworkCredential(
                    settings["SenderEmail"],
                    settings["Password"]
                    ),
                    EnableSsl = true
                };
                await smtp.SendMailAsync(mailMessage);
                DateTime expiryTime = DateTime.Now.AddMinutes(5);
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }
        private string SendOtpEmailBody(string otp, string username)
        {
            int currentYear = DateTime.Now.Year;
            return $@"
<!DOCTYPE html>
<html lang='en'>
<head>
  <meta charset='UTF-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
  <title>Verify your identity</title>
</head>
<body style='margin:0; padding:0; background:#f0f4f8; font-family:Arial, sans-serif;'>

  <table width='100%' cellpadding='0' cellspacing='0' role='presentation'
    style='background:#f0f4f8; padding:40px 0;'>
    <tr>
      <td align='center'>
        <table width='520' cellpadding='0' cellspacing='0' role='presentation'
          style='width:100%; max-width:520px; background:#ffffff; border-radius:12px;
                 border:1px solid #d6eaee; overflow:hidden;
                 box-shadow:0 2px 12px rgba(9,99,126,0.08);'>

          <!-- HEADER -->
          <tr>
            <td style='background:linear-gradient(135deg,#09637e,#088395);
                       padding:26px 36px; text-align:center;'>
              <p style='margin:0 0 3px; font-size:11px; font-weight:700;
                        color:rgba(255,255,255,0.6); letter-spacing:1.5px;
                        text-transform:uppercase;'>
                Human Resource Management
              </p>
              <p style='margin:0; font-size:20px; font-weight:900; color:#ffffff;
                        letter-spacing:0.5px;'>
                HRMS
              </p>
            </td>
          </tr>

          <!-- BODY -->
          <tr>
            <td style='padding:32px 36px 28px;'>

              <p style='margin:0 0 6px; font-size:17px; font-weight:800; color:#0d2d36;'>
                Verify your identity
              </p>
              <p style='margin:0 0 26px; font-size:14px; color:#5a8a94; line-height:1.65;'>
                Hi <strong style='color:#0d2d36;'>{username}</strong>, use the OTP below
                to complete your request.
              </p>

              <!-- OTP BOX -->
              <table width='100%' cellpadding='0' cellspacing='0' role='presentation'>
                <tr>
                  <td align='center' style='padding-bottom:24px;'>
                    <div style='display:inline-block; background:#ebf4f6;
                                border:1.5px solid #d6eaee; border-radius:10px;
                                padding:14px 48px; text-align:center;'>
                      <p style='margin:0 0 5px; font-size:10px; font-weight:700;
                                color:#5a8a94; letter-spacing:1.2px; text-transform:uppercase;'>
                        One-Time Password
                      </p>
                      <p style='margin:0; font-size:32px; font-weight:900;
                                color:#09637e; letter-spacing:10px; text-indent:10px;
                                font-family:Courier New, monospace;'>
                        {otp}
                      </p>
                    </div>
                  </td>
                </tr>
              </table>

              <!-- EXPIRY NOTICE -->
              <table width='100%' cellpadding='0' cellspacing='0' role='presentation'
                style='margin-bottom:22px;'>
                <tr>
                  <td style='background:#fef3c7; border-radius:8px; padding:11px 16px;'>
                    <p style='margin:0; font-size:13px; font-weight:700; color:#92400e;'>
                      &#9203;&nbsp; This OTP expires in <strong>5 minutes</strong>.
                      Do not share it with anyone.
                    </p>
                  </td>
                </tr>
              </table>

              <!-- DIVIDER -->
              <table width='100%' cellpadding='0' cellspacing='0' role='presentation'>
                <tr><td style='border-top:1px solid #d6eaee; padding-bottom:20px;'></td></tr>
              </table>

              <p style='margin:0 0 18px; font-size:13px; color:#5a8a94; line-height:1.7;'>
                If you did not request this, you can safely ignore this email.
                Your account remains secure.
              </p>

              <p style='margin:0; font-size:13px; color:#5a8a94; line-height:1.7;'>
                Regards,<br>
                <strong style='color:#0d2d36;'>HRMS Support Team</strong>
              </p>

            </td>
          </tr>

          <!-- FOOTER -->
          <tr>
            <td style='background:#f0f8fa; border-top:1px solid #d6eaee;
                       padding:15px 36px; text-align:center;'>
              <p style='margin:0 0 4px; font-size:11px; color:#5a8a94;'>
                &copy; {currentYear} Human Resource Management System &nbsp;&middot;&nbsp; All rights reserved
              </p>
              <p style='margin:0; font-size:11px; color:#94a3b8;'>
                This is an automated message — please do not reply.
              </p>
            </td>
          </tr>

        </table>
      </td>
    </tr>
  </table>

</body>
</html>";
        }
    }                                
}
