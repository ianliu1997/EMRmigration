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

namespace PalashDynamics.Administration
{
    public partial class frmPackageRelation : ChildWindow
    {
        #region Data Members
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        ObservableCollection<clsPackageSourceRelationVO> RelationList;
        #endregion

        #region Properties
        public long PackageID { get; set; }
        public long PackageUnitID { get; set; }
        public string PackageName { get; set; }
        public long TariffID { get; set; }
        public bool IsChecked = true;

        bool IsPageLoaded = false;

        public string msgText = "";

        #endregion

        #region Constructor

        public frmPackageRelation()
        {
            InitializeComponent();

            //dgPackageRelations.ItemsSource = null;
            //RelationList = new ObservableCollection<clsPackageSourceRelationVO>();
            //  dgPackageRelations.ItemsSource = RelationList;
            //FillPatientCategory();
            //FillCompany();

            //FillPatientTariff();

            //FillPatientPackageRelation();

        }

        #endregion

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoaded)
            {
                FillPatientCategory();
                FillPatientSource();
                FillCompany();
                FillPatientTariff();
                FillPatientPackageRelation();
                IsPageLoaded = true;
            }
        }

        private void ClearField()
        {
            cboCategory.SelectedValue = (long)0;
            cboSource.SelectedValue = (long)0;
            cboCompny.SelectedValue = (long)0;
            cboSource.SelectedValue = (long)0;
        }

        private void FillPatientPackageRelation()
        {
            try
            {
                clsGetPackageSourceTariffCompanyListBizActionVO BizAction = new clsGetPackageSourceTariffCompanyListBizActionVO();
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
                BizAction.tariffID = TariffID;

                BizAction.PackageLinkingDetails = new List<clsPackageSourceRelationVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        dgPackageRelations.ItemsSource = ((clsGetPackageSourceTariffCompanyListBizActionVO)args.Result).PackageLinkingDetails;
                    }
                };
                client.ProcessAsync(BizAction, User);
                client.CloseAsync();
            }
            catch (Exception)
            {

            }
        }

        #region Fill ComboBox

        /// <summary>
        /// Method Used to fill the all patient Category required for the Package
        /// </summary>
        private void FillPatientCategory()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_PatientCategoryMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> lstCategory = new List<MasterListItem>();
                        lstCategory.Add(new MasterListItem(0, "-- Select --"));
                        lstCategory.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        cboCategory.ItemsSource = null;
                        cboCategory.ItemsSource = lstCategory;
                        cboCategory.SelectedItem = lstCategory[0];
                    }
                };
                client.ProcessAsync(BizAction, User);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method Used to fill the all patient Source required for the Package.
        /// </summary>
        private void FillPatientSource()
        {
            clsGetPatientSourceListByPatientCategoryIdBizActionVO BizAction = new clsGetPatientSourceListByPatientCategoryIdBizActionVO();


            if (cboCategory.SelectedItem != null)
            {
                BizAction.SelectedPatientCategoryIdID = Convert.ToInt64(((MasterListItem)cboCategory.SelectedItem).ID);
            }
            BizAction.PatientSourceDetails = new List<clsPatientSourceVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<clsPatientSourceVO> objList = new List<clsPatientSourceVO>();
                    objList.Add(new clsPatientSourceVO { ID = 0, Description = "-- Select --" });
                    objList.AddRange(((clsGetPatientSourceListByPatientCategoryIdBizActionVO)e.Result).PatientSourceDetails);
                    cboSource.ItemsSource = null;
                    cboSource.ItemsSource = objList;
                    cboSource.SelectedItem = objList[0];
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        //private void FillPatientSource()
        //{
        //    try
        //    {
        //        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //        BizAction.MasterTable = MasterTableNameList.M_PatientSourceMaster;
        //        BizAction.Parent = new KeyValue();
        //        BizAction.Parent.Key = (cboCategory.SelectedItem as MasterListItem).ID;
        //        BizAction.Parent.Value = "Status";
        //        BizAction.MasterList = new List<MasterListItem>();
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (s, args) =>
        //        {
        //            if (args.Error == null && args.Result != null)
        //            {
        //                List<MasterListItem> lstPatientSource = new List<MasterListItem>();
        //                lstPatientSource.Add(new MasterListItem(0, "-- Select --"));
        //                lstPatientSource.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
        //                cboSource.ItemsSource = null;
        //                cboSource.ItemsSource = lstPatientSource;
        //                cboSource.SelectedItem = lstPatientSource[0];
        //            }
        //        };
        //        client.ProcessAsync(BizAction, User);
        //        client.CloseAsync();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// Method Used to fill the all patient Tariff's required for the Package.
        /// </summary>
        private void FillPatientTariff()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_TariffMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> lstTariff = new List<MasterListItem>();
                        lstTariff.Add(new MasterListItem(0, "-- Select --"));
                        lstTariff.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        cboTariff.ItemsSource = null;
                        cboTariff.ItemsSource = lstTariff;
                        cboTariff.SelectedItem = lstTariff[0];

                        MasterListItem objPackageTariffItem = new MasterListItem();
                        objPackageTariffItem = lstTariff.Where(lstTar => lstTar.ID == TariffID).ToList().FirstOrDefault();

                        cboTariff.SelectedItem = objPackageTariffItem;

                    }
                };
                client.ProcessAsync(BizAction, User);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method Used to fill the all patient Tariff's required for the Package.
        /// </summary>
        private void FillCompany()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_CompanyMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> lstCompany = new List<MasterListItem>();
                        lstCompany.Add(new MasterListItem(0, "-- Select --"));
                        lstCompany.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        cboCompny.ItemsSource = null;
                        cboCompny.ItemsSource = lstCompany;
                        cboCompny.SelectedItem = lstCompany[0];

                        if (this.DataContext != null)
                        {

                            cboCompny.SelectedValue = ((clsPatientSponsorVO)this.DataContext).CompanyID;
                        }
                    }
                };
                client.ProcessAsync(BizAction, User);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }



        #endregion

        #region SelectionChanged Event
        /// <summary>
        /// Category Selection Chaned Event Get Handled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cboCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboTariff.SelectedItem != null)
            {
                cboCompny.Visibility = Visibility.Visible;
                if (cboCategory.SelectedItem != null)
                {
                    FillPatientSource();
                }

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientCategoryL1Id_Retail == ((MasterListItem)cboCategory.SelectedItem).ID)
                {
                    cboCompny.IsEnabled = false;
                    cboCompny.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfCompanyID;
                }
                else
                {
                    cboCompny.IsEnabled = true;
                    cboCompny.SelectedValue = (long)0;
                }

            }
        }

        /// <summary>
        /// Company Selection Chaned Event Get Handled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cboCompany_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        #endregion

        #region Button Add Event
        /// <summary>
        /// Add Button Click Event Get Handled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (ValidToAdd())
            {


                string msgTitle = "Palash";
                string msgText = "Are you sure you want to save the Package Linking?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();

                //if (!RelationList.Contains(objPackageRelation))
                //    RelationList.Add(objPackageRelation);
                //else
                //    ShowMessageBox("Object already present", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                //dgPackageRelations.UpdateLayout();

                //MasterListItem objMaster = new MasterListItem(0, "-- Select --");
                //cboCategory.SelectedItem = objMaster;
                //cboSource.SelectedItem = objMaster;
                //cboTariff.SelectedItem = objMaster;
                //cboCompny.SelectedItem = objMaster;

            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                StatusChange = false;
                //if (CheckDuplicasy())
                //{
                Save();
                //}
            }
        }


        private void Save()
        {
            try
            {
                clsAddPackageSourceTariffCompanyRelationsBizActionVO BizActionVO = new clsAddPackageSourceTariffCompanyRelationsBizActionVO();

                BizActionVO.PackageSourceRelation = new clsPackageSourceRelationVO();


                if (!StatusChange)
                {
                    BizActionVO.PackageSourceRelation.PatientCategoryL1ID = (cboCategory.SelectedItem as MasterListItem).ID;
                    BizActionVO.PackageSourceRelation.PatientCategoryL2ID = (cboSource.SelectedItem as clsPatientSourceVO).ID;
                    BizActionVO.PackageSourceRelation.CompanyID = (cboCompny.SelectedItem as MasterListItem).ID;
                }
                BizActionVO.PackageSourceRelation.PatientCategoryL3ID = (cboTariff.SelectedItem as MasterListItem).ID;   //this.PackageName;

                BizActionVO.PackageSourceRelation.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizActionVO.PackageSourceRelation.Status = IsChecked;

                if (dgPackageRelations.SelectedItem != null)
                    BizActionVO.PackageSourceRelation.ID = ((clsPackageSourceRelationVO)dgPackageRelations.SelectedItem).ID;


                //if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientCategoryL1Id_Retail == ((MasterListItem)cboCategory.SelectedItem).ID)
                //{
                //    objPackageRelation.IsSaveForL2 = true;
                //}

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {

                        if (((clsAddPackageSourceTariffCompanyRelationsBizActionVO)arg.Result).ResultSuccessStatus == 1)
                        {
                            //SetupPage();
                            msgText = "Package Category Linking Details Saved Successfully";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();

                            ClearField();
                            FillPatientPackageRelation();

                            ////After Updation Back to BackPanel and Setup Page
                            //objAnimation.Invoke(RotationType.Backward);


                            //SetCommandButtonState("Save");

                        }
                        else if (((clsAddPackageSourceTariffCompanyRelationsBizActionVO)arg.Result).ResultSuccessStatus == 2)
                        {
                            msgText = "Record cannot be added because selected Category already exist with this Package Tariff!";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                            ClearField();
                            FillPatientPackageRelation();
                        }
                        if (((clsAddPackageSourceTariffCompanyRelationsBizActionVO)arg.Result).ResultSuccessStatus == 3)
                        {
                            //SetupPage();
                            msgText = "Package Company Linking Details Updated Successfully";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                            ClearField();
                            FillPatientPackageRelation();

                            ////After Updation Back to BackPanel and Setup Page
                            //objAnimation.Invoke(RotationType.Backward);


                            //SetCommandButtonState("Save");

                        }
                        //else if (((clsAddPackageSourceTariffCompanyRelationsBizActionVO)arg.Result).ResultSuccessStatus == 4)
                        //{
                        //    msgText = "Record cannot be added because selected Company already exist with this Package Tariff!";
                        //    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //    msgWindow.Show();
                        //}
                    }

                };
                client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion

        #region Private Method
        private bool ValidToAdd()
        {
            Boolean blnValid = true;
            if (cboCategory.SelectedItem == null || ((MasterListItem)cboCategory.SelectedItem).ID == 0)
            {
                blnValid = false;
                cboCategory.TextBox.SetValidation("Please select the Patient Category L1");
                cboCategory.TextBox.RaiseValidationError();
                cboCategory.Focus();
            }
            else
                cboCategory.TextBox.ClearValidationError();

            if (cboSource.SelectedItem == null || ((clsPatientSourceVO)cboSource.SelectedItem).ID == 0)
            {
                blnValid = false;
                cboSource.TextBox.SetValidation("Please select the Patient Category L2");
                cboSource.TextBox.RaiseValidationError();
                cboSource.Focus();
            }
            else
                cboSource.TextBox.ClearValidationError();


            if (((MasterListItem)cboCategory.SelectedItem).Description == "Corporate" && (cboCompny.SelectedItem == null || ((MasterListItem)cboCompny.SelectedItem).ID == 0))
            {
                blnValid = false;
                cboCompny.TextBox.SetValidation("Please select the Company");
                cboCompny.TextBox.RaiseValidationError();
                cboCompny.Focus();
            }
            else
                cboCompny.TextBox.ClearValidationError();

            return blnValid;
        }
        #endregion

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", "You want to delete item ? ", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
            msgWindow.Show();
            msgWindow.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindow_OnMessageBoxClosed);
        }

        void msgWindow_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsPackageSourceRelationVO objPackageRelation = dgPackageRelations.SelectedItem as clsPackageSourceRelationVO;
                RelationList.Where(z => z.PatientCategoryL2 == objPackageRelation.PatientCategoryL2 && z.PatientCategoryL3 == objPackageRelation.PatientCategoryL3).FirstOrDefault();
                RelationList.Remove(objPackageRelation);
                dgPackageRelations.UpdateLayout();
            }
        }
        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        bool StatusChange = false;

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            StatusChange = false;
            CheckBox chk = (CheckBox)sender;
            IsChecked = (bool)chk.IsChecked;
            StatusChange = true;
            Save();

        }

        private void dgPackageRelations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPackageRelations.SelectedItem != null)
            {
                cboCategory.SelectedValue = ((clsPackageSourceRelationVO)dgPackageRelations.SelectedItem).PatientCategoryL1ID;
                cboSource.SelectedValue = ((clsPackageSourceRelationVO)dgPackageRelations.SelectedItem).PatientCategoryL2ID;
                cboCompny.SelectedValue = ((clsPackageSourceRelationVO)dgPackageRelations.SelectedItem).CompanyID;
            }
        }

        
    }
}

