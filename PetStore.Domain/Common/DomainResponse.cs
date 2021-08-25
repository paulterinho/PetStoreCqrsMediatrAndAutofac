using Petstore.Swagger.Io.Common.Utils;
using Serilog;
using System;
using System.Collections.Generic;

namespace PetStore.Domain.Common
{
    public class DomainResponse<ErrorEnumType>
        where ErrorEnumType : Enum
    {
        public Dictionary<string, ErrorEnumType> Errors { get; set; }

        public bool Success { get { return Errors.Count == 0; } }

        public DomainResponse(Dictionary<string, ErrorEnumType> errors = null)
        {
            Errors = errors ?? new Dictionary<string, ErrorEnumType>();
        }

        public void AddErrors(Dictionary<string, ErrorEnumType> errors)
        {
            try
            {
                foreach (var error in errors)
                {
                    Errors.Add(error.Key, error.Value);
                }
            }
            catch (Exception exp)
            {
                Log.Logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw exp;
            }
        }

        public void AddError(string key, ErrorEnumType errorEnum)
        {
            try
            {
                Errors.Add(key, errorEnum);
            }
            catch (Exception exp)
            {
                Log.Logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw exp;
            }
        }
    }
}