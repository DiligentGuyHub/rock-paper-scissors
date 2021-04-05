using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace rock_paper_scissors
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length % 2 == 1 && args.Length >= 3 && args.Distinct().Count() == args.Count())
            {
                // 1. Generate safe 128-bit key
                byte[] key = new byte[128];
                using (var random = new RNGCryptoServiceProvider())
                {
                    random.GetBytes(key);
                }
                // 2. Computer move
                int computer = RandomNumberGenerator.GetInt32(0, args.Length);
                // 3. Calculate HMAC
                var HMAC = HashMessage(key, StringEncode(args[computer]));
                Console.WriteLine($"\nHMAC: {HashEncode(HMAC)}\n");
                // 4. User move
                int answer = 0;
                do
                {
                    Console.WriteLine("Available moves:");
                    foreach(var arg in args)
                    {
                        Console.WriteLine($"{Array.IndexOf(args, arg) + 1} - {arg}");
                    }
                    Console.Write("0 - Exit\nEnter your move: ");
                    answer = Convert.ToInt32(Console.ReadLine());
                }
                while (answer >= args.Length + 1);
                Console.WriteLine($"Your move: {args[answer - 1]}");
                Console.WriteLine($"Computer move: {args[computer]}");
                // 5. Show HMAC
                Console.WriteLine($"\nHMAC key: {HashEncode(key)}\n");
                
            }
            else
            {
                Console.WriteLine(
                    "\nIn order to play you need to follow the rules:\n" +
                    "1) The number of parameters must be greater than or equal to 3\n" +
                    "2) The number of parameters must be odd\n" +
                    "3) The parameters must not repeat each other\n\n" +
                    "Try the classics: rock-paper-scissors.exe rock paper scissors"
                    );
            }
        }

        private static byte[] HashMessage(byte[] key, byte[] message)
        {
            var hash = new HMACSHA256(key);
            return hash.ComputeHash(message);
        }
        private static byte[] StringEncode(string text)
        {
            var encoding = new ASCIIEncoding();
            return encoding.GetBytes(text);
        }
        private static string HashEncode(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
