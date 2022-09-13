using System;
using Infosys.EncryptDecrypt;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine($"Data Source Cipher Text :{Repository.Encrypt("(localdb)\\MSSQLLocalDB","ServerKey")}");
            //Console.WriteLine($"Database Cipher Text :{Repository.Encrypt("QuickKartDB","DatabaseKey")}");

            //Console.WriteLine(Repository.Decrypt("YzAcDCIyIX0sakQ6ZkBQHRcSdBASOw5sgGASMyEYYWcHOUZE4PTYEZ+qZ1D6b9t/pIqmr7JLhGQpoJ149lhZrA==", "ServerKey"));

            int a = 10, b = 20;

            Console.WriteLine("A :{0}\tB :{1}", a, b);
        }
    }
}
