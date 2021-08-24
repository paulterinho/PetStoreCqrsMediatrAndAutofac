using Petstore.Swagger.Io.Common.Command;
using Petstore.Swagger.Io.Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Petstore.Swagger.Io.Common
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
