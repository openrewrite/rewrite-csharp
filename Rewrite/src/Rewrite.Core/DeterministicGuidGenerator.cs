using System.Security.Cryptography;
using System.Text;

namespace Rewrite.Core;

public static class DeterministicGuidGenerator
{
    /// <summary>
    /// Generates a deterministic GUID from a given input string using SHA-1.
    /// </summary>
    /// <param name="input">The input string to hash.</param>
    /// <returns>A deterministic GUID derived from the input string.</returns>
    public static Guid FromString(string input)
    {
        if (input == null)
            throw new ArgumentNullException(nameof(input));

        using var sha1 = SHA1.Create();
        byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Use the first 16 bytes of the hash to create the GUID
        byte[] guidBytes = new byte[16];
        Array.Copy(hash, guidBytes, 16);

        // Set version to 5 (name-based SHA-1, per RFC 4122)
        guidBytes[6] = (byte)((guidBytes[6] & 0x0F) | (5 << 4));
        // Set variant to RFC 4122
        guidBytes[8] = (byte)((guidBytes[8] & 0x3F) | 0x80);

        return new Guid(guidBytes);
    }
}