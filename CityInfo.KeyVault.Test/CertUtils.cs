using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace CityInfo.KeyVault.Test
{
    public static class CertUtils
    {
        public static X509Certificate2 GetAadCert(string subjectName)
        {
            using var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly); //We need to open the certificate store before we get the certificates!
            var foundCert = store.Certificates
                .Find(X509FindType.FindBySubjectName, subjectName, validOnly: false)
                .OrderBy(certificate2 => Convert.ToDateTime(certificate2.GetEffectiveDateString()))
                .FirstOrDefault();
            return foundCert ?? throw new ArgumentException(
                $"No valid cert with subject/CN {subjectName} found in LocalMachine/My store");
        }
    }
}