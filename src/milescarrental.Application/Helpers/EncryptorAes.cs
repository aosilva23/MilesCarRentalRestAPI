using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace milescarrental.Application.Helpers
{
    public class EncryptorAes
    {
        public Aes CrearDES(string clave)
        {
            byte[] key;
            using (SHA256 mySHA256 = SHA256.Create())
            {
                key = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(clave));
            }

            Aes myAes = Aes.Create();
            myAes.KeySize = 256;
            myAes.Padding = PaddingMode.PKCS7;
            myAes.Key = key;
            myAes.IV = new byte[myAes.BlockSize / 8];
            return myAes;
        }

        public string EncriptarCadenaDeCaracteres(string claveUsuario, string vector)
        {
            byte[] textoPlanoBytes = Encoding.UTF8.GetBytes(claveUsuario);
            MemoryStream flujoMemoria = new MemoryStream();
            using (Aes des = CrearDES(vector))
            {
                CryptoStream flujoEncriptacion = new CryptoStream(flujoMemoria, des.CreateEncryptor(), CryptoStreamMode.Write);
                flujoEncriptacion.Write(textoPlanoBytes, 0, textoPlanoBytes.Length);
                flujoEncriptacion.FlushFinalBlock();
            }
            return Convert.ToBase64String(flujoMemoria.ToArray());
        }

        public string DesencriptarCadenaDeCaracteres(string claveUsuario, string vector)
        {
            byte[] bytesEncriptados = Convert.FromBase64String(claveUsuario);
            MemoryStream flujoMemoria = new MemoryStream();
            using (Aes des = CrearDES(vector))
            {
                CryptoStream flujoDesencriptacion = new CryptoStream(flujoMemoria, des.CreateDecryptor(), CryptoStreamMode.Write);
                flujoDesencriptacion.Write(bytesEncriptados, 0, bytesEncriptados.Length);

                try
                {
                    flujoDesencriptacion.FlushFinalBlock();
                }
                catch(Exception ex)
                {
                    string mensajeRaro = ex.ToString();
                    mensajeRaro = ex.ToString();
                }
                
            }
            return Encoding.UTF8.GetString(flujoMemoria.ToArray());
        }
    }
}
