using System.Security.Cryptography;
using System.Text;

namespace InvoiceDesigner.Application.Authorization
{
	public static class UserPaswordHasher
	{
		private const int keySize = 64;
		private const int iterations = 10000;
		private static HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

		public static (string, string) CreateHash(string password)
		{
			byte[] salt = RandomNumberGenerator.GetBytes(keySize);

			var hash = Rfc2898DeriveBytes.Pbkdf2(
				Encoding.UTF8.GetBytes(password),
				salt,
				iterations,
				hashAlgorithm,
				keySize);

			return (Convert.ToHexString(hash), Convert.ToHexString(salt));
		}

		public static bool VerifyPassword(string password, string hash, string salt)
		{
			byte[] saltBytes = Convert.FromHexString(salt);

			var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
				password,
				saltBytes,
				iterations,
				hashAlgorithm,
				keySize);

			return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
		}
	}
}
