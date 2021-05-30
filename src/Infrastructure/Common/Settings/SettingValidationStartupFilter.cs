using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;

namespace Infrastructure.Common.Settings
{
	public class SettingValidationStartupFilter : IStartupFilter
    {
        private readonly IEnumerable<IValidatable> validatableObjects;

        public SettingValidationStartupFilter(IEnumerable<IValidatable> validatableObjects)
        {
            this.validatableObjects = validatableObjects;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            foreach (var validatableObject in this.validatableObjects)
            {
                validatableObject.Validate();
            }

            return next;
        }
    }
}
