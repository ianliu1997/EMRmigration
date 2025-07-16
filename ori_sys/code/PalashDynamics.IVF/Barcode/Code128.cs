using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace PalashDynamics.Pharmacy.BarCode
{
    public class Code128
    {
        public Code128()
        {
            width = 300;
            height = 60;
            humanReadable = true;
            fontSize = 12;
            fontName = "Courier New";
            centered = false;
        }

        // By BHUSHAN FUTANE -- Barcode-128
        private int height;
        private bool humanReadable;
        private int width;
        private string fontName;
        private int fontSize;
        private bool centered;

        private static string[] left_UPCA = new string[] {"0001101", "0011001", "0010011", "0111101", "0100011", 
                                        "0110001", "0101111", "0111011" , "0110111", "0001011"};
        private static string[] right_UPCA = new string[] {"1110010", "1100110", "1101100", "1000010", "1011100",
                                        "1001110", "1010000", "1000100", "1001000", "1110100"}; //1s compliment of left odd

        private static string[] both_2of5 = new string[] { "NNWWN", "WNNNW", "NWNNW", "WWNNN", "NNWNW", "WNWNN",
                                        "NWWNN", "NNNWW", "WNNWN", "NWNWN" };
        Brush brush;
        Brush brush1;

        private static char[] Code128ComboAB = new char[] { 
            ' ', '!', '"', '#', '$', '%', '&', '\'', '(', ')', '*',
            '+', ',', '-', '.', '/', '0',  '1', '2', '3', '4', '5',
            '6', '7', '8', '9', ':', ';',  '<', '=', '>', '?', '@',
            'A', 'B', 'C', 'D', 'E', 'F',  'G', 'H', 'I', 'J', 'K',
            'L', 'M', 'N', 'O', 'P', 'Q',  'R', 'S', 'T', 'U', 'V',
            'W', 'X', 'Y', 'Z', '[', '\\', ']', '^', '_'
        };

        public static char[] Code128B = new char[] { 
            '`', 'a', 'b',  'c', 'd', 'e', 'f',  'g', 'h', 'i', 'j',
            'k', 'l', 'm',  'n', 'o', 'p', 'q',  'r', 's', 't', 'u',
            'v', 'w', 'x',  'y', 'z', '{', '|',  '}', '~'
        };

        public string[] Code128Encoding = new string[] {
            "11011001100", "11001101100", "11001100110", "10010011000", "10010001100", "10001001100", "10011001000", 
            "10011000100", "10001100100", "11001001000", "11001000100", "11000100100", "10110011100", "10011011100",
            "10011001110", "10111001100", "10011101100", "10011100110", "11001110010", "11001011100", "11001001110",
            "11011100100", "11001110100", "11101101110", "11101001100", "11100101100", "11100100110", "11101100100",
            "11100110100", "11100110010", "11011011000", "11011000110", "11000110110", "10100011000", "10001011000",
            "10001000110", "10110001000", "10001101000", "10001100010", "11010001000", "11000101000", "11000100010",
            "10110111000", "10110001110", "10001101110", "10111011000", "10111000110", "10001110110", "11101110110",
            "11010001110", "11000101110", "11011101000", "11011100010", "11011101110", "11101011000", "11101000110",
            "11100010110", "11101101000", "11101100010", "11100011010", "11101111010", "11001000010", "11110001010",
            "10100110000", "10100001100", "10010110000", "10010000110", "10000101100", "10000100110", "10110010000",
            "10110000100", "10011010000", "10011000010", "10000110100", "10000110010", "11000010010", "11001010000",
            "11110111010", "11000010100", "10001111010", "10100111100", "10010111100", "10010011110", "10111100100",
            "10011110100", "10011110010", "11110100100", "11110010100", "11110010010", "11011011110", "11011110110",
            "11110110110", "10101111000", "10100011110", "10001011110", "10111101000", "10111100010", "11110101000",
            "11110100010", "10111011110", "10111101110", "11101011110", "11110101110", "11010000100", "11010010000",
            "11010011100"
        };

        private static string Code128Stop = "11000111010";
        private enum Code128ChangeModes { CodeA = 101, CodeB = 100, CodeC = 99 };
        private enum Code128StartModes { CodeUnset = 0, CodeA = 103, CodeB = 104, CodeC = 105 };
        private enum Code128Modes { CodeUnset = 0, CodeA = 1, CodeB = 2, CodeC = 3 };

        #region Public Properties
        public bool Centered
        {
            get { return centered; }
            set { centered = value; }
        }

        public string FontName
        {
            get { return fontName; }
            set { fontName = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        public bool HumanReadable
        {
            get { return humanReadable; }
            set { humanReadable = value; }
        }
        public int FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }
        #endregion

        

        private string CheckDigitUPCA(string code)
        {
            int odd = 0;
            int even = 0;

            for (int i = 0; i < code.Length; i += 2)
                odd += Int32.Parse(code.Substring(i, 1));

            for (int i = 1; i < code.Length; i += 2)
                even += Int32.Parse(code.Substring(i, 1));

            int check = (10 - ((odd * 3) + even) % 10) % 10;
            return check.ToString().Trim();
        }        

        private string CheckDigitInterleaved(string code)
        {
            int odd = 0;
            int even = 0;

            for (int i = 0; i < code.Length; i += 2)
                even += Int32.Parse(code.Substring(i, 1));

            for (int i = 1; i < code.Length; i += 2)
                odd += Int32.Parse(code.Substring(i, 1));

            int check = (10 - ((odd * 3) + even) % 10) % 10;
            return check.ToString().Trim();
        }        

        int barwidth;

        private string humanText;
        public string getHumanText()
        {
            return humanText;
        }

        public List<Rectangle> DrawCode128(string code, int x, int y)
        {

            List<Rectangle> rList = new List<Rectangle>();
            List<int> encoded = new List<int>();
            Code128Modes currentMode = Code128Modes.CodeUnset;

            for (int i = 0; i < code.Length; i++)
            {
                if (IsNumber(code[i]) && i + 1 < code.Length && IsNumber(code[i + 1]))
                {
                    if (currentMode != Code128Modes.CodeC)
                    {
                        if (currentMode == Code128Modes.CodeUnset)
                            encoded.Add((int)Code128StartModes.CodeC);
                        else
                            encoded.Add((int)Code128ChangeModes.CodeC);
                        currentMode = Code128Modes.CodeC;
                    }
                    encoded.Add(Int32.Parse(code.Substring(i, 2)));
                    i++;
                }
                else
                {
                    if (currentMode != Code128Modes.CodeB)
                    {
                        if (currentMode == Code128Modes.CodeUnset)
                            encoded.Add((int)Code128StartModes.CodeB);
                        else
                            encoded.Add((int)Code128ChangeModes.CodeB);
                        currentMode = Code128Modes.CodeB;
                    }
                    encoded.Add(EncodeCodeB(code[i]));
                }
            }
            encoded.Add(CheckDigitCode128(encoded));

            string barbit = "";
            for (int i = 0; i < encoded.Count; i++)
            {
                barbit += Code128Encoding[encoded[i]];
            }
            barbit += Code128Stop;
            barbit += "11"; // end code



            barwidth = (int)Math.Floor((double)(width - 2) / (barbit.Length + 50)); // add 20 for padding
            if (barwidth <= 0)
                barwidth = 1;

     //       int padding = barwidth * 5;
            int padding = barwidth * 3;
            if (centered)
            {
                x = (int)x - (((barwidth * barbit.Length) + (padding * 2)) / 2);
            }
            height = 45;
            double start = x + padding;
            start = 0;
            for (int i = 1; i <= barbit.Length; i++)
            {
                string bit = barbit.Substring(i - 1, 1);
                if (bit == "0")
                {
                    brush = new SolidColorBrush(Colors.White);
                    Rectangle r = new Rectangle();
                    r.Fill = brush;
                    r.Width = barwidth;
                    r.Height = height;
                    r.SetValue(Canvas.LeftProperty, start);
                    r.SetValue(Canvas.TopProperty, 0.0);
                    rList.Add(r);
                }
                else 
                {
                    brush = new SolidColorBrush(Colors.Black);
                    Rectangle r = new Rectangle();
                    r.Fill = brush;
                    r.Width = barwidth;
                    r.Height = height;
                    r.SetValue(Canvas.LeftProperty, start);
                    r.SetValue(Canvas.TopProperty, 0.0);
                    rList.Add(r);
                }
                start += barwidth;
            }
            
            humanText = code;
            return rList;
        }

        private int CheckDigitCode128(List<int> codes)
        {
            int check = codes[0];
            for (int i = 1; i < codes.Count; i++)
            {
                check = check + (codes[i] * i);
            }
            return check % 103;
        }


        private bool IsNumber(char value)
        {
            return '0' == value || '1' == value || '2' == value || '3' == value ||
                   '4' == value || '5' == value || '6' == value || '7' == value ||
                   '8' == value || '9' == value;
        }

        private bool IsEven(int value)
        {
            return ((value & 1) == 0);
        }

        private bool IsOdd(int value)
        {
            return ((value & 1) == 1);
        }

        private int EncodeCodeB(char value)
        {
            for (int i = 0; i < Code128ComboAB.Length; i++)
            {
                if (Code128ComboAB[i] == value)
                    return i;
            }
            for (int i = 0; i < Code128B.Length; i++)
            {
                if (Code128B[i] == value)
                    return i + Code128ComboAB.Length;
            }
            throw new Exception("Invalid Character");
        }
    }
}
