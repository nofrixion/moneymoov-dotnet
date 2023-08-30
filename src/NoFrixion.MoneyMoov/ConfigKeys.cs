//-----------------------------------------------------------------------------
// Filename: ConfigKeys.cs
//
// Description: List of the configuration keys that are used by the application.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 17 Sep 2021  Aaron Clauson   Created, Carmichael House, Dublin, Ireland.
// 19 Sep 2022  Aaron Clauson   Moved from NoFrixion.Core to NoFrixion.Common.
//
// License: 
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

/// <summary>
/// Convenience class to hold the keys that are used to get configuration settings from
/// the appSettings files and elsewhere.
/// </summary>
public class ConfigKeys
{
    /// <summary>
    /// The IdentityServer Audience, which identifies the NoFrixion API that users will be calling,
    /// to grant authorised user access to.
    /// </summary>
    public const string IDENTITY_SERVER_AUDIENCE = "Auth0:Audience";

    /// <summary>
    /// The IdentityServer Audience for calls to APIs that require Strong Customer Authentication (SCA).
    /// </summary>
    public const string IDENTITY_SERVER_AUDIENCE_STRONG = "Auth0:AudienceStrong";

    /// <summary>
    /// The IdentityServer Audience for issuing tokens suitable for use in autonomous applications (machine-only apps,
    /// e,g, cron jobs).
    /// </summary>
    public const string IDENTITY_SERVER_AUDIENCE_MACHINE = "Auth0:AudienceMachine";

    /// <summary>
    /// Connection string for the MoneyMoov SQL database.
    /// </summary>
    public const string CONNECTION_STRING_MONEYMOOVDB = "ConnectionStrings:MoneyMoovDBConnStr";

    /// <summary>
    /// Connection string for the Redis distributed cache used by the portal web application.
    /// </summary>
    public const string CONNECTION_STRING_PORTAL_REDIS_CACHE = "ConnectionStrings:PortalRedis";

    /// <summary>
    /// Queue storage connection string.
    /// </summary>
    public const string CONNECTION_STRING_QUEUE_STORAGE = "ConnectionStrings:QueueStorageConnectionString";

    /// <summary>
    /// Service bus connection string.
    /// </summary>
    public const string CONNECTION_STRING_SERVICE_BUS = "ConnectionStrings:ServiceBusConnectionString";

    /// <summary>
    /// Configuration key for storing Checkout.com API secrets. The format of the key used to store
    /// the secrets is "CheckoutApiSecrets:{KeyName}", for example 
    /// "CheckoutApiSecrets:nofrixiondev".
    /// </summary>
    public const string CHECKOUT_API_SECRETS_KEY = "CheckoutApiSecrets";

    /// Configuration key for storing Apple Pay merchant ID.
    /// A merchant ID identifies us to Apple Pay as being able to accept payments.
    /// An example value is "merchant.nofrixion.sandbox".
    public const string APPLE_MERCHANT_ID_KEY = "Apple:MerchantId";
    
    /// Configuration key for storing Apple Pay merchant identity domain association filename.
    /// This file is the one that Apple gives us to upload to our web server to prove that we own.
    /// The file will be named "apple-developer-merchantid-domain-association.txt", where we need to
    /// change it for "apple-developer-merchantid-domain-association.{environment}.txt" where
    /// the environment is "dev", "sandbox" or "production".
    public const string APPLE_MERCHANT_IDENTITY_DOMAIN_ASSOCIATION_FILENAME = "Apple:MerchantIdentityDomainAssociationFilename";

    /// Configuration key for storing Apple Pay merchant identity certificate filename.
    /// This file is used to authenticate our sessions with the Apple Pay servers
    public const string APPLE_MERCHANT_IDENTITY_CERTIFICATE_FILENAME = "Apple:MerchantIdentityCertificateFilename";

