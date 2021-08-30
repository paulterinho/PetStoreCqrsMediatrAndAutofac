using FluentValidation;
using Petstore.Api.Application.Command;
using Petstore.Common.Command;
using System;

namespace Petstore.Api.Application.Validator
{

    namespace WaiverRequests.API.Application.Validators
    {
        /// <summary>
        /// Fluent Validator syntax checker. This is only for correct syntax, not for business rules (which will get checked in the Domain Layer)
        /// 
        /// @see https://docs.fluentvalidation.net/en/latest/conditions.html
        /// 
        /// </summary>
        public class CreatePetValidator : AbstractValidator<CreatePetCommand>
        {
            public CreatePetValidator()
            {
                // ==================================
                //      TOP LEVEL MEMBERS
                // ==================================

                // Make sure it's an empty guiud
                RuleFor(cmd => cmd.Pet.ResourceID)
                    .Equal(new Guid())
                    .WithErrorCode(PetStoreErrorValue.Pet_Resource_ID_must_be_00000000000000000000000000000000_when_creating_a_Pet.ToString());

                // Make sure name isn't empty
                RuleFor(cmd => cmd.Pet.Name)
                    .NotEqual(string.Empty)
                    .WithErrorCode(PetStoreErrorValue.Pet_Name_is_required.ToString());

                // make sure type isn't empty
                RuleFor(cmd => cmd.Pet.Type)
                    .NotNull()
                    .WithErrorCode(PetStoreErrorValue.Pet_Name_is_required.ToString());

            }
        }
    }
}