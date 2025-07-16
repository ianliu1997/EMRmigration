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
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using System.Windows.Browser;
using CIMS;
using System.Collections;
using PalashDynamics.Animations;
using System.Collections.ObjectModel;
using PalashDynamics.Collections;
using MessageBoxControl;
using System.Windows.Resources;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using System.Text;

namespace PalashDynamics.Pharmacy
{
    public partial class OpeningBalance : UserControl
    {
        Boolean IsItemListSelected = false;
        public Boolean isPageLoaded = false;
        private SwivelAnimation objAnimation;
        public List<clsItemMasterVO> objItemList;
        //Added by pallavi
        public Boolean lessMrp = false;
        public Boolean zeroQuantity = false;
        public Boolean BatchReqNotFilled = false;
        public Boolean ExpiryReqNotFilled = false;
        
        //private EventHandler<DataGridCellEditEndedEventArgs> dgAddOpeningBalance_CellEditEnded;
        public ObservableCollection<clsOpeningBalVO> OeningBalanceAddedItems { get; set; }
        public PagedSortableCollectionView<clsOpeningBalVO> DataList { get; private set; }
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        public OpeningBalance()
        {
            InitializeComponent();
            this.DataContext = new clsOpeningBalVO();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            dgAddOpeningBalanceItems.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgAddOpeningBalanceItems_CellEditEnded);
            
            //dtpFromDate.SelectedDate = DateTime.Now.Date;
            //dtpToDate.SelectedDate = DateTime.Now.Date;

            //Paging
            DataList = new PagedSortableCollectionView<clsOpeningBalVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            //======================================================

        }

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

            try
            {
                if (!isPageLoaded)
                {


                    SetCommandButtonState("New");
                    objAnimation.Invoke(RotationType.Backward);
                    OeningBalanceAddedItems = new ObservableCollection<clsOpeningBalVO>();
                    dgAddOpeningBalanceItems.ItemsSource = OeningBalanceAddedItems;
                    FillClinic();
                  
                    dtpFromDate.SelectedDate = DateTime.Now.Date;
                    dtpToDate.SelectedDate = DateTime.Now.Date;
                   // FillOpningBalanceDataGrid();
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
                                       cmbClinic.SelectedItem = item ;
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


        #region Set Command Button State New/Save/Modify/Print
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    cmdNew.IsEnabled = true;
                    cmdPrint.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;

                case "Save":

                    cmdNew.IsEnabled = true;
                    cmdPrint.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    IsCancel = true;
                    //cmdCancel.IsEnabled = false;
                    break;

                case "Modify":

                    cmdNew.IsEnabled = false;
                    cmdPrint.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    IsCancel = true;
                    cmdCancel.IsEnabled = true;
                    break;

                case "ClickNew":
                    cmdNew.IsEnabled = false;
                    cmdPrint.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;
                default:
                    break;
            }
        }

        #endregion


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