    /// Configuration key for storing Apple Pay merchant identity certificate password.
    /// This certificate is only being load in Prod where it's loaded from a Key Vault in Azure.
    public const string APPLE_MERCHANT_IDENTITY_CERTIFICATE = "Apple:MerchantIdentityCertificate";
    
    /// <summary>
    /// Configuration key for storing CyberSource API secrets. The format of the key used to store
    /// the secrets is "CyberSourceApiSecrets:{KeyName}", for example 
    /// "CyberSourceApiSecrets:nfx100002".
    /// </summary>
    public const string CYBERSOURCE_API_SECRETS_KEY = "CyberSourceApiSecrets";

    /// <summary>
    /// Configuration file element to hold CyberSource API sandbox public key. This must not
    /// be set in production.
    /// </summary>
    public const string CYBERSOURCE_SANDBOX_PUBLICKEY = "CyberSourceSandbox:PublicKey";

    /// <summary>
    /// Configuration file element to hold CyberSource API sandbox secret key. This must not
    /// be set in production.
    /// </summary>
    public const string CYBERSOURCE_SANDBOX_SECRETKEY = "CyberSourceSandbox:SecretKey";

    /// <summary>
    /// Configuration file element to hold CyberSource API sandbox merchant ID. This must not
    /// be set in production.
    /// </summary>
    public const string CYBERSOURCE_SANDBOX_MERCHANTID = "CyberSourceSandbox:MerchantID";

    /// <summary>
    /// Configuration file element to hold Checkout API sandbox public key setting. This must not
    /// be set in production.
    /// </summary>
    public const string CHECKOUT_SANDBOX_PUBLICKEY = "CheckoutSandbox:PublicKey";

    /// <summary>
    /// Configuration file element to hold Checkout API sandbox secret key setting. This must not
    /// be set in production.
    /// </summary>
    public const string CHECKOUT_SANDBOX_SECRETKEY = "CheckoutSandbox:SecretKey";

    /// <summary>
    /// The Modulr API Key
    /// </summary>
    public const string MODULR_API_KEY = "Modulr:ApiKey";

    /// <summary>
    /// The Modulr HMAC Secret
    /// </summary>
    public const string MODULR_HMAC_SECRET = "Modulr:Hmac";

    /// <summary>
    /// The Modulr URL base
    /// </summary>
    public const string MODULR_URL_BASE = "Modulr:UrlBase";

    /// <summary>
    /// The Azure Active Directory Client ID for the Compliance application.
    /// The Client ID will differ in the different resource groups so its important
    /// that the correct client ID is matched to prevent developer tokens accessing
    /// production.
    /// </summary>
    public const string NOFRIXION_COMPLIANCE_APP_CLIENT_ID = "NoFrixion:ComplianceAppClientId";

    /// <summary>
    /// The URL for CybserSource REST API requests.
    /// Test: apitest.cybersource.com
    /// Production: api.cybersource.com
    /// </summary>
    public const string NOFRIXION_CYBERSOURCE_API_URL_KEY = "NoFrixion:CyberSourceApiUrl";

    /// <summary>
    /// The timeout, in seconds, for CyberSource HTTP requests.
    /// </summary>
    public const string NOFRIXION_CYBERSOURCE_TIMEOUT_SECONDS_KEY = "NoFrixion:CyberSourceTimeoutSeconds";

    /// <summary>
    /// The timeout, in seconds, for use for services that use HTTP clients. Example 
    /// services are Modulr and Plaid. By setting a consistent timeout it
    /// can help deal with situations where a supplier service may fail and a NoFrixion client
    /// is left waiting for too long.
    /// </summary>
    public const string NOFRIXION_HTTP_CLIENT_TIMEOUT_SECONDS_KEY = "NoFrixion:HttpClientTimeoutSeconds";

    /// <summary>
    /// The URL for the NoFrixion MoneyMoov host. The production value is
    /// https://api.nofrixion.com.
    /// </summary>
    public const string NOFRIXION_MONEYMOOV_BASE_URL = "NoFrixion:MoneyMoovBaseUrl";

