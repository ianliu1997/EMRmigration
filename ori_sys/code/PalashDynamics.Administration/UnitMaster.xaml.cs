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
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Administration.UnitMaster;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Master;
using System.Reflection;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration;

namespace PalashDynamics.Administration
{
    public partial class UnitMaster : UserControl
    {
        public UnitMaster()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));


            DataList = new PagedSortableCollectionView<clsUnitMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            this.grdMaster.DataContext = DataList;
            this.DataPager.DataContext = DataList;
            FetchData();

        }

        #region Variable Declaration

        private SwivelAnimation objAnimation;
        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
        public PagedSortableCollectionView<clsUnitMasterVO> DataList { get; private set; }
        public List<clsDepartmentDetailsVO> DepartmentItemSource { get; set; }
        bool IsCancel = true;

        #endregion

        #region Paging
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

        private void UnitMaster_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                this.DataContext = new clsUnitMasterVO
                {
                    Country = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Country,
                    State = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.State,
                    District = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.District,
                    Pincode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Pincode,
                    City = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.City,
                    Area = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Area,
                };
                SetCommandButtonState("Load");
                //FillCountryList(); // cmt by Akshays
                FillCountry(); //add by Akshays
                //CheckValidation();
                //FetchData();

                FillDepartmentGrid();
                txtCode.Focus();
                FillCluster();
                Indicatior.Close();
            }
            txtCode.Focus();
            txtCode.UpdateLayout();
            IsPageLoded = true;
        }

        #region Combobox

        private void FillCountryList()
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            //BizAction.IsActive = true;
            BizAction.TableName = "M_UnitMaster";
            BizAction.ColumnName = "Country";
            BizAction.IsDecode = false;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtCountry.ItemsSource = null;
                    txtCountry.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List;
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private void FillStateList(string pCountry)
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            //BizAction.IsActive = true;
            BizAction.TableName = "M_UnitMaster";
            BizAction.ColumnName = "State";
            BizAction.IsDecode = false;

            if (!string.IsNullOrEmpty(pCountry))
            {
                pCountry = pCountry.Trim();
                BizAction.Parent = new KeyValue { Key = "Country", Value = pCountry };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtState.ItemsSource = null;
                    txtState.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List;
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();


        }

        private void FillCityList(string pState)
        {


            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            //BizAction.IsActive = true;
            BizAction.TableName = "M_UnitMaster";
            BizAction.ColumnName = "City";
            BizAction.IsDecode = false;

            if (!string.IsNullOrEmpty(pState))
            {
                pState = pState.Trim();
                BizAction.Parent = new KeyValue { Key = "State", Value = pState };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtCity.ItemsSource = null;
                    txtCity.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void GetDistrictList(string pState)
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            //BizAction.IsActive = true;
            BizAction.TableName = "M_UnitMaster";
            BizAction.ColumnName = "District";
            BizAction.IsDecode = false;

            if (!string.IsNullOrEmpty(pState))
            {
                pState = pState.Trim();
                BizAction.Parent = new KeyValue { Key = "State", Value = pState };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    //txtDistrict.ItemsSource = null;
                    //txtDistrict.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void GetTalukaList(string pState)
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            //BizAction.IsActive = true;
            BizAction.TableName = "M_UnitMaster";
            BizAction.ColumnName = "Taluka";
            BizAction.IsDecode = false;

            if (!string.IsNullOrEmpty(pState))
            {
                pState = pState.Trim();
                BizAction.Parent = new KeyValue { Key = "State", Value = pState };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    //txtTaluka.ItemsSource = null;
                    // txtTaluka.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void GetAreaList(string pCity)
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            //BizAction.IsActive = true;
            BizAction.TableName = "M_UnitMaster";
            BizAction.ColumnName = "Area";
            BizAction.IsDecode = false;

            if (!string.IsNullOrEmpty(pCity))
            {
                pCity = pCity.Trim();
                BizAction.Parent = new KeyValue { Key = "City", Value = pCity };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    txtArea.ItemsSource = null;
                    txtArea.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List;
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private void GetPinCodeList(string pArea)
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            //BizAction.IsActive = true;
            BizAction.TableName = "M_UnitMaster";
            BizAction.ColumnName = "Pincode";
            BizAction.IsDecode = false;

            if (!string.IsNullOrEmpty(pArea))
            {
                pArea = pArea.Trim();
                BizAction.Parent = new KeyValue { Key = "Area", Value = pArea };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtPinCode.ItemsSource = null;
                    txtPinCode.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }




        #endregion

        private void FillDepartmentGrid()
        {
            clsGetDepartmentListBizActionVO BizAction = new clsGetDepartmentListBizActionVO();

            BizAction.UnitDetails = new List<clsUnitMasterVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            dgDepartmentList.ItemsSource = null;
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetDepartmentListBizActionVO)arg.Result).UnitDetails != null)
                    {
                        BizAction.UnitDetails = ((clsGetDepartmentListBizActionVO)arg.Result).UnitDetails;
                        List<clsDepartmentDetailsVO> userDepartmentList = new List<clsDepartmentDetailsVO>();

                        foreach (var item in BizAction.UnitDetails)
                        {
                            if (item.IsActive == true)
                            {
                                userDepartmentList.Add(new clsDepartmentDetailsVO() { DepartmentID = item.DepartmentID, Code = item.Code, Department = item.Department, Status = false, IsDefault = false, IsActive = item.IsActive });
                            }
                        }
                        //var _tempList = from r in userDepartmentList
                        //                where r.Status == true
                        //                select r;
                        dgDepartmentList.ItemsSource = userDepartmentList;//_tempList.ToList();

                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillCluster()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ClusterMaster;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                    cmbCluster.ItemsSource = null;
                    cmbCluster.ItemsSource = objList;
                    cmbCluster.SelectedItem = objList[0];
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }


        private void FetchData()
        {
            clsGetUnitMasterListBizActionVO BizAction = new clsGetUnitMasterListBizActionVO();
            BizAction.UnitDetails = new List<clsUnitMasterVO>();

            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //grdMaster.ItemsSource = null;
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetUnitMasterListBizActionVO)arg.Result).UnitDetails != null)
                    {
                        //grdMaster.ItemsSource = ((clsGetUnitMasterListBizActionVO)arg.Result).UnitDetails;
                        clsGetUnitMasterListBizActionVO result = arg.Result as clsGetUnitMasterListBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;

                        if (result.UnitDetails != null)
                        {
                            DataList.Clear();

                            foreach (var item in result.UnitDetails)
                            {
                                DataList.Add(item);
                            }
                        }
                    }
                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();

        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("New");
            this.DataContext = new clsUnitMasterVO();
            ClearControl();
            txtResiCountryCode.WaterMark = "Country";
            txtResiSTD.WaterMark = "STD";
            txtContactNo.WaterMark = "Phone Number";


            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Clinic Details";
            objAnimation.Invoke(RotationType.Forward);
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            this.DataContext = null;

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";

            objAnimation.Invoke(RotationType.Backward);

            if (IsCancel == true)
            {
                mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Clinic Configuration";

                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmClinicConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

            }
            else
            {
                IsCancel = true;
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            bool SaveUnit = true;

            SaveUnit = CheckValidation();

            if (SaveUnit == true)
            {

                string msgTitle = "";
                string msgText = "Are you sure you want to save the Unit Master?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();

            }

        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                SaveUnitMaster();
        }

        private void SaveUnitMaster()
        {
            Indicatior.Show();
            clsAddUnitMasterBizActionVO BizAction = new clsAddUnitMasterBizActionVO();
            BizAction.UnitDetails = (clsUnitMasterVO)this.DataContext;

            BizAction.UnitDetails.ContactNo = txtContactNo.Text.Trim();
            //Added by neena
            BizAction.UnitDetails.ContactNo1 = txtContactNo1.Text.Trim();
            BizAction.UnitDetails.MobileNO = txtMobileNo.Text.Trim();
            if (!string.IsNullOrEmpty(txtMobCountryCode.Text.Trim()))
                BizAction.UnitDetails.MobileCountryCode = int.Parse(txtMobCountryCode.Text.Trim());
            //
            if (!string.IsNullOrEmpty(txtResiCountryCode.Text.Trim()))
                BizAction.UnitDetails.ResiNoCountryCode = int.Parse(txtResiCountryCode.Text.Trim());

            if (!string.IsNullOrEmpty(txtResiSTD.Text.Trim()))
                BizAction.UnitDetails.ResiSTDCode = int.Parse(txtResiSTD.Text.Trim());
            if ((MasterListItem)cmbCluster.SelectedItem != null)
                BizAction.UnitDetails.ClusterID = ((MasterListItem)cmbCluster.SelectedItem).ID;
            //added by akshays

            if (txtCountry.SelectedItem != null)
                BizAction.UnitDetails.Countryid = ((MasterListItem)txtCountry.SelectedItem).ID;

            if (txtState.SelectedItem != null)
                BizAction.UnitDetails.Stateid = ((MasterListItem)txtState.SelectedItem).ID;

            if (txtCity.SelectedItem != null)
                BizAction.UnitDetails.Cityid = ((MasterListItem)txtCity.SelectedItem).ID;

            if (txtArea.SelectedItem != null)
                BizAction.UnitDetails.Areaid = ((MasterListItem)txtArea.SelectedItem).ID;
            //closed by akshay

            //added by roihini dated 5.2.16 as per client requirement for patho dispatch
            if (chkIsProcessingUnit.IsChecked == true)
                BizAction.UnitDetails.IsProcessingUnit = true;
            else
                BizAction.UnitDetails.IsProcessingUnit = false;
            if (chkIsCollectionUnit.IsChecked == true)
                BizAction.UnitDetails.IsCollectionUnit = true;
            else
                BizAction.UnitDetails.IsCollectionUnit = false;
            //

            BizAction.UnitDetails.DepartmentDetails = (List<clsDepartmentDetailsVO>)dgDepartmentList.ItemsSource;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {

                if (arg.Error == null)
                {
                    if (((clsAddUnitMasterBizActionVO)arg.Result).SuccessStatus == -9)
                    {
                        string msgText = "";
                        msgText = "Can not save clinic : You can save maximum no of Clinics specified while subscription";

                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                        Indicatior.Close();
                    }
                    else
                    {
                        FetchData();
                        ClearControl();
                        objAnimation.Invoke(RotationType.Backward);
                        Indicatior.Close();


                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Clinic Master Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }
                SetCommandButtonState("Save");


            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            bool ModifyUnit = true;
            ModifyUnit = CheckValidation();
            if (ModifyUnit == true)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Update the Unit Master?";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

                msgW1.Show();
            }
        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                ModifyUnitMaster();
        }

        private void ModifyUnitMaster()
        {
            Indicatior.Show();
            clsAddUnitMasterBizActionVO BizAction = new clsAddUnitMasterBizActionVO();
            BizAction.UnitDetails = (clsUnitMasterVO)this.DataContext;
            BizAction.UnitDetails.UnitID = ((clsUnitMasterVO)grdMaster.SelectedItem).UnitID;

            BizAction.UnitDetails.ContactNo = txtContactNo.Text.Trim();
            //added by neena
            BizAction.UnitDetails.ContactNo1 = txtContactNo1.Text.Trim();
            BizAction.UnitDetails.MobileNO = txtMobileNo.Text.Trim();
            if (!string.IsNullOrEmpty(txtMobCountryCode.Text.Trim()))
                BizAction.UnitDetails.MobileCountryCode = int.Parse(txtMobCountryCode.Text.Trim());
            //
            if (!string.IsNullOrEmpty(txtResiCountryCode.Text.Trim()))
                BizAction.UnitDetails.ResiNoCountryCode = int.Parse(txtResiCountryCode.Text.Trim());
            if (!string.IsNullOrEmpty(txtResiSTD.Text.Trim()))
                BizAction.UnitDetails.ResiSTDCode = int.Parse(txtResiSTD.Text.Trim());

            if ((MasterListItem)cmbCluster.SelectedItem != null)
                BizAction.UnitDetails.ClusterID = ((MasterListItem)cmbCluster.SelectedItem).ID;
            BizAction.UnitDetails.DepartmentDetails = (List<clsDepartmentDetailsVO>)dgDepartmentList.ItemsSource;
            //added by akshays

            if (txtCountry.SelectedItem != null)
                BizAction.UnitDetails.Countryid = ((MasterListItem)txtCountry.SelectedItem).ID;

            if (txtState.SelectedItem != null)
                BizAction.UnitDetails.Stateid = ((MasterListItem)txtState.SelectedItem).ID;

            if (txtCity.SelectedItem != null)
                BizAction.UnitDetails.Cityid = ((MasterListItem)txtCity.SelectedItem).ID;

            if (txtArea.SelectedItem != null)
                BizAction.UnitDetails.Areaid = ((MasterListItem)txtArea.SelectedItem).ID;
            //closed by akshay


            //added by roihini dated 5.2.16 as per client requirement for patho dispatch
            if(chkIsProcessingUnit.IsChecked==true)
                BizAction.UnitDetails.IsProcessingUnit = true;
            else
                BizAction.UnitDetails.IsProcessingUnit = false;
            if (chkIsCollectionUnit.IsChecked == true)
                BizAction.UnitDetails.IsCollectionUnit = true;
            else
                BizAction.UnitDetails.IsCollectionUnit = false;
            //
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {

                if (arg.Error == null)
                {

                    FetchData();
                    ClearControl();
                    objAnimation.Invoke(RotationType.Backward);
                    Indicatior.Close();


                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("", "Clinic Master Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }
                SetCommandButtonState("Modify");


            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("View");
            ClearControl();

            FillData(((clsUnitMasterVO)grdMaster.SelectedItem).UnitID);

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + ((clsUnitMasterVO)grdMaster.SelectedItem).Description;
        }

        private void FillData(long iUnitID)
        {
            clsGetUnitDetailsByIDBizActionVO BizAction = new clsGetUnitDetailsByIDBizActionVO();
            BizAction.Details = new clsUnitMasterVO();

            BizAction.UnitID = iUnitID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Error == null && ea.Result != null)
                    {

                        if (grdMaster.SelectedItem != null)
                            objAnimation.Invoke(RotationType.Forward);

                        clsGetUnitDetailsByIDBizActionVO UnitVO = new clsGetUnitDetailsByIDBizActionVO();
                        UnitVO = (clsGetUnitDetailsByIDBizActionVO)ea.Result;
                        this.DataContext = null;
                        this.DataContext = UnitVO.Details;
                        txtDescription.Text = UnitVO.Details.Description;
                        txtPharmacyLicenseNo.Text = UnitVO.Details.PharmacyLicenseNo;

                        //((clsUnitMasterVO)this.DataContext).UnitCode = UnitVO.Details.UnitCode;
                        //((clsUnitMasterVO)this.DataContext).Name = UnitVO.Details.Name;
                        //((clsUnitMasterVO)this.DataContext).Description = UnitVO.Details.Description;
                        //((clsUnitMasterVO)this.DataContext).AddressLine1 = UnitVO.Details.AddressLine1;
                        //((clsUnitMasterVO)this.DataContext).AddressLine2 = UnitVO.Details.AddressLine2;
                        //((clsUnitMasterVO)this.DataContext).AddressLine3 = UnitVO.Details.AddressLine3;
                        //((clsUnitMasterVO)this.DataContext).Country = UnitVO.Details.Country;
                        //((clsUnitMasterVO)this.DataContext).State = UnitVO.Details.State;
                        //((clsUnitMasterVO)this.DataContext).District = UnitVO.Details.District;
                        //((clsUnitMasterVO)this.DataContext).Taluka = UnitVO.Details.Taluka;
                        //((clsUnitMasterVO)this.DataContext).Area = UnitVO.Details.Area;
                        //((clsUnitMasterVO)this.DataContext).Pincode = UnitVO.Details.Pincode;
                        //((clsUnitMasterVO)this.DataContext).Status = UnitVO.Details.Status;
                        //((clsUnitMasterVO)this.DataContext).Email = UnitVO.Details.Email;
                        //((clsUnitMasterVO)this.DataContext).FaxNo = UnitVO.Details.FaxNo;


                        //added by akshays 
                        txtCountry.SelectedValue = Convert.ToInt64(UnitVO.Details.Countryid);
                        txtCity.SelectedValue = Convert.ToInt64(UnitVO.Details.Cityid);
                        txtState.SelectedValue = Convert.ToInt64(UnitVO.Details.Stateid);
                        txtArea.SelectedValue = Convert.ToInt64(UnitVO.Details.Areaid);
                        //closed by akshays

                        //added by neena
                        if (UnitVO.Details.ContactNo1 != null)
                            txtContactNo1.Text = UnitVO.Details.ContactNo1.ToString();                       
                            txtMobCountryCode.Text = UnitVO.Details.MobileCountryCode.ToString();
                        if (UnitVO.Details.MobileNO != null)
                            txtMobileNo.Text = UnitVO.Details.MobileNO.ToString();
                        //

                        txtResiSTD.Text = txtResiCountryCode.Text = txtContactNo.Text = "";

                        txtResiSTD.WaterMark = "";
                        txtResiSTD.Text = UnitVO.Details.ResiSTDCode.ToString();

                        txtResiCountryCode.WaterMark = "";
                        txtResiCountryCode.Text = UnitVO.Details.ResiNoCountryCode.ToString();

                        txtContactNo.WaterMark = "";
                        txtContactNo.Text = UnitVO.Details.ContactNo;
                        //((clsUnitMasterVO)this.DataContext).City = UnitVO.Details.City;
                        if (UnitVO.Details.ClusterID != null)
                            cmbCluster.SelectedValue = UnitVO.Details.ClusterID;

                        if (UnitVO.Details.DepartmentDetails != null)
                        {
                            List<clsDepartmentDetailsVO> lstDepartment = (List<clsDepartmentDetailsVO>)dgDepartmentList.ItemsSource;
                            foreach (var item1 in UnitVO.Details.DepartmentDetails)
                            {
                                foreach (var item in lstDepartment)
                                {
                                    if (item.DepartmentID == item1.DepartmentID)
                                    {
                                        item.IsDefault = item1.IsDefault;
                                        item.Status = item1.Status;
                                    }
                                }
                            }
                            dgDepartmentList.ItemsSource = null;
                            dgDepartmentList.ItemsSource = lstDepartment;
                        }
                    }
                };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        #region Validation

        string textBefore = "";
        int selectionStart = 0;
        int selectionLength = 0;
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void txtCode_LostFocus(object sender, RoutedEventArgs e)
        {
            txtCode.Text = txtCode.Text.ToTitleCase();
        }

        private void txtDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            //txtDescription.Text = txtDescription.Text.ToTitleCase();
            txtDescription.Text = txtDescription.Text;
        }

        private void WaterMarkTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((WaterMarkTextbox)sender).Text;
            selectionStart = ((WaterMarkTextbox)sender).SelectionStart;
            selectionLength = ((WaterMarkTextbox)sender).SelectionLength;
        }

        private void WaterMarkTextbox_OnTextChanged(object sender, RoutedEventArgs e)
        {
            if (IsPageLoded)
            {
                if (string.IsNullOrEmpty(((WaterMarkTextbox)sender).Text))
                {

                }
                else if (!string.IsNullOrEmpty(((WaterMarkTextbox)sender).Text))
                {


                    if (!((WaterMarkTextbox)sender).Text.IsNumberValid())
                    {
                        ((WaterMarkTextbox)sender).Text = textBefore;
                        ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                        ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                    else if (((WaterMarkTextbox)sender).Text.Length > 10)
                    {
                        ((WaterMarkTextbox)sender).Text = textBefore;
                        ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                        ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                }
            }

        }

        private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text.Length > 0)
            {
                if (txtEmail.Text.IsEmailValid())
                    txtEmail.ClearValidationError();
                else
                {
                    txtEmail.SetValidation("Please enter valid Email");
                    txtEmail.RaiseValidationError();
                }
            }
        }

        private void txtCountry_LostFocus(object sender, RoutedEventArgs e)
        {

            // FillStateList(txtCountry.Text);

        }

        private void txtState_LostFocus(object sender, RoutedEventArgs e)
        {


            FillCityList(txtState.Text);
            GetDistrictList(txtState.Text);
            GetTalukaList(txtState.Text);

        }

        private void txtDistrict_LostFocus(object sender, RoutedEventArgs e)
        {
            //txtDistrict.Text = txtDistrict.Text.ToTitleCase();
        }

        private void txtTaluka_LostFocus(object sender, RoutedEventArgs e)
        {
            // txtTaluka.Text = txtTaluka.Text.ToTitleCase();
        }

        private void txtAutocompleteNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((AutoCompleteBox)sender).Text;
        }

        private void txtAutocompleteNumber_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((AutoCompleteBox)sender).Text))
            {
                if (!((AutoCompleteBox)sender).Text.IsNumberValid())
                {
                    ((AutoCompleteBox)sender).Text = textBefore;
                    //((TextBox)sender).SelectionStart = selectionStart;
                    //((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        private void txtArea_LostFocus(object sender, RoutedEventArgs e)
        {
            txtArea.Text = txtArea.Text.ToTitleCase();
            GetPinCodeList(txtArea.Text);

        }

        private void txtCity_LostFocus(object sender, RoutedEventArgs e)
        {
            txtCity.Text = txtCity.Text.ToTitleCase();
            GetAreaList(txtCity.Text);
        }

        private void chkDepartment_Click(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                if (dgDepartmentList.SelectedItem != null)
                {
                    if (((clsDepartmentDetailsVO)dgDepartmentList.SelectedItem).IsActive == false)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Department is not available in clinic", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        //((clsDepartmentDetailsVO)dgDepartmentList.SelectedItem).Status = false;
                        DepartmentItemSource = (List<clsDepartmentDetailsVO>)dgDepartmentList.ItemsSource;

                        DepartmentItemSource[dgDepartmentList.SelectedIndex].Status = false;

                        dgDepartmentList.ItemsSource = null;
                        dgDepartmentList.ItemsSource = DepartmentItemSource;

                    }
                }

            }

        }

        private void txtFaxNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsItNumber())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void TextName_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (string.IsNullOrEmpty(((TextBox)sender).Text))
            //{
            //}
            //else if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            //{
            //    if (!((TextBox)sender).Text.IsItCharacterWithHyphan())
            //    {
            //        ((TextBox)sender).Text = textBefore;
            //        ((TextBox)sender).SelectionStart = selectionStart;
            //        ((TextBox)sender).SelectionLength = selectionLength;
            //        textBefore = "";
            //        selectionStart = 0;
            //        selectionLength = 0;
            //    }
            //}

        }

        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
        {
            //textBefore = ((TextBox)sender).Text;
            //selectionStart = ((TextBox)sender).SelectionStart;
            //selectionLength = ((TextBox)sender).SelectionLength;
        }

        private bool CheckValidation()
        {
            bool result = true;

            if (txtDescription.Text == "")
            {
                txtDescription.SetValidation("Please Enter Description");
                txtDescription.RaiseValidationError();
                txtDescription.Focus();

                result = false;
            }
            else
                txtDescription.ClearValidationError();

            if (txtCode.Text == "")
            {
                txtCode.SetValidation("Please Enter Code");
                txtCode.RaiseValidationError();
                txtCode.Focus();

                result = false;
            }
            else
                txtCode.ClearValidationError();


            if (txtArea.Text == "")
            {
                txtArea.SetValidation("Please Enter Area");
                txtArea.RaiseValidationError();
                txtArea.Focus();

                result = false;
            }
            else
                txtArea.ClearValidationError();

            if (txtPinCode.Text == "")
            {
                txtPinCode.SetValidation("Please Enter Pincode");
                txtPinCode.RaiseValidationError();
                txtPinCode.Focus();

                result = false;
            }
            else
                txtPinCode.ClearValidationError();

            return result;
        }

        private bool chkDepartment()
        {
            clsDepartmentDetailsVO dept = ((List<clsDepartmentDetailsVO>)dgDepartmentList.ItemsSource).FirstOrDefault(p => p.Status == true);

            if (dept != null)
            {
                return true;
            }
            else
            {
                string msgTitle = "Palash";
                string msgText = "Please Select Department";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgW.Show();

                return false;
            }
        }

        #endregion

        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    //cmdNew.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Save":
                    // cmdNew.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Modify":
                    // cmdNew.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    //cmdNew.IsEnabled = false;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;

                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Reset All Control

        private void ClearControl()
        {
            txtCode.Text = string.Empty;
            txtDescription.Text = "";
            txtResiSTD.Text = string.Empty;
            txtResiCountryCode.Text = string.Empty;
            txtFaxNo.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtContactNo.Text = string.Empty;
            txtAddress1.Text = string.Empty;
            txtAddress2.Text = string.Empty;
            txtAddress3.Text = string.Empty;
            txtCountry.Text = string.Empty;
            txtState.Text = string.Empty;
            //txtDistrict.Text = string.Empty;
            // txtTaluka.Text = string.Empty;
            txtArea.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtPinCode.Text = string.Empty;
            txtPharmacyLicenseNo.Text = string.Empty;
            txtClinicRegNo.Text = string.Empty;
            txtTINNo.Text = string.Empty;
            txtGSTNNo.Text = string.Empty;
            txtShopNo.Text = string.Empty;
            txtTradeNo.Text = string.Empty;

            this.DataContext = new clsUnitMasterVO();

            if ((List<clsDepartmentDetailsVO>)dgDepartmentList.ItemsSource != null)
            {
                List<clsDepartmentDetailsVO> lList = (List<clsDepartmentDetailsVO>)dgDepartmentList.ItemsSource;
                foreach (var item in lList)
                {
                    item.Status = false;
                    item.IsDefault = false;
                }
                dgDepartmentList.ItemsSource = null;
                dgDepartmentList.ItemsSource = lList;
            }
        }

        #endregion

        private void txtPharmacyLicenseNo_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtClinicRegNo_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtShopNo_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtTradeNo_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void cmdSave_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void txtCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtCountry.SelectedItem != null && txtCountry.SelectedValue != null)
                if (((MasterListItem)txtCountry.SelectedItem).ID > 0)
                {
                    ((clsUnitMasterVO)this.DataContext).Countryid = ((MasterListItem)txtCountry.SelectedItem).ID;
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    txtState.ItemsSource = objList;
                    txtState.SelectedItem = objM;
                    txtCity.ItemsSource = objList;
                    txtCity.SelectedItem = objM;
                    txtArea.ItemsSource = objList;
                    txtArea.SelectedItem = objM;
                    FillState(((MasterListItem)txtCountry.SelectedItem).ID);
                }
        }

        private void txtState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtState.SelectedItem != null && txtState.SelectedValue != null)
                if (((MasterListItem)txtState.SelectedItem).ID > 0)
                {
                    ((clsUnitMasterVO)this.DataContext).Stateid = ((MasterListItem)txtState.SelectedItem).ID;
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    txtCity.ItemsSource = objList;
                    txtCity.SelectedItem = objM;
                    txtArea.ItemsSource = objList;
                    txtArea.SelectedItem = objM;
                    FillCity(((MasterListItem)txtState.SelectedItem).ID);
                }
        }

        private void txtCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtState.SelectedItem != null && txtState.SelectedValue != null && (MasterListItem)txtCity.SelectedItem != null)
                if (((MasterListItem)txtState.SelectedItem).ID > 0)
                {
                    if (((MasterListItem)txtCity.SelectedItem).ID > 0)
                        ((clsUnitMasterVO)this.DataContext).Cityid = ((MasterListItem)txtCity.SelectedItem).ID;

                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    txtArea.ItemsSource = null;
                    txtArea.SelectedItem = objM;
                    FillRegion(((MasterListItem)txtCity.SelectedItem).ID);
                }
        }

        private void txtArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)txtCity.SelectedItem != null)
            {
                if (((MasterListItem)txtArea.SelectedItem).ID > 0 && (MasterListItem)txtArea.SelectedItem != null)
                    ((clsUnitMasterVO)this.DataContext).Areaid = ((MasterListItem)txtArea.SelectedItem).ID;
            }
        }

        public void FillState(long CountryID)
        {
            clsGetStateDetailsByCountyIDBizActionVO BizAction = new clsGetStateDetailsByCountyIDBizActionVO();
            BizAction.CountryId = CountryID;
            BizAction.ListStateDetails = new List<clsStateVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails != null)
                    {
                        if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails.Count > 0)
                        {
                            foreach (clsStateVO item in ((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails)
                            {
                                MasterListItem obj = new MasterListItem();
                                obj.ID = item.Id;
                                obj.Description = item.Description;
                                objList.Add(obj);
                            }
                        }
                    }

                    txtState.ItemsSource = null;
                    txtState.ItemsSource = objList.DeepCopy();

                    //txtSpouseState.ItemsSource = null;
                    //txtSpouseState.ItemsSource = objList.DeepCopy();

                    if (this.DataContext != null)
                    {
                        txtState.SelectedValue = ((clsUnitMasterVO)this.DataContext).Stateid;
                        //txtSpouseState.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.StateID;
                    }
                    else
                    {
                        txtState.SelectedItem = objM;
                        // txtSpouseState.SelectedItem = objM;
                    }

                }


            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        public void FillCity(long StateID)
        {
            clsGetCityDetailsByStateIDBizActionVO BizAction = new clsGetCityDetailsByStateIDBizActionVO();
            BizAction.StateId = StateID;
            BizAction.ListCityDetails = new List<clsCityVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.ListCityDetails = ((clsGetCityDetailsByStateIDBizActionVO)args.Result).ListCityDetails;
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    if (BizAction.ListCityDetails != null)
                    {
                        if (BizAction.ListCityDetails.Count > 0)
                        {
                            foreach (clsCityVO item in BizAction.ListCityDetails)
                            {
                                MasterListItem obj = new MasterListItem();
                                obj.ID = item.Id;
                                obj.Description = item.Description;
                                objList.Add(obj);
                            }
                        }
                    }
                    txtCity.ItemsSource = null;
                    txtCity.ItemsSource = objList.DeepCopy();

                    // txtSpouseCity.ItemsSource = null;
                    // txtSpouseCity.ItemsSource = objList.DeepCopy();

                    if (this.DataContext != null)
                    {
                        txtCity.SelectedValue = ((clsUnitMasterVO)this.DataContext).Cityid;
                        //  txtSpouseCity.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.CityID;
                    }
                    else
                    {
                        txtCity.SelectedItem = objM;
                        // txtSpouseCity.SelectedItem = objM;
                    }

                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        public void FillRegion(long CityID)
        {
            clsGetRegionDetailsByCityIDBizActionVO BizAction = new clsGetRegionDetailsByCityIDBizActionVO();
            BizAction.CityId = CityID;
            BizAction.ListRegionDetails = new List<clsRegionVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.ListRegionDetails = ((clsGetRegionDetailsByCityIDBizActionVO)args.Result).ListRegionDetails;
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    if (BizAction.ListRegionDetails != null)
                    {
                        if (BizAction.ListRegionDetails.Count > 0)
                        {
                            foreach (clsRegionVO item in BizAction.ListRegionDetails)
                            {
                                MasterListItem obj = new MasterListItem();
                                obj.ID = item.Id;
                                obj.Description = item.Description;
                                objList.Add(obj);
                            }
                        }
                    }
                    txtArea.ItemsSource = null;
                    txtArea.ItemsSource = objList.DeepCopy();

                    // txtSpouseArea.ItemsSource = null;
                    // txtSpouseArea.ItemsSource = objList.DeepCopy();

                    if (this.DataContext != null)
                    {
                        txtArea.SelectedValue = ((clsUnitMasterVO)this.DataContext).Areaid;
                        //   txtSpouseArea.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.RegionID;
                    }
                    else
                    {
                        txtArea.SelectedItem = objM;
                        //  txtSpouseArea.SelectedItem = objM;
                    }
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        public void FillCountry()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_CountryMaster;
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
                    txtCountry.ItemsSource = null;
                    txtCountry.ItemsSource = objList.DeepCopy();
                    txtCountry.SelectedItem = objList[0];

                    //  txtSpouseCountry.ItemsSource = null;
                    //txtSpouseCountry.ItemsSource = objList.DeepCopy();
                }
                if (this.DataContext != null)
                {
                    txtCountry.SelectedValue = ((clsUnitMasterVO)this.DataContext).Countryid;
                    // txtSpouseCountry.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.CountryID;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
    }
}
