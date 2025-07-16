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

namespace DataDrivenApplication
{
    public partial class PrintPage : UserControl
    {
        public PrintPage()
        {
            InitializeComponent();
        }

        public void SetHeaderAndFooterText(string patient,string template)
        {
            txtPatientName.Text = patient;
            txtTemplate.Text = template;            
        }

        public void SetSectionHeader(string section)
        {
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(50);
            PageBody.RowDefinitions.Add(row);

            TextBlock txtSectionName = new TextBlock();            
            txtSectionName.FontSize = 16;
            //txtSectionName.FontWeight = FontWeights.Bold;
            txtSectionName.HorizontalAlignment = HorizontalAlignment.Center;
            txtSectionName.Margin = new Thickness(0,10,0,10);
            txtSectionName.Text = section;
            Grid.SetRow(txtSectionName, PageBody.RowDefinitions.Count - 1);
            Grid.SetColumnSpan(txtSectionName,2);
            //Grid.SetColumn(txtSectionName, 0);
            PageBody.Children.Add(txtSectionName);
        }

        public void SetFieldText(string Field)
        {
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(25);
            PageBody.RowDefinitions.Add(row);

            TextBlock txtFieldName = new TextBlock();
            txtFieldName.HorizontalAlignment = HorizontalAlignment.Right;
            txtFieldName.Text = Field;
            Grid.SetRow(txtFieldName,PageBody.RowDefinitions.Count-1);
            Grid.SetColumn(txtFieldName,0);
            PageBody.Children.Add(txtFieldName);

            

        }

        public void SetFieldValue(string FieldValue)
        {
            TextBlock txtFieldValue = new TextBlock();
            txtFieldValue.Text = FieldValue;
            Grid.SetRow(txtFieldValue, PageBody.RowDefinitions.Count - 1);
            Grid.SetColumn(txtFieldValue, 1);
            PageBody.Children.Add(txtFieldValue);
        }
    }
}
