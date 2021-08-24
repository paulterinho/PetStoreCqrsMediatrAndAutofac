using Petstore.Swagger.Io.Common.Utils;
using System;
using System.Threading.Tasks;

namespace PetStore.Domain.Common
{
    public class Result<ErrorEnumType>
        where ErrorEnumType : Enum
    {
        public readonly int RowsAffected = 0;
        public readonly bool IsFailure = false;
        public readonly ResultException<ErrorEnumType>? Error;

        public Result(int rowsAffected, bool isFailure, ResultException<ErrorEnumType>? error)
        {
            RowsAffected = rowsAffected;
            IsFailure = isFailure;
            Error = error;
        }

        public static async Task<Result<ErrorEnumType>> Success(int rowsAffected)
        {
            Result<ErrorEnumType> returnResult = new Result<ErrorEnumType>(rowsAffected, false, null);
            return await Task.FromResult(returnResult);
        }

        public static async Task<Result<ErrorEnumType>> Failure(ResultException<ErrorEnumType> value)
        {
            Result<ErrorEnumType> returnResult = new Result<ErrorEnumType>(0, true, value);
            return await Task.FromResult(returnResult);
        }
    }

}
