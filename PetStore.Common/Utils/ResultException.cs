using System;
using System.Collections.Generic;

namespace Petstore.Swagger.Io.Common.Utils
{


    /// <summary>
    /// Exception type for pets exceptions
    /// </summary>
    /// 
    /// <typeparam name="ErrorEnumType">The Enum that contains the Errors. Generally created by NSwag</typeparam>
    /// <typeparam name="ErrorDictionaryType">We are defining this in the SDKs, and one of the design quirks is we have to pass it donw here. It has additional serialization methods we need.</typeparam>
    public class ResultException<ErrorEnumType> //PetStoreErrors
        : Exception
        where ErrorEnumType : Enum
    {
        public ErrorEnumType Code { get; private set; }

        private IDictionary<string, ErrorEnumType> _Errors { get; set; }

        public IDictionary<string, ErrorEnumType> Errors
        {
            get
            {
                if (_Errors == null)
                {
                    _Errors = new Dictionary<string, ErrorEnumType>();
                }
                return _Errors;
            }
        }

        public ResultException(ErrorEnumType errorEnumTypeCode, IDictionary<string, ErrorEnumType> errors, string message = null)
            : base(message)
        {
            _Errors = errors;
            Code = errorEnumTypeCode;
        }

        public void AddErrors(IDictionary<string, ErrorEnumType> errors)
        {
            foreach (var e in errors)
            {
                Errors.Add(e.Key, e.Value);
            }
        }

        public ResultException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}