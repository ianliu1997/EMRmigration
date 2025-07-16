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
using PalashDynamics.ValueObjects;
using System.IO;
using System.Xml.Linq;
using System.Windows.Resources;
using System.Reflection;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.IPD;
using System.ComponentModel;
using PalashDynamics.Collections;
using MessageBoxControl;
using System.Windows.Browser;
using PalashDynamics.IPD.Forms;

namespace PalashDynamics.IPD
{
    //Added by kiran
    public partial class frmIntakeOutputChart : UserControl, IPreInitiateCIMS
    {
        #region IPreInitiateCIMS Members

        PalashDynamics.ValueObjects.Administration.clsMenuVO _SelfMenuDetails = null;

        public void PreInitiate(PalashDynamics.ValueObjects.Administration.clsMenuVO _MenuDetails)
        {
            _SelfMenuDetails = _MenuDetails;
        }

        #endregion

        public string ModuleName { get; set; }
        public string Action { get; set; }
        string msgTitle = "PALASHDYNAMICS";
        string msgText = "";
        bool IsViewClick;
        long AdmUnitID, AdmID, ID, UnitID;
        private clsIPDAdmissionVO obj = null;
        private clsIPDIntakeOutputChartVO objint = new clsIPDIntakeOutputChartVO();
        UIElement objPatientSearch = null;
        List<clsIPDIntakeOutputChartVO> ObjAddList = null;
        public PagedSortableCollectionView<clsIPDIntakeOutputChartVO> DataList { get; private set; }