    /// <summary>
    /// The URL to access the main NoFrixion MoneyMoov API. The production value is
    /// https://api.nofrixion.com/api/v1.
    /// </summary>
    public const string NOFRIXION_MONEYMOOV_API_BASE_URL = "NoFrixion:MoneyMoovApiBaseUrl";

    /// <summary>
    /// The API scope set in Azure Active Directory that the MoneyMoov API will accept 
    /// JWT tokens for.
    /// </summary>
    public const string NOFRIXION_MONEYMOOV_API_SCOPE = "NoFrixion:MoneyMoovApiScope";

    /// <summary>
    /// A friendly name for the major release version.
    /// </summary>
    public const string NOFRIXION_MONEYMOOV_RELEASE_NAME = "NoFrixion:ReleaseName";

    /// <summary>
    /// A configuration setting to indicate whether the application is operating in sandbox mode
    /// or not.
    /// </summary>
    public const string NOFRIXION_ISSANDBOX_KEY = "NoFrixion:IsSandbox";

    /// <summary>
    /// A configuration setting to indicate whether the application is operating in development mode
    /// or not. Development mode is a special case of Sandbox mode. To be safe if in development
    /// mode sandbox mode should also be true.
    /// </summary>
    public const string NOFRIXION_ISDEVELOPMENT_KEY = "NoFrixion:IsDevelopment";

    /// <summary>
    /// A development only setting to allow the pay element path to be set to a custom location.
    /// If not set, or the API is not running in development mode, the pay element path defaults
    /// to the NoFrixion CDN at https://cdn.nofrixion.com/nofrixion.js.
    /// </summary>
    public const string NOFRIXION_PAYELEMENT_DEVPATH = "NoFrixion:PayElementDevPath";

    /// <summary>
    /// A development only setting to allow the new pay element (using React under the hood) path to be set to a custom location.
    /// If not set, or the API is not running in development mode, the pay element path defaults
    /// to the NoFrixion CDN at https://cdn.nofrixion.com/nofrixion-nextgen.js.
    /// </summary>
    public const string NOFRIXION_NEXTGEN_PAYELEMENT_DEVPATH = "NoFrixion:NextgenPayElementDevPath";

    /// <summary>
    /// A development only setting to set the location of the hosted pay web component javascript file.
    /// If not set, or the API is not running in development mode, the path defaults
    /// to the NoFrixion CDN at https://cdn.nofrixion.com/nofrixion-hosted.js.
    /// </summary>
    public const string NOFRIXION_HOSTEDPAY_DEVPATH = "NoFrixion:HostedPayDevPath";

    /// <summary>
    /// The base URL the portal is operating on. On Azure the portal won't know its own
    /// URL as it gets set on the application gateway, not the app service.
    /// </summary>
    public const string NOFRIXION_PORTAL_BASE_URL_KEY = "NoFrixion:PortalBaseUrl";
    
    /// <summary>
    /// The Barclays Solution Partner ID value that needs to be set when submitting card payments
    /// to the CyberSource (Smartpay Fuse) gateway.
    /// </summary>
    public const string BARCLAYS_PARTNER_ID = "Barclays:PartnerID";

    /// <summary>
    /// The key used to sign api tokens.
    /// </summary>
    public const string NOFRIXION_API_TOKEN_KEY = "NoFrixion:ApiTokenKey";

    /// <summary>
    /// NoFrixion Identity domain
    /// </summary>
    public const string NOFRIXION_IDENTITY_DOMAIN = "NoFrixion:NoFrixionIdentityDomain";

    /// <summary>
    /// The NoFrixion Identity Client ID.
    /// </summary>
    public const string NOFRIXION_IDENTITY_CLIENTID = "NoFrixion:ClientId";

    /// <summary>
    /// The NoFrixion Identity Client Secret.
    /// </summary>
    public const string NOFRIXION_IDENTITY_CLIENTSECRET = "NoFrixion:ClientSecret";

