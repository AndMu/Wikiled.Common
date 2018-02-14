using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Wikiled.Common.Arguments
{
    /// <summary>
    /// Common guard class for argument validation.
    /// </summary>
    [DebuggerStepThrough]
    public static class Guard
    {
        /// <summary>
        /// Ensures the given <paramref name="value"/> is not null.
        /// Throws <see cref="ArgumentNullException"/> otherwise.
        /// </summary>
        /// <exception cref="System.ArgumentException">The <paramref name="value"/> is null.</exception>
        public static void NotNull<T>(Expression<Func<T>> reference, T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(GetParameterName(reference), "Parameter cannot be null.");
            }
        }

        /// <summary>Construct
        /// Ensures that array is non emptu and not null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reference"></param>
        /// <param name="value"></param>
        public static void NotEmpty<T>(Expression<Func<T[]>> reference, T[] value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(GetParameterName(reference), "Parameter cannot be null.");
            }

            if (value.Length == 0)
            {
                throw new ArgumentOutOfRangeException(GetParameterName(reference), "Array is empty.");
            }
        }

        /// <summary>
        /// Ensures the given string <paramref name="value"/> is not null or empty.
        /// Throws <see cref="ArgumentNullException"/> in the first case, or 
        /// <see cref="ArgumentException"/> in the latter.
        /// </summary>
        /// <exception cref="System.ArgumentException">The <paramref name="value"/> is null or an empty string.</exception>
        public static void NotNullOrEmpty(Expression<Func<string>> reference, string value)
        {
            NotNull<string>(reference, value);
            if (value.Length == 0)
            {
                throw new ArgumentException("Parameter cannot be empty.", GetParameterName(reference));
            }
        }

        /// <summary>
        /// Ensures the given string <paramref name="value"/> is valid according 
        /// to the <paramref name="validate"/> function. Throws <see cref="ArgumentNullException"/> 
        /// otherwise.
        /// </summary>
        /// <exception cref="System.ArgumentException">The <paramref name="value"/> is not valid according 
        /// to the <paramref name="validate"/> function.</exception>
        public static void IsValid<T>(Expression<Func<T>> reference, T value, Func<T, bool> validate, string message)
        {
            if (!validate(value))
            {
                throw new ArgumentException(message, GetParameterName(reference));
            }
        }

        /// <summary>
        /// Ensures the given string <paramref name="value"/> is valid according 
        /// to the <paramref name="validate"/> function. Throws <see cref="ArgumentNullException"/> 
        /// otherwise.
        /// </summary>
        /// <exception cref="System.ArgumentException">The <paramref name="value"/> is not valid according 
        /// to the <paramref name="validate"/> function.</exception>
        public static void IsValid<T>(Expression<Func<T>> reference, T value, Func<T, bool> validate, string format, params object[] args)
        {
            if (!validate(value))
            {
                throw new ArgumentException(string.Format(format, args), GetParameterName(reference));
            }
        }

        private static string GetParameterName(Expression reference)
        {
            var lambda = reference as LambdaExpression;
            var member = lambda?.Body as MemberExpression;
            return member?.Member.Name ?? "NULL";
        }
    }
}