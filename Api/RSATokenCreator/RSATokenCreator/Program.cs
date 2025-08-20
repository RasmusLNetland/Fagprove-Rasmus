using System.Security.Cryptography;

namespace RSATokenCreator
{
    // TOOL FOR GENERATING RSA KEY USED IN API
    class Program
    {
        static void Main()
        {
            using (RSA rsa = RSA.Create(4096)) // 4096-bit key is strong
            {
                // Private + public key
                string privateKeyXml = rsa.ToXmlString(true);

                // Public key only (for verification)
                string publicKeyXml = rsa.ToXmlString(false);

                Console.WriteLine("Private key (FullTokenKey):");
                Console.WriteLine(privateKeyXml);
                Console.WriteLine("\nPublic key (for verification):");
                Console.WriteLine(publicKeyXml);
            }
        }
    }
}