    /// The maximum time the authentication cookie can be valid for.
    /// Note: This should match the access and identity token expiries on the
    /// Identity Server Client.
    public const string NOFRIXION_AUTHENTICATION_COOKIE_EXPIRY_SECONDS = "NoFrixion:AuthenticationCookieExpirySeconds";

    /// <summary>
    /// NoFrixion's official name and address for use in printed statements.
    /// </summary>
    public const string NOFRIXION_STATEMENT_COMPANY_DETAILS = "NoFrixion:StatementCompanyDetails";

    /// <summary>
    /// Cron timer expression for pending payout status check.
    /// </summary>
    public const string NOFRIXION_PENDING_PAYOUT_CRON_SCHEDULE_EXPRESSION = "NoFrixion:PendingPayoutsScheduleExpression";

    /// <summary>
    /// NoFrixion's merchant notification failure days limit before merchant is notified.
    /// </summary>
    public const string NOFRIXION_MERCHANT_NOTIFICATION_FAILURE_DAYS = "NoFrixion:MerchantNotificationFailureDays";

    /// <summary>
    /// NoFrixion's merchant notification disabled days limit before merchant is notified.
    /// </summary>
    public const string NOFRIXION_MERCHANT_NOTIFICATION_DISABLED_DAYS = "NoFrixion:MerchantNotificationDisabledDays";

    /// <summary>
    /// HTTP requests to these endpoints will not be logged to the analytics service.
    /// </summary>
    public const string NOFRIXION_ANALYTICS_IGNORED_ENDPOINTS = "NoFrixion:Analytics:IgnoredEndpoints";

    /// <summary>
    /// A configuration setting to indicate whether the new payment requests dashboard is enabled or not.
    /// </summary>
    public const string NOFRIXION_NEW_PAYMENT_REQUEST_DASHBOARD_ENABLED = "NoFrixion:NewPaymentRequestsDashboardEnabled";

    /// <summary>
    /// Site key for google reCAPTCHA.
    /// </summary>
    public const string RECAPTCHA_SITE_KEY = "Recaptcha:SiteKey";

    /// <summary>
    /// Secret key for google reCAPTCHA.
    /// </summary>
    public const string RECAPTCHA_SECRET_KEY = "Recaptcha:Secret";

    /// <summary>
    /// The base URL for the Mailgun API used for sending email notifications.
    /// </summary>
    public const string MAILGUN_API_BASE_URL = "Mailgun:BaseUrl";

    /// <summary>
    /// The API key to authenticate Mailgun API calls.
    /// </summary>
    public const string MAILGUN_API_KEY = "Mailgun:ApiKey";

    /// <summary>
    /// The from address to used when sending emails via the Mailgun API.
    /// </summary>
    public const string MAILGUN_SEND_FROM_ADDRESS = "Mailgun:SendFromAddress";

    /// <summary>
    /// HTML email template for inviting users to MoneyMoov via the Mailgun API.
    /// </summary>
    public const string MAILGUN_INVITE_USER_EMAIL_TEMPLATE = "Mailgun:InviteUserEmailTemplate";

    /// <summary>
    /// HTML email template for inviting newly onboarded customer to MoneyMoov through compliance portal via the Mailgun API.
    /// </summary>
    public const string MAILGUN_INVITE_USER_FROM_COMPLIANCE_EMAIL_TEMPLATE = "Mailgun:InviteUserFromComplianceEmailTemplate";

    /// <summary>
    /// HTML email template for admins to invite users to MoneyMoov via the Mailgun API.
    /// </summary>
    public const string MAILGUN_INVITE_USER_BY_ADMIN_EMAIL_TEMPLATE = "Mailgun:InviteUserByAdminEmailTemplate";

    /// <summary>
    /// HTML email template for sending payment notifications via the Mailgun API.
    /// </summary>
    public const string MAILGUN_PAYMENT_NOTIFICATION_EMAIL_TEMPLATE = "Mailgun:PaymentNotificationEmailTemplate";

