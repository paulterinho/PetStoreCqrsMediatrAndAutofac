using Petstore.Common.Command;
using Petstore.Common.Utils;
using System;
using System.Collections.Generic;

namespace Petstore.Common
{
    public class PetStoreException : ResultException<PetStoreErrorValue>
    {

        public PetStoreException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PetStoreException(PetStoreErrorValue errorEnumTypeCode, IDictionary<string, PetStoreErrorValue> errors=null, string message = null) : base(errorEnumTypeCode, errors, message)
        {
        }
    }
}
