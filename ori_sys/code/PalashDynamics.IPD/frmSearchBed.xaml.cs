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
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.Administration.IPD;

namespace PalashDynamics.IPD
{
    public partial class frmSearchBed : ChildWindow
    {
        public event RoutedEventHandler OnOKButton_Click;
        public long WardID { get; set; }
        public long BedCategoryID { get; set; }
        public long BedID { get; set; }
        public string BedNO { get; set; }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
       
        public frmSearchBed()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmSearchBed_Loaded);
        }

        void frmSearchBed_Loaded(object sender, RoutedEventArgs e)
        {         
            FillBedCategory();
            FillWard();
            FillBed();            
        }
              
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            if (dgBedList.SelectedItem != null)
            {
                clsIPDBedMasterVO bed = (clsIPDBedMasterVO)dgBedList.SelectedItem;
                BedID = bed.ID;
                BedNO = bed.Description;
                WardID = bed.WardID;
                BedCategoryID = bed.BedCategoryID;
            }
            else
            {
                string msgText = "";
                msgText = "Please Select the Bed";
                
                MessageBoxControl.MessageBoxChildWindow msgW =
                      new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
           
                msgW.Show();
            }
          
            if (OnOKButton_Click != null)
                OnOKButton_Click(this, new RoutedEventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void FillBedCategory()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ClassMaster;
            BizAction.IsActive = true;


            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {

                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    cmbBedCategory.ItemsSource = null;
                    cmbBedCategory.ItemsSource = objList;
                    cmbBedCategory.SelectedValue = BedCategoryID;
                }

            

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

        }

        private void FillWard()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_WardMaster;
            BizAction.IsActive = true;


            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {

                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);



                    cmbWard.ItemsSource = null;
                    cmbWard.ItemsSource = objList;
                    cmbWard.SelectedValue = WardID;
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

        }

        private void FillBed()
        {
            try
            {
                clsGetIPDBedListBizActionVO BizAction = new clsGetIPDBedListBizActionVO();
                BizAction.BedDetails = new List<clsIPDBedMasterVO>();

                if (cmbBedCategory.SelectedItem != null)
                   BizAction.BedCategoryID = ((MasterListItem)cmbBedCategory.SelectedItem).ID;

                if (cmbWard.SelectedItem != null)
                    BizAction.WardID = ((MasterListItem)cmbWard.SelectedItem).ID;
                
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        if (((clsGetIPDBedListBizActionVO)e.Result).BedDetails != null)
                        {
                            dgBedList.ItemsSource = ((clsGetIPDBedListBizActionVO)e.Result).BedDetails;
                        }
                    }
                    else
                    {
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
         
            }
            catch (Exception ex)
            {

                
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FillBed();
        }
    }
}