    /// <summary>
    /// HTML email template for sending email to inviter when a user accepts the invite via the Mailgun API.
    /// </summary>
    public const string MAILGUN_USER_INVITE_ACCEPT_TEMPLATE = "Mailgun:UserInviteAcceptTemplate";

    /// <summary>
    /// HTML email template for sending email to invitee when admin assigns user role to
    /// invitee via the Mailgun API.
    /// </summary>
    public const string MAILGUN_USER_ASSIGN_USER_ROLE_TEMPLATE = "Mailgun:UserAssignedUserRoleTemplate";

    /// <summary>
    /// HTML email template for sending email to inviter when invitee requests a new invite.
    /// </summary>
    public const string MAILGUN_REQUEST_NEW_INVITE_TEMPLATE = "Mailgun:RequestNewInviteTemplate";

    /// <summary>
    /// HTML email template for sending payment request status updates.
    /// </summary>
    public const string MAILGUN_PAYMENT_REQUEST_STATUS_UPDATE_TEMPLATE = "Mailgun:PaymentRequestStatusUpdate";

    /// <summary>
    /// HTML email template for sending email to notify merchant that merchant notifications 
    /// has been failing for over a day.
    /// </summary>
    public const string MAILGUN_MERCHANT_NOTIFICATION_FAILURE_TEMPLATE = "Mailgun:MerchantNotificationFailureTemplate";

    /// <summary>
    /// HTML email template for sending email to notify merchant that merchant notifications has been disabled
    /// and been failing for over 5 days.
    /// </summary>
    public const string MAILGUN_MERCHANT_NOTIFICATION_DISABLED_TEMPLATE = "Mailgun:MerchantNotificationDisabledTemplate";

    /// <summary>
    /// HTML email template for sending email to notify merchant that a payment request has expired.
    /// </summary>
    public const string MAILGUN_EXPIRED_PAYMENT_REQUEST_TEMPLATE = "Mailgun:ExpiredPaymentRequestTemplate";

    /// <summary>
    /// HTML email template for sending rule status updates.
    /// </summary>
    public const string MAILGUN_RULE_STATUS_UPDATE_TEMPLATE = "Mailgun:RuleStatusUpdate";

    /// <summary>
    /// HTML email template for sending rule delete notification.
    /// </summary>
    public const string MAILGUN_RULE_DELETE_TEMPLATE = "Mailgun:RuleDelete";

    /// <summary>
    /// The destination address to send email notifications when a new user
    /// registers on the portal.
    /// </summary>
    public const string NOFRIXION_NEWACCOUNT_NOTIFY_EMAIL = "NoFrixion:NewAccountNotifyEmail";

    /// <summary>
    /// The number of times a failed merchant notification is retried.
    /// </summary>
    public const string NOFRIXION_FAILED_NOTIFICATIONS_RETRY_COUNT = "NoFrixion:FailedNotificationRetryCount";

    /// <summary>
    /// The route for the notification callback route
    /// </summary>
    public const string NOFRIXION_NOTIFICATION_CALLBACK_URL = "NoFrixion:NotificationCallbackUrl";

    /// <summary>
    /// The secret for the notification callback from Modulr
    /// </summary>
    public const string NOFRIXION_NOTIFICATION_SECRET = "NoFrixion:NotificationSecret";

    /// <summary>
    /// The cron schedule for the Synchronise Accounts Schedule
    /// </summary>
    public const string NOFRIXION_TRANSACTION_MONITOR_SCHEDULE = "NoFrixion:TransactionMonitorSchedule";

    /// <summary>
    /// The URL to specify for calling the payment request APIs. The initial use for this was
    /// the payment element calling the submit payment APIs.
    /// </summary>
    public const string NOFRIXION_PAYMENTREQUEST_URL = "NoFrixion:PaymentRequestUrl";

    /// <summary>
    /// THe base URL the Bitcoin worker is listening on.
    /// </summary>
    public const string NOFRIXION_BITCOIN_WORKER_API_BASE_URL = "NoFrixion:BitcoinWorkerApiBaseUrl";

