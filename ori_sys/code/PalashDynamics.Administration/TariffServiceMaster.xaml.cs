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
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using System.Windows.Browser;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Data;
using System.Reflection;
using PalashDynamics.Collections;
namespace PalashDynamics.Administration
{
    public partial class TariffServiceMaster : UserControl
    {
        WaitIndicator waitIndicator = new WaitIndicator();
        private SwivelAnimation objAnimation;
        PagedCollectionView GroupByTariff = null;

        //added by akshays

        #region Paging

        public PagedSortableCollectionView<clsServiceMasterVO> DataList { get; private set; }

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

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {

            FillTariffServiceListGrid(true);

        }

        #endregion

        //closed by akshays


        
        public TariffServiceMaster()
        {
            InitializeComponent();
            //added by akshays 
            DataList = new PagedSortableCollectionView<clsServiceMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            //closed by akshays
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillTariffs();
            FillSpecialization();
            
            FillCodeType();
            FillTariffServiceListGrid(true);
            
        }
        private void FillCodeType()
        {
            try
            {


                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_CodeType;
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

                        cboCodeType.ItemsSource = null;
                        cboCodeType.ItemsSource = objList;
                        cboCodeType.SelectedValue = objList[0].ID;

                    }

                    if (this.DataContext != null)
                    {
                        cboCodeType.SelectedValue = ((clsServiceMasterVO)this.DataContext).CodeType;
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
        private void FillTariffs()
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

                        List<MasterListItem> objList = new List<MasterListItem>();


                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                        cboTariffs.ItemsSource = null;
                        cboTariffs.ItemsSource = objList;
                        cboTariffs.SelectedValue = objList[0].ID;

                    }

                    if (this.DataContext != null)
                    {
                        cboTariffs.SelectedValue = ((clsServiceMasterVO)this.DataContext).CodeType;
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

        private void FillSpecialization()
        {
            try
            {


                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_Specialization;
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


                        cboSpecialization.ItemsSource = null;
                        cboSpecialization.ItemsSource = objList;
                        cboSpecialization.SelectedValue = objList[0].ID;
                        cboSpecialization1.ItemsSource = null;
                        cboSpecialization1.ItemsSource = objList;
                        cboSpecialization1.SelectedValue = objList[0].ID;
                    }

                    if (this.DataContext != null)
                    {
                        cboSpecialization.SelectedValue = ((clsServiceMasterVO)this.DataContext).Specialization;
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

        private void cboSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboSpecialization.SelectedItem != null)
            {
                FillSubSpecialization(((MasterListItem)cboSpecialization.SelectedItem).ID.ToString());
            }
        }

        private void FillSubSpecialization(string fkSpecializationID)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_SubSpecialization;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            if (fkSpecializationID != null)
            {

                BizAction.Parent = new KeyValue { Key = fkSpecializationID, Value = "fkSpecializationID" };
            }
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
                    cboSubSpecialization.ItemsSource = null;
                    cboSubSpecialization.ItemsSource = objList;
                    cboSubSpecialization.SelectedValue = objList[0].ID;
                }

                //if (this.DataContext != null)
                //{
                //    cboSubSpecialization.SelectedValue = ((clsServiceMasterVO)this.DataContext).SubSpecialization;
                //   // cboSubSpecialization1.SelectedValue = ((clsServiceMasterVO)this.DataContext).SubSpecialization;

                //}

            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillSubSpecializationForBackPanel(string fkSpecializationID)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_SubSpecialization;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            if (fkSpecializationID != null)
            {

                BizAction.Parent = new KeyValue { Key = fkSpecializationID, Value = "fkSpecializationID" };
            }
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
                    
                    cboSubSpecialization1.ItemsSource = null;
                    cboSubSpecialization1.ItemsSource = objList;
                    cboSubSpecialization1.SelectedValue = objList[0].ID;

                }

                if (this.DataContext != null)
                {
                   // cboSubSpecialization.SelectedValue = ((clsServiceMasterVO)this.DataContext).SubSpecialization;
                    cboSubSpecialization1.SelectedValue = ((clsServiceMasterVO)this.DataContext).SubSpecialization;

                }

            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {

            FillTariffServiceListGrid(false);
          
        }
        WaitIndicator wait = new WaitIndicator();

        private void FillTariffServiceListGrid(bool isLoad)
        {
            wait.Show();
            try
            {
                clsGetTariffServiceMasterListBizActionVO objBizActionvo = new clsGetTariffServiceMasterListBizActionVO();
                // objBizActionvo.GetAllTariffServices = true;
                //if (isLoad == true)
                //{
                //    objBizActionvo.Specialization = 0;
                //    objBizActionvo.SubSpecialization = 0;
                //    objBizActionvo.TariffID = 0;
                //    objBizActionvo.ServiceName = null;
                //}
                //else
                //{
                if (((MasterListItem)cboSpecialization.SelectedItem) !=null)
                    objBizActionvo.Specialization = ((MasterListItem)cboSpecialization.SelectedItem).ID;
                if (((MasterListItem)cboSubSpecialization.SelectedItem) != null)
                    objBizActionvo.SubSpecialization = ((MasterListItem)cboSubSpecialization.SelectedItem).ID;
                if (((MasterListItem)cboTariffs.SelectedItem) != null)
                    objBizActionvo.TariffID = ((MasterListItem)cboTariffs.SelectedItem).ID;
                    objBizActionvo.ServiceName = txtName.Text;

                //}
                objBizActionvo.IsPagingEnabled = true;
                objBizActionvo.StartIndex = DataList.PageIndex * DataList.PageSize;
                objBizActionvo.MaximumRows = DataList.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    //if (args.Error == null && args.Result != null)
                    //{
                    //    clsGetTariffServiceMasterListBizActionVO result = args.Result as clsGetTariffServiceMasterListBizActionVO;
                    //    DataList.TotalItemCount = result.TotalRows;
                    //   // grdServices.ItemsSource = null;
                    //   // grdServices.ItemsSource = ((clsGetTariffServiceMasterListBizActionVO)args.Result).ServiceList;
                    //   //added by akshays
                    //    if (result.ServiceList != null)
                    //    {
                    //        DataList.Clear();

                    //        foreach (var item in result.ServiceList)
                    //        {
                    //            DataList.Add(item);
                    //        }

                    //    }
                    //    grdServices.ItemsSource = null;
                    //    grdServices.ItemsSource = DataList;
                    //    dgDataPager.Source = DataList;
                    //    //closed by akshays


                    //    //PagedCollectionView collection = new PagedCollectionView(((clsGetTariffServiceMasterListBizActionVO)args.Result).ServiceList);
                    //    //collection.GroupDescriptions.Add(new PropertyGroupDescription("Tariff Name"));
                    //    //grdServices.ItemsSource = collection;
                    //    //grdServices.SelectedIndex = -1;
                    //}

                    ////if (this.DataContext != null)
                    ////{
                    ////    // cboSubSpecialization.SelectedValue = ((clsServiceMasterVO)this.DataContext).SubSpecialization;
                    ////}
                    ////dgDataPager.Source = DataList;
                    //EmptyUI();
                    //wait.Close();
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetTariffServiceMasterListBizActionVO result = args.Result as clsGetTariffServiceMasterListBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        DataList.Clear();
                        if (result.ServiceList != null)
                        {
                            foreach (var item in result.ServiceList)
                            {
                                DataList.Add(item);
                            }

                        }
                        
                      
                    }
                    grdServices.ItemsSource = null;
                    grdServices.ItemsSource = DataList;
                    dgDataPager.Source = null;
                    dgDataPager.PageSize = objBizActionvo.MaximumRows;
                    dgDataPager.Source = DataList;


                   // EmptyUI();
                    wait.Close();

                };

