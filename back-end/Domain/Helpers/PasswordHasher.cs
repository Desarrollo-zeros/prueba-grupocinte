using Microsoft.AspNetCore.Identity;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;


//ref : https://www.meziantou.net/how-to-store-a-password-in-a-web-application.htm
//ref : https://security.stackexchange.com/questions/133239/what-is-the-specific-reason-to-prefer-bcrypt-or-pbkdf2-over-sha256-crypt-in-pass
//ref : https://medium.com/@mpreziuso/password-hashing-pbkdf2-scrypt-bcrypt-1ef4bb9c19b3
public class PasswordHasher
{
    public byte Version { set; get; } = 1;
    public int SaltSize { get; } = 128 / 8; // 128 bits
    public HashAlgorithmName HashAlgorithmName { get; } = HashAlgorithmName.SHA256;



    public string HashPassword(string password)
    {
        if (password == null)
            throw new ArgumentNullException(nameof(password));

        byte[] salt = GenerateSalt(SaltSize);
        byte[] hash = HashPasswordWithSalt(password, salt);

        var inArray = new byte[1 + SaltSize + hash.Length];
        inArray[0] = Version;
        Buffer.BlockCopy(salt, 0, inArray, 1, SaltSize);
        Buffer.BlockCopy(hash, 0, inArray, 1 + SaltSize, hash.Length);

        return Convert.ToBase64String(inArray);
    }

    public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string password)
    {
        if (password == null)
            return PasswordVerificationResult.Failed;

        if (hashedPassword == null)
            return PasswordVerificationResult.Failed;

        Span<byte> numArray = Convert.FromBase64String(hashedPassword);
        if (numArray.Length < 1)
            return PasswordVerificationResult.Failed;

        byte version = numArray[0];
        if (version > Version)
            return PasswordVerificationResult.SuccessRehashNeeded;

        var salt = numArray.Slice(1, SaltSize).ToArray();
        var bytes = numArray.Slice(1 + SaltSize).ToArray();

        var hash = HashPasswordWithSalt(password, salt);

        if (FixedTimeEquals(hash, bytes))
            return PasswordVerificationResult.Success;

        return PasswordVerificationResult.Failed;
    }




    private byte[] HashPasswordWithSalt(string password, byte[] salt)
    {
        byte[] hash;
        using (var hashAlgorithm = HashAlgorithm.Create(HashAlgorithmName.Name))
        {
            byte[] input = Encoding.UTF8.GetBytes(password);
            hashAlgorithm.TransformBlock(salt, 0, salt.Length, salt, 0);
            hashAlgorithm.TransformFinalBlock(input, 0, input.Length);
            hash = hashAlgorithm.Hash;
        }

        return hash;
    }

    //ref : https://es.wikipedia.org/wiki/Sal_(criptografía)

    private static byte[] GenerateSalt(int byteLength)
    {
        using var cryptoServiceProvider = new RNGCryptoServiceProvider();
        var data = new byte[byteLength];
        cryptoServiceProvider.GetBytes(data);
        return data;
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static bool FixedTimeEquals(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
    {
        // NoOptimization because we want this method to be exactly as non-short-circuiting
        // as written.
        //
        // NoInlining because the NoOptimization would get lost if the method got inlined.

        if (left.Length != right.Length)
        {
            return false;
        }

        int length = left.Length;
        int accum = 0;

        for (int i = 0; i < length; i++)
        {
            accum |= left[i] - right[i];
        }

        return accum == 0;
    }


}