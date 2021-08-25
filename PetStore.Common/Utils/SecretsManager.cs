using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Petstore.Swagger.Io.Common.Utils;
using Serilog;
using System;

namespace PetStore.Common.Utils
{
    /// <summary>
    /// @see https://aws.amazon.com/blogs/security/how-to-use-aws-secrets-manager-client-side-caching-in-dotnet/
    /// </summary>
    public class SecretsManager : IGetDbConnectionString
    {
        private readonly ILogger _logger;

        private string Environment { get; }

        private string DataBaseConnectionString;

        public SecretsManager(ILogger logger, string environment)
        {
            try
            {
                _logger = logger;

                // this can be null too. 
                //System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                Environment = environment;
            }
            catch (Exception exp)
            {
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
            }
        }

        private string GetSecret(string secretName, string region, bool isDevelopmentEnvironment)
        {
            IAmazonSecretsManager client;

            Console.WriteLine("Secret requested from Secrets Manager");
            if (isDevelopmentEnvironment)
            {
                var accessKeyID = "ACCESS-KEY-ID-HERE";
                var accessSecretKey = "ACCESS-SECRET-HERE";
                client = new AmazonSecretsManagerClient(accessKeyID, accessSecretKey, RegionEndpoint.GetBySystemName(region));
            }
            else
            {
                client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));
            }

            GetSecretValueRequest request = new GetSecretValueRequest();
            request.SecretId = secretName;
            request.VersionStage = "AWSCURRENT"; // VersionStage defaults to AWSCURRENT if unspecified.

            GetSecretValueResponse response = null;

            try
            {
                response = client.GetSecretValueAsync(request).Result;
                Console.WriteLine("Secret successfully retrieved from Secrets Manager");
            }

            #region Exception Handling
            catch (DecryptionFailureException exp)
            {
                // Secrets Manager can't decrypt the protected secret text using the provided KMS key.
                // Deal with the exception here, and/or rethrow at your discretion.
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }
            catch (InternalServiceErrorException exp)
            {
                // An error occurred on the server side.
                // Deal with the exception here, and/or rethrow at your discretion.
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }
            catch (InvalidParameterException exp)
            {
                // You provided an invalid value for a parameter.
                // Deal with the exception here, and/or rethrow at your discretion
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }
            catch (InvalidRequestException exp)
            {
                // You provided a parameter value that is not valid for the current state of the resource.
                // Deal with the exception here, and/or rethrow at your discretion.
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }
            catch (ResourceNotFoundException exp)
            {
                // We can't find the resource that you asked for.
                // Deal with the exception here, and/or rethrow at your discretion.
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }
            catch (System.AggregateException ae)
            {
                // More than one of the above exceptions were triggered.
                // Deal with the exception here, and/or rethrow at your discretion.
                _logger.Error(ae, PetStoreConstants.ERROR_LOGGING_FORMAT, ae.Message);
                throw;
            }
            #endregion //Exception Handling

            return response.SecretString ?? null;
        }

        public string GetDbConnectionString(string clientID)
        {
            if (DataBaseConnectionString == null)
            {

                ////var secretJSONString = SecretsManager.GetSecret("", RegionEndpoint.USEast1);
                ////WaiverSecretsManager.GetSecret();
                ////var secret = SecretsManager.GetSecret(WaiversConstants.SECRETS_NAME, "us-east-1");

                //if (Environment == DevelopmentEnvironment.EnvironmentName)
                //{
                //    _logger.Information("IsDevelopmentEnvironment: {0}", Environment);
                //    DataBaseConnectionString = JObject.Parse(DevelopmentEnvironment.DbConnection)[SecretNames.DbConnection].ToString();
                //}
                //else
                //{
                //    // TODO: set up secrets manager this up and get it working...
                //    //var secretJSONString = SecretsManager.GetSecret(SecretNames.GetSecretName(SecretNames.DbConnection, TenantClientID), TenantSecretsRegion);
                //    //DataBaseConnectionString = JObject.Parse(secretJSONString)[SecretNames.DbConnection].ToString();

                //    // Ideally, it'd get an ARN to amazon resource name. This would be instead of doing it by TenantClientID / Region

                //    // TODO: remove this hard coded value. 
                //    _logger.Information("IsDevelopmentEnvironment: {0}", Environment);
                //    DataBaseConnectionString = JObject.Parse(DevelopmentEnvironment.DbConnection)[SecretNames.DbConnection].ToString();
                //}

                //cx_ui.Data.Utilities.ConfigurationCache.RetrieveDWConnectionString

                //HttpContext.Current.Request.Url.Host
                //Request.Url.Host
                //string test = HttpContext.Current.Request.Url.Host;


                throw new NotImplementedException();
            }

            return DataBaseConnectionString;
        }
    }
}