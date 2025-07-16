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
using CIMS.Forms;
using PalashDynamics.CRM;
using CIMS;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Administration;
using System.Windows.Data;
using PalashDynamics.Collections;
using System.Reflection;
using PalashDynamics.ValueObjects.Administration.PackageNew;
using PalashDynamics.UserControls;
namespace PalashDynamics.Administration
{
    public partial class frmPackageMainMaster : UserControl
    {

        public frmPackageMainMaster()
        {
            InitializeComponent();
            this.DataContext = new clsServiceMasterVO();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            DataList = new PagedSortableCollectionView<clsServiceMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);

            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;

            //IsPackage.IsChecked = true;
        }



        #region Variable Declaration

        SwivelAnimation objAnimation;
        bool IsNew = false;
        bool IsCancel = true;
        public string msgText;
        public string msgTitle;

        ObservableCollection<clsPackageServiceDetailsVO> ServiceList { get; set; }
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;

        public Boolean isPageLoaded = false;
        public bool EditMode { get; set; }
        public bool isView = false;

        public long pkServiceID { get; set; }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        long RelCount = 0;

        private List<clsPackageRelationsVO> _MemberRelationsList = new List<clsPackageRelationsVO>();

        public List<clsPackageRelationsVO> MemberRelationsList
        {
            get { return _MemberRelationsList; }
            set { _MemberRelationsList = value; }
        }

        public clsAddServiceMasterNewBizActionVO objBizActionVO;

        List<MasterListItem> objRelList;

        #endregion

