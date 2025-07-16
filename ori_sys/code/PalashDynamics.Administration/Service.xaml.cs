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
using PalashDynamics.Collections;
using System.Reflection;
namespace PalashDynamics.Administration
{
   
    public partial class Service : UserControl
    {
        bool Edit = false;
        #region Public Variables

        public List<long> lstTariffs = new List<long>();
        private SwivelAnimation objAnimation;
        public long pkServiceID { get; set; }
        public Boolean isPageLoaded = false;
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        public bool isView = false;
        public string msgText;
        public string msgTitle;
        bool IsCancel = true;
        #endregion

        #region Pagging

        public PagedSortableCollectionView<clsServiceMasterVO> DataList { get; private set; }

        public int DataListPageSize
        {
            get { return DataList.PageSize; }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
            }
        }

        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            BindServiceListGrid();
        }

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        /// 

        #endregion

        public Service()
        {
            InitializeComponent();
            this.DataContext = new clsServiceMasterVO();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            #region Pagging

            DataList = new PagedSortableCollectionView<clsServiceMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;

            #endregion
        }

        private void CmdNew_Click(object sender, RoutedEventArgs e)
        {
          
            objAnimation.Invoke(RotationType.Forward);
            EmptyUI();
            ValidateUI();
           
            this.DataContext = new clsServiceMasterVO();
           
            FillTariffList();
            ((clsServiceMasterVO)this.DataContext).EditMode = false;
            //((clsServiceMasterVO)this.DataContext).ServiceCode = "Auto Service Code";
            //txtServiceCode.Text = ((clsServiceMasterVO)this.DataContext).ServiceCode;
            SetCommandButtonState("New");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //pnlApplicableToDoctors.Visibility = Visibility.Collapsed;
            //pnlRateEditable.Visibility = Visibility.Collapsed;
            if (!isPageLoaded)
            {
                FillCodeType();
                FillSpecialization();
                FillTariffList();
                BindServiceListGrid();
                EditMode = false;
                isPageLoaded = true;
                SetCommandButtonState("Load");
            }           
        }

        private void FillTariffList()
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

                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                        lstTariff.ItemsSource = null;
                        lstTariff.ItemsSource = objList;

                        if (isView == true)
                        {
                            BindTariffApplicable(pkServiceID);
                            isView = false;
                        }
                            
                    }
                };

                client.ProcessAsync(BizAction, User); // new clsUserVO());
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

                                    cboSpecialization1.ItemsSource = null;
                                    cboSpecialization1.ItemsSource = objList;
                                    cboSpecialization1.SelectedItem = objList[0];
                                    cboSpecialization.ItemsSource = null;
                                    cboSpecialization.ItemsSource = objList;
                                    cboSpecialization.SelectedItem = objList[0];
                                }

                                if (this.DataContext != null)
                                {
                                    cboSpecialization1.SelectedValue = ((clsServiceMasterVO)this.DataContext).Specialization;
                                }
                    };

                    client.ProcessAsync(BizAction, User); //new clsUserVO());
                    client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
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
            client.ProcessAsync(BizAction, User); //new clsUserVO());
            client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
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
                txtMaxRate.IsEnabled = true;
                txtMinRate.IsEnabled = true;
            }
            else
            {
                txtMaxRate.IsEnabled = false;
                txtMinRate.IsEnabled = false;

                txtMaxRate.Text = "0.00";
                txtMinRate.Text = "0.00";

            }
        }

        private void cboSpecialization1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboSpecialization1.SelectedItem != null && ((MasterListItem)cboSpecialization1.SelectedItem).ID != 0)
            {
                FillSubSpecialization(((MasterListItem)cboSpecialization1.SelectedItem).ID.ToString());
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
                    cboSubSpecialization1.ItemsSource = null;
                    cboSubSpecialization1.ItemsSource = objList;
                    cboSubSpecialization1.SelectedValue = objList[0].ID;
                    //cboSubSpecialization1.SelectedItem = objList;
                    //cboSubSpecialization1.SelectedValue=;
                }

                if (this.DataContext != null)
                {
                    cboSubSpecialization1.SelectedValue = ((clsServiceMasterVO)this.DataContext).SubSpecialization;
                }
            };

            client.ProcessAsync(BizAction, User);// new clsUserVO());
            client.CloseAsync();
        }
        public clsAddServiceMasterBizActionVO objBizActionVO;
        /// <summary>
        /// Save Service Master Details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            if(!Edit)
             IsNew = true;

            bool ValidationStatus = ValidateUI();

            if (ValidationStatus == true)
            {
                if ((MasterListItem)cboSpecialization1.SelectedItem == null)
                {
                    msgText = "Specialization is required";
                    MessageBoxControl.MessageBoxChildWindow msg2 =
                      new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);

                    msg2.Show();

                    cboSpecialization1.TextBox.Focus();

                }
                else if (((MasterListItem)cboSpecialization1.SelectedItem).ID == 0)
                {
                    msgText = "Specialization is required";
                    MessageBoxControl.MessageBoxChildWindow msg3 =
                      new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);

                    msg3.Show();

                    cboSpecialization1.TextBox.Focus();

                }
                else
                {
                    msgText = "Are you sure you want to Save the Record";
                    MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                      new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowSave_OnMessageBoxClosed);

                    msgWindowUpdate.Show();
                }
            }
        }

        private void msgWindowSave_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                //waiting.Show();
                if (CheckDuplicasy())
                {
                    Save();
                    SetCommandButtonState("Save");          
                }
               // waiting.Close();
                    
            }
            else
            {
            }
        }

        private void Save()
        {
            try
            {

                if (ValidateUI())
                {

                    objBizActionVO = new clsAddServiceMasterBizActionVO();
                    objBizActionVO.ServiceMasterDetails = new clsServiceMasterVO();
                    objBizActionVO.ServiceMasterDetails.EditMode = ((clsServiceMasterVO)this.DataContext).EditMode;
                    objBizActionVO.ServiceMasterDetails.ServiceCode = ((clsServiceMasterVO)this.DataContext).ServiceCode;
                    objBizActionVO.ServiceMasterDetails.ID = pkServiceID == null ? 0 : pkServiceID;
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

                    // objBizActionVO.ServiceMasterDetails.OutSource = ((clsServiceMasterVO)this.DataContext).OutSource;
                    objBizActionVO.ServiceMasterDetails.InHouse = ((clsServiceMasterVO)this.DataContext).InHouse;
                    objBizActionVO.ServiceMasterDetails.DoctorShare = ((clsServiceMasterVO)this.DataContext).DoctorShare;
                    objBizActionVO.ServiceMasterDetails.DoctorSharePercentage = ((clsServiceMasterVO)this.DataContext).DoctorSharePercentage;
                    objBizActionVO.ServiceMasterDetails.DoctorShareAmount = txtDoctorApplicableAmount.Text == "" ? 0 : decimal.Parse(txtDoctorApplicableAmount.Text);
                    objBizActionVO.ServiceMasterDetails.DoctorSharePercentage = txtDoctorApplicablePercent.Text == "" ? 0 : decimal.Parse(txtDoctorApplicablePercent.Text);
                    objBizActionVO.ServiceMasterDetails.RateEditable = ((clsServiceMasterVO)this.DataContext).RateEditable;
                    objBizActionVO.ServiceMasterDetails.MaxRate = txtMaxRate.Text == "" ? 0 : decimal.Parse(txtMaxRate.Text);
                    objBizActionVO.ServiceMasterDetails.MinRate = txtMinRate.Text == "" ? 0 : decimal.Parse(txtMinRate.Text);
                    objBizActionVO.ServiceMasterDetails.Rate = txtServiceRate.Text == "" ? 0 : decimal.Parse(txtServiceRate.Text);
                    objBizActionVO.ServiceMasterDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    objBizActionVO.ServiceMasterDetails.Status = true;
                    objBizActionVO.ServiceMasterDetails.CheckedAllTariffs = chkAllTariffs.IsChecked == true ? true : false;
                    objBizActionVO.ServiceMasterDetails.CreatedUnitID = 1;
                    objBizActionVO.ServiceMasterDetails.UpdatedUnitID = 1;
                    objBizActionVO.ServiceMasterDetails.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    objBizActionVO.ServiceMasterDetails.AddedOn = DateTime.Now.DayOfWeek.ToString();
                    objBizActionVO.ServiceMasterDetails.AddedDateTime = DateTime.Now;

                    objBizActionVO.ServiceMasterDetails.IsPackage = ((clsServiceMasterVO)this.DataContext).IsPackage;

                    objBizActionVO.ServiceMasterDetails.AddedWindowsLoginName = "palash";

                    //List<MasterListItem> ObjMasterList = new List<MasterListItem>();
                    //ObjMasterList = (List<MasterListItem>)lstTariff.ItemsSource;
                    //foreach (var item in ObjMasterList)
                    //{
                    //    if(item.Status==true)
                    //        objBizActionVO.ServiceMasterDetails.TariffIDList.Add(item.ID);
                    //}
                    objBizActionVO.ServiceMasterDetails.TariffIDList = lstTariffs;

                    //**********************Inserting Service Master***********************************//

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            //if (((clsServiceMasterVO)this.DataContext).EditMode == true)
                            //{
                            //    //clsAddServiceMasterBizActionVO objAddServiceMasterBizActionVO = (clsAddServiceMasterBizActionVO)arg.Result;
                            //    //List<MasterListItem> objlist = (List<MasterListItem>)lstTariff.ItemsSource;
                            //    //updateServiceTariffMaster(pkServiceID, objlist, objAddServiceMasterBizActionVO);

                            //    //Added By Pallavi
                            //    clsAddServiceMasterBizActionVO objAddServiceMasterBizActionVO = (clsAddServiceMasterBizActionVO)arg.Result;
                            //    List<MasterListItem> objlist = (List<MasterListItem>)lstTariff.ItemsSource;
                            //    updateServiceTariffMaster1(pkServiceID,lstTariffs, objAddServiceMasterBizActionVO);
                            //}
                            //else
                            //{
                            //    long ServiceID = ((clsAddServiceMasterBizActionVO)(arg.Result)).ServiceID;
                            //    clsAddServiceMasterBizActionVO objAddServiceMasterBizActionVO = (clsAddServiceMasterBizActionVO)arg.Result;
                            //    //******************* Fill Service Tariff Master ***************************
                            //    FillServiceTariffMaster(ServiceID, lstTariffs, objAddServiceMasterBizActionVO);


                            //}





                            //long ServiceID = ((clsAddServiceMasterBizActionVO)(arg.Result)).ServiceID;
                            //clsAddServiceMasterBizActionVO objAddServiceMasterBizActionVO = (clsAddServiceMasterBizActionVO)arg.Result;
                            ////******************* Fill Service Tariff Master ***************************
                            //FillServiceTariffMaster(ServiceID, lstTariffs, objAddServiceMasterBizActionVO);
                            string msgTitle = "";
                            string msgText = "Service Added Successfully";

                            MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);


                            msgW.Show();
                            objAnimation.Invoke(RotationType.Backward);
                            BindServiceListGrid();
                            SetCommandButtonState("Load");




                        }
                        else
                        {
                            string msgTitle = "";
                            string msgText = "Some error occurred while saving";

                            MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);


                            msgW.Show();
                            SetCommandButtonState("New");
                        }
                    };
                    client.ProcessAsync(objBizActionVO, User); //new clsUserVO());
                    client.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                                                                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindowUpdate.Show();

              
            }
           
        }

        private bool ValidateUI()
        {

            bool result = true;
            try
            {

                // Modified By Yogita
                if (((MasterListItem)cboSpecialization1.SelectedItem).ID == 0)
                {
                    cboSpecialization1.SetValidation("Specialization is required");
                    cboSpecialization1.RaiseValidationError();
                    cboSpecialization1.Focus();
                    result = false;
                }
                else if (((MasterListItem)cboSpecialization1.SelectedItem) == null)
                {
                    cboSpecialization1.SetValidation("Specialization is required");
                    cboSpecialization1.RaiseValidationError();
                    cboSpecialization1.Focus();
                    result = false;
                }
                
                else
                    cboSpecialization1.TextBox.ClearValidationError();

                 //End
                if (string.IsNullOrEmpty(txtServiceRate.Text.Trim()))
                {
                    txtServiceRate.SetValidation("Service Rate is required");
                    txtServiceRate.RaiseValidationError();
                    txtServiceRate.Focus();
                    result = false;
                }
                else if (Convert.ToDecimal(txtServiceRate.Text.Trim()) == 0)
                    {
                        txtServiceRate.SetValidation("Service Rate can't be zero");
                        txtServiceRate.RaiseValidationError();
                        txtServiceRate.Focus();
                        result = false;
                    }
                else
                {
                    txtServiceRate.ClearValidationError();
                    //txtServiceRate.is
                }
                if (string.IsNullOrEmpty(txtServiceName.Text.Trim()))
                {
                    txtServiceName.SetValidation("Service Name is required");
                    txtServiceName.RaiseValidationError();
                    txtServiceName.Focus();
                    result = false;
                }
                else
                    txtServiceName.ClearValidationError();

              



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

                                txtMinRate.SetValidation(" Min Rate Amount should be number");
                                txtMinRate.RaiseValidationError();
                                result = false;

                            }
                        }

                        else
                        {
                            txtMinRate.ClearValidationError();
                        }


                        if (txtMaxRate.Text != "")
                        {
                            if (Extensions.IsItDecimal(txtMaxRate.Text) == true)
                            {

                                txtMaxRate.SetValidation(" Max Rate Amount should be number");
                                txtMaxRate.RaiseValidationError();
                                result = false;

                            }
                        }

                        else
                        {
                            txtMaxRate.ClearValidationError();
                        }
                    }
                    catch (Exception Ex)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW =
                                                                                      new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Min. Rate or max Rate", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW.Show();
                        txtServiceRate.Text = "0.00";
                        txtMinRate.Text = "0.00";
                        txtMaxRate.Text = "0.00";


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

                if (chkRateEditable.IsChecked == true)
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

                    if (!txtMinRate.Text.Equals("") && (txtMinRate.Text != "0"))
                    {
                        if (!txtMaxRate.Text.Equals("") && (txtMaxRate.Text != "0"))
                        {
                            if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                            {
                                try
                                {
                                    if ((Convert.ToDecimal(txtServiceRate.Text) > Convert.ToDecimal(txtMaxRate.Text)) || (Convert.ToDecimal(txtServiceRate.Text) < Convert.ToDecimal(txtMinRate.Text)))
                                    {
                                        MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                                                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Service Rate Must Be In Between Min. Rate & Max. Rate!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        msgWindowUpdate.Show();

                                        result = false;
                                    }
                                }
                                catch (Exception Ex)
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                                                                                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter min. rate max. rate correctly", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgWindowUpdate.Show();

                                    result = false;
                                }
                            }

                        }
                    }

                }

            }
            catch (Exception Ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                                                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindowUpdate.Show();

                result = false;
            }
           
            return result;
        }

        /// <summary>
       
        /// </summary>
        /// <param name="pkServiceID">Service ID</param>
        /// <param name="objlist">This contains tariffs applied at the time of saving </param>
        /// <param name="objAddServiceMasterBizActionVO"></param>
        //private void updateServiceTariffMaster(long pkServiceID, List<MasterListItem> objlist, clsAddServiceMasterBizActionVO objAddServiceMasterBizActionVO)
        //{
        //    try
        //    {
        //        clsGetTariffServiceBizActionVO objBizActionVO = new clsGetTariffServiceBizActionVO();
        //        objBizActionVO.ServiceMaster = new clsServiceMasterVO();
        //        objBizActionVO.ServiceMaster.ServiceID = pkServiceID;
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                
        //        client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null && arg.Result != null)
        //            {
        //                if (((clsServiceMasterVO)this.DataContext).EditMode == true)
        //                {
        //                      List<clsServiceMasterVO> objTariffIDList=  ((clsGetTariffServiceBizActionVO)(arg.Result)).ServiceList;
        //                      string TariffIDs = "0";
        //                      if (objTariffIDList != null)
        //                      {
        //                          foreach (clsServiceMasterVO item in objTariffIDList)
        //                          {
        //                              TariffIDs = TariffIDs + (TariffIDs == "" ? item.TariffServiceMasterID.ToString() : "," + item.TariffServiceMasterID.ToString());
        //                          }
        //                      }        
        //                      clsDeletetTriffServiceClassRateDetailsBizActionVO objDeletetTriffServiceClassRate = new clsDeletetTriffServiceClassRateDetailsBizActionVO();

        //                      objDeletetTriffServiceClassRate.ServiceMasterDetails = new clsServiceMasterVO();

        //                      objDeletetTriffServiceClassRate.ServiceMasterDetails.TariffIDs = TariffIDs;
        //                      DeleteAllTariffServiceClassRateDetails(objDeletetTriffServiceClassRate, pkServiceID, objAddServiceMasterBizActionVO, objlist);                          
        //                 }
        //                else
        //                {
        //                }
        //            }
        //            else
        //            {
        //            }
        //        };
        //        client.ProcessAsync(objBizActionVO, User); //new clsUserVO());
        //        client.CloseAsync();
        //    }
        //    catch (Exception)
        //    {                
        //        throw;
        //    }
        //}
       // Added by pallavi
        private void updateServiceTariffMaster1(long pkServiceID, List<long> objlist,clsAddServiceMasterBizActionVO objAddServiceMasterBizActionVO)
        {
            try
            {
                clsUpdateServiceTariffMasterBizActionVO objVo = new clsUpdateServiceTariffMasterBizActionVO();
                objVo.ServiceMasterDetails = objAddServiceMasterBizActionVO.ServiceMasterDetails;
                objVo.ServiceMasterDetails.ServiceID = pkServiceID;
                objVo.ServiceMasterDetails.TariffIDList = objlist;


                objVo.ServiceID = pkServiceID;
                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        //UpdateTariffServiceMaster1();
                    }
                    else
                    {
                    }
                };
                client.ProcessAsync(objVo, User); //new clsUserVO());
                client.CloseAsync();
        
            }
            catch (Exception)
            {
                throw;
            }


            
        }

        private void DeleteAllTariffServiceClassRateDetails(clsDeletetTriffServiceClassRateDetailsBizActionVO objDeletetTriffServiceClassRate, long pkServiceID, clsAddServiceMasterBizActionVO objAddServiceMasterBizActionVO, List<MasterListItem> objlist)
        {
            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);

            client1.ProcessCompleted += (s1, arg1) =>
            {
                if (arg1.Error == null && arg1.Result != null)
                {
                    if (((clsServiceMasterVO)this.DataContext).EditMode == true)
                    {
                        DeleteServiceTariffMaster(pkServiceID, objAddServiceMasterBizActionVO, objlist);
                    }
                }
                else
                {
                }
            };
            client1.ProcessAsync(objDeletetTriffServiceClassRate, User); //new clsUserVO());
            client1.CloseAsync();
        }
             

        private void DeleteServiceTariffMaster(long pkServiceID, clsAddServiceMasterBizActionVO objAddServiceMasterBizActionVO, List<MasterListItem> objlist)
        {
            clsDeleteTariffServiceAndServiceTariffBizActionVO objAddServiceTariff = new clsDeleteTariffServiceAndServiceTariffBizActionVO();
           
            objAddServiceTariff.ServiceMasterDetails = new clsServiceMasterVO();
            objAddServiceTariff.ServiceMasterDetails.ServiceID = pkServiceID;
           
            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);

            client1.ProcessCompleted += (s1, arg1) =>
            {
                if (arg1.Error == null && arg1.Result != null)
                {
                    if (((clsServiceMasterVO)this.DataContext).EditMode == true)
                    {
                        FillServiceTariffMasterAfterEdit(pkServiceID, objlist, objAddServiceMasterBizActionVO);
                    }
                }
                else
                {
                }
            };
            client1.ProcessAsync(objAddServiceTariff, User);// new clsUserVO());
            client1.CloseAsync();
        }

        private void FillServiceTariffMasterAfterEdit(long pkServiceID, List<MasterListItem> objlist, clsAddServiceMasterBizActionVO objAddServiceMasterBizActionVO)
        {
            clsAddServiceTariffBizActionVO objBizActionVO = new clsAddServiceTariffBizActionVO();
            objBizActionVO.ServiceMasterDetails = objAddServiceMasterBizActionVO.ServiceMasterDetails;
            objBizActionVO.ServiceMasterDetails.TariffIDList = lstTariffs;
            objBizActionVO.ServiceMasterDetails.ServiceID = pkServiceID;
           
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                        //*************************Fill Tariff Serevice Master*****************
                        FillTariffServiceMaster(pkServiceID, lstTariffs, objAddServiceMasterBizActionVO);
                }
                else
                {
                }
            };
            client.ProcessAsync(objBizActionVO, User);// new clsUserVO());
            client.CloseAsync();
        }

        private void UpdateTariffService(long pkServiceID, List<MasterListItem> objlist, clsAddServiceMasterBizActionVO objAddServiceMasterBizActionVO, long TariffserviceID)
        {
            clsAddTariffServiceBizActionVO objBizActionVO = new clsAddTariffServiceBizActionVO();
            objBizActionVO.ServiceMasterDetails = objAddServiceMasterBizActionVO.ServiceMasterDetails;

            objBizActionVO.Tariffs = objlist;
            objBizActionVO.ServiceMasterDetails.ServiceCode = objAddServiceMasterBizActionVO.ServiceMasterDetails.ServiceCode;
            objBizActionVO.ServiceMasterDetails.ServiceID = pkServiceID;
            objBizActionVO.ServiceMasterDetails.ID = TariffserviceID;
          
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsServiceMasterVO)this.DataContext).EditMode == true)
                    {
                    }
                    else
                    {
                        string msgTitle = "";
                        string msgText = "Service Added Successfully";

                        MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);
                        
                        msgW.Show();
                        objAnimation.Invoke(RotationType.Backward);
                        BindServiceListGrid();
                    }
                }
                else
                {
                }
            };
            client.ProcessAsync(objBizActionVO, User); //new clsUserVO());
            client.CloseAsync();
        }

      
        /// <summary>
        /// Insert record in to Service Tariff Master
        /// </summary>
        /// <param name="ServiceID"></param>
        /// <param name="lstTariffs">This contains list of tariff applied to service</param>
        /// <param name="obj">Object of clsAddServiceMasterBizActionVO contains details of service master</param>
        private void FillServiceTariffMaster(long ServiceID, List<long> lstTariffs,clsAddServiceMasterBizActionVO obj)
        {
            clsAddServiceTariffBizActionVO objBizActionVO = new clsAddServiceTariffBizActionVO();

            objBizActionVO.ServiceMasterDetails = obj.ServiceMasterDetails;
            objBizActionVO.ServiceMasterDetails.TariffIDList = lstTariffs;
            objBizActionVO.ServiceMasterDetails.ServiceID = ServiceID;
            
          
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsServiceMasterVO)this.DataContext).EditMode == true)
                    {
                        FillTariffServiceMaster(ServiceID, lstTariffs, obj);  
                    }
                    else
                    {                       
                        //*************************Fill Tariff Serevice Master*****************
                        FillTariffServiceMaster(ServiceID, lstTariffs, obj);                       
                    }
                }
                else
                {
                }
            };
            client.ProcessAsync(objBizActionVO, User); //new clsUserVO());
            client.CloseAsync();            
        }
       

      

        /// <summary>
        /// Insert record in Tariff Service Master
        /// </summary>
        /// <param name="ServiceID"></param>
        /// <param name="lstTariffs">Contains details of tariffs applied to the service</param>
        /// <param name="obj"></param>
        private void FillTariffServiceMaster(long ServiceID, List<long> lstTariffs,clsAddServiceMasterBizActionVO obj)
        {          
            clsAddTariffServiceBizActionVO _objBizActionVO = new clsAddTariffServiceBizActionVO();
            _objBizActionVO.ServiceMasterDetails = obj.ServiceMasterDetails;

            _objBizActionVO.TariffList = lstTariffs;
            _objBizActionVO.ServiceMasterDetails.ServiceCode = obj.ServiceMasterDetails.ServiceCode;
            _objBizActionVO.ServiceMasterDetails.ServiceID = ServiceID;
            _objBizActionVO.TariffServiceForm = false;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsServiceMasterVO)this.DataContext).EditMode == true)
                    {
                        string msgTitle = "";
                        string msgText = "Service Updated Successfully";

                        MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);
                        lstTariffs.Clear();

                        msgW.Show();
                        objAnimation.Invoke(RotationType.Backward);
                        BindServiceListGrid();
                    }
                    else
                    {
                        string msgTitle = "";
                        string msgText = "Service Added Successfully";

                        MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);
                        lstTariffs.Clear();

                        msgW.Show();
                        objAnimation.Invoke(RotationType.Backward);
                        BindServiceListGrid();
                    }
                }
                else
                {
                }
            };
            client.ProcessAsync(_objBizActionVO, User); //new clsUserVO());
            client.CloseAsync();
        }

        private void BindServiceListGrid()
        {
            try
            {
                WaitIndicator obj = new WaitIndicator();
                obj.Show();

                clsGetServiceMasterListBizActionVO BizActionObj = new clsGetServiceMasterListBizActionVO();
                
                BizActionObj.GetAllServiceListDetails = true;
                BizActionObj.ServiceList = new List<clsServiceMasterVO>();
                
                BizActionObj.IsPagingEnabled = true;
                BizActionObj.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                BizActionObj.MaximumRows = DataList.PageSize;
                                
                BizActionObj.ServiceName = txtSearchCriteria.Text;
                if (cboSpecialization.SelectedItem==null)
                {
                    BizActionObj.Specialization = 0;    
                }
                else
                {
                    BizActionObj.Specialization = ((MasterListItem)cboSpecialization.SelectedItem).ID == null ? 0 : ((MasterListItem)cboSpecialization.SelectedItem).ID;     //(long) cboSpecialization.SelectedItem; //((clsServiceMasterVO)this.DataContext).Specialization;
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetServiceMasterListBizActionVO result = args.Result as clsGetServiceMasterListBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        if (result.ServiceList != null)
                        {
                            DataList.Clear();
                            //foreach (var item in result.ServiceList)
                            //{
                            //    DataList.Add(item);
                            //}
                            foreach (var item in result.ServiceList)
                            {
                                DataList.Add(item);
                            }

                            grdServices.ItemsSource = null;
                            grdServices.ItemsSource = DataList;

                            dgDataPager.Source = null;
                            dgDataPager.PageSize = BizActionObj.MaximumRows;
                            dgDataPager.Source = DataList;   
                        }
                        //grdServices.ItemsSource = null;
                        //grdServices.ItemsSource = ((clsGetServiceMasterListBizActionVO)args.Result).ServiceList;
                        obj.Close();
                    }
                };
                client.ProcessAsync(BizActionObj, User); //new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
      
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            try
            {
                if (result == MessageBoxResult.Yes)
                {
                }
                else
                {
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            EmptyUI();
        }

        private void EmptyUI()
        {
            clsServiceMasterVO obj = new clsServiceMasterVO();
            txtServiceCode.Text = "";
            txtCode.Text = "";
            txtServiceLongDescription.Text = "";
            txtServiceName.Text = "";
            txtServiceRate.Text = "";
            txtServiceShortDescription.Text = "";
            txtMinRate.Text = "";
            txtMaxRate.Text = "";
            txtDoctorApplicableAmount.Text = "";
            txtDoctorApplicablePercent.Text = "";
            cboCodeType.SelectedValue = obj.CodeType;
            cboSpecialization1.SelectedValue = obj.Specialization;
            cboSubSpecialization1.SelectedValue = obj.SubSpecialization;
            chkApplicableToAllDoctors.IsChecked = obj.DoctorShare;
            chkConcession.IsChecked = obj.Concession;
            txtConcessionAmount.Text = "";
            txtConcessionAmount.IsEnabled = false;
            txtConcessionPercentage.Text = "";
            txtConcessionPercentage.IsEnabled = false;
            //chkIServiceActive.IsChecked = true;
            chkAllTariffs.IsChecked = obj.CheckedAllTariffs;
            chkInHouse.IsChecked = obj.InHouse;
            IsPackage.IsChecked = obj.IsPackage;
            chkRateEditable.IsChecked = obj.RateEditable;
            txtMinRate.IsEnabled = false;
            txtMaxRate.IsEnabled = false;
            chkServiceTax.IsChecked = obj.ServiceTax;
            txtServiceTaxAmount.Text = "";
            txtServiceTaxAmount.IsEnabled = false;
            txtServiceTaxPercentage.Text = "";
            txtServiceTaxPercentage.IsEnabled = false;
            chkStaffDiscount.IsChecked = obj.StaffDependantDiscount;
            txtStaffDiscountAmount.Text = "";
            txtStaffDiscountAmount.IsEnabled = false;
            txtStaffDiscountPercentage.Text = "";
            txtStaffDiscountPercentage.IsEnabled = false;
            chkStaffParentDiscount.IsChecked = obj.StaffDependantDiscount;
            txtStaffParentAmount.Text = "";
            txtStaffParentPercentage.IsEnabled = false;
            txtStaffParentPercentage.Text = "";
            txtStaffParentAmount.IsEnabled = false;
            txtServiceName.ClearValidationError();
            txtServiceRate.ClearValidationError();
            txtDoctorApplicablePercent.IsEnabled = false;
            txtDoctorApplicableAmount.IsEnabled = false;
            IsNew = false;
            Edit = false;
        }

        private void chkTariff_Checked(object sender, RoutedEventArgs e)
        {
            if (EditMode!=true)
            {
                CheckBox objCheckBox =(CheckBox) sender;

                if (objCheckBox.Tag!=null)
                {
                    long tag = (long)objCheckBox.Tag;
                    lstTariffs.Add(tag);    
                } 
            }          
        }

        private void chkTariff_Unchecked(object sender, RoutedEventArgs e)
        {
            if (EditMode != true)
            {
                if (chkAllTariffs.IsChecked == true)
                    chkAllTariffs.IsChecked = false;
                    CheckBox objCheckBox = (CheckBox)sender;
                    if (objCheckBox.Tag!=null)
                    {
                        long tag = (long)objCheckBox.Tag;
                        lstTariffs.Remove(tag);
                    }
            }
        }
              
        private void grdServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
        }

        private void GetServiceClassRateDetail(long pkServiceID)
        {
            try
            {
                clsGetServiceMasterListBizActionVO objSeviceMaster = new clsGetServiceMasterListBizActionVO();
                objSeviceMaster.ServiceMaster = new clsServiceMasterVO();
                objSeviceMaster.ServiceMaster.ID = pkServiceID;
                objSeviceMaster.GetAllServiceClassRateDetails = true;
                
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetServiceMasterListBizActionVO obj = args.Result as clsGetServiceMasterListBizActionVO;
                        if (obj!=null)
                        {
                            txtServiceRate.Text = obj.ServiceMaster.Rate.ToString();    
                        }
                    }
                };
                client.ProcessAsync(objSeviceMaster, User); //new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void BindTariffApplicable(long pkServiceID)
        {
            try
            {
                clsGetServiceMasterListBizActionVO objSeviceMaster = new clsGetServiceMasterListBizActionVO();
                objSeviceMaster.GetAllTariffIDDetails = true;
                objSeviceMaster.ServiceMaster = new clsServiceMasterVO();
                objSeviceMaster.ServiceMaster.ServiceID = pkServiceID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetServiceMasterListBizActionVO obj = args.Result as clsGetServiceMasterListBizActionVO;

                        List<clsServiceMasterVO> objServiceList = obj.ServiceList;
                        List<MasterListItem> objlist = (List<MasterListItem>)lstTariff.ItemsSource;
                        if (objServiceList != null)
                        {
                            if (objlist != null)
                            {
                                foreach (MasterListItem item in objlist)
                                {
                                    foreach (clsServiceMasterVO items in objServiceList)
                                    {
                                        if (item.ID == items.ID)
                                        {
                                            item.Status = items.ServiceTariffMasterStatus;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                };
                client.ProcessAsync(objSeviceMaster, User); //new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {

                throw;
            }
          



        }

        public bool EditMode { get; set; }
                
        private void CmdCancel1_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
           objAnimation.Invoke(RotationType.Backward);
           lstTariff.ItemsSource = null;
           lstTariffs.Clear();

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
           }
        
        }

        decimal PrevServiceRate = 0;
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            IsNew = false;
            Edit = true;
            clsServiceMasterVO objServiceVO = new clsServiceMasterVO();
            objServiceVO = (clsServiceMasterVO)grdServices.SelectedItem;
            clsGetServiceMasterListBizActionVO objVo = new clsGetServiceMasterListBizActionVO();
            objVo.GetAllServiceMasterDetailsForID = true;
            objVo.ServiceMaster = new clsServiceMasterVO();
            objVo.ServiceMaster.ID = objServiceVO.ID;
            objVo.ServiceMaster.UnitID = objServiceVO.UnitID;

            EditMode = true;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    clsGetServiceMasterListBizActionVO obj = args.Result as clsGetServiceMasterListBizActionVO;
                    if (obj != null)
                    {
                        this.DataContext = obj.ServiceMaster;
                        ((clsServiceMasterVO)this.DataContext).EditMode = true;
                        if (objServiceVO != null)
                        {
                            objServiceVO.EditMode = true;
                            
                            cboCodeType.SelectedValue = objServiceVO.CodeType;
                            //LongDescription.Text = objServiceVO.LongDescription;
                            //ServiceShortDescription.Text = objServiceVO.ShortDescription;

                            cboSpecialization1.SelectedValue = objServiceVO.Specialization;
                            FillSubSpecialization(objServiceVO.Specialization.ToString());
                            cboSubSpecialization1.SelectedValue = objServiceVO.SubSpecialization;
                            if (obj.ServiceMaster.Concession == true)
                            {
                                txtConcessionAmount.IsEnabled = true;
                                txtConcessionPercentage.IsEnabled = true;
                            }
                            else
                            {
                                txtConcessionAmount.IsEnabled = false;
                                txtConcessionPercentage.IsEnabled = false;
                            }
                            if (obj.ServiceMaster.DoctorShare == true)
                            {
                                txtDoctorApplicableAmount.IsEnabled = true;
                                txtDoctorApplicablePercent.IsEnabled = true;
                            }
                            else
                            {
                                txtDoctorApplicableAmount.IsEnabled = false;
                                txtDoctorApplicablePercent.IsEnabled = false;
                            }
                            if (obj.ServiceMaster.RateEditable == true)
                            {
                                txtMinRate.IsEnabled = true;
                                txtMaxRate.IsEnabled = true;
                            }
                            else
                            {
                                txtMinRate.IsEnabled = false;
                                txtMaxRate.IsEnabled = false;
                            }

                            if (obj.ServiceMaster.ServiceTax == true)
                            {
                                txtServiceTaxAmount.IsEnabled = true;
                                txtServiceTaxPercentage.IsEnabled = true;
                            }
                            else
                            {
                                txtServiceTaxAmount.IsEnabled = false;
                                txtServiceTaxPercentage.IsEnabled = false;
                            }

                            if (obj.ServiceMaster.StaffDependantDiscount == true)
                            {
                                txtStaffParentAmount.IsEnabled = true;
                                txtStaffParentPercentage.IsEnabled = true;
                            }
                            else
                            {
                                txtStaffParentAmount.IsEnabled = false;
                                txtStaffParentPercentage.IsEnabled = false;
                            }
                            if (obj.ServiceMaster.StaffDiscount == true)
                            {
                                txtStaffDiscountAmount.IsEnabled = true;
                                txtStaffDiscountPercentage.IsEnabled = true;
                            }
                            else
                            {
                                txtStaffDiscountAmount.IsEnabled = false;
                                txtStaffDiscountPercentage.IsEnabled = false;
                            }

                            pkServiceID = obj.ServiceMaster.ID;
                            isView = true;
                            FillTariffList();
                            //BindTariffApplicable(pkServiceID);
                            GetServiceClassRateDetail(pkServiceID);


                            objAnimation.Invoke(RotationType.Forward);
                            EditMode = false;

                            SetCommandButtonState("View");

                        }
                    }
                }
            };
            client.ProcessAsync(objVo, User); //new clsUserVO());
            client.CloseAsync();
           
        }
    
        private void cboSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {

            BindServiceListGrid();
            //try
            //{

            //    clsGetServiceBySpecializationBizActionVO objBizActionVO = new clsGetServiceBySpecializationBizActionVO();
            //    objBizActionVO.ServiceMaster = new clsServiceMasterVO();
            //    objBizActionVO.ServiceMaster.Description = txtSearchCriteria.Text;
            //    objBizActionVO.ServiceMaster.Specialization = ((MasterListItem)cboSpecialization.SelectedItem).ID;     //(long) cboSpecialization.SelectedItem; //((clsServiceMasterVO)this.DataContext).Specialization;

            //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //    client.ProcessCompleted += (s, args) =>
            //    {
            //        if (args.Error == null && args.Result != null)
            //        {

            //            grdServices.ItemsSource = null;
            //            grdServices.ItemsSource = ((clsGetServiceBySpecializationBizActionVO)args.Result).ServiceList;

            //        }

            //    };

            //    client.ProcessAsync(objBizActionVO, new clsUserVO());
            //    client.CloseAsync();

            //}
            //catch (Exception)
            //{
                
            //    throw;
            //}
           
        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            long ID = Convert.ToInt64(chk.Tag);
            //int status;
            //status = chk.IsChecked == true ? 1 : 0;
            clsAddServiceMasterBizActionVO objBizAction = new clsAddServiceMasterBizActionVO();
            objBizAction.UpdateServiceMasterStatus = true;
            objBizAction.ServiceMasterDetails = new clsServiceMasterVO();
            objBizAction.ServiceMasterDetails.ID = ID;
            objBizAction.ServiceMasterDetails.Status = (bool)chk.IsChecked;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {

                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow("Service Master", "Service Status Changed Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW.Show();

                }

            };

            client.ProcessAsync(objBizAction, User); //new clsUserVO());
            client.CloseAsync();
            

            
        }

        private void chkServiceTax_Click(object sender, RoutedEventArgs e)
        {
            if (chkServiceTax.IsChecked==true)
            {
                txtServiceTaxPercentage.IsEnabled = true;
                txtServiceTaxAmount.IsEnabled = true;
            }
            if (chkServiceTax.IsChecked==false)
            {
                txtServiceTaxPercentage.IsEnabled = false;
                txtServiceTaxAmount.IsEnabled = false;
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
            if (chkStaffDiscount.IsChecked == false)
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
            if (chkStaffParentDiscount.IsChecked == false)
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
            if (chkConcession.IsChecked == false)
            {
                txtConcessionAmount.IsEnabled = false;
                txtConcessionPercentage.IsEnabled = false;

                txtConcessionAmount.Text = "0.00";
                txtConcessionPercentage.Text = "0.00";
            }
        }

        private void chkAllTariffs_Click(object sender, RoutedEventArgs e)
        {
            if (chkAllTariffs.IsChecked==true)
            {
                List<MasterListItem> objlist = (List<MasterListItem>)lstTariff.ItemsSource;
                foreach (var item in objlist)
                {
                    item.Status = true; 
                }  
            }
            if (chkAllTariffs.IsChecked == false)
            {
                List<MasterListItem> objlist = (List<MasterListItem>)lstTariff.ItemsSource;
                foreach (var item in objlist)
                {
                    item.Status = false;
                }
            }
           
        }

        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            objAnimation.Invoke(RotationType.Backward);
            SetCommandButtonState("Cancel");
        }
        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    
                    CmdSave.IsEnabled = false;
                    CmdCancel1.IsEnabled = true;
                    CmdCancel.IsEnabled = false;
                    CmdNew.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "New":

                    CmdSave.IsEnabled = true;
                    CmdNew.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    CmdCancel1.IsEnabled = false;
                    IsCancel = false;
                    break;
                case "Save":
                    CmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdCancel1.IsEnabled = true;
                    CmdCancel.IsEnabled = false;
                    IsCancel = true;
                    break;
              
                case "Cancel":
                    CmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdCancel1.IsEnabled = true;
                    CmdCancel.IsEnabled = false;
               
                    break;
                case "View":
                
                    CmdSave.IsEnabled = true;
                    CmdCancel.IsEnabled = true;
                    CmdNew.IsEnabled = false;
                    CmdCancel1.IsEnabled = false;
                    IsCancel = false;
                    break;
                default:
                    break;
            }
        }

        #endregion

        private void ServiceRate_LostFocus(object sender, RoutedEventArgs e)
        {
           // if(string.IsNullOrEmpty(
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
                  

                    String str1 = txtServiceTaxPercentage.Text.Substring(txtServiceTaxPercentage.Text.IndexOf(".")+1);
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
                            ServiceTaxPer =0;
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
                txtDoctorApplicableAmount.Text = "0.00";
            }
        }
        bool IsNew = false;
        private bool CheckDuplicasy()
        {
            clsServiceMasterVO ServiceItem;
            clsServiceMasterVO ServiceItem1;
            if (IsNew)
            {
                //ServiceItem = ((PagedSortableCollectionView<clsServiceMasterVO>)grdServices.ItemsSource).FirstOrDefault(p => p.Code.ToString().ToUpper().Equals(txtCode.Text.ToUpper()));
                ServiceItem1 = ((PagedSortableCollectionView<clsServiceMasterVO>)grdServices.ItemsSource).FirstOrDefault(p => p.ServiceName.ToUpper().Trim().Equals(txtServiceName.Text.ToUpper()));
            }
            else
            {
               // ServiceItem = ((PagedSortableCollectionView<clsServiceMasterVO>)grdServices.ItemsSource).FirstOrDefault(p => p.Code.ToString().ToUpper().Equals(txtCode.Text.ToUpper()) && p.ID != ((clsServiceMasterVO)grdServices.SelectedItem).ID);
                ServiceItem1 = ((PagedSortableCollectionView<clsServiceMasterVO>)grdServices.ItemsSource).FirstOrDefault(p => p.ServiceName.ToUpper().Trim().Equals(txtServiceName.Text.ToUpper()) && p.ID != ((clsServiceMasterVO)grdServices.SelectedItem).ID);
                Edit = false;
            }

            //if (ServiceItem != null)
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //               new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because CODE already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //    msgW1.Show();
            //    return false;
            //}
           if (ServiceItem1 != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because service name already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            else
            {
                return true;
            }
        }

        private void txtServiceRate_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtServiceRate.Text != "")
                {
                   
                    if (Extensions.IsItDecimal(txtServiceRate.Text) == true)
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
        
      

        
    }
}
