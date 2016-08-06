using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security;
using System.Security.Cryptography;
using System.Globalization;
using System.Text;
using Microsoft.SharePoint.Client;
using System.Configuration;

/// <summary>
/// Summary description for Utilities
/// </summary>
public static class Utilities
{
    public static string decyrpt(string secure)
    {

        // Convert the hex dump to byte array
        int length = secure.Length / 2;
        byte[] encryptedData = new byte[length];
        for (int index = 0; index < length; ++index)
        {
            encryptedData[index] = byte.Parse(secure.Substring(2 * index, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }

        // Decrypt the byte array to Unicode byte array
        byte[] data = ProtectedData.Unprotect(encryptedData, (byte[])null, DataProtectionScope.CurrentUser);

        // Convert Unicode byte array to string
        string decryptedSecure = Encoding.Unicode.GetString(data);

        return decryptedSecure;
    }

    public static SharePointOnlineCredentials sp365credential()
    {
        SecureString secureString = new SecureString();

        string username = @ConfigurationManager.AppSettings["username"];
        string password = @ConfigurationManager.AppSettings["password"];

        string decyrptedPassword = Utilities.decyrpt(password);

        Char[] decyrptedPasswordArrray = decyrptedPassword.ToCharArray();

        foreach (char ch in decyrptedPasswordArrray)
            secureString.AppendChar(ch);

        SharePointOnlineCredentials spCred = new SharePointOnlineCredentials(username, secureString);

        return spCred;
    }

}