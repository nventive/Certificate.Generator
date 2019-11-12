using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CertificateGenerator.Extensions
{
    internal static class CertificateRequestExtensions
    {
		private const string BeginCertificateRequestLine = "-----BEGIN CERTIFICATE REQUEST-----";
		private const string EndCertificateRequestLine = "-----END CERTIFICATE REQUEST-----";

		private const int ContentLineLength = 64;

		/// <summary>
		/// Gets the PEM-encoded content for the given <see cref="CertificateRequest"/>.
		/// Adapted version of Microsoft code: <see href="https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.x509certificates.certificaterequest.createsigningrequest?view=netframework-4.8#System_Security_Cryptography_X509Certificates_CertificateRequest_CreateSigningRequest" />
		/// </summary>
		/// <param name="request">The certificate request to get the signing request from</param>
		/// <returns>The PEM-encoded signing request</returns>
		public static string GetPemEncodedSigningRequest(this CertificateRequest request)
		{
			var builder = new StringBuilder();

			builder.AppendLine(BeginCertificateRequestLine);

			var content = Convert.ToBase64String(request.CreateSigningRequest());

			int offset = 0;

			while (offset < content.Length)
			{
				int lineEnd = Math.Min(offset + ContentLineLength, content.Length);

				builder.AppendLine(content.Substring(offset, lineEnd - offset));

				offset = lineEnd;
			}

			builder.AppendLine(EndCertificateRequestLine);

			return builder.ToString();
		}
	}
}
