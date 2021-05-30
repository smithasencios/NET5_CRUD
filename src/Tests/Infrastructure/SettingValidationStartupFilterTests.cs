using Infrastructure.Common.Settings;
using Microsoft.AspNetCore.Builder;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Tests.Infrastructure
{
	public class SettingValidationStartupFilterTests
	{
        [Fact]
        public void Configure_WithValidIValidatableInstances_DoesntThrowException()
        {
            // Arrange
            var mocked = new Mock<Action<IApplicationBuilder>>();
            var validatables = new List<IValidatable>
            {
                new ApplicationSettings()
                {
                    ElasticSearchServer = "http://test:23",
                    NpgSqlConnection = "connection",
                    NpgSqlPassword = "password",
                    RedisServer = "redis",
                },
            };

            var settingValidationStartupFilter = new SettingValidationStartupFilter(validatables);

            // Act
            var exception = Record.Exception(() => settingValidationStartupFilter.Configure(mocked.Object));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void Configure_WithinValidIValidatableInstances_DoesntThrowException()
        {
            // Arrange
            var mocked = new Mock<Action<IApplicationBuilder>>();
            var validatables = new List<IValidatable>
            {
                new ApplicationSettings(),
            };

            var settingValidationStartupFilter = new SettingValidationStartupFilter(validatables);

            // Act
            // Assert
            Assert.Throws<ValidationException>(() => settingValidationStartupFilter.Configure(mocked.Object));
        }
    }
}
