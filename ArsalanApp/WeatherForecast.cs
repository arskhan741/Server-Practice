using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace ArsalanApp
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }

        [Benchmark]
        public async Task SomeTask(float delay)
        {
            //BenchmarkRunner.Run<WeatherForecast>();
            await Task.Delay((int)delay);
        }
    }
}