                client.ProcessAsync(objBizActionvo, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                wait.Close();
            }
        }
        
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            wait.Show();
            try
            {
                clsServiceMasterVO objServiceVO = new clsServiceMasterVO();
                objServiceVO = (clsServiceMasterVO)grdServices.SelectedItem;


                // EditMode = true;
                this.DataContext = objServiceVO;

                if (objServiceVO != null)
                {
                    PrevServiceRate = objServiceVO.Rate;
                    objServiceVO.EditMode = true;
                    cboCodeType.SelectedValue = objServiceVO.CodeType;
                    cboSpecialization1.SelectedValue = objServiceVO.Specialization;
                    cboSubSpecialization1.SelectedValue = objServiceVO.SubSpecialization;
                    cboCodeType.SelectedValue = objServiceVO.CodeType;
                    

                    if (objServiceVO.Concession == true)
                    {
                        txtConcessionPercentage.IsEnabled = true;
                        txtConcessionAmount.IsEnabled = true;
                    }
                    else
                    {
                        txtConcessionPercentage.IsEnabled = false;
                        txtConcessionAmount.IsEnabled = false;
                    }
                    if (objServiceVO.DoctorShare == true)
                    {
                        txtDoctorApplicableAmount.IsEnabled = true;
                        txtDoctorApplicablePercent.IsEnabled = true;

                    }
                    else
                    {
                        txtDoctorApplicableAmount.IsEnabled = false;
                        txtDoctorApplicablePercent.IsEnabled = false;
                    }
                    if (objServiceVO.RateEditable == true)
                    {
                        txtMinRate.IsEnabled = true;
                        txtMaxRate.IsEnabled = true;

                    }
                    else
                    {
                        txtMinRate.IsEnabled = false;
                        txtMaxRate.IsEnabled = false;
                    }

                    if (objServiceVO.ServiceTax == true)
                    {
                        txtServiceTaxAmount.IsEnabled = true;
                        txtServiceTaxPercentage.IsEnabled = true;
                    }
                    else
                    {
                        txtServiceTaxAmount.IsEnabled = false;
                        txtServiceTaxPercentage.IsEnabled = false;
                    }

                    if (objServiceVO.StaffDependantDiscount == true)
                    {
                        txtStaffParentAmount.IsEnabled = true;
                        txtStaffParentPercentage.IsEnabled = true;
                    }
                    else
                    {
                        txtStaffParentAmount.IsEnabled = false;
                        txtStaffParentPercentage.IsEnabled = false;
                    }
                    if (objServiceVO.StaffDiscount == true)
                    {
                        txtStaffDiscountAmount.IsEnabled = true;
                        txtStaffDiscountPercentage.IsEnabled = true;
                    }
                    else
                    {
                        txtStaffDiscountAmount.IsEnabled = false;
                        txtStaffDiscountPercentage.IsEnabled = false;
                    }


                    pkTariffServiceMasterID = objServiceVO.TariffServiceMasterID;
                    pkServiceID = objServiceVO.ID;

                    //FillTariffList();
                    //BindTariffApplicable(pkServiceID);
                    //  GetTariffServiceClassRateDetail(objServiceVO.TariffServiceMasterID);

                    GetTariffServiceClassListRateDetail(objServiceVO.ID, objServiceVO.TariffID);
                    // FillSubSpecialization(objServiceVO.Specialization.ToString());
                    txtServiceRate.Text = Convert.ToString(((clsServiceMasterVO)grdServices.SelectedItem).BaseServiceRate);
                    objAnimation.Invoke(RotationType.Forward);
                    //EditMode = false;


                }
                wait.Close();
            }
            catch (Exception)
            {
                wait.Close();
            }
        }

        private void GetTariffServiceClassListRateDetail(long serviceID, long TariffID)
        {
            try
            {
                clsGetTariffServiceClassBizActionVO objSeviceMaster = new clsGetTariffServiceClassBizActionVO();
                objSeviceMaster.ClassList = new  List<clsServiceTarrifClassRateDetailsVO>();
                objSeviceMaster.ServiceID = serviceID;
                objSeviceMaster.TariffID = TariffID;
                //objSeviceMaster.ServiceMaster.t = pkServiceID;
                //objSeviceMaster.GetAllServiceClassRateDetails = true;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {

                        clsGetTariffServiceClassBizActionVO obj = args.Result as clsGetTariffServiceClassBizActionVO;
                        if (obj != null )
                        {
                            grdClass.ItemsSource = null;
                            grdClass.ItemsSource = obj.ClassList;
                        }
                        if (this.DataContext != null)
                        {
                            txtServiceRate.Text = ((clsServiceMasterVO)DataContext).BaseServiceRate.ToString();
                        }

                    }

                };

                client.ProcessAsync(objSeviceMaster, new clsUserVO());
                client.CloseAsync();

            }
            catch (Exception)
            {


            }
        }
        
        private void GetTariffServiceClassRateDetail(long tariffserviceid)
        {
            try
            {
                clsGetTariffServiceClassRateBizActionVO objSeviceMaster = new clsGetTariffServiceClassRateBizActionVO();
                objSeviceMaster.ServiceMaster = new clsServiceMasterVO();
                objSeviceMaster.ServiceMaster.TariffServiceMasterID = tariffserviceid;
               //objSeviceMaster.ServiceMaster.t = pkServiceID;
                //objSeviceMaster.GetAllServiceClassRateDetails = true;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {

                        clsGetTariffServiceClassRateBizActionVO obj = args.Result as clsGetTariffServiceClassRateBizActionVO;
                        if (obj != null)
                        {
                            txtServiceRate.Text = obj.ServiceMaster.Rate.ToString();
                        }

                    }

                };

                client.ProcessAsync(objSeviceMaster, new clsUserVO());
                client.CloseAsync();

            }
            catch (Exception)
            {

                
            }
        }
      
        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            bool ValidationStatus = ValidateUI();
            try
            {
                clsAddTariffServiceBizActionVO objBizActionVO;

                if (ValidationStatus == true)
                {
                    objBizActionVO = new clsAddTariffServiceBizActionVO();
                    objBizActionVO.UpdateTariffServiceMaster = true;
                    objBizActionVO.ServiceMasterDetails = new clsServiceMasterVO();
                    objBizActionVO.ServiceMasterDetails.EditMode = ((clsServiceMasterVO)this.DataContext).EditMode;
                    objBizActionVO.ServiceMasterDetails.ServiceCode = ((clsServiceMasterVO)this.DataContext).ServiceCode;
                    objBizActionVO.ServiceMasterDetails.ServiceID = pkServiceID == null ? 0 : pkServiceID;
                    //objBizActionVO.ServiceMasterDetails.ServiceID = ((clsServiceMasterVO)this.DataContext).ServiceID;
                    objBizActionVO.ServiceMasterDetails.TariffServiceMasterID = pkTariffServiceMasterID == null ? 0 : pkTariffServiceMasterID;
                    objBizActionVO.ServiceMasterDetails.CodeType = ((MasterListItem)cboCodeType.SelectedItem).ID;
                    objBizActionVO.ServiceMasterDetails.Code = txtCode.Text == "" ? "" : txtCode.Text;
                    objBizActionVO.ServiceMasterDetails.Specialization = ((MasterListItem)cboSpecialization1.SelectedItem).ID;
                    objBizActionVO.ServiceMasterDetails.SubSpecialization = ((MasterListItem)cboSubSpecialization1.SelectedItem).ID;
                    objBizActionVO.ServiceMasterDetails.ServiceName = txtServiceName.Text == "" ? "" : txtServiceName.Text;
                    objBizActionVO.ServiceMasterDetails.ShortDescription = txtServiceShortDescription.Text == "" ? "" : txtServiceShortDescription.Text;
                    objBizActionVO.ServiceMasterDetails.LongDescription = txtServiceLongDescription.Text == "" ? "" : txtServiceLongDescription.Text;

                    objBizActionVO.ServiceMasterDetails.StaffDiscount = ((clsServiceMasterVO)this.DataContext).StaffDiscount;
                    objBizActionVO.ServiceMasterDetails.StaffDiscountAmount = txtStaffDiscountAmount.Text == "" ? 0 : decimal.Parse(txtStaffDiscountAmount.Text);
                    objBizActionVO.ServiceMasterDetails.StaffDiscountPercent = txtStaffDiscountPercentage.Text == "" ? 0 : decimal.Parse(txtStaffDiscountPercentage.Text);

                    objBizActionVO.ServiceMasterDetails.StaffDependantDiscount = ((clsServiceMasterVO)this.DataContext).StaffDependantDiscount;
                    objBizActionVO.ServiceMasterDetails.StaffDependantDiscountAmount = txtStaffParentAmount.Text == "" ? 0 : decimal.Parse(txtStaffParentAmount.Text);
                    objBizActionVO.ServiceMasterDetails.StaffDependantDiscountPercent = txtStaffParentPercentage.Text == "" ? 0 : decimal.Parse(txtStaffParentPercentage.Text);

                    // objBizActionVO.ServiceMasterDetails.GeneralDiscount = ((clsServiceMasterVO)this.DataContext).GeneralDiscount;

                    objBizActionVO.ServiceMasterDetails.Concession = ((clsServiceMasterVO)this.DataContext).Concession;
                    objBizActionVO.ServiceMasterDetails.ConcessionAmount = txtConcessionAmount.Text == "" ? 0 : decimal.Parse(txtConcessionAmount.Text);
                    objBizActionVO.ServiceMasterDetails.ConcessionPercent = txtConcessionPercentage.Text == "" ? 0 : decimal.Parse(txtConcessionPercentage.Text);

                    objBizActionVO.ServiceMasterDetails.ServiceTax = ((clsServiceMasterVO)this.DataContext).ServiceTax;
                    objBizActionVO.ServiceMasterDetails.ServiceTaxAmount = txtServiceTaxAmount.Text == "" ? 0 : decimal.Parse(txtServiceTaxAmount.Text);
                    objBizActionVO.ServiceMasterDetails.ServiceTaxPercent = txtServiceTaxPercentage.Text == "" ? 0 : decimal.Parse(txtServiceTaxPercentage.Text);

                    objBizActionVO.ServiceMasterDetails.TarrifCode = ((clsServiceMasterVO)this.DataContext).TarrifCode;
                    objBizActionVO.ServiceMasterDetails.TarrifName = txtTariffServiceName.Text;
                    objBizActionVO.ServiceMasterDetails.TariffID = ((clsServiceMasterVO)this.DataContext).TariffID;
                    //objBizActionVO.ServiceMasterDetails.ID = ((clsServiceMasterVO)this.DataContext).ID;
                    objBizActionVO.ServiceMasterDetails.ID = objBizActionVO.ServiceMasterDetails.ServiceID;
                    List<long> lstTariffId = new List<long>();
                    lstTariffId.Add(objBizActionVO.ServiceMasterDetails.TariffID);
                    objBizActionVO.TariffList = lstTariffId;

                    //===========================================
                    objBizActionVO.ClassList = (List<clsServiceTarrifClassRateDetailsVO>)grdClass.ItemsSource;
                    //===========================================

                    //objBizActionVO.ServiceMasterDetails.ID = objBizActionVO.ServiceMasterDetails.TariffServiceMasterID;
                    objBizActionVO.ServiceMasterDetails.OutSource = ((clsServiceMasterVO)this.DataContext).OutSource;
                    objBizActionVO.ServiceMasterDetails.InHouse = ((clsServiceMasterVO)this.DataContext).InHouse;
                    objBizActionVO.ServiceMasterDetails.DoctorShare = ((clsServiceMasterVO)this.DataContext).DoctorShare;
                    objBizActionVO.ServiceMasterDetails.DoctorSharePercentage = ((clsServiceMasterVO)this.DataContext).DoctorSharePercentage;
                    objBizActionVO.ServiceMasterDetails.DoctorShareAmount = txtDoctorApplicableAmount.Text == "" ? 0 : decimal.Parse(txtDoctorApplicableAmount.Text);
                    objBizActionVO.ServiceMasterDetails.DoctorSharePercentage = txtDoctorApplicablePercent.Text == "" ? 0 : decimal.Parse(txtDoctorApplicablePercent.Text);
                    objBizActionVO.ServiceMasterDetails.RateEditable = ((clsServiceMasterVO)this.DataContext).RateEditable;
                    objBizActionVO.ServiceMasterDetails.MaxRate = txtMaxRate.Text == "" ? 0 : decimal.Parse(txtMaxRate.Text);
                    objBizActionVO.ServiceMasterDetails.MinRate = txtMinRate.Text == "" ? 0 : decimal.Parse(txtMinRate.Text);
                    //objBizActionVO.ServiceMasterDetails.Rate = txtServiceRate.Text == "" ? 0 : decimal.Parse(txtServiceRate.Text);
                    objBizActionVO.ServiceMasterDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    objBizActionVO.ServiceMasterDetails.Status = true;
                    //objBizActionVO.ServiceMasterDetails.CheckedAllTariffs = chkAllTariffs.IsChecked == true ? true : false;
                    objBizActionVO.ServiceMasterDetails.CreatedUnitID = 1;
                    objBizActionVO.ServiceMasterDetails.UpdatedUnitID = 1;
                    objBizActionVO.ServiceMasterDetails.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    objBizActionVO.ServiceMasterDetails.UpdatedOn = DateTime.Now.DayOfWeek.ToString();
                    objBizActionVO.ServiceMasterDetails.UpdatedDateTime = DateTime.Now;

                    objBizActionVO.ServiceMasterDetails.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;

                    objBizActionVO.TariffServiceForm = true;
                    //**********************Inserting Service Master***********************************//

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null && ((clsAddTariffServiceBizActionVO)arg.Result).ServiceMasterDetails != null)
                        {

                            string msgTitle = "";
                            string msgText = "Service Updated Successfully";


                            MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);


                            msgW.Show();
                            objAnimation.Invoke(RotationType.Backward);
                            FillTariffServiceListGrid(false);

                        }
                        else
                        {
                            string msgTitle = "";
                            string msgText = "Some error occurred while saving";


                            MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        }

                    };

                    client.ProcessAsync(objBizActionVO, new clsUserVO());
                    client.CloseAsync();
                }
                else
                {

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Required Fields To Save The Record", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                                 new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
               
            }
        }
        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
           // EmptyUI();
        }

        private void EmptyUI()
        {
            cboTariffs.SelectedValue = 0;
            cboSpecialization.SelectedValue = 0;
            cboSubSpecialization.SelectedValue = 0;
            
        }
        private bool ValidateUI()
        {
            bool result = true;
            try
            {
                if (txtServiceName.Text == "")
                {

                    txtServiceName.SetValidation("Service Name is required");
                    txtServiceName.RaiseValidationError();
                    txtServiceName.Focus();
                    result = false;
                }
                else
                {
                    txtServiceName.ClearValidationError();
                }


                if ((MasterListItem)cboSpecialization1.SelectedItem == null)
                {
                    cboSpecialization1.TextBox.SetValidation("Please Select Specialization");
                    cboSpecialization1.TextBox.RaiseValidationError();
                    cboSpecialization1.Focus();
                    result = false;
                }

                else
                {
                    if (cboSpecialization1.SelectedItem != null)
                    {
                        if (((MasterListItem)cboSpecialization1.SelectedItem).ID == 0)
                        {
                            cboSpecialization1.TextBox.SetValidation("Please Select Specialization");
                            cboSpecialization1.TextBox.RaiseValidationError();
                            cboSpecialization1.Focus();
                            result = false;
                        }
                    }
                    else
                        cboSpecialization1.TextBox.ClearValidationError();
                }



                if (txtServiceRate.Text == "")
                {

                    txtServiceRate.SetValidation("Service Rate is required");
                    txtServiceRate.RaiseValidationError();
                    txtServiceRate.Focus();
                    result = false;
                }
                else if (Extensions.IsItDecimal(txtServiceRate.Text) == true)
                {
                    txtServiceRate.SetValidation("Rate should be Numeric");
                    txtServiceRate.RaiseValidationError();
                    txtServiceRate.Focus();
                    result = false;
                }
                else if (Convert.ToDecimal(txtServiceRate.Text) == 0)
                {
                    txtServiceRate.SetValidation("Rate should not be zero");
                    txtServiceRate.RaiseValidationError();
                    txtServiceRate.Focus();
                    result = false;
                }

                else
                {
                    txtServiceRate.ClearValidationError();
                }


                if (txtServiceTaxPercentage.Text != "")
                {
                    if (Extensions.IsItDecimal(txtServiceTaxPercentage.Text) == true)
                    {

                        txtServiceTaxPercentage.SetValidation(" Tax Percentage should be number");
                        txtServiceTaxPercentage.RaiseValidationError();
                        result = false;

                    }
                }

                else
                {
                    txtServiceTaxPercentage.ClearValidationError();
                }


                if (txtServiceTaxAmount.Text != "")
                {
                    if (Extensions.IsItDecimal(txtServiceTaxAmount.Text) == true)
                    {

                        txtServiceTaxAmount.SetValidation(" Tax Amount should be number");
                        txtServiceTaxAmount.RaiseValidationError();
                        result = false;

                    }
                }

                else
                {
                    txtServiceTaxAmount.ClearValidationError();
                }


                if (txtStaffDiscountPercentage.Text != "")
                {
                    if (Extensions.IsItDecimal(txtStaffDiscountPercentage.Text) == true)
                    {

                        txtStaffDiscountPercentage.SetValidation(" Staff Discount Percentage should be number");
                        txtStaffDiscountPercentage.RaiseValidationError();
                        result = false;

                    }
                }

                else
                {
                    txtStaffDiscountPercentage.ClearValidationError();
                }


                if (txtStaffDiscountAmount.Text != "")
                {
                    if (Extensions.IsItDecimal(txtStaffDiscountAmount.Text) == true)
                    {

                        txtStaffDiscountAmount.SetValidation(" Staff Discount Amount should be number");
                        txtStaffDiscountAmount.RaiseValidationError();
                        result = false;

                    }
                }

                else
                {
                    txtStaffDiscountAmount.ClearValidationError();
                }


                if (txtStaffParentPercentage.Text != "")
                {
                    if (Extensions.IsItDecimal(txtStaffParentPercentage.Text) == true)
                    {

                        txtStaffParentPercentage.SetValidation(" Staff Dependant Discount Percentage should be number");
                        txtStaffParentPercentage.RaiseValidationError();
                        result = false;

                    }
                }

                else
                {
                    txtStaffParentPercentage.ClearValidationError();
                }


                if (txtStaffParentAmount.Text != "")
                {
                    if (Extensions.IsItDecimal(txtStaffParentAmount.Text) == true)
                    {

                        txtStaffParentAmount.SetValidation(" Staff Dependant Discount Amount should be number");
                        txtStaffParentAmount.RaiseValidationError();
                        result = false;

                    }
                }

                else
                {
                    txtStaffParentAmount.ClearValidationError();
                }

                if (txtConcessionPercentage.Text != "")
                {
                    if (Extensions.IsItDecimal(txtConcessionPercentage.Text) == true)
                    {

                        txtConcessionPercentage.SetValidation(" Concession Percentage should be number");
                        txtConcessionPercentage.RaiseValidationError();
                        result = false;

                    }
                }

                else
                {
                    txtConcessionPercentage.ClearValidationError();
                }


                if (txtConcessionAmount.Text != "")
                {
                    if (Extensions.IsItDecimal(txtConcessionAmount.Text) == true)
                    {

                        txtConcessionAmount.SetValidation(" Concession Amount should be number");
                        txtConcessionAmount.RaiseValidationError();
                        result = false;

                    }
                }

                else
                {
                    txtConcessionAmount.ClearValidationError();
                }

                if (txtDoctorApplicablePercent.Text != "")
                {
                    if (Extensions.IsItDecimal(txtDoctorApplicablePercent.Text) == true)
                    {

                        txtDoctorApplicablePercent.SetValidation(" Doctor Share Percentage should be number");
                        txtDoctorApplicablePercent.RaiseValidationError();
                        result = false;

                    }
                }

                else
                {
                    txtDoctorApplicablePercent.ClearValidationError();
                }
                if (txtDoctorApplicableAmount.Text != "")
                {
                    if (Extensions.IsItDecimal(txtDoctorApplicableAmount.Text) == true)
                    {

                        txtDoctorApplicableAmount.SetValidation(" Doctor Share Amount should be number");
                        txtDoctorApplicableAmount.RaiseValidationError();
                        result = false;

                    }
                }

                else
                {
                    txtDoctorApplicableAmount.ClearValidationError();
                }

                if (chkRateEditable.IsChecked == true)
                {

                    try
                    {
                        if (txtMinRate.Text != "")
                        {
                            if (Extensions.IsItDecimal(txtMinRate.Text) == true)
                            {
                                txtMinRate.SetValidation(" Min Rate Amount should be number Or Not Negative Number.");
                                txtMinRate.RaiseValidationError();
                                result = false;
                            }
                            else if (txtServiceRate.Text != "")
                            {
                                if (Convert.ToDouble(txtServiceRate.Text) < Convert.ToDouble(txtMinRate.Text))
                                {
                                    txtMinRate.SetValidation(" Min Rate should be less than or equal to Base Rate");
                                    txtMinRate.RaiseValidationError();
                                    result = false;
                                }
                            }
                            else if (txtMaxRate.Text != "")
                            {
                                if (Convert.ToDouble(txtMaxRate.Text) < Convert.ToDouble(txtMinRate.Text))
                                {
                                    txtMinRate.SetValidation(" Min Rate should be less than or equal to Max Rate");
                                    txtMinRate.RaiseValidationError();
                                    result = false;
                                }
                            }
                            else
                                txtMinRate.ClearValidationError();
                        }

                        else
                        {
                            txtMinRate.ClearValidationError();
                        }


                        if (txtMaxRate.Text != "")
                        {
                            if (Extensions.IsItDecimal(txtMaxRate.Text) == true)
                            {
                                txtMaxRate.SetValidation(" Max Rate Amount should be number Or Not Negative Number.");
                                txtMaxRate.RaiseValidationError();
                                result = false;
                            }
                            else if (txtServiceRate.Text != "")
                            {
                                if (Convert.ToDouble(txtServiceRate.Text) < Convert.ToDouble(txtMaxRate.Text))
                                {
                                    txtMaxRate.SetValidation(" Max Rate should be less than or equal to Base Rate");
                                    txtMaxRate.RaiseValidationError();
                                    result = false;
                                }
                            }
                            else if (txtMinRate.Text != "")
                            {
                                if (Convert.ToDouble(txtMaxRate.Text) < Convert.ToDouble(txtMinRate.Text))
                                {
                                    txtMaxRate.SetValidation(" Max Rate should be greater than or equal to Max Rate");
                                    txtMaxRate.RaiseValidationError();
                                    result = false;
                                }
                            }
                            else
                                txtMaxRate.ClearValidationError();
                        }

                        else
                        {
                            txtMaxRate.ClearValidationError();
                        }

                        //if (Convert.ToDecimal(txtServiceRate.Text) > Convert.ToDecimal(txtMaxRate.Text) || Convert.ToDecimal(txtServiceRate.Text) < Convert.ToDecimal(txtMinRate.Text))
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //                                          new MessageBoxControl.MessageBoxChildWindow("Error occured while adding service rate", "Service rate must be in between min. rate & max. rate", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //    msgW1.Show();
                        //    result = false;
                        //}


                    

                    }
                    catch (Exception Ex)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                                                        new MessageBoxControl.MessageBoxChildWindow("", "Please enter max. rate min. rate correctly", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        result = false;
                    }
                }
                    

                    if (chkServiceTax.IsChecked == true)
                    {
                        //if ((!txtServiceTaxPercentage.Text.Equals("")) && (txtServiceTaxPercentage.Text != "0"))
                        //{
                        //    if (!txtServiceTaxAmount.Text.Equals("") && (txtServiceTaxAmount.Text!="0"))
                        //    {
                        //        MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                        //     new MessageBoxControl.MessageBoxChildWindow("Only one can be added", "Either Service Tax Percentage or Service Tax Amount Can Be Added!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //        msgWindowUpdate.Show();

                        //        result = false;
                        //    }
                        //}

                        if ((txtServiceTaxPercentage.Text.Equals("")) || (txtServiceTaxPercentage.Text == "0"))
                        {
                            if (txtServiceTaxAmount.Text.Equals("") || (txtServiceTaxAmount.Text == "0"))
                            {
                                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Service Tax Percentage or Service Tax Amount Need To Be Added!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindowUpdate.Show();
                                result = false;

                            }
                        }

                    }

                    if (chkStaffDiscount.IsChecked == true)
                    {
                        //if (!txtStaffDiscountPercentage.Text.Equals("")&& (txtStaffDiscountPercentage.Text!="0"))
                        //{
                        //    if (!txtStaffDiscountAmount.Text.Equals("")&&(txtStaffDiscountAmount.Text!="0"))
                        //    {
                        //        MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                        //     new MessageBoxControl.MessageBoxChildWindow("Only one can be added", "Either Staff Discount Percentage or Staff Discount Amount Can Be Added!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //        msgWindowUpdate.Show();

                        //        result = false;
                        //    }
                        //}
                        if (txtStaffDiscountPercentage.Text.Equals("") || (txtStaffDiscountPercentage.Text == "0"))
                        {
                            if (txtStaffDiscountAmount.Text.Equals("") || (txtStaffDiscountAmount.Text == "0"))
                            {
                                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Staff Discount Percentage or Staff Discount Amount Need To Be Added!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindowUpdate.Show();

                                result = false;
                            }
                        }
                    }

                    if (chkStaffParentDiscount.IsChecked == true)
                    {
                        //if (!txtStaffParentPercentage.Text.Equals("") && (txtStaffParentPercentage.Text!="0"))
                        //{
                        //    if (!txtStaffParentAmount.Text.Equals("") && (txtStaffParentAmount.Text!="0"))
                        //    {
                        //        MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                        //     new MessageBoxControl.MessageBoxChildWindow("Only one can be added", "Either Staff Parent Percentage or Staff Parent Amount Can Be Added!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //        msgWindowUpdate.Show();

                        //        result = false;
                        //    }
                        //}

                        if (txtStaffParentPercentage.Text.Equals("") || (txtStaffParentPercentage.Text == "0"))
                        {
                            if (txtStaffParentAmount.Text.Equals("") || (txtStaffParentAmount.Text == "0"))
                            {
                                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Either Staff Parent Percentage or Staff Parent Amount Need To Be Added!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindowUpdate.Show();

                                result = false;
                            }
                        }

                    }

                    if (chkConcession.IsChecked == true)
                    {
                        //if (!txtConcessionPercentage.Text.Equals("") && (txtConcessionPercentage.Text!="0"))
                        //{
                        //    if (!txtConcessionAmount.Text.Equals("") && (txtConcessionAmount.Text!="0"))
                        //    {
                        //        MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                        //     new MessageBoxControl.MessageBoxChildWindow("Only one can be added", "Either Concession Percentage or Concession Amount Can Be Added!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //        msgWindowUpdate.Show();

                        //        result = false;
                        //    }
                        //}

                        if (txtConcessionPercentage.Text.Equals("") || (txtConcessionPercentage.Text == "0"))
                        {
                            if (txtConcessionAmount.Text.Equals("") || (txtConcessionAmount.Text == "0"))
                            {
                                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Either Concession Percentage or Concession Amount Can Be Added!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindowUpdate.Show();

                                result = false;
                            }
                        }
                    }


                    if (chkApplicableToAllDoctors.IsChecked == true)
                    {

                        //if (!txtDoctorApplicablePercent.Text.Equals("") && (txtDoctorApplicablePercent.Text!="0"))
                        //{
                        //    if (!txtDoctorApplicableAmount.Text.Equals("") && (txtDoctorApplicableAmount.Text!="0"))
                        //    {
                        //        MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                        //     new MessageBoxControl.MessageBoxChildWindow("Only one can be added", "Either Doctor Share Percentage or Doctor Share Amount Can Be Added!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //        msgWindowUpdate.Show();

                        //        result = false;
                        //    }
                        //}

                        if (txtDoctorApplicablePercent.Text.Equals("") || (txtDoctorApplicablePercent.Text == "0"))
                        {
                            if (txtDoctorApplicableAmount.Text.Equals("") || (txtDoctorApplicableAmount.Text == "0"))
                            {
                                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Either Doctor Share Percentage or Doctor Share Amount Need To Be Added!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindowUpdate.Show();

                                result = false;
                            }
                        }

                    }
                
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindowUpdate.Show();

                result = false;
            }
           
            return result;
        }

        public long pkServiceID { get; set; }

        public long pkTariffServiceMasterID { get; set; }

        private void chkServiceTax_Click(object sender, RoutedEventArgs e)
        {
            if (chkServiceTax.IsChecked==true)
            {
                txtServiceTaxAmount.IsEnabled = true;
                txtServiceTaxPercentage.IsEnabled = true;    
            }
            else
            {
                txtServiceTaxAmount.IsEnabled = false;
                txtServiceTaxPercentage.IsEnabled = false;

                txtServiceTaxAmount.Text = "0.00";
                txtServiceTaxPercentage.Text = "0.00";   
            }
            
        }

        private void chkStaffDiscount_Click(object sender, RoutedEventArgs e)
        {
            if (chkStaffDiscount.IsChecked == true)
            {
                txtStaffDiscountAmount.IsEnabled = true;
                txtStaffDiscountPercentage.IsEnabled = true;
            }
            else
            {
                txtStaffDiscountAmount.IsEnabled = false;
                txtStaffDiscountPercentage.IsEnabled = false;

                txtStaffDiscountAmount.Text = "0.00";
                txtStaffDiscountPercentage.Text = "0.00";
            }
        }

        private void chkStaffParentDiscount_Click(object sender, RoutedEventArgs e)
        {
            if (chkStaffParentDiscount.IsChecked == true)
            {
                txtStaffParentAmount.IsEnabled = true;
                txtStaffParentPercentage.IsEnabled = true;
            }
            else
            {
                txtStaffParentAmount.IsEnabled = false;
                txtStaffParentPercentage.IsEnabled = false;

                txtStaffParentAmount.Text = "0.00";
                txtStaffParentPercentage.Text = "0.00";


            }
        }

        private void chkConcession_Click(object sender, RoutedEventArgs e)
        {
            if (chkConcession.IsChecked == true)
            {
                txtConcessionAmount.IsEnabled = true;
                txtConcessionPercentage.IsEnabled = true;
            }
            else
            {
                txtConcessionAmount.IsEnabled = false;
                txtConcessionPercentage.IsEnabled = false;

                txtConcessionAmount.Text = "0.00";
                txtConcessionPercentage.Text = "0.00";
            }
        }

        private void chkApplicableToAllDoctors_Click(object sender, RoutedEventArgs e)
        {
            if (chkApplicableToAllDoctors.IsChecked == true)
            {
                txtDoctorApplicableAmount.IsEnabled = true;
                txtDoctorApplicablePercent.IsEnabled = true;
            }
            else
            {
                txtDoctorApplicableAmount.IsEnabled = false;
                txtDoctorApplicablePercent.IsEnabled = false;

                txtDoctorApplicableAmount.Text = "0.00";
                txtDoctorApplicablePercent.Text = "0.00";
            }
        }

        private void chkRateEditable_Click(object sender, RoutedEventArgs e)
        {
            if (chkRateEditable.IsChecked == true)
            {
                txtMinRate.IsEnabled = true;
                txtMaxRate.IsEnabled = true;
            }
            else
            {
                txtMinRate.IsEnabled = false;
                txtMaxRate.IsEnabled = false;

                txtMinRate.Text = "0.00";
                txtMaxRate.Text = "0.00";
            }
        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
           
            CheckBox chkstatus = (CheckBox)sender;
            long tagID;
            bool status;
          
                tagID = (long)chkstatus.Tag;
                status = (bool)chkstatus.IsChecked;

                ChangeTariffServiceStatus(tagID, status);
            
        }
               
        private void ChangeTariffServiceStatus(long tagID,bool status)
        {
            clsChangeTariffServiceStatusBizActionVO BizActionobj = new clsChangeTariffServiceStatusBizActionVO();
            BizActionobj.ServiceMasterDetails = new clsServiceMasterVO();
            BizActionobj.ServiceMasterDetails.ID = tagID;
            if (status==true)
            {
                BizActionobj.SuccessStatus = 1;
            }
            else if (status == false)
            {
                BizActionobj.SuccessStatus = 0;
            }
            //BizActionobj.SuccessStatus=
           // BizActionobj.ServiceMasterDetails.Status = status;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {

                    string msgTitle = "";
                    string msgText = "Service status Updated Successfully";

                    MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);


                    msgW.Show();
                    objAnimation.Invoke(RotationType.Backward);
                    FillTariffServiceListGrid(true);



                }
                else
                {

                }

            };
            client.ProcessAsync(BizActionobj, new clsUserVO());
            client.CloseAsync();

        }

        private void CmdCancel1_Click(object sender, RoutedEventArgs e)
        {
            objAnimation.Invoke(RotationType.Backward);
            
        }

        private void cboSpecialization1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboSpecialization1.SelectedItem != null)
            {
                FillSubSpecializationForBackPanel(((MasterListItem)cboSpecialization1.SelectedItem).ID.ToString());
            }
        }

        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
            mElement.Text = "Billing Configuration";          

            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmBillingConfiguration") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

            
           
        }

        private void txtServiceTaxPercentage_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtServiceTaxPercentage.Text.Equals("") && (txtServiceTaxPercentage.Text != "0"))
                {
                    if (Extensions.IsItDecimal(txtServiceTaxPercentage.Text) == false)
                    {

                        if (Convert.ToDecimal(txtServiceTaxPercentage.Text) > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                  new MessageBoxControl.MessageBoxChildWindow("", "Service tax percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            txtServiceTaxPercentage.Text = "0.00";
                            txtServiceTaxAmount.Text = "0.00";

                            return;
                        }


                        String str1 = txtServiceTaxPercentage.Text.Substring(txtServiceTaxPercentage.Text.IndexOf(".") + 1);
                        if (Convert.ToDecimal(str1) > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                                        new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            txtServiceTaxPercentage.Text = "0.00";
                            return;
                        }
                    }
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal ServiceRate = 0;
                        ServiceRate = Convert.ToDecimal(txtServiceRate.Text);

                        decimal ServiceTaxPer = 0;
                        if (Extensions.IsItDecimal(txtServiceTaxPercentage.Text) == false)
                        {
                            ServiceTaxPer = Convert.ToDecimal(txtServiceTaxPercentage.Text);
                        }
                        else
                        {
                            txtServiceTaxPercentage.SetValidation(" Service Tax should be number");
                            txtServiceTaxPercentage.RaiseValidationError();
                            ServiceTaxPer = 0;
                        }


                        decimal ServiceTaxAmount = 0;
                        ServiceTaxAmount = ((ServiceRate * ServiceTaxPer) / 100);

                        txtServiceTaxAmount.Text = ServiceTaxAmount.ToString("0.00");

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                  new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtServiceTaxPercentage.Text = "0.00";
            }
        }

        private void txtServiceTaxAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtServiceTaxAmount.Text.Equals("") && (txtServiceTaxAmount.Text != "0"))
                {
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal ServiceRate = 0;
                        ServiceRate = Convert.ToDecimal(txtServiceRate.Text);

                        decimal ServiceTaxAmount = 0;
                        if (Extensions.IsItDecimal(txtServiceTaxAmount.Text) == false)
                        {
                            ServiceTaxAmount = Convert.ToDecimal(txtServiceTaxAmount.Text);
                        }
                        else
                        {
                            txtServiceTaxAmount.SetValidation(" Service Tax Amount should be number");
                            txtServiceTaxAmount.RaiseValidationError();
                            ServiceTaxAmount = 0;
                        }

                        decimal ServiceTaxPer = 0;
                        ServiceTaxPer = (100 * ServiceTaxAmount) / ServiceRate;
                        if (ServiceTaxPer > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                  new MessageBoxControl.MessageBoxChildWindow("", "Service tax percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            txtServiceTaxPercentage.Text = "0.00";
                            txtServiceTaxAmount.Text = "0.00";
                            ServiceTaxPer = 0;

                            return;
                        }

                        txtServiceTaxPercentage.Text = ServiceTaxPer.ToString("0.00");

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                  new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtServiceTaxAmount.Text = "0.00";
            }
        }

        private void txtStaffDiscountPercentage_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtStaffDiscountPercentage.Text.Equals("") && (txtStaffDiscountPercentage.Text != "0"))
                {
                    if (Extensions.IsItDecimal(txtStaffDiscountPercentage.Text) == false)
                    {
                        if (Convert.ToDecimal(txtStaffDiscountPercentage.Text) > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                  new MessageBoxControl.MessageBoxChildWindow("", "staff discount percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            txtStaffDiscountPercentage.Text = "0.00";
                            txtStaffDiscountAmount.Text = "0.00";
                            return;
                        }
                        String str1 = txtStaffDiscountPercentage.Text.Substring(txtStaffDiscountPercentage.Text.IndexOf(".") + 1);
                        if (Convert.ToDecimal(str1) > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                                        new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            txtStaffDiscountPercentage.Text = "0.00";
                            return;
                        }
                    }
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal Rate = 0;
                        Rate = Convert.ToDecimal(txtServiceRate.Text);

                        decimal Percent = 0;
                        if (Extensions.IsItDecimal(txtStaffDiscountPercentage.Text) == false)
                        {
                            Percent = Convert.ToDecimal(txtStaffDiscountPercentage.Text);
                        }

                        else
                        {
                            txtStaffDiscountPercentage.SetValidation(" Staff Discount Percent should be number");
                            txtStaffDiscountPercentage.RaiseValidationError();
                            Percent = 0;
                        }

                        decimal TaxAmount = 0;
                        TaxAmount = ((Rate * Percent) / 100);

                        txtStaffDiscountAmount.Text = TaxAmount.ToString("0.00");

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                  new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtStaffDiscountPercentage.Text = "0.00";
            }
        }

        private void txtStaffDiscountAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtStaffDiscountAmount.Text.Equals("") && (txtStaffDiscountAmount.Text != "0"))
                {
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal Rate = 0;
                        Rate = Convert.ToDecimal(txtServiceRate.Text);

                        decimal TaxAmount = 0;
                        if (Extensions.IsItDecimal(txtStaffDiscountAmount.Text) == false)
                        {
                            TaxAmount = Convert.ToDecimal(txtStaffDiscountAmount.Text);

                        }

                        else
                        {
                            txtStaffDiscountAmount.SetValidation(" Staff Discount Amount should be number");
                            txtStaffDiscountAmount.RaiseValidationError();
                            TaxAmount = 0;
                        }

                        decimal TaxPer = 0;
                        TaxPer = (100 * TaxAmount) / Rate;
                        if (TaxPer > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                  new MessageBoxControl.MessageBoxChildWindow("", "staff discount percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            txtStaffDiscountPercentage.Text = "0.00";
                            txtStaffDiscountAmount.Text = "0.00";
                            TaxPer = 0;
                            return;
                        }

                        txtStaffDiscountPercentage.Text = TaxPer.ToString("0.00");

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                  new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtStaffDiscountAmount.Text = "0.00";
            }


        }

        private void txtStaffParentPercentage_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtStaffParentPercentage.Text.Equals("") && (txtStaffParentPercentage.Text != "0"))
                {
                    if (!txtStaffParentPercentage.Text.Equals("") && (txtStaffParentPercentage.Text != "0"))
                    {
                        if (Extensions.IsItDecimal(txtStaffParentPercentage.Text) == false)
                        {
                            if (Convert.ToDecimal(txtStaffParentPercentage.Text) > 100)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                                      new MessageBoxControl.MessageBoxChildWindow("", "staff parent percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW.Show();
                                txtStaffParentPercentage.Text = "0.00";
                                txtStaffParentAmount.Text = "0.00";
                                return;
                            }
                            String str1 = txtStaffParentPercentage.Text.Substring(txtStaffParentPercentage.Text.IndexOf(".") + 1);
                            if (Convert.ToDecimal(str1) > 100)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                            new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW.Show();
                                txtStaffParentPercentage.Text = "0.00";
                                return;
                            }
                        }
                    }
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal Rate = 0;
                        Rate = Convert.ToDecimal(txtServiceRate.Text);

                        decimal Percent = 0;
                        if (Extensions.IsItDecimal(txtStaffParentPercentage.Text) == false)
                        {
                            Percent = Convert.ToDecimal(txtStaffParentPercentage.Text);
                        }

                        else
                        {
                            txtStaffParentPercentage.SetValidation(" Staff Parent Percent should be number");
                            txtStaffParentPercentage.RaiseValidationError();
                            Percent = 0;
                        }

                        decimal TaxAmount = 0;
                        TaxAmount = ((Rate * Percent) / 100);

                        txtStaffParentAmount.Text = TaxAmount.ToString("0.00");

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                  new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtStaffParentPercentage.Text = "0.00";
            }
        }

        private void txtStaffParentAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtStaffParentAmount.Text.Equals("") && (txtStaffParentAmount.Text != "0"))
                {
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal Rate = 0;
                        Rate = Convert.ToDecimal(txtServiceRate.Text);

                        decimal TaxAmount = 0;
                        if (Extensions.IsItDecimal(txtStaffParentAmount.Text) == false)
                        {
                            TaxAmount = Convert.ToDecimal(txtStaffParentAmount.Text);

                        }

                        else
                        {
                            txtStaffParentAmount.SetValidation(" Staff Parent Amount should be number");
                            txtStaffParentAmount.RaiseValidationError();
                            TaxAmount = 0;
                        }

                        decimal TaxPer = 0;
                        TaxPer = (100 * TaxAmount) / Rate;
                        if (TaxPer > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                  new MessageBoxControl.MessageBoxChildWindow("", "staff parent percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            txtStaffParentPercentage.Text = "0.00";
                            txtStaffParentAmount.Text = "0.00";
                            TaxPer = 0;
                            return;
                        }

                        txtStaffParentPercentage.Text = TaxPer.ToString("0.00");

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                  new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtStaffParentAmount.Text = "0.00";
            }


        }

        private void txtConcessionPercentage_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtConcessionPercentage.Text.Equals("") && (txtConcessionPercentage.Text != "0"))
                {
                    if (!txtConcessionPercentage.Text.Equals("") && (txtConcessionPercentage.Text != "0"))
                    {
                        if (Extensions.IsItDecimal(txtConcessionPercentage.Text) == false)
                        {
                            if (Convert.ToDecimal(txtConcessionPercentage.Text) > 100)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                                      new MessageBoxControl.MessageBoxChildWindow("", "Concession percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW.Show();
                                txtConcessionPercentage.Text = "0.00";
                                txtConcessionAmount.Text = "0.00";
                                return;
                            }
                            String str1 = txtConcessionPercentage.Text.Substring(txtConcessionPercentage.Text.IndexOf(".") + 1);
                            if (Convert.ToDecimal(str1) > 100)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                            new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW.Show();
                                txtConcessionPercentage.Text = "0.00";
                                return;
                            }
                        }
                    }
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal Rate = 0;
                        Rate = Convert.ToDecimal(txtServiceRate.Text);

                        decimal Percent = 0;
                        if (Extensions.IsItDecimal(txtConcessionPercentage.Text) == false)
                        {
                            Percent = Convert.ToDecimal(txtConcessionPercentage.Text);
                        }

                        else
                        {
                            txtConcessionPercentage.SetValidation("Concession Percent should be number");
                            txtConcessionPercentage.RaiseValidationError();
                            Percent = 0;
                        }

                        decimal TaxAmount = 0;
                        TaxAmount = ((Rate * Percent) / 100);

                        txtConcessionAmount.Text = TaxAmount.ToString("0.00");

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                  new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtConcessionPercentage.Text = "0.00";
            }
        }

        private void txtConcessionAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtConcessionAmount.Text.Equals("") && (txtConcessionAmount.Text != "0"))
                {
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal Rate = 0;
                        Rate = Convert.ToDecimal(txtServiceRate.Text);

                        decimal TaxAmount = 0;
                        if (Extensions.IsItDecimal(txtConcessionAmount.Text) == false)
                        {
                            TaxAmount = Convert.ToDecimal(txtConcessionAmount.Text);

                        }

                        else
                        {
                            txtConcessionAmount.SetValidation(" Concession Amount should be number");
                            txtConcessionAmount.RaiseValidationError();
                            TaxAmount = 0;
                        }


                        decimal TaxPer = 0;
                        TaxPer = (100 * TaxAmount) / Rate;
                        if (TaxPer > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                  new MessageBoxControl.MessageBoxChildWindow("", "Concession percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            txtConcessionPercentage.Text = "0.00";
                            txtConcessionAmount.Text = "0.00";
                            TaxPer = 0;
                            return;
                        }

                        txtConcessionPercentage.Text = TaxPer.ToString("0.00");

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                  new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtConcessionAmount.Text = "0.00";
            }

        }

        private void txtDoctorApplicablePercent_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtDoctorApplicablePercent.Text.Equals("") && (txtDoctorApplicablePercent.Text != "0"))
                {
                    if (!txtDoctorApplicablePercent.Text.Equals("") && (txtDoctorApplicablePercent.Text != "0"))
                    {
                        if (Extensions.IsItDecimal(txtDoctorApplicablePercent.Text) == false)
                        {
                            if (Convert.ToDecimal(txtDoctorApplicablePercent.Text) > 100)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                                      new MessageBoxControl.MessageBoxChildWindow("", "Doctor applicable percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW.Show();
                                txtDoctorApplicablePercent.Text = "0.00";
                                txtDoctorApplicableAmount.Text = "0.00";
                                return;
                            }
                            String str1 = txtDoctorApplicablePercent.Text.Substring(txtDoctorApplicablePercent.Text.IndexOf(".") + 1);
                            if (Convert.ToDecimal(str1) > 100)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                            new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW.Show();
                                txtDoctorApplicablePercent.Text = "0.00";
                                return;
                            }
                        }
                    }
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal Rate = 0;
                        Rate = Convert.ToDecimal(txtServiceRate.Text);

                        decimal Percent = 0;
                        if (Extensions.IsItDecimal(txtDoctorApplicablePercent.Text) == false)
                        {
                            Percent = Convert.ToDecimal(txtDoctorApplicablePercent.Text);

                        }

                        else
                        {
                            txtDoctorApplicablePercent.SetValidation("Doctor Applicable Percent should be number");
                            txtDoctorApplicablePercent.RaiseValidationError();
                            Percent = 0;
                        }

                        decimal TaxAmount = 0;
                        TaxAmount = ((Rate * Percent) / 100);

                        txtDoctorApplicableAmount.Text = TaxAmount.ToString("0.00");

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                  new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtDoctorApplicablePercent.Text = "0.00";
            }
        }

        private void txtDoctorApplicableAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtDoctorApplicableAmount.Text.Equals("") && (txtDoctorApplicableAmount.Text != "0"))
                {
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal Rate = 0;
                        Rate = Convert.ToDecimal(txtServiceRate.Text);

                        decimal TaxAmount = 0;
                        if (Extensions.IsItDecimal(txtDoctorApplicableAmount.Text) == false)
                        {
                            TaxAmount = Convert.ToDecimal(txtDoctorApplicableAmount.Text);

                        }

                        else
                        {
                            txtDoctorApplicableAmount.SetValidation(" Doctor Applicable Amount should be number");
                            txtDoctorApplicableAmount.RaiseValidationError();
                            TaxAmount = 0;
                        }


                        decimal TaxPer = 0;
                        TaxPer = (100 * TaxAmount) / Rate;
                        if (TaxPer > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                  new MessageBoxControl.MessageBoxChildWindow("", "Doctor applicable percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            txtDoctorApplicablePercent.Text = "0.00";
                            txtDoctorApplicableAmount.Text = "0.00";
                            TaxPer = 0;
                            return;
                        }

                        txtDoctorApplicablePercent.Text = TaxPer.ToString("0.00");

                    }
                }

            }
            catch (Exception ex)
            {
            
                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                  new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtDoctorApplicablePercent.Text = "0.00";
            }

        }

        decimal PrevServiceRate = 0;
        private void txtServiceRate_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtServiceRate.Text != "")
                {
                    if(Extensions.IsItDecimal(txtServiceRate.Text) == true)
                    {
                        txtServiceRate.SetValidation("Rate should be Numeric");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                            new MessageBoxControl.MessageBoxChildWindow("", "Rate should be Numeric", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                        txtServiceRate.Text = PrevServiceRate.ToString();
                        return;
                    }
                    else if (chkRateEditable.IsChecked == true)
                    {
                        try
                        {
                            if (Convert.ToDecimal(txtServiceRate.Text) > Convert.ToDecimal(txtMaxRate.Text) || Convert.ToDecimal(txtServiceRate.Text) < Convert.ToDecimal(txtMinRate.Text))
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                                      new MessageBoxControl.MessageBoxChildWindow("Error occured while adding service rate", "Service rate must be in between min. rate & max. rate", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                                txtServiceRate.Text = PrevServiceRate.ToString();
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                                            new MessageBoxControl.MessageBoxChildWindow("", "Incorrect min.rate or max. rate", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            txtMaxRate.Text = "0.00";
                            txtMinRate.Text = "0.00";
                            txtServiceRate.Text = "0.00";
                            return;
                        }
                    }

                    decimal ServiceRate = 0;
                    ServiceRate = Convert.ToDecimal(txtServiceRate.Text);

                    if (chkServiceTax.IsChecked == true)
                    {

                        decimal ServiceTaxPer = Convert.ToDecimal(txtServiceTaxPercentage.Text);

                        decimal ServiceTaxAmount = 0;
                        ServiceTaxAmount = ((ServiceRate * ServiceTaxPer) / 100);

                        txtServiceTaxAmount.Text = ServiceTaxAmount.ToString("0.00");
                    }
                    if (chkStaffDiscount.IsChecked == true)
                    {
                        decimal Percent = 0;
                        Percent = Convert.ToDecimal(txtStaffDiscountPercentage.Text);
                        decimal TaxAmount = 0;
                        TaxAmount = ((ServiceRate * Percent) / 100);

                        txtStaffDiscountAmount.Text = TaxAmount.ToString("0.00");

                    }
                    if (chkStaffParentDiscount.IsChecked == true)
                    {
                        decimal Percent = 0;
                        Percent = Convert.ToDecimal(txtStaffParentPercentage.Text);
                        decimal TaxAmount = 0;
                        TaxAmount = ((ServiceRate * Percent) / 100);
                        txtStaffParentAmount.Text = TaxAmount.ToString("0.00");
                    }
                    if (chkConcession.IsChecked == true)
                    {
                        decimal Percent = 0;
                        Percent = Convert.ToDecimal(txtConcessionPercentage.Text);
                        decimal TaxAmount = 0;
                        TaxAmount = ((ServiceRate * Percent) / 100);
                        txtConcessionAmount.Text = TaxAmount.ToString("0.00");
                    }
                    if (chkApplicableToAllDoctors.IsChecked == true)
                    {
                        decimal Percent = 0;
                        Percent = Convert.ToDecimal(txtDoctorApplicablePercent.Text);
                        decimal TaxAmount = 0;
                        TaxAmount = ((ServiceRate * Percent) / 100);
                        txtDoctorApplicableAmount.Text = TaxAmount.ToString("0.00");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtServiceRate.Text = "0.00";
                return;
            }
        }

        private void chkApplyToAllClasses_Click(object sender, RoutedEventArgs e)
        {
             List<clsServiceTarrifClassRateDetailsVO> List = ( List<clsServiceTarrifClassRateDetailsVO> )grdClass.ItemsSource;
            if (List != null)
            {
                if (chkApplyToAllClasses.IsChecked == true)
                {
                    foreach (var item in List)
                    {
                        item.Rate = Convert.ToDouble(txtServiceRate.Text);
                        item.Status = true;
                    }
                }
                else
                {
                    foreach (var item in List)
                    {
                        item.Status = false;
                    }
                }
                
                grdClass.ItemsSource = null;
                grdClass.ItemsSource = List;

            }

        }

        private void chkAddClass_Click(object sender, RoutedEventArgs e)
        {
            if (grdClass.SelectedItem != null  )
            {
                clsServiceTarrifClassRateDetailsVO myObj = grdClass.SelectedItem as clsServiceTarrifClassRateDetailsVO;
                if (myObj.ClassID == 1 && myObj.Status == false)
                {
                    ((clsServiceTarrifClassRateDetailsVO)grdClass.SelectedItem).Status = true;
                }
            }
        }

       

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FillTariffServiceListGrid(false);
            }
        }
    }
}
