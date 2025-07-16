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
using System.Collections.ObjectModel;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Administration.PackageNew;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using System.Windows.Data;
using System.Text;
using PalashDynamics.ValueObjects.Administration;
using OPDModule;



namespace CIMS.Forms
{
    public partial class ConditionalServiceSearchForPackage : ChildWindow
    {
        #region Variable Declaration
        public event RoutedEventHandler OnAddButton_Click;
        public List<bool> check = new List<bool>();

        public List<clsServiceMasterVO> ServiceItemSource { get; set; }

        public List<clsServiceMasterVO> SelectedServices { get; set; }

        private ObservableCollection<clsServiceMasterVO> _OtherServiceSelected;
        public ObservableCollection<clsServiceMasterVO> SelectedOtherServices { get { return _OtherServiceSelected; } }

        public long TariffID { get; set; }
        public long PackageID { get; set; }         // Added BY CDS
        public long MemberRelationID { get; set; }  // Added BY CDS

        public long MainServiceID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }

        public long SponsorID { get; set; }
        public long SponsorUnitID { get; set; }

        public string MainServiceName { get; set; }

        public long PatientSourceID { get; set; }

        public long ClassID { get; set; }
        public long PatientTariffID { get; set; }

        bool IsPageLoded = false;
        bool IsTariffFisrtFill = true;

        public PagedSortableCollectionView<clsServiceMasterVO> DataList { get; private set; }

        WaitIndicator Indicatior = null;

        public long PackageTariffID { get; set; }
        public long ServiceTariffID { get; set; }

        #endregion

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

        public ConditionalServiceSearchForPackage()
        {
            InitializeComponent();
            ClassID = 1; // Default

            this.Loaded += new RoutedEventHandler(ConditionalServiceSearchForPackage_Loaded);

            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsServiceMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;
            //======================================================
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            long ID = 0;

            //if ((MasterListItem)cmbTariff.SelectedItem != null)
            //    ID = ((MasterListItem)cmbTariff.SelectedItem).ID;

            GetPackageConditionalServicesAndRelations();
        }