        private void CmdAdd_Click(object sender, RoutedEventArgs e)
        {



        }

               
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            objItemList = null;
        }

        int ClickedFlag1 = 0;
        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag1 += 1;
            bool isValid = true;
            if (ClickedFlag1 == 1)
            {
                isValid = checkValidationForMRPAndPurchaseRate();

                if (isValid)
                {
                    //By Anjali..............................
                   StringBuilder strError = new StringBuilder();

                   //foreach (var item in OeningBalanceAddedItems.ToList<clsOpeningBalVO>())
                   //{
                       if (OeningBalanceAddedItems != null && OeningBalanceAddedItems.ToList<clsOpeningBalVO>().Count > 0)
                       {
                           var item1 = from r in OeningBalanceAddedItems.ToList<clsOpeningBalVO>()
                                       where (r.IsBatchRequired == true && r.ItemExpiredInDays == 0)
                                       select r;

                           if (item1 !=null && item1.ToList().Count > 0)
                           {
                               foreach(var clsOpeningBalVO in item1.ToList())
                               {
                               if (strError.ToString().Length > 0)
                                   strError.Append(",");
                               strError.Append(item1.ToList()[0].ItemName);

                               }
                           }
                       }
                   //}
                    //...............................................
                   string msgTitle = "";
                   string msgText = "";
                   if (strError != null && Convert.ToString(strError) != string.Empty)
                   {
                       msgText = "Expired In Days Are not Defined In Master For " + strError + " Item ,\n Are You Sure You Want To Save Opening Balance Details?";
                   }
                   else
                   {
                       msgText = "Are you sure you want to save the Opening Balance Details";
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
                    ClickedFlag1 = 0;
            }
           
        }

        WaitIndicator Indicatior;
        private void SaveOpeningBalance()
        {
            try
            {


                Indicatior = new WaitIndicator();
                Indicatior.Show();



                //if (!isValid)
                //{


                    clsAddOpeningBalanceBizActionVO objBizActionVO = new clsAddOpeningBalanceBizActionVO();
                    objBizActionVO.OpeningBalanceList = new List<clsOpeningBalVO>();
                    objBizActionVO.OpeningBalanceList = OeningBalanceAddedItems.ToList<clsOpeningBalVO>();
                    objBizActionVO.OpeningBalVO = new clsOpeningBalVO();
                    objBizActionVO.OpeningBalVO.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    objBizActionVO.OpeningBalVO.AddedDateTime = DateTime.Now;
                    objBizActionVO.OpeningBalVO.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    //objBizActionVO.OpeningBalVO.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    objBizActionVO.OpeningBalVO.Remarks = txtremarks.Text;
                    objBizActionVO.OpeningBalVO.UnitId = ((MasterListItem)cmbBackClinic.SelectedItem).ID;

                    //objBizActionVO.OpeningBalVO.StoreID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.



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
                                                                      new MessageBoxControl.MessageBoxChildWindow("Opening Balance", "Batch already exists, updated successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);  //Batch already exists
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
                            //Indicatior.Close();
                            //System.Windows.Browser.HtmlPage.Window.Alert("Error occured while adding visit details.");
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Opening Balance details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }

                    };

                    Client.ProcessAsync(objBizActionVO, User);
                    Client.CloseAsync();
                //}
                //else
                //{
                    
                //    if (lessMrp == true && zeroQuantity == true)
                //    {
                //        MessageBoxControl.MessageBoxChildWindow msgW1;
                //        msgW1 = new MessageBoxControl.MessageBoxChildWindow("Validation Error", "Please Enter MRP Greater Than Rate & Please Enter Quantity Greate Than 0", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //        msgW1.Show();
                //        Indicatior.Close();
                //    }
                //    else if (lessMrp == true)
                //        {
                //            MessageBoxControl.MessageBoxChildWindow msgW1;
                //            msgW1 = new MessageBoxControl.MessageBoxChildWindow("Validation Error", "Please Enter MRP Greater Than Rate", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //            msgW1.Show();
                //            Indicatior.Close();
                //        }
                //    else if (zeroQuantity == true)
                //    {
                //        MessageBoxControl.MessageBoxChildWindow msgW1;
                //        msgW1 = new MessageBoxControl.MessageBoxChildWindow("Validation Error", "Please Enter Quantity Greater Than 0", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //        msgW1.Show();
                //        Indicatior.Close();
                //    }
                //    else if(BatchReqNotFilled == true)
                //    {
                //        Indicatior.Close();                                
                //    }
                //    else if (ExpiryReqNotFilled == true)
                //    {
                //        Indicatior.Close();
                //    }
                //}
                    
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
                        if (item.IsBatchRequired == true )
                        {
                            if (item.BatchCode == "" || item.BatchCode == null)
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
                                isValid =false;

                                MessageBoxControl.MessageBoxChildWindow msgW3 =
                              new MessageBoxControl.MessageBoxChildWindow("Expiry Date Required!.", "Please Enter Expiry Date!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW3.Show();
                                break;
                            }
                                //By Anjali.........................
                            else if (item.IsBatchRequired && item.ExpiryDate.HasValue && item.ItemExpiredInDays >0 && (int)(item.ExpiryDate.Value - DateTime.Now).TotalDays < item.ItemExpiredInDays)
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
                                                      where r.BatchCode == item.BatchCode && r.ExpiryDate == item.ExpiryDate 
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
                            msgW1 = new MessageBoxControl.MessageBoxChildWindow("Validation Error", "Please Enter Quantity Greater Than 0 for Item" + item.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            break;
                        }
                        if (item.Rate == 0)
                         {
                             isValid = false;
                             MessageBoxControl.MessageBoxChildWindow msgW1;
                             msgW1 = new MessageBoxControl.MessageBoxChildWindow("Validation Error", "Please Enter Rate Greater Than Zero For Item" + item.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                             msgW1.Show();
                             break;
                         }
                        
                    }
                }

                            
                

                if (objList.Count <= 0)
                {
                    isValid = false;
                    MessageBoxControl.MessageBoxChildWindow msgW1;
                    msgW1 = new MessageBoxControl.MessageBoxChildWindow("Validation Error", "Can not save opening balance with zero items", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
                else if (((MasterListItem)cmbBackStore.SelectedItem).ID == 0)
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
                if (cmbStore.SelectedItem != null) objBizActionVO.OpeningBalance.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                if (cmbClinic.SelectedItem !=null) objBizActionVO.OpeningBalance.UnitId = ((MasterListItem)cmbClinic.SelectedItem).ID; // ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                if (dtpFromDate.SelectedDate != null)
                {
                    objBizActionVO.OpeningBalance.FromDate = dtpFromDate.SelectedDate.Value.Date;
                }
                if (dtpToDate.SelectedDate != null)
                {
                    objBizActionVO.OpeningBalance.ToDate = dtpToDate.SelectedDate.Value.Date;
                }
                //objBizActionVO.OpeningBalance.IsPagingEnabled = true;
                //objBizActionVO.OpeningBalance.StartIndex = 0;
                //objBizActionVO.OpeningBalance.MinRows = 20;

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

                            //dgOpeningBalanceList.ItemsSource = null;
                            //dgOpeningBalanceList.ItemsSource = ((clsGetStockDetailsForOpeningBalanceBizActionVO)arg.Result).ItemList;
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
                            indicator.Close();
                        }
                    }
                    else
                    {

                    }

                };

                Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception ex)
            {

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
            FillBackStores(((MasterListItem)cmbBackClinic.SelectedItem).ID);
            txttotcamt.Text = "";
            txtVatamt.Text = "";
            cmbBackClinic.IsEnabled = true;
            cmbBackStore.IsEnabled = true;


            SetCommandButtonState("ClickNew");
            objAnimation.Invoke(RotationType.Forward);

        }
        public string ModuleName { get; set; }
        public string Action { get; set; }
        bool IsCancel = true;
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            //SetCommandButtonState("New");
            //objAnimation.Invoke(RotationType.Backward);
            SetCommandButtonState("Cancel");
           

            //txtSearch.Text = string.Empty;

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
            else if (((MasterListItem)cmbBackStore.SelectedItem).ID == 0)
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
        
        
            
            if(Result ==true)
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

                StoreId = Itemswin.StoreID;
                foreach (var item in Itemswin.SelectedItems)
                {
                    //bool isExist = CheckForItemExistance(item.ID);
                    //if (!isExist)
                    //{
                    OeningBalanceAddedItems.Add(
                   new clsOpeningBalVO()
                   {
                       IsBatchRequired = item.BatchesRequired,
                       ItemID = item.ID,
                       ItemName = item.ItemName,
                       PUOM = item.PUOM,
                       SUOM = item.SUOM,
                    //   InclusiveOfTax = item.InclusiveOfTax,
                       EnableInclusiveOfTax = item.InclusiveOfTax == false ? true : false,
                       StoreID = Itemswin.StoreID,
                       DiscountOnSale = item.DiscountOnSale,
                       VATPercent = (float)item.VatPer,
                       Rate = (float)item.PurchaseRate,
                       MRP = (float)item.MRP,
                       ConversionFactor=Convert.ToSingle(item.ConversionFactor),
                       ItemExpiredInDays = item.ItemExpiredInDays
                   });
                }
                MasterListItem Store = new MasterListItem();
                Store.ID = StoreId;
                Store.Description = ((MasterListItem)Itemswin.cmbStore.SelectedItem).Description;

                cmbBackStore.SelectedItem = Store;
                
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
            //throw new NotImplementedException();
            //throw new NotImplementedException();
           
            if (dgAddOpeningBalanceItems.SelectedItem != null)
            {

                //if (e.Column.DisplayIndex == 1 || e.Column.DisplayIndex == 3 || e.Column.DisplayIndex == 8
                //   || e.Column.DisplayIndex == 10 || e.Column.DisplayIndex == 11 || e.Column.DisplayIndex == 12 || e.Column.DisplayIndex == 13)
               // if (e.Column.DisplayIndex == 8 || e.Column.DisplayIndex == 7
               //|| e.Column.DisplayIndex == 10 || e.Column.DisplayIndex == 11 || e.Column.DisplayIndex == 12 || e.Column.DisplayIndex == 13)
                if (e.Column.DisplayIndex == 7)
                {
                    clsOpeningBalVO obj = new clsOpeningBalVO();
                    obj = (clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem;
                    if (obj.Quantity == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = null;
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Zero Quantity.", "Quantity Can't Be Zero. Please Enter Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        obj.Quantity = 1;
                        return;
                        //dgAddOpeningBalanceItems.CurrentColumn = dgAddOpeningBalanceItems[7, e.Row];

                    }
                    if (obj.Quantity < 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = null;
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("", "Quantity Can't Be Negative. Please Enter Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        obj.Quantity = 1;
                        return;
                    }

                    

                  
                }
                if (e.Column.DisplayIndex == 9)
                {
                    clsOpeningBalVO obj = new clsOpeningBalVO();
                    obj = (clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem;
                    if (obj.MRP < obj.Rate)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = null;
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("", "Mrp should be greater than rate", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate + 1;
                        return;
                        //dgAddOpeningBalanceItems.CurrentColumn = dgAddOpeningBalanceItems[7, e.Row];

                    }
 
                }

                if (e.Column.DisplayIndex == 2 || e.Column.DisplayIndex == 3)
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

                                    if (obj.BatchCode =="" || obj.BatchCode == null)
                                    {
                                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                              new MessageBoxControl.MessageBoxChildWindow("Batch Required!.", "Please Enter Batch!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        msgW3.Show();
                                        return;
                                    }
                                   if (obj.ExpiryDate == null)
                                    {
                                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                              new MessageBoxControl.MessageBoxChildWindow("Expiry Date Required!.", "Please Enter Expiry Date!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        msgW3.Show();
                                        return;
                                    }
                                   else
                                       if (obj.ExpiryDate < DateTime.Now.Date)
                                       {
                                           MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                        new MessageBoxControl.MessageBoxChildWindow("", "Expiry date should not be less than current date!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                           msgW3.Show();
                                           obj.ExpiryDate = DateTime.Now.Date;
                                           return;
                                       }
                                }
                            }
                        }
                    }
                }
                if (e.Column.DisplayIndex == 11)
                {
                    if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).DiscountOnSale > 100)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                             new MessageBoxControl.MessageBoxChildWindow("", "Discount on sale can't be greater than 100!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).DiscountOnSale = 0;
                        return;
                    }

                    if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).DiscountOnSale < 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                             new MessageBoxControl.MessageBoxChildWindow("", "Discount On Sale can't be Negetive value!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).DiscountOnSale = 0;
                        return;
                    }
                }

                if (e.Column.DisplayIndex == 12)
                {
                    if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).VATPercent > 100)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                             new MessageBoxControl.MessageBoxChildWindow("", "VAT Percent can't be greater than 100!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).VATPercent = 0;
                        return;
                    }

                    if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).VATPercent < 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                             new MessageBoxControl.MessageBoxChildWindow("", "VAT Percent can't be Negetive value!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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
            ((clsOpeningBalVO)this.DataContext).TotalVAT = VATAmount;
            ((clsOpeningBalVO)this.DataContext).TotalNet = NetAmount;
        }
        private void cmbBackClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbBackClinic.SelectedItem != null)
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
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_Store;
            BizAction.MasterList = new List<MasterListItem>();

            if (clinicId > 0)
            {
                BizAction.Parent = new KeyValue { Value = "ClinicID", Key = clinicId.ToString() };
            }

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
                    cmbStore.ItemsSource = null;
                    cmbStore.ItemsSource = objList;

                    cmbBackStore.ItemsSource = null;
                    cmbBackStore.ItemsSource = objList;


                    //if (objList.Count > 1)
                    //    cmbStore.SelectedItem = objList[1];
                    //else
                        cmbStore.SelectedItem = objList[0];
                        cmbBackStore.SelectedItem = objList[0];
                        FillOpningBalanceDataGrid();

                    //FillOpningBalanceDataGrid();
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillBackStores(long clinicId)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_Store;
            BizAction.MasterList = new List<MasterListItem>();

            if (clinicId > 0)
            {
                BizAction.Parent = new KeyValue { Value = "ClinicID", Key = clinicId.ToString() };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                   
                    
                    objList.Add(new MasterListItem(0, "- Select -"));
                    if (clinicId > 0)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }
                  
                    cmbBackStore.ItemsSource = null;
                    cmbBackStore.ItemsSource = objList;                   
                    cmbBackStore.SelectedItem = objList[0];
                 

                    
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
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
                bool IsPrint = true;
                IsPrint = ChkClinicAndStore();
                if (IsPrint)
                {
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
                    if (cmbStore.SelectedItem != null)
                    {
                        StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                    }
                    string URL;
                    if (FDT != null && TDT != null && TDP != null)
                    {
                        URL = "../Reports/InventoryPharmacy/OpeningBalance.aspx?FromDate=" + FDT.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + TDT.Value.ToString("dd/MMM/yyyy") + "&ToDatePrint=" + TDP.Value.ToString("dd/MMM/yyyy") + "&ClinicID=" + ClincID + "&StoreID=" + StoreID;
                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                    }
                    else
                    {
                        URL = "../Reports/InventoryPharmacy/OpeningBalance.aspx?ClinicID=" + ClincID + "&StoreID=" + StoreID;
                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

                    }
                }
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

            if ((MasterListItem)cmbStore.SelectedItem == null)
            {
                cmbStore.TextBox.SetValidation("Please select Store");
                cmbStore.TextBox.RaiseValidationError();
                cmbStore.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbStore.SelectedItem).ID == 0)
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
            tb.Text = tb.Text.ToUpper();
            tb.SelectionStart = tb.Text.Length;
        }

        private void txtBatchCode_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
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
                StoreId = ((MasterListItem)cmbBackStore.SelectedItem).ID;
        }

    }
}