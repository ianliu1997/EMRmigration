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

using PalashDynamics.Pharmacy.Enquiry_Search;
using PalashDynamics.ValueObjects.Inventory.Quotation;
using PalashDynamics.Collections;

namespace PalashDynamics.Pharmacy
{
    public partial class QuotationDetails : UserControl
    {

        #region Declaration
        Boolean IsPageLoded = false;
        private SwivelAnimation objAnimation;
        public ObservableCollection<clsItemsEnquiryTermConditionVO> termcondition { get; set; }
        public ObservableCollection<clsQuotationDetailsVO> QuotationItems { get; set; }
        public long EnqID { get; set; }
        public long storeID { get; set; }
        WaitIndicator Indicatior;
        #endregion

        public PagedSortableCollectionView<clsQuotaionVO> DataList { get; private set; }
        #region Constructore
        public QuotationDetails()
        {
            InitializeComponent();
            this.DataContext = new clsQuotaionVO();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            grdQuotationdetails.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(grdQuotationdetails_CellEditEnded);
            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsQuotaionVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 10;
            //======================================================
        }
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
            FillQuotationDataGrid();

        }



        #endregion


        #region On New Button Click
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            EmptyUI();
            SetCommandButtonState("ClickNew");
            objAnimation.Invoke(RotationType.Forward);
            QuotationItems.Clear();
            if (txtLeadTime.Text.Trim() == "")
            {
                txtLeadTime.SetValidation("Please enter lead time");
                txtLeadTime.RaiseValidationError();
                txtLeadTime.Focus();
            }
            else
                txtLeadTime.ClearValidationError();
            //dggrdTermsandCondition.ItemsSource = null;
            //grdQuotationdetails.ItemsSource = null;
            //  Validate();
        }
        #endregion

        void EmptyUI()
        {
            try
            {
                foreach (var item in (ObservableCollection<clsItemsEnquiryTermConditionVO>)dggrdTermsandCondition.ItemsSource)
                {
                    item.IsCheckedStatus = false;
                    item.Remarks = "";
                }
                txtOther.Text = "";
                txtTotalAmount.Text = "";
                txtNETAmount.Text = "";
                txtRemarks.Text = "";
                txtLeadTime.Text = "";
                txtTotalConcession.Text = "";
                dtpFromDate.SelectedDate = DateTime.Now.Date;
                dtpValidity.SelectedDate = DateTime.Now.Date;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #region On Save Button Click


        int ClickedFlag1 = 0;
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                ClickedFlag1 = ClickedFlag1 + 1;
                //Indicatior = new WaitIndicator();
                //Indicatior.Show();

                if (ClickedFlag1 == 1)
                {
                    bool IsValidate = Validate();

                    if (IsValidate == true)
                    {


                        clsAddQuotationBizActionVO BizActionOBJ = new clsAddQuotationBizActionVO();
                        BizActionOBJ.Quotation = new clsQuotaionVO();
                        BizActionOBJ.Quotation.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        BizActionOBJ.Quotation.StoreID = ((clsStoreVO)cmbstore.SelectedItem).StoreId == null ? 0 : ((clsStoreVO)cmbstore.SelectedItem).StoreId;
                        BizActionOBJ.Quotation.SupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID == null ? 0 : ((MasterListItem)cmbSupplier.SelectedItem).ID;
                        BizActionOBJ.Quotation.Date = dtpFromDate1.SelectedDate.Value;
                        BizActionOBJ.Quotation.Time = DateTime.Now;
                        BizActionOBJ.Quotation.EnquiryID = EnqID;//((clsQuotaionVO)this.DataContext).EnquiryID == null ? 0 : ((clsQuotaionVO)this.DataContext).EnquiryID;
                        BizActionOBJ.Quotation.Notes = ((clsQuotaionVO)this.DataContext).Remarks == null ? "" : ((clsQuotaionVO)this.DataContext).Remarks;
                        BizActionOBJ.Quotation.TotalAmount = ((clsQuotaionVO)this.DataContext).TotalAmount == null ? 0 : ((clsQuotaionVO)this.DataContext).TotalAmount;
                        BizActionOBJ.Quotation.TotalConcession = ((clsQuotaionVO)this.DataContext).TotalConcession == null ? 0 : ((clsQuotaionVO)this.DataContext).TotalConcession;
                        BizActionOBJ.Quotation.TotalNet = ((clsQuotaionVO)this.DataContext).TotalNet == null ? 0 : ((clsQuotaionVO)this.DataContext).TotalNet;
                        BizActionOBJ.Quotation.Other = ((clsQuotaionVO)this.DataContext).Other == null ? 0 : ((clsQuotaionVO)this.DataContext).Other;
                        BizActionOBJ.Quotation.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        BizActionOBJ.Quotation.AddedDateTime = DateTime.Now;
                        BizActionOBJ.Quotation.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                        BizActionOBJ.Quotation.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                        BizActionOBJ.Quotation.CreatedUnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        //By Umesh
                        BizActionOBJ.Quotation.ValidityDate = dtpValidity.SelectedDate.Value;
                        BizActionOBJ.Quotation.LeadTime = txtLeadTime.Text == "" ? 0 : Convert.ToInt64(txtLeadTime.Text);
                        //

                        BizActionOBJ.Quotation.Items = new List<clsQuotationDetailsVO>();
                        BizActionOBJ.Quotation.Items = QuotationItems.ToList<clsQuotationDetailsVO>();
                        //BizActionOBJ.QuotaionList = new List<clsQuotaionVO>();
                        //BizActionOBJ.QuotaionList = QuotationItems.ToList<clsQuotaionVO>();
                        BizActionOBJ.TermsAndConditions = new List<clsItemsEnquiryTermConditionVO>();
                        ObservableCollection<clsItemsEnquiryTermConditionVO> objObserv = (ObservableCollection<clsItemsEnquiryTermConditionVO>)dggrdTermsandCondition.ItemsSource;

                        // List<clsItemsEnquiryTermConditionVO> objList= (((ObservableCollection<clsItemsEnquiryTermConditionVO>)dggrdTermsandCondition.ItemsSource).ToList<clsItemsEnquiryTermConditionVO>);
                        BizActionOBJ.TermsAndConditions = objObserv.ToList<clsItemsEnquiryTermConditionVO>();//termcondition.ToList<clsItemsEnquiryTermConditionVO>();

                        if (BizActionOBJ.Quotation.Items.Count > 0)
                        {


                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            Client.ProcessCompleted += (s, arg) =>
                            {
                                ClickedFlag1 = 0;
                                if (arg.Error == null && ((clsAddQuotationBizActionVO)arg.Result).Quotation != null)
                                {
                                    if (arg.Result != null)
                                    {

                                        if (((clsAddQuotationBizActionVO)arg.Result) != null)
                                        {
                                            //Indicatior.Close();
                                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                            new MessageBoxControl.MessageBoxChildWindow("Quotation", "Quotation details saved successfully with quotation no. " + ((clsAddQuotationBizActionVO)arg.Result).Quotation.QuotationNo, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                            SetCommandButtonState("Save");
                                            msgW1.Show();

                                            objAnimation.Invoke(RotationType.Backward);
                                            FillQuotationDataGrid();
                                        }
                                    }
                                }
                                else
                                {
                                    //Indicatior.Close();
                                    //System.Windows.Browser.HtmlPage.Window.Alert("Error occured while adding visit details.");
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Quotation details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                }

                            };

                            Client.ProcessAsync(BizActionOBJ, ((IApplicationConfiguration)App.Current).CurrentUser);
                            Client.CloseAsync();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                                  new MessageBoxControl.MessageBoxChildWindow("Quotation", "Please select items.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            Indicatior.Close();
                        }
                    }
                    else
                        ClickedFlag1 = 0;


                }
            }
            catch (Exception ex)
            {
                ClickedFlag1 = 0;
                throw;
            }


        }
        #endregion


        #region Function to Validate Form On Save
        private bool Validate()
        {
            bool isValid = true;
            MessageBoxControl.MessageBoxChildWindow mgbx = null;
            if (dtpFromDate1.SelectedDate == null)
            {
                dtpFromDate1.SetValidation("Please Enter Item quotation Date");
                dtpFromDate1.RaiseValidationError();
                dtpFromDate1.Focus();
                isValid = false;
            }
            else
                dtpFromDate1.ClearValidationError();
            if (dtpFromDate1.SelectedDate < DateTime.Now.Date)
            {
                dtpFromDate1.SetValidation("Please Enter quotation date greater than today's date");
                dtpFromDate1.RaiseValidationError();
                dtpFromDate1.Focus();
                isValid = false;
            }
            else
                dtpFromDate1.ClearValidationError();

            if (dtpValidity.SelectedDate < DateTime.Now.Date)
            {
                dtpValidity.SetValidation("Please Enter quotation validity date greater than today's date");
                dtpValidity.RaiseValidationError();
                dtpValidity.Focus();
                isValid = false;
            }
            else
                dtpValidity.ClearValidationError();
            if (txtLeadTime.Text.Trim() == "")
            {
                txtLeadTime.SetValidation("Please enter lead time");
                txtLeadTime.RaiseValidationError();
                txtLeadTime.Focus();
                isValid = false;
            }
            else
                txtLeadTime.ClearValidationError();


            if (cmbstore.SelectedItem == null)
            {
                cmbstore.TextBox.SetValidation("Plase Select the Store");
                cmbstore.TextBox.RaiseValidationError();

                cmbstore.Focus();
                isValid = false;
            }
            else if (((clsStoreVO)cmbstore.SelectedItem).StoreId == 0)
            {
                cmbstore.TextBox.SetValidation("Plase Select the Store");
                cmbstore.TextBox.RaiseValidationError();

                cmbstore.Focus();
                isValid = false;
            }
            else
                cmbstore.TextBox.ClearValidationError();


            if (cmbSupplier.SelectedItem == null)
            {
                cmbSupplier.TextBox.SetValidation("Please Select the Supplier");
                cmbSupplier.TextBox.RaiseValidationError();

                cmbSupplier.Focus();
                isValid = false;
            }
            else if (((MasterListItem)cmbSupplier.SelectedItem).ID == 0)
            {
                cmbSupplier.TextBox.SetValidation("Plase Select the Supplier");
                cmbSupplier.TextBox.RaiseValidationError();

                cmbSupplier.Focus();
                isValid = false;
            }
            else if (QuotationItems.Count > 0 && QuotationItems != null)
            {

                List<clsQuotationDetailsVO> objList = QuotationItems.ToList<clsQuotationDetailsVO>();

                if (objList != null && objList.Count > 0)
                {
                    foreach (var item in objList)
                    {
                        if (item.Rate == 0)
                        {
                            isValid = false;

                            mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Cost Price", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            mgbx.Show();
                            break;
                        }

                        //if (item.Quantity > 99999)
                        //{
                        //    isValid = false;

                        //    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity Should Be Less Than 5 Digits", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //    mgbx.Show();
                        //    break;
                        //}

                    }
                    //return true;
                }
            }
            else
            {
                cmbSupplier.TextBox.ClearValidationError();
                cmbstore.TextBox.ClearValidationError();
            }


            return isValid;
        }
        #endregion


        #region Fill Quotaion Grid On Front Panel
        private void FillQuotationDataGrid()
        {
            //    try
            //    {
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            DataList.Clear();
            clsGetQuotationBizActionVO objBizActionVO = new clsGetQuotationBizActionVO();
            objBizActionVO.Quotation = new clsQuotaionVO();
            if (cmbSearchStore.SelectedItem != null)
            {
                objBizActionVO.SearchStoreID = ((clsStoreVO)cmbSearchStore.SelectedItem).StoreId;
            }
            if (((MasterListItem)cmbSupplierSearch.SelectedItem).ID != null && ((MasterListItem)cmbSupplierSearch.SelectedItem).ID != 0)
            {
                objBizActionVO.SearchSupplierID = ((MasterListItem)cmbSupplierSearch.SelectedItem).ID;
            }
            objBizActionVO.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            objBizActionVO.ItemIDs = "";
            objBizActionVO.SupplierIDs = "";
            if (dtpFromDate.SelectedDate != null)
                objBizActionVO.SearchFromDate = dtpFromDate.SelectedDate.Value;
            if (dtpToDate.SelectedDate != null)
                objBizActionVO.SearchToDate = dtpToDate.SelectedDate.Value.AddDays(1);
            objBizActionVO.IsPagingEnabled = true;
            objBizActionVO.StartIndex = DataList.PageIndex * DataList.PageSize;
            objBizActionVO.MaxRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        clsGetQuotationBizActionVO obj = ((clsGetQuotationBizActionVO)arg.Result);

                        if (obj.QuotaionList != null)
                        {
                            DataList.TotalItemCount = obj.TotalRowCount;


                            DataList.Clear();
                            foreach (var item in obj.QuotaionList)
                            {
                                DataList.Add(item);
                            }



                            dgQuotationList.ItemsSource = null;
                            dgQuotationList.ItemsSource = DataList;

                            datapager.Source = null;
                            datapager.PageSize = objBizActionVO.MaxRows;
                            datapager.Source = DataList;
                        }
                        //dgQuotationList.ItemsSource = null;
                        //dgQuotationList.ItemsSource = obj.QuotaionList;
                        Indicatior.Close();
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
                Indicatior.Close();
            };

            Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
            //}
            //catch (Exception)
            //{

            //    throw;
            //}

        }
        #endregion


        #region Set Command Button State New/Save/Modify/Print
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = false;
                    //cmbstore.SelectedValue = 0;
                    //cmbSupplier.SelectedValue = 0;

                    //   FillStore();
                    //FillSupplier();
                    break;

                case "Save":

                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;

                    cmdCancel.IsEnabled = false;
                    break;

                case "Modify":

                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = false;

                    cmdCancel.IsEnabled = true;
                    break;

                case "ClickNew":
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdCancel.IsEnabled = true;

                    FillStore();
                    FillSupplier();


                    break;

                default:
                    break;
            }
        }

        #endregion


        #region On Cancel Button Click
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            EmptyUI();
            SetCommandButtonState("New");
            objAnimation.Invoke(RotationType.Backward);
        }
        #endregion


        #region On Form Laoded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {

                QuotationItems = new ObservableCollection<clsQuotationDetailsVO>();
                FillStore();
                FillSupplier();

                FillTearmsAndConditionGrid();
                grdQuotationdetails.ItemsSource = null;
                grdQuotationdetails.ItemsSource = QuotationItems;
                dtpFromDate.SelectedDate = DateTime.Now.Date;
                dtpFromDate1.SelectedDate = DateTime.Now.Date;
                dtpToDate.SelectedDate = DateTime.Now.Date;
                dtpValidity.SelectedDate = DateTime.Now.Date;
                SetCommandButtonState("New");
                IsPageLoded = true;
            }

        }
        #endregion


        #region FillTermsAndConditionGrid

        public void FillTearmsAndConditionGrid()
        {

            termcondition = new ObservableCollection<clsItemsEnquiryTermConditionVO>();
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_TermAndCondition;
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

                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    foreach (var item in objList)
                    {
                        clsItemsEnquiryTermConditionVO TermConditem = new clsItemsEnquiryTermConditionVO();
                        TermConditem.TermsConditionID = item.ID;
                        TermConditem.TermsCondition = item.Description;

                        termcondition.Add(TermConditem);

                    }

                    dggrdTermsandCondition.ItemsSource = null;
                    dggrdTermsandCondition.ItemsSource = termcondition;


                }

            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        #endregion


        #region Fill both Supplier dropdown (1-search,2-save)
        private void FillSupplier()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_Supplier;
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
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                        //cmbBloodGroup.ItemsSource = null;
                        //cmbBloodGroup.ItemsSource = objList;
                        cmbSupplierSearch.ItemsSource = null;
                        cmbSupplierSearch.ItemsSource = objList;
                        cmbSupplier.ItemsSource = null;
                        cmbSupplier.ItemsSource = objList;

                        //if (objList.Count > 1)
                        //{
                        //    cmbSupplierSearch.SelectedItem = objList[1];
                        //    cmbSupplier.SelectedItem = objList[1];
                        //}
                        //else
                        //{
                        cmbSupplierSearch.SelectedItem = objList[0];
                        cmbSupplier.SelectedItem = objList[0];
                        //}

                    }
                    FillQuotationDataGrid();
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion


        #region Fill both store dropdown (1-search,2-save)
        //private void FillStore()
        //{
        //    try
        //    {
        //        clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
        //        //False when we want to fetch all items
        //        clsItemMasterVO obj = new clsItemMasterVO();
        //        obj.RetrieveDataFlag = false;
        //        BizActionObj.ItemMatserDetails = new List<clsStoreVO>();


        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


        //        client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);

        //        client.ProcessCompleted += (s, args) =>
        //        {
        //            if (args.Error == null && args.Result != null)
        //            {
        //                BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;



        //                clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--select--" };
        //                BizActionObj.ItemMatserDetails.Insert(0, Default);

        //                List<clsStoreVO> objList = new List<clsStoreVO>();
        //                objList = BizActionObj.ItemMatserDetails;
        //                if (objList != null)
        //                {
        //                    //objList.Insert(0, new clsStoreVO { StoreName = "--Select--" });
        //                    cmbSearchStore.ItemsSource = null;
        //                    cmbSearchStore.ItemsSource = objList;//BizActionObj.ItemMatserDetails;
        //                    cmbstore.ItemsSource = null;
        //                    cmbstore.ItemsSource = objList;//BizActionObj.ItemMatserDetails;

        //                    //if (objList.Count > 1)
        //                    //{
        //                    //    cmbstore.SelectedItem = objList[1];
        //                    //    cmbSearchStore.SelectedItem = objList[1];
        //                    //}
        //                    //else
        //                    //{
        //                        cmbstore.SelectedItem = objList[0];
        //                        cmbSearchStore.SelectedItem = objList[0];
        //                    //}

        //                }


        //                //var result = from item in BizActionObj.ItemMatserDetails
        //                //             where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
        //                //             select item;

        //                //List<clsStoreVO> objList = (List<clsStoreVO>)result.ToList();
        //                //objList.Insert(0, new clsStoreVO { StoreName = " --Select-- " });

        //                //cmbSearchStore.ItemsSource = objList;
        //                //cmbstore.ItemsSource = objList;

        //                //cmbSearchStore.SelectedItem = objList[0];
        //                //cmbstore.SelectedItem = objList[0];







        //            }
        //            FillSupplier();

        //        };

        //        client.CloseAsync();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        private void FillStore()
        {

            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            //False when we want to fetch all items
            clsItemMasterVO obj = new clsItemMasterVO();
            obj.RetrieveDataFlag = false;
            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizActionObj.ToStoreList = new List<clsStoreVO>();
            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();
            List<clsStoreVO> objList = new List<clsStoreVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true };
                    // BizActionObj.ItemMatserDetails.Insert(0, Default);

                    BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
                    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                    //BizActionObj.ItemMatserDetails.Insert(0, Default);
                    // cmbFromStore.ItemsSource = BizActionObj.ItemMatserDetails;

                    //if (!((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    //{
                    //    var result = from item in BizActionObj.ItemMatserDetails
                    //                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                    //                 select item;
                    //    objList = (List<clsStoreVO>)result.ToList();
                    //}
                    //else
                    //{
                    //    var result1 = from item in BizActionObj.ItemMatserDetails
                    //                 where  item.Status == true
                    //                 select item;
                    //    //objList = BizActionObj.ItemMatserDetails;
                    //    objList = result1.ToList();
                    //}
                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true && item.IsQuarantineStore == false
                                 select item;
                    objList = (List<clsStoreVO>)result.ToList();
                    objList.Insert(0, Default);
                    BizActionObj.ToStoreList.Insert(0, Default);

                    var NonQSAndUserDefinedStores = from item in BizActionObj.ToStoreList.ToList()
                                                    where item.IsQuarantineStore == false
                                                    select item;

                    NonQSAndUserDefinedStores.ToList().Insert(0, Default);

                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        if (result.ToList().Count > 0)
                        {
                            cmbSearchStore.ItemsSource = result.ToList();
                            cmbSearchStore.SelectedItem = result.ToList()[0];
                            cmbstore.ItemsSource = result.ToList();
                            cmbstore.SelectedItem = result.ToList()[0];
                        }
                    }
                    else
                    {
                        if (NonQSAndUserDefinedStores != null)
                        {
                            cmbSearchStore.ItemsSource = NonQSAndUserDefinedStores.ToList();
                            cmbSearchStore.SelectedItem = NonQSAndUserDefinedStores.ToList()[0];
                            cmbstore.ItemsSource = NonQSAndUserDefinedStores.ToList();
                            cmbstore.SelectedItem = NonQSAndUserDefinedStores.ToList()[0];
                        }
                    }
                }
            };

            client.CloseAsync();

        }

        #endregion


        #region On Enquiry Button Click
        private void cmdEnquiry_Click(object sender, RoutedEventArgs e)
        {
            if (QuotationItems != null)
                QuotationItems.Clear();
            if (cmbstore.SelectedItem == null)
            {

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                               new MessageBoxControl.MessageBoxChildWindow("", "Please Select the Store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
            else if (((clsStoreVO)cmbstore.SelectedItem).StoreId == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                               new MessageBoxControl.MessageBoxChildWindow("", "Please Select the Store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }

            else if (cmbSupplier.SelectedItem == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                               new MessageBoxControl.MessageBoxChildWindow("", "Please Select the Supplier", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
            else if (((MasterListItem)cmbSupplier.SelectedItem).ID == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                            new MessageBoxControl.MessageBoxChildWindow("", "Please Select the Supplier", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
            else
            {
                EnquirySearch SearchWindow = new EnquirySearch();
                SearchWindow.storeID = ((clsStoreVO)cmbstore.SelectedItem).StoreId;
                SearchWindow.supplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;
                SearchWindow.cmbstore.IsEnabled = false;
                SearchWindow.cmbSupplier.IsEnabled = false;

                SearchWindow.onOKButton_Click += new RoutedEventHandler(SearchWindow_onOKButton_Click);
                SearchWindow.Show();
            }




        }
        #endregion


        #region Event Handler for Search Enquiry
        void SearchWindow_onOKButton_Click(object sender, RoutedEventArgs e)
        {
            EnquirySearch Enquiry = (EnquirySearch)sender;
            if (Enquiry.SelectedItems != null)
            {
                foreach (var item in Enquiry.SelectedItems)
                {
                    if (QuotationItems.Where(enquiryItems => enquiryItems.ItemID == item.ItemId).Any() == true)
                        QuotationItems.Select(enquiryItems => { enquiryItems.Quantity = enquiryItems.Quantity + item.Quantity; return enquiryItems; }).ToList();
                    else
                        QuotationItems.Add(new clsQuotationDetailsVO { ItemName = item.ItemName, ItemCode = item.ItemCode, Quantity = item.Quantity, ItemID = item.ItemId, PUM = item.UOM });
                }

                //grdQuotationdetails.ItemsSource = QuotationItems;
                grdQuotationdetails.Focus();
                grdQuotationdetails.UpdateLayout();

                EnqID = Enquiry.EnquiryID == null ? 0 : Enquiry.EnquiryID;


                cmbstore.SelectedValue = Enquiry.storeID;
                storeID = Enquiry.storeID;
                //((clsStoreVO)cmbstore.SelectedItem).StoreId = storeID;
                cmbstore.SelectedValue = Enquiry.storeID;
                //cmbstore.Selected = Enquiry.storeID;
            }
        }
        #endregion


        #region On Data Grid Cell Edit
        double orgTaxPercent = 0;
        void grdQuotationdetails_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.Column.DisplayIndex == 2)
            {
                if (((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).Quantity < 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                               new MessageBoxControl.MessageBoxChildWindow("", "Quantity Can not be Negative", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                    ((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).Quantity = 0;
                    return;
                }

                ((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).Quantity = Convert.ToSingle(System.Math.Round(((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).Quantity, 1));
                if (((int)((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).Quantity).ToString().Length > 5)
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx = null;
                    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity Length Should Not Be Greater Than 5 Digits.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    mgbx.Show();
                    ((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).Quantity = 1;
                    return;


                }
            }
            if (e.Column.DisplayIndex == 4)
            {
                if (((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).Rate < 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                               new MessageBoxControl.MessageBoxChildWindow("", "Cost Price Can not be Negative", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                    ((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).Rate = 0;
                    return;
                }
            }

            if (e.Column.DisplayIndex == 6)
            {
                if (((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).ExcisePercent > 100)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                               new MessageBoxControl.MessageBoxChildWindow("", "Excise Percentage Should Not Greater Than 100%", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                    ((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).ExcisePercent = 0;
                    ((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).ExciseAmount = 0;
                    return;
                }
                if (((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).ExcisePercent < 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                               new MessageBoxControl.MessageBoxChildWindow("", "Excise Percentage Should not be Negative", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                    ((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).ExcisePercent = 0;
                    return;
                }
            }
            if (e.Column.DisplayIndex == 8)
            {
                if (((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).TAXPercent > 100)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                               new MessageBoxControl.MessageBoxChildWindow("", "Tax Percentage Should Not  Greater Than 100%", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                    ((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).TAXPercent = 0;
                    return;
                }
                if (((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).TAXPercent < 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                               new MessageBoxControl.MessageBoxChildWindow("", "TAX Percentage Should Not  Negative", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                    ((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).TAXPercent = 0;
                    return;
                }
            }
            if (e.Column.DisplayIndex == 10)
            {
                if (((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).ConcessionPercent > 100)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                               new MessageBoxControl.MessageBoxChildWindow("", "Concession Percentage Should Not  Greater Than 100%", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                    ((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).ConcessionPercent = 0;
                    return;
                }
                if (((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).ConcessionPercent == 100)
                {
                    if (((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).TAXPercent != 0)
                    {
                        orgTaxPercent = ((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).TAXPercent;
                        ((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).TAXPercent = 0;
                    }
                }
                else
                {
                    if (((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).TAXPercent == 0)
                    {
                        ((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).TAXPercent = orgTaxPercent;
                    }
                }

                if (((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).ConcessionPercent < 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                               new MessageBoxControl.MessageBoxChildWindow("", "Concession Percentage Should Not  Negative", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                    ((clsQuotationDetailsVO)grdQuotationdetails.SelectedItem).ConcessionPercent = 0;
                    return;
                }
            }

            if (grdQuotationdetails.SelectedItem != null)
            {
                if (e.Column.DisplayIndex == 5 || e.Column.DisplayIndex == 7 || e.Column.DisplayIndex == 2 || e.Column.DisplayIndex == 3 || e.Column.DisplayIndex == 4
                   || e.Column.DisplayIndex == 6 || e.Column.DisplayIndex == 9 || e.Column.DisplayIndex == 8 || e.Column.DisplayIndex == 10)


                    CalculateOpeningBalanceSummary();
            }
        }

        #endregion


        #region calculate summary
        private void CalculateOpeningBalanceSummary()
        {
            double ExcAmount, TAXAmount, Amount, NetAmount, ConcessionAmount;

            ExcAmount = TAXAmount = Amount = NetAmount = ConcessionAmount = 0;

            foreach (var item in QuotationItems)
            {
                ExcAmount += item.ExciseAmount;
                TAXAmount += item.TAXAmount;
                Amount += item.Amount;
                NetAmount += item.NetAmount;
                ConcessionAmount += item.ConcessionAmount;
            }

            ((clsQuotaionVO)this.DataContext).TotalTAX = TAXAmount;
            ((clsQuotaionVO)this.DataContext).TotalAmount = Amount;
            ((clsQuotaionVO)this.DataContext).TotalConcession = ConcessionAmount;
            ((clsQuotaionVO)this.DataContext).TotalNet = NetAmount;
            //txtTotalAmount.Text = Amount.ToString();
            txtTotalConcession.Text = ConcessionAmount.ToString();
            //txtNETAmount.Text = NetAmount.ToString();
        }
        #endregion


        #region On Add Item Click
        private void lnkAddItems_Click(object sender, RoutedEventArgs e)
        {
            if (QuotationItems != null)
                QuotationItems.Clear();
            EnqID = 0;
            MessageBoxControl.MessageBoxChildWindow mgbx = null;
            if (cmbstore.SelectedItem == null)
            {
                cmbstore.TextBox.SetValidation("Please Select the Store");
                cmbstore.TextBox.RaiseValidationError();

                cmbstore.Focus();

            }
            else if (((clsStoreVO)cmbstore.SelectedItem).StoreId == 0)
            {
                cmbstore.TextBox.SetValidation("Please Select the Store");

                cmbstore.TextBox.RaiseValidationError();
                cmbstore.Focus();

            }
            else
            {
                ItemList Itemswin = new ItemList();
                Itemswin.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
                Itemswin.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                Itemswin.StoreID = ((clsStoreVO)cmbstore.SelectedItem).StoreId;
                Itemswin.cmbStore.IsEnabled = false;
                Itemswin.ShowBatches = false;
                Itemswin.OnSaveButton_Click += new RoutedEventHandler(Itemswin_OnSaveButton_Click);
                Itemswin.Show();
            }

        }
        #endregion


        #region Event Hnadler For Item Search
        void Itemswin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ItemList Itemswin = (ItemList)sender;

            if (Itemswin.SelectedItems != null)
            {


                foreach (var item in Itemswin.SelectedItems)
                {
                    //bool isExist = CheckForItemExistance(item.ID);
                    //if (!isExist)
                    //{

                    if (QuotationItems.Where(QuotationItems1 => QuotationItems1.ItemID == item.ID).Any() == false)
                    {
                        QuotationItems.Add(
                    new clsQuotationDetailsVO
                 {

                     ItemID = item.ID,
                     ItemName = item.ItemName,
                     ItemCode = item.ItemCode,
                     TAXPercent = (double)item.TotalPerchaseTaxPercent,

                     PUM = item.PUOM,
                     //SUOM = item.SUOM,
                     //InclusiveOfTax = item.InclusiveOfTax,
                     //EnableInclusiveOfTax = item.InclusiveOfTax == false ? true : false,
                     //StoreID = Itemswin.StoreID




                 });
                    }

                    //}


                }

                grdQuotationdetails.Focus();
                grdQuotationdetails.UpdateLayout();
                CalculateOpeningBalanceSummary();
                // dgAddGRNItems.SelectedIndex = GRNAddedItems.Count - 1;
            }
        }
        #endregion


        #region On Front Panel Quotation Grid Selection Change
        private void dgQuotationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                grdQuotation.ItemsSource = null;
                if (dgQuotationList.SelectedIndex != -1)
                {



                    clsGetQuotationDetailsBizActionVO objBizActionVO = new clsGetQuotationDetailsBizActionVO();
                    clsQuotaionVO objList = (clsQuotaionVO)dgQuotationList.SelectedItem;

                    objBizActionVO.SearchQuotationID = objList.ID;
                    objBizActionVO.IsPagingEnabled = true;
                    objBizActionVO.StartIndex = 0;
                    objBizActionVO.MinRows = 20;
                    objBizActionVO.UnitID = objList.UnitId;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {
                                clsGetQuotationDetailsBizActionVO obj = ((clsGetQuotationDetailsBizActionVO)arg.Result);
                                grdQuotation.ItemsSource = null;
                                grdQuotation.ItemsSource = obj.QuotaionList;

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

                    Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        #region On Search Button Click
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FillQuotationDataGrid();
        }
        #endregion

        private void cmdDeleteQuotationdetails_Click(object sender, RoutedEventArgs e)
        {
            if (grdQuotationdetails.SelectedItem != null)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Delete the selected Item ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        QuotationItems.RemoveAt(grdQuotationdetails.SelectedIndex);
                        CalculateOpeningBalanceSummary();
                    }
                };

                msgWD.Show();
            }
        }

        private void cmbstore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbstore.SelectedItem != null)
                {
                    if (QuotationItems != null)
                        QuotationItems.Clear();
                }
            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        private void txtExpirationdays_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() || Convert.ToInt64(((TextBox)sender).Text) <= 0 && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void AttachDoc_Click(object sender, RoutedEventArgs e)
        {
            if (dgQuotationList.SelectedItem != null)
            {
                QuotationAttachment win = new QuotationAttachment();
                win.Title = "Quotation No:" + ((clsQuotaionVO)dgQuotationList.SelectedItem).QuotationNo;//((IApplicationConfiguration)App.Current).SelectedPatient.FirstName + " " + ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
                win.QuotationID = Convert.ToInt64(((clsQuotaionVO)dgQuotationList.SelectedItem).QuotationNo);
                win.QuotationValidity = ((clsQuotaionVO)dgQuotationList.SelectedItem).ValidityDate;
                win.LeadTime = ((clsQuotaionVO)dgQuotationList.SelectedItem).LeadTime;
                win.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Quotation", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

    }
}
