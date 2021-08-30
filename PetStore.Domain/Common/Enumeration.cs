using Petstore.Common.Utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PetStore.Domain.Common
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/net-core-microservice-domain-model
    /// </summary>
    public abstract class Enumeration<E> : IComparable
        where E : Enum
    {
        /// <summary>
        /// Secret sauce: allows us to pass in a Enum, and get the string value in a Big-O (1) sort of way.
        /// </summary>
        private static Dictionary<int, string> _EnumDictionary = EnumUtils.CreateDictionaryByEnum<E>();

        public string Name { get; private set; }

        public int ID { get; private set; }

        /// <summary>
        /// This is the actually enum that this object is using
        /// </summary>
        public E EnumValue { get; private set; }

        public Enumeration(E enumValue)
        {
            try
            {
                ID = (int)(object)enumValue;
                Name = _EnumDictionary[ID];
                EnumValue = enumValue;
            }
            catch (Exception exp)
            {
                Log.Logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT);
                throw exp;
            }
        }

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration<E>
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration<E>;

            if (otherValue == null)
                return false;

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = ID.Equals(otherValue.ID);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() => ID.GetHashCode();

        public static int AbsoluteDifference(Enumeration<E> firstValue, Enumeration<E> secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.ID - secondValue.ID);
            return absoluteDifference;
        }

        public static T FromValue<T>(int value) where T : Enumeration<E>
        {
            var matchingItem = Parse<T, int>(value, "value", item => item.ID == value);
            return matchingItem;
        }

        private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration<E>
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
                throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");

            return matchingItem;
        }

        public int CompareTo(object other) => ID.CompareTo(((Enumeration<E>)other).ID);
    }
}