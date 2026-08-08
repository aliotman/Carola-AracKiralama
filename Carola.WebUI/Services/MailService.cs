using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Carola.WebUI.Services
{
    public class MailService
    {
        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendApprovalMailAsync(string toEmail, string toName, int reservationId, decimal totalPrice, string pickupDate, string returnDate, string couponCode)
        {
            var mailSettings = _configuration.GetSection("MailSettings");

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(mailSettings["DisplayName"], mailSettings["Email"]));
            email.To.Add(new MailboxAddress(toName, toEmail));
            email.Subject = "Rezervasyonunuz Onaylandı! 🎉";

            var builder = new BodyBuilder();
            builder.HtmlBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: 'Segoe UI', Arial, sans-serif; background: #f4f4f4; margin: 0; padding: 0; }}
        .container {{ max-width: 600px; margin: 40px auto; background: #ffffff; border-radius: 16px; overflow: hidden; box-shadow: 0 4px 24px rgba(0,0,0,.1); }}
        .header {{ background: linear-gradient(135deg, #1a1a2e, #16213e); padding: 40px; text-align: center; }}
        .header img {{ width: 60px; }}
        .header h1 {{ color: #ffffff; font-size: 28px; margin: 16px 0 0; }}
        .header p {{ color: #a0aec0; margin: 8px 0 0; }}
        .content {{ padding: 40px; }}
        .greeting {{ font-size: 18px; font-weight: 700; color: #1a1a2e; margin-bottom: 16px; }}
        .info-box {{ background: #f8f9fa; border-radius: 12px; padding: 24px; margin: 24px 0; }}
        .info-row {{ display: flex; justify-content: space-between; padding: 10px 0; border-bottom: 1px solid #e2e8f0; }}
        .info-row:last-child {{ border-bottom: none; }}
        .info-label {{ color: #718096; font-size: 14px; }}
        .info-value {{ color: #1a1a2e; font-weight: 600; font-size: 14px; }}
        .price {{ font-size: 24px; font-weight: 800; color: #e63946; }}
        .coupon-box {{ background: linear-gradient(135deg, #667eea, #764ba2); border-radius: 16px; padding: 32px; margin: 28px 0; text-align: center; }}
        .coupon-title {{ color: rgba(255,255,255,.8); font-size: 14px; font-weight: 600; text-transform: uppercase; letter-spacing: 1px; }}
        .coupon-discount {{ color: #ffffff; font-size: 48px; font-weight: 800; margin: 8px 0; }}
        .coupon-subtitle {{ color: rgba(255,255,255,.7); font-size: 13px; margin-bottom: 20px; }}
        .coupon-code-box {{ background: rgba(255,255,255,.15); border: 2px dashed rgba(255,255,255,.4); border-radius: 10px; padding: 14px 24px; display: inline-block; }}
        .coupon-code {{ color: #ffffff; font-size: 24px; font-weight: 800; letter-spacing: 4px; }}
        .coupon-note {{ color: rgba(255,255,255,.6); font-size: 12px; margin-top: 12px; }}
        .btn {{ display: inline-block; background: #e63946; color: #ffffff; padding: 14px 32px; border-radius: 10px; text-decoration: none; font-weight: 700; font-size: 15px; margin-top: 24px; }}
        .footer {{ background: #f8f9fa; padding: 24px 40px; text-align: center; }}
        .footer p {{ color: #a0aec0; font-size: 12px; margin: 4px 0; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🚗 Carola Rent A Car</h1>
            <p>Rezervasyonunuz Onaylandı!</p>
        </div>
        <div class='content'>
            <div class='greeting'>Merhaba, {toName}! 👋</div>
            <p style='color:#718096;line-height:1.7;'>Rezervasyon talebiniz başarıyla onaylanmıştır. Aşağıda rezervasyon detaylarınızı bulabilirsiniz.</p>

            <div class='info-box'>
                <div class='info-row'>
                    <span class='info-label'>Rezervasyon No</span>
                    <span class='info-value'>#@reservationId</span>
                </div>
                <div class='info-row'>
                    <span class='info-label'>Alış Tarihi</span>
                    <span class='info-value'>@pickupDate</span>
                </div>
                <div class='info-row'>
                    <span class='info-label'>İade Tarihi</span>
                    <span class='info-value'>@returnDate</span>
                </div>
                <div class='info-row'>
                    <span class='info-label'>Toplam Tutar</span>
                    <span class='info-value price'>$@totalPrice</span>
                </div>
            </div>

            <div class='coupon-box'>
                <div class='coupon-title'>🎁 Size Özel İndirim Kuponu</div>
                <div class='coupon-discount'>%30</div>
                <div class='coupon-subtitle'>Bir sonraki kiralamanızda geçerlidir</div>
                <div class='coupon-code-box'>
                    <div class='coupon-code'>@couponCode</div>
                </div>
                <div class='coupon-note'>Bu kupon 30 gün geçerlidir.</div>
            </div>

            <p style='color:#718096;line-height:1.7;'>Herhangi bir sorunuz olursa bizimle iletişime geçmekten çekinmeyin.</p>
            <a href='#' class='btn'>Rezervasyonumu Görüntüle</a>
        </div>
        <div class='footer'>
            <p>© 2024 Carola Rent A Car. Tüm hakları saklıdır.</p>
            <p>Bu e-posta otomatik olarak gönderilmiştir.</p>
        </div>
    </div>
</body>
</html>";

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(mailSettings["Host"], int.Parse(mailSettings["Port"]!), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(mailSettings["Email"], mailSettings["Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public string GenerateCouponCode()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return "CAROLA-" + new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}