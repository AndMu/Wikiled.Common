using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace Wikiled.Common.Reflection
{
    public static class ExpressionReflectionExtensions
    {
        public static Func<TInstance, string> GetValueGetter<TInstance>(this PropertyInfo propertyInfo)
        {
            if (typeof(TInstance) != propertyInfo.DeclaringType)
            {
                throw new ArgumentException();
            }

            var instance = Expression.Parameter(propertyInfo.DeclaringType, "i");
            Expression property = Expression.Property(instance, propertyInfo);
            if (propertyInfo.PropertyType == typeof(DateTime))
            {
                var format = Expression.Constant("dd/MM/yyyy HH:mm:ss");
                var culture = Expression.Constant(CultureInfo.InvariantCulture);
                //new "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture
                //DateTime date;
                //var ta = date.ToString("Test", CultureInfo.InvariantCulture)
                property = Expression.Call(property, "ToString", null, format, culture);
            }
            else if (propertyInfo.PropertyType != typeof(string))
            {
                property = Expression.Call(property, "ToString", new Type[0]);
            }

            return (Func<TInstance, string>)Expression.Lambda(property, instance).Compile();
        }

        public static Func<TInstance, TReturn> GetValueGetter<TInstance, TValue, TReturn>(
            this PropertyInfo propertyInfo,
            Func<TValue, TReturn> converter)
        {
            if (typeof(TInstance) != propertyInfo.DeclaringType)
            {
                throw new ArgumentException();
            }

            var instance = Expression.Parameter(propertyInfo.DeclaringType, "i");
            var property = Expression.Property(instance, propertyInfo);
            Expression<Func<TValue, TReturn>> lambdaConvert = input => converter(input);
            var callExpr = Expression.Invoke(lambdaConvert, property);
            return (Func<TInstance, TReturn>)Expression.Lambda(callExpr, instance).Compile();
        }

        public static Action<TInstance, TInput> GetValueSetter<TInstance, TValue, TInput>(
            this PropertyInfo propertyInfo,
            Func<TInput, TValue> converter)
        {
            if (typeof(TInstance) != propertyInfo.DeclaringType)
            {
                throw new ArgumentException();
            }

            var instance = Expression.Parameter(propertyInfo.DeclaringType, "instance");
            var argument = Expression.Parameter(typeof(TInput), "value");

            Expression<Func<TInput, TValue>> lambdaConvert = input => converter(input);
            var callExpr = Expression.Invoke(lambdaConvert, argument);

            var setterCall = Expression.Call(instance, propertyInfo.GetSetMethod(), callExpr);
            return (Action<TInstance, TInput>)Expression.Lambda(setterCall, instance, argument).Compile();
        }

        public static Func<TInstance, TValue> GetValueGetter<TInstance, TValue>(this PropertyInfo propertyInfo)
        {
            if (typeof(TInstance) != propertyInfo.DeclaringType)
            {
                throw new ArgumentException();
            }

            var returnValue = typeof(TValue);
            var instance = Expression.Parameter(propertyInfo.DeclaringType, "i");
            var property = Expression.Property(instance, propertyInfo);
            Expression convert = property;
            if (returnValue != propertyInfo.PropertyType)
            {
                convert = Expression.TypeAs(property, returnValue);
            }

            return (Func<TInstance, TValue>)Expression.Lambda(convert, instance).Compile();
        }

        public static Action<TInstance, TValue> GetValueSetter<TInstance, TValue>(this PropertyInfo propertyInfo)
        {
            if (typeof(TInstance) != propertyInfo.DeclaringType)
            {
                throw new ArgumentException();
            }

            var returnValue = typeof(TValue);
            var instance = Expression.Parameter(propertyInfo.DeclaringType, "i");
            var argument = Expression.Parameter(typeof(TValue), "a");
            Expression convert = argument;

            if (returnValue != propertyInfo.PropertyType)
            {
                convert = Expression.Convert(argument, propertyInfo.PropertyType);
            }

            var setterCall = Expression.Call(instance, propertyInfo.GetSetMethod(), convert);
            return (Action<TInstance, TValue>)Expression.Lambda(setterCall, instance, argument).Compile();
        }
    }
}
