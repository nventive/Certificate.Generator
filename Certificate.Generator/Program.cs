using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using CertificateGenerator.Extensions;

namespace CertificateGenerator
{
	public static class Program
	{
		private static Lazy<string> _temporaryDirectoryPath = new Lazy<string>(CreateTemporaryDirectory);

		/// <summary>
		/// Helper tool to create Apple certificates.
		/// This program creates a certificate signing request to be used in the Apple Developer console to obtain a certificate. 
		/// A P12 is then generated using this certificate.
		/// </summary>
		/// <param name="emailAddress">A relevant email address to use in the request (ex: the mail of the owner of the account)</param>
		/// <param name="commonName">The name to use in the request; usually the name of the Apple Developer organization</param>
		/// <param name="countryCode">The country code of the account</param>
		/// <param name="requestFilePath">Optional: the path where to place the .csr file</param>
		/// <param name="p12FilePath">Optional: the path where to place the .p12 file</param>
		/// <returns></returns>
		public static int Main(
			string emailAddress,
			string commonName,
			string countryCode,
			string requestFilePath = null,
			string p12FilePath = null
		)
		{
			//Use temporary files in nothing is specified
			requestFilePath = requestFilePath ?? GetTemporaryFilePath("csr");
			p12FilePath = p12FilePath ?? GetTemporaryFilePath("p12");

			// Generate a new key
			var key = RSA.Create(2048);

			//Create the CSR
			var request = CreateCertificateRequest(key, emailAddress, commonName, countryCode);
			var requestContent = request.GetPemEncodedSigningRequest();

			//Save the CSR
			File.WriteAllText(requestFilePath, requestContent);

			Console.WriteLine($"Created {requestFilePath} to use to create a certificate.");
			
			var certificateFilePath = ReadLine.Read("Location of the .cer file:");

			//Load the certificate and include the private key
			var certificate = new X509Certificate2(X509Certificate.CreateFromCertFile(certificateFilePath)).CopyWithPrivateKey(key);

			var password = ReadLine.ReadPassword("Enter the password to use for the p12:");

			//Export to a p12
			var content = certificate.Export(X509ContentType.Pkcs12, password);

			//Save the p12
			File.WriteAllBytes(p12FilePath, content);

			Console.WriteLine($"Successfully exported certificate under {p12FilePath}");

			return 0;
		}
		
		private static CertificateRequest CreateCertificateRequest(RSA key, string emailAddress, string commonName, string countryCode)
		{
			var request = new CertificateRequest($"CN={commonName}, C={countryCode}", key, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

			//The email address needs to be added to the request as an alternative name
			var alternativeNameBuilder = new SubjectAlternativeNameBuilder();

			alternativeNameBuilder.AddEmailAddress(emailAddress);

			request.CertificateExtensions.Add(alternativeNameBuilder.Build());

			return request;
		}

		private static string CreateTemporaryDirectory() => Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), $"CertificateGenerator.{Guid.NewGuid()}")).FullName;

		private static string GetTemporaryFilePath(string extension) => Path.Combine(_temporaryDirectoryPath.Value, $"{Guid.NewGuid()}.{extension}");
	}
}