        # region Pagging

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

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            BindServiceListGrid();
        }

        # endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isPageLoaded)
            {
                ValidateUI();
                //FillCodeType();
                FillSpecialization();
                //FillTariffList();
                BindServiceListGrid();
                FillRalations();
                FillValidity();

                EditMode = false;

                SetCommandButtonState("Load");

                chkAll.IsChecked = false;
            }
            isPageLoaded = true;
        }

        # region Combo Methods and Events

        private void FillRalations()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_RelationMaster;
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
                        //List<MasterListItem> objRelList = new List<MasterListItem>();
                        objRelList = new List<MasterListItem>();

                        objRelList.Add(new MasterListItem(0, "-- Select --"));
                        objRelList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                        cboMemberRelation.ItemsSource = null;
                        cboMemberRelation.ItemsSource = objRelList;
                        cboMemberRelation.SelectedItem = objRelList[0];

                        RelCount = objRelList.ToList().Count() - 1;
                    }

                    //if (this.DataContext != null)
                    //{
                    //    cboMemberRelation.SelectedValue = ((clsServiceMasterVO)this.DataContext).Specialization;
                    //}
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

                        //cboCodeType.ItemsSource = null;
                        //cboCodeType.ItemsSource = objList;
                        //cboCodeType.SelectedValue = objList[0].ID;
                    }

                    //if (this.DataContext != null)
                    //{
                    //    cboCodeType.SelectedValue = ((clsServiceMasterVO)this.DataContext).CodeType;
                    //}
                };
                client.ProcessAsync(BizAction, User); //new clsUserVO());
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

        private void FillValidity()
        {
            List<MasterListItem> lst = new List<MasterListItem>();
            lst.Add(new MasterListItem() { ID = 0, Description = "--Select--", Status = true });
            lst.Add(new MasterListItem() { ID = 1, Description = "Month", Status = true });
            lst.Add(new MasterListItem() { ID = 2, Description = "Days", Status = true });
            cmbValidity.ItemsSource = lst;
            cmbValidity.SelectedItem = lst[0];
        }

        private void cboSpecialization1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboSpecialization1.SelectedItem != null && ((MasterListItem)cboSpecialization1.SelectedItem).ID != 0)
            {
                FillSubSpecialization(((MasterListItem)cboSpecialization1.SelectedItem).ID.ToString());
            }
        }

        # endregion

        # region Click Events

        private void CmdNew_Click(object sender, RoutedEventArgs e)
        {
            objAnimation.Invoke(RotationType.Forward);
            EmptyUI();

            this.DataContext = new clsServiceMasterVO();

            ((clsServiceMasterVO)this.DataContext).EditMode = false;

            SetCommandButtonState("New");
            ((clsServiceMasterVO)this.DataContext).IsPackage = true;

            List<MasterListItem> objSelfRelationList = new List<MasterListItem>();
            objSelfRelationList = objRelList.Where(lstRel => lstRel.ID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfRelationID).ToList();

            if (objSelfRelationList != null && objSelfRelationList.Count > 0)
            {
                clsPackageRelationsVO objItem = new clsPackageRelationsVO();

                objItem.Relation = objSelfRelationList[0].Description;
                objItem.RelationID = objSelfRelationList[0].ID;

                MemberRelationsList.Add(objItem);
            }

            if (MemberRelationsList != null && MemberRelationsList.Count > 0)
            {
                dgRelation.ItemsSource = null;
                dgRelation.ItemsSource = MemberRelationsList;
            }

        }

        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";

            txtSearchCriteria.Text = "";
            cboSpecialization.SelectedValue = (long)0;

            if (IsCancel == true)
            {

                mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Package Configuration";
                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.PackageConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

            }
            else
            {
                IsCancel = true;
                BindServiceListGrid();
            }
            objAnimation.Invoke(RotationType.Backward);


        }

        private void cmdAddMemberRelations_Click(object sender, RoutedEventArgs e)
        {
            bool addRelationDetails = false;

            addRelationDetails = CheckRelationsInfo();

            if (addRelationDetails == true)
            {
                var itemDuplicate = from r in MemberRelationsList
                                    where r.RelationID == ((MasterListItem)cboMemberRelation.SelectedItem).ID
                                    select r;

                if (itemDuplicate.ToList().Count == 0)
                {
                    clsPackageRelationsVO objItem = new clsPackageRelationsVO();

                    objItem.Relation = ((MasterListItem)cboMemberRelation.SelectedItem).Description;
                    objItem.RelationID = ((MasterListItem)cboMemberRelation.SelectedItem).ID;

                    MemberRelationsList.Add(objItem);

                    cboMemberRelation.SelectedValue = 0;

                    dgRelation.ItemsSource = null;
                    dgRelation.ItemsSource = MemberRelationsList;
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Relation already added.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                }
            }

        }

        private void hlbDeleteKin_Click(object sender, RoutedEventArgs e)
        {
            clsPackageRelationsVO RemoveableItem = new clsPackageRelationsVO();
            RemoveableItem = (clsPackageRelationsVO)dgRelation.SelectedItem;

            if (RemoveableItem.RelationID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfRelationID)
            {
                if (dgRelation.ItemsSource != null)
                    MemberRelationsList = (List<clsPackageRelationsVO>)dgRelation.ItemsSource;

                if (RemoveableItem != null)
                    MemberRelationsList.Remove(RemoveableItem);

                dgRelation.ItemsSource = null;
                dgRelation.ItemsSource = MemberRelationsList;

                if (MemberRelationsList.Count() < 16)
                    chkAll.IsChecked = false;
            }
            else
            {
                msgText = "You cann't delete this relation, as its a Default Relation !";
                MessageBoxControl.MessageBoxChildWindow msg2 =
                  new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);

                msg2.Show();
            }

        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            //if (!Edit)
            //    IsNew = true;

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
                    msgText = "Are you sure you want to Save ?";
                    MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                      new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowSave_OnMessageBoxClosed);

                    msgWindowUpdate.Show();
                }
            }
        }

        void msgWindowSave_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                ////waiting.Show();
                //if (CheckDuplicasy())
                //{
                Save(false);
                SetCommandButtonState("Save");
                //}
                //// waiting.Close();

            }
            else
            {
            }
        }

        private void Save(bool IsModify)
        {
            try
            {

                if (ValidateUI())
                {

                    objBizActionVO = new clsAddServiceMasterNewBizActionVO();
                    objBizActionVO.ServiceMasterDetails = new clsServiceMasterVO();
                    objBizActionVO.ServiceMasterDetails.EditMode = ((clsServiceMasterVO)this.DataContext).EditMode;
                    objBizActionVO.ServiceMasterDetails.ServiceCode = ((clsServiceMasterVO)this.DataContext).ServiceCode;
                    objBizActionVO.ServiceMasterDetails.ID = ((clsServiceMasterVO)this.DataContext).ID;

                    //objBizActionVO.ServiceMasterDetails.Code = txtCode.Text == "" ? "" : txtCode.Text;

                    if (cboSpecialization1.SelectedItem != null)
                        objBizActionVO.ServiceMasterDetails.Specialization = ((MasterListItem)cboSpecialization1.SelectedItem).ID;

                    if (cboSubSpecialization1.SelectedItem != null)
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


                    objBizActionVO.ServiceMasterDetails.SeniorCitizen = ((clsServiceMasterVO)this.DataContext).SeniorCitizen;
                    objBizActionVO.ServiceMasterDetails.SeniorCitizenConAmount = txtSeniorCitizenPerAmount.Text == "" ? 0 : decimal.Parse(txtSeniorCitizenPerAmount.Text);
                    objBizActionVO.ServiceMasterDetails.SeniorCitizenConPercent = txtSeniorCitizenPer.Text == "" ? 0 : decimal.Parse(txtSeniorCitizenPer.Text);
                    objBizActionVO.ServiceMasterDetails.SeniorCitizenAge = ((clsServiceMasterVO)this.DataContext).SeniorCitizenAge;


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
                    //objBizActionVO.ServiceMasterDetails.CheckedAllTariffs = chkAllTariffs.IsChecked == true ? true : false;  // used to set for select all tariffs
                    objBizActionVO.ServiceMasterDetails.CreatedUnitID = 1;
                    objBizActionVO.ServiceMasterDetails.UpdatedUnitID = 1;
                    objBizActionVO.ServiceMasterDetails.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    objBizActionVO.ServiceMasterDetails.AddedOn = DateTime.Now.DayOfWeek.ToString();
                    objBizActionVO.ServiceMasterDetails.AddedDateTime = DateTime.Now;

                    objBizActionVO.ServiceMasterDetails.IsPackage = true;

                    objBizActionVO.ServiceMasterDetails.EffectiveDate = ((clsServiceMasterVO)this.DataContext).EffectiveDate;
                    objBizActionVO.ServiceMasterDetails.ExpiryDate = ((clsServiceMasterVO)this.DataContext).ExpiryDate;
                    objBizActionVO.ServiceMasterDetails.IsFamily = ((clsServiceMasterVO)this.DataContext).IsFamily;
                    objBizActionVO.ServiceMasterDetails.FamilyMemberCount = ((clsServiceMasterVO)this.DataContext).FamilyMemberCount;
                    objBizActionVO.ServiceMasterDetails.AddedWindowsLoginName = "palash";

                    // Code Commented Becouse the Set AllRelation Combo Box is Collapsed 
                    // So the below code is Commented by CDS 

                    if (RelCount == MemberRelationsList.Count)
                    {
                        foreach (var item in MemberRelationsList)
                        {
                            if (item.IsSetAll == false)
                                item.IsSetAll = true;
                            else
                                item.IsSetAll = true;
                        }
                        objBizActionVO.FamilyMemberMasterDetails = MemberRelationsList;
                    }
                    else
                    {
                        foreach (var item in MemberRelationsList)
                        {
                            if (item.IsSetAll == true)
                                item.IsSetAll = false;
                        }
                        objBizActionVO.FamilyMemberMasterDetails = MemberRelationsList;
                    } //End of else 
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            string strMsg=string.Empty;
                            if (IsModify) strMsg = "Updated"; else strMsg = "Saved";

                            string msgTitle = "";
                            string msgText = "";

                            txtSearchCriteria.Text = "";
                            cboSpecialization.SelectedValue = (long)0;

                            if (((clsAddServiceMasterNewBizActionVO)arg.Result).SuccessStatus == 3)
                            {
                                msgText = "Record cannot be " + strMsg + ", Package Name already exist!."; //"Record cannot be save, Package Name already exist!.";

                                MessageBoxControl.MessageBoxChildWindow msgW =
                               new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                msgW.Show();
                                SetCommandButtonState("New");

                            }
                            else if (((clsAddServiceMasterNewBizActionVO)arg.Result).SuccessStatus == 0)
                            {
                                msgText = "Package " + strMsg + " Successfully"; //"Package Added Successfully";

                                MessageBoxControl.MessageBoxChildWindow msgW =
                                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);


                                msgW.Show();
                                objAnimation.Invoke(RotationType.Backward);
                                BindServiceListGrid();
                                SetCommandButtonState("Load");
                            }

                        }
                        else
                        {
                            string msgTitle = "";
                            string msgText = "Some error occurred while saving";

                            MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);


                            msgW.Show();
                            SetCommandButtonState("New");

                            txtSearchCriteria.Text = "";
                            cboSpecialization.SelectedValue = (long)0;
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

        private void CmdModify_Click(object sender, RoutedEventArgs e)
        {
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
                    msgText = "Are you sure you want to Modify ?";
                    MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                      new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowModify_OnMessageBoxClosed);

                    msgWindowUpdate.Show();
                }
            }
        }

        void msgWindowModify_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                Save(true);
                SetCommandButtonState("Modify");
            }

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {

            IsNew = false;
            chkAll.IsChecked = false;
            //Edit = true;
            clsServiceMasterVO objServiceVO = new clsServiceMasterVO();
            objServiceVO = (clsServiceMasterVO)grdServices.SelectedItem;
            clsGetServiceMasterListNewBizActionVO objVo = new clsGetServiceMasterListNewBizActionVO();
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
                    clsGetServiceMasterListNewBizActionVO obj = args.Result as clsGetServiceMasterListNewBizActionVO;
                    if (obj != null)
                    {
                        this.DataContext = obj.ServiceMaster;
                        ((clsServiceMasterVO)this.DataContext).EditMode = true;
                        if (objServiceVO != null)
                        {
                            objServiceVO.EditMode = true;

                            //cboCodeType.SelectedValue = obj.ServiceMaster.CodeType;  // objServiceVO.CodeType;
                            ////LongDescription.Text = objServiceVO.LongDescription;
                            ////ServiceShortDescription.Text = objServiceVO.ShortDescription;

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


                            if (obj.ServiceMaster.SeniorCitizen == true)
                            {
                                txtSeniorCitizenAge.IsEnabled = true;
                                txtSeniorCitizenPerAmount.IsEnabled = true;
                                txtSeniorCitizenPer.IsEnabled = true;
                            }
                            else
                            {
                                txtSeniorCitizenAge.IsEnabled = false;
                                txtSeniorCitizenPerAmount.IsEnabled = false;
                                txtSeniorCitizenPer.IsEnabled = false;
                            }

                            pkServiceID = obj.ServiceMaster.ID;
                            isView = true;
                            //FillTariffList();
                            ////BindTariffApplicable(pkServiceID);
                            //GetServiceClassRateDetail(pkServiceID);


                            var ob1 = obj.FamilyMemberMasterDetails.ToList().Where(z => z.IsSetAll == true);

                            // Code Commented Becouse the Set AllRelation Combo Box is Collapsed 
                            // So the below code is Commented by CDS 

                            if (ob1.ToList().Count == 1)
                            {
                                List<MasterListItem> objAllRelList = new List<MasterListItem>();
                                objAllRelList = (List<MasterListItem>)(cboMemberRelation.ItemsSource);


                                var result = from r in objAllRelList
                                             where r.ID > 0
                                             select r;

                                MemberRelationsList.Clear();

                                foreach (var item in result)
                                {
                                    MemberRelationsList.Add(new clsPackageRelationsVO()
                                    {
                                        Relation = item.Description,
                                        ID = item.ID

                                    });
                                }
                                chkAll.IsChecked = true;
                            }
                            else
                            {
                                MemberRelationsList.Clear();
                                MemberRelationsList = obj.FamilyMemberMasterDetails;
                                if (RelCount == MemberRelationsList.Count())
                                    chkAll.IsChecked = true;
                            }


                            dgRelation.ItemsSource = null;
                            dgRelation.ItemsSource = MemberRelationsList;

                            objAnimation.Invoke(RotationType.Forward);
                            EditMode = false;

                            SetCommandButtonState("View");
                            if (objServiceVO.IsFreezed)
                                cmdModify.IsEnabled = false;
                            else
                                cmdModify.IsEnabled = true;
                        }
                    }
                }
            };
            client.ProcessAsync(objVo, User); //new clsUserVO());
            client.CloseAsync();
        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            long ID = Convert.ToInt64(chk.Tag);
            //int status;
            //status = chk.IsChecked == true ? 1 : 0;
            clsAddServiceMasterBizActionVO objBizAction = new clsAddServiceMasterBizActionVO();//clsAddServiceMasterNewBizActionVO objBizAction = new clsAddServiceMasterNewBizActionVO();
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
                        new MessageBoxControl.MessageBoxChildWindow("Package", "Package Service Status Changed Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW.Show();

                }

            };

            client.ProcessAsync(objBizAction, User); //new clsUserVO());
            client.CloseAsync();

        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            BindServiceListGrid();
            dgDataPager.PageIndex = 0;
        }

        private void cmdPackageDefination_Click(object sender, RoutedEventArgs e)
        {
            if (grdServices.SelectedItem != null)
            {
                frmPackageDefinationMaster win = new frmPackageDefinationMaster();
                win.OnAddButton_Click += new RoutedEventHandler(win_OnAddButton_Click);
                win.PackageServiceID = ((clsServiceMasterVO)grdServices.SelectedItem).ID;
                win.PackageServiceUnitID = ((clsServiceMasterVO)grdServices.SelectedItem).UnitID;
                win.PackageID = ((clsServiceMasterVO)grdServices.SelectedItem).PackageID;
                win.IsFreeze = ((clsServiceMasterVO)grdServices.SelectedItem).IsFreezed;
                win.IsFromPackageMainMaster = true;  // Added by CDS
                win.PackageRate =Convert.ToDouble(((clsServiceMasterVO)grdServices.SelectedItem).Rate);
                if (((clsServiceMasterVO)grdServices.SelectedItem).PackageID > 0)
                    win.IsView = true;
                win.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                        new MessageBoxControl.MessageBoxChildWindow("Package", "Select Package Service to add Package Definition", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW.Show();
            }
        }

        void win_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            BindServiceListGrid();
        }

        //protected override void OnClosed(EventArgs e)
        //{
        //    base.OnClosed(e);
        //    Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        //}

        private void cmdPackageRelation_Click(object sender, RoutedEventArgs e)
        {
            frmPackageRelation win = new frmPackageRelation();
            win.PackageID = ((clsServiceMasterVO)grdServices.SelectedItem).PackageID;
            win.PackageName = ((clsServiceMasterVO)grdServices.SelectedItem).Description;
            win.TariffID = ((clsServiceMasterVO)grdServices.SelectedItem).TariffID;
            win.Show();
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

        private void chkServiceTax_Click(object sender, RoutedEventArgs e)
        {
            if (chkServiceTax.IsChecked == true)
            {
                txtServiceTaxPercentage.IsEnabled = true;
                txtServiceTaxAmount.IsEnabled = true;
            }
            if (chkServiceTax.IsChecked == false)
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
                        if (Extensions.IsPositiveNumber(txtServiceTaxPercentage.Text) == true) // txtServiceTaxPercentage.Text.IsPositiveDoubleValid()
                        {
                            if (Extensions.IsItDecimal(txtServiceTaxPercentage.Text) == false)
                            {
                                ServiceTaxPer = Convert.ToDecimal(txtServiceTaxPercentage.Text);
                            }
                        }
                        //else
                        //{
                        //    txtServiceTaxPercentage.SetValidation(" Service Tax should be Positive number");//txtServiceTaxPercentage.SetValidation(" Service Tax should be number");
                        //    txtServiceTaxPercentage.RaiseValidationError();
                        //    ServiceTaxPer = 0;
                        //}

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
                            txtServiceTaxAmount.SetValidation(" Service Tax Amount should be Positive number");//txtServiceTaxAmount.SetValidation(" Service Tax Amount should be number");
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
                        if (Extensions.IsPositiveNumber(txtStaffDiscountPercentage.Text) == true)
                        {
                            if (Extensions.IsItDecimal(txtStaffDiscountPercentage.Text) == false)
                            {
                                Percent = Convert.ToDecimal(txtStaffDiscountPercentage.Text);
                            }
                        }
                        //else
                        //{
                        //    txtStaffDiscountPercentage.SetValidation(" Staff Discount Percent should be Positive number");//txtStaffDiscountPercentage.SetValidation(" Staff Discount Percent should be number");
                        //    txtStaffDiscountPercentage.RaiseValidationError();
                        //    Percent = 0;
                        //}
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
                            txtStaffDiscountAmount.SetValidation(" Staff Discount Amount should be Positive number");//txtStaffDiscountAmount.SetValidation(" Staff Discount Amount should be number");
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
                            txtStaffParentPercentage.SetValidation(" Staff Parent Percent should be Positive number");//txtStaffParentPercentage.SetValidation(" Staff Parent Percent should be number");
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
                            txtStaffParentAmount.SetValidation(" Staff Parent Amount should be Positive number");//txtStaffParentAmount.SetValidation(" Staff Parent Amount should be number");
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
                        if (Extensions.IsPositiveNumber(txtConcessionPercentage.Text) == true)
                        {
                            if (Extensions.IsItDecimal(txtConcessionPercentage.Text) == false)
                            {
                                Percent = Convert.ToDecimal(txtConcessionPercentage.Text);
                            }
                        }
                        //else
                        //{
                        //    txtConcessionPercentage.SetValidation("Concession Percent should be Positive number");//txtConcessionPercentage.SetValidation("Concession Percent should be number");
                        //    txtConcessionPercentage.RaiseValidationError();
                        //    Percent = 0;
                        //}
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
                            txtConcessionAmount.SetValidation(" Concession Amount should be Positive number");//txtConcessionAmount.SetValidation(" Concession Amount should be number");
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
                        if (Extensions.IsPositiveNumber(txtDoctorApplicablePercent.Text) == true)
                        {
                            if (Extensions.IsItDecimal(txtDoctorApplicablePercent.Text) == false)
                            {
                                Percent = Convert.ToDecimal(txtDoctorApplicablePercent.Text);

                            }
                        }

                        //else
                        //{
                        //    txtDoctorApplicablePercent.SetValidation("Doctor Applicable Percent should be Positive number");//txtDoctorApplicablePercent.SetValidation("Doctor Applicable Percent should be number");
                        //    txtDoctorApplicablePercent.RaiseValidationError();
                        //    Percent = 0;
                        //}

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
                            txtDoctorApplicableAmount.SetValidation(" Doctor Applicable Amount should be Positive number");//txtDoctorApplicableAmount.SetValidation(" Doctor Applicable Amount should be number");
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
                        txtServiceRate.Text = "0";    //PrevServiceRate.ToString();
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
                                txtServiceRate.Text = "0";    //PrevServiceRate.ToString();
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

        private void chkSeniorCitizen_Click(object sender, RoutedEventArgs e)
        {
            if (chkSeniorCitizen.IsChecked == true)
            {
                txtSeniorCitizenPer.IsEnabled = true;
                txtSeniorCitizenPerAmount.IsEnabled = true;
                txtSeniorCitizenAge.IsEnabled = true;
            }
            if (chkSeniorCitizen.IsChecked == false)
            {
                txtSeniorCitizenPer.IsEnabled = false;
                txtSeniorCitizenPerAmount.IsEnabled = false;
                txtSeniorCitizenPerAmount.Text = "0.00";
                txtSeniorCitizenPer.Text = "0.00";
                txtSeniorCitizenAge.IsEnabled = false;
            }
        }

        private void txtSeniorCitizenPer_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtSeniorCitizenPer.Text.Equals("") && (txtSeniorCitizenPer.Text != "0"))
                {
                    if (!txtSeniorCitizenPer.Text.Equals("") && (txtSeniorCitizenPer.Text != "0"))
                    {
                        if (Extensions.IsItDecimal(txtSeniorCitizenPer.Text) == false)
                        {
                            if (Convert.ToDecimal(txtSeniorCitizenPer.Text) > 100)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                                      new MessageBoxControl.MessageBoxChildWindow("", "Concession percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW.Show();
                                txtSeniorCitizenPer.Text = "0.00";
                                txtSeniorCitizenPerAmount.Text = "0.00";
                                return;
                            }
                            String str1 = txtSeniorCitizenPer.Text.Substring(txtSeniorCitizenPer.Text.IndexOf(".") + 1);
                            if (Convert.ToDecimal(str1) > 100)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                            new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW.Show();
                                txtSeniorCitizenPer.Text = "0.00";
                                return;
                            }
                        }
                    }
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal Rate = 0;
                        Rate = Convert.ToDecimal(txtServiceRate.Text);

                        decimal Percent = 0;
                        if (Extensions.IsItDecimal(txtSeniorCitizenPer.Text) == false)
                        {
                            Percent = Convert.ToDecimal(txtSeniorCitizenPer.Text);
                        }

                        else
                        {
                            txtSeniorCitizenPer.SetValidation("Concession Percent should be number");
                            txtSeniorCitizenPer.RaiseValidationError();
                            Percent = 0;
                        }


                        decimal TaxAmount = 0;
                        TaxAmount = ((Rate * Percent) / 100);

                        txtSeniorCitizenPerAmount.Text = TaxAmount.ToString("0.00");

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                  new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtSeniorCitizenPer.Text = "0.00";
            }
        }

        private void txtSeniorCitizenPerAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtSeniorCitizenPerAmount.Text.Equals("") && (txtSeniorCitizenPerAmount.Text != "0"))
                {
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal Rate = 0;
                        Rate = Convert.ToDecimal(txtServiceRate.Text);

                        decimal TaxAmount = 0;
                        if (Extensions.IsPositiveNumber(txtServiceTaxPercentage.Text) == false)
                        {
                            if (Extensions.IsItDecimal(txtSeniorCitizenPerAmount.Text) == false)
                            {
                                TaxAmount = Convert.ToDecimal(txtSeniorCitizenPerAmount.Text);

                            }
                        }
                        //else
                        //{
                        //    txtSeniorCitizenPerAmount.SetValidation("Concession Amount should be number");
                        //    txtSeniorCitizenPerAmount.RaiseValidationError();
                        //    TaxAmount = 0;
                        //    txtSeniorCitizenPerAmount.Text = "0.00";
                        //}


                        decimal TaxPer = 0;

                        TaxPer = (100 * TaxAmount) / Rate;
                        if (TaxPer > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                  new MessageBoxControl.MessageBoxChildWindow("", "Concession percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            txtSeniorCitizenPer.Text = "0.00";
                            txtSeniorCitizenPerAmount.Text = "0.00";
                            TaxPer = 0;
                            return;
                        }

                        txtSeniorCitizenPer.Text = TaxPer.ToString("0.00");

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                  new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtSeniorCitizenPerAmount.Text = "0.00";
            }
        }

        private void txtServiceRate_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValueDouble())
            {
                if (textBefore != null)
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        private void txtServiceRate_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        # endregion

        # region Methods

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    cmdPackageDefination.IsEnabled = true;
                    cmdPackageRateClinic.IsEnabled = true;
                    cmdAssignTariff.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    cmdPackageDefination.IsEnabled = false;
                    cmdPackageRateClinic.IsEnabled = false;
                    cmdAssignTariff.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Save":
                    cmdNew.IsEnabled = true;
                    cmdPackageDefination.IsEnabled = true;
                    cmdPackageRateClinic.IsEnabled = true;
                    cmdAssignTariff.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdModify.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = true;
                    cmdPackageDefination.IsEnabled = true;
                    cmdPackageRateClinic.IsEnabled = true;
                    cmdAssignTariff.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    cmdPackageDefination.IsEnabled = true;
                    cmdPackageRateClinic.IsEnabled = true;
                    cmdAssignTariff.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;

                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdPackageDefination.IsEnabled = false;
                    cmdPackageRateClinic.IsEnabled = false;
                    cmdAssignTariff.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                default:
                    break;
            }
        }

        private bool CheckRelationsInfo()
        {
            bool result = true;
            try
            {

                if ((MasterListItem)cboMemberRelation.SelectedItem == null || ((MasterListItem)cboMemberRelation.SelectedItem).ID == 0)
                {
                    cboMemberRelation.TextBox.SetValidation("Select Ralation to add");
                    cboMemberRelation.TextBox.RaiseValidationError();
                    cboMemberRelation.Focus();
                    result = false;

                }
                else
                    cboMemberRelation.TextBox.ClearValidationError();


            }
            catch (Exception)
            {

                // throw;
            }
            return result;
        }

        private bool ValidateUI()
        {

            bool result = true;
            try
            {

                if (isPageLoaded)
                {

                    if (((MasterListItem)cboSpecialization1.SelectedItem).ID == 0)
                    {
                        cboSpecialization1.TextBox.SetValidation("Specialization is required");
                        cboSpecialization1.TextBox.RaiseValidationError();
                        cboSpecialization1.Focus();
                        result = false;
                    }
                    else if (((MasterListItem)cboSpecialization1.SelectedItem) == null)
                    {
                        cboSpecialization1.TextBox.SetValidation("Specialization is required");
                        cboSpecialization1.TextBox.RaiseValidationError();
                        cboSpecialization1.Focus();
                        result = false;
                    }
                    else
                        cboSpecialization1.TextBox.ClearValidationError();
                }

                if (string.IsNullOrEmpty(txtServiceRate.Text.Trim()))
                {
                    txtServiceRate.SetValidation("Package Rate is required");
                    txtServiceRate.RaiseValidationError();
                    txtServiceRate.Focus();
                    result = false;
                }
                else
                {
                    txtServiceRate.ClearValidationError();
                    //txtServiceRate.is
                }

                if (txtServiceRate.Text == "0" || txtServiceRate.Text == "")
                {
                    txtServiceRate.SetValidation("Package Rate is required");
                    txtServiceRate.RaiseValidationError();
                    txtServiceRate.Focus();
                    result = false;
                }
                else
                {
                    txtServiceRate.ClearValidationError();
                }

                if (string.IsNullOrEmpty(txtServiceName.Text.Trim()))
                {
                    txtServiceName.SetValidation("Package Name is required");
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

                        txtServiceTaxPercentage.SetValidation(" Tax Percentage should be Positive number"); //txtServiceTaxPercentage.SetValidation(" Tax Percentage should be number");
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
                                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Package Tax Percentage or Service Tax Amount Need To Be Added!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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

                if (chkSeniorCitizen.IsChecked == true)
                {
                    if (txtSeniorCitizenAge.Text.Equals("") || (txtSeniorCitizenAge.Text.Trim() == "0"))
                    {
                        txtSeniorCitizenAge.SetValidation("Senior Citizen Age is required");
                        txtSeniorCitizenAge.RaiseValidationError();
                        txtSeniorCitizenAge.Focus();
                        result = false;
                    }
                    else if (int.Parse(txtSeniorCitizenAge.Text) < 60 || int.Parse(txtSeniorCitizenAge.Text) > 120)
                    {
                        txtSeniorCitizenAge.SetValidation("Please enter age Between 60 to 120");
                        txtSeniorCitizenAge.RaiseValidationError();
                        txtSeniorCitizenAge.Focus();
                        result = false;
                    }

                    if (txtSeniorCitizenPer.Text.Equals("") || (txtSeniorCitizenPer.Text.Trim() == "0"))
                    {
                        if (txtSeniorCitizenPerAmount.Text.Equals("") || (txtSeniorCitizenPerAmount.Text.Trim() == "0"))
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Either Concession Percentage or Concession Amount Can Be Added!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindowUpdate.Show();

                            result = false;
                        }
                    }
                    else if (txtSeniorCitizenPer.Text.Equals("") || (txtSeniorCitizenPer.Text.Trim() == "0.00"))
                    {
                        if (txtSeniorCitizenPerAmount.Text.Equals("") || (txtSeniorCitizenPerAmount.Text.Trim() == "0.00"))
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
                                    //if ((Convert.ToDecimal(txtServiceRate.Text) > Convert.ToDecimal(txtMaxRate.Text)) || (Convert.ToDecimal(txtServiceRate.Text) < Convert.ToDecimal(txtMinRate.Text)))
                                    //{
                                    //    MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                                    //                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Service Rate Must Be In Between Min. Rate & Max. Rate!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    //    msgWindowUpdate.Show();

                                    //    result = false;
                                    //}
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

                if (isPageLoaded)
                {
                    if (dtpEffectiveDate.SelectedDate != null && dtpExpiryDate.SelectedDate != null)
                    {
                        if (dtpEffectiveDate.SelectedDate.Value.Date.Date > dtpExpiryDate.SelectedDate.Value.Date.Date)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                         new MessageBoxControl.MessageBoxChildWindow("", "Package Effective date can not be greater than Package Expiry date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                            msgW1.Show();
                            result = false;
                            //return result;

                        }

                        //if (dtpExpiryDate.SelectedDate.Value.Date.Date < dtpEffectiveDate.SelectedDate.Value.Date.Date)
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //                 new MessageBoxControl.MessageBoxChildWindow("", "Package Expiry date can not be less than Package Effective date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        //    msgW1.Show();
                        //    result = false;

                        //}
                    }
                    else
                    {
                        if (dtpEffectiveDate.SelectedDate == null)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Package Effective date is required.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                            msgW1.Show();
                            dtpEffectiveDate.Focus();

                            result = false;
                            return result;

                        }


                        if (dtpExpiryDate.SelectedDate == null)
                        {
                            //dtpExpiryDate.SetValidation("Package Expiry date is required");
                            //dtpExpiryDate.RaiseValidationError();
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "Package Expiry date is required.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                            msgW1.Show();
                            dtpExpiryDate.Focus();
                            result = false;
                        }
                        else
                            dtpExpiryDate.ClearValidationError();
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

        private void EmptyUI()
        {
            clsServiceMasterVO obj = new clsServiceMasterVO();
            txtServiceCode.Text = "";
            //txtCode.Text = "";
            txtServiceLongDescription.Text = "";
            txtServiceName.Text = "";
            txtServiceRate.Text = "";
            txtServiceShortDescription.Text = "";
            txtMinRate.Text = "";
            txtMaxRate.Text = "";
            txtDoctorApplicableAmount.Text = "";
            txtDoctorApplicablePercent.Text = "";
            //cboCodeType.SelectedValue = obj.CodeType;
            cboSpecialization1.SelectedValue = obj.Specialization;
            cboSubSpecialization1.SelectedValue = obj.SubSpecialization;
            chkApplicableToAllDoctors.IsChecked = obj.DoctorShare;
            chkConcession.IsChecked = obj.Concession;
            txtConcessionAmount.Text = "";
            txtConcessionAmount.IsEnabled = false;
            txtConcessionPercentage.Text = "";
            txtConcessionPercentage.IsEnabled = false;
            ////chkIServiceActive.IsChecked = true;
            //chkAllTariffs.IsChecked = obj.CheckedAllTariffs;
            //chkInHouse.IsChecked = obj.InHouse;
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

            txtDoctorApplicablePercent.IsEnabled = false;
            txtDoctorApplicableAmount.IsEnabled = false;
            IsNew = false;


            txtSeniorCitizenAge.IsEnabled = false;
            txtSeniorCitizenPerAmount.IsEnabled = false;
            txtSeniorCitizenPer.IsEnabled = false;
            this.DataContext = new clsServiceMasterVO();

            MemberRelationsList = new List<clsPackageRelationsVO>();
            dgRelation.ItemsSource = MemberRelationsList;
            IsFamily.IsChecked = false;

            cboMemberRelation.SelectedValue = (long)0;
        }

        private void BindServiceListGrid()
        {
            try
            {
                WaitIndicator obj = new WaitIndicator();
                obj.Show();

                clsGetServiceMasterListNewBizActionVO BizActionObj = new clsGetServiceMasterListNewBizActionVO();

                BizActionObj.GetAllServiceListDetails = true;
                BizActionObj.ServiceList = new List<clsServiceMasterVO>();
                BizActionObj.FromPackage = true;
                //BizActionObj.frFromPackage
                BizActionObj.IsPagingEnabled = true;
                BizActionObj.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                BizActionObj.MaximumRows = DataList.PageSize;

                BizActionObj.ServiceName = txtSearchCriteria.Text;
                if (cboSpecialization.SelectedItem == null)
                {
                    BizActionObj.Specialization = 0;
                }
                else
                {
                    BizActionObj.Specialization = ((MasterListItem)cboSpecialization.SelectedItem).ID == null ? 0 : ((MasterListItem)cboSpecialization.SelectedItem).ID;     //(long)cboSpecialization.SelectedItem; //((clsServiceMasterVO)this.DataContext).Specialization;
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetServiceMasterListNewBizActionVO result = args.Result as clsGetServiceMasterListNewBizActionVO;
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

        # endregion

        private void cboSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboSpecialization.SelectedItem != null)
            {
                BindServiceListGrid();
            }

        }

        private void grdServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((clsServiceMasterVO)grdServices.SelectedItem != null)
            {
                if (((clsServiceMasterVO)grdServices.SelectedItem).Status && ((clsServiceMasterVO)grdServices.SelectedItem).IsFreezed && ((clsServiceMasterVO)grdServices.SelectedItem).IsApproved)
                {
                    cmdAssignTariff.IsEnabled = true;
                }
                else
                {
                    cmdAssignTariff.IsEnabled = false;
                }
            }
        }

        private void ServiceRate_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void grdRelation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextNameV_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtValidity_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                BindServiceListGrid();
                dgDataPager.PageIndex = 0;
            }
        }

        private void chkFreeze_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grdServices.SelectedItem != null && ((clsServiceMasterVO)(grdServices.SelectedItem)).PackageID > 0)
                {
                    WaitIndicator obj = new WaitIndicator();
                    obj.Show();

                    CheckBox chk = (CheckBox)sender;

                    clsAddPackageServiceNewBizActionVO objBizAction = new clsAddPackageServiceNewBizActionVO();
                    objBizAction.UpdatePackageStatus = true;

                    objBizAction.Details = new clsPackageServiceVO();

                    objBizAction.Details.ID = ((clsServiceMasterVO)(grdServices.SelectedItem)).PackageID;
                    objBizAction.Details.UnitID = ((clsServiceMasterVO)(grdServices.SelectedItem)).PackageUnitID;

                    objBizAction.Details.IsFreezed = (bool)chk.IsChecked;
                    objBizAction.UpdatePackageFreezeStatus = (bool)chk.IsChecked;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                new MessageBoxControl.MessageBoxChildWindow("Package", "Package Freezed Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW.Show();

                            obj.Close();
                            BindServiceListGrid();

                        }

                    };

                    client.ProcessAsync(objBizAction, ((IApplicationConfiguration)App.Current).CurrentUser); //new clsUserVO());
                    client.CloseAsync();
                }
                else
                {
                    if (((CheckBox)sender).IsChecked == true)
                    {
                        ((CheckBox)sender).IsChecked = false;
                    }
                    else
                    {
                        ((CheckBox)sender).IsChecked = true;
                    }

                    MessageBoxControl.MessageBoxChildWindow msgW =
                                                    new MessageBoxControl.MessageBoxChildWindow("Package", "Package can't be Freeze, as Package Details are not added ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW.Show();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //obj.Close();
            }
        }

        private void chkApproved_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grdServices.SelectedItem != null && ((clsServiceMasterVO)(grdServices.SelectedItem)).PackageID > 0)
                {
                    WaitIndicator obj = new WaitIndicator();
                    obj.Show();

                    if (((clsServiceMasterVO)grdServices.SelectedItem).IsFreezed == true && ((clsServiceMasterVO)grdServices.SelectedItem).Status == true)
                    {
                        string msgTitle = "Palash";
                        string msgText = "Are you sure you want to change Approve status for Package ?";

                        MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                        msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.Yes)
                            {
                                CheckBox chk = (CheckBox)sender;
                                //long ID = Convert.ToInt64(chk.Tag);

                                clsAddPackageServiceNewBizActionVO objBizAction = new clsAddPackageServiceNewBizActionVO();
                                objBizAction.UpdatePackgeApproveStatus = true;

                                objBizAction.Details = new clsPackageServiceVO();

                                objBizAction.Details.ID = ((clsServiceMasterVO)(grdServices.SelectedItem)).PackageID;
                                objBizAction.Details.UnitID = ((clsServiceMasterVO)(grdServices.SelectedItem)).PackageUnitID;

                                objBizAction.Details.IsApproved = (bool)chk.IsChecked;

                                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                                client.ProcessCompleted += (s, args) =>
                                {
                                    if (args.Error == null && args.Result != null)
                                    {

                                        MessageBoxControl.MessageBoxChildWindow msgW =
                                            new MessageBoxControl.MessageBoxChildWindow("Package", "Package Approve Status Changed Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgW.Show();

                                        obj.Close();
                                        BindServiceListGrid();

                                    }

                                };

                                client.ProcessAsync(objBizAction, ((IApplicationConfiguration)App.Current).CurrentUser); //new clsUserVO());
                                client.CloseAsync();

                            }
                            else
                            {
                                if (((CheckBox)sender).IsChecked == true)
                                {
                                    ((CheckBox)sender).IsChecked = false;
                                }
                                else
                                {
                                    ((CheckBox)sender).IsChecked = true;
                                }

                                obj.Close();
                            }
                        };

                        msgWD.Show();


                    }
                    else
                    {
                        if (((CheckBox)sender).IsChecked == true)
                        {
                            ((CheckBox)sender).IsChecked = false;
                        }
                        else
                        {
                            ((CheckBox)sender).IsChecked = true;
                        }

                        obj.Close();

                        MessageBoxControl.MessageBoxChildWindow msgW =
                                   new MessageBoxControl.MessageBoxChildWindow("Package", "Package must be Freezed with Status Active before Approve", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.Show();
                    }
                }
                else
                {
                    if (((CheckBox)sender).IsChecked == true)
                    {
                        ((CheckBox)sender).IsChecked = false;
                    }
                    else
                    {
                        ((CheckBox)sender).IsChecked = true;
                    }

                    MessageBoxControl.MessageBoxChildWindow msgW =
                                                    new MessageBoxControl.MessageBoxChildWindow("Package", "Package can't be Approve, as Package Details are not added ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW.Show();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //obj.Close();
            }
        }

        private void cmdPackageRateClinicwise_Click(object sender, RoutedEventArgs e)
        {
            frmPackageRateClinicWise win = new frmPackageRateClinicWise();
            win.PackageID = ((clsServiceMasterVO)grdServices.SelectedItem).PackageID;
            win.PackageSeriveID = ((clsServiceMasterVO)grdServices.SelectedItem).ID;
            win.Show();
        }

        private void chkAll_Checked(object sender, RoutedEventArgs e)
        {
            List<MasterListItem> objAllRelList = new List<MasterListItem>();
            objAllRelList = (List<MasterListItem>)(cboMemberRelation.ItemsSource);




            var itemDuplicate = from r in objAllRelList
                                where r.ID > 0
                                select r;

            if (chkAll.IsChecked == true)
            {
                try
                {
                    bool addRelationDetails = true;



                    if (addRelationDetails == true)
                    {
                        //List<MasterListItem> objAllRelList = new List<MasterListItem>();
                        //objAllRelList = (List<MasterListItem>)(cboMemberRelation.ItemsSource);


                        MemberRelationsList.Clear();

                        //var itemDuplicate = from r in objAllRelList
                        //                    where r.ID > 0
                        //                    select r;


                        foreach (MasterListItem mst in itemDuplicate)
                        {
                            clsPackageRelationsVO objItem = new clsPackageRelationsVO();

                            objItem.Relation = mst.Description;
                            // objItem.RelationID = mst.ID;
                            objItem.RelationID = 0;
                            objItem.IsSetAll = true;

                            MemberRelationsList.Add(objItem);

                        }

                        cboMemberRelation.SelectedValue = 0;
                        dgRelation.ItemsSource = null;
                        dgRelation.ItemsSource = MemberRelationsList;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {

                for (int i = 0; i < MemberRelationsList.Count; i++)
                {
                    foreach (MasterListItem mst in itemDuplicate)
                    {
                        clsPackageRelationsVO objItem = new clsPackageRelationsVO();
                        if (MemberRelationsList[i].Relation == mst.Description)
                        {
                            //objItem.Relation = mst.Description;
                            //objItem.RelationID = mst.ID;
                            //MemberRelationsList.Add(objItem);
                            MemberRelationsList[i].RelationID = mst.ID;
                            //MemberRelationsList = MemberRelationsList.Select(r => { r.RelationID = mst.ID; return r; }).ToList();
                        }
                    }
                }

                cboMemberRelation.SelectedValue = 0;
                dgRelation.ItemsSource = null;
                dgRelation.ItemsSource = MemberRelationsList;
            }


        }

        private void txtServiceTaxPercentage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsPositiveDoubleValid() && textBefore != null)
            {

                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtServiceTaxPercentage_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtServiceTaxAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValidDigintWithTwoDecimalPlaces() && textBefore != null)
            {

                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtServiceTaxAmount_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtMaxRate_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (chkRateEditable.IsChecked == true && (txtServiceRate.Text != null && txtServiceRate.Text != "0" && txtServiceRate.Text != ""))
            //{
            //    if (txtMaxRate.Text != null && txtMaxRate.Text != "0")
            //        if (Convert.ToDouble(txtMaxRate.Text) > Convert.ToDouble(txtServiceRate.Text))
            //        {
            //            MessageBoxControl.MessageBoxChildWindow msgW =
            //                              new MessageBoxControl.MessageBoxChildWindow("", "Maximum Rate Should Be Less Than Base Rate", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //            msgW.Show();
            //            txtMaxRate.Text = txtServiceRate.Text;
            //        }

            //}
        }

        private void txtSeniorCitizenAge_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsItNumber() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
            //else if (((TextBox)sender).Text != string.Empty)
            //{
            //    if (Convert.ToInt32(txtSeniorCitizenAge.Text.Trim()) < 60 || Convert.ToInt32(txtSeniorCitizenAge.Text.Trim()) > 120)
            //    {
            //        MessageBoxControl.MessageBoxChildWindow msgW =
            //                          new MessageBoxControl.MessageBoxChildWindow("", "Please enter age Between 60 to 120 Years", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //        msgW.Show();
            //        txtSeniorCitizenAge.Text = "0";
            //    }
            //}
        }

        private void txtSeniorCitizenAge_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtSeniorCitizenAge_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtSeniorCitizenAge.Text != null && Convert.ToInt32(txtSeniorCitizenAge.Text.Trim()) > 0)
            {
                if (Convert.ToInt32(txtSeniorCitizenAge.Text.Trim()) < 60 || Convert.ToInt32(txtSeniorCitizenAge.Text.Trim()) > 120)
                {
                    txtSeniorCitizenAge.SetValidation("Please enter age Between 60 to 120 Years");
                    txtSeniorCitizenAge.RaiseValidationError();
                    txtSeniorCitizenAge.Focus();

                    //MessageBoxControl.MessageBoxChildWindow msgW =
                    //                  new MessageBoxControl.MessageBoxChildWindow("", "Please enter age Between 60 to 120 Years", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //msgW.Show();
                    txtSeniorCitizenAge.Text = "0";
                }
                else
                    txtSeniorCitizenAge.ClearValidationError();

            }
        }

        private void cmdAssignTariff_Click(object sender, RoutedEventArgs e)
        {
            if (grdServices.SelectedItem != null)
            {
                clsServiceMasterVO objServiceVO = new clsServiceMasterVO();
                objServiceVO = (clsServiceMasterVO)grdServices.SelectedItem;

                AssignTariffPopUp Win = new AssignTariffPopUp();
                Win.ServiceID = ((clsServiceMasterVO)grdServices.SelectedItem).ID;
                Win.Show();
                Win.GetSelectedServiceDetails(objServiceVO);

            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select the Service to link with Tariff.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
            }
        }

        private void txtCount_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCount.Text) && !txtCount.Text.IsItNumber())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = String.Empty;
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void cmdAssignConsent_Click(object sender, RoutedEventArgs e)
        {
            if (grdServices.SelectedItem != null)
            {
                clsServiceMasterVO objServiceVO = new clsServiceMasterVO();
                objServiceVO = (clsServiceMasterVO)grdServices.SelectedItem;

                AssignPackageConsent Win = new AssignPackageConsent();
                Win.ServiceID = ((clsServiceMasterVO)grdServices.SelectedItem).ID;
                Win.Show();
                //Win.GetSelectedServiceDetails(objServiceVO);

            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select the Service to link with Consent.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
            }
        }
    }
}
