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
using CIMS;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects.Patient;
using System.Collections.ObjectModel;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects;
using System.IO;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using System.Windows.Data;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;
using PalashDynamics.UserControls;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmLabTestFemale : ChildWindow, IInitiateCIMS
    {

        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "NEW":
                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW5.Show();
                        IsPatientExist = false;
                        break;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW5.Show();

                        IsPatientExist = false;
                        break;
                    }

                    IsPatientExist = true;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                    mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;
                    break;


            }
        }

        #endregion

        #region Properties
        public int PageSize
        {
            get
            {
                return MasterList.PageSize;
            }
            set
            {
                if (value == MasterList.PageSize) return;
                MasterList.PageSize = value;
                MasterListBack.PageSize = value;
                // OnPropertyChanged("PageSize");
            }
        }
        public byte[] AttachedFileContents { get; set; }

        #endregion

        #region Public Variables

        public bool IsPatientExist = false;
        public bool IsPageLoded = false;
        private SwivelAnimation objAnimation;
        string msgTitle = "PALASH DYNAMICS"; // "PALASHDYNAMICS";
        string msgText = "";
        long CoupleId { get; set; }
        long CoupleUnitId { get; set; }
        private clsCoupleVO _CoupleDetails = new clsCoupleVO();
        public clsCoupleVO CoupleDetails
        {
            get
            {
                return _CoupleDetails;
            }
            set
            {
                _CoupleDetails = value;
            }
        }
        public long ParamUnitID { get; set; }
        public long GRNReturnParamUnitID { get; set; }
        public string ModuleName { get; set; }
        public string Action { get; set; }
        public long CategoryId = 0, TestId = 0, ParamId = 0;
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        public PagedSortableCollectionView<clsPathoResultEntryVO> MasterList { get; private set; }
        public PagedSortableCollectionView<clsPathoResultEntryVO> MasterListBack { get; private set; }
        public bool IsCategory, IsTest, IsParameter, IsLabName, IsValue, IsResult, IsUnit, IsModify = false, IsCancel = true;
        byte[] data1;
        FileInfo Attachment1;
        public string fileName1;
        public bool IsNew = false;
        ObservableCollection<clsPathoResultEntryVO> TestList { get; set; }
        ObservableCollection<clsPathoResultEntryVO> TestListBack { get; set; }
        PagedCollectionView collection;
        System.Collections.ObjectModel.ObservableCollection<string> AttachedFileNameList = new System.Collections.ObjectModel.ObservableCollection<string>();
        WaitIndicator indicator = new WaitIndicator();

        #endregion

        public frmLabTestFemale(long PatientID, long PatientUnitID)
        {
            InitializeComponent();
            (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID = PatientID;
            (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId = PatientUnitID;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            IsNew = true;
            FillCategoryMaster();
            FillLabMaster();
            FillResultMaster();
            fillBackGrid();

            ProcDate.SelectedDate = DateTime.Now.Date.Date;
            ProcTime.Value = DateTime.Now;
            TestList = new ObservableCollection<clsPathoResultEntryVO>();
            TestListBack = new ObservableCollection<clsPathoResultEntryVO>();
            cmbLab.SelectedValue = (long)2;
            ParamId = 0;
            //this.Title = "Female Investigation (Name:- " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
            //         " " + ((IApplicationConfiguration)App.Current).SelectedPatient.LastName +")";

        }

        private void fillBackGrid()
        {
            try
            {
                clsGetPathoTestResultEntryDateWiseBizActionVO bizActionVO = new clsGetPathoTestResultEntryDateWiseBizActionVO();
                bizActionVO.SearchExpression = "";
                bizActionVO.PagingEnabled = true;
                //bizActionVO.Date = System.DateTime.Now.Date;
                //bizActionVO.MaximumRows = MasterListBack.PageSize;
                //bizActionVO.StartRowIndex = MasterListBack.PageIndex * MasterListBack.PageSize;
                bizActionVO.PatientID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
                // getstoreinfo = new clsIPDBedMasterVO();
                bizActionVO.UnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
                bizActionVO.fromform = false;
                bizActionVO.PathoResultEntry = new List<clsPathoResultEntryVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.PathoResultEntry = (((clsGetPathoTestResultEntryDateWiseBizActionVO)args.Result).PathoResultEntry);
                        // grdResultEntryBack.DataContext = null;
                        if (bizActionVO.PathoResultEntry.Count > 0)
                        {
                            //MasterListBack.Clear();
                            //MasterListBack.TotalItemCount = (int)(((clsGetPathoTestResultEntryDateWiseBizActionVO)args.Result).TotalRows);
                            TestListBack = new ObservableCollection<clsPathoResultEntryVO>();
                            foreach (clsPathoResultEntryVO item in bizActionVO.PathoResultEntry)
                            {
                                //MasterListBack.Add(item);
                                TestListBack.Add(item);
                            }
                            //PagedCollectionView collection = new PagedCollectionView(MasterListBack);
                            collection = new PagedCollectionView(TestListBack);
                            collection.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
                            //grdResultEntryBack.DataContext = collection;
                            grdResultEntryBack.ItemsSource = collection;
                            grdResultEntryBack.SelectedIndex = -1;
                        }
                    }
                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void FillCategoryMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_PathoCategory;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                //BizAction.MasterTable.f
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
                        cmbCategory.ItemsSource = null;
                        cmbCategory.ItemsSource = objList;
                        cmbCategory.SelectedItem = objList[0];
                    }
                    if (this.DataContext != null)
                    {
                        if (IsNew == true)
                            cmbCategory.SelectedValue = (long)0; //((clsPathoResultEntryVO)this.DataContext).CategoryID;
                        else
                            cmbCategory.SelectedValue = ((clsPathoResultEntryVO)this.DataContext).CategoryID;
                        //else if (((clsPathoResultEntryVO)grdResultEntry.SelectedItem).CategoryID != null && ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).CategoryID > 0)
                        //else if ((clsPathoResultEntryVO)grdResultEntry.SelectedItem != null)
                        //{
                        //    cmbCategory.SelectedValue = ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).CategoryID;
                        //}
                        //else
                        //    cmbCategory.SelectedValue = (long)0;
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

        private void FillTestMaster()
        {
            try
            {
                clsGetPathoTestListForResultEntryBizActionVO BizAction = new clsGetPathoTestListForResultEntryBizActionVO();
                BizAction.TestList = new List<clsPathoTestMasterVO>();
                if ((((IApplicationConfiguration)App.Current).SelectedPatient).Gender == Genders.None.ToString())
                    BizAction.ApplicaleTo = (int)Genders.None;
                else if ((((IApplicationConfiguration)App.Current).SelectedPatient).Gender == Genders.Male.ToString())
                    BizAction.ApplicaleTo = (int)Genders.Male;
                else if ((((IApplicationConfiguration)App.Current).SelectedPatient).Gender == Genders.Female.ToString())
                    BizAction.ApplicaleTo = (int)Genders.Female;
                BizAction.Category = CategoryId;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetPathoTestListForResultEntryBizActionVO)arg.Result).TestList != null)
                        {
                            clsGetPathoTestListForResultEntryBizActionVO result = arg.Result as clsGetPathoTestListForResultEntryBizActionVO;
                            List<clsPathoTestMasterVO> objList = new List<clsPathoTestMasterVO>();

                            List<MasterListItem> LstCheck = new List<MasterListItem>();
                            LstCheck.Add(new MasterListItem(0, "-- Select --"));
                            if (result.TestList != null)
                            {
                                objList.Clear();
                                foreach (var item in result.TestList)
                                {
                                    MasterListItem objMaster = new MasterListItem();
                                    objMaster.ID = item.ID;
                                    objMaster.Description = item.Description;
                                    LstCheck.Add(objMaster);
                                    //objList.Add(item);
                                }

                                cmbTest.ItemsSource = null;
                                cmbTest.ItemsSource = LstCheck;
                                //cmbTest.SelectedItem = LstCheck[0];
                            }

                            if (IsNew == true)
                                cmbTest.SelectedValue = (long)0; //((clsPathoResultEntryVO)this.DataContext).TestID;
                            else
                                cmbTest.SelectedValue = ((clsPathoResultEntryVO)this.DataContext).TestID;

                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }

                    //else if ((clsPathoResultEntryVO)grdResultEntry.SelectedItem != null)
                    //{
                    //    cmbTest.SelectedValue = ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).TestID;
                    //}

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void FillParameterMaster()
        {
            try
            {
                clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO BizAction = new clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO();
                BizAction.ParameterList = new List<clsPathoTestParameterVO>();
                BizAction.ItemList = new List<clsPathoTestItemDetailsVO>();
                BizAction.SampleList = new List<clsPathoTestSampleVO>();
                if (cmbTest.SelectedItem != null && ((MasterListItem)cmbTest.SelectedItem).ID > 0)
                {
                    BizAction.TestID = TestId;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO DetailsVO = new clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO();
                            DetailsVO = (clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO)arg.Result;

                            if (DetailsVO.ParameterList != null)
                            {

                                List<clsPathoTestParameterVO> objList = new List<clsPathoTestParameterVO>();

                                for (int i = 0; i < DetailsVO.ParameterList.Count; i++)
                                {
                                    clsPathoTestParameterVO obj = new clsPathoTestParameterVO();
                                    if (i == 0)
                                    {
                                        obj.ID = 0;
                                        obj.Description = "-- Select --";
                                        objList.Add(obj);
                                    }

                                    obj = DetailsVO.ParameterList[i];
                                    objList.Add(obj);
                                }
                                cmbParameter.ItemsSource = null;
                                cmbParameter.ItemsSource = objList;
                                cmbParameter.SelectedItem = objList[0];
                            }
                            if (this.DataContext != null)
                            {
                                //cmbParameter.SelectedValue = (long)0;
                                cmbParameter.SelectedValue = ((clsPathoResultEntryVO)this.DataContext).ParameterID;
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                            msgW1.Show();
                        }

                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void FillParameUnitMaster(long ParamId)
        {
            try
            {
                clsGetPathoParameterUnitBizActionVO BizAction = new clsGetPathoParameterUnitBizActionVO();
                BizAction.MasterList = new List<MasterListItem>();
                BizAction.ParamID = ParamId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.AddRange(((clsGetPathoParameterUnitBizActionVO)arg.Result).MasterList);
                        cmbUnit.ItemsSource = null;
                        {
                            objList.Add(new MasterListItem(0, "- Select -"));
                            cmbUnit.ItemsSource = objList;
                            if (this.DataContext != null)
                            {
                                if (IsNew == true)
                                    cmbUnit.SelectedValue = (long)0;
                                else
                                    cmbUnit.SelectedValue = ((clsPathoResultEntryVO)this.DataContext).ParameterUnitID;
                                //    cmbUnit.SelectedValue = ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).ParameterUnitID;
                            }
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        void FillLabMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_LaboratoryMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                //BizAction.MasterTable.f
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        cmbLab.ItemsSource = null;
                        cmbLab.ItemsSource = objList;
                        cmbLab.SelectedValue = (long)2;
                    }
                    if (this.DataContext != null)
                    {
                        cmbLab.SelectedValue = ((clsPathoResultEntryVO)this.DataContext).LabID;
                        cmbLab.SelectedValue = (long)2;
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

        void FillResultMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ResultTypeMaster;
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
                        cmbResultType.ItemsSource = null;
                        cmbResultType.ItemsSource = objList;
                    }
                    if (this.DataContext != null)
                    {
                        cmbResultType.SelectedValue = ((clsPathoResultEntryVO)this.DataContext).ResultType;
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FillHelpValueData(long ParamSTID)
        {
            try
            {
                clsGetHelpValuesFroResultEntryBizActionVO BizAction = new clsGetHelpValuesFroResultEntryBizActionVO();
                BizAction.HelpValueList = new List<clsPathoTestParameterVO>();
                BizAction.ParameterID = ParamSTID;    //ParameterID;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {

                        if (((clsGetHelpValuesFroResultEntryBizActionVO)arg.Result).HelpValueList != null)
                        {
                            List<clsPathoTestParameterVO> objList = new List<clsPathoTestParameterVO>();

                            clsPathoTestParameterVO obj = new clsPathoTestParameterVO();

                            obj.ID = 0;
                            obj.Description = "-- Select --";
                            objList.Add(obj);

                            List<clsPathoTestParameterVO> objHelpValueList = new List<clsPathoTestParameterVO>();

                            objHelpValueList.AddRange(((clsGetHelpValuesFroResultEntryBizActionVO)arg.Result).HelpValueList);

                            foreach (clsPathoTestParameterVO item in objHelpValueList)
                            {
                                clsPathoTestParameterVO objHelpItem = new clsPathoTestParameterVO();
                                objHelpItem = item;
                                objHelpItem.Description = item.HelpValue;

                                objList.Add(objHelpItem);
                            }

                            cmbValueType.ItemsSource = objList;

                            cmbValueType.SelectedValue = (long)0;

                            //dgHelpValueList.ItemsSource = ((clsGetHelpValuesFroResultEntryBizActionVO)arg.Result).HelpValueList;
                            //NormalRange = ((clsGetHelpValuesFroResultEntryBizActionVO)arg.Result).HelpValueList;

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
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ValidateForm()
        {
            if (((MasterListItem)cmbResultType.SelectedItem).ID > 0)
            {
                cmbResultType.TextBox.ClearValidationError();
                IsResult = true;
            }
            else
            {
                cmbResultType.TextBox.SetValidation("Please Select Result Type.");
                cmbResultType.TextBox.RaiseValidationError();
                IsResult = false;
            }
            if (((MasterListItem)cmbLab.SelectedItem).ID > 0)
            {
                cmbLab.TextBox.ClearValidationError();
                IsLabName = true;
            }
            else
            {
                cmbLab.TextBox.SetValidation("Please Select Lab.");
                cmbLab.TextBox.RaiseValidationError();
                IsLabName = false;
            }

            if (((clsPathoTestParameterVO)cmbParameter.SelectedItem).ID > 0)
            {
                cmbParameter.TextBox.ClearValidationError();
                IsParameter = true;
            }
            else
            {
                cmbParameter.TextBox.SetValidation("Please Select Parameter.");
                cmbParameter.TextBox.RaiseValidationError();
                IsParameter = false;
            }
            if (((MasterListItem)cmbTest.SelectedItem).ID > 0)
            {
                cmbTest.TextBox.ClearValidationError();
                IsTest = true;
            }
            else
            {
                cmbTest.TextBox.SetValidation("Please Select Test.");
                cmbTest.TextBox.RaiseValidationError();
                IsTest = false;
            }
            if (((MasterListItem)cmbCategory.SelectedItem).ID > 0)
            {
                cmbCategory.TextBox.ClearValidationError();
                IsCategory = true;
            }
            else
            {
                cmbCategory.TextBox.SetValidation("Please Select Test Category.");
                cmbCategory.TextBox.RaiseValidationError();
                IsCategory = false;
            }

        }

        private void CmdNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidateForm();
                if (IsCategory == true && IsTest == true && IsParameter == true && IsLabName == true)
                {
                    if (IsModify == false)
                    {
                        msgText = "Are You Sure \n  You Want To Save ?";
                        MessageBoxControl.MessageBoxChildWindow msgWin =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);
                        msgWin.Show();
                    }
                    else
                    {
                        msgText = "Are You Sure \n  You Want To Modify ?";
                        MessageBoxControl.MessageBoxChildWindow msgWin =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnModiyTestResult);
                        msgWin.Show();
                    }
                }
                else if (IsCategory == false)
                    cmbCategory.Focus();
                else if (IsTest == false)
                    cmbTest.Focus();
                else if (IsParameter == false)
                    cmbParameter.Focus();
                else if (IsLabName == false)
                    cmbLab.Focus();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private clsPathoResultEntryVO CreateFormData()
        {
            clsPathoResultEntryVO addNewResult = new clsPathoResultEntryVO();

            addNewResult.ID = 0;
            //addNewResult = (clsPathoResultEntryVO)this.grdBackPanel.DataContext;
            addNewResult.Status = true;

            if (grdResultEntryBack.SelectedItem != null)
                addNewResult.ID = ((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).ID;
            if ((MasterListItem)cmbCategory.SelectedItem != null)
                addNewResult.CategoryID = ((MasterListItem)cmbCategory.SelectedItem).ID;
            if ((MasterListItem)cmbTest.SelectedItem != null)
                addNewResult.TestID = ((MasterListItem)cmbTest.SelectedItem).ID;
            if ((clsPathoTestParameterVO)cmbParameter.SelectedItem != null)
                addNewResult.ParameterID = ((clsPathoTestParameterVO)cmbParameter.SelectedItem).ParamSTID; //((MasterListItem)cmbParameter.SelectedItem).ID;
            if ((MasterListItem)cmbLab.SelectedItem != null)
                addNewResult.LabID = ((MasterListItem)cmbLab.SelectedItem).ID;
            if (ProcDate.SelectedDate != null)
                addNewResult.Date = (DateTime)ProcDate.SelectedDate;
            if (ProcTime.Value != null)
                addNewResult.Time = (DateTime)ProcTime.Value;

            //if (txtValue.Text != null)
            //    addNewResult.ResultValue = txtValue.Text;

            if (((clsPathoTestParameterVO)cmbParameter.SelectedItem).IsNumeric == true)
            {
                if (txtValue.Text != null)
                    addNewResult.ResultValue = txtValue.Text;
            }
            else
            {
                if (cmbValueType.SelectedItem != null)
                {
                    addNewResult.ResultValue = ((clsPathoTestParameterVO)cmbValueType.SelectedItem).Description;
                }
            }

            if ((MasterListItem)cmbResultType.SelectedItem != null)
                addNewResult.ResultType = ((MasterListItem)cmbResultType.SelectedItem).ID;
            addNewResult.Note = txtNotes.Text;

            addNewResult.PatientID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            addNewResult.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;

            addNewResult.AttachmentFileName = fileName1;
            addNewResult.Attachment = AttachedFileContents;

            return addNewResult;
        }

        void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    clsPathoTestResultEntryBizActionVO bizActionVO = new clsPathoTestResultEntryBizActionVO();
                    clsPathoResultEntryVO addNewTestEntryVO = new clsPathoResultEntryVO();
                    if (cmbUnit.SelectedItem != null)
                    {
                        //bizActionVO.PathoResultEntry.ParameterUnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
                        addNewTestEntryVO.ParameterUnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
                    }

                    addNewTestEntryVO = (clsPathoResultEntryVO)this.DataContext;
                    addNewTestEntryVO = CreateFormData();
                    if ((MasterListItem)cmbUnit.SelectedItem != null)
                    {
                        addNewTestEntryVO.ParameterUnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
                        addNewTestEntryVO.ParameterUnitName = ((MasterListItem)cmbUnit.SelectedItem).Description;
                    }

                    addNewTestEntryVO.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewTestEntryVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewTestEntryVO.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewTestEntryVO.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewTestEntryVO.AddedDateTime = System.DateTime.Now;
                    addNewTestEntryVO.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizActionVO.PathoResultEntry = addNewTestEntryVO;
                    //bizActionVO.objBedMatserDetails.Add(addNewBlockVO);

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsPathoTestResultEntryBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Test Result Saved Successfully!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();

                                ClearUI();
                                fillBackGrid();
                            }
                            //else if (((clsPathoTestResultEntryBizActionVO)args.Result).SuccessStatus == 2)
                            //{
                            //    msgText = "Test Result Updated Successfully!";
                            //    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            //    msgWindow.Show();

                            //    ClearUI();
                            //    fillBackGrid();
                            //}
                            else
                            {
                                msgText = "Error Occured while Saving Test Result";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                        }
                    };
                    client.ProcessAsync(bizActionVO, new clsUserVO());
                    client.CloseAsync();
                }
                else
                {
                    if (IsCategory == false)
                        cmbCategory.Focus();
                    else if (IsTest == false)
                        cmbTest.Focus();
                    else if (IsParameter == false)
                        cmbParameter.Focus();
                    else if (IsLabName == false)
                        cmbLab.Focus();
                    else
                        cmbResultType.Focus();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void msgWin_OnModiyTestResult(MessageBoxResult result)
        {
            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    clsPathoTestResultEntryBizActionVO bizActionVO = new clsPathoTestResultEntryBizActionVO();
                    clsPathoResultEntryVO addNewTestEntryVO = new clsPathoResultEntryVO();

                    if (cmbUnit.SelectedItem != null)
                    {
                        //bizActionVO.PathoResultEntry.ParameterUnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
                        addNewTestEntryVO.ParameterUnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
                    }

                    addNewTestEntryVO = (clsPathoResultEntryVO)this.DataContext;
                    addNewTestEntryVO = CreateFormData();
                    if (((MasterListItem)cmbUnit.SelectedItem) != null)
                    {
                        addNewTestEntryVO.ParameterUnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
                        addNewTestEntryVO.ParameterUnitName = ((MasterListItem)cmbUnit.SelectedItem).Description;
                    }

                    addNewTestEntryVO.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewTestEntryVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewTestEntryVO.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewTestEntryVO.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewTestEntryVO.AddedDateTime = System.DateTime.Now;
                    addNewTestEntryVO.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizActionVO.PathoResultEntry = addNewTestEntryVO;
                    //bizActionVO.objBedMatserDetails.Add(addNewBlockVO);

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            //if (((clsPathoTestResultEntryBizActionVO)args.Result).SuccessStatus == 1)
                            //{
                            //    msgText = "Record Is Successfully Submitted!";
                            //    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            //    msgWindow.Show();

                            //    ClearUI();
                            //    fillBackGrid();
                            //}
                            //else 
                            if (((clsPathoTestResultEntryBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record Updated Successfully!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();

                                ClearUI();
                                fillBackGrid();
                                CmdSave.Content = "Save";
                                IsNew = true;
                                IsModify = false;
                            }
                            else
                            {
                                msgText = "Error Occured while Updating Test Result";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                        }
                    };
                    client.ProcessAsync(bizActionVO, new clsUserVO());
                    client.CloseAsync();
                }
                else
                {
                    if (IsCategory == false)
                        cmbCategory.Focus();
                    else if (IsTest == false)
                        cmbTest.Focus();
                    else if (IsParameter == false)
                        cmbParameter.Focus();
                    else if (IsLabName == false)
                        cmbLab.Focus();
                    else
                        cmbResultType.Focus();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void ClearUI()
        {
            this.grdBackPanel.DataContext = new clsPathoResultEntryVO();

            cmbCategory.SelectedValue = (long)0;//((clsPathoResultEntryVO)this.DataContext).CategoryID;
            cmbTest.SelectedValue = (long)0; //((clsPathoResultEntryVO)this.DataContext).TestID;
            cmbParameter.SelectedValue = (long)0;// ((clsPathoResultEntryVO)this.DataContext).ParameterID;
            cmbTest.ItemsSource = null;
            cmbParameter.ItemsSource = null;
            ProcDate.SelectedDate = DateTime.Now.Date.Date;
            ProcTime.Value = DateTime.Now;
            txtValue.Text = "";
            //cmbResultType.Text = "";
            cmbResultType.SelectedValue = (long)0;// ((clsPathoResultEntryVO)this.DataContext).ResultType;
            cmbParameter.SelectedValue = (long)0;
            txtNotes.Text = "";
            //hlbEditTemplate.Content = "";
        }

        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ViewImage_ClickBack(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).AttachmentFileName))
            {
                if (((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).Attachment != null)

                //if (AttachedFileContents != null)
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + ((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).AttachmentFileName });
                            AttachedFileNameList.Add(((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).AttachmentFileName);

                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", ((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).AttachmentFileName, ((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).Attachment);
                }
            }
        }

        #region File View Event

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFN.Text))
            {
                if (AttachedFileContents != null)
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + txtFN.Text.Trim() });
                            AttachedFileNameList.Add(txtFN.Text.Trim());

                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", txtFN.Text.Trim(), AttachedFileContents);
                }
            }
        }

        #endregion

        #region File Browse
        private void CmdBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == true)
            {

                txtFN.Text = openDialog.File.Name;
                try
                {
                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        AttachedFileContents = new byte[stream.Length];
                        stream.Read(AttachedFileContents, 0, (int)stream.Length);
                        fileName1 = openDialog.File.Name;
                    }
                }
                catch (Exception ex)
                {
                    string msgText = "Error while reading file.";

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow("Palsh", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
            }
        }
        #endregion

        private void cmdDeleteService_Click(object sender, RoutedEventArgs e)
        {
            if (grdResultEntryBack.SelectedItem != null)
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Delete the selected Item ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {

                        DeleteTheRecordBack();

                    }
                };
                msgWD.Show();
            }
        }

        private void DeleteTheRecordBack()
        {
            clsDeletePathoTestResultEntryBizActionVO BizAction = new clsDeletePathoTestResultEntryBizActionVO();
            clsPathoResultEntryVO objBizAction = new clsPathoResultEntryVO();
            objBizAction.ID = ((clsPathoResultEntryVO)this.grdResultEntryBack.SelectedItem).ID;
            BizAction.PathoResultEntry = objBizAction;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    msgText = "Record is successfully Deleted!";

                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                    msgWindow.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.OK)
                        {

                            fillBackGrid();

                        }
                    };
                    msgWindow.Show();


                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void cmdDeleteServiceBack_Click(object sender, RoutedEventArgs e)
        {
            if (grdResultEntryBack.SelectedItem != null)
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Delete the selected Item ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        DeleteTheRecord();

                    }
                };
                msgWD.Show();
            }
        }
        private void DeleteTheRecord()
        {
            clsDeletePathoTestResultEntryBizActionVO BizAction = new clsDeletePathoTestResultEntryBizActionVO();
            clsPathoResultEntryVO objBizAction = new clsPathoResultEntryVO();
            objBizAction.ID = ((clsPathoResultEntryVO)this.grdResultEntryBack.SelectedItem).ID;
            BizAction.PathoResultEntry = objBizAction;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    msgText = "Record is successfully Deleted!";

                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgWindow.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            DeleteTheRecord();

                        }
                    };
                    msgWindow.Show();
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void cmbCategory_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            cmbParameter.SelectedValue = (long)0;
            cmbTest.SelectedValue = (long)0;
            cmbUnit.SelectedValue = (long)0;
            if (cmbCategory.SelectedItem != null)
            {
                CategoryId = ((MasterListItem)cmbCategory.SelectedItem).ID;
                if (CategoryId > 0)
                    FillTestMaster();
            }
        }

        private void cmbTest_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTest.SelectedItem != null)
            {
                TestId = ((MasterListItem)cmbTest.SelectedItem).ID;
                if (TestId > 0)
                    FillParameterMaster();
            }
        }

        private void cmbParameter_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ParamId = 0;
            if (cmbParameter.SelectedItem != null)
            {
                ParamId = ((clsPathoTestParameterVO)cmbParameter.SelectedItem).ParamSTID;
                if (ParamId > 0)
                {
                    FillParameUnitMaster(ParamId);
                    // FillParameUnitMaster();
                }

                if (((clsPathoTestParameterVO)cmbParameter.SelectedItem).IsNumeric == true)
                {
                    txtValue.Visibility = Visibility.Visible;
                    cmbValueType.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (ParamId > 0)
                    {
                        txtValue.Visibility = Visibility.Collapsed;
                        cmbValueType.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        txtValue.Visibility = Visibility.Visible;
                        cmbValueType.Visibility = Visibility.Collapsed;
                    }

                    FillHelpValueData(ParamId);

                }

            }
        }

        private void txtStoreCode_LostFocus(object sender, RoutedEventArgs e)
        {
            clsGetResultOnParameterSelectionBizActionVO Bizaction = new clsGetResultOnParameterSelectionBizActionVO();

            if (cmbParameter.SelectedItem != null)
            {
                ParamId = ((clsPathoTestParameterVO)cmbParameter.SelectedItem).ParamSTID;
            }
            else
            {
                msgText = "Please Select Parameter. ";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
            }
            Bizaction.ParamID = ParamId;
            Bizaction.DOB = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).DateOfBirth;
            Bizaction.Gender = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).Gender;

            if (txtValue.Text != "" && txtValue.Text != String.Empty)
            {
                Bizaction.resultValue = Convert.ToDouble(txtValue.Text);
            }
            else
            {
                txtValue.Text = " 0 ";
                Bizaction.resultValue = Convert.ToDouble(txtValue.Text);
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList = cmbResultType.ItemsSource as List<MasterListItem>;

                    Bizaction.PathoResultEntry = new List<clsPathoResultEntryVO>();
                    Bizaction.PathoResultEntry = ((clsGetResultOnParameterSelectionBizActionVO)args.Result).PathoResultEntry;

                    cmbResultType.SelectedItem = objList.Where(z => z.ID == 0).FirstOrDefault();
                    foreach (clsPathoResultEntryVO item in Bizaction.PathoResultEntry)
                    {
                        if ((Bizaction.resultValue <= item.MaxValue && Bizaction.resultValue >= item.MinValue && item.MaxValue != 0 && item.MinValue != 0))
                        {
                            cmbResultType.SelectedItem = objList.Where(z => z.ID == 1).FirstOrDefault();
                        }
                        else
                        {
                            cmbResultType.SelectedItem = objList.Where(z => z.ID == 2).FirstOrDefault();
                        }
                    }
                }
            };
            client.ProcessAsync(Bizaction, new clsUserVO());
            client.CloseAsync();
        }

        private void hlbModify_ClickBack(object sender, RoutedEventArgs e)
        {

            if (grdResultEntryBack.SelectedItem != null)
            {
                this.DataContext = (clsPathoResultEntryVO)grdResultEntryBack.SelectedItem;
                IsNew = false;
                if (((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).CategoryID != null)
                    cmbCategory.SelectedValue = ((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).CategoryID;
                if (((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).TestID != null)
                    cmbTest.SelectedValue = ((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).TestID;
                if (((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).ParameterID != null)
                    cmbParameter.SelectedValue = ((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).ParameterID;
                if (((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).ParameterUnitID != null)
                    cmbUnit.SelectedValue = ((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).ParameterUnitID;
                if (((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).Date != null)
                    ProcDate.SelectedDate = ((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).Date;
                if (((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).Time != null)
                    ProcTime.Value = ((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).Time;

                //if (((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).ResultValue != null)
                //    txtValue.Text = ((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).ResultValue;

                if (((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).ResultValue != null)
                {
                    if (((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).IsNumeric == true)
                    {
                        txtValue.Text = ((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).ResultValue;
                    }
                    else
                    {
                        foreach (clsPathoTestParameterVO item in ((List<clsPathoTestParameterVO>)cmbValueType.ItemsSource))
                        {
                            if (item.ResultValue == ((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).ResultValue)
                            {
                                cmbValueType.SelectedValue = item.ID;
                                break;
                            }
                        }


                    }
                }

                if (((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).LabID != null)
                    cmbLab.SelectedValue = ((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).LabID;
                cmbResultType.SelectedValue = ((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).ResultType;
                if (((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).AttachmentFileName != null)
                    txtFN.Text = ((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).AttachmentFileName;
                if (((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).Note != null)
                    txtNotes.Text = ((clsPathoResultEntryVO)grdResultEntryBack.SelectedItem).Note;
                CmdSave.Content = "Modify";
                IsNew = false;
                IsModify = true;
            }

        }

        private void grdResultEntryBack_SelectedChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValueDouble() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtValue_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

    }
}