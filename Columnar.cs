using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        ///////////////////////////////////////////////////////////////////////////////
        public List<int> Analyse(string plainText, string cipherText)
        {
            double rows = cipherText.Length;

            SortedDictionary<int, int> dec = new SortedDictionary<int, int>();
            int R = 1;
            while (R < cipherText.Length)
            {
                plainText = plainText.ToUpperInvariant();
                cipherText = cipherText.ToUpperInvariant();


                double columns = (plainText.Length + R - 1) / R;

                string[,] matrix = new string[(int)columns, (int)rows];
                int index = 0;

                int i = 0;
                while (i < columns)
                {
                    int s = 0;
                    while (s < R)
                    {
                        if (index >= plainText.Length)
                        {
                            matrix[i, s] = string.Empty;

                        }
                        else
                        {
                            matrix[i, s] = plainText[index++].ToString();

                        }
                        s++;
                    }
                    i++;
                }

                bool found = true;
                string new_txt = (string)cipherText.Clone();

                List<string> txt = new List<string>();

                int j = 0;
                while (j < R)
                {
                    string substring = string.Empty;

                    int k = 0;
                    while (k < columns)
                    {
                        substring += matrix[k++, j];

                    }
                    txt.Add(substring);
                    j++;
                }

                dec = new SortedDictionary<int, int>();
                int l = 0;
                while (l < txt.Count)
                {
                    int x = new_txt.IndexOf(txt[l]);
                    if (x != -1)
                    {
                        dec.Add(x, l + 1);
                    }
                    else
                    {
                        found = false;
                    }
                    l++;
                }

                if (found)
                    break;

                R++;
            }

            List<int> key = new List<int>();
            Dictionary<int, int> dec2 = new Dictionary<int, int>();

            int m = 0;
            while (m < dec.Count)
            {
                dec2.Add(dec.ElementAt(m).Value, m + 1);
                m++;
            }

            int n = 1;
            while (n < dec2.Count + 1)
            {
                key.Add(dec2[n++]);

            }

            return key;

        }


        public string Decrypt(string cipherText, List<int> key)
        {
            

            double columns = key.Count;
            double rows = Math.Ceiling((double)cipherText.Length / columns);
            char[,] matrix = new char[(int)rows, (int)columns]; 
            int c_index = 0;

            
            for (int C = 0; C < columns; C++) 
            {
                for (int R = 0; R < rows; R++)
                {
                    if (c_index < cipherText.Length)
                    {
                        matrix[R, C] = cipherText[c_index];
                        c_index++;
                    }
                    else
                    {
                        matrix[R, C] = 'x';
                    }
                }
            }

           
            
            int column_matrix = cipherText.Length % key.Count;
            int depth = cipherText.Length / key.Count;
            string text = ""; //empty text

            for (int i = 0; i < depth + (column_matrix > 0 ? 1 : 0); i++)
            {
                
                for (int k = 0; k < key.Count; k++)
                {
                    int column = key[k] - 1;
                    if (column < column_matrix)
                    {
                        int cipherIndex = column * (depth + 1) + i;
                        text += matrix[i, column];
                    }
                    else
                    {
                        int cipherIndex = column * depth + column_matrix + i;
                        text += matrix[i, column];
                    }
                }
            }

            return text;

        }

        public string Encrypt(string plainText, List<int> key)
        {
            //throw new NotImplementedException();

            double rows = key.Count();
            //double columns = (double)plainText.Length / rows;
            double columns = Math.Ceiling((double)plainText.Length / rows);

            char[,] matrix = new char[(int)columns, (int)rows];

            double x = plainText.Length / rows;

            int c_index = 0;
            for (int R = 0; R < columns; R++)
            {


                for (int c = 0; c < rows; c++)
                {
                    matrix[R, c] = plainText[R];
                    if (c_index < plainText.Length)
                    {

                        matrix[R, c] = plainText[c_index];
                        c_index++;
                    }
                    else
                    {


                        matrix[R, c] = 'x';
                    }
                }



            }
            ///////////////////////////////////////////


            int[] keyArray = new int[key.Count];
            for (int i = 0; i < key.Count; i++)
            {
                keyArray[key[i] - 1] = i;
            }

            string myciphertext = "";

            for (int i = 0; i < key.Count; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    myciphertext += matrix[j, keyArray[i]];
                }
            }

            Console.WriteLine(myciphertext.ToUpper());
            return myciphertext.ToUpper();


        }











    }
}

