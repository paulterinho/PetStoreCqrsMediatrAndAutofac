using Petstore.Common.Command;
using PetStore.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Based on: https://github.com/dotnet-architecture/eShopOnContainers/blob/main/src/Services/Ordering/Ordering.Domain/AggregatesModel/OrderAggregate/OrderStatus.cs
/// </summary>
/// 
namespace PetStore.Domain.Model
{
    public class PetTypeEnum
    : Enumeration<PetTypeValue>
    {
        public static PetTypeEnum Bat = new PetTypeEnum(PetTypeValue.Bat);
        public static PetTypeEnum Cat = new PetTypeEnum(PetTypeValue.Cat);
        public static PetTypeEnum Dog = new PetTypeEnum(PetTypeValue.Dog);
        public static PetTypeEnum Goat = new PetTypeEnum(PetTypeValue.Goat);
        public static PetTypeEnum Monkey = new PetTypeEnum(PetTypeValue.Monkey);
        public static PetTypeEnum Rock = new PetTypeEnum(PetTypeValue.Rock);
        public static PetTypeEnum Sloth = new PetTypeEnum(PetTypeValue.Sloth);

        public PetTypeEnum(PetTypeValue waiverStatus)
            : base(waiverStatus) { }

        public static IEnumerable<PetTypeEnum> List() =>
            new[] { Bat, Cat, Dog, Goat, Monkey, Rock, Sloth };

        public static PetTypeEnum? FromName(string name)
        {
            PetTypeEnum? returnType = null;

            if (name != null)
            {
                var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

                if (state != null)
                {
                    returnType = state;
                }
            }

            return returnType;
        }
    }
}