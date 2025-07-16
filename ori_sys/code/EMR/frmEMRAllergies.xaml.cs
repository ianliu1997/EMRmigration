using System;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Linq;
using System.Windows.Data;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.SearchResultLists;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.RSIJ;
using System.Collections.Generic;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Inventory;
namespace EMR
{
    public partial class frmEMRAllergies : UserControl
    {
        #region Data Member
        public Boolean IsControlEnable { get; set; }
        private PagedCollectionView pcvAllergiesList = null;
        public clsVisitVO CurrentVisit { get; set; }
        private clsEMRAllergiesVO SavedAllergies { get; set; }
        List<clsGetDrugForAllergies> StoreDrug = new List<clsGetDrugForAllergies>();
        //string sMsgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorProcessing_Msg");

        Boolean flag = false;
        #endregion

        #region Constructor
        public frmEMRAllergies()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmEMRAllergies_Loaded);
            this.DataContext = new clsEMRAllergiesVO();
            SavedAllergies = new clsEMRAllergiesVO();
        }
        #endregion

        #region Loaded
        void frmEMRAllergies_Loaded(object sender, RoutedEventArgs e)
        {
            GetPatientDrugAllergies();
            GetPatientAllergies();
           
            FillMolecule();
            

            if (CurrentVisit.VisitTypeID == 2 && !CurrentVisit.OPDIPD)
            {
                spSpecDoctor.Visibility = Visibility.Collapsed;
              //  this.IsControlEnable = false;
            }
            else if (this.CurrentVisit.VisitTypeID == 1)
            {
                spSpecDoctor.Visibility = Visibility.Collapsed;
            }
            else
            {
                //spSpecDoctor.Visibility = Visibility.Visible;
                FillSpecialization();
                FillDoctor();
            }
            //DateTime d = CurrentVisit.Date;
            //if (d.ToString("d") != DateTime.Now.ToString("d"))
            //{
            //    cmdSave.IsEnabled = false;
            //}

            // EMR Changes Added by Ashish Z. on dated 02062017
            if (CurrentVisit.EMRModVisitDate <= DateTime.Now)
            {
                cmdSave.IsEnabled = false;
            }
            //End

           //cmdSave.IsEnabled = IsControlEnable;
        }
        #endregion

        #region Private Methods

        private void GetPatientDrugAllergies()
        {
            clsGetDrugForAllergies patientDrugAllergies = new clsGetDrugForAllergies();
            clsGetPatientDrugAllergiesBizActionVO BiZAction = new clsGetPatientDrugAllergiesBizActionVO();
            BiZAction.PatientID = CurrentVisit.PatientId;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && ((clsGetPatientDrugAllergiesBizActionVO)arg.Result).DrugAllergiesList != null)
                {
                    StoreDrug = ((clsGetPatientDrugAllergiesBizActionVO)arg.Result).DrugAllergiesList;
                }
            };
            Client.ProcessAsync(BiZAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillDrugList()
        {
            try
            {
                clsGetItemListBizActionVO BizActionObject = new clsGetItemListBizActionVO();
                BizActionObject.ItemList = new List<clsItemMasterVO>();
                if ((MasterListItem)cmbMolecule.SelectedItem != null)
                    BizActionObject.FilterIMoleculeNameId = ((MasterListItem)cmbMolecule.SelectedItem).ID;
                BizActionObject.FilterClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizActionObject.FromEmr = true;
                BizActionObject.ForReportFilter = true;
                BizActionObject.IsQtyShow = false;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        clsGetItemListBizActionVO result = ea.Result as clsGetItemListBizActionVO;
                        if (result.MasterList != null)
                        {
                            foreach (MasterListItem item in result.MasterList)
                            {
                                item.Description = item.Name;
                                var item1 = from r in StoreDrug
                                            where (r.DrugId == item.ID
                                           )
                                            select r;
                                if (item1.ToList().Count > 0)
                                {
                                    item.Status = true;
                                }
                                List<MasterListItem> objList = new List<MasterListItem>();
                                objList.Add(new MasterListItem(0, "-- Select --"));
                                objList.AddRange(((clsGetItemListBizActionVO)ea.Result).MasterList);
                                List<MasterListItem> lstCC = objList.DeepCopy();
                                acbAssChiefComplaints.ItemsSource = null;
                                acbAssChiefComplaints.ItemsSource = lstCC;
                                acbAssChiefComplaints.SelectedItem = lstCC[0];
                            }
                        }
                    }
                };
                client.ProcessAsync(BizActionObject, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception) { }
        }

        private void FillMolecule()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_Molecule;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                       flag = false;
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                        cmbMolecule.ItemsSource = objList;
                        cmbMolecule.SelectedItem = objList[0];
                       flag = true;
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception) { }
        }

        private void GetPatientAllergies()
        {
            clsEMRAllergiesVO objAllergy = new clsEMRAllergiesVO();
            clsGetPatientAllergiesBizActionVO BizAction = new clsGetPatientAllergiesBizActionVO();
            BizAction.VisitID = this.CurrentVisit.ID;
            BizAction.PatientID = this.CurrentVisit.PatientId;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null && ((clsGetPatientAllergiesBizActionVO)arg.Result).AllergiesList != null)
                    {
                        this.DataContext = ((clsGetPatientAllergiesBizActionVO)arg.Result).CurrentAllergy;
                        SavedAllergies = ((clsGetPatientAllergiesBizActionVO)arg.Result).CurrentAllergy.DeepCopy();
                    }
                }
                else
                {
                    ShowMessageBox("Error ocurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void NavigateToDashBoard()
        {
            this.Content = null;
            ((((((((this.Parent) as ContentControl).Parent as Border).Parent as DockPanel).Parent as DockPanel).FindName("tvPatientEMR") as TreeView)).Items[0] as TreeViewItem).IsSelected = true;
        }

        private Boolean IsValidate()
        {
            Boolean blnValid = true;
            clsEMRAllergiesVO objAllergy = this.DataContext as clsEMRAllergiesVO;
            string msgText = string.Empty;
            if (SavedAllergies == null)
                SavedAllergies = new clsEMRAllergiesVO();
            if (String.IsNullOrEmpty(objAllergy.DrugAllergy) && String.IsNullOrEmpty(objAllergy.FoodAllergy) && String.IsNullOrEmpty(objAllergy.OtherAllergy) && String.IsNullOrEmpty(SavedAllergies.DrugAllergy) && String.IsNullOrEmpty(SavedAllergies.FoodAllergy) && String.IsNullOrEmpty(SavedAllergies.OtherAllergy))
            {
                msgText = "Please Enter Allergies";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("EnterAllergies_Msg");
                blnValid = false;
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            }
            else if (objAllergy.OtherAllergy == SavedAllergies.OtherAllergy && objAllergy.DrugAllergy == SavedAllergies.DrugAllergy && objAllergy.FoodAllergy == SavedAllergies.FoodAllergy)
            {
                blnValid = false;
                msgText = "Please Enter New Allergies";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("EnterNewAllergy_Msg");
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            }
            return blnValid;
        }

        private void SaveAllergies()
        {
            clsAddUpdatePatientAllergiesBizActionVO BizAction = new clsAddUpdatePatientAllergiesBizActionVO();
            BizAction.VisitID = this.CurrentVisit.ID;
            BizAction.PatientID = this.CurrentVisit.PatientId;
            BizAction.OPDIPD = this.CurrentVisit.OPDIPD;
            BizAction.VisitUnitID = CurrentVisit.VisitID;
            BizAction.PatientUnitID = CurrentVisit.PatientUnitId;
            BizAction.CurrentAllergies = this.DataContext as clsEMRAllergiesVO;
            BizAction.DrugAllergies = StoreDrug;
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        UserControl winEMR;
                        TreeView tvEMR = null;
                        winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmEMR;
                        frmEMR wEMR = winEMR as frmEMR;
                        string Str = this.txtDrug.Text.Replace("\r", "");
                        if (Str.Length > 150)
                        {
                            Str = Str.Substring(0, 150) + "...";
                        }
                        wEMR.allergies.Content = String.Format(this.txtDrug.Text.Replace("\r", "")) == String.Empty ? "" : String.Format(Str);
                        tvEMR = winEMR.FindName("tvPatientEMR") as TreeView;

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                    else
                    {
                        ShowMessageBox("Error ocurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void NavigateToNextMenu()
        {
            UserControl winEMR;
            TreeView tvEMR = null;
            //if (this.CurrentVisit.VisitTypeID == 2)
            //{
            //    winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmIPDEMR;
            //    frmIPDEMR wEMR = winEMR as frmIPDEMR;
            //    wEMR.allergies.Content = String.Format(this.txtDrug.Text.Replace("\r", "")) == String.Empty ? "" : String.Format(this.txtDrug.Text.Replace("\r", ""));
            //    tvEMR = winEMR.FindName("tvPatientEMR") as TreeView;
            //}
            //else if (this.CurrentVisit.VisitTypeID == 1)
            //{
                winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmEMR;
                frmEMR wEMR = winEMR as frmEMR;
                wEMR.allergies.Content = String.Format(this.txtDrug.Text.Replace("\r", "")) == String.Empty ? "" : String.Format(this.txtDrug.Text.Replace("\r", ""));
                tvEMR = winEMR.FindName("tvPatientEMR") as TreeView;
            //}


            TreeViewItem SelectedItem = tvEMR.SelectedItem as TreeViewItem;
            clsMenuVO objMenu = SelectedItem.DataContext as clsMenuVO;
            if (SelectedItem.HasItems == true)
            {
                (SelectedItem.Items[0] as TreeViewItem).IsSelected = true;
            }
            else if (objMenu.Parent.Trim() == "Patient EMR")
            {
                int iCount = tvEMR.Items.Count;
                int iMenuIndex = Convert.ToInt32(objMenu.MenuOrder);
                if (objMenu.MenuOrder < iCount)
                {
                    if ((tvEMR.Items[iMenuIndex] as TreeViewItem).HasItems == true)
                    {
                        ((tvEMR.Items[iMenuIndex] as TreeViewItem).Items[0] as TreeViewItem).IsSelected = true;
                    }
                    else
                        (tvEMR.Items[iMenuIndex] as TreeViewItem).IsSelected = true;
                }
            }
            else
            {
                int iCount = (SelectedItem.Parent as TreeViewItem).Items.Count;
                int iMenuIndex = Convert.ToInt32(objMenu.MenuOrder);
                if (iCount > objMenu.MenuOrder)
                {
                    ((SelectedItem.Parent as TreeViewItem).Items[iMenuIndex] as TreeViewItem).IsSelected = true;
                }
                else
                {
                    objMenu = (SelectedItem.Parent as TreeViewItem).DataContext as clsMenuVO;
                    int iIndex = Convert.ToInt32(objMenu.MenuOrder);
                    (tvEMR.Items[iIndex] as TreeViewItem).IsSelected = true;
                }
            }
        }

        #region Fill DOCTER AND SPLIZATION COMBO
        private void FillDoctor()
        {
            //clsGetRSIJDoctorDepartmentDetailsBizActionVO BizAction = new clsGetRSIJDoctorDepartmentDetailsBizActionVO();
            //BizAction.MasterList = new List<MasterListItem>();
            //if (cmbSpecialization.SelectedItem != null)
            //{
            //    BizAction.IsForReferral = true;
            //    BizAction.SpecialCode = sDeptCode;
            //}
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            //client.ProcessCompleted += (s, arg) =>
            //{
            //    if (arg.Error == null && arg.Result != null)
            //    {
            //        List<MasterListItem> objList = new List<MasterListItem>();
            //        objList.Add(new MasterListItem("0", "-- Select --"));
            //        objList.AddRange(((clsGetRSIJDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);
            //        cmbDoctor.ItemsSource = null;
            //        cmbDoctor.ItemsSource = objList;
            //        cmbDoctor.SelectedItem = objList[0];
            //        if (this.DataContext != null)
            //        {
            //            cmbDoctor.SelectedValue = objList[0].ID;
            //            if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
            //            {
            //                cmbDoctor.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorCode;
            //                cmbDoctor.IsEnabled = false;
            //            }
            //            else
            //                cmbDoctor.SelectedValue = "0";
            //        }
            //    }
            //};
            //client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //client.CloseAsync();
            List<MasterListItem> objList = new List<MasterListItem>();
            objList.Add(new MasterListItem(this.CurrentVisit.DoctorCode, this.CurrentVisit.Doctor));
            cmbDoctor.ItemsSource = null;
            cmbDoctor.ItemsSource = objList;
            cmbDoctor.SelectedItem = objList[0];
            cmbDoctor.IsEnabled = false;
        }
        private void FillSpecialization()
        {
            //try
            //{
            //    clsGetRSIJMasterListBizActionVO BizAction = new clsGetRSIJMasterListBizActionVO();
            //    BizAction.MasterTable = MasterTableNameList.SPESIAL;
            //    BizAction.CodeColumn = "KDSPESIAL";
            //    BizAction.DescriptionColumn = "NMSPESIAL";
            //    BizAction.MasterList = new List<MasterListItem>();
            //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //    client.ProcessCompleted += (s, arg) =>
            //    {
            //        if (arg.Error == null && arg.Result != null)
            //        {
            //            List<MasterListItem> objList = new List<MasterListItem>();
            //            objList.Add(new MasterListItem("0", "-- Select --"));
            //            objList.AddRange(((clsGetRSIJMasterListBizActionVO)arg.Result).MasterList);
            //            cmbSpecialization.ItemsSource = null;
            //            cmbSpecialization.ItemsSource = objList;
            //            if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
            //            {
            //                string sSpecCode = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorSpecCode;
            //                cmbSpecialization.SelectedItem = objList.Where(z => z.Code == sSpecCode).FirstOrDefault();
            //                cmbSpecialization.IsEnabled = false;
            //            }
            //            else
            //            {
            //                cmbSpecialization.SelectedItem = objList[0];
            //            }
            //        }
            //    };
            //    client.ProcessAsync(BizAction, new clsUserVO());
            //}
            //catch (Exception)
            //{
            //}
            List<MasterListItem> objList = new List<MasterListItem>();
            objList.Add(new MasterListItem("0", this.CurrentVisit.DoctorSpecialization));
            cmbSpecialization.ItemsSource = null;
            cmbSpecialization.ItemsSource = objList;
            cmbSpecialization.SelectedItem = objList[0];
            cmbSpecialization.IsEnabled = false;
        }
        #endregion

        #endregion

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        #region Event

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes && CurrentVisit != null)
            {
                SaveAllergies();
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidate())
            {
                if (CurrentVisit != null)
                    SaveAllergies();
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigateToDashBoard();
        }

        void msgWinCancel_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                NavigateToDashBoard();
            }
        }

        private void chkMultipleCC_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            TextBox txtTarget = chk.Name == "chkMultipleCC" ? txtDrug : txtDrug;
            string strDescription = (chk.DataContext as MasterListItem).Description;
            if (chk.IsChecked == true && (chk.DataContext as MasterListItem).ID != 0 && !txtTarget.Text.Contains(strDescription))
            {
                var item1 = from r in StoreDrug
                            where (r.DrugId == (chk.DataContext as MasterListItem).ID
                           )
                            select r;
                if (item1.ToList().Count == 0)
                {
                        clsGetDrugForAllergies getdrugdata = new clsGetDrugForAllergies();
                        getdrugdata.DrugId = (chk.DataContext as MasterListItem).ID;
                        getdrugdata.DrugName = (chk.DataContext as MasterListItem).Description;
                        StoreDrug.Add(getdrugdata);
                }
                txtTarget.Text = String.Format(txtTarget.Text + "," + strDescription).Trim(',');
            }
            else if (!String.IsNullOrEmpty(txtDrug.Text))
            {
                 var item1 = from r in StoreDrug
                                where (r.DrugId == (chk.DataContext as MasterListItem).ID
                               ) select r;
                 if (item1.ToList().Count == 0)
                 {
                     foreach (var getrefinedate in item1.ToList())
                     {
                         clsGetDrugForAllergies getdrugdata = new clsGetDrugForAllergies();
                         getdrugdata.DrugId = getrefinedate.DrugId;
                         getdrugdata.DrugName = getrefinedate.DrugName;
                         StoreDrug.Add(getdrugdata);
                     }
                 }
                 else
                 {
                     var itemToRemove = StoreDrug.Single(r => r.DrugId == (chk.DataContext as MasterListItem).ID);
                     StoreDrug.Remove(itemToRemove);
                     chk.IsChecked = false;
                     txtTarget.Text = String.Format(txtTarget.Text.Replace(strDescription, String.Empty).Trim(',')).Trim();
                     txtTarget.Text = String.Format(txtTarget.Text.Replace(",,", ","));
                 }
            }
        }

        private void cmbMolecule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            flag = false;
            if (flag == true)
            {
                FillDrugList();
            }
        }

        private void acbAssChiefComplaints_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbMolecule.SelectedItem != null)
            {
                long z = ((MasterListItem)cmbMolecule.SelectedItem).ID;
            }
        }
              
        #endregion
    }
}
