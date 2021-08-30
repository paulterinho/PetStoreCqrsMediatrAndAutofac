using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Petstore.Common.Utils
{
    /// <summary>
    /// C# makes it difficult to get the EnumMember.Value property for String Enums. This utility makes it easier to deal with.
    /// </summary>
    public class EnumUtils
    {

        /// <summary>
        // Let's us get the EnumMember.Value property (The string display value of an enum)
        /// </summary>
        /// <typeparam name="T">The Enum Type whose value you wish to get.</typeparam>
        /// <param name="enumVal">The Enum you wish to get the Value of</param>
        /// <returns></returns>
        public static string GetEnumMemberAttrValue<T>(object enumVal, bool returnEmptyIfNull = false)
        {
            if (returnEmptyIfNull && enumVal == null)
            {
                return "";
            }

            string enumMemberValueStr = null;
            try
            {
                var enumType = typeof(T);
                var memInfo = enumType.GetMember(enumVal.ToString());
                var attr = memInfo.FirstOrDefault()?.GetCustomAttributes(false).OfType<EnumMemberAttribute>().FirstOrDefault();
                if (attr != null)
                {
                    enumMemberValueStr = attr.Value;
                }
            }
            catch (Exception exp)
            {
                Log.Logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw exp;
            }

            return enumMemberValueStr;
        }

        /// <summary>
        /// Get an enum based on it's EnumMember.Value String value. 
        /// </summary>
        /// <typeparam name="T">Any Enum</typeparam>
        /// <param name="value">The selected Enum Member</param>
        /// <returns></returns>
        public static object FindByValue<T>(string value)
            where T : Enum
        {
            object returnEnum = null;
            try
            {
                // https://stackoverflow.com/questions/105372/how-to-enumerate-an-enum
                foreach (T e in (T[])Enum.GetValues(typeof(T)))
                {
                    string foundValue = GetEnumMemberAttrValue<T>(e);

                    if (value.Equals(foundValue, StringComparison.OrdinalIgnoreCase))
                    {
                        returnEnum = e;
                        break;
                    }
                }
            }
            catch (Exception exp)
            {
                Log.Logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw exp;
            }

            return returnEnum;
        }

        /// <summary>
        /// Allows us to find a String Enum by it's string values. Use this if you need to find more than one. 
        /// </summary>
        /// <typeparam name="T">The Enum Type</typeparam>
        /// <param name="values">The string values you are looking for.</param>
        /// <param name="cachedDict">A previously cached Dictionary if you have one (Use `CreateEnumDictionaryByValue` to create one.)</param>
        /// <returns>The list of Enums you are looking for.</returns>
        public static List<T> FindByValues<T>(string[] values, Dictionary<string, T> cachedDict = null)
            where T : Enum
        {
            List<T> returnEnums = new List<T>();
            try
            {
                Dictionary<string, T> dict = cachedDict ?? CreateDictionaryByValue<T>();

                // https://stackoverflow.com/questions/105372/how-to-enumerate-an-enum
                foreach (string value in values)
                {

                    T foundEnum = default;
                    if (dict.TryGetValue(value, out foundEnum))
                    {
                        returnEnums.Add(foundEnum);
                    }
                }
            }
            catch (Exception exp)
            {
                Log.Logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw exp;
            }

            return returnEnums.Count > 0 ? returnEnums : null;
        }

        /// <summary>
        /// Creates a Dictionary with the key as the MemberEnum.Value. This will allow us to a Big-O of O(1) if we cache the values at runtime.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<string, T> CreateDictionaryByValue<T>(IEqualityComparer<string> comparer = null)
        {
            Dictionary<string, T> dict = null;

            try
            {
                // https://stackoverflow.com/questions/5583717/enum-to-dictionary-in-c-sharp
                dict = Enum.GetValues(typeof(T))
                   .Cast<T>()
                   .ToDictionary(t => GetEnumMemberAttrValue<T>(t), t => t, comparer);
            }
            catch (Exception exp)
            {
                Log.Logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw exp;
            }

            return dict;
        }

        /// <summary>
        /// Uses the `ToString` method to create the key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static Dictionary<string, T> CreateDictionaryByToString<T>(IEqualityComparer<string> comparer = null)
        {
            Dictionary<string, T> dict = null;

            try
            {
                // https://stackoverflow.com/questions/5583717/enum-to-dictionary-in-c-sharp
                dict = Enum.GetValues(typeof(T))
                   .Cast<T>()
                   .ToDictionary(t => t.ToString(), t => t, comparer);
            }
            catch (Exception exp)
            {
                Log.Logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw exp;
            }

            return dict;
        }

        /// <summary>
        /// Get a dictionary with a key as the Enum's int value and the value as the human readble part.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static Dictionary<int, string> CreateDictionaryByEnum<T>()
            where T : Enum
        {
            Dictionary<int, string> dict = null;

            try
            {
                if (!typeof(T).IsEnum)
                    throw new ArgumentException("Type must be an enum");

                // @see https://stackoverflow.com/questions/5583717/enum-to-dictionary-in-c-sharp
                dict = Enum.GetValues(typeof(T))
                    .Cast<T>()
                    // secret sauce: see that double cast? ((int)(object)t) Apparently you need to do that to cast an enum inside a lambda
                    .ToDictionary(t => (int)(object)t, t => GetEnumMemberAttrValue<T>(t));
            }
            catch (Exception exp)
            {
                Log.Logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw exp;
            }

            return dict;
        }

        /// <summary>
        /// Creates a simple list of objects from Enums.
        /// </summary>
        /// <typeparam name="EnumType">The type of Enum you want to convert.</typeparam>
        /// <returns></returns>
        public static List<EnumObject<EnumType>> ToObjectList<EnumType>()
            where EnumType : Enum
        {
            // TODO: figure out how to make a dictionary with the enum as the key :(
            try
            {
                return Enum.GetValues(typeof(EnumType))
               .Cast<EnumType>()
               .Select(t => new EnumObject<EnumType>()
               {
                   // This casting to an Int may result in a error one day. It's actually an object. 
                   Id = (int)Enum.Parse(typeof(EnumType), t.ToString()),
                   DisplayValue = GetEnumMemberAttrValue<EnumType>(t),
                   Enum = t, // Kendo components need the actual Enum when working with a MultiSelect.
                   Name = t.ToString()
               })
               .ToList();
            }
            catch (Exception exp)
            {
                Log.Logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw exp;
            }
        }

        public static List<EnumObject<EnumType>> ToObjectList<EnumType>(IEnumerable<EnumType> enumList)
          where EnumType : Enum
        {
            // TODO: figure out how to make a dictionary with the enum as the key :(
            try
            {
                return ToObjectList<EnumType>().Where(x => enumList.Contains(x.Enum)).ToList();
            }
            catch (Exception exp)
            {
                Log.Logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw exp;
            }
        }

        public static int GetIntValue(Enum enumValue)
        {
            return (int)Enum.Parse(enumValue.GetType(), enumValue.ToString());
        }

        /// <summary>
        /// Simple POCO for converting Enums to POCOs. See the  `EnumToObjectList` method.
        /// </summary>
        /// <typeparam name="T">The type of Enum you wish to conver to. </typeparam>
        public class EnumObject<T>
        {
            public int Id;
            public string DisplayValue;
            public T Enum;// Kendo components need the actual Enum when working with a MultiSelect.
            public string Name;// enum value name as string
        }
    }
}
