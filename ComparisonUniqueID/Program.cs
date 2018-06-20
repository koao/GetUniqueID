using System;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace ComparisonUniqueID
{
    class Program
    {
        static void Main(string[] args)
        {
            string pass = "hoge";
            string hash = GetMD5HashString(pass + GetVolumeSerials());
            Console.WriteLine(hash);

            string filePath = @"c:\aky.hsuid";

            if(System.IO.File.Exists(filePath))
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(filePath);
                string s = sr.ReadToEnd();
                sr.Close();

                if (hash == s)
                {
                    Console.WriteLine("match");
                }
                else
                {
                    Console.WriteLine("mismatch");
                }
            }
            else
            {
                Console.WriteLine("File does not exist");
            }
        }

        /// <summary>
        /// ストレージのシリアルナンバーを取得する。複数ある場合は連結して返す。
        /// </summary>
        /// <returns>シリアルナンバー</returns>
        public static string GetVolumeSerials()
        {
            var volumes = new ManagementClass("Win32_DiskDrive").GetInstances();

            string str = "";
            foreach (var volume in volumes)
            {
                str += volume["SerialNumber"].ToString();
            }
            return str;
        }

        /// <summary>
        /// ハッシュ値を計算し、返す。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetMD5HashString(string text)
        {
            // 文字列をバイト型配列に変換する
            byte[] data = Encoding.UTF8.GetBytes(text);

            // MD5ハッシュアルゴリズム生成
            var algorithm = new MD5CryptoServiceProvider();

            // ハッシュ値を計算する
            byte[] bs = algorithm.ComputeHash(data);

            // リソースを解放する
            algorithm.Clear();

            // バイト型配列を16進数文字列に変換
            var result = new StringBuilder();
            foreach (byte b in bs)
            {
                result.Append(b.ToString("X2"));
            }
            return result.ToString();
        }
    }
}
