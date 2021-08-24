using System;
using System.Collections.Generic;
using System.Text;

namespace Petstore.Swagger.Io.Common.Utils
{
    public class PetStoreConstants
    {
        /// <summary>
        /// Error message template that expects the following error to be the exp.Message property of your Exception.
        /// </summary>
        public const string ERROR_LOGGING_FORMAT = "ERROR handling message: {ExceptionMessage}";
    }
}
