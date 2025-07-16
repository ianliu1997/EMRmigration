using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using System.Windows.Browser;
using CIMS;
using PalashDynamics.Animations;
using System.Collections.ObjectModel;
using PalashDynamics.Collections;
using MessageBoxControl;
using System.Windows.Resources;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using PalashDynamics.Pharmacy.ItemSearch;
using OPDModule.Forms;
using System.Text;

namespace PalashDynamics.Pharmacy.Inventory
{
    public partial class frmOpeningBalance : UserControl
    {
        public frmOpeningBalance()
        {
            InitializeComponent();

            this.DataContext = new clsOpeningBalVO();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            dgAddOpeningBalanceItems.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgAddOpeningBalanceItems_CellEditEnded);


            //Paging
            DataList = new PagedSortableCollectionView<clsOpeningBalVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            //======================================================


        }

        #region Variable Declarartion
        public Boolean isPageLoaded = false;
        private SwivelAnimation objAnimation;
        public List<clsItemMasterVO> objItemList;
        public Boolean lessMrp = false;
        public Boolean zeroQuantity = false;
        public Boolean BatchReqNotFilled = false;
        public Boolean ExpiryReqNotFilled = false;

        public ObservableCollection<clsOpeningBalVO> OeningBalanceAddedItems { get; set; }
        public PagedSortableCollectionView<clsOpeningBalVO> DataList { get; private set; }
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        List<MasterListItem> PUMLIst = new List<MasterListItem>();
        int ClickedFlag1 = 0;
        WaitIndicator Indicatior;

        #endregion

