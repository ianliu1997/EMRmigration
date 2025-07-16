using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Administration.PackageNew;
using PalashDynamics.ValueObjects.Administration.PatientSourceMaster;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace PalashDynamics.Administration
{
    public partial class frmPackageRateClinicWise : ChildWindow
    {
        public frmPackageRateClinicWise()
        {
            InitializeComponent();
        }



        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GetData();

        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        //private void FillClinicList()
        //{
        //    try
        //    {
        //        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //        BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
        //        BizAction.Parent = new KeyValue();
        //        BizAction.Parent.Key = "1";
        //        BizAction.Parent.Value = "Status";
        //        BizAction.MasterList = new List<MasterListItem>();
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (s, args) =>
        //        {
        //            if (args.Error == null && args.Result != null)
        //            {
        //                List<MasterListItem> objList = new List<MasterListItem>();
        //                objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
        //                dgClinic.ItemsSource = null;
        //                dgClinic.ItemsSource = objList;
        //                GetData();

        //            }
        //        };

        //        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        client.CloseAsync();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        bool Update = false;
        private void GetData()
        {
            try
            {
                clsGetPackageRateClinicWiseBizActionVO BizAction = new clsGetPackageRateClinicWiseBizActionVO();
                BizAction.PackageID = PackageID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<clsPackageRateClinicWiseVO> objList = new List<clsPackageRateClinicWiseVO>();
                        objList.AddRange(((clsGetPackageRateClinicWiseBizActionVO)args.Result).PackageRateClinicWiseList);
                        //dgClinic.ItemsSource = null;
                        //dgClinic.ItemsSource = objList;

                        PagedCollectionView collection = new PagedCollectionView(objList);
                        collection.GroupDescriptions.Add(new PropertyGroupDescription("UnitName"));
                        dgClinic.ItemsSource = null;
                        dgClinic.ItemsSource = collection;

                    }
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        public long TariffID = 0;
        public long TariffUnitID = 0;
        public long PackageID = 0;
        public long PackageSeriveID = 0;

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {           
            List<clsPackageRateClinicWiseVO> objlist = (List<clsPackageRateClinicWiseVO>)((PagedCollectionView)(dgClinic.ItemsSource)).SourceCollection;
            int iCount = objlist.Count;
            if (iCount > 0)
            {

                clsAddPackageRateClinicWiseBizActionVO BizAction = new clsAddPackageRateClinicWiseBizActionVO();

                BizAction.tariffID = TariffID;
                BizAction.PackageID = PackageID;
                BizAction.PackageServiceID = PackageSeriveID;

                BizAction.PackageRateClinicWiseList = new List<clsPackageRateClinicWiseVO>();

                foreach (var item in objlist)
                {
                    if (item.Status == true)
                    {
                        BizAction.PackageRateClinicWiseList.Add(item);
                    }
                }
               
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Clinic Assigned Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);


                        msgW.Show();
                        this.Close();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Clinic", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);


                msgW.Show();
            }
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            List<clsPackageRateClinicWiseVO> objlist = (List<clsPackageRateClinicWiseVO>)((PagedCollectionView)(dgClinic.ItemsSource)).SourceCollection;
            if (chkAllClinic.IsChecked == true)
            {

                foreach (var item in objlist)
                {
                    item.Status = true;
                    item.IsRateEnabled = true;
                }
            }
            if (chkAllClinic.IsChecked == false)
            {

                foreach (var item in objlist)
                {
                    item.Status = false;
                    item.IsRateEnabled = false;
                    item.Rate = Convert.ToDecimal("0.00");
                }
            }
            dgClinic.ItemsSource = null;
            dgClinic.ItemsSource = objlist;



        }

        private void txtRate_Changed(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValueDouble())
            {
                if (textBefore != null)
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
            else
            {
                List<clsPackageRateClinicWiseVO> objlist = (List<clsPackageRateClinicWiseVO>)((PagedCollectionView)(dgClinic.ItemsSource)).SourceCollection;
                string TextValue = ((TextBox)sender).Text;
                foreach (var item in objlist)
                {
                    if (item.UnitID == ((clsPackageRateClinicWiseVO)dgClinic.SelectedItem).UnitID && item.PatientCategoryL3 == ((clsPackageRateClinicWiseVO)dgClinic.SelectedItem).PatientCategoryL3)
                    {
                        item.Rate = Convert.ToDecimal(TextValue);

                    }
                }
            }
        }

        FrameworkElement element;
        DataGridRow row;
        TextBox txtRate;

        private void SingleCheckBox_Click(object sender, RoutedEventArgs e)
        {
            element = dgClinic.Columns.Last().GetCellContent(dgClinic.SelectedItem);
            row = DataGridRow.GetRowContainingElement(element.Parent as FrameworkElement);
            txtRate = FindVisualChild<TextBox>(row, "txtRate");

            if (((CheckBox)sender).IsChecked == true)
            {
                txtRate.IsEnabled = true;
            }
            else if (((CheckBox)sender).IsChecked == false)
            {
                txtRate.IsEnabled = false;
                txtRate.Text = "0.00";
            }
        }

        private ChildControl FindVisualChild<ChildControl>(DependencyObject DependencyObj, String TextBoxName)
         where ChildControl : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(DependencyObj); i++)
            {
                DependencyObject Child = VisualTreeHelper.GetChild(DependencyObj, i);

                if (Child != null && Child is ChildControl)
                {
                    if (Child is TextBox)
                    {
                        if (((TextBox)Child).Name.Equals(TextBoxName))
                        {
                            return (ChildControl)Child;
                        }
                    }
                    else if (Child is DataGrid)
                    {
                        if (((DataGrid)Child).Name.Equals(TextBoxName))
                        {
                            return (ChildControl)Child;
                        }
                    }
                    else
                    {
                        ChildControl ChildOfChild = FindVisualChild<ChildControl>(Child, TextBoxName);

                        if (ChildOfChild != null)
                        {
                            return ChildOfChild;
                        }
                    }
                }
                else
                {
                    ChildControl ChildOfChild = FindVisualChild<ChildControl>(Child, TextBoxName);

                    if (ChildOfChild != null)
                    {
                        return ChildOfChild;
                    }
                }
            }
            return null;
        }

        private void txtRate_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }


    }
}

