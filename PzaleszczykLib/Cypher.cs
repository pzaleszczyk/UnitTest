using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PzaleszczykLib
{
    public class Cypher
    {

        public String[] A_analyzeAll(String text)
        {
            int[] primes = { 1, 3, 5, 7, 9, 11, 15, 17, 19, 21, 23, 25 };
            int index = 0;
            String[] result = new String[312];
            for (int i = 0; i < 26; i++)
            {
                foreach (int prime in primes)
                    result[index++] = A_decrypt(text, prime, i);
            }
            return result;
        }

        public int Inverse(int value)
        {
            value %= 26;
            for (int x = 1; x < 26; x++)
            {
                if ((value * x) % 26 == 1)
                {
                    return x;
                }
            }
            return -1;
        }

        public String[] A_analyze(String extra, String text)
        {
            int[] primes = { 1, 3, 5, 7, 9, 11, 15, 17, 19, 21, 23, 25 };
            int key_a = -1;
            int key_b = -1;
            String result = "";

            for (int i = 0; i < 26; i++)
            {
                foreach (int prime in primes)
                {
                    String decrypted = A_decrypt(text, prime, i);
                    if (decrypted.Contains(extra))
                    {
                        key_b = i;
                        key_a = prime;
                        result = decrypted;
                        break;
                    }
                }
            }
            if (key_a > 0)
            {
                return new String[] { key_a + "", key_b + "", result };
            }
            else
            {
                throw new Exception("ERROR: Nie da sie znalesc klucza!");
            }
        }


        public String A_decrypt(String text, int a, int b)
        {
            int[] result = new int[text.Length];
            char[] characters = new char[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                //Polskie znaki przepisuje
                if ((int)text[i] > 260)
                {
                    result[i] = text[i];
                }

                else if (Char.IsLower(text[i]))
                {
                    result[i] = (text[i] - 'a' - b % 26);
                    while (result[i] % a != 0 || result[i] < 0)
                    {
                        result[i] = (result[i] + 26);
                    }
                    result[i] = ('a' + (result[i] / a) % 26);
                }
                else if (Char.IsUpper(text[i]))
                {
                    result[i] = (text[i] - 'A' - b % 26);
                    while (result[i] % a != 0)
                    {
                        result[i] = (result[i] + 26);
                    }
                    result[i] = ('A' + (result[i] / a) % 26);
                }
                else
                    result[i] = text[i];
                characters[i] = (char)result[i];
            }
            return new String(characters);
        }

        public int GCD(int left, int right)
        {
            return (int)BigInteger.GreatestCommonDivisor(BigInteger.Parse("" + left), BigInteger.Parse("" + right));
        }

        public String A_crypt(String text, int a, int b)
        {
            if (GCD(26, a) != 1 || Inverse(a) == -1)
            {
                throw new Exception("ERROR: Niepoprawny format klucza");
            }

            int[] result = new int[text.Length];
            char[] characters = new char[text.Length];

            for (int i = 0; i < text.Length; i++)
            {
                //Polski znak
                if ((int)text[i] > 260)
                {
                    result[i] = text[i];
                }
                //Lower case 97~
                else if (Char.IsLower(text[i]))
                {
                    result[i] = ('a' + ((b + a * ((int)text[i] - 'a')) % 26));
                }
                //UpperCase 64~
                else if (Char.IsUpper(text[i]))
                    result[i] = ('A' + ((b + a * ((int)text[i] - 'A')) % 26));
                else
                    result[i] = text[i];
                characters[i] = (char)result[i];
            }
            return new String(characters);
        }

        public String C_crypt(String text, int key)
        {
            if (key > 25 || key < 1)
            {
                throw new Exception("ERROR: Klucz nie jest z przedzialu (1-25)");
            }

            int[] result = new int[text.Length];
            char[] characters = new char[text.Length];

            for (int i = 0; i < text.Length; i++)
            {
                //Polski znak
                if ((int)text[i] > 260)
                {
                    result[i] = text[i];
                }
                else if (Char.IsLower(text[i]))
                {
                    result[i] = ('a' + ((key + (int)text[i] - 'a') % 26));
                }
                else if (Char.IsUpper(text[i]))
                    result[i] = ('A' + ((key + (int)text[i] - 'A') % 26));
                else
                    result[i] = text[i];
                characters[i] = (char)result[i];
            }
            return new String(characters);

        }
        public String C_decrypt(String text, int key)
        {
            int[] result = new int[text.Length];
            char[] characters = new char[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                //Polskie znaki przepisuje
                if ((int)text[i] > 260)
                {
                    result[i] = text[i];
                }
                else if (Char.IsLower(text[i]))
                {
                    result[i] = ('a' + ((26 - key + (int)text[i] - 'a') % 26));
                }
                else if (Char.IsUpper(text[i]))
                    result[i] = ('A' + ((26 - key + (int)text[i] - 'A') % 26));
                else
                    result[i] = text[i];
                characters[i] = (char)result[i];
            }
            return new String(characters);
        }
        public String[] C_analyzeAll(String text)
        {
            String[] result = new String[25];
            for (int i = 1; i < 26; i++)
            {
                result[i - 1] = C_decrypt(text, i);
            }
            return result;
        }
    

        public String[] C_analyze(String extra, String text)
        {
            int key = -1;
            String result = "";
            for (int i = 1; i < 26; i++)
            {
                String decrypted = C_decrypt(text, i);
                if (decrypted.Contains(extra))
                {
                    key = i;
                    result = decrypted;
                    break;
                }
            }
            if (key > 0)
            {
                return new String[] { key + "", result };
            }
            else
            {
                throw new Exception("ERROR: Nie da sie znalesc klucza!");
            }
        }
    }
}
