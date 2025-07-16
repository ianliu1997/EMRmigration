using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Reflection;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Collections;
using System.Text;
using PalashDynamics.UserControls;
using System.Windows.Data;
using System.Windows.Input;

namespace PalashDynamics.Administration
{
    public partial class frmDoctorServiceRateCategory : UserControl
    {
        #region Variables Declaration
        private SwivelAnimation objAnimation;
        bool IsCancel = true;
        public PagedSortableCollectionView<clsServiceMasterVO> DataList { get; private set; }
        public PagedSortableCollectionView<clsServiceMasterVO> MasterList { get; private set; }

        PagedCollectionView PCV;
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
        private List<clsServiceMasterVO> _OtherServiceSelected;
        private List<clsServiceMasterVO> SelectedServiceList;
        private List<clsServiceMasterVO> DeletedServiceList;
        WaitIndicator WIndicator = null;
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        bool IsFromNewClick = false;
        #endregion

        #region Constructor and Loaded
        public frmDoctorServiceRateCategory()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            _OtherServiceSelected = new List<clsServiceMasterVO>();
            SelectedServiceList = new List<clsServiceMasterVO>();
            DeletedServiceList = new List<clsServiceMasterVO>();
            WIndicator = new WaitIndicator();
            //======================================================
            //Front Pannel Paging
            MasterList = new PagedSortableCollectionView<clsServiceMasterVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            DataListPageSizeFP = 15;
            DataPagerDocFrontPannel.PageSize = DataListPageSizeFP;
            DataPagerDocFrontPannel.Source = MasterList;
            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsServiceMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            DataPagerServices.PageSize = DataListPageSize;
            DataPagerServices.Source = DataList;
            //======================================================
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillDoctorCategoryComboBox();
            SetCommandButtonState("Load");
            //FillSpecialization();
            //FillDataGridServices();
            GetFrontPannelDataGridList();
            FillClassMasterComboBox();

        }
        #endregion

        #region Private methods
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    CmdModify.IsEnabled = false;
                    CmdSave.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    CmdNew.IsEnabled = true;
                    IsCancel = true;
                    break;

                case "New":
                    CmdModify.IsEnabled = false;
                    CmdSave.IsEnabled = true;
                    CmdNew.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;

                case "Save":
                    CmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;

                case "Modify":
                    CmdNew.IsEnabled = true;
                    CmdModify.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;

                case "Cancel":
                    CmdNew.IsEnabled = true;
                    CmdModify.IsEnabled = false;
                    CmdSave.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    break;

