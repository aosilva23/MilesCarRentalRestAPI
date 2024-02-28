using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace milescarrental.Application.Helpers
{
    public class Encryptor
    {
        public static TripleDES CrearDES(string clave)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            TripleDES des = new TripleDESCryptoServiceProvider();
            des.Key = md5.ComputeHash(Encoding.Unicode.GetBytes(clave));
            des.IV = new byte[des.BlockSize / 8];
            return des;
        }

        public string EncriptarCadenaDeCaracteres(string textoPlano, string contrasegnia)
        {
            byte[] textoPlanoBytes = Encoding.Unicode.GetBytes(textoPlano);
            MemoryStream flujoMemoria = new MemoryStream();
            TripleDES des = CrearDES(contrasegnia);
            CryptoStream flujoEncriptacion = new CryptoStream(flujoMemoria, des.CreateEncryptor(), CryptoStreamMode.Write);
            flujoEncriptacion.Write(textoPlanoBytes, 0, textoPlanoBytes.Length);
            flujoEncriptacion.FlushFinalBlock();

            return Convert.ToBase64String(flujoMemoria.ToArray());
        }

        public string DesencriptarCadenaDeCaracteres(string textoEncriptado, string contrasegnia)
        {
            byte[] bytesEncriptados = Convert.FromBase64String(textoEncriptado);
            MemoryStream flujoMemoria = new MemoryStream();
            TripleDES des = CrearDES(contrasegnia);
            CryptoStream flujoDesencriptacion = new CryptoStream(flujoMemoria, des.CreateDecryptor(), CryptoStreamMode.Write);
            flujoDesencriptacion.Write(bytesEncriptados, 0, bytesEncriptados.Length);
            flujoDesencriptacion.FlushFinalBlock();

            return Encoding.Unicode.GetString(flujoMemoria.ToArray());
        }
    }
}