        #region 'Paging'



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
        WaitIndicator indicator = new WaitIndicator();

        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {

            FillOpningBalanceDataGrid();
        }



        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // cmdCancel.IsEnabled = false;
            try
            {
                if (!isPageLoaded)
                {
                    SetCommandButtonState("New");
                    objAnimation.Invoke(RotationType.Backward);
                    OeningBalanceAddedItems = new ObservableCollection<clsOpeningBalVO>();
                    dgAddOpeningBalanceItems.ItemsSource = OeningBalanceAddedItems;
                    FillClinic();
                    FillPUM();
                    FillItemGroup();
                    dtpFromDate.SelectedDate = DateTime.Now.Date;
                    dtpToDate.SelectedDate = DateTime.Now.Date;
                    isPageLoaded = true;
                }
            }
            catch (Exception)
            {

            }

        }

        private void FillClinic()
        {

            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
                BizAction.MasterList = new List<MasterListItem>();

                //if (pClinicID > 0)
                //{
                //    BizAction.Parent = new KeyValue { Value = "ClinicID", Key = pClinicID.ToString() };
                //}

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                        //cmbBloodGroup.ItemsSource = null;
                        //cmbBloodGroup.ItemsSource = objList;
                        cmbClinic.ItemsSource = null;
                        cmbClinic.ItemsSource = objList;

                        cmbBackClinic.ItemsSource = null;
                        cmbBackClinic.ItemsSource = objList;

                        #region Added by Pallavi

                        //if unit is not head office
                        if (!((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                        {
                            //for selecting unitid according to user login unit id
                            var res = from r in objList
                                      where r.ID == User.UserLoginInfo.UnitId
                                      select r;

                            cmbClinic.IsEnabled = false;
                            cmbBackClinic.IsEnabled = false;
                        }
                        //else
                        //{
                        if (objList.Count > 1)
                        {
                            // cmbClinic.SelectedItem = objList[1];
                            foreach (var item in objList)
                            {
                                if (item.ID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                                {
                                    cmbClinic.SelectedItem = item;
                                    cmbBackClinic.SelectedItem = item;
                                    cmbBackClinic.UpdateLayout();
                                    cmbClinic.UpdateLayout();//  ((MasterListItem)res.First());
                                    break;
                                }
                            }

                        }

                        //    else
                        //        cmbClinic.SelectedItem = objList[0];
                        //}



                        #endregion



                        //if (objList.Count > 1)
                        //    cmbClinic.SelectedItem = objList[1];
                        //     
                        //else
                        //    cmbClinic.SelectedItem = objList[0];

                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        private void FillPUM()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_UnitOfMeasure;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        PUMLIst = new List<MasterListItem>();
                        PUMLIst.Add(new MasterListItem(0, "- Select -"));
                        PUMLIst.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        private void FillItemGroup()
        {

            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_ItemGroup;
                BizAction.MasterList = new List<MasterListItem>();

                //if (pClinicID > 0)
                //{
                //    BizAction.Parent = new KeyValue { Value = "ClinicID", Key = pClinicID.ToString() };
                //}

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        cmbItemGroup.ItemsSource = null;
                        cmbItemGroup.ItemsSource = objList;
                        cmbItemGroup.SelectedItem = objList[0];

                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        private void FillItemList()
        {

            try
            {
                objItemList = new List<clsItemMasterVO>();

                clsGetItemListBizActionVO BizActionObj = new clsGetItemListBizActionVO();
                //False when we want to fetch all items
                //clsItemMasterVO obj = new clsItemMasterVO();
                //obj.RetrieveDataFlag = false;
                BizActionObj.ItemDetails = new clsItemMasterVO();
                BizActionObj.ItemDetails.RetrieveDataFlag = false;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        //acbItemName.ItemsSource = null;
                        objItemList = ((clsGetItemListBizActionVO)args.Result).ItemList;


                    }

                };

                client.ProcessAsync(BizActionObj, new clsUserVO());
                client.CloseAsync();




            }
            catch (Exception)
            {

                throw;
            };


        }

        private void FillUnit()
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

                        //cboClinicName.ItemsSource = null;
                        //cboClinicName.ItemsSource = objList;

                    }

                    if (this.DataContext != null)
                    {
                        //cboClinicName.SelectedValue = ((clsItemMasterVO)this.DataContext).ClinicID;
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

        // Method To Fill Unit Of Mesurements with Conversion Factors for Selected Item
        private void FillUOMConversions(AutoCompleteBox cmbConversions)
        {
            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();

                clsGetItemConversionFactorListBizActionVO BizAction = new clsGetItemConversionFactorListBizActionVO();

                BizAction.ItemID = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ItemID;
                BizAction.UOMConversionList = new List<clsConversionsVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        Indicatior.Close();

                        List<MasterListItem> UOMConvertLIst = new List<MasterListItem>();
                        MasterListItem objConversion = new MasterListItem();
                        objConversion.ID = 0;
                        objConversion.Description = "- Select -";
                        UOMConvertLIst.Add(objConversion);

                        if (((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList != null)
                            UOMConvertLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList);
                        cmbConversions.ItemsSource = UOMConvertLIst.DeepCopy();

                        if (UOMConvertLIst != null)
                            cmbConversions.SelectedItem = UOMConvertLIst[0];

                        //List<clsConversionsVO> ConvertLst = new List<clsConversionsVO>();
                        //ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList());
                        //ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.ToUOM).Select(y => y.First()).ToList());

                        //List<clsConversionsVO> MainConvertLst = new List<clsConversionsVO>();
                        //MainConvertLst = UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList().DeepCopy();

                        //ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList());



                        List<clsConversionsVO> UOMConversionLIst = new List<clsConversionsVO>();
                        UOMConversionLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConversionList);

                        if (dgAddOpeningBalanceItems.SelectedItem != null)
                        {
                            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).UOMConversionList = UOMConversionLIst.DeepCopy();
                            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).UOMList = UOMConvertLIst.DeepCopy();
                        }


                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                Indicatior.Close();
                throw;
            }


        }

        #region Set Command Button State New/Save/Modify/Print
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    cmdNew.IsEnabled = true;
                    cmdPrint.IsEnabled = true;
                    chkPrintReport.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    //    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;

                case "Save":

                    cmdNew.IsEnabled = true;
                    cmdPrint.IsEnabled = true;
                    chkPrintReport.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    IsCancel = true;
                    //cmdCancel.IsEnabled = false;
                    break;

                case "Modify":

                    cmdNew.IsEnabled = false;
                    cmdPrint.IsEnabled = false;
                    chkPrintReport.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    IsCancel = true;
                    cmdCancel.IsEnabled = true;
                    break;

                case "ClickNew":
                    cmdNew.IsEnabled = false;
                    cmdPrint.IsEnabled = false;
                    chkPrintReport.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    cmdPrint.IsEnabled = true;
                    chkPrintReport.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }

        #endregion

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            objItemList = null;
        }


        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            bool isSave = false;
            ClickedFlag1 += 1;
            bool isValid = true;
            if (ClickedFlag1 == 1)
            {
                isValid = checkValidationForMRPAndPurchaseRate();

                if (isValid)
                {
                    List<clsCheckDuplicasyVO> tmpobj = new List<clsCheckDuplicasyVO>();
                    foreach (var item in OeningBalanceAddedItems.ToList<clsOpeningBalVO>())
                    {
                        clsCheckDuplicasyVO tmpobjVO = new clsCheckDuplicasyVO();
                        tmpobjVO.ItemID = item.ItemID;
                        if (string.IsNullOrEmpty(item.BatchCode))
                            tmpobjVO.BatchCode = string.Empty; //.Trim()
                        else
                            tmpobjVO.BatchCode = Convert.ToString(item.BatchCode.Trim());

                        tmpobjVO.ExpiryDate = item.ExpiryDate;
                        tmpobjVO.CostPrice = item.Rate / item.BaseConversionFactor;
                        tmpobjVO.MRP = item.MRP / item.BaseConversionFactor;
                        tmpobjVO.IsBatchRequired = item.IsBatchRequired;
                        tmpobjVO.ItemName = item.ItemName;
                        tmpobjVO.StoreID = item.StoreID;
                        tmpobjVO.TransactionTypeID = InventoryTransactionType.OpeningBalance;
                        tmpobjVO.IsFromOpeningBalance = true;

                        tmpobj.Add(tmpobjVO);



                        //List<clsCheckDuplicasyVO> tmpobj = new List<clsCheckDuplicasyVO>();
                        //foreach (var item in OeningBalanceAddedItems)
                        //{
                        //    tmpobj.Add(new clsCheckDuplicasyVO
                        //    {
                        //        ItemID = item.ItemID,
                        //        BatchCode = Convert.ToString(item.BatchCode.Trim()),
                        //        ExpiryDate = item.ExpiryDate,
                        //        //CostPrice=item.MainRate,
                        //        //MRP=item.MainMRP,
                        //        CostPrice = item.Rate / item.BaseConversionFactor,
                        //        MRP = item.MRP / item.BaseConversionFactor,
                        //        IsBatchRequired = item.IsBatchRequired,
                        //        ItemName = item.ItemName,
                        //        StoreID = item.StoreID,
                        //        TransactionTypeID = InventoryTransactionType.OpeningBalance,//1//item.TransactionTypeID
                        //        IsFromOpeningBalance = true
                        //    });
                    }

                    clsCheckDuplicasyBizActionVO BizAction = new clsCheckDuplicasyBizActionVO();
                    BizAction.lstDuplicasy = tmpobj;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        string ItemName = "";
                        string BatchCode = "";

                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsCheckDuplicasyBizActionVO)args.Result).SuccessStatus == 0)
                            {
                                isSave = true;
                            }
                        }
                        if (isSave)
                        {
                            //By Anjali..............................
                            StringBuilder strError = new StringBuilder();


                            if (OeningBalanceAddedItems != null && OeningBalanceAddedItems.ToList<clsOpeningBalVO>().Count > 0)
                            {
                                var item1 = from r in OeningBalanceAddedItems.ToList<clsOpeningBalVO>()
                                            where (r.IsBatchRequired == true && r.ItemExpiredInDays == 0)
                                            select r;

                                if (item1 != null && item1.ToList().Count > 0)
                                {
                                    foreach (clsOpeningBalVO t in item1.ToList())
                                    {
                                        if (strError.ToString().Length > 0)
                                            strError.Append(",");
                                        strError.Append(t.ItemName);

                                    }
                                }
                            }

                            //...............................................
                            string msgTitle = "";
                            string msgText = "";
                            if (strError != null && Convert.ToString(strError) != string.Empty)
                            {
                                msgText = "Expired In Days Are not Defined In Master For Item " + strError + ",\n Are You Sure You Want To Save Opening Balance Details?";
                            }
                            else
                            {
                                msgText = "Are you sure you want to save the Opening Balance Details?";
                            }

                            MessageBoxControl.MessageBoxChildWindow msgW =
                                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                            msgW.OnMessageBoxClosed += (res) =>
                            {
                                if (res == MessageBoxResult.Yes)
                                {
                                    SaveOpeningBalance();
                                }
                                else
                                    ClickedFlag1 = 0;
                            };

                            msgW.Show();
                        }
                        else
                        {
                            ItemName = ((clsCheckDuplicasyBizActionVO)args.Result).ItemName;
                            BatchCode = ((clsCheckDuplicasyBizActionVO)args.Result).BatchCode;
                            string msgtext = "";
                            if (((clsCheckDuplicasyBizActionVO)args.Result).IsBatchRequired == true)
                            {
                                msgtext = "For item " + "'" + ItemName + "'" + " with Batch " + "'" + BatchCode + "'" + " opening balance is already done.";
                            }
                            else
                            {
                                msgtext = "For item " + "'" + ItemName + "'" + " opening balance is already done for same details.";
                            }

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", msgtext, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);  //Batch already exists
                            ClickedFlag1 = 0;
                            msgW1.Show();
                        }
                    };
                    client.ProcessAsync(BizAction, new clsUserVO());
                    client.CloseAsync();
                }
                else
                    ClickedFlag1 = 0;
            }
        }

        private void SaveOpeningBalance()
        {
            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                clsAddOpeningBalanceBizActionVO objBizActionVO = new clsAddOpeningBalanceBizActionVO();
                objBizActionVO.OpeningBalanceList = new List<clsOpeningBalVO>();
                objBizActionVO.OpeningBalVO = new clsOpeningBalVO();
                objBizActionVO.OpeningBalanceList = OeningBalanceAddedItems.ToList<clsOpeningBalVO>();
                objBizActionVO.OpeningBalVO.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                objBizActionVO.OpeningBalVO.AddedDateTime = DateTime.Now;
                objBizActionVO.OpeningBalVO.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                objBizActionVO.OpeningBalVO.Remarks = txtremarks.Text;

                if (cmbBackClinic.SelectedItem != null)
                    objBizActionVO.OpeningBalVO.UnitId = ((MasterListItem)cmbBackClinic.SelectedItem).ID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    ClickedFlag1 = 0;
                    if (arg.Error == null && ((clsAddOpeningBalanceBizActionVO)arg.Result).OpeningBalanceList != null)
                    {
                        if (arg.Result != null)
                        {
                            if (((clsAddOpeningBalanceBizActionVO)arg.Result).SuccessStatus == 2)
                            {
                                Indicatior.Close();
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Batch already exists, updated successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);  //Batch already exists
                                SetCommandButtonState("Save");
                                msgW1.Show();

                                objAnimation.Invoke(RotationType.Backward);
                                FillOpningBalanceDataGrid();
                            }
                            else
                                if (((clsAddOpeningBalanceBizActionVO)arg.Result).OpeningBalanceList != null)
                                {

                                    Indicatior.Close();
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Opening Balance", "Opening Balance details saved successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    SetCommandButtonState("Save");
                                    msgW1.Show();

                                    objAnimation.Invoke(RotationType.Backward);
                                    FillOpningBalanceDataGrid();
                                }
                        }
                    }
                    else
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Opening Balance details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }

                };

                Client.ProcessAsync(objBizActionVO, User);
                Client.CloseAsync();


            }
            catch (Exception)
            {
                ClickedFlag1 = 0;
                throw;
            }
        }

        private bool checkValidationForMRPAndPurchaseRate()
        {
            try
            {


                bool isValid = true;
                BatchReqNotFilled = false;
                ExpiryReqNotFilled = false;
                List<clsOpeningBalVO> objList = OeningBalanceAddedItems.ToList<clsOpeningBalVO>();
                if (objList != null && objList.Count > 0)
                {
                    foreach (var item in objList)
                    {
                        if (item.IsBatchRequired == true)
                        {
                            if (string.IsNullOrWhiteSpace(item.BatchCode) || item.BatchCode == "" || item.BatchCode == null)
                            {
                                BatchReqNotFilled = true;
                                isValid = false;

                                MessageBoxControl.MessageBoxChildWindow msg =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Batch Code is Required for " + item.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                msg.Show();
                                break;
                            }
                            else if (item.ExpiryDate == null)
                            {
                                ExpiryReqNotFilled = true;
                                isValid = false;

                                MessageBoxControl.MessageBoxChildWindow msgW3 =
                              new MessageBoxControl.MessageBoxChildWindow("Expiry Date Required!.", "Please Enter Expiry Date!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW3.Show();
                                break;
                            }
                            //By Anjali.........................
                            else if (item.IsBatchRequired && item.ExpiryDate.HasValue && item.ItemExpiredInDays > 0 && (int)(item.ExpiryDate.Value - DateTime.Now).TotalDays < item.ItemExpiredInDays)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW3 =
                              new MessageBoxControl.MessageBoxChildWindow("Item has Expiry date less than" + item.ItemExpiredInDays + "days !", "Please Enter Proper Expiry Date!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW3.Show();
                                isValid = false;
                                break;
                            }
                            //............................
                            else
                            {
                                if (isValid)
                                {
                                    try
                                    {
                                        var chkitem = from r in OeningBalanceAddedItems
                                                      where r.BatchCode == item.BatchCode //&& r.ExpiryDate == item.ExpiryDate
                                                      && r.ItemID == item.ItemID

                                                      select new clsOpeningBalVO
                                                      {
                                                          Status = r.Status,
                                                          ID = r.ID
                                                      };

                                        if (chkitem.ToList().Count > 1)
                                        {
                                            isValid = false;
                                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                    new MessageBoxControl.MessageBoxChildWindow("", "Same Batch appears more than once for " + item.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                            msgW1.Show();
                                            break;
                                        }

                                        var chkitem1 = from r in OeningBalanceAddedItems
                                                       where r.ItemID == item.ItemID && item.IsBatchRequired == false
                                                       select new clsOpeningBalVO
                                                       {
                                                           Status = r.Status,
                                                           ID = r.ID
                                                       };
                                        if (chkitem1.ToList().Count > 1)
                                        {
                                            isValid = false;
                                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                    new MessageBoxControl.MessageBoxChildWindow("", "Same Item appears more than once for NonBatchWise item " + item.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                            msgW1.Show();
                                            break;
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        isValid = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            var chkitem1 = from r in OeningBalanceAddedItems
                                           where r.ItemID == item.ItemID && item.IsBatchRequired == false && (r.Rate / r.BaseConversionFactor) == (item.Rate / item.BaseConversionFactor)
                                                 && (r.MRP / r.BaseConversionFactor) == (item.MRP / item.BaseConversionFactor)
                                           select new clsOpeningBalVO
                                           {
                                               Status = r.Status,
                                               ID = r.ID
                                           };
                            if (chkitem1.ToList().Count > 1)
                            {
                                isValid = false;
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Non Batch Item " + item.ItemName + " appears more than once for same CP and MRP ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                msgW1.Show();
                                break;
                            }
                        }

                        if (item.SelectedUOM.ID == 0)
                        {
                            // BatchReqNotFilled = true;
                            isValid = false;

                            MessageBoxControl.MessageBoxChildWindow msg =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select UOM For " + item.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msg.Show();
                            break;
                        }

                        if (item.BaseConversionFactor <= 0 || item.ConversionFactor <= 0)
                        {
                            isValid = false;
                            MessageBoxControl.MessageBoxChildWindow msgW1;
                            msgW1 = new MessageBoxControl.MessageBoxChildWindow("Validation Error", "Can Not Save, Please Assign Conversion Factor For Item " + "'" + item.ItemName + "'", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            break;
                        }

                        if (item.MRP < item.Rate)
                        {
                            isValid = false;
                            lessMrp = true;
                            MessageBoxControl.MessageBoxChildWindow msgW1;
                            msgW1 = new MessageBoxControl.MessageBoxChildWindow("Validation Error", "Please Enter MRP Greater Than Cost Prize for Item " + item.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            break;
                        }
                        //Added by Pallavi
                        if (item.Quantity == 0)
                        {
                            isValid = false;
                            zeroQuantity = true;
                            MessageBoxControl.MessageBoxChildWindow msgW1;
                            msgW1 = new MessageBoxControl.MessageBoxChildWindow("Validation Error", "Please Enter Quantity Greater Than 0 for Item " + item.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            break;
                        }
                        if (item.Rate == 0)
                        {
                            isValid = false;
                            MessageBoxControl.MessageBoxChildWindow msgW1;
                            msgW1 = new MessageBoxControl.MessageBoxChildWindow("Validation Error", "Please Enter CP Greater Than Zero For Item " + item.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            break;
                        }

                        if ((item.SingleQuantity).ToString().Length > 5)
                        {
                            isValid = false;
                            zeroQuantity = true;
                            MessageBoxControl.MessageBoxChildWindow msgW1;
                            msgW1 = new MessageBoxControl.MessageBoxChildWindow("Validation Error", "Quantity Should Not Be Greater Than 5 Digits For Item " + item.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            break;
                        }

                        //if (item.Re_Order == 0)
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow mgbx = null;
                        //    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Reorder Quantity Is Not Define For This Item.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //    mgbx.Show();
                        //    //obj.SingleQuantity = 1;
                        //    // return;
                        //}

                        //else if ((item.SingleQuantity * item.BaseConversionFactor) < item.Re_Order * item.StockCF)
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow mgbx = null;
                        //    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Entered Quantity Is Less Than Reorder Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //    mgbx.Show();
                        //    //obj.SingleQuantity = 1;
                        //    // return;
                        //}

                    }
                }

                if (objList.Count <= 0)
                {
                    isValid = false;
                    MessageBoxControl.MessageBoxChildWindow msgW1;
                    msgW1 = new MessageBoxControl.MessageBoxChildWindow("Validation Error", "Can not save opening balance with zero items ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();

                }


                if (cmbBackClinic.SelectedItem == null)
                {
                    cmbBackClinic.SetValidation("Please Select Clinic");
                    cmbBackClinic.RaiseValidationError();
                    cmbBackClinic.Focus();
                    isValid = false;

                }
                else if (((MasterListItem)cmbBackClinic.SelectedItem).ID == 0)
                {
                    cmbBackClinic.SetValidation("Please Select Clinic");
                    cmbBackClinic.RaiseValidationError();
                    cmbBackClinic.Focus();
                    isValid = false;

                }
                else
                    cmbBackClinic.ClearValidationError();

                if (cmbBackStore.SelectedItem == null)
                {
                    cmbBackStore.SetValidation("Please Select Store");
                    cmbBackStore.RaiseValidationError();
                    cmbBackStore.Focus();
                    isValid = false;

                }
                else if (((clsStoreVO)cmbBackStore.SelectedItem).StoreId == 0)
                {
                    cmbBackStore.SetValidation("Please Select Store");
                    cmbBackStore.RaiseValidationError();
                    cmbBackStore.Focus();
                    isValid = false;

                }
                else
                    cmbBackStore.ClearValidationError();

                //Check

                return isValid;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void FillOpningBalanceDataGrid()
        {

            try
            {
                if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
                {
                    if (dtpFromDate.SelectedDate.Value.Date > dtpToDate.SelectedDate.Value.Date)
                    {
                        dtpFromDate.SetValidation("From Date should be less than To Date");
                        dtpFromDate.RaiseValidationError();
                        dtpFromDate.Focus();
                        return;
                    }
                    else
                    {
                        dtpFromDate.ClearValidationError();
                    }

                }

                if (dtpFromDate.SelectedDate != null && dtpFromDate.SelectedDate.Value.Date > DateTime.Now.Date)
                {
                    dtpFromDate.SetValidation("From Date should not be greater than Today's Date");
                    dtpFromDate.RaiseValidationError();
                    dtpFromDate.Focus();
                    return;
                }
                else
                {
                    dtpFromDate.ClearValidationError();
                }

                if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate == null)
                {
                    dtpToDate.SetValidation("Plase Select To Date");
                    dtpToDate.RaiseValidationError();
                    dtpToDate.Focus();
                    return;
                }
                else
                {
                    dtpToDate.ClearValidationError();
                }

                //if (this.dtpFromDate.SelectedDate != null && this.dtpToDate.SelectedDate != null)
                //{
                //    BizAction.FromDate = ((DateTime)this.dtpFromDate.SelectedDate).Date;
                //    BizAction.ToDate = ((DateTime)this.dtpToDate.SelectedDate).Date;
                //}

                if (dtpToDate.SelectedDate != null && dtpFromDate.SelectedDate == null)
                {
                    dtpFromDate.SetValidation("Plase Select From Date");
                    dtpFromDate.RaiseValidationError();
                    dtpFromDate.Focus();
                    return;
                }
                else
                {
                    dtpFromDate.ClearValidationError();
                }



                dgOpeningBalanceList.ItemsSource = null;
                indicator.Show();

                clsGetStockDetailsForOpeningBalanceBizActionVO objBizActionVO = new clsGetStockDetailsForOpeningBalanceBizActionVO();
                objBizActionVO.OpeningBalance = new clsOpeningBalVO();
                if (cmbStore.SelectedItem != null) objBizActionVO.OpeningBalance.StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
                if (cmbClinic.SelectedItem != null) objBizActionVO.OpeningBalance.UnitId = ((MasterListItem)cmbClinic.SelectedItem).ID; // ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                objBizActionVO.OpeningBalance.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                if (this.dtpFromDate.SelectedDate != null)
                {
                    objBizActionVO.OpeningBalance.FromDate = ((DateTime)this.dtpFromDate.SelectedDate).Date;
                }
                if (this.dtpToDate.SelectedDate != null)
                {
                    objBizActionVO.OpeningBalance.ToDate = ((DateTime)this.dtpToDate.SelectedDate).Date;
                }

                if (cmbItemGroup.SelectedItem != null && ((MasterListItem)cmbItemGroup.SelectedItem).ID > 0)
                {
                    objBizActionVO.OpeningBalance.ItemGroupID = ((MasterListItem)cmbItemGroup.SelectedItem).ID;
                }

                if (!string.IsNullOrEmpty(txtItemName.Text))
                {
                    objBizActionVO.OpeningBalance.ItemName = txtItemName.Text.ToString();
                }

                objBizActionVO.OpeningBalance.IsPagingEnabled = true;
                objBizActionVO.OpeningBalance.StartIndex = DataList.PageIndex * DataList.PageSize;
                objBizActionVO.OpeningBalance.MaxRows = DataList.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {


                            clsGetStockDetailsForOpeningBalanceBizActionVO objBizAction = (clsGetStockDetailsForOpeningBalanceBizActionVO)arg.Result;
                            if (objBizAction.ItemList != null)
                            {
                                DataList.TotalItemCount = objBizAction.OpeningBalance.TotalRows;
                                DataList.Clear();
                                foreach (var item in objBizAction.ItemList)
                                {
                                    DataList.Add(item);
                                }

                                dgOpeningBalanceList.ItemsSource = null;
                                dgOpeningBalanceList.ItemsSource = DataList;

                                DataPager.Source = null;
                                DataPager.PageSize = objBizActionVO.OpeningBalance.MaxRows;
                                DataPager.Source = DataList;

                            }
                            else
                                dgOpeningBalanceList.ItemsSource = null;
                        }
                    }
                    else
                    {

                    }
                    indicator.Close();
                };

                Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception ex)
            {
                indicator.Close();
                throw;
            }
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            OeningBalanceAddedItems.Clear();
            txtNetamt.Text = "";
            txtremarks.Text = "";
            StoreId = 0;
            FillClinic();
            //FillBackStores(((MasterListItem)cmbBackClinic.SelectedItem).ID);
            txttotcamt.Text = "";
            txtVatamt.Text = "";
            cmbBackClinic.IsEnabled = true;
            cmbBackStore.IsEnabled = true;
            cmdCancel.IsEnabled = true;

            SetCommandButtonState("ClickNew");
            objAnimation.Invoke(RotationType.Forward);

        }
        public string ModuleName { get; set; }
        public string Action { get; set; }
        bool IsCancel = true;
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            objAnimation.Invoke(RotationType.Backward);
            if (IsCancel == true)
            {

                ModuleName = "PalashDynamics.Administration";
                Action = "PalashDynamics.Administration.frmInventoryConfiguration";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Inventory Configuration";
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                IsCancel = true;
            }

        }

        void c_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {

                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);

                myData = asm.CreateInstance(Action) as UIElement;

                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);




            }
            catch (Exception ex)
            {
                throw;
            }



        }

        private void lnkAddItems_Click(object sender, RoutedEventArgs e)
        {

            bool Result = true;
            if (cmbBackStore.SelectedItem == null)
            {
                cmbBackStore.TextBox.SetValidation("Please Select Store");
                cmbBackStore.TextBox.RaiseValidationError();
                cmbBackStore.Focus();
                Result = false;
            }
            else if (((clsStoreVO)cmbBackStore.SelectedItem).StoreId == 0)
            {
                cmbBackStore.TextBox.SetValidation("Please Select Store");
                cmbBackStore.TextBox.RaiseValidationError();
                cmbBackStore.Focus();
                Result = false;
            }
            if (cmbBackClinic.SelectedItem == null)
            {
                cmbBackClinic.TextBox.SetValidation("Please Select Clinic");
                cmbBackClinic.TextBox.RaiseValidationError();
                cmbBackClinic.Focus();
                Result = false;
            }
            else if (((MasterListItem)cmbBackClinic.SelectedItem).ID == 0)
            {
                cmbBackClinic.TextBox.SetValidation("Please Select Clinic");
                cmbBackClinic.TextBox.RaiseValidationError();
                cmbBackClinic.Focus();
                Result = false;
            }



            if (Result == true)
            {
                cmbBackClinic.ClearValidationError();
                cmbBackStore.ClearValidationError();
                ItemList Itemswin = new ItemList();
                Itemswin.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
                if (cmbBackClinic.SelectedItem != null)
                    Itemswin.ClinicID = ((MasterListItem)cmbBackClinic.SelectedItem).ID;

                Itemswin.ShowBatches = false;
                Itemswin.IsFromOpeningBalance = true;
                Itemswin.AllowStoreSelection = true;
                if (StoreId > 0)
                {
                    Itemswin.cmbStore.IsEnabled = false;
                    Itemswin.cmbStore.SelectedValue = StoreId;
                    Itemswin.StoreID = StoreId;
                }

                Itemswin.OnSaveButton_Click += new RoutedEventHandler(Itemswin_OnSaveButton_Click);
                Itemswin.Show();
            }

        }
        long StoreId = 0;
        void Itemswin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ItemList Itemswin = (ItemList)sender;

            if (Itemswin.SelectedItems != null)
            {
                dgAddOpeningBalanceItems.SelectedIndex = -1;

                StoreId = Itemswin.StoreID;
                foreach (var item in Itemswin.SelectedItems)
                {
                    //    var item1 =
                    //           from p in OeningBalanceAddedItems
                    //           where p.ItemID == item.ID
                    //           select p;
                    //    if (((List<clsOpeningBalVO>)item1.ToList<clsOpeningBalVO>()).Count > 0)
                    //    {
                    //        //  msgText = "Item is Already Added!";
                    //        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", "Item is Already Added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //        msgWindow.Show();
                    //    }

                    //    else
                    //    {

                    ////////OeningBalanceAddedItems.Add(
                    ////////new clsOpeningBalVO()
                    ////////{
                    ////////    IsBatchRequired = item.BatchesRequired,
                    ////////    BatchCode=item.BatchCode,

                    ////////    ItemID = item.ID,
                    ////////    ItemName = item.ItemName,
                    ////////    PUOM = item.PUOM,
                    ////////    MainPUOM = item.PUOM,
                    ////////    SUOM = item.SUOM,
                    ////////    InclusiveOfTax = item.InclusiveForOB,
                    ////////    ApplicableOn = item.ApplicableOnForOB,
                    ////////    EnableInclusiveOfTax = item.InclusiveOfTax == false ? true : false,
                    ////////    StoreID = Itemswin.StoreID,
                    ////////    DiscountOnSale = item.DiscountOnSale,
                    ////////    VATPercent = (float)item.VatPer,
                    ////////    Rate = (float)item.PurchaseRate * item.PurchaseToBaseCF,
                    ////////    MainRate = (float)item.PurchaseRate,
                    ////////    MRP = (float)item.MRP * item.PurchaseToBaseCF,
                    ////////    MainMRP = (float)item.MRP,
                    ////////    //   ConversionFactor =1, //Convert.ToSingle(item.ConversionFactor),
                    ////////    PUOMID = item.PUM,
                    ////////    SUOMID = item.SUM,
                    ////////    BaseUOMID = item.BaseUM,
                    ////////    BaseUOM = item.BaseUMString,
                    ////////    SellingUOMID = item.SellingUM,
                    ////////    SellingUOM = item.SellingUMString,
                    ////////    //SelectedUOM = new MasterListItem { ID = item.SUM, Description = item.SUOM }
                    ////////    //   SelectedUOM = new MasterListItem { ID = 0, Description = "--Select--" },
                    ////////    ////,
                    ////////    //PUOMList = PUMLIst.Where(u => u.Description.Equals(item.PUOM) || u.Description.Equals(item.SUOM)).ToList(),
                    ////////    //SelectedPUM = PUMLIst.Single(z => z.Description.Equals(item.PUOM))

                    ////////    SelectedUOM = new MasterListItem { ID = item.PUM, Description = item.PUOM },

                    ////////    ConversionFactor = item.PurchaseToBaseCF / item.StockingToBaseCF,
                    ////////    BaseConversionFactor = item.PurchaseToBaseCF,
                    ////////    ItemExpiredInDays = item.ItemExpiredInDays,
                    ////////    Re_Order = (float)(item.Re_Order),
                    ////////    StockCF = item.StockingToBaseCF

                    ////////});


                    clsOpeningBalVO obj = new clsOpeningBalVO();

                    obj.IsBatchRequired = item.BatchesRequired;
                    obj.BatchCode = item.BatchCode;
                    obj.ItemID = item.ID;
                    obj.ItemName = item.ItemName;
                    obj.PUOM = item.PUOM;
                    obj.MainPUOM = item.PUOM;
                    obj.SUOM = item.SUOM;
                    obj.InclusiveOfTax = item.InclusiveForOB;
                    obj.ApplicableOn = item.ApplicableOnForOB;
                    obj.EnableInclusiveOfTax = item.InclusiveOfTax == false ? true : false;
                    obj.StoreID = Itemswin.StoreID;
                    obj.DiscountOnSale = item.DiscountOnSale;
                    obj.VATPercent = (float)item.VatPer;
                    obj.Rate = (float)item.PurchaseRate * item.PurchaseToBaseCF;
                    obj.MainRate = (float)item.PurchaseRate;
                    obj.MRP = (float)item.MRP * item.PurchaseToBaseCF;
                    obj.MainMRP = (float)item.MRP;
                    obj.PUOMID = item.PUM;
                    obj.SUOMID = item.SUM;
                    obj.BaseUOMID = item.BaseUM;
                    obj.BaseUOM = item.BaseUMString;
                    obj.SellingUOMID = item.SellingUM;
                    obj.SellingUOM = item.SellingUMString;
                    obj.SelectedUOM = new MasterListItem { ID = item.PUM, Description = item.PUOM };
                    obj.ConversionFactor = item.PurchaseToBaseCF / item.StockingToBaseCF;
                    obj.BaseConversionFactor = item.PurchaseToBaseCF;
                    obj.ItemExpiredInDays = item.ItemExpiredInDays;
                    obj.Re_Order = (float)(item.Re_Order);
                    obj.StockCF = item.StockingToBaseCF;


                    OeningBalanceAddedItems.Add(obj);
                }

                //MasterListItem Store = new MasterListItem();
                //Store.ID = StoreId;
                //Store.Description = ((MasterListItem)Itemswin.cmbStore.SelectedItem).Description;

                clsStoreVO Store = new clsStoreVO();
                Store.StoreId = StoreId;
                //Store.StoreName = ((MasterListItem)Itemswin.cmbStore.SelectedItem).Description;

                //cmbBackStore.SelectedItem = Store;

                cmbBackClinic.IsEnabled = false;
                cmbBackStore.IsEnabled = false;

                dgAddOpeningBalanceItems.Focus();
                dgAddOpeningBalanceItems.UpdateLayout();
                CalculateOpeningBalanceSummary();

            }
        }

        private bool CheckForItemExistance(long ItemID)
        {

            List<clsOpeningBalVO> obj = new List<clsOpeningBalVO>();
            bool reasult = false;
            if (OeningBalanceAddedItems != null)
            {

                var lowNums =
                   from n in OeningBalanceAddedItems
                   where n.ItemID == ItemID
                   select n;

                foreach (var x in lowNums)
                {
                    obj.Add(x);

                }

            }
            if (obj.Count >= 1)
            {
                reasult = true;
            }
            return reasult;
        }

        void dgAddOpeningBalanceItems_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            //dgAddOpeningBalanceItems.Columns[4]
            if (dgAddOpeningBalanceItems.SelectedItem != null)
            {

                if (e.Column.DisplayIndex == 4)     //Quantity     if (e.Column.DisplayIndex == 7) 
                {
                    clsOpeningBalVO obj = new clsOpeningBalVO();
                    obj = (clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem;
                    obj.SingleQuantity = Convert.ToSingle(System.Math.Round(obj.SingleQuantity, 1));
                    if (((int)obj.SingleQuantity).ToString().Length > 5)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = null;
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity Length Should Not Be Greater Than 5 Digits.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        obj.SingleQuantity = 1;
                        return;
                    }

                    if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM != null && ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID == ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseUOMID && (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity % 1) != 0)
                    {
                        ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity = 1;
                        string msgText = "Quantity Cannot Be In Fraction.";
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();
                    }

                    if (obj.SingleQuantity == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = null;
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity Can't Be Zero. Please Enter Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        obj.SingleQuantity = 0;
                        return;
                    }
                    if (obj.SingleQuantity < 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = null;
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity Can't Be Negative. Please Enter Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        obj.SingleQuantity = 1;
                        return;
                    }

                    if (obj.Re_Order == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = null;
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Reorder Quantity Is Not Define For This Item.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbx.Show();
                        //obj.SingleQuantity = 1;
                        // return;
                    }

                    else if ((obj.SingleQuantity * obj.BaseConversionFactor) < obj.Re_Order * obj.StockCF)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = null;
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Entered Quantity Is Less Than Reorder Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbx.Show();
                        //obj.SingleQuantity = 1;
                        // return;
                    }

                    // if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).UOMConversionList == null || ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).UOMConversionList.Count == 0)
                    // {

                    //     ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Quantity = Convert.ToSingle(((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity) * ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor;
                    // }

                    if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM != null && ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID > 0)
                    {
                        ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Quantity = Convert.ToSingle(Convert.ToSingle(((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity) * ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor);
                        ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseQuantity = Convert.ToSingle(Convert.ToSingle(((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity) * ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseConversionFactor);
                        //   ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Amount = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate * ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseQuantity;
                        //   ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).VATAmount = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Amount * ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).VATPercent)/100;


                    }
                    else
                    {
                        CalculateConversionFactorCentral(((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID, ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SUOMID);

                        //((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Quantity = 0;
                        ////     ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity = 0;

                        //((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor = 0;
                        //((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseConversionFactor = 0;

                        //((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP;
                        //((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate;
                    }

                    //if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).UOMConversionList != null && ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).UOMConversionList.Count > 0)
                    //    CalculateConversionFactorCentral(((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID, ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SUOMID);
                    //else
                    //    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Quantity = Convert.ToSingle(Convert.ToSingle(((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity) * ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor);
                    //if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedPUM.Description == ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).PUOM)
                    //{
                    //    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Quantity = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity * ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor);


                    //}
                    //else
                    //{
                    //    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Quantity = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity);

                    //    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP / ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor);
                    //    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate / ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor);

                    //}
                    CalculateOpeningBalanceSummary();

                }

                if (e.Column.DisplayIndex == 9)  //CP
                {
                    clsOpeningBalVO obj = new clsOpeningBalVO();
                    obj = (clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem;
                    if (obj.Rate < 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = null;
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "CP Can't Be Negative. Please Enter CP Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        //  obj.MRP = 0;
                        obj.Rate = 0;
                        return;
                    }

                }

                if (e.Column.DisplayIndex == 10)  //MRP
                {
                    clsOpeningBalVO obj = new clsOpeningBalVO();
                    obj = (clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem;
                    if (obj.MRP < obj.Rate)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = null;
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "MRP should be greater than CP", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        //   ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate + 1;
                        return;
                    }
                    if (obj.MRP < 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = null;
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "MRP Can't Be Negative. Please Enter MRP Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        obj.MRP = 0;
                        return;
                    }

                }

                if (e.Column.DisplayIndex == 2) // Batch  //  if (e.Column.DisplayIndex == 2 || e.Column.DisplayIndex == 3)  Batch || Expiry
                {
                    clsOpeningBalVO obj = new clsOpeningBalVO();
                    obj = (clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem;

                    if (obj != null)
                    {
                        foreach (var item in OeningBalanceAddedItems)
                        {
                            if (item.ItemID == obj.ItemID)
                            {
                                if (item.IsBatchRequired == true)
                                {

                                    if (obj.BatchCode == "" || obj.BatchCode == null)
                                    {
                                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Batch.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        msgW3.Show();
                                        return;
                                    }
                                    //      if (obj.ExpiryDate == null)
                                    //      {
                                    //          MessageBoxControl.MessageBoxChildWindow msgW3 =
                                    //new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Expiry Date!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    //          msgW3.Show();
                                    //          return;
                                    //      }
                                    //      else
                                    //          if (obj.ExpiryDate < DateTime.Now.Date)
                                    //          {
                                    //              MessageBoxControl.MessageBoxChildWindow msgW3 =
                                    //                                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Expiry date should not be less than current date!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    //              msgW3.Show();
                                    //              obj.ExpiryDate = DateTime.Now.Date;
                                    //              return;
                                    //          }
                                }
                            }
                        }
                    }
                }

                if (e.Column.DisplayIndex == 3)   //  Expiry
                {
                    clsOpeningBalVO obj = new clsOpeningBalVO();
                    obj = (clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem;

                    if (obj != null)
                    {
                        foreach (var item in OeningBalanceAddedItems)
                        {
                            if (item.ItemID == obj.ItemID)
                            {
                                if (item.IsBatchRequired == true)
                                {

                                    //      if (obj.BatchCode == "" || obj.BatchCode == null)
                                    //      {
                                    //          MessageBoxControl.MessageBoxChildWindow msgW3 =
                                    //new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Batch!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    //          msgW3.Show();
                                    //          return;
                                    //      }
                                    if (obj.ExpiryDate == null)
                                    {
                                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Expiry Date!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        msgW3.Show();
                                        return;
                                    }
                                    else
                                        if (obj.ExpiryDate < DateTime.Now.Date)
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Expiry date should not be less than current date!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                            msgW3.Show();
                                            obj.ExpiryDate = DateTime.Now.Date;
                                            return;
                                        }
                                    //    else (obj.ExpiryDate < DateTime.Now.Date)
                                    //{
                                    //    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                    //                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Expiry date should not be less than current date!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    //    msgW3.Show();
                                    //    obj.ExpiryDate = DateTime.Now.Date;
                                    //    return;
                                    //}

                                    if (obj.ExpiryDate > DateTime.Now.AddYears(3))
                                    {
                                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Expiry date is greater than three years!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgW3.Show();
                                        return;
                                    }

                                    if (item.IsBatchRequired && item.ExpiryDate.HasValue && item.ItemExpiredInDays > 0)
                                    {
                                        TimeSpan day = item.ExpiryDate.Value - DateTime.Now;
                                        int Day1 = (int)day.TotalDays;
                                        Int64 ExpiredDays = item.ItemExpiredInDays;
                                        if (Day1 < ExpiredDays)
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Item has Expiry date less than " + ExpiredDays + " days !", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                            msgW3.Show();
                                            //isValid = false;
                                            //return isValid;
                                        }
                                    }
                                    else if (item.ExpiryDate.HasValue && item.ItemExpiredInDays == 0)
                                    {

                                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Item does not have Expiry In Days At Master Level", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgW3.Show();
                                    }
                                }
                            }
                        }
                    }
                }

                if (e.Column.DisplayIndex == 12)   // Discount On Sale
                {
                    if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).DiscountOnSale > 100)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Discount on sale can't be greater than 100!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).DiscountOnSale = 0;
                        return;
                    }

                    if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).DiscountOnSale < 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Discount On Sale can't be Negetive value!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).DiscountOnSale = 0;
                        return;
                    }
                }

                if (e.Column.DisplayIndex == 13)  // VAT %
                {
                    if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).VATPercent > 100)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "VAT Percent can't be greater than 100!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).VATPercent = 0;
                        return;
                    }

                    if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).VATPercent < 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                             new MessageBoxControl.MessageBoxChildWindow("Palash", "VAT Percent can't be Negetive value!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).VATPercent = 0;
                        return;
                    }
                }
                //if()

                CalculateOpeningBalanceSummary();


            }
        }

        private void CalculateOpeningBalanceSummary()
        {
            double VATAmount, Amount, NetAmount;

            VATAmount = Amount = NetAmount = 0;

            foreach (var item in OeningBalanceAddedItems)
            {
                Amount += item.Amount;
                VATAmount += item.VATAmount;
                NetAmount += item.NetAmount;
            }


            ((clsOpeningBalVO)this.DataContext).TotalAmount = Amount;
            ((clsOpeningBalVO)this.DataContext).TotalVAT = Math.Round(VATAmount, 2);
            ((clsOpeningBalVO)this.DataContext).TotalNet = Math.Round(NetAmount, 2);
            //txtNetamt.Text = NetAmount.ToString();
            //txttotcamt.Text = Amount.ToString();
            //txtVatamt.Text = VATAmount.ToString();
        }
        private void cmbBackClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbBackClinic.SelectedItem != null && ((MasterListItem)cmbBackClinic.SelectedItem).ID > 0)
            {
                long clinicId = ((MasterListItem)cmbBackClinic.SelectedItem).ID;

                FillBackStores(clinicId);
            }



        }
        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            long clinicId = ((MasterListItem)cmbClinic.SelectedItem).ID;

            FillStores(clinicId);
        }

        private void FillStores(long clinicId)
        {
            //clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            //BizAction.MasterTable = MasterTableNameList.M_Store;
            //BizAction.MasterList = new List<MasterListItem>();
            ////BizActionObj.IsUserwiseStores = true;
            ////BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            ////BizActionObj.ToStoreList = new List<clsStoreVO>();

            //if (clinicId > 0)
            //{
            //    BizAction.Parent = new KeyValue { Value = "ClinicID", Key = clinicId.ToString() };
            //}

            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //Client.ProcessCompleted += (s, e) =>
            //{
            //    if (e.Error == null && e.Result != null)
            //    {
            //        List<MasterListItem> objList = new List<MasterListItem>();

            //        //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
            //        objList.Add(new MasterListItem(0, "- Select -"));
            //        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

            //        //cmbBloodGroup.ItemsSource = null;
            //        //cmbBloodGroup.ItemsSource = objList;
            //        cmbStore.ItemsSource = null;
            //        cmbStore.ItemsSource = objList;

            //        cmbBackStore.ItemsSource = null;
            //        cmbBackStore.ItemsSource = objList;


            //        //if (objList.Count > 1)
            //        //    cmbStore.SelectedItem = objList[1];
            //        //else
            //        cmbStore.SelectedItem = objList[0];
            //        cmbBackStore.SelectedItem = objList[0];
            //        FillOpningBalanceDataGrid();

            //        //FillOpningBalanceDataGrid();
            //    }
            //};

            //Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //Client.CloseAsync();

            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            clsItemMasterVO obj = new clsItemMasterVO();
            obj.RetrieveDataFlag = false;
            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizActionObj.ToStoreList = new List<clsStoreVO>();
            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
                    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true };
                    clsStoreVO select = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true };
                    BizActionObj.ItemMatserDetails.Insert(0, select);
                    BizActionObj.ToStoreList.Insert(0, Default);
                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == clinicId && item.Status == true//((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                                 select item;

                    var NonQSAndUserDefinedStores = from item in BizActionObj.ToStoreList.ToList()
                                                    where item.IsQuarantineStore == false
                                                    select item;

                    NonQSAndUserDefinedStores.ToList().Insert(0, select);

                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        if (result.ToList().Count > 0)
                        {
                            cmbBackStore.ItemsSource = result.ToList();
                            cmbBackStore.SelectedItem = result.ToList()[0];
                            cmbStore.ItemsSource = result.ToList();
                            cmbStore.SelectedItem = result.ToList()[0];
                        }
                    }
                    else
                    {
                        if (NonQSAndUserDefinedStores != null)
                        {

                            cmbBackStore.ItemsSource = NonQSAndUserDefinedStores.ToList();
                            cmbBackStore.SelectedItem = NonQSAndUserDefinedStores.ToList()[0];
                            cmbStore.ItemsSource = NonQSAndUserDefinedStores.ToList();
                            cmbStore.SelectedItem = NonQSAndUserDefinedStores.ToList()[0];
                        }
                    }
                    FillOpningBalanceDataGrid();
                }
            };
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillBackStores(long clinicId)
        {
            //clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            //BizAction.MasterTable = MasterTableNameList.M_Store;
            //BizAction.MasterList = new List<MasterListItem>();

            //if (clinicId > 0)
            //{
            //    BizAction.Parent = new KeyValue { Value = "ClinicID", Key = clinicId.ToString() };
            //}

            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //Client.ProcessCompleted += (s, e) =>
            //{
            //    if (e.Error == null && e.Result != null)
            //    {
            //        List<MasterListItem> objList = new List<MasterListItem>();



            //        objList.Add(new MasterListItem(0, "- Select -"));
            //        if (clinicId > 0)
            //        {
            //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
            //        }

            //        cmbBackStore.ItemsSource = null;
            //        cmbBackStore.ItemsSource = objList;
            //        cmbBackStore.SelectedItem = objList[0];



            //    }
            //};

            //Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //Client.CloseAsync();

            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            clsItemMasterVO obj = new clsItemMasterVO();
            obj.RetrieveDataFlag = false;
            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizActionObj.ToStoreList = new List<clsStoreVO>();
            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
                    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true };
                    clsStoreVO select = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true };
                    BizActionObj.ItemMatserDetails.Insert(0, select);
                    BizActionObj.ToStoreList.Insert(0, Default);
                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == clinicId && item.Status == true && item.IsQuarantineStore == false//((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                                 select item;
                    var NonQSAndUserDefinedStores = from item in BizActionObj.ToStoreList.ToList()
                                                    where item.IsQuarantineStore == false
                                                    select item;

                    NonQSAndUserDefinedStores.ToList().Insert(0, select);

                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        if (result.ToList().Count > 0)
                        {
                            cmbBackStore.ItemsSource = result.ToList();
                            cmbBackStore.SelectedItem = result.ToList()[0];
                            cmbStore.ItemsSource = result.ToList();
                            cmbStore.SelectedItem = result.ToList()[0];
                        }
                    }
                    else
                    {
                        if (NonQSAndUserDefinedStores != null)
                        {
                            cmbBackStore.ItemsSource = NonQSAndUserDefinedStores.ToList();
                            cmbBackStore.SelectedItem = NonQSAndUserDefinedStores.ToList()[0];
                            cmbStore.ItemsSource = NonQSAndUserDefinedStores.ToList();
                            cmbStore.SelectedItem = NonQSAndUserDefinedStores.ToList()[0];
                        }
                    }
                }
            };
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FillOpningBalanceDataGrid();
        }

        private void cmdDeleteOpeningBalanceItems_Click(object sender, RoutedEventArgs e)
        {
            if (dgAddOpeningBalanceItems.SelectedItem != null)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Delete the selected Item ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        OeningBalanceAddedItems.RemoveAt(dgAddOpeningBalanceItems.SelectedIndex);
                        CalculateOpeningBalanceSummary();
                    }
                };

                msgWD.Show();
            }
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            if (DataList.Count > 0)
            {
                // bool IsPrint = true;
                // IsPrint = ChkClinicAndStore();
                //if (IsPrint)
                //{
                long ClincID = 0;
                long StoreID = 0;
                Nullable<DateTime> FDT = null;
                Nullable<DateTime> TDT = null;
                Nullable<DateTime> TDP = null;

                if (dtpFromDate.SelectedDate != null)
                {
                    FDT = dtpFromDate.SelectedDate.Value.Date.Date;
                }
                if (dtpToDate.SelectedDate != null)
                {
                    TDT = dtpToDate.SelectedDate.Value.Date.Date.AddDays(1);
                    TDP = dtpToDate.SelectedDate.Value.Date.Date;
                }
                if (cmbClinic.SelectedItem != null)
                {
                    ClincID = ((MasterListItem)cmbClinic.SelectedItem).ID;
                }

                long UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                if (cmbStore.SelectedItem != null)
                {
                    StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
                }
                string URL;
                if (FDT != null && TDT != null && TDP != null)
                {
                    URL = "../Reports/InventoryPharmacy/OpeningBalance.aspx?FromDate=" + FDT.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + TDT.Value.ToString("dd/MMM/yyyy") + "&ToDatePrint=" + TDP.Value.ToString("dd/MMM/yyyy") + "&ClinicID=" + ClincID + "&StoreID=" + StoreID + "&UserID=" + UserID + "&IsExcel=" + chkPrintReport.IsChecked;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {
                    URL = "../Reports/InventoryPharmacy/OpeningBalance.aspx?ClinicID=" + ClincID + "&StoreID=" + StoreID + "&IsExcel=" + chkPrintReport.IsChecked;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

                }
                //  }
            }
            else
            {
                string msgTitle = "";
                string msgText = "Records not Found";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW.Show();
            }

        }

        private bool ChkClinicAndStore()
        {
            bool result = true;


            if ((MasterListItem)cmbClinic.SelectedItem == null)
            {
                cmbClinic.TextBox.SetValidation("Please select Clinic");
                cmbClinic.TextBox.RaiseValidationError();
                cmbClinic.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbClinic.SelectedItem).ID == 0)
            {
                cmbClinic.TextBox.SetValidation("Please select Clinic");
                cmbClinic.TextBox.RaiseValidationError();
                cmbClinic.Focus();
                result = false;

            }
            else
                cmbClinic.TextBox.ClearValidationError();

            if ((clsStoreVO)cmbStore.SelectedItem == null)
            {
                cmbStore.TextBox.SetValidation("Please select Store");
                cmbStore.TextBox.RaiseValidationError();
                cmbStore.Focus();
                result = false;
            }
            else if (((clsStoreVO)cmbStore.SelectedItem).StoreId == 0)
            {
                cmbStore.TextBox.SetValidation("Please select Store");
                cmbStore.TextBox.RaiseValidationError();
                cmbStore.Focus();
                result = false;

            }
            else
                cmbStore.TextBox.ClearValidationError();

            return result;
        }

        #region Added by Ashish Thombre

        private void txtBatchCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            //tb.Text = tb.Text.ToUpper();
            tb.SelectionStart = tb.Text.Length;
        }

        private void txtBatchCode_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;

            string tmpBachCode = string.Empty;
            tmpBachCode = tb.Text.ToString().Trim();
            tb.Text = tmpBachCode;
            if (!tb.Text.IsItSpecialChar())
            {
                MessageBoxChildWindow cw = null;
                cw = new MessageBoxChildWindow("", "Special character not allowed.", MessageBoxButtons.Ok, MessageBoxIcon.Information);

                cw.Show();
                tb.Text = string.Empty;
            }
        }

        #endregion

        private void cmbBackStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbBackStore.SelectedItem != null)
                StoreId = ((clsStoreVO)cmbBackStore.SelectedItem).StoreId;
        }

        private void cmbPUOM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgAddOpeningBalanceItems.SelectedItem != null)
            {
                //List<clsConversionsVO> UOMConvertLIst = new List<clsConversionsVO>();

                //if (dgAddOpeningBalanceItems.SelectedItem != null)
                //    UOMConvertLIst = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).UOMConversionList;

                clsConversionsVO objConversion = new clsConversionsVO();

                AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;
                long SelectedUomId = 0;

                //((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity = 0;
                //((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Quantity = 0;


                if (cmbConversions.SelectedItem != null && ((MasterListItem)cmbConversions.SelectedItem).ID > 0)
                {
                    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM = ((MasterListItem)cmbConversions.SelectedItem);

                    ////objConversion = CalculateConversionFactor(((MasterListItem)cmbConversions.SelectedItem).ID, ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseUOMID);
                    //CalculateConversionFactor(((MasterListItem)cmbConversions.SelectedItem).ID, ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SUOMID);

                    // Function Parameters
                    // FromUOMID - Transaction UOM
                    // ToUOMID - Stocking UOM
                    CalculateConversionFactorCentral(((MasterListItem)cmbConversions.SelectedItem).ID, ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SUOMID);
                }
                else
                {
                    //MasterListItem objConversionSet = new MasterListItem();
                    //objConversionSet.ID = 0;
                    //objConversionSet.Description = "- Select -";

                    //cmbConversions.SelectedItem = objConversionSet;

                    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor = 0;
                    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseConversionFactor = 0;

                    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP;
                    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate;
                }

                ////if (cmbConversions.SelectedItem != null)
                ////    SelectedUomId = ((MasterListItem)cmbConversions.SelectedItem).ID;

                ////if (UOMConvertLIst.Count > 0)
                ////    objConversion = UOMConvertLIst.Where(z => z.FromUOMID == SelectedUomId && z.ToUOMID == ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SUOMID).FirstOrDefault();

                //if (cmbConversions.SelectedItem != null)
                //{
                //    if (((MasterListItem)cmbConversions.SelectedItem).ID == ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SUOMID)
                //    {
                //        ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor = 1;
                //    }
                //    else
                //    {
                //        if (objConversion != null)
                //        {
                //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor = objConversion.ConversionFactor;
                //        }
                //        else
                //        {
                //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor = 0;
                //        }
                //    }
                //}

                ////objConversion.ID = 0;
                ////objConversion.Description = "- Select -";
                ////UOMConvertLIst.Add(objConversion);

                ////UOMConvertLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList);
                ////cmbConversions.ItemsSource = UOMConvertLIst.DeepCopy();

                ////List<clsConversionsVO> ConvertLst = new List<clsConversionsVO>();
                ////ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList());
                ////ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.ToUOM).Select(y => y.First()).ToList());

                ////List<clsConversionsVO> MainConvertLst = new List<clsConversionsVO>();
                ////MainConvertLst = UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList().DeepCopy();

                ////ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList());


                //((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity = 0;
                //((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Quantity = 0;


                ////if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID != ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SUOMID)
                ////{
                ////    if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor > 0)
                ////    {
                ////        ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP / ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor);
                ////        ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate / ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor);
                ////    }
                ////}
                ////else
                ////{

                ////    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP;
                ////    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate;

                ////}

                ////if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedPUM.Description != ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).PUOM)
                ////{

                ////    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP;
                ////    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate;

                ////}
                ////else
                ////{
                ////    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP / ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor);
                ////    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate / ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor);
                ////}


                CalculateOpeningBalanceSummary();
            }
        }

        private void CalculateConversionFactorCentral(long FromUOMID, long ToUOMID)
        {
            clsConversions objConversionVO = new clsConversions();

            try
            {
                List<clsConversionsVO> UOMConvertLIst = new List<clsConversionsVO>();

                if (dgAddOpeningBalanceItems.SelectedItem != null)
                {
                    UOMConvertLIst = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).UOMConversionList;



                    objConversionVO.UOMConvertLIst = UOMConvertLIst;

                    objConversionVO.MainMRP = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP;
                    objConversionVO.MainRate = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate;
                    objConversionVO.SingleQuantity = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity;

                    long BaseUOMID = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseUOMID;
                    //long TransactionUOMID = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID;

                    // Function Parameters
                    // FromUOMID - Transaction UOM
                    // ToUOMID - Stocking UOM
                    // BaseUOMID - Base UOM
                    objConversionVO.CalculateConversionFactor(FromUOMID, ToUOMID, BaseUOMID);
                    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseConversionFactor = objConversionVO.BaseConversionFactor;
                    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate = objConversionVO.Rate;
                    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = objConversionVO.MRP;


                    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Quantity = objConversionVO.Quantity;
                    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseQuantity = objConversionVO.BaseQuantity;

                    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseRate = objConversionVO.BaseRate;
                    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseMRP = objConversionVO.BaseMRP;

                    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor = objConversionVO.ConversionFactor;


                }

            }
            catch (Exception ex)
            {
                objConversionVO = null;
            }
        }

        # region Commented2

        //private void CalculateConversionFactor(long FromUOMID, long ToUOMID)
        //{
        //    List<clsConversionsVO> UOMConvertLIst = new List<clsConversionsVO>();

        //    if (dgAddOpeningBalanceItems.SelectedItem != null)
        //        UOMConvertLIst = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).UOMConversionList;

        //    clsConversionsVO objConversionFrom = new clsConversionsVO();
        //    clsConversionsVO objConversionTo = new clsConversionsVO();

        //    if (UOMConvertLIst.Count > 0)
        //    {
        //        //objConversion = UOMConvertLIst.Where(z => z.FromUOMID == FromUOMID && z.ToUOMID == ToUOMID).FirstOrDefault();

        //        objConversionFrom = UOMConvertLIst.Where(z => z.FromUOMID == FromUOMID && z.ToUOMID == ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseUOMID).FirstOrDefault();
        //        objConversionTo = UOMConvertLIst.Where(z => z.FromUOMID == ToUOMID && z.ToUOMID == ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseUOMID).FirstOrDefault();
        //    }

        //    float CalculatedCF = 0;
        //    float CalculatedFromCF = 0;
        //    float CalculatedToCF = 0;

        //    if (objConversionFrom != null) //&& objConversionTo != null
        //    {
        //        CalculatedFromCF = objConversionFrom.ConversionFactor;
        //        CalculatedToCF = objConversionTo.ConversionFactor;

        //        CalculatedCF = CalculatedFromCF / CalculatedToCF;

        //        ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor = CalculatedCF;
        //        ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseConversionFactor = CalculatedFromCF;
        //    }


        //    //if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID != ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseUOMID)  //e.g. (Selected Transaction UOM) Box != Strip (Item Master Base UOM) 
        //    //{
        //    if (CalculatedCF > 0 && FromUOMID != ToUOMID) // e.g. (Selected Transaction UOM) Box != Strip (Item Master Stock UOM) 
        //    {
        //        if (CalculatedCF.ToString().IsItNumber()) // e.g. Strip to Tab
        //        {
        //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP * CalculatedFromCF);
        //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate * CalculatedFromCF);

        //            //* ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity

        //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Quantity = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity * ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor);
        //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseQuantity = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity * ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseConversionFactor);

        //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseRate = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate;
        //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseMRP = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP;
        //        }
        //        else     // e.g. Tab to Strip  (Reverse flow 1 Tablet = How many Strip ? if CF = 10 then 1/10)
        //        {
        //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP * CalculatedFromCF;
        //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate * CalculatedFromCF;

        //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Quantity = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity) * CalculatedCF;
        //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseQuantity = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity) * CalculatedFromCF;

        //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseRate = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate;
        //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseMRP = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP;
        //        }
        //    }
        //    else if (CalculatedCF > 0 && FromUOMID == ToUOMID)  // e.g. (Selected Transaction UOM) Strip == Strip (Item Master Stock UOM) 
        //    {
        //        if (UOMConvertLIst.Count > 0)
        //        {
        //            clsConversionsVO objConversionFromSame = new clsConversionsVO();
        //            clsConversionsVO objConversionToSame = new clsConversionsVO();

        //            objConversionFromSame = UOMConvertLIst.Where(z => z.FromUOMID == ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID && z.ToUOMID == ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseUOMID).FirstOrDefault();
        //            //objConversionFromSame = UOMConvertLIst.Where(z => z.FromUOMID == ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).PUOMID && z.ToUOMID == ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseUOMID).FirstOrDefault();
        //            objConversionToSame = UOMConvertLIst.Where(z => z.FromUOMID == ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID && z.ToUOMID == ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseUOMID).FirstOrDefault();

        //            float CalculatedFromCFSame = 0;
        //            float CalculatedToCFSame = 0;

        //            CalculatedFromCFSame = objConversionFromSame.ConversionFactor;
        //            CalculatedToCFSame = objConversionToSame.ConversionFactor;

        //            CalculatedCF = CalculatedFromCFSame / CalculatedToCFSame;

        //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor = CalculatedCF;
        //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseConversionFactor = CalculatedFromCFSame;

        //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP * CalculatedFromCFSame);
        //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate * CalculatedFromCFSame);

        //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Quantity = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity) * CalculatedCF;
        //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseQuantity = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity * ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseConversionFactor);

        //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseRate = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate;
        //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseMRP = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP;

        //        }
        //    }
        //    //}
        //    //else   //e.g. (Selected Transaction UOM) Box == Box (Item Master Purchase UOM) 
        //    //{

        //    //    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP;
        //    //    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate;

        //    //}
        //}

        # endregion

        # region Commented

        //private void CalculateConversionFactor(long FromUOMID, long ToUOMID)      //private clsConversionsVO CalculateConversionFactor(long FromUOMID, long ToUOMID)
        //{
        //    List<clsConversionsVO> UOMConvertLIst = new List<clsConversionsVO>();

        //    if (dgAddOpeningBalanceItems.SelectedItem != null)
        //        UOMConvertLIst = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).UOMConversionList;

        //    clsConversionsVO objConversionFrom = new clsConversionsVO();
        //    clsConversionsVO objConversionTo = new clsConversionsVO();

        //    if (UOMConvertLIst.Count > 0)
        //    {
        //        //objConversion = UOMConvertLIst.Where(z => z.FromUOMID == FromUOMID && z.ToUOMID == ToUOMID).FirstOrDefault();

        //        objConversionFrom = UOMConvertLIst.Where(z => z.FromUOMID == FromUOMID && z.ToUOMID == ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseUOMID).FirstOrDefault();
        //        objConversionTo = UOMConvertLIst.Where(z => z.FromUOMID == ToUOMID && z.ToUOMID == ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseUOMID).FirstOrDefault();
        //    }

        //    float CalculatedCF = 0;
        //    float CalculatedFromCF = 0;
        //    float CalculatedToCF = 0;

        //    if (objConversionFrom != null && objConversionTo != null)
        //    {
        //        CalculatedFromCF = objConversionFrom.ConversionFactor;
        //        CalculatedToCF = objConversionTo.ConversionFactor;

        //        CalculatedCF = CalculatedFromCF / CalculatedToCF;

        //        ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor = CalculatedCF;
        //    }


        //    if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID != ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).PUOMID)  //e.g. (Selected Transaction UOM) Box != Strip (Item Master Purchase UOM) 
        //    {
        //        if (CalculatedCF > 0 && FromUOMID != ToUOMID) // e.g. (Selected Transaction UOM) Box != Strip (Item Master Stock UOM) 
        //        {
        //            if (CalculatedCF.ToString().IsItNumber()) // e.g. Strip to Tab
        //            {
        //                ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP / CalculatedCF);
        //                ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate / CalculatedCF);
        //            }
        //            else     // e.g. Tab to Strip  (Reverse flow 1 Tablet = How many Strip ? if CF = 10 then 1/10)
        //            {
        //                ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP * CalculatedCF;
        //                ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate * CalculatedCF;
        //            }
        //        }
        //        else if (CalculatedCF > 0 && FromUOMID == ToUOMID)  // e.g. (Selected Transaction UOM) Strip == Strip (Item Master Stock UOM) 
        //        {
        //            if (UOMConvertLIst.Count > 0)
        //            {
        //                clsConversionsVO objConversionFromSame = new clsConversionsVO();
        //                clsConversionsVO objConversionToSame = new clsConversionsVO();

        //                objConversionFromSame = UOMConvertLIst.Where(z => z.FromUOMID == ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).PUOMID && z.ToUOMID == ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseUOMID).FirstOrDefault();
        //                objConversionToSame = UOMConvertLIst.Where(z => z.FromUOMID == ToUOMID && z.ToUOMID == ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseUOMID).FirstOrDefault();

        //                float CalculatedFromCFSame = 0;
        //                float CalculatedToCFSame = 0;

        //                CalculatedFromCFSame = objConversionFromSame.ConversionFactor;
        //                CalculatedToCFSame = objConversionToSame.ConversionFactor;

        //                CalculatedCF = CalculatedFromCFSame / CalculatedToCFSame;

        //                ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP / CalculatedCF);
        //                ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate / CalculatedCF);

        //            }
        //        }
        //    }
        //    else   //e.g. (Selected Transaction UOM) Box == Box (Item Master Purchase UOM) 
        //    {

        //        ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP;
        //        ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate;

        //    }

        //    //if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedPUM.Description == ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).PUOM)
        //    //{
        //    //    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Quantity = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity * ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor);


        //    //}
        //    //else
        //    //{
        //    //    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Quantity = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity);

        //    //    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP / ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor);
        //    //    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate / ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor);

        //    //}

        //    //if (objConversion == null)
        //    //{
        //    //    objConversion = UOMConvertLIst.Where(z => z.FromUOMID == ToUOMID && z.ToUOMID == FromUOMID).FirstOrDefault();

        //    //    float CalculatedCF = 0;

        //    //    if (UOMConvertLIst.Count > 0)
        //    //    {
        //    //        CalculatedCF = 0;

        //    //        CalculatedCF = (1 / objConversion.MainConversionFactor);
        //    //        objConversion.ConversionFactor = CalculatedCF;
        //    //    }
        //    //    else
        //    //    {


        //    //    }
        //    //}

        //    //return objConversionFrom;

        //}

        # endregion

        private void txtSingleQuantity_LostFocus(object sender, RoutedEventArgs e)
        {
            clsOpeningBalVO obj = new clsOpeningBalVO();
            obj = (clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem;
            if (obj.SingleQuantity == 0)
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = null;
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity Can't Be Zero. Please Enter Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                return;

            }
            else if (obj.SingleQuantity < 0)
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = null;
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity Can't Be Negative. Please Enter Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                return;
            }
            else
            {


                if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedPUM.Description == ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).PUOM)
                {
                    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Quantity = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity * ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor);
                }
                else
                {
                    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Quantity = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity);
                }

            }
            CalculateOpeningBalanceSummary();

        }

        private void cmbBUOM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbPUOM_GotFocus(object sender, RoutedEventArgs e)
        {
            AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;

            if (cmbConversions.ItemsSource == null || (cmbConversions.ItemsSource != null && ((List<MasterListItem>)cmbConversions.ItemsSource).ToList().Count == 0) || (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).UOMConversionList == null || ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).UOMConversionList.Count == 0))
            {
                FillUOMConversions(cmbConversions);
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgAddOpeningBalanceItems.SelectedItem != null)
            {
                Conversion win = new Conversion();

                win.FillUOMConversions(((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ItemID, ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID);
                win.OnSaveButton_Click += new RoutedEventHandler(win_OnSaveButton_Click);
                win.OnCancelButton_Click += new RoutedEventHandler(win_OnCancelButton_Click);
                win.Show();
            }

        }

        void win_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        void win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            Conversion Itemswin = (Conversion)sender;

            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM = (MasterListItem)Itemswin.cmbConversion.SelectedItem;
            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).UOMList = Itemswin.UOMConvertLIst;
            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).UOMConversionList = Itemswin.UOMConversionLIst;
            if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity > 0)
            {
                if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM != null && ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID == ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseUOMID && (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity % 1) != 0)
                {
                    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity = 1;
                    string msgText = "Quantity Cannot Be In Fraction.";
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();
                }
            }
            CalculateConversionFactorCentral(((MasterListItem)Itemswin.cmbConversion.SelectedItem).ID, ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SUOMID);

            clsOpeningBalVO objj = new clsOpeningBalVO();
            objj = (clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem;
            if (objj.Re_Order == 0 && objj.SingleQuantity > 0)
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = null;
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Reorder Quantity Is Not Define For This Item.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                mgbx.Show();
                //obj.SingleQuantity = 1;
                // return;
            }

            else if (((objj.BaseQuantity) < objj.Re_Order * objj.StockCF) && objj.SingleQuantity > 0)
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = null;
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Entered Quantity Is Less Than Reorder Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                mgbx.Show();
                //obj.SingleQuantity = 1;
                // return;
            }

            CalculateOpeningBalanceSummary();

        }

        bool BarCodePrintValidation()
        {
            bool result = true;
            if (dgOpeningBalanceList.SelectedItem == null)
            {
                result = false;
                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please select Item Batch.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                msgbox.Show();
            }
            else if (string.IsNullOrEmpty(((clsOpeningBalVO)dgOpeningBalanceList.SelectedItem).Barcode))
            {
                result = false;
                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Bracode is not generated for this Batch Item.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                msgbox.Show();
            }
            return result;
        }

        private void CmdPrintBarcode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BarCodePrintValidation())
                {
                    BarcodeForm win = new BarcodeForm();

                    #region For Barcode Size Changes 05072018

                    string date = "";
                    long ItemID = 0;
                    string BatchID = null;
                    string BatchCode = null;
                    string ItemCode = null;
                    string MRP = null;
                    string ItemName = "";

                    if (dgOpeningBalanceList.SelectedItem != null)
                    {
                        if ((((clsOpeningBalVO)dgOpeningBalanceList.SelectedItem).ItemName).Length > 15)
                            ItemName = (((clsOpeningBalVO)dgOpeningBalanceList.SelectedItem).ItemName).Substring(0, 14);
                        else
                            ItemName = ((clsOpeningBalVO)dgOpeningBalanceList.SelectedItem).ItemName;
                    }

                    win.MyItemText.Text = ItemName;
                    win.MyItemText1.Text = ItemName;
                    win.MyItemText2.Text = ItemName;
                    win.MyItemText3.Text = ItemName;
                    ////////////////////////////////////////////

                    if (dgOpeningBalanceList.SelectedItem != null)
                    {
                        if (dgOpeningBalanceList.SelectedItem != null && ((clsOpeningBalVO)dgOpeningBalanceList.SelectedItem).MRP > 0)
                            MRP = String.Format("{0:0.00}", ((clsOpeningBalVO)dgOpeningBalanceList.SelectedItem).MRP);
                    }

                    win.MyBatchUnitMRPText.Text = MRP;
                    win.MyBatchUnitMRPText1.Text = MRP;
                    win.MyBatchUnitMRPText2.Text = MRP;
                    win.MyBatchUnitMRPText3.Text = MRP;
                    ////////////////////////////////////////////

                    if (dgOpeningBalanceList.SelectedItem != null)
                    {
                        if (((clsOpeningBalVO)dgOpeningBalanceList.SelectedItem).ExpiryDate != null)
                        {
                            date = ((clsOpeningBalVO)dgOpeningBalanceList.SelectedItem).ExpiryDate.Value.ToString("MM/yy");
                        }
                        else
                            date = null;
                    }

                    if (date != null)
                    {
                        win.MyExpiryText.Text = date.ToString();
                        win.MyExpiryText1.Text = date.ToString();
                        win.MyExpiryText2.Text = date.ToString();
                        win.MyExpiryText3.Text = date.ToString();
                    }
                    ////////////////////////////////////////////

                    if (dgOpeningBalanceList.SelectedItem != null)
                    {
                        ItemID = ((clsOpeningBalVO)dgOpeningBalanceList.SelectedItem).ItemID;
                        if (((clsOpeningBalVO)dgOpeningBalanceList.SelectedItem).BatchCode != null)
                        {
                            string str = ((clsOpeningBalVO)dgOpeningBalanceList.SelectedItem).BatchCode;
                            BatchID = Convert.ToString(((clsOpeningBalVO)dgOpeningBalanceList.SelectedItem).BatchID);
                            //BatchCode = str.Substring(0, 3) + "/" + BatchID.ToString() + "B";
                            //if (str.Length > 5)
                            //    BatchCode = str.Substring(0, 4);
                            //else
                            //    BatchCode = str;
                            BatchCode = str;
                        }
                        else
                        {
                            BatchID = ((clsOpeningBalVO)dgOpeningBalanceList.SelectedItem).BatchID + "BI";
                        }
                    }

                    win.MyBatchCodeText.Text = BatchCode.ToString();
                    win.MyBatchCodeText1.Text = BatchCode.ToString();
                    win.MyBatchCodeText2.Text = BatchCode.ToString();
                    win.MyBatchCodeText3.Text = BatchCode.ToString();
                    ////////////////////////////////////////////

                    #endregion

                    win.PrintData = ((clsOpeningBalVO)dgOpeningBalanceList.SelectedItem).Barcode;
                    win.PrintItem = ((clsOpeningBalVO)dgOpeningBalanceList.SelectedItem).ItemName;
                    win.PrintDate = "";// date;
                    win.PrintFrom = "Opening Balance";
                    win.Show();
                }
            }
            catch (Exception EX)
            {
                throw;
            }
        }
    }
}
