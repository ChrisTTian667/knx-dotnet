using System;
using System.Linq.Expressions;

namespace Knx.Common
{
    public static class ExpressionFuncTExtension
    {
        public static string GetPropertyName<T>(this Expression<Func<T>> propertyExpression)
        {
            var lambda = (LambdaExpression)propertyExpression;

            MemberExpression memberExpression;

            if (lambda.Body is UnaryExpression unaryExpression)
                memberExpression = (MemberExpression)unaryExpression.Operand;
            else
                memberExpression = (MemberExpression)lambda.Body;

            return memberExpression.Member.Name;
        }

        public static string GetArgumentName<T>(this Expression<Func<T>> argument) where T : class
        {
            var body = (MemberExpression)argument.Body;
            return body.Member.Name;
        }

        public static Type GetArgumentType<T>(this Expression<Func<T>> argument) where T : class
        {
            return argument.Body.Type;
        }
    }
}
