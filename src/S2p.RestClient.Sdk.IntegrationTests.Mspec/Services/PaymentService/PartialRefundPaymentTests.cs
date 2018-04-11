﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;
using S2p.RestClient.Sdk.Entities;
using S2p.RestClient.Sdk.Infrastructure;
using S2p.RestClient.Sdk.Services;

namespace S2p.RestClient.Sdk.IntegrationTests.Mspec.Services.PaymentService
{
    partial class PaymentServiceTests
    {
        [Subject(typeof(Sdk.Services.PaymentService))]
        public class When_a_native__partial_refund_is_performend_for_a_payment
        {

            private static RefundService RefundService { get; set; }
            private static ApiRefundRequest RefundRequest { get; set; }
            private static ApiResult<ApiRefundResponse> RefundApiResult { get; set; }

            private Establish context = () => {
                InitializeHttpBuilder();
                HttpClient = HttpClientBuilder.Build();
                PaymentService = new Sdk.Services.PaymentService(HttpClient, BaseAddress);
                RefundService = new Sdk.Services.RefundService(HttpClient, BaseAddress);
                PaymentRequest = new ApiPaymentRequest
                {
                    Payment = new PaymentRequest
                    {
                        Amount = 1960,
                        Currency = "DKK",
                        Description = "test capture SDK",
                        MethodID = 75,
                        ReturnURL = "http://demo.smart2pay.com/redirect.php",
                        MerchantTransactionID = MerchantTransactionID,
                        Articles = new List<Article> { new Article{
                        MerchantArticleID = "1231",
                        Name = "TEST",
                        Quantity = 2,
                        Price = 1000,
                        VAT = 1000,
                        Discount = 200,
                        Type = ArticleType.Product
                    }},
                        BillingAddress = new Address()
                        {
                            HouseNumber = "",
                            Street = "Seffleberggate 56,1 mf",
                            ZipCode = "6800",
                            City = "Varde",
                            Country = "DK"
                        },
                        ShippingAddress = new Address
                        {
                            HouseNumber = "",
                            Street = "Seffleberggate 56,1 mf",
                            ZipCode = "6800",
                            City = "Varde",
                            Country = "DK"
                        },
                        Customer = new Customer
                        {
                            FirstName = "Testperson-dk",
                            LastName = "Approved",
                            Gender = "1",
                            Email = "youremail@email.com",
                            Phone = "20123456",
                            SocialSecurityNumber = "0801363945"
                        },
                        TokenLifetime = 10
                    }
                };

                RefundRequest = new ApiRefundRequest
                {
                    Refund = new RefundRequest()
                    {
                        Amount = 980,
                        MerchantTransactionID = MerchantTransactionID,
                        Articles = new List<Article>
                        { new Article()
                            {
                                MerchantArticleID = PaymentRequest.Payment.Articles[0].MerchantArticleID,
                                Quantity = 1
                            }   
                        }

                    }
             };

            };

            private Because of = () => {
                RefundApiResult = BecauseAsync().GetAwaiter().GetResult();
            };

            private static async Task<ApiResult<ApiRefundResponse>> BecauseAsync()
            {
                var createPaymentResult = await PaymentService.CreatePaymentAsync(PaymentRequest);
                await Task.Delay(2000);
                var capturedPaymentResult = await PaymentService.CapturePaymentAsync(createPaymentResult.Value.Payment.ID.ToString());
                await Task.Delay(2000);
                return await RefundService.CreateRefundAsync(capturedPaymentResult.Value.Payment.ID.ToString(), RefundRequest);
            }

            private Cleanup after = () => { HttpClient.Dispose(); };

            private It should_have_created_status_code = () => {
                RefundApiResult.HttpResponse.StatusCode.ShouldEqual(HttpStatusCode.Created);
            };

            private It should_have_the_same_merchant_transaction_id = () =>
            {
                RefundApiResult.Value.Refund.MerchantTransactionID.ShouldEqual(RefundRequest.Refund.MerchantTransactionID);
            };

            private It should_have_the_correct_amount = () =>
            {
                RefundApiResult.Value.Refund.Amount.ShouldEqual(RefundRequest.Refund.Amount);
            };

            private It should_have_the_same_currency = () =>
            {
                RefundApiResult.Value.Refund.Currency.ShouldEqual(PaymentRequest.Payment.Currency);
            };

            private It should_have_the_correct_status_id = () =>
            {
                RefundApiResult.Value.Refund.Status.ID.ShouldEqual(PaymentStatusDefinition.Open);
            };

            private It should_have_the_correct_status_info = () =>
            {
                RefundApiResult.Value.Refund.Status.Info.ShouldEqual(nameof(PaymentStatusDefinition.Open));
            };

            private It should_have_correct_merchant_article_id = () => {
                RefundApiResult.Value.Refund.Articles[0].MerchantArticleID
                    .ShouldEqual(PaymentRequest.Payment.Articles[0].MerchantArticleID);
            };
        }
    }
}

