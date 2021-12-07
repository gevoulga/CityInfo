using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    //https://code-maze.com/csharp-async-enumerable-yield/
    //https://www.tpeczek.com/2021/07/aspnet-core-6-and-iasyncenumerable.html
    
    [ApiController]
    [Route("api/[controller]")]
    public class LoremIpsumController : ControllerBase
    {
        private static readonly string[] Words = {"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
            "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
            "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};

        [HttpGet] //Task<IActionResult>
        public async Task<IActionResult> GetLoremIpsum()
        {
            return Ok(generate());
        }

        private async IAsyncEnumerable<string> generate()
        {
            var rand = new Random();
            int numWords = rand.Next(Words.Length) + 1;

            for (int i = 0; i < numWords; i++)
            {
                var word = Words[rand.Next(Words.Length)];
                yield return word;
                await Task.Delay(500);
            }
        }


        // static string LoremIpsum(int minWords, int maxWords,
        //     int minSentences, int maxSentences,
        //     int numParagraphs) {
        //
        //     var words = new[]{"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
        //         "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
        //         "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};
        //
        //     var rand = new Random();
        //     int numSentences = rand.Next(maxSentences - minSentences)
        //                        + minSentences + 1;
        //     int numWords = rand.Next(maxWords - minWords) + minWords + 1;
        //
        //     StringBuilder result = new StringBuilder();
        //
        //     for(int p = 0; p < numParagraphs; p++) {
        //         result.Append("<p>");
        //         for(int s = 0; s < numSentences; s++) {
        //             for(int w = 0; w < numWords; w++) {
        //                 if (w > 0) { result.Append(" "); }
        //                 result.Append(words[rand.Next(words.Length)]);
        //             }
        //             result.Append(". ");
        //         }
        //         result.Append("</p>");
        //     }
        //
        //     return result.ToString();
        // }
    }
}