﻿using S2p.RestClient.Sdk.Infrastructure.Extensions;
using S2p.RestClient.Sdk.Infrastructure.Helper;
using S2p.RestClient.Sdk.Validation;
using System.Globalization;
using System.Text.RegularExpressions;

namespace S2p.RestClient.Sdk.Entities.Validators
{
    public class AlternativePaymentRequestValidator : AbstractValidator<AlternativePaymentRequest>
    {
        public AlternativePaymentRequestValidator()
        {
            var addressValidator = new AddressValidator();
            var customerValidator = new CustomerValidator();
            var articleValidator = new ArticleValidator();

            AddRuleFor(x => x.ID)
                .WithPredicate(x =>
                    Regex.IsMatch(x.ID.ToString(CultureInfo.InvariantCulture), ValidationRegexConstants.ID))
                .WithErrorMessage(
                    Operator.InvalidPropertyMessage<AlternativePaymentRequest>(x => x.ID, ValidationRegexConstants.ID));
            AddRuleFor(x => x.SkinID)
                .WithPredicate(x =>
                    x.SkinID == null || Regex.IsMatch(x.SkinID.Value.ToString(CultureInfo.InvariantCulture),
                        ValidationRegexConstants.SkinID))
                .WithErrorMessage(
                    Operator.InvalidPropertyMessage<AlternativePaymentRequest>(x => x.SkinID,
                        ValidationRegexConstants.SkinID));
            AddRuleFor(x => x.MerchantTransactionID)
                .WithPredicate(x =>
                    !string.IsNullOrWhiteSpace(x.MerchantTransactionID) && Regex.IsMatch(x.MerchantTransactionID,
                        ValidationRegexConstants.MerchantTransactionID))
                .WithErrorMessage(Operator.InvalidPropertyMessage<AlternativePaymentRequest>(
                    x => x.MerchantTransactionID, ValidationRegexConstants.MerchantTransactionID));
            AddRuleFor(x => x.OriginatorTransactionID)
                .WithPredicate(x =>
                    string.IsNullOrWhiteSpace(x.OriginatorTransactionID) || Regex.IsMatch(x.OriginatorTransactionID,
                        ValidationRegexConstants.OriginatorTransactionID))
                .WithErrorMessage(Operator.InvalidPropertyMessage<AlternativePaymentRequest>(
                    x => x.OriginatorTransactionID, ValidationRegexConstants.OriginatorTransactionID));
            AddRuleFor(x => x.Amount)
                .WithPredicate(x => x == null || x.Amount.Value > 0 && Regex.IsMatch(x.Amount.Value.ToString(CultureInfo.InvariantCulture), ValidationRegexConstants.Amount))
                .WithErrorMessage(Operator.InvalidPropertyMessage<AlternativePaymentRequest>(x => x.Amount));
            AddRuleFor(x => x.Currency)
                .WithPredicate(x => !string.IsNullOrWhiteSpace(x.Currency) && Currency.Exists(x.Currency))
                .WithErrorMessage(Operator.InvalidPropertyMessage<AlternativePaymentRequest>(x => x.Currency));
            AddRuleFor(x => x.ReturnURL)
                .WithPredicate(x => !string.IsNullOrWhiteSpace(x.ReturnURL) && Regex.IsMatch(x.ReturnURL, ValidationRegexConstants.ReturnURL))
                .WithErrorMessage(Operator.InvalidPropertyMessage<AlternativePaymentRequest>(x => x.ReturnURL, ValidationRegexConstants.ReturnURL));
            AddRuleFor(x => x.Description)
                .WithPredicate(x => string.IsNullOrWhiteSpace(x.Description) || Regex.IsMatch(x.Description, ValidationRegexConstants.Description))
                .WithErrorMessage(Operator.InvalidPropertyMessage<AlternativePaymentRequest>(x => x.Description, ValidationRegexConstants.Description));
            AddRuleFor(x => x.MethodID)
                .WithPredicate(x => x.MethodID == null || Regex.IsMatch(x.MethodID.Value.ToString(CultureInfo.InvariantCulture), ValidationRegexConstants.MethodID))
                .WithErrorMessage(Operator.InvalidPropertyMessage<AlternativePaymentRequest>(x => x.MethodID, ValidationRegexConstants.MethodID));
            AddRuleFor(x => x.MethodOptionID)
                .WithPredicate(x => x.MethodOptionID == null || Regex.IsMatch(x.MethodOptionID.Value.ToString(CultureInfo.InvariantCulture), ValidationRegexConstants.MethodOptionID))
                .WithErrorMessage(Operator.InvalidPropertyMessage<AlternativePaymentRequest>(x => x.MethodOptionID, ValidationRegexConstants.MethodOptionID));
            AddRuleFor(x => x.Guaranteed)
                .WithPredicate(x => x.Guaranteed == null || Regex.IsMatch(x.Guaranteed.ToString(), ValidationRegexConstants.Guaranteed))
                .WithErrorMessage(Operator.InvalidPropertyMessage<AlternativePaymentRequest>(x => x.Guaranteed, ValidationRegexConstants.Guaranteed));
            AddRuleFor(x => x.RedirectInIframe)
                .WithPredicate(x => x.RedirectInIframe == null || Regex.IsMatch(x.RedirectInIframe.ToString(), ValidationRegexConstants.Guaranteed))
                .WithErrorMessage(Operator.InvalidPropertyMessage<AlternativePaymentRequest>(x => x.RedirectInIframe, ValidationRegexConstants.Guaranteed));
            AddRuleFor(x => x.RedirectMerchantInIframe)
                .WithPredicate(x => x.RedirectMerchantInIframe == null || Regex.IsMatch(x.RedirectMerchantInIframe.ToString(), ValidationRegexConstants.Guaranteed))
                .WithErrorMessage(Operator.InvalidPropertyMessage<AlternativePaymentRequest>(x => x.RedirectMerchantInIframe, ValidationRegexConstants.Guaranteed));
            AddRuleFor(x => x.Language)
                .WithPredicate(x => string.IsNullOrWhiteSpace(x.Language) || Regex.IsMatch(x.Language, ValidationRegexConstants.Language))
                .WithErrorMessage(Operator.InvalidPropertyMessage<AlternativePaymentRequest>(x => x.Language, ValidationRegexConstants.Language));

            AddInnerValidatorFor(x => x.BillingAddress, () => InnerValidator.Create(addressValidator, true));

            AddInnerValidatorFor(x => x.ShippingAddress, () => InnerValidator.Create(addressValidator, true));

            AddInnerValidatorFor(x => x.Customer, () => InnerValidator.Create(customerValidator, true));

            AddInnerValidatorFor(x => x.Articles, () => InnerValidator.Create(new EnumerableValidator<Article>(articleValidator), true));
           
        }
    }
}
