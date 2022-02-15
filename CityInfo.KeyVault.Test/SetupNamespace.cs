using System;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using NUnit.Framework;

namespace CityInfo.KeyVault.Test;

[SetUpFixture]
public class SetupNamespace
{
    // public TestContext TestContext { get; set; }

    public static SecretClient KeyVaultSecretClient { get; private set; }

    [OneTimeSetUp]
    public static void SetupForNamespace()
    {
        var source =  TestContext.Parameters;
        var keyVaultUrl = new Uri(source["KeyVaultUrl"] as string);
        var credential = TokenCredential(source);
        KeyVaultSecretClient = new SecretClient(keyVaultUrl, credential);
        TestContext.Progress.WriteLine("Initialized Secret Client");
    }

    private static TokenCredential TokenCredential(TestParameters source)
    {
        var testClientAadCertificate = source["CN"] as string;
        var tenantId = source["KeyVaultTenantId"] as string;
        var clientId = source["ClientId"] as string;
        if (string.IsNullOrWhiteSpace(testClientAadCertificate))
        {
            TestContext.Progress.WriteLine("Using Default Azure authentication for KeyVault");
            return new DefaultAzureCredential();
            // return new InteractiveBrowserCredential(tenantId, clientId);
        }

        TestContext.Progress.WriteLine("Using certificate authentication for KeyVault");
        var aadCert = CertUtils.GetAadCert(testClientAadCertificate);
        var certOptions = new ClientCertificateCredentialOptions {SendCertificateChain = true};
        return new ClientCertificateCredential(tenantId, clientId, aadCert, certOptions);
    }

    [OneTimeTearDown]
    public static void TeardownForNamespace()
    {
        TestContext.Progress.WriteLine("Tearing Down Secret Client");
    }
}