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
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Browser;
using System.Reflection;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects.Patient;
using System.Collections.ObjectModel;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects;
using System.IO;

using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using System.Windows.Data;

using PalashDynamics.UserControls;


namespace PalashDynamics.MIS.IVF
{
    public partial class PatientInvestigationReport : UserControl
    {

        public PatientInvestigationReport()
        {
            InitializeComponent();
        }
        public long ParamUnitID { get; set; }
        public long GRNReturnParamUnitID { get; set; }
        public string ModuleName { get; set; }
        public string Action { get; set; }
        public long CategoryId = 0, TestId = 0, ParamId = 0,ResultId=0;
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
        private clsPathoResultEntryVO CreateFormData()
        {
            clsPathoResultEntryVO addNewResult = new clsPathoResultEntryVO();

            addNewResult.ID = 0;
           
            addNewResult.Status = true;

           
            if ((MasterListItem)cmbCategory.SelectedItem != null)
                addNewResult.CategoryID = ((MasterListItem)cmbCategory.SelectedItem).ID;
            if ((MasterListItem)cmbTest.SelectedItem != null)
                addNewResult.TestID = ((MasterListItem)cmbTest.SelectedItem).ID;
            if ((clsPathoTestParameterVO)cmbParameter.SelectedItem != null)
                addNewResult.ParameterID = ((clsPathoTestParameterVO)cmbParameter.SelectedItem).ParamSTID; //((MasterListItem)cmbParameter.SelectedItem).ID;
          

          
            addNewResult.PatientID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            addNewResult.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;

            addNewResult.AttachmentFileName = fileName1;
            
            return addNewResult;
        }
        private void cmbCategory_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {


            if (cmbCategory.SelectedItem != null)
            {
                cmbParameter.SelectedValue = (long)0;
                cmbTest.SelectedValue = (long)0;
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

        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            Nullable<DateTime> dtpF = null;
            Nullable<DateTime> dtpT = null;
            Nullable<DateTime> dtpP = null;
            long CategoryId=0;
          
            bool chkToDate = true;
            string msgTitle = "";

            if (dtpFromDate.SelectedDate != null)
            {
                dtpF = dtpFromDate.SelectedDate.Value.Date.Date;
            }

            if (dtpToDate.SelectedDate != null)
            {
                dtpT = dtpToDate.SelectedDate.Value.Date.Date;
                if (dtpF.Value > dtpT.Value)
                {
                    dtpToDate.SelectedDate = dtpFromDate.SelectedDate;
                    dtpT = dtpF;
                    chkToDate = false;
                }
                else
                {
                    dtpP = dtpT;
                    dtpT = dtpT.Value.AddDays(1);
                }
            }

            if (dtpT != null)
            {
                if (dtpF != null)
                {
                    dtpF = dtpFromDate.SelectedDate.Value.Date.Date;
                
                }
            }
          
             if ((MasterListItem)cmbCategory.SelectedItem != null)
            {
                CategoryId = ((MasterListItem)cmbCategory.SelectedItem).ID;
            }


             if ((MasterListItem)cmbTest.SelectedItem != null)
             {
                 TestId = ((MasterListItem)cmbTest.SelectedItem).ID;
             }


             if ((clsPathoTestParameterVO)cmbParameter.SelectedItem != null)
             {
                 ParamId = ((clsPathoTestParameterVO)cmbParameter.SelectedItem).ParamSTID;
             }
             if ((MasterListItem)cmbResultType.SelectedItem != null)
             {
                 ResultId = ((MasterListItem)cmbResultType.SelectedItem).ID;
             }
           
            //if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            //{
            //    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((MasterListItem)cmbCategory.SelectedItem).ID)

            //        Category = 0;
            //}


       
           
            if (chkToDate == true)
            {
                string URL;
                if (dtpF != null && dtpT != null && dtpP != null)
                {
                    URL = "../Reports/IVF/PatientInvestigationReport.aspx?FromDate=" + dtpF.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + dtpT.Value.ToString("dd/MMM/yyyy") + "&CategoryId=" + CategoryId + "&TestId=" + TestId + "&ParamId=" + ParamId + "&ResultId=" + ResultId + "&ToDatePrint=" + dtpP.Value.ToString("dd/MMM/yyyy");
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {


                }
            }
            else
            {
                string msgText = "Incorrect Date Range. From Date cannot be greater than To Date.";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
            } 
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.IVF.IVFReport") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TestList = new ObservableCollection<clsPathoResultEntryVO>();
            TestListBack = new ObservableCollection<clsPathoResultEntryVO>();
            dtpFromDate.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date;
            FillCategoryMaster();
            FillResultMaster();
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
                        cmbResultType.SelectedItem = objList[0];
                    }
                    if (this.DataContext != null)
                    {
                        cmbResultType.SelectedValue = ((clsPathoResultEntryVO)this.DataContext).ResultType;
                    }
                };
                //if (this.DataContext != null)
                //{
                //    if (IsNew == true)
                //        cmbResultType.SelectedValue = (long)0; //((clsPathoResultEntryVO)this.DataContext).CategoryID;
                //    else
                //        cmbResultType.SelectedValue = ((clsPathoResultEntryVO)this.DataContext).CategoryID;

               // }
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void FillTestMaster()
        {
            try
            {
                clsGetPathoTestListForResultEntryBizActionVO BizAction = new clsGetPathoTestListForResultEntryBizActionVO();
                BizAction.TestList = new List<clsPathoTestMasterVO>();
                //if ((((IApplicationConfiguration)App.Current).SelectedPatient).Gender == Genders.None.ToString())
                //    BizAction.ApplicaleTo = (int)Genders.None;
                //else if ((((IApplicationConfiguration)App.Current).SelectedPatient).Gender == Genders.Male.ToString())
                //    BizAction.ApplicaleTo = (int)Genders.Male;
                //else if ((((IApplicationConfiguration)App.Current).SelectedPatient).Gender == Genders.Female.ToString())
                //    BizAction.ApplicaleTo = (int)Genders.Female;
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
                               // cmbTest.SelectedItem = LstCheck[0];
                            }
                            cmbTest.SelectedValue = (long)0; 
                            //if (IsNew == true)
                            //    cmbTest.SelectedValue = (long)0; //((clsPathoResultEntryVO)this.DataContext).TestID;
                            //else
                            //    cmbTest.SelectedValue = ((clsPathoResultEntryVO)this.DataContext).TestID;

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

    }
}
