using FairPlayCombined.Common.CustomExceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Common
{
    public static class DisplayHelper
    {

        public static string MaxLengthFor<TModel>(Expression<Func<TModel, object>> expression)
        {
            MemberExpression? memberExpression = null;
            memberExpression = expression.Body.NodeType switch
            {
                //Check https://stackoverflow.com/questions/3049825/given-a-member-access-lambda-expression-convert-it-to-a-specific-string-represe
                ExpressionType.Convert or ExpressionType.ConvertChecked => ((expression.Body is UnaryExpression ue) ? ue.Operand : null) as MemberExpression,
                _ => expression.Body as MemberExpression,
            };
            PropertyInfo propertyBeingAccessed = (PropertyInfo)memberExpression!.Member;
            StringLengthAttribute stringLengthAttribute =
                propertyBeingAccessed!.GetCustomAttribute<StringLengthAttribute>() ??
                throw new RuleException($"Property '{propertyBeingAccessed.Name}' of type " +
                    $"'{expression.Parameters[0].Type.FullName}' does not have a " +
                    $"{nameof(StringLengthAttribute)}");
            var maxLength = stringLengthAttribute.MaximumLength;
            return maxLength.ToString();
        }
        public static string DisplayFor<TModel>(Expression<Func<TModel, object>> expression)
        {
            MemberExpression? memberExpression = null;
            memberExpression = expression.Body.NodeType switch
            {
                //Check https://stackoverflow.com/questions/3049825/given-a-member-access-lambda-expression-convert-it-to-a-specific-string-represe
                ExpressionType.Convert or ExpressionType.ConvertChecked => ((expression.Body is UnaryExpression ue) ? ue.Operand : null) as MemberExpression,
                _ => expression.Body as MemberExpression,
            };
            PropertyInfo propertyBeingAccessed = (PropertyInfo)memberExpression!.Member;
            DisplayAttribute displayAttribute =
                propertyBeingAccessed!.GetCustomAttribute<DisplayAttribute>() ??
                throw new RuleException($"Property '{propertyBeingAccessed.Name}' of type " +
                    $"'{expression.Parameters[0].Type.FullName}' does not have a " +
                    $"{nameof(DisplayAttribute)}");
            var displayName = displayAttribute.GetName()!;
            return displayName;
        }

        public static string DisplayForEnum<TModel>(Expression<Func<TModel, object>> expression)
        {
            try
            {
                ConstantExpression? memberExpression = null;
                memberExpression = expression.Body.NodeType switch
                {
                    //Check https://stackoverflow.com/questions/3049825/given-a-member-access-lambda-expression-convert-it-to-a-specific-string-represe
                    ExpressionType.Convert or ExpressionType.ConvertChecked => ((expression.Body is UnaryExpression ue) ? ue.Operand : null) as ConstantExpression,
                    _ => expression.Body as ConstantExpression,
                };

                var enumType = typeof(TModel);
                var memberInfo = enumType.GetMember(memberExpression!.Value!.ToString()!)
                    .Single();
                DisplayAttribute displayAttribute =
                    memberInfo!.GetCustomAttribute<DisplayAttribute>() ??
                    throw new RuleException($"Enum value '{memberExpression.Value}' of type " +
                        $"'{expression.Parameters[0].Type.FullName}' does not have a " +
                        $"{nameof(DisplayAttribute)}");
                var displayName = displayAttribute.GetName()!;
                return displayName;
            }
            catch (Exception ex)
            {
                throw new RuleException(ex.Message);
            }
        }
    }
}