        private void ConditionalServiceSearchForPackage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                GetPackageConditionalServicesAndRelations();
            }

            IsPageLoded = true;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;
            if (_OtherServiceSelected == null)
            {
                isValid = false;
            }
            else if (_OtherServiceSelected.Count <= 0)
            {
                isValid = false;
            }

            if (isValid)
            {
                this.DialogResult = true;
                if (OnAddButton_Click != null)
                    OnAddButton_Click(this, new RoutedEventArgs());
            }
            else
            {
                string strMsg = "No Service/s Selected for Adding";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

            }

        }

        PalashServiceClient client = null;
        public PagedCollectionView pcv = null;

        private void GetPackageConditionalServicesAndRelations()
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();

            try
            {
                clsGetPackageConditionalServicesNewBizActionVO BizAction = new clsGetPackageConditionalServicesNewBizActionVO();

                BizAction.ServiceConditionList = new List<clsServiceMasterVO>();
                BizAction.TariffID = TariffID;
                BizAction.PackageID = PackageID;
                BizAction.ServiceID = MainServiceID;
                BizAction.PatientID = PatientID;
                BizAction.PatientUnitID = PatientUnitID;

                BizAction.SponsorID = SponsorID;
                BizAction.SponsorUnitID = SponsorUnitID;

                BizAction.PatientDateOfBirth = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth;
               
                //BizAction.MemberRelationID = ((IApplicationConfiguration)App.Current).SelectedPatient.MemberRelationID; ;
                ////////BizAction.MemberRelationID = 2;
                // Added By CDS                 
                BizAction.MemberRelationID = MemberRelationID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                dgServiceList.ItemsSource = null;

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsGetPackageConditionalServicesNewBizActionVO)arg.Result).ServiceConditionList != null)
                    {

                        clsGetPackageConditionalServicesNewBizActionVO result = arg.Result as clsGetPackageConditionalServicesNewBizActionVO;
                        DataList.TotalItemCount = result.ServiceConditionList.Count;  // result.TotalRows;
                        DataList.Clear();

                        if (result.ServiceConditionList != null)
                        {

                            foreach (var item in result.ServiceConditionList)
                            {
                                DataList.Add(item);
                                lblMainServicename.Text = MainServiceName;
                                txtServiceUsedQty.Text = item.MainServiceUsedQuantity.ToString(); //"1";
                            }

                            pcv = new PagedCollectionView(DataList);
                            pcv.GroupDescriptions.Add(new PropertyGroupDescription("ConditionType"));

                            //pcv = new PagedCollectionView(RentalAdvanceDataList);
                            //pcv.GroupDescriptions.Add(new PropertyGroupDescription("AdvanceFrom"));

                            //dgAdvances.ItemsSource = null;
                            //dgAdvances.ItemsSource = pcv;


                            dgServiceList.ItemsSource = null;
                            dgServiceList.ItemsSource = pcv;  //DataList;

                            dgDataPager.Source = null;
                            dgDataPager.PageSize = 100;  //BizAction.MaximumRows;
                            dgDataPager.Source = pcv;  //DataList;

                        }
                    }
                    else
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }

                    Indicatior.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());

            }
            catch (Exception)
            {
                // throw;
                Indicatior.Close();
            }
            finally
            {
                Indicatior.Close();
            }
        }

        private void chkService_Click(object sender, RoutedEventArgs e)
        {
            #region OLD CODE
            //if (((CheckBox)sender).IsChecked == true)
            //{

            //    check[dgServiceList.SelectedIndex] = true;
            //}
            //else
            //{
            //    check[dgServiceList.SelectedIndex] = false;
            //}
            #endregion

            bool IsValid = true;
            if (dgServiceList.SelectedItem != null)
            {
                if (_OtherServiceSelected == null)
                    _OtherServiceSelected = new ObservableCollection<clsServiceMasterVO>();

                CheckBox chk = (CheckBox)sender;
                StringBuilder strError = new StringBuilder();
                bool CheckGenderForPackage = false;
                bool IsTariffPackage = false;

                //// Used to check whether it is Package. If it is then Code = "Package" (Code set using ApplicableType field in SP - "CIMS_GetPatientTariffDetails" and table - "M_TariffMaster")
                //if (((MasterListItem)cmbTariff.SelectedItem).Code == "Package")
                //{
                //    IsTariffPackage = true;

                //    //If it is Package then check whether selected service Gender = selected patient gender
                //    if (((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableTo == ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString != "Both")
                //    {
                //        CheckGenderForPackage = true;
                //    }
                //}

                string strMsg = "";

                bool AndFlag = false;
                bool OrFlag = false;

                if (((clsServiceMasterVO)dgServiceList.SelectedItem).ConditionType == "AND")
                {
                    var item1 = from r in _OtherServiceSelected
                                where r.ConditionType == "OR"
                                select new clsServiceMasterVO
                                {
                                    Status = r.Status,
                                    ID = r.ID,
                                    UnitID = r.UnitID,
                                    //ServiceID = r.ServiceID,
                                    ServiceName = r.ServiceName
                                };

                    if (item1.ToList().Count > 0)
                    {
                        OrFlag = true;
                    }
                }

                if (((clsServiceMasterVO)dgServiceList.SelectedItem).ConditionType == "OR")
                {
                    var item2 = from r in _OtherServiceSelected
                                where r.ConditionType == "AND"
                                select new clsServiceMasterVO
                                {
                                    Status = r.Status,
                                    ID = r.ID,
                                    UnitID = r.UnitID,
                                    //ServiceID = r.ServiceID,
                                    ServiceName = r.ServiceName
                                };

                    if (item2.ToList().Count > 0)
                    {
                        AndFlag = true;
                    }
                }

                bool UsedMainFlag = false;
                bool UsedAndFlag = false;
                bool UsedOrFlag = false;
                bool UsedOrFlagSec = false;
                bool UsedAndFlagSec = false;

                long UsedMainCnt = 0;
                long UsedAndCnt = 0;
                long UsedOrCnt = 0;

                string SecORService = "";

                if (txtServiceUsedQty.Text.Trim() != String.Empty && txtServiceUsedQty.Text.IsValueDouble())
                    UsedMainCnt = Convert.ToInt64(txtServiceUsedQty.Text.Trim());

                if (UsedMainCnt > 0)
                    UsedMainFlag = true;

                if (UsedMainFlag == true && ((clsServiceMasterVO)dgServiceList.SelectedItem).ConditionType == "OR")
                {
                    //UsedOrFlag = true;
                }

                if (UsedMainFlag == true && ((clsServiceMasterVO)dgServiceList.SelectedItem).ConditionType == "AND")
                {
                    var item3 = from r in DataList
                                where r.ConditionType == "OR" && r.ConditionalUsedQuantity > 0 && r.IsSet == true
                                && r.ConditionServiceID != ((clsServiceMasterVO)dgServiceList.SelectedItem).ConditionServiceID
                                select new clsServiceMasterVO
                                {
                                    Status = r.Status,
                                    ID = r.ID,
                                    UnitID = r.UnitID,
                                    //ServiceID = r.ServiceID,
                                    ServiceName = r.ServiceName
                                };

                    if (item3.ToList().Count > 0)
                    {
                        //UsedOrFlag = true;
                    }
                }

                if (UsedMainFlag == false && ((clsServiceMasterVO)dgServiceList.SelectedItem).ConditionType == "AND")
                {
                    var item4 = from r in DataList
                                where r.ConditionType == "OR" && r.ConditionalUsedQuantity > 0
                                && r.ConditionServiceID != ((clsServiceMasterVO)dgServiceList.SelectedItem).ConditionServiceID
                                select new clsServiceMasterVO
                                {
                                    Status = r.Status,
                                    ID = r.ID,
                                    UnitID = r.UnitID,
                                    //ServiceID = r.ServiceID,
                                    ServiceName = r.ServiceName
                                };

                    if (item4.ToList().Count > 0)
                    {
                        SecORService = (item4.ToList()).FirstOrDefault().ServiceName;
                        //UsedAndFlagSec = true;
                    }
                }

                if (UsedMainFlag == false && ((clsServiceMasterVO)dgServiceList.SelectedItem).ConditionType == "OR")
                {
                    var item5 = from r in DataList
                                where r.ConditionType == "AND" && r.ConditionalUsedQuantity > 0 && r.IsSet == true
                                && r.ConditionServiceID != ((clsServiceMasterVO)dgServiceList.SelectedItem).ConditionServiceID
                                select new clsServiceMasterVO
                                {
                                    Status = r.Status,
                                    ID = r.ID,
                                    UnitID = r.UnitID,
                                    //ServiceID = r.ServiceID,
                                    ServiceName = r.ServiceName
                                };

                    if (item5.ToList().Count > 0)
                    {
                        //UsedAndFlag = true;
                    }
                }

                if (UsedMainFlag == false && ((clsServiceMasterVO)dgServiceList.SelectedItem).ConditionType == "OR")
                {
                    var item6 = from r in DataList
                                where r.ConditionType == "OR" && r.ConditionalUsedQuantity > 0
                                && r.ConditionServiceID != ((clsServiceMasterVO)dgServiceList.SelectedItem).ConditionServiceID
                                select new clsServiceMasterVO
                                {
                                    Status = r.Status,
                                    ID = r.ID,
                                    UnitID = r.UnitID,
                                    //ServiceID = r.ServiceID,
                                    ServiceName = r.ServiceName
                                };

                    if (item6.ToList().Count > 0)
                    {
                        SecORService = (item6.ToList()).FirstOrDefault().ServiceName;
                        //UsedOrFlagSec = true;
                    }
                }

                if (chk.IsChecked == true)
                {
                    if (AndFlag == false && OrFlag == false & UsedAndFlag == false && UsedOrFlag == false && UsedOrFlagSec == false && UsedAndFlagSec == false)
                    {
                        if (_OtherServiceSelected.Count > 0)
                        {
                            var item = from r in _OtherServiceSelected
                                       where r.ID == ((clsServiceMasterVO)dgServiceList.SelectedItem).ID  //ServiceID
                                       select new clsServiceMasterVO
                                       {
                                           Status = r.Status,
                                           ID = r.ID,
                                           UnitID = r.UnitID,
                                           //ServiceID = r.ServiceID,
                                           ServiceName = r.ServiceName
                                       };
                            if (item.ToList().Count > 0)
                            {
                                if (strError.ToString().Length > 0)
                                    strError.Append(",");
                                strError.Append(((clsServiceMasterVO)dgServiceList.SelectedItem).ServiceName);

                                if (!string.IsNullOrEmpty(strError.ToString()))
                                {
                                    strMsg = "Services already Selected : " + strError.ToString();

                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();

                                    //((clsServiceMasterVO)dgServiceList.SelectedItem).SelectService = false;

                                    //dgServiceList.ItemsSource = null;
                                    //dgServiceList.ItemsSource = DataList;
                                    //dgServiceList.UpdateLayout();
                                    //dgServiceList.Focus();

                                    IsValid = false;
                                }
                            }
                            else
                            {

                                _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);

                                //if (IsTariffPackage == false)
                                //{
                                //    _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                                //}
                                //else if (IsTariffPackage == true)
                                //{
                                //    if (CheckGenderForPackage == true && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString != "Both")
                                //    {
                                //        _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                                //    }
                                //    else if (CheckGenderForPackage == false && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString == "Both")
                                //    {
                                //        _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                                //    }
                                //    else
                                //    {
                                //        IsValid = false;

                                //        ((CheckBox)sender).IsChecked = false;

                                //        string strMsg = "Services is only for : " + ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString;

                                //        MessageBoxControl.MessageBoxChildWindow msgW2 =
                                //                   new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                //        msgW2.Show();
                                //    }
                                //}

                            }


                        }
                        else
                        {

                            _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);

                            //if (IsTariffPackage == false)
                            //{
                            //    _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                            //}
                            //else if (IsTariffPackage == true)
                            //{
                            //    if (CheckGenderForPackage == true && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString != "Both")
                            //    {
                            //        _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                            //    }
                            //    else if (CheckGenderForPackage == false && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString == "Both")
                            //    {
                            //        _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                            //    }
                            //    else
                            //    {
                            //        IsValid = false;
                            //        string strMsg = "Services is only for : " + ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString;

                            //        MessageBoxControl.MessageBoxChildWindow msgW2 =
                            //                   new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            //        msgW2.Show();
                            //    }
                            //}
                        }

                    }
                    else
                    {
                        if (AndFlag == true)
                        {
                            strMsg = "Services already Selected with AND Condition, can't select AND and Or conditions together !";
                        }
                        else if (OrFlag == true)
                        {
                            strMsg = "Services already Selected with OR Condition, can't select AND and Or conditions together !";
                        }
                        //else if (UsedMainFlag == false && UsedAndFlag == true)
                        //{
                        //    strMsg = "You have already used the Service from AND Condition , can't select services from OR Condition !";
                        //}
                        //else if (UsedMainFlag == true && UsedOrFlag == true)
                        //{
                        //    strMsg = "You have already used the Main Services: " + lblMainServicename.Text.Trim() + " , can't select services from OR Condition !";
                        //}
                        else if (UsedMainFlag == false && UsedOrFlagSec == true)
                        {
                            strMsg = "You have already used the Service: " + SecORService.Trim() + " from OR Condition , can't select another services from OR Condition !";
                        }
                        else if (UsedMainFlag == false && UsedAndFlagSec == true)
                        {
                            strMsg = "You have already used the Service: " + SecORService.Trim() + " from OR Condition , can't select another services from AND Condition !";
                        }

                        //((clsServiceMasterVO)dgServiceList.SelectedItem).IsSet = false;

                        long ServiceID = ((clsServiceMasterVO)dgServiceList.SelectedItem).ID;  //ServiceID;
                        //this._OtherServiceSelected.Remove((clsServiceMasterVO)dgSelectedServiceList.SelectedItem);
                        foreach (var Service in DataList.Where(x => x.ID == ServiceID))
                        {
                            Service.IsSet = false;
                        }

                        pcv = new PagedCollectionView(DataList);
                        pcv.GroupDescriptions.Add(new PropertyGroupDescription("ConditionType"));

                        dgServiceList.ItemsSource = null;
                        dgServiceList.ItemsSource = pcv;  //DataList;

                        dgServiceList.UpdateLayout();
                        dgServiceList.Focus();

                        dgDataPager.Source = null;
                        dgDataPager.PageSize = 100;  //BizAction.MaximumRows;
                        dgDataPager.Source = pcv;  //DataList;

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                }
                else
                    _OtherServiceSelected.Remove((clsServiceMasterVO)dgServiceList.SelectedItem);


                if (IsValid == true)
                {

                    //foreach (var item in _OtherServiceSelected)
                    //{
                    //    item.SelectService = true;

                    //}

                    //dgSelectedServiceList.ItemsSource = null;
                    //dgSelectedServiceList.ItemsSource = _OtherServiceSelected;
                    //dgSelectedServiceList.UpdateLayout();
                    //dgSelectedServiceList.Focus();
                }

            }


        }

        # region Condition ANDOR OLD Logic

        //bool IsValid = true;
        //    if (dgServiceList.SelectedItem != null)
        //    {
        //        if (_OtherServiceSelected == null)
        //            _OtherServiceSelected = new ObservableCollection<clsServiceMasterVO>();

        //        CheckBox chk = (CheckBox)sender;
        //        StringBuilder strError = new StringBuilder();
        //        bool CheckGenderForPackage = false;
        //        bool IsTariffPackage = false;

        //        //// Used to check whether it is Package. If it is then Code = "Package" (Code set using ApplicableType field in SP - "CIMS_GetPatientTariffDetails" and table - "M_TariffMaster")
        //        //if (((MasterListItem)cmbTariff.SelectedItem).Code == "Package")
        //        //{
        //        //    IsTariffPackage = true;

        //        //    //If it is Package then check whether selected service Gender = selected patient gender
        //        //    if (((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableTo == ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString != "Both")
        //        //    {
        //        //        CheckGenderForPackage = true;
        //        //    }
        //        //}

        //        string strMsg = "";

        //        bool AndFlag = false;
        //        bool OrFlag = false;

        //        if (((clsServiceMasterVO)dgServiceList.SelectedItem).ConditionType == "AND")
        //        {
        //            var item1 = from r in _OtherServiceSelected
        //                        where r.ConditionType == "OR"
        //                        select new clsServiceMasterVO
        //                        {
        //                            Status = r.Status,
        //                            ID = r.ID,
        //                            UnitID = r.UnitID,
        //                            //ServiceID = r.ServiceID,
        //                            ServiceName = r.ServiceName
        //                        };

        //            if (item1.ToList().Count > 0)
        //            {
        //                OrFlag = true;
        //            }
        //        }

        //        if (((clsServiceMasterVO)dgServiceList.SelectedItem).ConditionType == "OR")
        //        {
        //            var item2 = from r in _OtherServiceSelected
        //                        where r.ConditionType == "AND"
        //                        select new clsServiceMasterVO
        //                        {
        //                            Status = r.Status,
        //                            ID = r.ID,
        //                            UnitID = r.UnitID,
        //                            //ServiceID = r.ServiceID,
        //                            ServiceName = r.ServiceName
        //                        };

        //            if (item2.ToList().Count > 0)
        //            {
        //                AndFlag = true;
        //            }
        //        }

        //        bool UsedMainFlag = false;
        //        bool UsedAndFlag = false;
        //        bool UsedOrFlag = false;
        //        bool UsedOrFlagSec = false;
        //        bool UsedAndFlagSec = false;

        //        long UsedMainCnt = 0;
        //        long UsedAndCnt = 0;
        //        long UsedOrCnt = 0;

        //        string SecORService = "";

        //        if (txtServiceUsedQty.Text.Trim() != String.Empty && txtServiceUsedQty.Text.IsValueDouble())
        //            UsedMainCnt = Convert.ToInt64(txtServiceUsedQty.Text.Trim());

        //        if (UsedMainCnt > 0)
        //            UsedMainFlag = true;

        //        if (UsedMainFlag == true && ((clsServiceMasterVO)dgServiceList.SelectedItem).ConditionType == "OR")
        //        {
        //            UsedOrFlag = true;
        //        }

        //        if (UsedMainFlag == true && ((clsServiceMasterVO)dgServiceList.SelectedItem).ConditionType == "AND")
        //        {
        //            var item3 = from r in DataList
        //                        where r.ConditionType == "OR" && r.ConditionalUsedQuantity > 0 && r.IsSet == true
        //                        && r.ConditionServiceID != ((clsServiceMasterVO)dgServiceList.SelectedItem).ConditionServiceID
        //                        select new clsServiceMasterVO
        //                        {
        //                            Status = r.Status,
        //                            ID = r.ID,
        //                            UnitID = r.UnitID,
        //                            //ServiceID = r.ServiceID,
        //                            ServiceName = r.ServiceName
        //                        };

        //            if (item3.ToList().Count > 0)
        //            {
        //                UsedOrFlag = true;
        //            }
        //        }

        //        if (UsedMainFlag == false && ((clsServiceMasterVO)dgServiceList.SelectedItem).ConditionType == "AND")
        //        {
        //            var item4 = from r in DataList
        //                        where r.ConditionType == "OR" && r.ConditionalUsedQuantity > 0
        //                        && r.ConditionServiceID != ((clsServiceMasterVO)dgServiceList.SelectedItem).ConditionServiceID
        //                        select new clsServiceMasterVO
        //                        {
        //                            Status = r.Status,
        //                            ID = r.ID,
        //                            UnitID = r.UnitID,
        //                            //ServiceID = r.ServiceID,
        //                            ServiceName = r.ServiceName
        //                        };

        //            if (item4.ToList().Count > 0)
        //            {
        //                SecORService = (item4.ToList()).FirstOrDefault().ServiceName;
        //                UsedAndFlagSec = true;
        //            }
        //        }

        //        if (UsedMainFlag == false && ((clsServiceMasterVO)dgServiceList.SelectedItem).ConditionType == "OR")
        //        {
        //            var item5 = from r in DataList
        //                        where r.ConditionType == "AND" && r.ConditionalUsedQuantity > 0 && r.IsSet == true
        //                        && r.ConditionServiceID != ((clsServiceMasterVO)dgServiceList.SelectedItem).ConditionServiceID
        //                        select new clsServiceMasterVO
        //                        {
        //                            Status = r.Status,
        //                            ID = r.ID,
        //                            UnitID = r.UnitID,
        //                            //ServiceID = r.ServiceID,
        //                            ServiceName = r.ServiceName
        //                        };

        //            if (item5.ToList().Count > 0)
        //            {
        //                UsedAndFlag = true;
        //            }
        //        }

        //        if (UsedMainFlag == false && ((clsServiceMasterVO)dgServiceList.SelectedItem).ConditionType == "OR")
        //        {
        //            var item6 = from r in DataList
        //                        where r.ConditionType == "OR" && r.ConditionalUsedQuantity > 0
        //                        && r.ConditionServiceID != ((clsServiceMasterVO)dgServiceList.SelectedItem).ConditionServiceID
        //                        select new clsServiceMasterVO
        //                        {
        //                            Status = r.Status,
        //                            ID = r.ID,
        //                            UnitID = r.UnitID,
        //                            //ServiceID = r.ServiceID,
        //                            ServiceName = r.ServiceName
        //                        };

        //            if (item6.ToList().Count > 0)
        //            {
        //                SecORService = (item6.ToList()).FirstOrDefault().ServiceName;
        //                UsedOrFlagSec = true;
        //            }
        //        }

        //        if (chk.IsChecked == true)
        //        {
        //            if (AndFlag == false && OrFlag == false & UsedAndFlag == false && UsedOrFlag == false && UsedOrFlagSec == false && UsedAndFlagSec == false)
        //            {
        //                if (_OtherServiceSelected.Count > 0)
        //                {
        //                    var item = from r in _OtherServiceSelected
        //                               where r.ID == ((clsServiceMasterVO)dgServiceList.SelectedItem).ID  //ServiceID
        //                               select new clsServiceMasterVO
        //                               {
        //                                   Status = r.Status,
        //                                   ID = r.ID,
        //                                   UnitID = r.UnitID,
        //                                   //ServiceID = r.ServiceID,
        //                                   ServiceName = r.ServiceName
        //                               };
        //                    if (item.ToList().Count > 0)
        //                    {
        //                        if (strError.ToString().Length > 0)
        //                            strError.Append(",");
        //                        strError.Append(((clsServiceMasterVO)dgServiceList.SelectedItem).ServiceName);

        //                        if (!string.IsNullOrEmpty(strError.ToString()))
        //                        {
        //                            strMsg = "Services already Selected : " + strError.ToString();

        //                            MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                                       new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //                            msgW1.Show();

        //                            //((clsServiceMasterVO)dgServiceList.SelectedItem).SelectService = false;

        //                            //dgServiceList.ItemsSource = null;
        //                            //dgServiceList.ItemsSource = DataList;
        //                            //dgServiceList.UpdateLayout();
        //                            //dgServiceList.Focus();

        //                            IsValid = false;
        //                        }
        //                    }
        //                    else
        //                    {

        //                        _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);

        //                        //if (IsTariffPackage == false)
        //                        //{
        //                        //    _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
        //                        //}
        //                        //else if (IsTariffPackage == true)
        //                        //{
        //                        //    if (CheckGenderForPackage == true && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString != "Both")
        //                        //    {
        //                        //        _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
        //                        //    }
        //                        //    else if (CheckGenderForPackage == false && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString == "Both")
        //                        //    {
        //                        //        _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
        //                        //    }
        //                        //    else
        //                        //    {
        //                        //        IsValid = false;

        //                        //        ((CheckBox)sender).IsChecked = false;

        //                        //        string strMsg = "Services is only for : " + ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString;

        //                        //        MessageBoxControl.MessageBoxChildWindow msgW2 =
        //                        //                   new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //                        //        msgW2.Show();
        //                        //    }
        //                        //}

        //                    }


        //                }
        //                else
        //                {

        //                    _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);

        //                    //if (IsTariffPackage == false)
        //                    //{
        //                    //    _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
        //                    //}
        //                    //else if (IsTariffPackage == true)
        //                    //{
        //                    //    if (CheckGenderForPackage == true && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString != "Both")
        //                    //    {
        //                    //        _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
        //                    //    }
        //                    //    else if (CheckGenderForPackage == false && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString == "Both")
        //                    //    {
        //                    //        _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
        //                    //    }
        //                    //    else
        //                    //    {
        //                    //        IsValid = false;
        //                    //        string strMsg = "Services is only for : " + ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString;

        //                    //        MessageBoxControl.MessageBoxChildWindow msgW2 =
        //                    //                   new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //                    //        msgW2.Show();
        //                    //    }
        //                    //}
        //                }

        //            }
        //            else
        //            {
        //                if (AndFlag == true)
        //                {
        //                    strMsg = "Services already Selected with AND Condition, can't select AND and Or conditions together !";
        //                }
        //                else if (OrFlag == true)
        //                {
        //                    strMsg = "Services already Selected with OR Condition, can't select AND and Or conditions together !";
        //                }
        //                else if (UsedMainFlag == false && UsedAndFlag == true)
        //                {
        //                    strMsg = "You have already used the Service from AND Condition , can't select services from OR Condition !";
        //                }
        //                else if (UsedMainFlag == true && UsedOrFlag == true)
        //                {
        //                    strMsg = "You have already used the Main Services: " + lblMainServicename.Text.Trim() + " , can't select services from OR Condition !";
        //                }
        //                else if (UsedMainFlag == false && UsedOrFlagSec == true)
        //                {
        //                    strMsg = "You have already used the Service: " + SecORService.Trim() + " from OR Condition , can't select another services from OR Condition !";
        //                }
        //                else if (UsedMainFlag == false && UsedAndFlagSec == true)
        //                {
        //                    strMsg = "You have already used the Service: " + SecORService.Trim() + " from OR Condition , can't select another services from AND Condition !";
        //                }

        //                //((clsServiceMasterVO)dgServiceList.SelectedItem).IsSet = false;

        //                long ServiceID = ((clsServiceMasterVO)dgServiceList.SelectedItem).ID;  //ServiceID;
        //                //this._OtherServiceSelected.Remove((clsServiceMasterVO)dgSelectedServiceList.SelectedItem);
        //                foreach (var Service in DataList.Where(x => x.ID == ServiceID))
        //                {
        //                    Service.IsSet = false;
        //                }

        //                pcv = new PagedCollectionView(DataList);
        //                pcv.GroupDescriptions.Add(new PropertyGroupDescription("ConditionType"));

        //                dgServiceList.ItemsSource = null;
        //                dgServiceList.ItemsSource = pcv;  //DataList;

        //                dgServiceList.UpdateLayout();
        //                dgServiceList.Focus();

        //                dgDataPager.Source = null;
        //                dgDataPager.PageSize = 100;  //BizAction.MaximumRows;
        //                dgDataPager.Source = pcv;  //DataList;

        //                MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //                msgW1.Show();
        //            }
        //        }
        //        else
        //            _OtherServiceSelected.Remove((clsServiceMasterVO)dgServiceList.SelectedItem);


        //        if (IsValid == true)
        //        {

        //            //foreach (var item in _OtherServiceSelected)
        //            //{
        //            //    item.SelectService = true;

        //            //}

        //            //dgSelectedServiceList.ItemsSource = null;
        //            //dgSelectedServiceList.ItemsSource = _OtherServiceSelected;
        //            //dgSelectedServiceList.UpdateLayout();
        //            //dgSelectedServiceList.Focus();
        //        }

        //    }

        # endregion
    }
}

