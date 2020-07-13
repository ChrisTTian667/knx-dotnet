using System;
using System.Linq.Expressions;

namespace Knx
{
    public static class ObjectHelper
    {
        public static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentException("expression must be a property expression");

            return memberExpression.Member.Name;
        }
    }
}