                case "View":
                    CmdModify.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdNew.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                default:
                    break;
            }
        }

        private void FillDoctorCategoryComboBox()
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

                    //**** for BackPannel Combo
                    cmbDocCategoryBP.ItemsSource = null;
                    cmbDocCategoryBP.ItemsSource = objList;
                    cmbDocCategoryBP.SelectedItem = objList[0];
                }

                if (this.DataContext != null)
                {
                    cmbDocCategory.SelectedValue = ((clsDoctorVO)this.DataContext).Specialization;
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillClassMasterComboBox()
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

                    //---Front Pannel Class ComboBox----------
                    cmbClassNameFP.ItemsSource = null;
                    cmbClassNameFP.ItemsSource = objList;
                    cmbClassNameFP.SelectedItem = objList[0];
                    //---Back Pannel Class ComboBox-----------
                    cmbClassName.ItemsSource = null;
                    cmbClassName.ItemsSource = objList;
                    cmbClassName.SelectedItem = objList[0];
                    //----------------------------------------
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillUnSelectedCategoryMasterComboBox()
        {
            clsGetUnSelectedRecordForCategoryComboBoxBizActionVO BizAction = new clsGetUnSelectedRecordForCategoryComboBoxBizActionVO();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetUnSelectedRecordForCategoryComboBoxBizActionVO)e.Result).MasterListForCombo);

                    //if (objList.Count() > 1)
                    //{
                    //    cmbDocCategoryBP.TextBox.SetValidation("You Do not have Category for Selection, Please Add Category in Category Master");
                    //    cmbDocCategoryBP.TextBox.RaiseValidationError();
                    //    cmbDocCategoryBP.Focus();
                    //}
                    //else
                    //{
                    //    cmbDocCategoryBP.TextBox.ClearValidationError();
                    cmbDocCategoryBP.ItemsSource = null;
                    cmbDocCategoryBP.ItemsSource = objList;
                    cmbDocCategoryBP.SelectedItem = objList[0];
                    // }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        public void GetBackPannelComboBoxStatus(long ID, string Desc)
        {
            clsGetUnSelectedRecordForCategoryComboBoxBizActionVO BizAction = new clsGetUnSelectedRecordForCategoryComboBoxBizActionVO();
            BizAction.IsFromDocSerLinling = false;
            BizAction.UnitID = ID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    //List<MasterListItem> objList = new List<MasterListItem>();
                    //objList.Add(new MasterListItem(0, "-- Select --"));
                    //objList.AddRange(((clsGetUnSelectedRecordForCategoryComboBoxBizActionVO)e.Result).MasterList);

                    //if (objList.Count() > 1)
                    //{
                    //    cmbDocCategoryBP.TextBox.SetValidation("You Do not have Category for Selection, Please Add Category in Category Master");
                    //    cmbDocCategoryBP.TextBox.RaiseValidationError();
                    //    cmbDocCategoryBP.Focus();
                    //}
                    //else
                    //{
                    //    cmbDocCategoryBP.TextBox.ClearValidationError();
                    //cmbDocCategoryBP.ItemsSource = null;
                    //cmbDocCategoryBP.ItemsSource = objList;
                    //cmbDocCategoryBP.SelectedItem = objList[0];
                    // }

                    if (((clsGetUnSelectedRecordForCategoryComboBoxBizActionVO)e.Result).SuccessStatus == 2)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("", "'" + Desc + "'" + " category already in use, Please select another one", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        CmdSave.IsEnabled = false;
                    }
                    else if (((clsGetUnSelectedRecordForCategoryComboBoxBizActionVO)e.Result).SuccessStatus == 1) CmdSave.IsEnabled = true;
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

        private void FillDataGridServices()
        {
            try
            {
                clsGetServiceMasterListBizActionVO BizAction = new clsGetServiceMasterListBizActionVO();
                BizAction.ServiceMaster = new clsServiceMasterVO();
                BizAction.ServiceList = new List<clsServiceMasterVO>();
                BizAction.IsFromDocSerRateCat = true;
                //if ((MasterListItem)cmbSpecializationBP.SelectedItem != null) //((MasterListItem)cmbSpecializationBP.SelectedItem).ID != 0
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.ConsultationID != null)
                    BizAction.Specialization = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.ConsultationID; //((MasterListItem)cmbSpecializationBP.SelectedItem).ID;
                if ((MasterListItem)cmbClassName.SelectedItem != null) //((MasterListItem)cmbSubSpecializationBP.SelectedItem).ID != 0
                    BizAction.ServiceMaster.ClassID = ((MasterListItem)cmbClassName.SelectedItem).ID;
                if (!string.IsNullOrEmpty(txtServiceNameBP.Text))
                    BizAction.ServiceName = txtServiceNameBP.Text;

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
                            //dgServices.ItemsSource = ((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList;
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
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

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

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillDataGridServices();
        }

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetFrontPannelDataGridList();
        }

        private bool Validation(bool IsFromNewClick)
        {
            bool IsValidate = true;
            if ((MasterListItem)cmbDocCategoryBP.SelectedItem == null)
            {
                cmbDocCategoryBP.TextBox.SetValidation("Please Select Clinic");
                cmbDocCategoryBP.TextBox.RaiseValidationError();
                cmbDocCategoryBP.Focus();
                IsValidate = false;
            }
            else if (((MasterListItem)cmbDocCategoryBP.SelectedItem).ID == 0)
            {
                cmbDocCategoryBP.TextBox.SetValidation("Please, Select Doctor Category");
                cmbDocCategoryBP.TextBox.RaiseValidationError();
                cmbDocCategoryBP.Focus();
                IsValidate = false;
            }
            else
                cmbDocCategoryBP.TextBox.ClearValidationError();

            if (_OtherServiceSelected.ToList().Count == 0 && !IsFromNewClick)
            {
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Please Select Service", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                IsValidate = false;
            }
            return IsValidate;
        }

        private void SaveServices(bool IsModify)
        {
            try
            {
                WIndicator.Show();
                clsAddUpdateDoctorServiceRateCategoryBizActionVO BizActionObj = new clsAddUpdateDoctorServiceRateCategoryBizActionVO();
                if ((MasterListItem)cmbDocCategoryBP.SelectedItem != null)
                    BizActionObj.CategoryID = ((MasterListItem)cmbDocCategoryBP.SelectedItem).ID;
                BizActionObj.IsModify = IsModify;
                BizActionObj.ServiceMasterDetailsList = _OtherServiceSelected.ToList();
                BizActionObj.DeletedServiceList = DeletedServiceList.ToList();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    WIndicator.Close();
                    if (arg.Error == null)
                    {
                        if ((clsAddUpdateDoctorServiceRateCategoryBizActionVO)arg.Result != null)
                        {
                            if (!IsModify)
                            {
                                GetFrontPannelDataGridList();
                                SetCommandButtonState("Load");
                                objAnimation.Invoke(RotationType.Backward);
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Doctor Service Rate Category Added Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                            else
                            {
                                GetFrontPannelDataGridList();
                                SetCommandButtonState("Load");
                                objAnimation.Invoke(RotationType.Backward);
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Doctor Service Rate Category Updated Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

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

            cmbDocCategory.SelectedValue = (long)0;
            txtServiceName.Text = string.Empty;
            cmbClassNameFP.SelectedValue = (long)0;

            cmbDocCategoryBP.SelectedValue = (long)0;
            txtServiceNameBP.Text = string.Empty;
            cmbClassName.SelectedValue = (long)0;
            //cmbSpecializationBP.SelectedValue = (long)0;
            //cmbSubSpecializationBP.SelectedValue = (long)0;
        }

        private void GetFrontPannelDataGridList()
        {
            try
            {
                WIndicator.Show();
                clsGetFrontPannelDataGridListBizActionVO BizActionVO = new clsGetFrontPannelDataGridListBizActionVO();
                BizActionVO.ServiceMasterDetails = new clsServiceMasterVO();
                BizActionVO.CategoryID = 0;
                BizActionVO.ServiceMasterDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                if (cmbDocCategory.SelectedItem != null)
                    BizActionVO.ServiceMasterDetails.CategoryID = ((MasterListItem)cmbDocCategory.SelectedItem).ID;
                if (cmbClassNameFP.SelectedItem != null)
                    BizActionVO.ServiceMasterDetails.ClassID = ((MasterListItem)cmbClassNameFP.SelectedItem).ID;
                //if (cmbSubSpecialization.SelectedItem != null)
                //    BizActionVO.ServiceMasterDetails.SubSpecialization = ((MasterListItem)cmbSubSpecialization.SelectedItem).ID;
                if (!string.IsNullOrEmpty(txtServiceName.Text))
                    BizActionVO.ServiceMasterDetails.ServiceName = txtServiceName.Text;

                BizActionVO.PagingEnabled = true;
                BizActionVO.MaximumRows = MasterList.PageSize;
                BizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    BizActionVO.ServiceMasterDetailsList = new List<clsServiceMasterVO>();
                    WIndicator.Close();
                    if (arg.Error == null && arg.Result != null)
                    {
                        clsGetFrontPannelDataGridListBizActionVO objVO = arg.Result as clsGetFrontPannelDataGridListBizActionVO;
                        MasterList.Clear();
                        if (objVO != null)
                        {
                            MasterList.TotalItemCount = (int)((clsGetFrontPannelDataGridListBizActionVO)arg.Result).TotalRows;
                            if (objVO.ServiceMasterDetailsList.Count > 0)
                            {
                                foreach (var item in objVO.ServiceMasterDetailsList)
                                {
                                    MasterList.Add(item);
                                }
                                dgDocCategoryFrontP.ItemsSource = null;
                                PCV = new PagedCollectionView(MasterList);
                                PCV.GroupDescriptions.Add(new PropertyGroupDescription("CategoryName"));
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
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

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

        private void GetServicesListById(long lCategortId)
        {
            try
            {
                WIndicator.Show();
                clsGetFrontPannelDataGridListBizActionVO BizActionVO = new clsGetFrontPannelDataGridListBizActionVO();
                BizActionVO.ServiceMasterDetails = new clsServiceMasterVO();
                BizActionVO.CategoryID = lCategortId;
                BizActionVO.ServiceMasterDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //if (cmbDocCategory.SelectedItem != null && ((MasterListItem)cmbDocCategory.SelectedItem).ID > 0)
                //BizActionVO.ServiceMasterDetails.CategoryID = ((MasterListItem)cmbDocCategory.SelectedItem).ID;
                //if (cmbSpecialization.SelectedItem != null)
                //    BizActionVO.ServiceMasterDetails.Specialization = ((MasterListItem)cmbSpecialization.SelectedItem).ID;
                //if (cmbSubSpecialization.SelectedItem != null)
                //    BizActionVO.ServiceMasterDetails.SubSpecialization = ((MasterListItem)cmbSubSpecialization.SelectedItem).ID;
                //if (!string.IsNullOrEmpty(txtServiceName.Text))
                //  BizActionVO.ServiceMasterDetails.ServiceName = txtServiceName.Text;

                //BizActionVO.PagingEnabled = true;
                //BizActionVO.MaximumRows = MasterList.PageSize;
                //BizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    BizActionVO.ServiceMasterDetailsList = new List<clsServiceMasterVO>();
                    WIndicator.Close();
                    if (arg.Error == null && arg.Result != null)
                    {
                        clsGetFrontPannelDataGridListBizActionVO objVO = arg.Result as clsGetFrontPannelDataGridListBizActionVO;
                        if (SelectedServiceList != null)
                            SelectedServiceList.Clear();
                        if (_OtherServiceSelected != null)
                            _OtherServiceSelected.Clear();
                        if (objVO != null)
                        {
                            cmbDocCategoryBP.SelectedValue = objVO.CategoryID;
                            cmbDocCategoryBP.IsEnabled = false;
                            foreach (clsServiceMasterVO item in objVO.ServiceMasterDetailsList.ToList())
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
                        FillDataGridServices();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

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

        #endregion

        #region Button Click Events
        private void CmdNew_Click(object sender, RoutedEventArgs e)
        {
            IsFromNewClick = true;
            ClearUI();
            Validation(true);
            FillDataGridServices();
            objAnimation.Invoke(RotationType.Forward);
            cmbDocCategoryBP.IsEnabled = true;
            SetCommandButtonState("New");
            //FillUnSelectedCategoryMasterComboBox();
        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation(false))
            {
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Do You Want To Save the Services?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWindow.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        SaveServices(false);
                    }
                };
                msgWindow.Show();

            }

        }

        private void CmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (Validation(false))
            {
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Do You Want To Update the Services?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWindow.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        SaveServices(true);
                    }
                };
                msgWindow.Show();

            }
        }

        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            objAnimation.Invoke(RotationType.Backward);
            ClearUI();
            IsFromNewClick = false;
            if (IsCancel == true)
            {
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Billing Configuration";
                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmBillingConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            else
            {
                IsCancel = true;
                GetFrontPannelDataGridList();
                //DataPagerDocFrontPannel.PageIndex = 0;
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            GetFrontPannelDataGridList();
            DataPagerDocFrontPannel.PageIndex = 0;

        }

        private void hlkView_Click(object sender, RoutedEventArgs e)
        {
            ClearUI();
            IsFromNewClick = false;
            HyperlinkButton HY = sender as HyperlinkButton;
            //long catid = ((PalashDynamics.ValueObjects.Administration.clsServiceMasterVO)((((CollectionViewGroup)HY.DataContext)).Items[0])).CategoryID;
            SelectedServiceList = new List<clsServiceMasterVO>();
            GetServicesListById(((PalashDynamics.ValueObjects.Administration.clsServiceMasterVO)((((CollectionViewGroup)HY.DataContext)).Items[0])).CategoryID);
            SetCommandButtonState("View");
            objAnimation.Invoke(RotationType.Forward);

        }

        private void cmdSearchBP_Click(object sender, RoutedEventArgs e)
        {
            FillDataGridServices();
            if (DataPagerServices.PageIndex > 0)
                DataPagerServices.PageIndex = 0;
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
                            _OtherServiceSelected.Add((clsServiceMasterVO)dgServices.SelectedItem);
                        }
                    }
                    else
                    {
                        _OtherServiceSelected.Add((clsServiceMasterVO)dgServices.SelectedItem);
                    }
                    foreach (var item in _OtherServiceSelected)
                    {
                        item.SelectService = true;
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

                    //    clsServiceMasterVO obj;
                    //    if (objVO != null)
                    //    {
                    //        obj = _OtherServiceSelected.Where(z => z.ServiceID == objVO.ServiceID).FirstOrDefault();
                    //        _OtherServiceSelected.Remove(obj);
                    //        //DataList.ToList().Where(z => z.ServiceID == obj.ServiceID).FirstOrDefault().SelectService = false;
                    //        foreach (var item in DataList)
                    //        {
                    //            if (item.ServiceID == obj.ServiceID)
                    //            {
                    //                item.SelectService = false;
                    //            }
                    //        }
                    //        dgServices.ItemsSource = null;
                    //        dgServices.ItemsSource = DataList;
                    //        DataPagerServices.Source = null;
                    //        DataPagerServices.Source = DataList;
                    //        dgServices.UpdateLayout();
                    //    }
                    //    else if (objServiceVO != null)
                    //    {
                    //        obj = _OtherServiceSelected.Where(z => z.ServiceID == objServiceVO.ServiceID).FirstOrDefault();
                    //        _OtherServiceSelected.Remove(obj);
                    //        //DataList.ToList().Where(z => z.ServiceID == obj.ServiceID).FirstOrDefault().SelectService = false;
                    //        foreach (var item in DataList)
                    //        {
                    //            if (item.ServiceID == obj.ServiceID)
                    //            {
                    //                item.SelectService = false;
                    //            }
                    //        }
                    //        dgServices.ItemsSource = null;
                    //        dgServices.ItemsSource = DataList;
                    //        DataPagerServices.Source = null;
                    //        DataPagerServices.Source = DataList;
                    //        dgServices.UpdateLayout();
                    //    }
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
                    if (res == MessageBoxResult.Yes)
                    {
                        clsServiceMasterVO objVO = (clsServiceMasterVO)dgDocCategoryBP.SelectedItem;
                        clsServiceMasterVO objServiceVO = dgServices.SelectedItem as clsServiceMasterVO;
                        clsServiceMasterVO obj;
                        if (objVO != null)
                        {
                            obj = _OtherServiceSelected.Where(z => z.ServiceID == objVO.ServiceID).FirstOrDefault();
                            _OtherServiceSelected.Remove(obj);
                            DeletedServiceList.Add(obj); //
                            //DataList.ToList().Where(z => z.ServiceID == obj.ServiceID).FirstOrDefault().SelectService = false;
                            foreach (var item in DataList)
                            {
                                if (item.ServiceID == obj.ServiceID)
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
                        else if (objServiceVO != null)
                        {
                            obj = _OtherServiceSelected.Where(z => z.ServiceID == objServiceVO.ServiceID).FirstOrDefault();
                            _OtherServiceSelected.Remove(obj);
                            //DataList.ToList().Where(z => z.ServiceID == obj.ServiceID).FirstOrDefault().SelectService = false;
                            foreach (var item in DataList)
                            {
                                if (item.ServiceID == obj.ServiceID)
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
                    }
                };

                msgWD.Show();
            }
        }
        #endregion

        #region Other Selection Events(SelectionChanged, KeyDown)
        //private void cmbSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if ((MasterListItem)cmbSpecialization.SelectedItem != null)
        //    {
        //        FillSubSpecialization(((MasterListItem)cmbSpecialization.SelectedItem).ID.ToString(), false);
        //    }
        //}

        //private void cmbSpecializationBP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if ((MasterListItem)cmbSpecializationBP.SelectedItem != null)
        //    {
        //        FillSubSpecialization(((MasterListItem)cmbSpecializationBP.SelectedItem).ID.ToString(), true);
        //    }
        //}

        private void txtServiceName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                GetFrontPannelDataGridList();
                DataPagerDocFrontPannel.PageIndex = 0;
            }

        }

        private void txtServiceNameBP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                FillDataGridServices();
                if (DataPagerServices.PageIndex > 0)
                    DataPagerServices.PageIndex = 0;
            }
        }

        private void txtRate_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtRate_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)sender).Text) && !((TextBox)sender).Text.IsPositiveDoubleValid())       //IsValueDouble
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = String.Empty;
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void cmbDocCategoryBP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDocCategoryBP.SelectedItem != null && ((MasterListItem)cmbDocCategoryBP.SelectedItem).ID > 0 && IsFromNewClick)
            {
                GetBackPannelComboBoxStatus(((MasterListItem)cmbDocCategoryBP.SelectedItem).ID, ((MasterListItem)cmbDocCategoryBP.SelectedItem).Description);
            }
        }

        private void chk_Click(object sender, RoutedEventArgs e)
        {
            //CheckBox chk = sender as CheckBox;
            //List<clsServiceMasterVO> tempList = new List<clsServiceMasterVO>();
            //bool check = chk.IsChecked.Value;
            //if (check)
            //{
            //    foreach (var item in DataList.ToList())
            //    {
            //        item.SelectService = true;
            //        tempList.Add(item);
            //    }

            //    _OtherServiceSelected = tempList.ToList();

            //    dgServices.ItemsSource = null;
            //    dgServices.ItemsSource = DataList;
            //    DataPagerServices.Source = null;
            //    DataPagerServices.Source = DataList;
            //    dgServices.UpdateLayout();


            //    dgDocCategoryBP.ItemsSource = null;
            //    dgDocCategoryBP.ItemsSource = _OtherServiceSelected;
            //    dgDocCategoryBP.UpdateLayout();
            //    dgDocCategoryBP.Focus();

            //}
            //else
            //{
            //    foreach (var p in DataList.ToList())
            //    {
            //        chk = dgServices.Columns[0].GetCellContent(p) as CheckBox;
            //        if (chk != null)
            //            chk.IsChecked = false;
            //    }
            //    tempList.Clear();
            //}
        }

        private void dgServices_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            CheckBox chk = (CheckBox)this.dgServices.Columns[0].GetCellContent(e.Row);
        }
        #endregion

    }
}
