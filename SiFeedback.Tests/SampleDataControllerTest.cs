using SiFeedback.Web.Controllers;
using System;
using System.Collections.Generic;
using Xunit;

namespace SiFeedback.Tests
{
    public class SampleDataControllerTest
    {
        [Fact]
        public void GetSimpleData_ShouldNotBeEmpty()
        {
            SampleDataController controller = new SampleDataController();

            IEnumerable<SampleDataController.WeatherForecast> data = controller.WeatherForecasts();

            Assert.NotNull(data);
        }
    }
}
