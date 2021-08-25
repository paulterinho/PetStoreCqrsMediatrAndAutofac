using Dapper;
using System;
using System.Data;

namespace Petstore.Swagger.Io.Api.Application.Config
{

    /// <summary>
    /// Dapper has problems with Guids, and needs a tiny bit of help.
    /// 
    /// https://stackoverflow.com/questions/5898988/map-string-to-guid-with-dapper
    /// </summary>
    public class DapperGuidTypeHandler : SqlMapper.TypeHandler<Guid>
    {

        public override Guid Parse(object value)
        {
            return new Guid((string)value);
        }

        public override void SetValue(IDbDataParameter parameter, Guid guid)
        {
            parameter.Value = guid.ToString();
        }
    }
}