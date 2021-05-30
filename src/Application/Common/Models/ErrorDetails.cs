using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Application.Common.Models
{
	public class ErrorDetails
	{
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                },
                Formatting = Formatting.Indented,
            });
        }
    }
}