    /// <summary>
    /// The Yapily application ID
    /// </summary>
    public const string YAPILY_APPLICATION_UUID = "Yapily:ApplicationUuid";

    /// <summary>
    /// The Yapily Client Secret
    /// </summary>
    public const string YAPILY_CLIENT_SECRET = "Yapily:ClientSecret";

    /// <summary>
    /// The base URL for Yapily
    /// </summary>
    public const string YAPILY_BASE_URL = "Yapily:BaseUrl";

    /// <summary>
    /// The application user id used by Yapily. 
    /// </summary>
    public const string YAPILY_APPLICATION_USER_ID = "Yapily:ApplicationUserId";

    /// <summary>
    /// The user for which the authorisation request was created by Yapily. 
    /// </summary>
    public const string YAPILY_USER_UUID = "Yapily:UserUuid";

    /// <summary>
    /// Used to instruct the pay element to transmit raw details or use iframes and encryption.
    /// </summary>
    public const string NOFRIXION_CARD_TRANSMIT_RAW = "NoFrixion:CardTransmitRaw";

    /// <summary>
    /// Specifies the default lifetime (in seconds) for cached statements.
    /// </summary>
    public const string NOFRIXION_STATEMENT_CACHE_DEFAULT_LIFETIME_SECONDS = "NoFrixion:StatementCacheDefaultLifetimeSeconds";

    /// <summary>
    /// RabbitMQ host name
    /// </summary>
    public const string RABBITMQ_HOST_NAME = "RabbitMQ:HostName";

    /// <summary>
    /// RabbitMQ host name
    /// </summary>
    public const string RABBITMQ_VIRTUAL_HOST = "RabbitMQ:VirtualHost";

    /// <summary>
    /// RabbitMQ host name
    /// </summary>
    public const string RABBITMQ_ANALYTICS_VIRTUAL_HOST = "RabbitMQ:AnalyticsVirtualHost";

    /// <summary>
    /// RabbitMQ port number
    /// </summary>
    public const string RABBITMQ_PORT_NUMBER = "RabbitMQ:PortNumber";

    /// <summary>
    /// RabbitMQ user name
    /// </summary>
    public const string RABBITMQ_USER_NAME = "RabbitMQ:UserName";

    /// <summary>
    /// RabbitMQ password
    /// </summary>
    public const string RABBITMQ_PASSWORD = "RabbitMQ:Password";

    /// <summary>
    /// Url for the Seq logging instance being used. Typically only specified as an environment
    /// variable, not an app setting. 
    /// </summary>
    public const string SEQ_URL = "Seq_Url";

    /// <summary>
    /// Api key for the Seq logging instance being used. Typically only specified as an environment
    /// variable, not an app setting. 
    /// </summary>
    public const string SEQ_APIKEY = "Seq_ApiKey";

    /// <summary>
    /// License key for the IronPDF library.
    /// </summary>
    public const string IRONPDF_LICENSE_KEY = "IronPDF:LicenseKey";
    
    /// <summary>
    /// Name of the config file section used to hold the RavenDB settings.
    /// </summary>
    public const string RAVENDB_SECTION_KEY = "RavenDB";

    /// <summary>
    /// Name of the config file section used to hold the RavenDB bookkeeping database settings.
    /// </summary>
    public const string RAVENDB_BOOKKEEPING_SECTION_KEY = "RavenDBBookkeeping";

    /// <summary>
    /// Name of key for config setting that indicates whether Banking Circle is being used in sandbox mode or not.
    /// </summary>
    public const string BANKING_CIRCLE_IS_SANDBOX_KEY = "BankingCircle:IsSandbox";

    /// <summary>
    /// Name of key for config setting for the certificate path that needs to be used with the HTTP client communicating
    /// with the Banking Circle server. Only one of certificate path or base64 certificate is needed.
    /// </summary>
    public const string BANKING_CIRCLE_CERTIFICATE_PATH_KEY = "BankingCircle:CertificatePath";

