using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TickController : ControllerBase
    {
        private readonly ILogger<TickController> _logger;

        public TickController(ILogger<TickController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet] //Task<IActionResult>
        public async Task<IActionResult> GetTicks()
        {
            var result = Observable.Interval(TimeSpan.FromSeconds(1))
                .Select(l => l.ToString())
                // .SelectMany(async s => await Check() ? s : Observable.Empty())
                // .Merge(2)
                .Take(5);
            // _logger.LogInformation(message: "Ticks: {Result}", result);
            var task = result.ToAsyncEnumerable();
            return Ok(task);
        }

        private async Task<bool> Check()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            return new Random().Next(1) == 0;
        }

        private void CreateParallelRxStream()
        {
            var res = Observable.Range(1, 10)
                .Select(x => TaskCreator(x).ToObservable())
                //Executed out-of-order tasks
                .Merge(3) //If no args, no max-parallelism is specified. 
                //.Concat() //Concat will execute the stream in-order
                .Do(x => Console.WriteLine($"{x} concatenated"));
        }

        private Task<int> TaskCreator(int i)
        {
            throw new NotImplementedException();
        }
    }
}