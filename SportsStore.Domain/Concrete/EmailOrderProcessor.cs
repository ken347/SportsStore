using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Concrete
{
    // TODO 替換為真實實現
    public class EmailSettings
    {
        public string MailToAddress = "orders@example.com";
        public string MailFromAddress = "sportsstore@example.com";
        public bool UseSsl = true;
        public string Username = "MySmtpUsername";
        public string Password = "MySmtpPassword";
        public string ServerName = "smtp.example.com";
        public int ServerPort = 587;
        // 沒有SMTP的替代方案，將郵件信息以.eml檔格式寫到指定的資料夾
        public bool WriteAsFile = true;  
        public string FileLocation = @"C:\root\RH\sports_store_emails";
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings _emailSettings;

        public EmailOrderProcessor(EmailSettings emailSettings) => _emailSettings = emailSettings;

        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = _emailSettings.UseSsl;
                smtpClient.Host = _emailSettings.ServerName;
                smtpClient.Port = _emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(_emailSettings.Username,
                    _emailSettings.Password);

                if (_emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = _emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder()
                    .AppendLine("已提交新訂單")
                    .AppendLine("---")
                    .AppendLine("品項:");

                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (小計: {2:c}", line.Quantity,
                                      line.Product.Name,
                                      subtotal);
                }

                body.AppendFormat("總訂單價值: {0:c}", cart.ComputeTotalValue())
                    .AppendLine("---")
                    .AppendLine("運送到:")
                    .AppendLine(shippingInfo.Name)
                    .AppendLine(shippingInfo.Line1)
                    .AppendLine(shippingInfo.Line2 ?? "")
                    .AppendLine(shippingInfo.Line3 ?? "")
                    .AppendLine(shippingInfo.City)
                    .AppendLine(shippingInfo.State ?? "")
                    .AppendLine(shippingInfo.Country)
                    .AppendLine(shippingInfo.Zip)
                    .AppendLine("---")
                    .AppendFormat("包裝: {0}",
                        shippingInfo.GiftWrap ? "Yes" : "No");

                MailMessage mailMessage = new MailMessage(
                                       _emailSettings.MailFromAddress,   // 從哪來
                                       _emailSettings.MailToAddress,     // 送到哪
                                       "新訂單已提交!",          // 標題
                                       body.ToString());                // 主體

                if (_emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }

                smtpClient.Send(mailMessage);
            }
        }
    }
}
