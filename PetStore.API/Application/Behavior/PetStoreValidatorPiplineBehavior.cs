using FluentValidation;
using FluentValidation.Results;
using Petstore.Api.Application.Behavior;
using Petstore.Common;
using Petstore.Common.Command;
using Petstore.Common.Utils;
using Serilog;
using System;
using System.Collections.Generic;

namespace Petstore.API.Application.Validators
{
    /// <summary>
    /// This is an command interceptor. Check out the MediatorModule config file to see how we are tying this into the Mediatr request pipeline. 
    /// 
    /// Here's how it works, and this will hurt your brain: Autofac is providing a class that uses the IValidator interface and injecting them into this class to be used. 
    /// 
    /// See documentation for FluentValidation at https://docs.fluentvalidation.net/en/latest/start.html
    /// </summary>
    public class PetStoreValidatorPipelineBehavior<TRequest, TResponse> : ValidatorBehavior<TRequest, TResponse, PetStoreException, PetStoreErrorValue>
    {
        private Dictionary<string, PetStoreErrorValue> StringToErrorEnumDictionary;

        public PetStoreValidatorPipelineBehavior(IValidator<TRequest>[] validators, ILogger logger) :
            base(validators, logger)
        {
            StringToErrorEnumDictionary = EnumUtils.CreateDictionaryByToString<PetStoreErrorValue>();
        }


        protected override PetStoreException CreateException(List<ValidationFailure> failures)
        {
            Dictionary<string, PetStoreErrorValue> errorsToPassToFrontEnd = new Dictionary<string, PetStoreErrorValue>();
            string currentFailureKey = "";

            failures.ForEach(failure =>
            {

                try
                {
                    // NOTE:    We can't pass Enums from FluentValidator, only String Error Codes. Given that we
                    //          need to look up what string key is of the Enum we actually want to send back.
                    PetStoreErrorValue errorValue;
                    currentFailureKey = failure.ErrorCode;

                    if (!errorsToPassToFrontEnd.ContainsKey(failure.PropertyName))
                    {
                        // try to parse this error
                        errorValue = StringToErrorEnumDictionary[currentFailureKey];
                        errorsToPassToFrontEnd.Add(failure.PropertyName, errorValue);
                    }
                }
                catch (KeyNotFoundException exp)
                {
                    // We've encountered a validation that doesn't yet have mention in the PetStore.API.yaml file. Log it and hopefully a dev will put one in for it later.
                    string errMsg = "PetStoreErrorValue not found. Value found: " + currentFailureKey;
                    _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, errMsg);
                }
                catch (Exception exp)
                {
                    _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                    throw exp;
                }

            });

            return new PetStoreException(
                PetStoreErrorValue.PetStore_has_the_following_validation_errors,
                errorsToPassToFrontEnd,
                PetStoreErrorValue.PetStore_has_the_following_validation_errors.ToString());
        }
    }
}