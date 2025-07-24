using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Threading.Tasks;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;
using System.Text;

namespace DiamondShopSystem.BLL.Services.SaleEmail
{
    public class SaleEmailService : ISaleEmailService
    {
        private readonly ILogger<SaleEmailService> _logger;
        private readonly Email.EmailSettings _emailSettings;

        public SaleEmailService(ILogger<SaleEmailService> logger, IOptions<Email.EmailSettings> emailSettings)
        {
            _logger = logger;
            _emailSettings = emailSettings.Value;
        }

        public async Task SendOrderUpdateEmailAsync(string recipientEmail, OrderResponseDto orderDetails)
        {
            try
            {
                using (var client = new SmtpClient(_emailSettings.SmtpHost))
                {
                    client.Port = _emailSettings.SmtpPort;
                    client.Credentials = new System.Net.NetworkCredential(_emailSettings.SmtpUser, _emailSettings.SmtpPass);
                    client.EnableSsl = true;

                    var subject = $"Diamond Order #{orderDetails.Id} - {((Enums.OrderStatus)orderDetails.Status).ToString()}";
                    var body = GenerateDiamondEmailTemplate(orderDetails);

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_emailSettings.FromEmail),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true,
                    };
                    mailMessage.To.Add(recipientEmail);

                    await client.SendMailAsync(mailMessage);
                    _logger.LogInformation("Order update email sent successfully to {Email} for Order {OrderId}", recipientEmail, orderDetails.Id);
                }
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, "Failed to send order update email to {Email} for Order {OrderId}. SMTP Error: {SmtpErrorCode}", recipientEmail, orderDetails.Id, ex.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while sending order update email to {Email} for Order {OrderId}", recipientEmail, orderDetails.Id);
            }
        }

        private string GenerateDiamondEmailTemplate(OrderResponseDto orderDetails)
        {
            var statusText = ((Enums.OrderStatus)orderDetails.Status).ToString();
            var orderDate = orderDetails.OrderDate.ToShortDateString();
            var totalPrice = orderDetails.TotalPrice.ToString("C");
            var vipStatus = orderDetails.VipApplied ? "Yes" : "No";

            var html = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Diamond Shop Order Update</title>
    <style>
        body {{
            margin: 0;
            padding: 0;
            font-family: 'Segoe UI', Arial, sans-serif;
            background: #f5f5f5;
            color: #333;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            background: #ffffff;
            box-shadow: 0 0 20px rgba(0,0,0,0.1);
        }}
        .header {{
            background: #1e3c72;
            padding: 40px 30px;
            text-align: center;
            color: white;
        }}
        .diamond-icon {{
            width: 48px;
            height: 48px;
            background: #fff;
            margin: 0 auto 20px;
            clip-path: polygon(50% 0%, 0% 100%, 100% 100%);
        }}
        .header h1 {{
            color: #fff;
            margin: 0;
            font-size: 28px;
            font-weight: 600;
            letter-spacing: 1px;
        }}
        .content {{
            padding: 40px 30px;
        }}
        .status-badge {{
            display: inline-block;
            padding: 15px 30px;
            background: #2a5298;
            color: white;
            font-weight: 600;
            font-size: 16px;
            margin: 20px 0;
            text-transform: uppercase;
            letter-spacing: 1px;
        }}
        .order-info {{
            background: #f8f9fa;
            padding: 30px;
            margin: 30px 0;
            border-left: 4px solid #2a5298;
        }}
        .order-info h3 {{
            color: #1e3c72;
            margin-top: 0;
            font-size: 18px;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 1px;
        }}
        .info-row {{
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 12px 0;
            border-bottom: 1px solid #e9ecef;
        }}
        .info-row:last-child {{
            border-bottom: none;
        }}
        .info-label {{
            font-weight: 600;
            color: #495057;
            text-transform: uppercase;
            font-size: 12px;
            letter-spacing: 0.5px;
        }}
        .info-value {{
            color: #1e3c72;
            font-weight: 600;
            font-size: 14px;
        }}
        .items-table {{
            width: 100%;
            border-collapse: collapse;
            margin: 25px 0;
            border: 1px solid #dee2e6;
        }}
        .items-table th {{
            background: #1e3c72;
            color: white;
            padding: 15px;
            text-align: left;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            font-size: 12px;
        }}
        .items-table td {{
            padding: 15px;
            border-bottom: 1px solid #dee2e6;
            background: #fff;
            font-size: 14px;
        }}
        .items-table tr:nth-child(even) td {{
            background: #f8f9fa;
        }}
        .delivery-section {{
            background: #fff3cd;
            padding: 30px;
            margin: 30px 0;
            border-left: 4px solid #ffc107;
        }}
        .delivery-section h3 {{
            color: #856404;
            margin-top: 0;
            font-size: 18px;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 1px;
        }}
        .footer {{
            background: #1e3c72;
            color: white;
            text-align: center;
            padding: 30px;
        }}
        .footer p {{
            margin: 0;
            font-size: 16px;
            font-weight: 500;
        }}
        @media (max-width: 600px) {{
            .container {{
                margin: 0;
                box-shadow: none;
            }}
            .content, .header, .footer {{
                padding: 20px;
            }}
            .info-row {{
                flex-direction: column;
                align-items: flex-start;
                gap: 8px;
            }}
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>DIAMOND SHOP SYSTEM</h1>
        </div>
        
        <div class='content'>
            <h2>Dear Valued Customer,</h2>
            <p>Your order has been updated with the following information:</p>
            
            <div class='status-badge'>
                STATUS: {statusText}
            </div>
            
            <div class='order-info'>
                <h3>Order Information</h3>
                <div class='info-row'>
                    <span class='info-label'>Order Number: </span>
                    <span class='info-value'>#{orderDetails.Id}</span>
                </div>
                <div class='info-row'>
                    <span class='info-label'>Order Date: </span>
                    <span class='info-value'>{orderDate}</span>
                </div>
                <div class='info-row'>
                    <span class='info-label'>Total Amount: </span>
                    <span class='info-value'>{totalPrice}</span>
                </div>
                <div class='info-row'>
                    <span class='info-label'>Sales Consultant: </span>
                    <span class='info-value'>{orderDetails.SaleStaff}</span>
                </div>
                <div class='info-row'>
                    <span class='info-label'>VIP Benefits Applied: </span>
                    <span class='info-value'>{vipStatus}</span>
                </div>
            </div>";

            // Add items section if available
            if (orderDetails.OrderDetails != null && orderDetails.OrderDetails.Any())
            {
                html += @"
            <div class='order-info'>
                <h3>Order Items</h3>
                <table class='items-table'>
                    <thead>
                        <tr>
                            <th>Product ID</th>
                            <th>Quantity</th>
                            <th>Unit Price</th>
                        </tr>
                    </thead>
                    <tbody>";

                foreach (var item in orderDetails.OrderDetails)
                {
                    html += $@"
                        <tr>
                            <td>{item.Id}</td>
                            <td>{item.Quantity}</td>
                            <td>{item.UnitPrice:C}</td>
                        </tr>";
                }

                html += @"
                    </tbody>
                </table>
            </div>";
            }

            // Add delivery section if available
            if (orderDetails.Delivery != null)
            {
                var dispatchTime = orderDetails.Delivery.DispatchTime?.ToString() ?? "Preparing";
                var deliveryTime = orderDetails.Delivery.DeliveryTime?.ToString() ?? "TBD";

                html += $@"
            <div class='delivery-section'>
                <h3>Delivery Information</h3>
                <div class='info-row'>
                    <span class='info-label'>Shipping Address</span>
                    <span class='info-value'>{orderDetails.Delivery.ShippingAddress}</span>
                </div>
                <div class='info-row'>
                    <span class='info-label'>Dispatch Time</span>
                    <span class='info-value'>{dispatchTime}</span>
                </div>
                <div class='info-row'>
                    <span class='info-label'>Expected Delivery</span>
                    <span class='info-value'>{deliveryTime}</span>
                </div>
                <div class='info-row'>
                    <span class='info-label'>Delivery Status</span>
                    <span class='info-value'>{orderDetails.Delivery.Status}</span>
                </div>
            </div>";
            }

            html += @"
        </div>
        
        <div class='footer'>
            <p>THANK YOU FOR CHOOSING OUR DIAMOND COLLECTION</p>
            <p>Your trust is our commitment</p>
        </div>
    </div>
</body>
</html>";

            return html;
        }
    }
}