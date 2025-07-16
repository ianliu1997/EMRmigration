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
using PalashDynamics.UserControls;
using PalashDynamics.Pharmacy.ViewModels;

namespace PalashDynamics.Pharmacy.Inventory_DashBoard
{
    public partial class InventoryDashBoard_New : UserControl
    {
        WaitIndicator Indicatior = null;
        public InventoryDashBoard_New()
        {
            InitializeComponent();
            //this.DataContext = new HomeInventoryViewModel();
            this.Loaded += new RoutedEventHandler(InventoryDashBoard_New_Loaded);
        }
        bool IsPageLoded = false;

        void InventoryDashBoard_New_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                Indicatior.Close();
                ReorderLevel_Click(sender,e);
            }
            IsPageLoded = true;
        }

        private void ReorderLevel_Click(object sender, RoutedEventArgs e)
        {
            SetDockPanelVisibility("Reorder");
            ReorderLevel ReOrder = new ReorderLevel();
            ReOrder.Height = Reorders.Height - 20;
            ReOrder.Width = Reorders.Width - 10;
            this.ReorderContent.Content = ReOrder;
            ReorderContent.Visibility = Visibility.Visible;
            this.ReorderContent.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            this.ReorderContent.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
        }

        private void Expired_Click(object sender, RoutedEventArgs e)
        {
            SetDockPanelVisibility("Expired");
            ExpiredItems Expired = new ExpiredItems();
            Expired.Height = Expireds.Height - 20;
            Expired.Width = Expireds.Width - 10;
            this.ExpiredContent.Content = Expired;
            ExpiredContent.Visibility = Visibility.Visible;
            this.ExpiredContent.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            this.ExpiredContent.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
        }

        private void Indents_Click(object sender, RoutedEventArgs e)
        {
            SetDockPanelVisibility("Indents");
            PendingIndents Indent = new PendingIndents();
            Indent.Height = Expireds.Height - 20;
            Indent.Width = Expireds.Width - 10;
            this.PIndentContent.Content = Indent;
            PIndentContent.Visibility = Visibility.Visible;
            this.PIndentContent.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            this.PIndentContent.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
        }

        private void PPR_Click(object sender, RoutedEventArgs e)
        {
            SetDockPanelVisibility("PPR");
            PendingPR PR = new PendingPR();
            PR.Height = PPR.Height - 20;
            PR.Width = PPR.Width - 10;
            this.PPRContent.Content = PR;
            PPRContent.Visibility = Visibility.Visible;
            this.PPRContent.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            this.PPRContent.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
        }

        private void PPO_Click(object sender, RoutedEventArgs e)
        {
            SetDockPanelVisibility("PPO");
            PendingPO PO = new PendingPO();
            PO.Height = PPO.Height - 20;
            PO.Width = PPO.Width - 10;
            this.PPOContent.Content = PO;
            PPOContent.Visibility = Visibility.Visible;
            this.PPOContent.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            this.PPOContent.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
        }

        private void outterCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Reorders.Width = outterCanvas.ActualWidth - 10;
            Reorders.Height = outterCanvas.ActualHeight - 10;
            Expireds.Width = outterCanvas.ActualWidth - 10;
            Expireds.Height = outterCanvas.ActualHeight - 10;
            PIndents.Height = outterCanvas.ActualHeight - 10;
            PIndents.Width = outterCanvas.ActualWidth - 10;
            PPO.Height = outterCanvas.ActualHeight - 10;
            PPO.Width = outterCanvas.ActualWidth - 10;
            PPR.Width = outterCanvas.ActualWidth - 10;
            PPR.Height = outterCanvas.ActualHeight - 10;
            //Finance.Width = outterCanvas.ActualWidth - 10;

            //Finance.Height = outterCanvas.ActualHeight - 10;
            //CryoBank.Width = outterCanvas.ActualWidth - 10;
            //CryoBank.Height = outterCanvas.ActualHeight - 10;
        }


        private void SetDockPanelVisibility(string sState)
        {
            switch (sState)
            {
                case "PPR":
                    PPR.Visibility = Visibility.Visible;
                    PPRContent.Visibility = Visibility.Visible;
                    PIndents.Visibility = Visibility.Collapsed;
                    PIndentContent.Visibility = Visibility.Collapsed;
                    Reorders.Visibility = Visibility.Collapsed;
                    ReorderContent.Visibility = Visibility.Collapsed;
                    Expireds.Visibility = Visibility.Collapsed;
                    ExpiredContent.Visibility = Visibility.Collapsed;     
                    PPOContent.Visibility = Visibility.Collapsed;
                    PPO.Visibility = Visibility.Collapsed;
                    this.PPRContent.Content = null;
                    break;

                case "PPO":
                  //  Appointments.Visibility = Visibility.Collapsed;
                    PPO.Visibility = Visibility.Visible;
                    PPOContent.Visibility = Visibility.Visible;                    
                    PIndents.Visibility = Visibility.Collapsed;
                    PIndentContent.Visibility = Visibility.Collapsed;
                    Reorders.Visibility = Visibility.Collapsed;
                    ReorderContent.Visibility = Visibility.Collapsed;
                    Expireds.Visibility = Visibility.Collapsed;
                    ExpiredContent.Visibility = Visibility.Collapsed;    
                    PPR.Visibility = Visibility.Collapsed;
                    PPRContent.Visibility = Visibility.Collapsed;
                    this.PPOContent.Content = null;
                    break;

                case "Indents":
                    PIndents.Visibility = Visibility.Visible;
                    PIndentContent.Visibility = Visibility.Visible;
                    Reorders.Visibility = Visibility.Collapsed;
                    ReorderContent.Visibility = Visibility.Collapsed;
                    Expireds.Visibility = Visibility.Collapsed;
                    ExpiredContent.Visibility = Visibility.Collapsed;
                    PPO.Visibility = Visibility.Collapsed;
                    PPOContent.Visibility = Visibility.Collapsed;
                    PPR.Visibility = Visibility.Collapsed;
                    PPRContent.Visibility = Visibility.Collapsed;
                    this.PIndentContent.Content = null;
                    break;


                case "Reorder":
                    Reorders.Visibility = Visibility.Visible;
                    ReorderContent.Visibility = Visibility.Visible;
                    PIndents.Visibility = Visibility.Collapsed;
                    PIndentContent.Visibility = Visibility.Collapsed;
                    Expireds.Visibility = Visibility.Collapsed;
                    ExpiredContent.Visibility = Visibility.Collapsed;
                    PPO.Visibility = Visibility.Collapsed;
                    PPOContent.Visibility = Visibility.Collapsed; 
                    PPR.Visibility = Visibility.Collapsed;
                    PPRContent.Visibility = Visibility.Collapsed;
                    this.ReorderContent.Content = null;
                    break;

                case "Expired":
                    Reorders.Visibility = Visibility.Collapsed;
                    ReorderContent.Visibility = Visibility.Collapsed;
                    PIndents.Visibility = Visibility.Collapsed;
                    PIndentContent.Visibility = Visibility.Collapsed;
                    PPR.Visibility = Visibility.Collapsed;
                    PPRContent.Visibility = Visibility.Collapsed;
                    PPO.Visibility = Visibility.Collapsed;
                    PPOContent.Visibility = Visibility.Collapsed;
                    Expireds.Visibility = Visibility.Visible;
                    ExpiredContent.Visibility = Visibility.Visible;
                    this.ExpiredContent.Content = null;
                    break;

            }
        }
    
    }
}
