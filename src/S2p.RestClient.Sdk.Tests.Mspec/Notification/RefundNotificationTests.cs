﻿using System.Net;
using Machine.Specifications;
using S2p.RestClient.Sdk.Entities;
using S2p.RestClient.Sdk.Notifications;

namespace S2p.RestClient.Sdk.Tests.Mspec.Notification
{
    public partial class NotificationTests
    {
        public static INotificationProcessor NotificationProcessor;
        public static HttpStatusCode Response;

        [Subject(typeof(NotificationProcessor))]
        public class When_a_refund_notification_arrives
        {
            private static string NotificationBody;
            private static ApiRefundResponse Notification;

            private Establish context = () => {
                NotificationBody = "{" +
                                   "  \"Refund\": {" +
                                   "    \"ID\": 16405," +
                                   "    \"Created\": \"20170803095139\"," +
                                   "    \"MerchantTransactionID\": \"s2ptest_g28\"," +
                                   "    \"OriginatorTransactionID\": null," +
                                   "    \"InitialPaymentID\": 3005389," +
                                   "    \"Amount\": 100," +
                                   "    \"Currency\": \"EUR\"," +
                                   "    \"Description\": \"\"," +
                                   "    \"TypeID\": 5," +
                                   "    \"SiteID\": 30201," +
                                   "    \"Details\": null," +
                                   "    \"Customer\": null," +
                                   "    \"BillingAddress\": null," +
                                   "    \"BankAddress\": null," +
                                   "    \"Articles\": null," +
                                   "    \"Status\": {" +
                                   "      \"ID\": 2," +
                                   "      \"Info\": \"Success\"," +
                                   "      \"Reasons\": null" +
                                   "    }" +
                                   "  }" +
                                   "}";
                NotificationProcessor = new NotificationProcessor();
                NotificationProcessor.RefundNotificationEvent += (sender, response) => {
                    Notification = response;
                };
            };

            private Because of = () => {
                Response = NotificationProcessor.ProcessNotificationBody(NotificationBody);
            };

            private It should_have_no_content_response = () => {
                Response.ShouldEqual(HttpStatusCode.NoContent);
            };

            private It should_have_correct_notification_id = () => {
                Notification.Refund.ID.ShouldEqual(16405);
            };
        }
    }
}
