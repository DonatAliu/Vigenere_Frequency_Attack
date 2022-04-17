using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace VigenereDecrypter
{
    public class Program
    {

        public static string alphabet = "abcçdeëfghijklmnopqrstuvxyz";

        static void Main(string[] args)
        {
            byte[] inputBuffer = new byte[1024];
            Stream inputStream = Console.OpenStandardInput(inputBuffer.Length);
            Console.SetIn(new StreamReader(inputStream, Console.InputEncoding, false, inputBuffer.Length));

            // Marrim tekstin shqip pjese pjese dhe gjatesine e celesit
            Console.WriteLine("Shkruaj pjesen e pare te tekstit: ");
            string text_1 = Console.ReadLine();
            Console.WriteLine("Shkruaj pjesen e dyte te tekstit: ");
            string text_2 = Console.ReadLine();
            Console.WriteLine("Shkruaj pjesen e trete te tekstit: ");
            string text_3 = Console.ReadLine();
            string ciphertext = text_1 + text_2 + text_3;


            Console.WriteLine("Shkruaj gjatesine e celesit: ");
            string str_key_length = Console.ReadLine();
            int key_length = Convert.ToInt32(str_key_length);

            string keyletters = findkeyLetters(key_length, ciphertext);
            Console.WriteLine("Celesi: " + keyletters);
            //decrypting with the function we will make in the future 
            string decrypted = decrypt(ciphertext, keyletters);
            Console.WriteLine("Dekriptuar: " + decrypted);
        }
        //funksioni per ta gjete celesin
        public static string findkeyLetters(int shift, string text)
        {
            List<double> big_dotproduct_list = new List<double>();
            //e bejme nje dictionary me shkronjen dhe nje int e cila rritet per ta gjete frekuencen e ciphertext
            Dictionary<string, int> cipherFrequency = new Dictionary<string, int>()

            {
                  {"a", 0},{"ç", 0},{"ë", 0},{"h", 0},{"k", 0},{"n", 0},{"q", 0},{"t", 0},{"x", 0},
                  {"b", 0},{"d", 0},{"f", 0},{"i", 0},{"l", 0},{"o", 0},{"r", 0},{"u", 0},{"y", 0},
                  {"c", 0},{"e", 0},{"g", 0},{"j", 0},{"m", 0},{"p", 0},{"s", 0},{"v", 0},{"z", 0}

            };
            for (int off = 0; off <= shift - 1; off++)
            {

                for (int i = off; i < text.Length; i += shift)
                {
                    if (cipherFrequency.ContainsKey(text[i].ToString()))
                    {
                        cipherFrequency[text[i].ToString()]++;
                    }

                }

                int sum = 0;
                List<double> dot_product_list = new List<double>();

                var vector = cipherFrequency.OrderBy(key => key.Key);
                List<double> w = new List<double>();

                foreach (var x in vector)
                {
                    sum += cipherFrequency[x.Key];
                }

                foreach (var x in vector)
                {
                    if (cipherFrequency[x.Key] == 0)
                    {
                        w.Add(0);
                    }
                    else
                    {
                        w.Add((Convert.ToDouble(cipherFrequency[x.Key]) / sum));
                    }
                }

                sum = 0;
                double dot_product = 0;
                cipherFrequency = cipherFrequency.ToDictionary(p => p.Key, p => 0);

                for (int j = 0; j <= 26; j++)
                {
                    double[] x = new double[] { 0.06853, 0.01062, 0.00417, 0.0024, 0.03186, 0.09675, 0.09705, 0.00892, 0.01386, 0.04658, 0.07444, 0.03502, 0.03508, 0.02547, 0.03733, 0.06227, 0.03673, 0.02958, 0.00905, 0.06883, 0.057, 0.08656, 0.03462, 0.01356, 0.00075, 0.00621, 0.00625 }; //frekuenca
                    // use shiftRight function to shift 
                    var a_j = ShiftRight(x, j);

                    for (int k = 0; k <= 26; k++)
                    {
                        dot_product += (w[k] * a_j.ElementAt(k));
                    }

                    dot_product_list.Add(Math.Round(dot_product, 4));
                    dot_product = 0;

                }
                big_dotproduct_list.Add(dot_product_list.IndexOf(dot_product_list.Max()));
            }

            string out_str = "";

            big_dotproduct_list.ForEach(x => out_str += alphabet[(int)x]);

            return out_str;

        }
        public static int MathMod(int a, int b)
        {
            int c = ((a % b) + b) % b;
            return c;
        }
      public static IEnumerable<T> ShiftRight<T>(IList<T> values, int shift)
        {
            for (int index = 0; index < values.Count; index++)
            {
                yield return values[MathMod(index - shift, values.Count)];
            }
        }

    }
}
