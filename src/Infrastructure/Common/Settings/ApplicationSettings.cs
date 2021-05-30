using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common.Settings
{
	public class ApplicationSettings : IValidatable
	{
		[Required]
		public string NpgSqlConnection { get; set; }

		[Required]
		public string NpgSqlPassword { get; set; }

		[Required]
		public string RedisServer { get; set; }

		[Required]
		[Url]
		public string ElasticSearchServer { get; set; }

		public void Validate()
		{
			Validator.ValidateObject(this, new ValidationContext(this), validateAllProperties: true);
		}
	}
}
