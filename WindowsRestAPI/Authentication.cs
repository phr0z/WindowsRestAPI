using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Grapevine.Interfaces.Server;
using Newtonsoft.Json;
using WindowsRestAPI.Properties;
using System.Security;
using System.Security.Cryptography;

namespace WindowsRestAPI
{
    class Authentication
    {
        public static bool Enabled = Settings.Default.Authentication;
        public static string Username = Settings.Default.Username;
        public static string Password = Settings.Default.Password;
        public static ResponseMessage FailedAuthentication = new ResponseMessage { Message = "Failed to authenticate.", Successful = false };
        public static bool VerifyAuthentication(HttpListenerBasicIdentity identity)
        {
            if (identity.IsAuthenticated && identity.Name == Username && identity.Password == ToInsecureString(DecryptString(Password)))
            {
                return true;
            }
            else return false;
        }

        //https://stackoverflow.com/a/62018445
        static byte[] entropy = Encoding.Unicode.GetBytes("231db16d4e3d41458a7a3694566607e4");
        public static string EncryptString(SecureString input)
        {
            byte[] encryptedData = ProtectedData.Protect(Encoding.Unicode.GetBytes(ToInsecureString(input)), entropy, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }
        public static SecureString DecryptString(string encryptedData)
        {
            try
            {
                byte[] decryptedData = ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), entropy, DataProtectionScope.CurrentUser);
                return ToSecureString(Encoding.Unicode.GetString(decryptedData));
            }
            catch
            {
                return new SecureString();
            }
        }
        public static SecureString ToSecureString(string input)
        {
            SecureString secure = new SecureString();
            foreach (char c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }
        public static string ToInsecureString(SecureString input)
        {
            string returnValue = string.Empty;
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input);
            try
            {
                returnValue = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
            return returnValue;
        }
    }
}
