﻿using System.Linq;
using System.Net;
using Machine.Specifications;
using S2p.RestClient.Sdk.Entities;
using S2p.RestClient.Sdk.Infrastructure;

namespace S2p.RestClient.Sdk.IntegrationTests.Mspec.Services.AlternativePaymentService
{
    partial class PaymentServiceTests
    {
        [Subject(typeof(Sdk.Services.AlternativePaymentService))]
        public class When_requesting_payment_list
        {
            private static ApiResult<ApiAlternativePaymentListResponse> ApiListResult;
            private static AlternativePaymentsFilter PaymentsFilter;
            protected static PaymentFilteredBehaviourData Data;
            private const int Limit = 10;


            private Establish context = () => {
                InitializeHttpBuilder();
                HttpClient = HttpClientBuilder.Build();
                _alternativePaymentService = new Sdk.Services.AlternativePaymentService(HttpClient, BaseAddress);
                PaymentsFilter = new AlternativePaymentsFilter { };
            };

            private Because of = () => {
                ApiListResult = PaymentServiceTests._alternativePaymentService.GetPaymentListAsync().GetAwaiter().GetResult();
                Data = new PaymentFilteredBehaviourData
                {
                    ApiListResult = ApiListResult,
                    Limit = Limit,
                    PaymentsFilter = PaymentsFilter
                };
            };

            private Cleanup after = () => { HttpClient.Dispose(); };

            private It should_have_non_empty_payments_list = () => { ApiListResult.Value.Payments.Count.ShouldBeGreaterThan(0); };

            Behaves_like<PaymentFilteredBehavior> a_list_of_filtered_payments_response;

        }

        [Behaviors]
        public class PaymentFilteredBehavior
        {
            protected static PaymentFilteredBehaviourData Data;

            private It should_be_successful = () => { Data.ApiListResult.IsSuccess.ShouldBeTrue(); };

            private It should_have_ok_http_status = () => {
                Data.ApiListResult.HttpResponse.StatusCode.ShouldEqual(HttpStatusCode.OK);
            };

            private It should_have_correct_number_of_payments_in_list = () => { Data.ApiListResult.Value.Payments.Count.ShouldEqual(Data.Limit); };

            private It should_have_correct_site_id = () => {
                Data.ApiListResult.Value.Payments.Count(p => p.SiteID == null ||
                                                        p.SiteID.Value != ServiceTestsConstants
                                                            .PaymentSystemAuthenticationConfiguration.SiteId)
                    .ShouldEqual(0);
            };

            private It should_have_not_null_payment_ids = () => {
                Data.ApiListResult.Value.Payments.Count(p => p.ID == null || p.ID <= 0).ShouldEqual(0);
            };

            private It should_have_not_null_merchant_transaction_ids = () => {
                Data.ApiListResult.Value.Payments.Count(p => string.IsNullOrWhiteSpace(p.MerchantTransactionID)).ShouldEqual(0);
            };

            private It should_have_not_null_amounts = () => {
                Data.ApiListResult.Value.Payments.Count(p => p.Amount == null || p.Amount <= 0).ShouldEqual(0);
            };

            private It should_have_not_null_currency = () => {
                Data.ApiListResult.Value.Payments.Count(p => string.IsNullOrWhiteSpace(p.Currency)).ShouldEqual(0);
            };

            private It should_have_valid_total_pages = () => {
                Data.ApiListResult.Value.TotalPages.ShouldBeGreaterThan(0);
            };

            private It should_have_valid_count = () => { Data.ApiListResult.Value.Count.ShouldBeGreaterThan(0); };

            private It should_have_valid_total_count = () => {
                Data.ApiListResult.Value.TotalCount.ShouldBeGreaterThan(0);
            };

            private It should_have_valid_page_index = () => {
                Data.ApiListResult.Value.PageIndex.ShouldBeGreaterThan(0);
            };

            private It should_have_valid_page_size = () => {
                Data.ApiListResult.Value.PageSize.ShouldBeGreaterThan(0);
            };
        }

        public class PaymentFilteredBehaviourData
        {
            public ApiResult<ApiAlternativePaymentListResponse> ApiListResult { get; set; }
            public int Limit { get; set; }
            public AlternativePaymentsFilter PaymentsFilter { get; set; }

        }
    }
}
