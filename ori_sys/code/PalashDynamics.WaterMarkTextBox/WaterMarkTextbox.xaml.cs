using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PalashDynamics
{
    public partial class WaterMarkTextbox : UserControl
    {
        private string watermark;
        public string WaterMark
        {
            set
            {
                watermark = value;
            }

            get
            {
                return watermark;
            }
        }

        public TextBox Textbox
        {
            get
            {

                return textBox1;
                
            }
            set
            {

               
                    textBox1 = value;
              
            }
        }

        public string Text
        {
            get
            {
                if (textBox1.Text == this.watermark)
                {
                    return "";
                }
                else
                {
                    return textBox1.Text;
                }
            }
            set
            {

                if (value != null)
                {
                    DisableWaterMark();
                    textBox1.Text = value;
                }
            }
        }

        public int SelectionStart
        {
            get
            {
             
                    return textBox1.SelectionStart;
              
            }
            set
            {

                    textBox1.SelectionStart = value;
              
            }
        }

        public int SelectionLength
        {
            get
            {

                return textBox1.SelectionLength;

            }
            set
            {

                textBox1.SelectionLength = value;

            }
        }

        public TextAlignment TextBoxAlignment
        {
            get
            {

                return textBox1.TextAlignment;
               
            }
            set
            {

                textBox1.TextAlignment = value;
            }
        }

        public event RoutedEventHandler OnTextChanged;

        public WaterMarkTextbox()
        {
            InitializeComponent();
            
       
        }

        private void SetWaterMark()
        {
            textBox1.Foreground = new SolidColorBrush(Colors.LightGray);
            textBox1.Text = this.watermark;
            
        }

        void textBox1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                SetWaterMark();
                textBox1.GotFocus += new RoutedEventHandler(textBox1_GotFocus);
            }
        }

        private void DisableWaterMark()
        {
            textBox1.Text = "";
            textBox1.GotFocus -= textBox1_GotFocus;
            textBox1.Foreground = new SolidColorBrush(Colors.Black);
        }

        void textBox1_GotFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                DisableWaterMark();
            }
        }

        private void textBox1_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.watermark))
            {
                textBox1.GotFocus += new RoutedEventHandler(textBox1_GotFocus);
                textBox1.LostFocus += new RoutedEventHandler(textBox1_LostFocus);
                //SetWaterMark();
            }
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (OnTextChanged != null)
                OnTextChanged(this, new RoutedEventArgs());
        }


    }
}
