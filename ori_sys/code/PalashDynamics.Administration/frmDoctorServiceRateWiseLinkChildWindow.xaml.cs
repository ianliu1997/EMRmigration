using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Collections;
using PalashDynamics.Animations;
using PalashDynamics.UserControls;
using CIMS;
using System.Text;
using PalashDynamics.ValueObjects.Master.DoctorMaster;
using System.Windows.Data;

namespace PalashDynamics.Administration
{
    public partial class frmDoctorServiceRateWiseLinkChildWindow : ChildWindow
    {
        #region Variable Declarations
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        public long DoctorID { get; set; }  // Coming from DoctorMaster form.
        public string DoctorName { get; set; } // Coming from DoctorMaster form.
        public long DocCategory { get; set; }// Coming from DoctorMaster form.
        bool IsFromNewClick = false;
        private SwivelAnimation objAnimation;
        WaitIndicator WIndicator = null;
        PagedCollectionView PCV;
        bool IsCancel = true;
        public PagedSortableCollectionView<clsServiceMasterVO> DataList { get; private set; }
        public PagedSortableCollectionView<clsServiceMasterVO> MasterList { get; private set; }
        private List<clsServiceMasterVO> SelectedServiceList;
        private List<clsServiceMasterVO> _OtherServiceSelected;
        private List<clsServiceMasterVO> DeletedServiceList;
        public int DataListPageSizeFP
        {
            get
            {
                return MasterList.PageSize;
            }
            set
            {
                if (value == MasterList.PageSize) return;
                MasterList.PageSize = value;
                // RaisePropertyChanged("DataListPageSize");
            }
        }
        public int DataListPageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
                // RaisePropertyChanged("DataListPageSize");
            }
        }

        #endregion

        #region Constructor and Loaded Event

        public frmDoctorServiceRateWiseLinkChildWindow()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            _OtherServiceSelected = new List<clsServiceMasterVO>();
            SelectedServiceList = new List<clsServiceMasterVO>();
            DeletedServiceList = new List<clsServiceMasterVO>();
            WIndicator = new WaitIndicator();
            //Front Pannel Paging
            MasterList = new PagedSortableCollectionView<clsServiceMasterVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            DataListPageSizeFP = 15;
            DataPagerDocFrontPannel.PageSize = DataListPageSizeFP;
            DataPagerDocFrontPannel.Source = MasterList;
            //Paging
            DataList = new PagedSortableCollectionView<clsServiceMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            DataPagerServices.PageSize = DataListPageSize;
            DataPagerServices.Source = DataList;
            //======================================================
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Load");
            //FillSpecialization();
            FillSelectedDoctorServiceLinkingByClinic(0, false);
            FillDoctorCategoryCombo();
            FillClinicCombo();
            FillClassComboBox();
            FillDataGridServices();
            this.Title = "Doctor-Service Linking" + " : " + DoctorName;
            cmbDocCategoryBP.SelectedValue = DocCategory;
        }
        #endregion

        #region Private Methods
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillDataGridServices();
        }

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillSelectedDoctorServiceLinkingByClinic(0, false);
        }

        private bool Validation(bool IsFromNewClick)
        {
            bool IsValidate = true;
            if ((MasterListItem)cmbClinic.SelectedItem == null)
            {
                cmbClinic.TextBox.SetValidation("Please select Clinic");
                cmbClinic.TextBox.RaiseValidationError();
                cmbClinic.Focus();
                IsValidate = false;
            }
            else if (((MasterListItem)cmbClinic.SelectedItem).ID == 0)
            {
                cmbClinic.TextBox.SetValidation("Please select Clinic");
                cmbClinic.TextBox.RaiseValidationError();
                cmbClinic.Focus();
                IsValidate = false;
            }
            else
                cmbClinic.TextBox.ClearValidationError();

            if (_OtherServiceSelected.ToList().Count == 0 && !IsFromNewClick)
            {
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Please select Services for Doctor Category", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                IsValidate = false;
            }
            return IsValidate;
        }

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    CmdModify.IsEnabled = false;
                    CmdSaveServices.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    CmdNew.IsEnabled = true;
                    cmbClinic.IsEnabled = false;
                    IsCancel = true;
                    lblClinic.Visibility = Visibility.Collapsed;
                    cmbClinic.Visibility = Visibility.Collapsed;
                    break;

                case "New":
                    CmdModify.IsEnabled = true;
                    CmdSaveServices.IsEnabled = true;
                    CmdNew.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    cmbClinic.IsEnabled = true;
                    IsCancel = false;
                    CmdModify.Content = "Save";
                    lblClinic.Visibility = Visibility.Visible;
                    cmbClinic.Visibility = Visibility.Visible;
                    break;

                case "Save":
                    CmdNew.IsEnabled = true;
                    CmdSaveServices.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    IsCancel = true;
                    lblClinic.Visibility = Visibility.Collapsed;
                    cmbClinic.Visibility = Visibility.Collapsed;
                    break;

                case "Modify":
                    CmdNew.IsEnabled = true;
                    CmdModify.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    cmbClinic.IsEnabled = false;
                    IsCancel = true;
                    lblClinic.Visibility = Visibility.Collapsed;
                    cmbClinic.Visibility = Visibility.Collapsed;
                    break;

                case "Cancel":
                    CmdNew.IsEnabled = true;
                    CmdModify.IsEnabled = false;
                    CmdSaveServices.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    lblClinic.Visibility = Visibility.Collapsed;
                    cmbClinic.Visibility = Visibility.Collapsed;
                    break;

                case "View":
                    CmdModify.IsEnabled = true;
                    CmdSaveServices.IsEnabled = false;
                    CmdNew.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    cmbClinic.IsEnabled = false;
                    IsCancel = false;
                    CmdModify.Content = "Modify";
                    lblClinic.Visibility = Visibility.Visible;
                    cmbClinic.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void FillDoctorCategoryCombo()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_DoctorCategoryMaster;
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
                    cmbDocCategory.ItemsSource = null;
                    cmbDocCategory.ItemsSource = objList;
                    cmbDocCategory.SelectedItem = objList[0];

                    // for BackPannel
                    cmbDocCategoryBP.ItemsSource = null;
                    cmbDocCategoryBP.ItemsSource = objList;
                    cmbDocCategoryBP.SelectedItem = DocCategory; //objList[0];
                    //
                }

                if (this.DataContext != null)
                {
                    cmbDocCategory.SelectedValue = ((clsDoctorVO)this.DataContext).DoctorCategoryId;
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        //private void FillSpecialization()
        //{
        //    try
        //    {
        //        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //        BizAction.MasterTable = MasterTableNameList.M_Specialization;
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

        //                objList.Add(new MasterListItem(0, "-- Select --"));
        //                objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

        //                cmbSpecialization.ItemsSource = null;
        //                cmbSpecialization.ItemsSource = objList;
        //                cmbSpecialization.SelectedItem = objList[0];

        //                //for BackPannel
        //                cmbSpecializationBP.ItemsSource = null;
        //                cmbSpecializationBP.ItemsSource = objList;
        //                cmbSpecializationBP.SelectedItem = objList[0];
        //                //
        //            }
        //        };
        //        client.ProcessAsync(BizAction, new clsUserVO());
        //        client.CloseAsync();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //private void FillSubSpecialization(string fkSpecializationID, bool IsBackPannel)
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.MasterTable = MasterTableNameList.M_SubSpecialization;
        //    BizAction.Parent = new KeyValue();
        //    BizAction.Parent.Key = "1";
        //    BizAction.Parent.Value = "Status";
        //    if (fkSpecializationID != null)
        //    {
        //        BizAction.Parent = new KeyValue { Key = fkSpecializationID, Value = "fkSpecializationID" };
        //    }
        //    BizAction.MasterList = new List<MasterListItem>();
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, args) =>
        //    {
        //        if (args.Error == null && args.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem(0, "-- Select --"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

        //            if (IsBackPannel) // IsBackPannel=true flag for filling the SubSpecialization of BackPannel Combo.
        //            {
        //                cmbSubSpecializationBP.ItemsSource = null;
        //                cmbSubSpecializationBP.ItemsSource = objList;
        //                cmbSubSpecializationBP.SelectedValue = objList[0].ID;
        //            }
        //            else
        //            {
        //                cmbSubSpecialization.ItemsSource = null;
        //                cmbSubSpecialization.ItemsSource = objList;
        //                cmbSubSpecialization.SelectedValue = objList[0].ID;
        //            }



        //        }
        //    };

        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}

        private void FillClinicCombo()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
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
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                        //Front Pannel ComboBox--------------
                        cmbUnitFP.ItemsSource = null;
                        cmbUnitFP.ItemsSource = objList;
                        cmbUnitFP.SelectedItem = objList[0];
                        //-----------------------------------
                        //Back Pannel ComboBox---------------
                        cmbClinic.ItemsSource = null;
                        cmbClinic.ItemsSource = objList;
                        cmbClinic.SelectedItem = objList[0];
                        //-----------------------------------
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Fill the Back Pannel Sevices..
        /// </summary>
        private void FillDataGridServices()
        {
            try
            {
                clsGetServiceMasterListBizActionVO BizAction = new clsGetServiceMasterListBizActionVO();
                BizAction.ServiceMaster = new clsServiceMasterVO();
                BizAction.ServiceList = new List<clsServiceMasterVO>();
                BizAction.IsFromDocSerRateCat = true;
                if (!string.IsNullOrEmpty(txtServiceNameBP.Text))
                    BizAction.ServiceName = txtServiceNameBP.Text;
                if (cmbClassName.SelectedItem != null) //((MasterListItem)cmbSpecializationBP.SelectedItem).ID != 0
                    BizAction.ServiceMaster.ClassID = ((MasterListItem)cmbClassName.SelectedItem).ID;
                BizAction.Specialization = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.ConsultationID;
                //if ((MasterListItem)cmbSubSpecializationBP.SelectedItem != null) //((MasterListItem)cmbSubSpecializationBP.SelectedItem).ID != 0
                //    BizAction.SubSpecialization = ((MasterListItem)cmbSubSpecializationBP.SelectedItem).ID;

                BizAction.IsPagingEnabled = true;
                BizAction.MaximumRows = DataList.PageSize;
                BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                dgServices.ItemsSource = null;
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList != null)
                        {
                            DataList.TotalItemCount = (int)(((clsGetServiceMasterListBizActionVO)arg.Result).TotalRows);
                            DataList.Clear();
                            BizAction.ServiceList = ((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList;

                            BizAction.ServiceList.Select(d => d.ServiceID = d.ID).ToList();

                            if (_OtherServiceSelected != null && _OtherServiceSelected.Count > 0)
                            {
                                foreach (clsServiceMasterVO item in _OtherServiceSelected.ToList())
                                {
                                    BizAction.ServiceList.ToList().ForEach(x =>
                                    {
                                        if (item.ServiceID == x.ServiceID && item.ClassID == x.ClassID)
                                            x.SelectService = true;
                                    });
                                }
                            }
                            foreach (var item in BizAction.ServiceList)
                            {
                                DataList.Add(item);
                            }
                            dgServices.ItemsSource = null;
                            dgServices.ItemsSource = DataList.DeepCopy();

                            DataPagerServices.Source = null;
                            DataPagerServices.PageSize = BizAction.MaximumRows;
                            DataPagerServices.Source = DataList;
                        }
                    }

                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }

                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Fill the BackPannel DataGrid(Selected Services)
        /// </summary>
        private void FillSelectedDoctorServiceLinkingByCategory()
        {
            try
            {
                WIndicator.Show();
                clsGetDoctorServiceLinkingByCategoryBizActionVO BizActionVO = new clsGetDoctorServiceLinkingByCategoryBizActionVO();
                BizActionVO.ServiceMasterDetails = new clsServiceMasterVO();
                BizActionVO.IsForClinic = false;
                BizActionVO.ServiceMasterDetails.ID = DoctorID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    BizActionVO.ServiceMasterDetailsList = new List<clsServiceMasterVO>();
                    WIndicator.Close();
                    if (arg.Error == null && arg.Result != null)
                    {
                        clsGetDoctorServiceLinkingByCategoryBizActionVO objVO = arg.Result as clsGetDoctorServiceLinkingByCategoryBizActionVO;
                        if (SelectedServiceList != null)
                            SelectedServiceList.Clear();
                        if (_OtherServiceSelected != null)
                            _OtherServiceSelected.Clear();
                        if (objVO != null)
                        {
                            if (objVO.ServiceMasterDetailsList.Count > 0)
                            {
                                //**Front Pannel DataGrid
                                //dgDocCategoryFrontP.ItemsSource = null;
                                //dgDocCategoryFrontP.ItemsSource = objVO.ServiceMasterDetailsList.ToList();
                                //dgDocCategoryFrontP.UpdateLayout();
                                //**
                                //cmbDocCategoryBP.SelectedValue = objVO.CategoryID;
                                foreach (var item in objVO.ServiceMasterDetailsList)
                                {
                                    SelectedServiceList.Add(item);
                                }
                                _OtherServiceSelected = SelectedServiceList;
                                foreach (var item in _OtherServiceSelected)
                                {
                                    item.SelectService = true;
                                }
                                dgDocCategoryBP.ItemsSource = null;
                                dgDocCategoryBP.ItemsSource = _OtherServiceSelected;
                                dgDocCategoryBP.UpdateLayout();
                            }
                        }
                        cmbDocCategoryBP.SelectedValue = DocCategory;

                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                WIndicator.Close();
                throw;
            }
            finally
            {
                FillDataGridServices();
            }
        }

        /// <summary>
        /// Fill the Front Pannel Services
        /// </summary>
        private void FillSelectedDoctorServiceLinkingByClinic(long UnitID, bool IsFromViewClick)
        {
            try
            {
                WIndicator.Show();
                clsGetDoctorServiceLinkingByCategoryBizActionVO BizActionVO = new clsGetDoctorServiceLinkingByCategoryBizActionVO();
                BizActionVO.ServiceMasterDetails = new clsServiceMasterVO();
                BizActionVO.IsForClinic = true;
                BizActionVO.ServiceMasterDetails.ID = DoctorID;

                if (UnitID > 0)
                    BizActionVO.UnitID = UnitID;
                else if (cmbUnitFP.SelectedItem != null && ((MasterListItem)cmbUnitFP.SelectedItem).ID > 0)
                    BizActionVO.UnitID = ((MasterListItem)cmbUnitFP.SelectedItem).ID;

                if (!string.IsNullOrEmpty(txtServiceName.Text))
                    BizActionVO.ServiceMasterDetails.ServiceName = txtServiceName.Text;
                if (cmbClassNameFP.SelectedItem != null)
                    BizActionVO.ServiceMasterDetails.ClassID = ((MasterListItem)cmbClassNameFP.SelectedItem).ID;
                if (!IsFromViewClick)
                {
                    BizActionVO.PagingEnabled = true;
                    BizActionVO.MaximumRows = MasterList.PageSize;
                    BizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    BizActionVO.ServiceMasterDetailsList = new List<clsServiceMasterVO>();
                    WIndicator.Close();
                    if (arg.Error == null && arg.Result != null)
                    {
                        clsGetDoctorServiceLinkingByCategoryBizActionVO objVO = arg.Result as clsGetDoctorServiceLinkingByCategoryBizActionVO;
                        if (objVO != null)
                        {
                            MasterList.Clear();
                            if (IsFromViewClick)
                            {
                                cmbClinic.SelectedValue = objVO.UnitID;
                                cmbClinic.IsEnabled = false;
                                if (SelectedServiceList != null)
                                    SelectedServiceList.Clear();
                                if (_OtherServiceSelected != null)
                                    _OtherServiceSelected.Clear();
                                if (objVO.ServiceMasterDetailsList.Count > 0)
                                {
                                    foreach (var item in objVO.ServiceMasterDetailsList)
                                    {
                                        SelectedServiceList.Add(item);
                                    }
                                    _OtherServiceSelected = SelectedServiceList;
                                    foreach (var item in _OtherServiceSelected)
                                    {
                                        item.SelectService = true;
                                    }
                                    dgDocCategoryBP.ItemsSource = null;
                                    dgDocCategoryBP.ItemsSource = _OtherServiceSelected;
                                    dgDocCategoryBP.UpdateLayout();
                                    FillDataGridServices();
                                }
                            }
                            else
                            {
                                MasterList.TotalItemCount = (int)((clsGetDoctorServiceLinkingByCategoryBizActionVO)arg.Result).TotalRows;
                                if (objVO.ServiceMasterDetailsList.Count > 0)
                                {
                                    foreach (var item in objVO.ServiceMasterDetailsList)
                                    {
                                        MasterList.Add(item);
                                    }
                                    dgDocCategoryFrontP.ItemsSource = null;
                                    PCV = new PagedCollectionView(MasterList);
                                    PCV.GroupDescriptions.Add(new PropertyGroupDescription("UnitName"));
                                    // PCV.GroupDescriptions.Add(new PropertyGroupDescription("CategoryId"));
                                    dgDocCategoryFrontP.ItemsSource = PCV;
                                    dgDocCategoryFrontP.SelectedIndex = -1;
                                    DataPagerDocFrontPannel.Source = null;
                                    DataPagerDocFrontPannel.PageSize = MasterList.PageSize;
                                    DataPagerDocFrontPannel.Source = MasterList;
                                    dgDocCategoryFrontP.UpdateLayout();
                                }
                            }
                        }
                        cmbDocCategoryBP.SelectedValue = DocCategory;
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                WIndicator.Close();
                throw;
            }
        }

        private void SaveServices()
        {
            try
            {
                WIndicator.Show();
                clsAddUpdateDoctorServiceLinkingByCategoryBizActionVO BizActionObj = new clsAddUpdateDoctorServiceLinkingByCategoryBizActionVO();
                if (DoctorID > 0)
                    BizActionObj.DoctorID = DoctorID;
                if (cmbDocCategoryBP.SelectedItem != null)
                    BizActionObj.CategoryID = ((MasterListItem)cmbDocCategoryBP.SelectedItem).ID;
                if (cmbClinic.SelectedItem != null)
                    BizActionObj.UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
                BizActionObj.IsModify = true;
                if (_OtherServiceSelected != null && _OtherServiceSelected.Count > 0)
                    BizActionObj.ServiceMasterDetailsList = _OtherServiceSelected.ToList();
                BizActionObj.DeletedServiceList = DeletedServiceList.ToList();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    WIndicator.Close();
                    if (arg.Error == null)
                    {
                        if ((clsAddUpdateDoctorServiceLinkingByCategoryBizActionVO)arg.Result != null)
                        {
                            string strMsg = string.Empty;
                            if (IsFromNewClick) strMsg = "Saved"; else strMsg = "Updated";
                            //FillSelectedDoctorServiceLinkingByCategory();
                            FillSelectedDoctorServiceLinkingByClinic(0, false);
                            cmbClinic.SelectedValue = (long)0;
                            SetCommandButtonState("Load");
                            objAnimation.Invoke(RotationType.Backward);
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Services " + strMsg + " Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                WIndicator.Close();
                throw;
            }
        }

        private void ClearUI()
        {
            if (DeletedServiceList != null)
            {
                DeletedServiceList.Clear();
                DeletedServiceList = new List<clsServiceMasterVO>();
            }
            if (_OtherServiceSelected != null)
            {
                _OtherServiceSelected.Clear();
                _OtherServiceSelected = new List<clsServiceMasterVO>();
            }
            dgDocCategoryBP.ItemsSource = null;
            if (SelectedServiceList != null)
            {
                SelectedServiceList.Clear();
                SelectedServiceList = new List<clsServiceMasterVO>();
            }

            txtServiceName.Text = string.Empty;
            cmbClassNameFP.SelectedValue = (long)0;
            cmbUnitFP.SelectedValue = (long)0;

            txtServiceNameBP.Text = string.Empty;
            cmbDocCategoryBP.SelectedValue = (long)0;
            cmbClassName.SelectedValue = (long)0;
            cmbClinic.SelectedValue = (long)0;
            //cmbSpecializationBP.SelectedValue = (long)0;
            //cmbSubSpecializationBP.SelectedValue = (long)0;
        }

        private void FillClassComboBox()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_ClassMaster;
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
                    cmbClassNameFP.ItemsSource = null;
                    cmbClassNameFP.ItemsSource = objList;
                    cmbClassNameFP.SelectedItem = objList[0];

                    // for BackPannel
                    cmbClassName.ItemsSource = null;
                    cmbClassName.ItemsSource = objList;
                    cmbClassName.SelectedItem = objList[0];
                    //
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        public void GetBackPannelClinicComboBoxStatus(long ID, string ClinicName)
        {
            clsGetUnSelectedRecordForCategoryComboBoxBizActionVO BizAction = new clsGetUnSelectedRecordForCategoryComboBoxBizActionVO();
            BizAction.DoctorID = DoctorID;
            BizAction.IsFromDocSerLinling = true;
            BizAction.UnitID = ID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    if (((clsGetUnSelectedRecordForCategoryComboBoxBizActionVO)e.Result).SuccessStatus == 2)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("", "'" + ClinicName + "'" + " Clinic already in use, Please select another one", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        CmdModify.IsEnabled = false;
                    }
                    else if (((clsGetUnSelectedRecordForCategoryComboBoxBizActionVO)e.Result).SuccessStatus == 1) CmdModify.IsEnabled = true;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        #endregion

        #region Button Click Events
        private void CmdNew_Click(object sender, RoutedEventArgs e)
        {
            ClearUI();
            Validation(true);
            FillSelectedDoctorServiceLinkingByCategory();
            cmbClinic.IsEnabled = true;
            cmbClinic.SelectedValue = (long)0;
            objAnimation.Invoke(RotationType.Forward);
            SetCommandButtonState("New");
            IsFromNewClick = true;

        }

        private void CmdSaveServices_Click(object sender, RoutedEventArgs e)
        {
            if (Validation(false))
            {
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Do You Want To Save the Services?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWindow.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        SaveServices();
                    }
                };
                msgWindow.Show();

            }
        }

        private void CmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (Validation(false))
            {
                string strMsg = string.Empty;
                if (IsFromNewClick) strMsg = "Save"; else strMsg = "Update";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Do You Want to " + strMsg + " the Services?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWindow.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        SaveServices();
                    }
                };
                msgWindow.Show();

            }
        }

        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            IsFromNewClick = true;
            SetCommandButtonState("Cancel");
            cmbClinic.IsEnabled = false;
            ClearUI();
            cmbClinic.SelectedValue = (long)0;
            objAnimation.Invoke(RotationType.Backward);
            if (IsCancel == true)
            {
                this.DialogResult = false;
            }
            else
            {
                FillSelectedDoctorServiceLinkingByClinic(0, false);
                IsCancel = true;
            }
        }

        private void cmdSearchBP_Click(object sender, RoutedEventArgs e)
        {
            FillDataGridServices();
            DataPagerServices.PageIndex = 0;
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FillSelectedDoctorServiceLinkingByClinic(0, false);
            DataPagerDocFrontPannel.PageIndex = 0;
        }

        private void hlkView_Click(object sender, RoutedEventArgs e)
        {
            IsFromNewClick = false;
            ClearUI();
            HyperlinkButton HY = sender as HyperlinkButton;
            long _UnitID = ((PalashDynamics.ValueObjects.Administration.clsServiceMasterVO)((((CollectionViewGroup)HY.DataContext)).Items[0])).UnitID;
            FillDataGridServices();
            FillSelectedDoctorServiceLinkingByClinic(_UnitID, true);
            SetCommandButtonState("View");
            objAnimation.Invoke(RotationType.Forward);
        }

        private void chkServices_Click(object sender, RoutedEventArgs e)
        {
            if (dgServices.SelectedItem != null)
            {
                clsServiceMasterVO objVO = (clsServiceMasterVO)dgDocCategoryBP.SelectedItem;
                clsServiceMasterVO objServiceVO = dgServices.SelectedItem as clsServiceMasterVO;
                if (_OtherServiceSelected == null)
                    _OtherServiceSelected = new List<clsServiceMasterVO>();
                CheckBox chk = (CheckBox)sender;
                StringBuilder strError = new StringBuilder();
                if (chk.IsChecked == true)
                {
                    if (_OtherServiceSelected.Count > 0)
                    {
                        var item = from r in _OtherServiceSelected
                                   where r.ServiceID == ((clsServiceMasterVO)dgServices.SelectedItem).ServiceID && r.ClassID == ((clsServiceMasterVO)dgServices.SelectedItem).ClassID
                                   select new clsServiceMasterVO
                                   {
                                       Status = r.Status,
                                       ServiceID = r.ServiceID,
                                       ServiceName = r.ServiceName
                                   };
                        if (item.ToList().Count > 0)
                        {
                            if (strError.ToString().Length > 0)
                                strError.Append(",");
                            strError.Append(((clsServiceMasterVO)dgServices.SelectedItem).ServiceName);

                            if (!string.IsNullOrEmpty(strError.ToString()))
                            {
                                string strMsg = "Services already Selected : " + strError.ToString();

                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                        }
                        else
                        {
                            _OtherServiceSelected.Add(((clsServiceMasterVO)dgServices.SelectedItem));
                        }
                    }
                    else
                    {
                        _OtherServiceSelected.Add(((clsServiceMasterVO)dgServices.SelectedItem));
                    }

                    dgDocCategoryBP.ItemsSource = null;
                    dgDocCategoryBP.ItemsSource = _OtherServiceSelected;
                    dgDocCategoryBP.UpdateLayout();
                    dgDocCategoryBP.Focus();

                }
                else
                {
                    if (objServiceVO != null)
                    {
                        clsServiceMasterVO obj;
                        obj = _OtherServiceSelected.Where(z => z.ServiceID == objServiceVO.ServiceID).FirstOrDefault();
                        if (obj != null)
                        {
                            foreach (var item in DataList)
                            {
                                if (item.ServiceID == obj.ServiceID && item.ClassID == obj.ClassID) item.SelectService = true;
                            }
                        }
                        dgServices.ItemsSource = null;
                        dgServices.ItemsSource = DataList;
                        DataPagerServices.Source = null;
                        DataPagerServices.Source = DataList;
                        dgServices.UpdateLayout();
                    }



                    //clsServiceMasterVO obj;
                    //if (objVO != null)
                    //{
                    //    obj = _OtherServiceSelected.Where(z => z.ServiceID == objVO.ServiceID).FirstOrDefault();
                    //    _OtherServiceSelected.Remove(obj);
                    //    //DataList.ToList().Where(z => z.ServiceID == obj.ServiceID).FirstOrDefault().SelectService = false;
                    //    foreach (var item in DataList)
                    //    {
                    //        if (item.ServiceID == obj.ServiceID)
                    //        {
                    //            item.SelectService = false;
                    //        }
                    //    }
                    //    dgServices.ItemsSource = null;
                    //    dgServices.ItemsSource = DataList;
                    //    DataPagerServices.Source = null;
                    //    DataPagerServices.Source = DataList;
                    //    dgServices.UpdateLayout();
                    //}
                    //else if (objServiceVO != null)
                    //{
                    //    obj = _OtherServiceSelected.Where(z => z.ServiceID == objServiceVO.ServiceID).FirstOrDefault();
                    //    _OtherServiceSelected.Remove(obj);
                    //    //DataList.ToList().Where(z => z.ServiceID == obj.ServiceID).FirstOrDefault().SelectService = false;
                    //    foreach (var item in DataList)
                    //    {
                    //        if (item.ServiceID == obj.ServiceID)
                    //        {
                    //            item.SelectService = false;
                    //        }
                    //    }
                    //    dgServices.ItemsSource = null;
                    //    dgServices.ItemsSource = DataList;
                    //    DataPagerServices.Source = null;
                    //    DataPagerServices.Source = DataList;
                    //    dgServices.UpdateLayout();
                    //}
                }



            }
        }

        private void cmdDeleteService_Click(object sender, RoutedEventArgs e)
        {
            if (dgDocCategoryBP.SelectedItem != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure, you want to delete " + "'" + (dgDocCategoryBP.SelectedItem as clsServiceMasterVO).ServiceName + "'" + " Service?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    clsServiceMasterVO objVO = (clsServiceMasterVO)dgDocCategoryBP.SelectedItem;
                    clsServiceMasterVO obj;
                    if (objVO != null)
                    {
                        obj = _OtherServiceSelected.Where(z => z.ServiceID == objVO.ServiceID && z.ClassID == objVO.ClassID).FirstOrDefault();
                        _OtherServiceSelected.Remove(obj);
                        DeletedServiceList.Add(obj);
                        //DataList.ToList().Where(z => z.ServiceID == obj.ServiceID).FirstOrDefault().SelectService = false;
                        foreach (var item in DataList)
                        {
                            if (item.ServiceID == obj.ServiceID && item.ClassID == obj.ClassID)
                            {
                                item.SelectService = false;
                            }
                        }
                        dgServices.ItemsSource = null;
                        dgServices.ItemsSource = DataList;
                        DataPagerServices.Source = null;
                        DataPagerServices.Source = DataList;
                        dgServices.UpdateLayout();
                    }
                    foreach (var item in _OtherServiceSelected)
                    {
                        item.SelectService = true;
                    }
                    dgDocCategoryBP.ItemsSource = null;
                    dgDocCategoryBP.ItemsSource = _OtherServiceSelected;
                    dgDocCategoryBP.UpdateLayout();
                    dgDocCategoryBP.Focus();
                };

                msgWD.Show();
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region Other Selection Events(SelectionChanged, KeyDown)
        private void cmbSpecializationBP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if ((MasterListItem)cmbSpecializationBP.SelectedItem != null)
            //{
            //    FillSubSpecialization(((MasterListItem)cmbSpecializationBP.SelectedItem).ID.ToString(), true);
            //}
        }

        private void txtServiceName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                FillSelectedDoctorServiceLinkingByClinic(0, false);
                DataPagerDocFrontPannel.PageIndex = 0;
            }
        }

        private void txtServiceNameBP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                FillDataGridServices();
                DataPagerServices.PageIndex = 0;
            }
        }

        private void txtRate_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)sender).Text) && !((TextBox)sender).Text.IsPositiveDoubleValid())     // IsValueDouble
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = String.Empty;
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtRate_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbClinic.SelectedItem != null && ((MasterListItem)cmbClinic.SelectedItem).ID > 0 && IsFromNewClick)
            {
                GetBackPannelClinicComboBoxStatus(((MasterListItem)cmbClinic.SelectedItem).ID, ((MasterListItem)cmbClinic.SelectedItem).Description);
            }
        }

        private void cmbSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        #endregion
    }
}

