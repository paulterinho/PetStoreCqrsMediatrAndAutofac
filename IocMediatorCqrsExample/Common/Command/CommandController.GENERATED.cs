//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.13.1.0 (NJsonSchema v10.5.1.0 (Newtonsoft.Json v11.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

using System.Linq;

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."
#pragma warning disable 8073 // Disable "CS8073 The result of the expression is always 'false' since a value of type 'T' is never equal to 'null' of type 'T?'"
#pragma warning disable 3016 // Disable "CS3016 Arrays as attribute arguments is not CLS-compliant"

namespace Petstore.Swagger.Io.Common.Command
{
    using System = global::System;

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.13.1.0 (NJsonSchema v10.5.1.0 (Newtonsoft.Json v11.0.0.0))")]
    public interface IQueryController
    {
        /// <summary>List all pets</summary>
        /// <param name="limit">How many items to return at one time (max 100)</param>
        /// <returns>A paged array of pets</returns>
        System.Threading.Tasks.Task<SwaggerResponse<System.Collections.Generic.ICollection<Pet>>> PetsGetAsync(int? limit, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
    
        /// <summary>Info for a specific pet</summary>
        /// <param name="petId">The id of the pet to retrieve</param>
        /// <returns>Expected response to a valid request</returns>
        System.Threading.Tasks.Task<SwaggerResponse<System.Collections.Generic.ICollection<Pet>>> PetsGetAsync(string petId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.13.1.0 (NJsonSchema v10.5.1.0 (Newtonsoft.Json v11.0.0.0))")]
    [Microsoft.AspNetCore.Mvc.Route("v1")]
    public partial class QueryController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private IQueryController _implementation;
    
        public QueryController(IQueryController implementation)
        {
            _implementation = implementation;
        }
    
        /// <summary>List all pets</summary>
        /// <param name="limit">How many items to return at one time (max 100)</param>
        /// <returns>A paged array of pets</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("pets", Name = "listPets")]
        public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> PetsGet([Microsoft.AspNetCore.Mvc.FromQuery] int? limit, System.Threading.CancellationToken cancellationToken)
        {
            var result = await _implementation.PetsGetAsync(limit, cancellationToken).ConfigureAwait(false);
    
            var status = result.StatusCode;
            Microsoft.AspNetCore.Mvc.ObjectResult response = new Microsoft.AspNetCore.Mvc.ObjectResult(result.Result) { StatusCode = status };
    
            foreach (var header in result.Headers)
                Request.HttpContext.Response.Headers.Add(header.Key, new Microsoft.Extensions.Primitives.StringValues(header.Value.ToArray()));
    
            return response;
        }
    
        /// <summary>Info for a specific pet</summary>
        /// <param name="petId">The id of the pet to retrieve</param>
        /// <returns>Expected response to a valid request</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("pets/{petId}", Name = "showPetById")]
        public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> PetsGet(string petId, System.Threading.CancellationToken cancellationToken)
        {
            var result = await _implementation.PetsGetAsync(petId, cancellationToken).ConfigureAwait(false);
    
            var status = result.StatusCode;
            Microsoft.AspNetCore.Mvc.ObjectResult response = new Microsoft.AspNetCore.Mvc.ObjectResult(result.Result) { StatusCode = status };
    
            foreach (var header in result.Headers)
                Request.HttpContext.Response.Headers.Add(header.Key, new Microsoft.Extensions.Primitives.StringValues(header.Value.ToArray()));
    
            return response;
        }
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.13.1.0 (NJsonSchema v10.5.1.0 (Newtonsoft.Json v11.0.0.0))")]
    public interface ICommandController
    {
        /// <summary>Create a pet</summary>
        /// <returns>Null response</returns>
        System.Threading.Tasks.Task<SwaggerResponse> PetsAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.13.1.0 (NJsonSchema v10.5.1.0 (Newtonsoft.Json v11.0.0.0))")]
    [Microsoft.AspNetCore.Mvc.Route("v1")]
    public partial class CommandController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private ICommandController _implementation;
    
        public CommandController(ICommandController implementation)
        {
            _implementation = implementation;
        }
    
        /// <summary>Create a pet</summary>
        /// <returns>Null response</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("pets", Name = "createPets")]
        public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> Pets(System.Threading.CancellationToken cancellationToken)
        {
            var result = await _implementation.PetsAsync(cancellationToken).ConfigureAwait(false);
    
            var status = result.StatusCode;
            Microsoft.AspNetCore.Mvc.ObjectResult response = new Microsoft.AspNetCore.Mvc.ObjectResult(result) { StatusCode = status };
    
            foreach (var header in result.Headers)
                Request.HttpContext.Response.Headers.Add(header.Key, new Microsoft.Extensions.Primitives.StringValues(header.Value.ToArray()));
    
            return response;
        }
    
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.5.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Pet 
    {
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
        public long Id { get; set; }
    
        [Newtonsoft.Json.JsonProperty("name", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Name { get; set; }
    
        [Newtonsoft.Json.JsonProperty("tag", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Tag { get; set; }
    
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.5.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Pets : System.Collections.ObjectModel.Collection<Pet>
    {
    
    }
    
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.5.1.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Error 
    {
        [Newtonsoft.Json.JsonProperty("code", Required = Newtonsoft.Json.Required.Always)]
        public int Code { get; set; }
    
        [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Message { get; set; }
    
    
    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.13.1.0 (NJsonSchema v10.5.1.0 (Newtonsoft.Json v11.0.0.0))")]
    public partial class SwaggerResponse
    {
        public int StatusCode { get; private set; }

        public System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> Headers { get; private set; }

        public SwaggerResponse(int statusCode, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers)
        {
            StatusCode = statusCode;
            Headers = headers;
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.13.1.0 (NJsonSchema v10.5.1.0 (Newtonsoft.Json v11.0.0.0))")]
    public partial class SwaggerResponse<TResult> : SwaggerResponse
    {
        public TResult Result { get; private set; }

        public SwaggerResponse(int statusCode, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, TResult result)
            : base(statusCode, headers)
        {
            Result = result;
        }
    }

}

#pragma warning restore 1591
#pragma warning restore 1573
#pragma warning restore  472
#pragma warning restore  114
#pragma warning restore  108
#pragma warning restore 3016