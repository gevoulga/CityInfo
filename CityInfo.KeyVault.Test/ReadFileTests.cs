using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CityInfo.KeyVault.Test;

public class ReadFileTests
{
    private const string filename = @"task1762271.txt";

    [Test]
    public async Task ReadFile()
    {
        await foreach (var (user, pass, puid, lineNumber) in AsyncReadLines())
        {
            JsonObject item = new JsonObject();
            item.Add("UserName", user);
            item.Add("Password", pass);
            item.Add("Puid", puid);

            item.TryGetPropertyValue("UserName", out var uu);
            item.TryGetPropertyValue("Password", out var pp);
            item.TryGetPropertyValue("Puid", out var pu);
            TestContext.Progress.WriteLine($"{uu} -> {pp} -> {pu}");
            // to do with current text line
            // ...
        }
    }

    private async IAsyncEnumerable<(string user, string pass, string puid, int lineNumber)> AsyncReadLines()
    {
        using var reader = File.OpenText(filename);
        var lineNumber = -1;
        while (!reader.EndOfStream)
        {
            lineNumber++;
            var line = await reader.ReadLineAsync();
            var strings = line?.Split(",", 3);
            var user = strings?[0] ?? throw new ArgumentException(line);
            var pass = strings?[1] ?? throw new ArgumentException(line);
            var puid = strings?[2] ?? throw new ArgumentException(line);
            yield return (user, pass, puid, lineNumber);
        }
    }
}