    /// <summary>
    /// Name of key for config setting for a base 64 encoded certificate that needs to be used with the HTTP client 
    /// communicating with the Banking Circle server. Only one of certificate path or base64 certificate is needed.
    /// </summary>
    public const string BANKING_CIRCLE_CERTIFICATE_BASE64_KEY = "BankingCircle:CertificateBase64";

    /// <summary>
    /// Name of key for config setting that contains the password for the Banking Circle X509 certificate.
    /// </summary>
    public const string BANKING_CIRCLE_CERTIFICATE_PASSWORD_KEY = "BankingCircle:CertificatePassword";

    /// <summary>
    /// Name of key for config setting that holds the username for the Banking Circle token authorisation endpoint.
    /// </summary>
    public const string BANKING_CIRCLE_USERNAME_KEY = "BankingCircle:Username";

    /// <summary>
    /// Name of key for config setting that holds the password for the Banking Circle token authorisation endpoint.
    /// </summary>
    public const string BANKING_CIRCLE_PASSWORD_KEY = "BankingCircle:Password";

    /// <summary>
    /// Name of key for config setting that holds the company number assgined to NoFrixion by Banking Circle.
    /// </summary>
    public const string BANKING_CIRCLE_COMPANY_NUMBER_KEY = "BankingCircle:CompanyNumber";

    /// <summary>
    /// Name of key for config setting that holds NoFrixion's standard (non-agency) banking "physical" account ID.
    /// "Physical" refers to the fact that it's NoFrixion's settlement account with Banking Circle as opposed to
    /// a NoFrixion's customer account, which Banking Circle consider a "virtual" account.
    /// </summary>
    public const string BANKING_CIRCLE_STANDARD_ACCOUNTID_KEY = "BankingCircle:StandardPhysicalAccountID";

    /// <summary>
    /// Name of key for config setting that holds the company number assgied to NoFrixion by Banking Circle.
    /// </summary>
    public const string BANKING_CIRCLE_AGENCY_ACCOUNTID_KEY = "BankingCircle:AgencyPhysicalAccountID";
    
    /// <summary>
    /// Name of key for config setting that holds NoFrixion's standard (non-agency) banking "physical" IBAN.
    /// "Physical" refers to the fact that it's NoFrixion's settlement account with Banking Circle as opposed to
    /// a NoFrixion's customer account, which Banking Circle consider a "virtual" account.
    /// </summary>
    public const string BANKING_CIRCLE_STANDARD_ACCOUNT_IBAN = "BankingCircle:StandardPhysicalAccountIBAN";
    
    /// <summary>
    /// Name of key for config setting that holds NoFrixion's Agency banking "physical" IBAN.
    /// "Physical" refers to the fact that it's NoFrixion's settlement account with Banking Circle as opposed to
    /// a NoFrixion's customer account, which Banking Circle consider a "virtual" account.
    /// </summary>
    public const string BANKING_CIRCLE_AGENCY_ACCOUNT_IBAN = "BankingCircle:AgencyPhysicalAccountIBAN";
    
    /// <summary>
    /// An encryption key of 32 characters, compatible with the 256-bit key required by the AES-256-GCM encryption algorithm
    /// </summary>
    public const string BANKING_CIRCLE_NOTIFICATION_ENCRYPTION_KEY = "BankingCircle:NotificationEncryptionKey";

    /// <summary>
    /// The cron schedule to synchronise transactions with Banking Circle.
    /// </summary>
    public const string BANKING_CIRCLE_TRANSACTION_SYNCHRONISATION_SCHEDULE = "BankingCircle:TransactionSynchronisationSchedule";
    
    /// <summary>
    /// A configuration setting to indicate whether the new current accounts is enabled or not.
    /// </summary>
    public const string NOFRIXION_NEW_CURRENT_ACCOUNTS_ENABLED = "NoFrixion:NewCurrentAccountsEnabled";
}
