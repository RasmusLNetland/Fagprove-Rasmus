using System.Security.Cryptography;
using System.Text;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace ShopAppApi.Auxiliary;

public static class CredentialsUtils
{
    public static string CreatePasswordHash( string password, string salt )
    {
        string saltAndPwd = string.Concat( password, salt );
        MD5 md5Algorithm = MD5.Create();
        string hashedPwd = BinaryToHex( md5Algorithm.ComputeHash( Encoding.UTF8.GetBytes( saltAndPwd ) ) ) ?? string.Empty;
        return hashedPwd;
    }

    public static string CreatePasswordHash( string password, out string passwordSalt )
    {
        byte[] salt = RandomNumberGenerator.GetBytes( 24 );
        passwordSalt = Convert.ToBase64String( salt );
        string hash = CreatePasswordHash( password, passwordSalt );
        return hash;
    }

    private static string? BinaryToHex( byte[]? data )
    {
        if( data is null )
            return null;

        char[] hex = new char[checked(data.Length * 2)];
        for( int i = 0; i < data.Length; ++i )
        {
            byte num = data[i];
            hex[2 * i] = NibbleToHex( (byte)(num >> 4) );
            hex[2 * i + 1] = NibbleToHex( (byte)(num & 0xF) );
        }

        return new string( hex );
    }

    private static char NibbleToHex( byte nibble )
    {
        int aChar = nibble < 10 ? nibble + '0' : nibble - 10 + 'A';
        return (char)aChar;
    }
}