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
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.IVF
{
    public partial class SemenDetails : ChildWindow
    {
        public SemenDetails()
        {
            InitializeComponent();
            this.DataContext = new clsFemaleSemenDetailsVO();

            if (IsUpdate == true)
            {
                if (Details != null)
                {
                    this.DataContext = Details;

                }
            }
        }
        public event RoutedEventHandler OnSaveButton_Click;
        public clsFemaleSemenDetailsVO Details;

        public bool IsUpdate = false;

        private void FillSourceOfSemen()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_IVF_SourceSemenMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbSourceOfSemen.ItemsSource = null;
                    cmbSourceOfSemen.ItemsSource = objList;
                    cmbSourceOfSemen.SelectedItem = objList[0];

                    if (Details.SourceOfSemen != null && Details.SourceOfSemen != 0)
                    {

                        cmbSourceOfSemen.SelectedValue = Details.SourceOfSemen;
                    }

                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillMOSP()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_IVF_MethodOfSpermPreparationMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbMethodOfSpermpreparation.ItemsSource = null;
                    cmbMethodOfSpermpreparation.ItemsSource = objList;
                    cmbMethodOfSpermpreparation.SelectedItem = objList[0];

                    if (Details.MethodOfSpermPreparation != null && Details.MethodOfSpermPreparation != 0)
                    {

                        cmbMethodOfSpermpreparation.SelectedValue = Details.MethodOfSpermPreparation;
                    }

                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }



        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillSourceOfSemen();
            FillMOSP();
            if (Details == null)
            {

                Details = new clsFemaleSemenDetailsVO();
            }
            else
            {
                this.DataContext = Details;
            }
        }


        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (((MasterListItem)cmbMethodOfSpermpreparation.SelectedItem).ID == 0)
            {
                cmbMethodOfSpermpreparation.TextBox.SetValidation("Please select Method Of Sperm Preparation");
                cmbMethodOfSpermpreparation.TextBox.RaiseValidationError();
                cmbMethodOfSpermpreparation.Focus();

            }
            else if (((MasterListItem)cmbSourceOfSemen.SelectedItem).ID == 0)
            {
                cmbSourceOfSemen.TextBox.SetValidation("Please select Source Of Semen");
                cmbSourceOfSemen.TextBox.RaiseValidationError();
                cmbSourceOfSemen.Focus();
            }
            else
            {
                cmbMethodOfSpermpreparation.TextBox.ClearValidationError();
                cmbSourceOfSemen.TextBox.ClearValidationError();

                this.DialogResult = true;

                Details = (clsFemaleSemenDetailsVO)this.DataContext;

                if (cmbSourceOfSemen.SelectedItem != null)
                    Details.SourceOfSemen = ((MasterListItem)cmbSourceOfSemen.SelectedItem).ID;

                if (cmbMethodOfSpermpreparation.SelectedItem != null)
                    Details.MethodOfSpermPreparation = ((MasterListItem)cmbMethodOfSpermpreparation.SelectedItem).ID;


                if (OnSaveButton_Click != null)
                    OnSaveButton_Click(this, new RoutedEventArgs());
            }
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtTexBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (!((TextBox)sender).Text.IsItNumber())
            if (!((TextBox)sender).Text.IsValueDouble() && textBefore != null)
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

        }

        private void TexBox_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }
    }
}

