using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Core.JqGrid.Extensions
{
    public static class FilterExtensions
    {
        private static List<string> _ALLOW_NULL_SEARCH_COLUMNS = new List<string> { "publicPlaceIndicator" };
        public static Expression<Func<T, bool>> BuildExpression<T>(this Filter filter) where T : class
        {
            GroupOp groupOp = filter.groupOp.ToEnum<GroupOp>();
            bool isAnd = groupOp == GroupOp.AND;
            Expression<Func<T, bool>> predicate = null;

            foreach (var rule in filter.rules)
            {
                var expression = BuildExpression<T>(rule.field, rule.data, rule.op.ToEnum<WhereOperation>());

                if (predicate == null)
                    predicate = expression;
                else
                    predicate = predicate.Append(expression, isAnd);
            }

            foreach (var f in filter.groups)
            {
                var innerExpression = BuildExpression<T>(f);

                if (predicate == null)
                    predicate = innerExpression;
                else
                    predicate = predicate.Append(innerExpression, isAnd);
            }

            return predicate;
        }

        private static Expression<Func<T, bool>> BuildExpression<T>(string column, string value, WhereOperation operation)
        {
            ParameterExpression rootParameter = Expression.Parameter(typeof(T), "r");
            Expression<Func<T, bool>> predicate = null;

            foreach (var c in column.Split(','))
            {
                var lambdaExpr = Expression.Lambda<Func<T, bool>>(c.Contains('>') ? BuildCollectionExpression(rootParameter, c, value, operation) : BuildSimpleExpression(rootParameter, c, value, operation), rootParameter);

                if (predicate == null)
                    predicate = lambdaExpr;
                else
                    predicate = predicate.Append(lambdaExpr, false);
            }

            return predicate;
        }

        private static Expression BuildCollectionExpression(ParameterExpression rootParameter, string column, string value, WhereOperation operation)
        {
            var columnArr = column.Split('>');
            MemberExpression collectionAccess = GetMemberExpression(rootParameter, columnArr[0]);
            ParameterExpression collectionParameter = Expression.Parameter(collectionAccess.Type.GetGenericArguments()[0], "c");
            Expression collectionExpression = BuildSimpleExpression(collectionParameter, columnArr[1], value, operation);
            LambdaExpression lambdaForTheAnyCallPredicate = Expression.Lambda(collectionExpression, collectionParameter);

            return CallAny(collectionAccess, lambdaForTheAnyCallPredicate);
        }

        private static Expression BuildSimpleExpression(ParameterExpression rootParameter, string column, string value, WhereOperation operation)
        {
            MemberExpression leftExpr = GetMemberExpression(rootParameter, column);

            //change param value type
            //necessary to getting bool from string
            Expression rightExpr = Expression.Convert(Expression.Constant(StringToType(value, leftExpr.Type)), leftExpr.Type);

            //switch operation
            Expression expression = null;

            switch (operation)
            {
                case WhereOperation.IsNull:
                    expression = Expression.Equal(leftExpr, Expression.Constant(null, leftExpr.Type));
                    break;

                case WhereOperation.Equal:
                    expression = Expression.Equal(leftExpr, rightExpr);
                    break;

                case WhereOperation.NotEqual:
                    expression = Expression.NotEqual(leftExpr, rightExpr);
                    break;

                case WhereOperation.LessThan:
                    expression = Expression.LessThan(leftExpr, rightExpr);
                    break;

                case WhereOperation.LessThanOrEqual:
                    expression = Expression.LessThanOrEqual(leftExpr, rightExpr);
                    break;

                case WhereOperation.GreaterThan:
                    expression = Expression.GreaterThan(leftExpr, rightExpr);
                    break;

                case WhereOperation.GreaterThanOrEqual:
                    expression = Expression.GreaterThanOrEqual(leftExpr, rightExpr);
                    break;

                case WhereOperation.BeginsWith:
                    expression = Expression.Call(leftExpr, typeof(string).GetMethod("StartsWith", new[] { typeof(string) }), Expression.Constant(value));
                    break;

                case WhereOperation.NotBeginWith:
                    expression = Expression.Not(Expression.Call(leftExpr, typeof(string).GetMethod("StartsWith", new[] { typeof(string) }), Expression.Constant(value)));
                    break;

                case WhereOperation.EndsWith:
                    expression = Expression.Call(leftExpr, typeof(string).GetMethod("EndsWith", new[] { typeof(string) }), Expression.Constant(value));
                    break;

                case WhereOperation.NotEndWith:
                    expression = Expression.Not(Expression.Call(leftExpr, typeof(string).GetMethod("EndsWith", new[] { typeof(string) }), Expression.Constant(value)));
                    break;

                case WhereOperation.Contains:
                    expression = Expression.Call(leftExpr, typeof(string).GetMethod("Contains", new[] { typeof(string) }), Expression.Constant(value));
                    break;

                case WhereOperation.NotContain:
                    expression = Expression.Not(Expression.Call(leftExpr, typeof(string).GetMethod("Contains", new[] { typeof(string) }), Expression.Constant(value)));
                    break;
            }

            if (leftExpr.Type.IsGenericType && leftExpr.Type.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(leftExpr.Type) == typeof(bool))
            {
                bool addNotNull = !_ALLOW_NULL_SEARCH_COLUMNS.Contains(column) || !string.IsNullOrEmpty(value);
                if (addNotNull)
                {
                    var notNull = Expression.NotEqual(leftExpr, Expression.Constant(null, leftExpr.Type));
                    expression = Expression.AndAlso(expression, notNull);
                }               
            }

            return expression;
        }

        #region Helper

        public static object StringToType(string value, Type propertyType)
        {
            var underlyingType = Nullable.GetUnderlyingType(propertyType);
            if (underlyingType != null)
            {
                //an underlying nullable type, so the type is nullable
                //apply logic for null or empty test
                if (String.IsNullOrEmpty(value)) return null;
            }

            // Advance checking for remove the sorting index in the value
            String sortingIndex = "[SortingDelimiter]";
            if (value.Contains(sortingIndex))
            {
                value = value.Split(new String[] { sortingIndex }, StringSplitOptions.None)[1];
            }

            // Advance checking for String to Boolean Type
            if (propertyType.Name.ToUpper().Contains("BOOL") || (underlyingType != null && underlyingType.Name.ToUpper().Contains("BOOL")))
            {
                if (value.Trim() == "0")
                    value = "FALSE";
                else if (value.Trim() == "1")
                    value = "TRUE";
                else if (value.Trim() == "-1")
                    value = null;
            }
            //if (propertyType.Name.ToUpper().Contains("BOOL") && value.Trim() == "0")
            //{
            //    value = "FALSE";
            //}
            //else if (propertyType.Name.ToUpper().Contains("BOOL") && value.Trim() == "1")
            //{
            //    value = "TRUE";
            //}

            return Convert.ChangeType(value, underlyingType ?? propertyType);
        }

        private static MemberExpression GetMemberExpression(ParameterExpression rootParameter, string column)
        {
            return column.Split('.').Aggregate<string, MemberExpression>(null, (current, property) => Expression.Property(current ?? (rootParameter as Expression), property));
        }

        private static bool IsIEnumerable(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }

        private static Type GetIEnumerableImpl(Type type)
        {
            // Get IEnumerable implementation. Either type is IEnumerable<T> for some T, or it
            // implements IEnumerable<T> for some T. We need to find the interface.
            if (IsIEnumerable(type))
                return type;
            Type[] t = type.FindInterfaces((m, o) => IsIEnumerable(m), null);

            return t[0];
        }

        private static MethodBase GetGenericMethod(Type type, string name, Type[] typeArgs, Type[] argTypes, BindingFlags flags)
        {
            int typeArity = typeArgs.Length;
            var methods = type.GetMethods()
                .Where(m => m.Name == name)
                .Where(m => m.GetGenericArguments().Length == typeArity)
                .Select(m => m.MakeGenericMethod(typeArgs));

            return Type.DefaultBinder.SelectMethod(flags, methods.ToArray(), argTypes, null);
        }

        private static Expression CallAny(Expression collection, Expression predicateExpression)
        {
            Type cType = GetIEnumerableImpl(collection.Type);
            collection = Expression.Convert(collection, cType); // (see "NOTE" below)

            Type elemType = cType.GetGenericArguments()[0];
            Type predType = typeof(Func<,>).MakeGenericType(elemType, typeof(bool));

            // Enumerable.Any<T>(IEnumerable<T>, Func<T,bool>)
            MethodInfo anyMethod = (MethodInfo)
                GetGenericMethod(typeof(Enumerable), "Any", new[] { elemType },
                    new[] { cType, predType }, BindingFlags.Static);

            return Expression.Call(
                anyMethod,
                collection,
                predicateExpression);
        }

        #endregion Helper
    }
}