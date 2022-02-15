using System;
using System.Threading.Tasks;
using Azure.Security.KeyVault.Secrets;
using NUnit.Framework;

namespace CityInfo.KeyVault.Test;

public class SecretTests
{
    private SecretClient secretClient;

    [SetUp]
    public void Setup()
    {
        secretClient = SetupNamespace.KeyVaultSecretClient;
        TestContext.Progress.WriteLine("Test Setup");
    }
    
    [Test]
    public async Task GetSecretProperties()
    {
        var startIndex = 1032;
            var secretName = $"mstest-tfl-int-{startIndex}";
            
            TestContext.Progress.WriteLine($"[{DateTime.UtcNow}] >>> SET KeyVault secret '{secretName}' to ");
            var response = secretClient.GetPropertiesOfSecrets();
            foreach (var secretProps in response)
            {
                TestContext.Progress.WriteLine(
                    $"[{DateTime.UtcNow}] <<< Secret props '{secretProps}'");
            }
    }

    [Test]
    public async Task ReadExistingSecret()
    {
        var secretName = "test-secret";
        TestContext.Progress.WriteLine($"[{DateTime.UtcNow}] >>> GET KeyVault secret '{secretName}'");
        var response = await secretClient.GetSecretAsync(secretName);
        var secretValue = response.Value.Value;
        TestContext.Progress.WriteLine(
            $"[{DateTime.UtcNow}] <<< Successfully received secret '{secretName}' with value '{secretValue}'");

        Assert.Pass();
    }

    [Test]
    public async Task Test_1_SetSecret()
    {
        var secretName = "test-secret-from-test-runner";
        var secret = new KeyVaultSecret(secretName, "test-secret-from-test-runner-value");

        TestContext.Progress.WriteLine($"[{DateTime.UtcNow}] >>> SET KeyVault secret '{secretName}'");
        var response = await secretClient.SetSecretAsync(secret);
        var secretValue = response.Value.Value;
        TestContext.Progress.WriteLine(
            $"[{DateTime.UtcNow}] <<< Successfully set secret '{secretName}' with value '{secretValue}'");

        Assert.Pass();
    }

    [Test]
    public async Task Test_2_DeleteSecret()
    {
        var secretName = "test-secret-from-test-runner";
        TestContext.Progress.WriteLine($"[{DateTime.UtcNow}] >>> DELETE KeyVault secret '{secretName}'");
        var response = await secretClient.StartDeleteSecretAsync(secretName);
        var deleted = await response.WaitForCompletionAsync();
        TestContext.Progress.WriteLine(
            $"[{DateTime.UtcNow}] <<< Successfully delete secret '{secretName}' at '{deleted.Value.DeletedOn}'");
        var purged = await secretClient.PurgeDeletedSecretAsync(secretName);
        TestContext.Progress.WriteLine($"[{DateTime.UtcNow}] <<< Successfully purged secret '{secretName}' ");

        Assert.Pass();
    }
}