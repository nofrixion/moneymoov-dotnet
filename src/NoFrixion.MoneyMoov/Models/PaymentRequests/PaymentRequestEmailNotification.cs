//-----------------------------------------------------------------------------
// Filename: PaymentRequestEmailNotification.cs
// 
// Description: Represents the configuration properties for sending an email
// notification from a batch file processing job upon the successful creation
// of a payment request.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 20 Sep 2023  Aaron Clauson   Created, Harcourt Street, Dublin, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

using Quartz.Util;

namespace NoFrixion.MoneyMoov.Models;

public class PaymentRequestEmailNotification
{
    /// <summary>
    /// Name of the email template for the notification to send. 
    /// </summary>
    public string TemplateName { get; set; } = string.Empty;

    /// <summary>
    /// Email address to send the notification to. Can be multiple comma seaprated
    /// addresses.
    /// </summary>
    public string ToEmailAddress { get; set; } = string.Empty;

    /// <summary>
    /// Email address to use for the email notification from address.
    /// </summary>
    public string FromEmailAddress { get; set; } = string.Empty;

    /// <summary>
    /// Subject for the email notification.
    /// </summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// A mapping dictionary in the format:
    /// { "Template Variable Name": "Payment Request Property Value"}.
    /// where:
    /// "Template Variable Name" must exactly match the name of the variable in the email template.
    /// "Payment Request Property Value" can be one of:
    ///   - a simple value, e.g. "true" or "1234".
    ///   - the name of a Payment Request property in braces, e.g. "{Amount}".
    ///   - a combination of the two, e.g. "{Currency} {Amount} deposit"
    /// </summary>
    public Dictionary<string, string> TemplateVariables { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// Substitutes the payment request properties into the fields that can be set from payment request properties.
    /// </summary>
    /// <param name="paymentRequest">The payment request to set the substitution variables from.</param>
    /// <returns>A new payment request email notification instance with the substitution variables set.</returns>
    public PaymentRequestEmailNotification SubstituteVariables(PaymentRequest paymentRequest)
    {
        var notification = new PaymentRequestEmailNotification
        {
            TemplateName = TemplateName
        };

        notification.ToEmailAddress = Substitute(ToEmailAddress, paymentRequest);
        notification.FromEmailAddress = Substitute(FromEmailAddress, paymentRequest);
        notification.Subject = Substitute(Subject, paymentRequest);

        foreach (var kvp in TemplateVariables)
        {
            notification.TemplateVariables.Add(kvp.Key, Substitute(kvp.Value, paymentRequest));
        }

        return notification;
    }

    private string Substitute(string formatString, PaymentRequest paymentRequest)
    {
        if (string.IsNullOrEmpty(formatString))
        {
            return formatString;
        }

        string substitutedString = formatString;

        foreach (var property in typeof(PaymentRequest).GetProperties())
        {
            string matchString = "{" + property.Name + "}";

            if (substitutedString.Contains(matchString))
            {
                string propValue = property.GetValue(paymentRequest, null)?.ToString() ?? string.Empty;
                substitutedString = substitutedString.Replace(matchString, propValue, StringComparison.InvariantCultureIgnoreCase);
            }
        }

        return substitutedString;
    }
}