        public frmIntakeOutputChart()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmIntakeOutputChart_Loaded);
        }

        #region Added By Arati For Call From Nursing Console
        long NursingConsolePatientID = 0, NursingConsolePatientUnitID = 0,
        NursingConsoleAdmissionID = 0, NursingConsoleAdmissionUnitID = 0;
        bool IsFromNursingConsole = false;


        public frmIntakeOutputChart(long PatientID, long PatientUnitID, long AdmissionID, long AdmissionUnitID, bool IsNursingConsole)
        {
            NursingConsolePatientID = PatientID;
            NursingConsolePatientUnitID = PatientUnitID;
            NursingConsoleAdmissionID = AdmissionID;
            NursingConsoleAdmissionUnitID = AdmissionUnitID;
            IsFromNursingConsole = IsNursingConsole;
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmIntakeOutputChart_Loaded);
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
            mElement.Text = "Intake Output Chart";
        }
        #endregion

        #region Properties

        public int PageSizeData
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;

            }
        }
        #endregion

        void frmIntakeOutputChart_Loaded(object sender, RoutedEventArgs e)
        {
            DataList = new PagedSortableCollectionView<clsIPDIntakeOutputChartVO>();
            txtDate.Text = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
            DefaultValueForGrid();
            objint.IsEnable = true;
            dtpDate.SelectedDate = DateTime.Now;
            CommandButtonInputOutputChart("New");
            CmdPatientAppSearch.IsEnabled = true;
            cmdPatientSearch.IsEnabled = true;
            // Added By Arati For Call From Nursing Console
            if (IsFromNursingConsole && NursingConsolePatientID > 0)
            {
                obj = new clsIPDAdmissionVO();
                obj.AdmID = NursingConsoleAdmissionID;
                obj.AdmUnitID = NursingConsoleAdmissionUnitID;
                AdmID = NursingConsoleAdmissionID;
                AdmUnitID = NursingConsoleAdmissionUnitID;
                FindPatient(NursingConsolePatientID, NursingConsolePatientUnitID, "");
                CmdPatientAppSearch.IsEnabled = false;
                cmdPatientSearch.IsEnabled = false;
            }

            if (dgInputOutputList != null)
            {
                if (dgInputOutputList.Columns.Count > 0)
                {

                    if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    {
                        dgInputOutputList.Columns[0].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdSrNo");
                        dgInputOutputList.Columns[1].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdDate");
                        dgInputOutputList.Columns[2].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdTotalIntake");
                        dgInputOutputList.Columns[3].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdTotalOutput");
                        dgInputOutputList.Columns[4].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdIsFreezed");
                    }
                    else
                    {
                        dgInputOutputList.Columns[0].Header = "Sr No";
                        dgInputOutputList.Columns[1].Header = "Date";
                        dgInputOutputList.Columns[2].Header = "Total Intake";
                        dgInputOutputList.Columns[3].Header = "Total Output";
                        dgInputOutputList.Columns[4].Header = "Is Freezed";
                    }
                }
            }

            if (grdInPutOutput != null)
            {
                if (grdInPutOutput.Columns.Count > 0)
                {

                    if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    {
                        grdInPutOutput.Columns[0].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdTime");
                        grdInPutOutput.Columns[1].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdOralNg");
                        grdInPutOutput.Columns[2].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdTotalParental");
                        grdInPutOutput.Columns[3].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdOthers");
                        grdInPutOutput.Columns[4].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdTotalIntake");
                        grdInPutOutput.Columns[5].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdUrine");
                        grdInPutOutput.Columns[6].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdNg");
                        grdInPutOutput.Columns[7].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdDrain");
                        grdInPutOutput.Columns[8].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdOthers");
                        grdInPutOutput.Columns[9].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdTotalOutput");
                    }
                    else
                    {
                        grdInPutOutput.Columns[0].Header = "Time";
                      //  grdInPutOutput.Columns[1].Header = "Oral /Ng";
                          grdInPutOutput.Columns[1].Header = "IV Fluid/Oral";
                        
                        grdInPutOutput.Columns[2].Header = "Total Parental";
                        grdInPutOutput.Columns[3].Header = "Others";
                        grdInPutOutput.Columns[4].Header = "Total Intake";
                        grdInPutOutput.Columns[5].Header = "Urine";
                        grdInPutOutput.Columns[6].Header = "Ng";
                        grdInPutOutput.Columns[7].Header = "Drain";
                        grdInPutOutput.Columns[8].Header = "Others";
                        grdInPutOutput.Columns[9].Header = "Total Output";
                    }
                }
            }


        }

        # region Function For DefaultValueForGrid
        private void DefaultValueForGrid()
        {
            List<defaulClass> obj = null;
            grdInPutOutput.ItemsSource = null;

            obj = new List<defaulClass>();
            obj.Clear();

            int k = 16;
            int p = 0;

            int[] Ids = new int[5] { 9, 18, 19, 28, 29 };

            for (int i = 1, j = 0; i <= 29; i++)
            {
                if (!Ids.Contains(i))
                {
                    obj.Add(new defaulClass() { RowID = (i), Time = ((p + 8) % 24) + ":00", OralNg = 0, TotalParental = 0, IntakeOthers = 0, IntakeTotal = 0, Urine = 0, Ng = 0, Drain = 0, OutputOthers = 0, OutputTotal = 0, Styles = "", ReadOnly = "False" });
                    p++;
                }
                else
                {
                    j++;
                    // obj.Add(new defaulClass() { RowID = (i), Time = "8hrs Total", OralNg = 0, TotalParental = 0, IntakeOthers = 0, IntakeTotal = 0, Urine = 0, Ng = 0, Drain = 0, OutputOthers = 0, OutputTotal = 0, Styles = "Bold", ReadOnly = "True" });
                    if (j > 1)
                    {
                        i++;
                        // obj.Add(new defaulClass() { RowID = (i), Time = k + "hrs Total", OralNg = 0, TotalParental = 0, IntakeOthers = 0, IntakeTotal = 0, Urine = 0, Ng = 0, Drain = 0, OutputOthers = 0, OutputTotal = 0, Styles = "Bold", ReadOnly = "True" });
                        k = k + 8;
                    }
                }
            }

            grdInPutOutput.ItemsSource = obj;



        }

        #endregion

        #region Function For Calculate IntakeOutput Grid
        private void CalculateIntakeOutputGrid()
        {
            List<defaulClass> objList = ((List<defaulClass>)grdInPutOutput.ItemsSource);

            if (objList != null && objList.Count > 0)
            {
                int LessCount = 1, i = 1;
                bool IsSumTaken = false;
                foreach (defaulClass item in objList)
                {
                    int[] Ids = new int[5] { 9, 18, 19, 28, 29 };

                    //if ((item.RowID % 9) != 0 && !IsSumTaken)
                    if (!Ids.Contains(item.RowID))
                    {
                        item.IntakeTotal = item.OralNg + item.TotalParental + item.IntakeOthers;
                        item.OutputTotal = item.Urine + item.Ng + item.Drain + item.OutputOthers;
                    }
                    else
                    {
                        if (IsSumTaken)
                        {
                            int count = LessCount;
                            LessCount = 1;

                            item.OralNg = (from obj in objList where !Ids.Contains(obj.RowID) && obj.RowID < item.RowID && obj.RowID >= LessCount select obj).Sum(S => S.OralNg); ;
                            item.TotalParental = (from obj in objList where !Ids.Contains(obj.RowID) && obj.RowID < item.RowID && obj.RowID >= LessCount select obj).Sum(S => S.TotalParental);
                            item.IntakeOthers = (from obj in objList where !Ids.Contains(obj.RowID) && obj.RowID < item.RowID && obj.RowID >= LessCount select obj).Sum(S => S.IntakeOthers);
                            item.IntakeTotal = (from obj in objList where !Ids.Contains(obj.RowID) && obj.RowID < item.RowID && obj.RowID >= LessCount select obj).Sum(S => S.IntakeTotal);
                            item.Urine = (from obj in objList where !Ids.Contains(obj.RowID) && obj.RowID < item.RowID && obj.RowID >= LessCount select obj).Sum(S => S.Urine);
                            item.Ng = (from obj in objList where !Ids.Contains(obj.RowID) && obj.RowID < item.RowID && obj.RowID >= LessCount select obj).Sum(S => S.Ng);
                            item.Drain = (from obj in objList where !Ids.Contains(obj.RowID) && obj.RowID < item.RowID && obj.RowID >= LessCount select obj).Sum(S => S.Drain);
                            item.OutputOthers = (from obj in objList where !Ids.Contains(obj.RowID) && obj.RowID < item.RowID && obj.RowID >= LessCount select obj).Sum(S => S.OutputOthers);
                            item.OutputTotal = (from obj in objList where !Ids.Contains(obj.RowID) && obj.RowID < item.RowID && obj.RowID >= LessCount select obj).Sum(S => S.OutputTotal);

                            IsSumTaken = false;
                            LessCount = count;
                        }
                        else
                        {
                            item.OralNg = objList.Where(S => S.RowID < item.RowID && S.RowID >= LessCount).Sum(S => S.OralNg);
                            item.TotalParental = objList.Where(S => S.RowID < item.RowID && S.RowID >= LessCount).Sum(S => S.TotalParental);
                            item.IntakeOthers = objList.Where(S => S.RowID < item.RowID && S.RowID >= LessCount).Sum(S => S.IntakeOthers);
                            item.IntakeTotal = objList.Where(S => S.RowID < item.RowID && S.RowID >= LessCount).Sum(S => S.IntakeTotal);
                            item.Urine = objList.Where(S => S.RowID < item.RowID && S.RowID >= LessCount).Sum(S => S.Urine);
                            item.Ng = objList.Where(S => S.RowID < item.RowID && S.RowID >= LessCount).Sum(S => S.Ng);
                            item.Drain = objList.Where(S => S.RowID < item.RowID && S.RowID >= LessCount).Sum(S => S.Drain);
                            item.OutputOthers = objList.Where(S => S.RowID < item.RowID && S.RowID >= LessCount).Sum(S => S.OutputOthers);
                            item.OutputTotal = objList.Where(S => S.RowID < item.RowID && S.RowID >= LessCount).Sum(S => S.OutputTotal);

                            if (!i.Equals(1))
                            {
                                IsSumTaken = true;
                            }
                            LessCount = item.RowID + i;
                            i++;
                        }

                    }
                }
            }


            //int k = 16;
            //int p = 0;

            //long totaleight = 0;
            //long totalsi = 0;
            //long totaleight = 0;
            //long totalOral = 0;

            //for (int i = 0, j = 0; i < 27; i++)
            //{
            //    if (((i + 1) % 9) != 0)
            //    {
            //        objList[i].IntakeTotal = Convert.ToString(Convert.ToInt64(objList[i].OralNg) + Convert.ToInt64(objList[i].TotalParental) + Convert.ToInt64(objList[i].IntakeOthers));
            //        objList[i].OutputTotal = Convert.ToString(Convert.ToInt64(objList[i].Urine) + Convert.ToInt64(objList[i].Ng) + Convert.ToInt64(objList[i].Drain) + Convert.ToInt64(objList[i].OutputOthers));
            //        //objList.Add(new defaulClass() { Time = ((p + 8) % 24) + ":00", OralNg = "", TotalParental = "", IntakeOthers = "", IntakeTotal = "", Urine = "", Ng = "", Drain = "", OutputOthers = "", OutputTotal = "", Styles = "", ReadOnly = "False" });
            //        totalOral = Convert.ToInt64(objList[i].OralNg);


            //        p++;
            //    }
            //    else
            //    {
            //        j++;
            //        objList.Add(new defaulClass() { Time = "8hrs Total", OralNg = "", TotalParental = "", IntakeOthers = "", IntakeTotal = "", Urine = "", Ng = "", Drain = "", OutputOthers = "", OutputTotal = "", Styles = "Bold", ReadOnly = "True" });
            //        totaleight = totalOral;
            //        if (j > 1)
            //        {
            //            objList.Add(new defaulClass() { Time = k + "hrs Total", OralNg = "", TotalParental = "", IntakeOthers = "", IntakeTotal = "", Urine = "", Ng = "", Drain = "", OutputOthers = "", OutputTotal = "", Styles = "Bold", ReadOnly = "True" });
            //            k = k + 8;
            //        }
            //    }
            //}

            //grdInPutOutput.ItemsSource = objList;
        }

        #endregion


        #region Save
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {

            if (obj != null)
            {
                if (obj.AdmID > 0)
                {
                    if (checkValidation())
                    {
                        string msgTitle = "Palash";
                        if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                        {
                            msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("SaveVerification_Msg");
                        }
                        else
                        {
                            msgText = "Are you sure you want to save ?";
                        }

                        //string msgText = "Are you sure you want to save the Intake Output Chart Details ?";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                        msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_SaveOnMessageBoxClosed);

                        msgW.Show();
                    }
                    else
                    {
                        if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                        {
                            msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DateLessorEqualCurrentDateValidation_Msg");
                        }
                        else
                        {
                            msgText = "Date should be less or equal to current date";
                        }

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                    }
                }
                else
                {
                    if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    {
                        msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("PatientValidation_Msg");
                    }
                    else
                    {
                        msgText = "Please select patient.";
                    }

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                }
            }
            else
            {
                if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                {
                    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("PatientValidation_Msg");
                }
                else
                {
                    msgText = "Please select patient.";
                }

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
            }
        }

        private bool checkValidation()
        {
            bool result = true;

            if (dtpDate.SelectedDate != null)
            {
                if (dtpDate.SelectedDate.Value.Date > DateTime.Now.Date)
                {
                    result = false;

                    dtpDate.BorderBrush = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    dtpDate.BorderBrush = new SolidColorBrush(Colors.Gray);
                }
            }
            else
            {
                dtpDate.BorderBrush = new SolidColorBrush(Colors.Red);
                result = false;
            }

            return result;
        }

        void msgW_SaveOnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                SaveIntakeOutputChartDetail();
            }
        }

        private void SaveIntakeOutputChartDetail()
        {
            clsAddUpdateIntakeOutputChartAndDetailsBizActionVO BizAction = new clsAddUpdateIntakeOutputChartAndDetailsBizActionVO();
            try
            {
                BizAction.IntakeOutputDetails = new clsIPDIntakeOutputChartVO();
                BizAction.IntakeOutputList = new List<clsIPDIntakeOutputChartVO>();
                BizAction.IntakeOutputDetails.AdmID = obj.AdmID;
                BizAction.IntakeOutputDetails.AdmUnitID = obj.AdmUnitID;
                BizAction.IntakeOutputDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizAction.IntakeOutputDetails.Status = true;

                if (dtpDate.SelectedDate != null)
                    BizAction.IntakeOutputDetails.Date = dtpDate.SelectedDate;

                if (grdInPutOutput.ItemsSource != null)
                {
                    int[] Ids = new int[5] { 9, 18, 19, 28, 29 };
                    foreach (defaulClass item in ((List<defaulClass>)grdInPutOutput.ItemsSource))
                    {
                        if (!Ids.Contains(item.RowID))
                        {
                            clsIPDIntakeOutputChartVO objIntake = new clsIPDIntakeOutputChartVO();
                            objIntake.strTime = item.Time;
                            objIntake.Oral = item.OralNg;
                            objIntake.Total_Parenteral = item.TotalParental;
                            objIntake.OtherIntake = item.IntakeOthers;
                            objIntake.IntakeTotal = item.IntakeTotal;
                            objIntake.Urine = item.Urine;
                            objIntake.Ng = item.Ng;
                            objIntake.Drain = item.Drain;
                            objIntake.OtherOutput = item.OutputOthers;
                            objIntake.OutputTotal = item.OutputTotal;

                            BizAction.IntakeOutputList.Add(objIntake);
                        }
                    }
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if ((clsAddUpdateIntakeOutputChartAndDetailsBizActionVO)arg.Result != null)
                        {
                            //ClearControl();
                            if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                            {
                                msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("RecordSaved_Msg");
                            }
                            else
                            {
                                msgText = "Record saved successfully.";
                            }
                            //msgText="Intake Output Chart Details added successfully."

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                            DefaultValueForGrid();
                            GetIntakeOutputChartDetails(patientDetails.PatientID, patientDetails.PatientUnitID);
                            CommandButtonInputOutputChart("Save");
                        }
                    }
                    else
                    {
                        if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                        {
                            msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorWhileSaveModify_Msg");
                        }
                        else
                        {
                            msgText = "Error occured while processing.";
                        }

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
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

        # endregion

        #region Modify
        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            if (checkValidation())
            {
                string msgTitle = "Palash";
                if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                {
                    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("UpdateVerification_Msg");
                }
                else
                {
                    msgText = "Are you sure you want to update?";
                }

                //string msgText = "Are you sure you want to Update IntakeOutput chart Details?";
                MessageBoxControl.MessageBoxChildWindow msgW = null;
                msgW = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_MessageBoxClosed);
                msgW.Show();
            }
            else
            {
                if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                {
                    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DateLessorEqualCurrentDateValidation_Msg");
                }
                else
                {
                    msgText = "Date should be less or equal to current date";
                }

                //msgText="Date should be less than or equal to current date."
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
            }
        }

        void msgW_MessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                ModifyIntakeOutputChartDetail();
            }
        }



        private void ModifyIntakeOutputChartDetail()
        {
            if (dgInputOutputList.SelectedItem != null)
            {
                clsAddUpdateIntakeOutputChartAndDetailsBizActionVO BizAction = new clsAddUpdateIntakeOutputChartAndDetailsBizActionVO();
                try
                {
                    BizAction.IntakeOutputDetails = new clsIPDIntakeOutputChartVO();
                    BizAction.IntakeOutputDetails = (clsIPDIntakeOutputChartVO)dgInputOutputList.SelectedItem;
                    BizAction.IntakeOutputList = new List<clsIPDIntakeOutputChartVO>();
                    BizAction.IntakeOutputDetails.AdmID = obj.AdmID;
                    BizAction.IntakeOutputDetails.AdmUnitID = obj.AdmUnitID;
                    BizAction.IntakeOutputDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    BizAction.IntakeOutputDetails.Status = true;

                    if (dtpDate.SelectedDate != null)
                        BizAction.IntakeOutputDetails.Date = dtpDate.SelectedDate;

                    if (grdInPutOutput.ItemsSource != null)
                    {
                        int[] Ids = new int[5] { 9, 18, 19, 28, 29 };
                        foreach (defaulClass item in ((List<defaulClass>)grdInPutOutput.ItemsSource))
                        {
                            if (!Ids.Contains(item.RowID))
                            {
                                clsIPDIntakeOutputChartVO objIntake = new clsIPDIntakeOutputChartVO();
                                objIntake.strTime = item.Time;
                                objIntake.Oral = item.OralNg;
                                objIntake.Total_Parenteral = item.TotalParental;
                                objIntake.OtherIntake = item.IntakeOthers;
                                objIntake.IntakeTotal = item.IntakeTotal;
                                objIntake.Urine = item.Urine;
                                objIntake.Ng = item.Ng;
                                objIntake.Drain = item.Drain;
                                objIntake.OtherOutput = item.OutputOthers;
                                objIntake.OutputTotal = item.OutputTotal;

                                BizAction.IntakeOutputList.Add(objIntake);
                            }
                        }
                    }

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            if ((clsAddUpdateIntakeOutputChartAndDetailsBizActionVO)arg.Result != null)
                            {
                                //ClearControl();
                                if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                                {
                                    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("RecordModify_Msg");
                                }
                                else
                                {
                                    msgText = "Record updated successfully.";
                                }

                                //msgText="Intake Output Chart Details Updated Succesfully."
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                msgW1.Show();
                                DefaultValueForGrid();
                                GetIntakeOutputChartDetails(patientDetails.PatientID, patientDetails.PatientUnitID);
                            }
                        }
                        else
                        {
                            if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                            {
                                msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorWhileSaveModify_Msg");
                            }
                            else
                            {
                                msgText = "Error occured while processing.";
                            }

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                            msgW1.Show();
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

        }
        #endregion

        #region Update Status
        private void cmdUpdateStatusDetails_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatusIntakeOutputChart();
        }

        public void UpdateStatusIntakeOutputChart()
        {
            clsUpdateStatusIntakeOutputChartBizActionVO BizAction = new clsUpdateStatusIntakeOutputChartBizActionVO();
            try
            {
                if (dgInputOutputList.SelectedItem != null)
                {
                    BizAction.IntakeOutputDetails = new clsIPDIntakeOutputChartVO();
                    clsIPDIntakeOutputChartVO objIntakeOut = (clsIPDIntakeOutputChartVO)dgInputOutputList.SelectedItem;
                    BizAction.IntakeOutputDetails.ID = objIntakeOut.ID;
                    BizAction.IntakeOutputDetails.UnitID = objIntakeOut.UnitID;

                    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                    Client1.ProcessCompleted += (s, arg) =>
                    {

                        if (arg.Error == null && arg.Result != null)
                        {
                            if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                            {
                                msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DeletVerify_Msg");
                            }
                            else
                            {
                                msgText = "Record deleted Successfully.";
                            }

                            //msgText = "Record deleted successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                            //if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID > 0)
                            //    GetAutoChargesServiceList(((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID);
                            CommandButtonInputOutputChart("Modify");
                            GetIntakeOutputChartDetails(patientDetails.PatientID, patientDetails.PatientUnitID);
                            DefaultValueForGrid();
                        }
                        else
                        {

                        }
                    };
                    Client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client1.CloseAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion

        private void grdInPutOutput_LostFocus(object sender, RoutedEventArgs e)
        {
            CalculateIntakeOutputGrid();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //if (IsFromNursingConsole)//Added By Arati for Call From Nursing Console
            //{
            //    frmNursingConsole NursingConsole = new frmNursingConsole(true, NursingConsolePatientID, NursingConsolePatientUnitID);
            //    ((IApplicationConfiguration)App.Current).OpenMainContent(NursingConsole);
            //}
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            frmAdmissionList _AdmissionListObject = new frmAdmissionList();
            ((IApplicationConfiguration)App.Current).OpenMainContent(_AdmissionListObject);
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
            mElement.Text = "Admission List";

        }

        #region Added for Get Intake Output Chart Details
        bool isFreezed = false;
        private void GetIntakeOutputChartDetails(long PatientID, long PatientUnitID)
        {
            clsGetIntakeOutputChartDetailsBizActionVO BizAction = new clsGetIntakeOutputChartDetailsBizActionVO();
            
            BizAction.GetIntakeOutputList = new List<clsIPDIntakeOutputChartVO>();
            BizAction.GetIntakeOutputDetails = new clsIPDIntakeOutputChartVO();
            BizAction.GetIntakeOutputDetails.PatientID = PatientID;
            BizAction.GetIntakeOutputDetails.PatientUnitID = PatientUnitID;
           
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    clsGetIntakeOutputChartDetailsBizActionVO result = arg.Result as clsGetIntakeOutputChartDetailsBizActionVO;
                    DataList.TotalItemCount = result.TotalRows;

                    if (result.GetIntakeOutputList != null)
                    {
                        DataList.Clear();
                        foreach (var item in result.GetIntakeOutputList)
                        {
                            DataList.Add(item);                           
                            item.IsEnable = !(bool)item.IsFreezed;
                        }                       
                        dgInputOutputList.ItemsSource = null;
                        dgInputOutputList.ItemsSource = DataList;
                        dgInputOutputList.SelectedItem = null;
                        CommandButtonInputOutputChart("New");
                    }
                }

            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }


        bool Freezed = false;

        //Added For Get Intake Output Grid
        private void GetIntakeOutputChartDetailsByPatientID(clsIPDIntakeOutputChartVO objIntake)
        {
            clsGetIntakeOutputChartDetailsByPatientIDBizActionVO BizAction = new clsGetIntakeOutputChartDetailsByPatientIDBizActionVO();
            BizAction.GetIntakeOutputList = new List<clsIPDIntakeOutputChartVO>();
            BizAction.GetIntakeOutputDetails = new clsIPDIntakeOutputChartVO();

            BizAction.GetIntakeOutputDetails.IntakeOutputID = objIntake.ID;
            BizAction.GetIntakeOutputDetails.IntakeOutputIDUnitID = objIntake.UnitID;
            BizAction.GetIntakeOutputDetails.Date = objIntake.Date;
            //BizAction.GetVitalSDetails.Date = dtpTPRDate.SelectedDate.Value.Date;
            BizAction.IsPagingEnabled = false;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    clsGetIntakeOutputChartDetailsByPatientIDBizActionVO result = arg.Result as clsGetIntakeOutputChartDetailsByPatientIDBizActionVO;

                    if (result.GetIntakeOutputList != null)
                    {
                        List<defaulClass> objDefaultList = ((List<defaulClass>)grdInPutOutput.ItemsSource);
                        foreach (defaulClass item in objDefaultList)
                        {
                            foreach (clsIPDIntakeOutputChartVO obj in result.GetIntakeOutputList)
                            {

                                if (item.Time.Equals(obj.strTime))
                                {

                                    item.Time = obj.strTime;
                                    item.OralNg = obj.Oral;
                                    item.TotalParental = obj.Total_Parenteral;
                                    item.IntakeOthers = obj.OtherIntake;
                                    item.IntakeTotal = obj.IntakeTotal;
                                    item.Urine = obj.Urine;
                                    item.Ng = obj.Ng;
                                    item.Drain = obj.Drain;
                                    item.OutputOthers = obj.OtherOutput;
                                    item.OutputTotal = obj.OutputTotal;
                                }
                                Freezed = (bool)obj.IsFreezed;
                            }
                        }
                        if (Freezed.Equals(true))
                        {
                            grdInPutOutput.ItemsSource = null;
                            grdInPutOutput.ItemsSource = objDefaultList;
                            btnModify.IsEnabled = false;

                        }
                        else
                        {
                            grdInPutOutput.ItemsSource = null;
                            grdInPutOutput.ItemsSource = objDefaultList;
                        }
                        CalculateIntakeOutputGrid();
                    }

                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        #endregion

        private void CommandButtonInputOutputChart(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":

                    btnModify.IsEnabled = false;
                    btnSave.IsEnabled = true;
                    btnClose.IsEnabled = true;
                    break;

                case "Save":
                    btnModify.IsEnabled = false;
                    btnSave.IsEnabled = true;
                    btnClose.IsEnabled = true;
                    break;

                case "Modify":
                    btnClose.IsEnabled = true;
                    btnSave.IsEnabled = true;
                    btnModify.IsEnabled = false;
                    // btnNew.IsEnabled = true;
                    break;

                case "Cancel":
                    //btnNew.IsEnabled = true;
                    //cmdModify.IsEnabled = false;
                    //btnCloseEmergency.IsEnabled = false;
                    break;

                case "View":
                    // btn.IsEnabled = true;
                    btnModify.IsEnabled = true;
                    btnSave.IsEnabled = false;
                    btnClose.IsEnabled = true;
                    break;

                default:
                    break;
            }
        }

        #region Patient Search Buttons Click
        private void CmdPatientSearch_Click(object sender, RoutedEventArgs e)
        {
            IsViewClick = false;

            //ModuleName = "OPDModule";
            //Action = "OPDModule.Forms.PatientSearch";
            //UserControl rootPage = Application.Current.RootVisual as UserControl;

            //TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
            //mElement.Text = "Inventory Configuration";

            //WebClient c = new WebClient();
            //c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
            //c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));

            ////IsSearchClick = false;
            //ModuleName = "PalashDynamics.IPD";
            //Action = "PalashDynamics.IPD.IPDPatientSearch";
            //UserControl rootPage = Application.Current.RootVisual as UserControl;
            //WebClient c = new WebClient();
            //c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
            //c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));

            IPDPatientSearch frm = new IPDPatientSearch();
            frm.IsFromDischarge = true;
            if (IsViewClick == false)
            {
                frm.Closed += new EventHandler(cw_Closed);
            }
            frm.Show();

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


                if (myData is IInitiateCIMS)
                {
                    ((IInitiateCIMS)myData).Initiate(PatientTypes.IPD.ToString());
                }

                ChildWindow cw = new ChildWindow();
                cw = (ChildWindow)myData;
                if (IsViewClick == false)
                {
                    cw.Closed += new EventHandler(cw_Closed);
                }
                cw.Show();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void cw_Closed(object sender, EventArgs e)
        {
            if ((bool)((PalashDynamics.IPD.IPDPatientSearch)sender).DialogResult)     // if ((bool)((OPDModule.Forms.PatientSearch)sender).DialogResult)
            {
                GetSelectedPatientDetails();
            }
            else
            {
                txtMRNo.Text = "";
                Comman.SetDefaultHeader(_SelfMenuDetails);
            }
        }

        private void GetSelectedPatientDetails()
        {
            //Comment
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null)                            //SelectedPatientDetails
            {
                long PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;         //SelectedPatientDetails
                long UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;               //SelectedPatientDetails
                AdmID = ((IApplicationConfiguration)App.Current).SelectedPatient.IPDAdmissionID;             //SelectedPatientDetails.IPDAdmissionID
                AdmUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.AdmissionUnitID;     //SelectedPatientDetails.IPDAdmissionUnitID

                FindPatient(PatientID, UnitId, null);
                obj = new clsIPDAdmissionVO();
                obj.AdmID = ((IApplicationConfiguration)App.Current).SelectedPatient.IPDAdmissionID;         //SelectedPatientDetails.IPDAdmissionID
                obj.AdmUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.AdmissionUnitID; //SelectedPatientDetails.IPDAdmissionUnitID
            }
        }

        private void FindPatient(long PatientID, long PatientUnitId, string MRNO)
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();
            clsGetPatientBizActionVO BizAction = new clsGetPatientBizActionVO();
            BizAction.PatientDetails = new clsPatientVO();
            BizAction.PatientDetails.GeneralDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID; //PatientID;
            BizAction.PatientDetails.GeneralDetails.UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;       //PatientUnitId;
            BizAction.PatientDetails.GeneralDetails.MRNo = MRNO;
            BizAction.PatientDetails.GeneralDetails.IsIPD = true;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        if (!((clsGetPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientID.Equals(0))
                        {
                            //BindSelectedPatientDetails((clsGetPatientBizActionVO)arg.Result, Indicatior);
                            checkBedReleasd((clsGetPatientBizActionVO)arg.Result, Indicatior);
                        }
                        else
                        {
                            Indicatior.Close();
                            if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                            {
                                msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("MRChkValidation_Msg");
                            }
                            else
                            {
                                msgText = "Please check MR number.";
                            }


                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            txtMRNo.Focus();
                            msgW1.Show();
                            Comman.SetDefaultHeader(_SelfMenuDetails);
                        }
                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void checkBedReleasd(clsGetPatientBizActionVO PatientVO, WaitIndicator Indicatior)
        {
            //Comment
            clsGetBillAndBedByAdmIDAndAdmUnitIDBizActionVO BizAction = new clsGetBillAndBedByAdmIDAndAdmUnitIDBizActionVO();
            BizAction.DischargeList = new List<clsIPDDischargeVO>();
            BizAction.DischargeDetails = new clsIPDDischargeVO();
            BizAction.DischargeDetails.AdmID = ((IApplicationConfiguration)App.Current).SelectedPatient.IPDAdmissionID;          // PatientVO.PatientDetails.VisitAdmID;
            BizAction.DischargeDetails.AdmUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.AdmissionUnitID;     //PatientVO.PatientDetails.VisitAdmUnitID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    clsGetBillAndBedByAdmIDAndAdmUnitIDBizActionVO result = arg.Result as clsGetBillAndBedByAdmIDAndAdmUnitIDBizActionVO;
                    if (result != null)
                    {
                        string msgTitle = "Palash";
                        string msgText = string.Empty;
                        if (result.IsBedRelease.Equals(true))
                        {
                            if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                            {
                                msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("BedAlreadyRelease_Msg");
                            }
                            else
                            {
                                msgText = "Bed is already released.";
                            }


                            //msgText = "Bed is already released.";
                        }
                        else
                        {
                            BindSelectedPatientDetails(PatientVO, Indicatior);
                        }

                        if (!string.IsNullOrEmpty(msgText))
                        {
                            Comman.SetDefaultHeader(_SelfMenuDetails);
                            Indicatior.Close();
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW.Show();
                        }
                    }
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void BindSelectedPatientDetails(clsGetPatientBizActionVO PatientVO, WaitIndicator Indicatior)
        {
            txtMRNo.Text = PatientVO.PatientDetails.GeneralDetails.MRNo;
            //txtMRNoSearch.Text = PatientVO.PatientDetails.GeneralDetails.MRNo;

            patientDetails = new clsPatientGeneralVO();
            patientDetails.PatientID = PatientVO.PatientDetails.GeneralDetails.PatientID;
            patientDetails.PatientUnitID = PatientVO.PatientDetails.GeneralDetails.UnitId;

            GetIntakeOutputChartDetails(patientDetails.PatientID, patientDetails.PatientUnitID);

            DefaultValueForGrid();
            Comman.SetPatientDetailHeader(PatientVO.PatientDetails);
            Indicatior.Close();
            if (IsViewClick == true)
            {
                //Comment
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient = new clsIPDAdmissionVO();                //((IApplicationConfiguration)App.Current).SelectedPatientDetails = new clsPatientGeneralVO();
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientId = patientDetails.PatientID;       //SelectedPatientDetails
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId = patientDetails.PatientUnitID;      //SelectedPatientDetails

                ModuleName = "OPDModule";
                Action = "OPDModule.PatientAndVisitDetails";
                UserControl rootPage = Application.Current.RootVisual as UserControl;

                //TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                //mElement.Text = "Inventory Configuration";

                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
        }

        private void CmddischargePatientListSearch_Click(object sender, RoutedEventArgs e)
        {
            if (txtMRNo.Text.Length != 0)
            {
                IsViewClick = false;
                //GetPatientData();
                FindPatient(0, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID, txtMRNo.Text);
            }
            else
            {
                if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                {
                    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("MRNoValidation_Msg");
                }
                else
                {
                    msgText = "Please enter M.R. Number.";
                }

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                txtMRNo.Focus();
                msgW1.Show();
                Comman.SetDefaultHeader(_SelfMenuDetails);
            }

        }

        clsPatientGeneralVO patientDetails = null;
        public void GetPatientData()
        {

            clsGetPatientGeneralDetailsListBizActionVO BizActionObject = new clsGetPatientGeneralDetailsListBizActionVO();
            BizActionObject.PatientDetailsList = new List<clsPatientGeneralVO>();
            patientDetails = new clsPatientGeneralVO();
            BizActionObject.AdmissionWise = true;
            BizActionObject.MRNo = txtMRNo.Text.Trim();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null)
                {
                    if (ea.Result != null)
                    {
                        clsGetPatientGeneralDetailsListBizActionVO result = ea.Result as clsGetPatientGeneralDetailsListBizActionVO;
                        if (result.PatientDetailsList.Count > 0)
                        {
                            patientDetails.PatientID = result.PatientDetailsList[0].PatientID;
                            patientDetails.PatientUnitID = result.PatientDetailsList[0].UnitId;

                            clsPatientVO obj = new clsPatientVO() { FirstName = result.PatientDetailsList[0].FirstName, LastName = result.PatientDetailsList[0].LastName, MiddleName = result.PatientDetailsList[0].MiddleName, Gender = result.PatientDetailsList[0].Gender };
                            obj.GeneralDetails.MRNo = result.PatientDetailsList[0].MRNo;
                            Comman.SetPatientDetailHeader(obj);
                        }
                        else
                        {
                            if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                            {
                                msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("MRChkValidation_Msg");
                            }
                            else
                            {
                                msgText = "Please check MR number.";
                            }

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            txtMRNo.Focus();
                            msgW1.Show();
                            Comman.SetDefaultHeader(_SelfMenuDetails);
                        }
                    }
                }
            };
            client.ProcessAsync(BizActionObject, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        //bool IsViewClick;
        private void cmdPastDetails_Click(object sender, RoutedEventArgs e)
        {
            if (txtMRNo.Text.Length != 0)
            {
                IsViewClick = true;
                FindPatient(0, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID, txtMRNo.Text.Trim());
            }
            else
            {
                if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                {
                    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("MRNoValidation_Msg");
                }
                else
                {
                    msgText = "Please enter M.R. Number.";
                }

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                txtMRNo.Focus();
                msgW1.Show();
                Comman.SetDefaultHeader(_SelfMenuDetails);
            }
        }

        #endregion


        #region TextBox LostFocus

        private void txtOralNg_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (string.IsNullOrEmpty(txt.Text))
            {
                txt.Text = "0";
            }
            else
            {
                if (!((TextBox)e.OriginalSource).Text.IsItSpecialChar())
                {
                    ((TextBox)e.OriginalSource).Text = "0";
                    //((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Red);
                    //msgText = "Special characters are not allowed.";
                    //MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //msgWindow.Show();
                }
                else
                {
                    //((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Gray);
                }
            }
        }

        private void TextBlock_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (string.IsNullOrEmpty(txt.Text))
            {
                txt.Text = "0";
            }
            else
            {
                if (!((TextBox)e.OriginalSource).Text.IsItSpecialChar())
                {
                    ((TextBox)e.OriginalSource).Text = "0";
                    //((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Red);
                    //msgText = "Special characters are not allowed.";
                    //MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //msgWindow.Show();
                }
                else
                {
                    //((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Gray);
                }
            }
        }

        private void txtNg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.PlatformKeyCode == 109)
            {
                e.Handled = true;
            }
            else
                e.Handled = CIMS.Comman.HandleDecimal(sender, e);
        }

        #endregion

        private void dgInputOutputList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgInputOutputList.SelectedItem != null)
            {
                clsIPDIntakeOutputChartVO objIntake = (clsIPDIntakeOutputChartVO)dgInputOutputList.SelectedItem;
                dtpDate.SelectedDate = objIntake.Date;
                CommandButtonInputOutputChart("View");
                GetIntakeOutputChartDetailsByPatientID(objIntake);
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            clsIPDIntakeOutputChartVO objIntake = (clsIPDIntakeOutputChartVO)dgInputOutputList.SelectedItem;
            if (objIntake != null)
            {
                showreport(objIntake.ID, objIntake.UnitID);

            }
            else
            {
                string msgTitle = "Palash";
                if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                {
                    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("PatientValidation_Msg");
                }
                else
                {
                    msgText = "Please select patient.";
                }

                //string msgText = "Please Select Patient";
                MessageBoxChildWindow msgW = null;
                msgW = new MessageBoxChildWindow(msgTitle, msgText, MessageBoxButtons.Ok, MessageBoxIcon.Information);
                msgW.Show();
            }
        }

        public void showreport(long IntakeOutputID, long IntakeOutputIDUnitID)
        {
            //Comment
            string URL = "../Reports/IPD/BedTransfer.aspx?IntakeOutputID=" + IntakeOutputID + "&IntakeOutputIDUnitID=" + IntakeOutputIDUnitID + "&ReportID=" + 23; // Convert.ToInt64(Reports.IntakOutputReport);
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

        }

        private void txtMRNumber_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && txtMRNo.Text.Length != 0)
            {
                string mrno = txtMRNo.Text;
                FindPatient(0, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID, txtMRNo.Text.Trim());
            }
            else
            {

            }
        }


        #region Added by Ashutosh 21/11/2013
        private void chkIsFreeze_Click(object sender, RoutedEventArgs e)
        {
            bool Chkbox;
            Chkbox = (bool)((CheckBox)sender).IsChecked;
            if (Chkbox.Equals(true))
            {
               // msgText = "Are you sure you want to freeze ?";
                if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                {
                    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("Freeze_Msg");
                }
                else
                {
                    msgText = "Are you sure you want to freeze ?";
                }

                MessageBoxControl.MessageBoxChildWindow msgWindow =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWindow.OnMessageBoxClosed += new MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindow_OnMessageBoxClosed);
                msgWindow.Show();
            }

        }

        void msgWindow_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                UpdateIsFreezedStatus();
            }
        }


        private void UpdateIsFreezedStatus()
        {
            clsUpdateStatusIntakeOutputChartBizActionVO BizAction = new clsUpdateStatusIntakeOutputChartBizActionVO();
            BizAction.IsCalledForFreeze = true;
            try
            {
                if (dgInputOutputList.SelectedItem != null)
                {
                    BizAction.IntakeOutputDetails = new clsIPDIntakeOutputChartVO();
                    clsIPDIntakeOutputChartVO objIntakeOut = (clsIPDIntakeOutputChartVO)dgInputOutputList.SelectedItem;
                    BizAction.IntakeOutputDetails.ID = objIntakeOut.ID;
                    BizAction.IntakeOutputDetails.UnitID = objIntakeOut.UnitID;

                    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                    Client1.ProcessCompleted += (s, arg) =>
                    {

                        if (arg.Error == null && arg.Result != null)
                        {
                           // msgText = "Record freezed successfully.";
                            if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                            {
                                msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("RecordFreezeConfirm_Msg");
                            }
                            else
                            {
                                msgText = "Record freezed successfully.";
                            }
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                            CommandButtonInputOutputChart("Modify");
                            GetIntakeOutputChartDetails(patientDetails.PatientID, patientDetails.PatientUnitID);
                            btnModify.IsEnabled = false;
                            objint.IsEnable = false;
                            DefaultValueForGrid();
                        }
                        else
                        {
                           // msgText = "Error occured while processing.";
                            if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                            {
                                msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorWhileSaveModify_Msg");
                            }
                            else
                            {
                                msgText = "Error occured while processing.";
                            }
                            MessageBoxControl.MessageBoxChildWindow msgWindowNew = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindowNew.Show();
                        }
                    };
                    Client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client1.CloseAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        } 
        #endregion


    }

    public class defaulClass : INotifyPropertyChanged
    {
        private int _RowID;
        public int RowID
        {
            get { return _RowID; }
            set
            {
                if (_RowID != value)
                {
                    _RowID = value;
                    OnPropertyChanged("RowID");
                }
            }
        }

        private string _Time;

        public string Time
        {
            get { return _Time; }
            set { _Time = value; }
        }

        private string _ReadOnly;
        public string ReadOnly
        {
            get { return _ReadOnly; }
            set { _ReadOnly = value; }
        }

        private string _Style;
        public string Styles
        {
            get { return _Style; }
            set { _Style = value; }
        }

        private double _OralNg;

        public double OralNg
        {
            get { return _OralNg; }
            set
            {
                if (_OralNg != value)
                {
                    _OralNg = value;
                    OnPropertyChanged("OralNg");
                }
            }
        }
        private double _TotalParental;

        public double TotalParental
        {
            get { return _TotalParental; }
            set
            {
                if (_TotalParental != value)
                {
                    _TotalParental = value;
                    OnPropertyChanged("TotalParental");
                }

            }
        }


        private double _IntakeOthers;

        public double IntakeOthers
        {
            get { return _IntakeOthers; }
            set
            {
                if (_IntakeOthers != value)
                {
                    _IntakeOthers = value;
                    OnPropertyChanged("IntakeOthers");
                }
            }
        }

        private double _IntakeTotal;
        public double IntakeTotal
        {
            get { return _IntakeTotal; }
            set
            {
                if (_IntakeTotal != value)
                {
                    _IntakeTotal = value;
                    OnPropertyChanged("IntakeTotal");
                }
            }
        }

        private double _Urine;

        public double Urine
        {
            get { return _Urine; }
            set
            {
                if (_Urine != value)
                {
                    _Urine = value;
                    OnPropertyChanged("Urine");
                }
            }
        }

        private double _Ng;

        public double Ng
        {
            get { return _Ng; }
            set
            {
                if (_Ng != value)
                {
                    _Ng = value;
                    OnPropertyChanged("Ng");
                }
            }
        }

        private double _Drain;

        public double Drain
        {
            get { return _Drain; }
            set
            {
                if (_Drain != value)
                {
                    _Drain = value;
                    OnPropertyChanged("Drain");
                }
            }
        }


        private double _OutputOthers;

        public double OutputOthers
        {
            get { return _OutputOthers; }
            set
            {
                if (_OutputOthers != value)
                {
                    _OutputOthers = value;
                    OnPropertyChanged("OutputOthers");
                }
            }
        }

        private double _OutputTotal;

        public double OutputTotal
        {
            get { return _OutputTotal; }
            set
            {
                if (_OutputTotal != value)
                {
                    _OutputTotal = value;
                    OnPropertyChanged("OutputTotal");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
