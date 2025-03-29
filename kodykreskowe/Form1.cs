using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.VisualStyles;

namespace kodykreskowe_cc
{
    public partial class Form1 : Form
    {
        // tablice przechowujace informacje o kodowaniu poszczegolnych cyfr
        string[] leftA = {"0001101", "0011001", "0010011", "0111101", "0100011",
         "0110001", "0101111", "0111011", "0110111", "0001011"};
        string[] leftB = {"0100111", "0110011", "0011011", "0100001", "0011101",
         "0111001", "0000101", "0010001", "0001001", "0010111"};
        string[] right = {"1110010", "1100110", "1101100", "1000010", "1011100",
         "1001110", "1010000", "1000100", "1001000", "1110100"};

        // tablica przechowujaca informacje o kodowaniu pierwszej cyfry
        int[,]lF = {{0,0,0,0,0,0},
                    {0,0,1,0,1,1},
                    {0,0,1,1,0,1},
                    {0,0,1,1,1,0},
                    {0,1,0,0,1,1},
                    {0,1,1,0,0,1},
                    {0,1,1,1,0,0},
                    {0,1,0,1,0,1},
                    {0,1,0,1,1,0},
                    {0,1,1,0,1,0}};

        

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // pobranie cyfr podanych przez uzytkownika
            String str = textBox1.Text.ToString();
            int[] tmpArr = new int[12] {0,0,0,0,0,0,0,0,0,0,0,0};
            tmpArr = str.Select(c => c - '0').ToArray();
            int[] intArr = new int[13];
            Array.Copy(tmpArr, intArr, tmpArr.Length);

            // obliczanie sumy kontrolnej
            var digits = intArr;
            var even = digits.Where((x, i) => i % 2 == 0).Sum();
            var odd = digits.Where((x, i) => i % 2 != 0).Sum() * 3;
            int checkDigit = (10 - ((even + odd) % 10)) % 10;
            intArr[12] = checkDigit; 

            label2.Text = "Suma kontrolna = ";
            label2.Text += Convert.ToString(checkDigit);
            label3.Text = "";
            for ( int i = 0; i < intArr.Length; i++)
            {
                label3.Text += Convert.ToString(intArr[i]);
            }


            // tablica przechowujaca zakodowane cyfry kodu kreskowego
            StringBuilder sb = new StringBuilder();
            // bity startu
            sb.Append("101");
            int first = intArr[0];
            // kodowanie 6 cyfr lewej strony kodu kreskowego
            for(int i = 1; i < 7; i++)
            {
                int num = intArr[i];

                if (lF[first,i-1] == 0)
                {
                    sb.Append(leftA[num]);
                }
                if (lF[first, i-1] == 1)
                {
                    sb.Append(leftB[num]);
                }
            }
            // bity srodkowego znacznika
            sb.Append("01010");
            // kodowanie 6 cyfr prawej strony kodu kreskowego
            for (int i = 7; i < 13; i++)
            {          
                sb.Append(right[intArr[i]]);
            }
            // bity stopu
            sb.Append("101");

            label1.Text = Convert.ToString(sb);

            int width = 500;
            int height = 500;

            Bitmap bmp = new Bitmap(width, height);
            System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            System.Drawing.SolidBrush brush2 = new System.Drawing.SolidBrush(System.Drawing.Color.White);
            string strtemp = sb.ToString();
            float xPosition = 0;
            float yStart = 0;
            float xEnd = 0;
            float lineWidth = 3;

            // tworzenie grafiki na podstawie ciagu bitow
            Graphics g = Graphics.FromImage(bmp);
            for (int i = 0; i < sb.Length; i++)
            {
                // Rysowanie czarnych paskow
                if (strtemp[i] == '1')
                {
                        g.FillRectangle(brush, xPosition, yStart, lineWidth, height);
                }

                xPosition += lineWidth;
                xEnd = xPosition;
            }
            // zapisanie grafiki do pliku png
            g.Dispose();

            String filename = "barcode.png";
            bmp.Save(filename);
            pictureBox1.Image = Image.FromFile(filename);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
