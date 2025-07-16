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
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using CIMS;
using System.Windows.Media.Imaging;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.DashBoardVO;
using System.IO;
using System.Reflection;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmFertCheck : ChildWindow
    {
        public DateTime date;
        public long OocyteNo;
        public long SerialOocNo;
        public clsCoupleVO CoupleDetails;
        public long PlanTherapyId;
        public long PlanTherapyUnitId;
        public DateTime? ProcedureDate;
        public DateTime? ProcedureTime;
        double hours = 0;
        public bool IsClosed;

        public event RoutedEventHandler OnSaveButton_Click;

        public frmFertCheck()
        {
            InitializeComponent();
        }

        private void FillFertilizationResult()
        {
            List<MasterListItem> ObjList = new List<MasterListItem>();
            ObjList.Add(new MasterListItem(0, "- Select -"));
            ObjList.Add(new MasterListItem(1, "Normal"));
            ObjList.Add(new MasterListItem(2, "Abnormal"));
            cmbFertilizationCheckResult.ItemsSource = null;
            cmbFertilizationCheckResult.ItemsSource = ObjList;
            cmbFertilizationCheckResult.SelectedItem = ObjList[0];
        }

        private void CheckTimeAndReturnLabDay()
        {
            DateTime ProcudureDateTime = ((DateTime)ProcedureDate).Date.Add(((DateTime)ProcedureTime).TimeOfDay);
            DateTime CurrentObervationDateTime = ((DateTime)dtObservationDate.SelectedDate).Date.Add(((DateTime)dtObservationTime.Value).TimeOfDay);

            TimeSpan diff = CurrentObervationDateTime - ProcudureDateTime;
            hours = diff.TotalHours;

            if (hours < 16)
                blError = true;
            else if (hours >= 16 && hours <= 18)
                blSaveData = true;
            else if (hours > 18)
                blAlert = true;

            if (CurrentObervationDateTime < ProcudureDateTime)
                blLessDate = true;
        }

        private bool Validate()
        {
            bool result = true;

            if (dtObservationDate.SelectedDate == null)
            {
                dtObservationDate.SetValidation("Please Select Date");
                dtObservationDate.RaiseValidationError();
                dtObservationDate.Focus();
                result = false;
            }
            else
                dtObservationDate.ClearValidationError();

            if (dtObservationTime.Value == null)
            {
                dtObservationTime.SetValidation("Please Select Time");
                dtObservationTime.RaiseValidationError();
                dtObservationTime.Focus();
                result = false;
            }
            else
                dtObservationTime.ClearValidationError();

            if (cmbFertilizationCheck.ItemsSource != null)
            {
                if (cmbFertilizationCheck.SelectedItem == null)
                {
                    cmbFertilizationCheck.TextBox.SetValidation("Please select Fertilization");
                    cmbFertilizationCheck.TextBox.RaiseValidationError();
                    cmbFertilizationCheck.Focus();
                    result = false;
                }
                else if (((MasterListItem)cmbFertilizationCheck.SelectedItem).ID == 0)
                {
                    cmbFertilizationCheck.TextBox.SetValidation("Please select Fertilization");
                    cmbFertilizationCheck.TextBox.RaiseValidationError();
                    cmbFertilizationCheck.Focus();
                    result = false;
                }
                else
                    cmbFertilizationCheck.TextBox.ClearValidationError();
            }
            return result;
        }

        bool blError = false;
        bool blSaveData = false;
        bool blAlert = false;
        bool blLessDate = false;
        public long addSerialOocNo;
        List<MasterListItem> OocyteList = new List<MasterListItem>();
        List<MasterListItem> SelectedOocyteList = new List<MasterListItem>();

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                CheckTimeAndReturnLabDay();
                if (blLessDate)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                               new MessageBoxControl.MessageBoxChildWindow("", "Observation date can not be less than Procedure date ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW.Show();
                    blLessDate = false;
                }
                else if (blError)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                               new MessageBoxControl.MessageBoxChildWindow("", "Fertilization(16) hours are not complited yet.. Still you want to continue", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    msgW.Show();
                    blError = false;
                }
                else if (blAlert)
                {
                    //MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //          new MessageBoxControl.MessageBoxChildWindow("", "Fertilization check limit is crossed.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //msgW1.Show();
                    //msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                    //{
                    //    if (res == MessageBoxResult.OK)
                    //    {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Fertilization(18) hours are crossed.. Do you want to continue", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    msgW.Show();
                    //    }
                    //};
                    blAlert = false;
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save Fertilization details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    msgW.Show();
                }


                //-----added by neena dated 18/5/16

                OocyteList = (List<MasterListItem>)cmbApplay.ItemsSource;
                MasterListItem newItem1 = new MasterListItem();

                foreach (var item in OocyteList)
                {
                    if (item.Status == true)
                    {
                        long id1 = 0;
                        string str = "";
                        id1 = item.ID;
                        str = item.Description;
                        addSerialOocNo = item.ID - OocyteNo;
                        if (addSerialOocNo == 0 ) //|| addSerialOocNo < 0)
                        {
                            addSerialOocNo = 1;
                        }

                        newItem1 = new MasterListItem(id1, str, addSerialOocNo);
                        SelectedOocyteList.Add(newItem1);
                    }
                }
                //-------------
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Save();
        }

        private void Save()
        {
            clsIVFDashboard_AddUpdateFertCheckBizActionVO BizAction = new clsIVFDashboard_AddUpdateFertCheckBizActionVO();

            BizAction.FertCheckDetails = new clsIVFDashboard_FertCheck();
            //BizAction.FertCheckDetails.ID = ((clsIVFDashboard_LabDaysVO)this.DataContext).ID;
            BizAction.FertCheckDetails.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.FertCheckDetails.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.FertCheckDetails.PlanTherapyID = PlanTherapyId;
            BizAction.FertCheckDetails.PlanTherapyUnitID = PlanTherapyUnitId;

            //added by neena dated 18/5/16
            addSerialOocNo = 0;
            BizAction.FertCheckDetails.OcyteListList = SelectedOocyteList;
            BizAction.FertCheckDetails.OocyteNumber = OocyteNo;
            BizAction.FertCheckDetails.SerialOocyteNumber = SerialOocNo;

            DateTime? ProcedureDate = null;
            if (dtObservationDate.SelectedDate != null)
                ProcedureDate = dtObservationDate.SelectedDate.Value.Date;
            if (dtObservationTime.Value != null)
                ProcedureDate = ProcedureDate.Value.Add(dtObservationTime.Value.Value.TimeOfDay);

            BizAction.FertCheckDetails.Date = ProcedureDate;
            BizAction.FertCheckDetails.Time = ProcedureDate;

            //BizAction.FertCheckDetails.Date = dtObservationDate.SelectedDate.Value.Date;
            //BizAction.FertCheckDetails.Time = Convert.ToDateTime(dtObservationTime.Value);

            if (chkFreeze.IsChecked == true)
                BizAction.FertCheckDetails.Isfreezed = true;
            else
                BizAction.FertCheckDetails.Isfreezed = false;

            if ((MasterListItem)cmbFertilizationCheck.SelectedItem != null)
                BizAction.FertCheckDetails.FertCheck = ((MasterListItem)cmbFertilizationCheck.SelectedItem).ID;

            if ((MasterListItem)cmbFertilizationCheckResult.SelectedItem != null)
                BizAction.FertCheckDetails.FertCheckResult = ((MasterListItem)cmbFertilizationCheckResult.SelectedItem).ID;

            if (txtRemarks.Text != "")
                BizAction.FertCheckDetails.Remarks = txtRemarks.Text;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.OK)
                        {
                            this.DialogResult = true;
                            if (OnSaveButton_Click != null)
                                OnSaveButton_Click(this, new RoutedEventArgs());
                        }
                    };
                    msgW1.Show();
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

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //dtObservationDate.SelectedDate = DateTime.Now;
            //dtObservationTime.Value = DateTime.Now;

            DateTime ProcudureDateTime = ((DateTime)ProcedureDate).Date.Add(((DateTime)ProcedureTime).TimeOfDay);
            DateTime DefaultDate = ProcudureDateTime.AddHours(16);
            dtObservationDate.SelectedDate = DefaultDate;
            dtObservationTime.Value = DefaultDate;
            GetFertCheckDate();
            fillOocyteDetails();
            fillFertCheck();
            fillFertDetails();

            if (IsClosed)
                cmdNew.IsEnabled = false;
        }

        //added by neena

        private void GetFertCheckDate()
        {
            clsIVFDashboard_GetFertCheckBizActionVO BizAction = new clsIVFDashboard_GetFertCheckBizActionVO();
            BizAction.IsGetDate = true;
            BizAction.FertCheckDetails = new clsIVFDashboard_FertCheck();
            BizAction.FertCheckDetails.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.FertCheckDetails.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.FertCheckDetails.PlanTherapyID = PlanTherapyId;
            BizAction.FertCheckDetails.PlanTherapyUnitID = PlanTherapyUnitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetFertCheckBizActionVO)arg.Result).FertCheckDetails != null)
                    {
                        if (((clsIVFDashboard_GetFertCheckBizActionVO)arg.Result).FertCheckDetails.Date != null)
                        {
                            dtObservationDate.SelectedDate = ((clsIVFDashboard_GetFertCheckBizActionVO)arg.Result).FertCheckDetails.Date;
                        }
                        if (((clsIVFDashboard_GetFertCheckBizActionVO)arg.Result).FertCheckDetails.Date != null)
                        {
                            dtObservationTime.Value = ((clsIVFDashboard_GetFertCheckBizActionVO)arg.Result).FertCheckDetails.Date;
                        } 
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

        List<MasterListItem> objList = new List<MasterListItem>();
        private void fillOocyteDetails()
        {
            try
            {

                clsIVFDashboard_GetOPUDetailsBizActionVO BizAction = new clsIVFDashboard_GetOPUDetailsBizActionVO();
                BizAction.Details = new clsIVFDashboard_OPUVO();
                BizAction.Details.PlanTherapyID = PlanTherapyId;
                BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitId;
                if (CoupleDetails != null)
                {
                    BizAction.Details.PatientID = CoupleDetails.FemalePatient.PatientID;
                    BizAction.Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                }

                //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
                //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details != null)
                        {

                            if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.ID > 0)
                            {

                                //objList.Add(new MasterListItem(0, "-- Select --"));
                                //objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                                for (int i = 1; i <= ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.OocyteRetrived; i++)
                                {
                                    if (i != OocyteNo && OocyteNo != 0)
                                    {

                                        objList.Add(new MasterListItem(i, "Oocyte No " + i));
                                    }
                                }
                                filldata();
                                //added by neena 

                            }



                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        private void filldata()
        {
            clsIVFDashboard_GetFertCheckBizActionVO BizAction = new clsIVFDashboard_GetFertCheckBizActionVO();
            BizAction.FertCheckDetails = new clsIVFDashboard_FertCheck();
            BizAction.IsApply = true;
            BizAction.FertCheckDetails.PlanTherapyID = PlanTherapyId;
            BizAction.FertCheckDetails.PlanTherapyUnitID = PlanTherapyUnitId;
            BizAction.FertCheckDetails.SerialOocyteNumber = OocyteNo;

           
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetFertCheckBizActionVO)arg.Result).Oocytelist != null)
                    {
                        //  by neena
                        List<clsIVFDashboard_FertCheck> mlist = ((clsIVFDashboard_GetFertCheckBizActionVO)arg.Result).Oocytelist;
                        List<MasterListItem> missingVehicle1 = new List<MasterListItem>();
                        var productFirstChars =
                                               from p in objList
                                               select p.ID;
                        var customerFirstChars =
                                                from c in mlist
                                                where (c.Isfreezed == true)
                                                select c.OocyteNumber
                                                ;

                        //by neena
                        var missingVehicles = productFirstChars.Intersect(customerFirstChars);
                        MasterListItem newItem = new MasterListItem();
                        foreach (var item in missingVehicles)
                        {
                            string str = "";
                            str = "Oocyte Number " + item;
                            long id = item;
                            newItem = new MasterListItem(item, str);

                            missingVehicle1.Add(newItem);
                        }

                        cmbApplay.ItemsSource = null;
                        cmbApplay.ItemsSource = missingVehicle1;
                        //cmbApplay.SelectedItem = missingVehicles[0];  


                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }


        private void fillFertDetails()
        {
            clsIVFDashboard_GetFertCheckBizActionVO BizAction = new clsIVFDashboard_GetFertCheckBizActionVO();
            BizAction.FertCheckDetails = new clsIVFDashboard_FertCheck();
            BizAction.FertCheckDetails.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.FertCheckDetails.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.FertCheckDetails.PlanTherapyID = PlanTherapyId;
            BizAction.FertCheckDetails.PlanTherapyUnitID = PlanTherapyUnitId;

            BizAction.FertCheckDetails.OocyteNumber = OocyteNo;
            BizAction.FertCheckDetails.SerialOocyteNumber = SerialOocNo;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetFertCheckBizActionVO)arg.Result).FertCheckDetails != null)
                    {
                        if (((clsIVFDashboard_GetFertCheckBizActionVO)arg.Result).FertCheckDetails.Date != null)
                        {
                            dtObservationDate.SelectedDate = ((clsIVFDashboard_GetFertCheckBizActionVO)arg.Result).FertCheckDetails.Date;
                        }
                        //else
                        //{
                        //    dtObservationDate.SelectedDate = date;
                        //}
                        if (((clsIVFDashboard_GetFertCheckBizActionVO)arg.Result).FertCheckDetails.Time != null)
                        {
                            dtObservationTime.Value = ((clsIVFDashboard_GetFertCheckBizActionVO)arg.Result).FertCheckDetails.Time;
                        }
                        //else
                        //{
                        //    dtObservationTime.Value = DateTime.Now;
                        //}

                        if (((clsIVFDashboard_GetFertCheckBizActionVO)arg.Result).FertCheckDetails.FertCheck != 0)
                        {
                            cmbFertilizationCheck.SelectedValue = ((clsIVFDashboard_GetFertCheckBizActionVO)arg.Result).FertCheckDetails.FertCheck;
                        }

                        if (((clsIVFDashboard_GetFertCheckBizActionVO)arg.Result).FertCheckDetails.FertCheckResult != 0)
                        {
                            cmbFertilizationCheckResult.SelectedValue = ((clsIVFDashboard_GetFertCheckBizActionVO)arg.Result).FertCheckDetails.FertCheckResult;
                        }

                        if (((clsIVFDashboard_GetFertCheckBizActionVO)arg.Result).FertCheckDetails.Remarks != null)
                        {
                            txtRemarks.Text = ((clsIVFDashboard_GetFertCheckBizActionVO)arg.Result).FertCheckDetails.Remarks;
                        }

                        if (((clsIVFDashboard_GetFertCheckBizActionVO)arg.Result).FertCheckDetails.Isfreezed == true)
                        {
                            cmdNew.IsEnabled = false;
                            chkFreeze.IsChecked = true;
                        }

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

        private void fillFertCheck()
        {
            List<MasterListItem> mlSourceOfSperm = new List<MasterListItem>();
            MasterListItem Default = new MasterListItem(0, "- Select -");
            mlSourceOfSperm.Insert(0, Default);
            EnumToList(typeof(FertCheck), mlSourceOfSperm);
            cmbFertilizationCheck.ItemsSource = mlSourceOfSperm;
            cmbFertilizationCheck.SelectedItem = Default;
            // cmbPayMode.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PaymentModeID;
            FillFertilizationResult();
        }

        public static void EnumToList(Type EnumType, List<MasterListItem> TheMasterList)
        {
            Array Values = GetValues(EnumType);
            foreach (int Value in Values)
            {

                string Display = Enum.GetName(EnumType, Value);
                MasterListItem Item = new MasterListItem(Value, Display);
                TheMasterList.Add(Item);
            }
        }

        public static object[] GetValues(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            List<object> values = new List<object>();

            var fields = from field in enumType.GetFields()
                         where field.IsLiteral
                         select field;

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(enumType);
                values.Add(value);
            }

            return values.ToArray();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void cmbFertilizationCheck_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)cmbFertilizationCheck.SelectedItem).ID == 1)
            {
                cmbFertilizationCheckResult.Visibility = Visibility.Visible;
            }
            else
                cmbFertilizationCheckResult.Visibility = Visibility.Collapsed;
        }
    }
}

