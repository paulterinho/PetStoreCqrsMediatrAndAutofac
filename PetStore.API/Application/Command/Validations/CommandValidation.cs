using FluentValidation;
using Petstore.Swagger.Io.Common.Utils;
using Serilog;
using System;

namespace Petstore.Swagger.Io.Api.Application.Command
{

    /// <summary>
    /// This class exists to show how to integrate a validator into the Mediatr request pipeline. 
    /// </summary>
    public class CreatePetValidatior : AbstractValidator<CreatePetCommand>
    {
        public CreatePetValidatior(ILogger logger)
        {
            try
            {
                //TODO FUTURE TBD

                // NOTE, here we can do syntax checking for a request BEFORE a domain object is even marshalled (PetStore). 
                /*
                        RuleFor(command => command.PetStore.ResourceID).NotEmpty();

                        // other examples of what Fluent Validator can do.
                        RuleFor(command => command.CardNumber).NotEmpty().Length(12, 19);
                        RuleFor(command => command.CardHolderName).NotEmpty();
                        RuleFor(command => command.CardExpiration).NotEmpty().Must(BeValidExpirationDate).WithMessage("Please specify a valid card expiration date");
                        RuleFor(command => command.CardSecurityNumber).NotEmpty().Length(3);
                        RuleFor(command => command.CardTypeId).NotEmpty();
                        RuleFor(command => command.OrderItems).Must(ContainOrderItems).WithMessage("No order items found");
                */
            }
            catch (Exception exp)
            {
                logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw exp;
            }
        }
    }
}