﻿using System;
using System.Linq.Expressions;

namespace S2p.RestClient.Sdk.Infrastructure.Extensions
{
    public static class Operator
    {
        public static string Nameof<T>(Expression<Func<T, object>> propertyExpression)
        {
            var unaryExpression = propertyExpression.Body as UnaryExpression;
            var expression = unaryExpression?.Operand ?? propertyExpression.Body;
            var memberExpression = expression as MemberExpression;
            return memberExpression?.Member.Name;
        }

        public static string InvalidPropertyMessage<T>(Expression<Func<T, object>> propertyExpression)
        {
            return $"Invalid {Nameof(propertyExpression)}";
        }

        public static string InvalidPropertyMessage<T>(Expression<Func<T, object>> propertyExpression, string regex)
        {
            return $"Invalid {Nameof(propertyExpression)}, Regex: {regex.ValueIfNull(() => string.Empty)}";
        }
    }
}
