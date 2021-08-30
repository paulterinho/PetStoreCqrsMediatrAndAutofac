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

        public const string APP_SETTINGS_PROP_HOST_URI = "ClientID";

        public const string APP_SETTINGS_PROP_CONNECTION_STRING = "ConnectionStrings:DefaultConnection";
    }
}
