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
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.IVF.TherpyExecution
{
    public partial class DeleteDrug : ChildWindow
    {

        public List<clsTherapyDrug> DrugList { get; set; }
    
        public event RoutedEventHandler OnCloseButton_Click;


        public DeleteDrug()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(DeleteDrug_Loaded);
        }

        void DeleteDrug_Loaded(object sender, RoutedEventArgs e)
        {
          
            dgDeleteDrug.ItemsSource = null;
            dgDeleteDrug.ItemsSource = DrugList;

        }
     
        private void cmdDeleteDrug_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxControl.MessageBoxChildWindow msgWD =
                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Are You Sure You Want To Delete Drug ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
            {
                if (res == MessageBoxResult.Yes)
                {
                   
                    DrugList.RemoveAt(dgDeleteDrug.SelectedIndex);
                    dgDeleteDrug.ItemsSource = null;
                    if (DrugList.Count > 0)
                        dgDeleteDrug.ItemsSource = DrugList;
                }
            };
            msgWD.Show();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            if (OnCloseButton_Click != null)
                OnCloseButton_Click(this, new RoutedEventArgs());
        }
    }
}

