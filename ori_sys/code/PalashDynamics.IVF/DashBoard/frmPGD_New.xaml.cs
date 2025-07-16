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
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference; //PalashServiceReferance;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;//DataTemplateServiceRef;
using System.Windows.Browser;
using System.IO;
using PalashDynamics.ValueObjects.DashBoardVO;
using CIMS;
using System.Collections.ObjectModel;
using MessageBoxControl;
using PalashDynamics.Controls;
using PalashDynamics.ValueObjects.Master.DoctorMaster;
using System.Windows.Media.Imaging;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmPGD_New : ChildWindow
    {
        public long Day = 0;
        public long LabDayID;
        public long LabDayUnitID;
        public long LabDayNo;
        public long PatientID;
        public long PatientUnitID;
        public long PlanTherapyID;
        public long PlanTherapyUnitID;
        public long OocyteNumber;
        public long SerialOocyteNumber;
        public DateTime LabDateDay3;
        public DateTime LabTimeDay3;
        public DateTime SampleRecTime;
        public long time;
        public bool IsFreezed = false;
        public bool FrozenPGDPGS;

        private ObservableCollection<clsPGDFISHVO> _FISH = new ObservableCollection<clsPGDFISHVO>();
        public ObservableCollection<clsPGDFISHVO> FISH
        {
            get { return _FISH; }
            set { _FISH = value; }
        }
        private ObservableCollection<clsPGDFISHVO> _RemoveFISH = new ObservableCollection<clsPGDFISHVO>();
        public ObservableCollection<clsPGDFISHVO> RemoveFISH
        {
            get { return _RemoveFISH; }
            set { _RemoveFISH = value; }
        }
        private ObservableCollection<clsPGDKaryotypingVO> _Karyotyping = new ObservableCollection<clsPGDKaryotypingVO>();
        public ObservableCollection<clsPGDKaryotypingVO> Karyotyping
        {
            get { return _Karyotyping; }
            set { _Karyotyping = value; }
        }
        private ObservableCollection<clsPGDKaryotypingVO> _RemoveKaryotyping = new ObservableCollection<clsPGDKaryotypingVO>();
        public ObservableCollection<clsPGDKaryotypingVO> RemoveKaryotyping
        {
            get { return _RemoveKaryotyping; }
            set { _RemoveKaryotyping = value; }
        }

        public frmPGD_New()
        {
            InitializeComponent();
            this.DataContext = null;
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();
            }

        }


        private bool Validate()
        {
            bool result = true;
            SampleRecTime = Convert.ToDateTime(SampleReceiveTime.Value);

            if (txtChromosomalDisease.Text == null)
            {
                txtChromosomalDisease.SetValidation("Please Enter Chromosomal Disease");
                txtChromosomalDisease.RaiseValidationError();
                txtChromosomalDisease.Focus();
                result = false;
            }
            else
                txtChromosomalDisease.ClearValidationError();

            //Dispatch Date
            if (SampleReceiveDate.SelectedDate == null)
            {
                SampleReceiveDate.SetValidation("Please Select Date");
                SampleReceiveDate.RaiseValidationError();
                SampleReceiveDate.Focus();
                return false;
            }
            else if (SampleReceiveTime.Value == null)
            {
                SampleReceiveTime.SetValidation("Please Select Date");
                SampleReceiveTime.RaiseValidationError();
                SampleReceiveTime.Focus();
                return false;
            }
            else
            {
                SampleReceiveDate.ClearValidationError();
                SampleReceiveTime.ClearValidationError();
            }
            //END

            //Sample date with condition

            if (SampleReceiveDate.SelectedDate != null && LabDateDay3 != null)
            {
                if (LabDateDay3.Date > SampleReceiveDate.SelectedDate.Value.Date)
                {

                    SampleReceiveDate.SetValidation("Dispatch Date Should be Greater Than Lab Day 3");
                    SampleReceiveDate.RaiseValidationError();
                    SampleReceiveDate.Focus();
                    return false;
                }


                //else if (LabDateDay3.Date == SampleReceiveDate.SelectedDate.Value.Date)
                //{               



                //    SampleReceiveTime.SetValidation("Dispatch Time Should be Greater Than Lab Day 3 Time");
                //    SampleReceiveTime.RaiseValidationError();
                //    SampleReceiveTime.Focus();
                //    return false;
                //}

                else
                    SampleReceiveDate.ClearValidationError();
            }
            ////END/////

            if (cmbPhysician.SelectedItem == null)
            {
                cmbPhysician.TextBox.SetValidation("Please select Physician");
                cmbPhysician.TextBox.RaiseValidationError();
                cmbPhysician.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbPhysician.SelectedItem).ID == 0)
            {
                cmbPhysician.TextBox.SetValidation("Please select Physician");
                cmbPhysician.TextBox.RaiseValidationError();
                cmbPhysician.Focus();
                result = false;
            }
            else
                cmbPhysician.TextBox.ClearValidationError();

            ////Biopsy Date//////
            if (PGDDate.SelectedDate == null)
            {
                PGDDate.SetValidation("Please Select Date");
                PGDDate.RaiseValidationError();
                PGDDate.Focus();
                return false;
            }
            else if (PGDTime.Value == null)
            {
                PGDTime.SetValidation("Please Select Date");
                PGDTime.RaiseValidationError();
                PGDTime.Focus();
                return false;
            }

            else
            {
                PGDDate.ClearValidationError();
                PGDTime.ClearValidationError();
            }
            ///END
            ///
            //Biopsy date condition
            if (SampleReceiveDate.SelectedDate != null && PGDDate.SelectedDate != null)
            {
                if (SampleReceiveDate.SelectedDate.Value.Date > PGDDate.SelectedDate.Value.Date)
                {
                    PGDDate.SetValidation("Date of Biopsy should be Greater than Dispatch Date");
                    PGDDate.RaiseValidationError();
                    PGDDate.Focus();
                    return false;
                }
                else
                    PGDDate.ClearValidationError();
            }
            /////END
            if (cmbBiopsy.SelectedItem == null)
            {
                cmbBiopsy.TextBox.SetValidation("Please select Biopsy");
                cmbBiopsy.TextBox.RaiseValidationError();
                cmbBiopsy.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbBiopsy.SelectedItem).ID == 0)
            {
                cmbBiopsy.TextBox.SetValidation("Please select Biopsy");
                cmbBiopsy.TextBox.RaiseValidationError();
                cmbBiopsy.Focus();
                result = false;
            }
            else
                cmbBiopsy.TextBox.ClearValidationError();

            if (ResultDate.SelectedDate == null)
            {
                ResultDate.SetValidation("Please Select Date");
                ResultDate.RaiseValidationError();
                ResultDate.Focus();
                return false;
            }
            else if (TimeOfResult.Value == null)
            {
                TimeOfResult.SetValidation("Please Select Date");
                TimeOfResult.RaiseValidationError();
                TimeOfResult.Focus();
                return false;
            }
            else
            {
                ResultDate.ClearValidationError();
                TimeOfResult.ClearValidationError();
            }

            if (ResultDate.SelectedDate != null && PGDDate.SelectedDate != null)
            {
                if (ResultDate.SelectedDate.Value.Date < PGDDate.SelectedDate.Value.Date)
                {
                    ResultDate.SetValidation("Date of Result should be Greater than Biopsy Date");
                    ResultDate.RaiseValidationError();
                    ResultDate.Focus();
                    return false;
                }

                else if (ResultDate.SelectedDate.Value.Date == PGDDate.SelectedDate.Value.Date)
                {
                    if (PGDTime.Value == TimeOfResult.Value)
                    {

                        TimeOfResult.SetValidation("Result Time Should be Greater Than Dispatch Time");
                        TimeOfResult.RaiseValidationError();
                        TimeOfResult.Focus();
                        return false;
                    }

                }

                else
                    PGDDate.ClearValidationError();




            }
            if (txtReasonofreferral.Text == null)
            {
                txtReasonofreferral.SetValidation("Please Enter Reason of referral");
                txtReasonofreferral.RaiseValidationError();
                txtReasonofreferral.Focus();
                result = false;
            }
            else
                txtReasonofreferral.ClearValidationError();

            return result;
        }
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsAddUpdatePGDHistoryBizActionVO BizActionVO = new clsAddUpdatePGDHistoryBizActionVO();
                BizActionVO.PGDHistoryDetails = new clsPGDHistoryVO();
                BizActionVO.PGDHistoryDetails.ID = HistoryID;
                BizActionVO.PGDHistoryDetails.PatientID = PatientID;
                BizActionVO.PGDHistoryDetails.PatientUnitID = PatientUnitID;
                BizActionVO.PGDHistoryDetails.PlanTherapyID = PlanTherapyID;
                BizActionVO.PGDHistoryDetails.PlanTherapyUnitID = PlanTherapyUnitID;
                BizActionVO.PGDHistoryDetails.ChromosomalDisease = txtChromosomalDisease.Text;
                BizActionVO.PGDHistoryDetails.XLinkedDominant = txtXDominantDisorder.Text;
                BizActionVO.PGDHistoryDetails.XLinkedRecessive = txtXRecessiveDisorder.Text;
                BizActionVO.PGDHistoryDetails.AutosomalRecessive = txtAutosomalRecessiveDisorder.Text;
                BizActionVO.PGDHistoryDetails.AutosomalDominant = txtAutosomalDominantDisorder.Text;
                BizActionVO.PGDHistoryDetails.Ylinked = txtYLinkedDisorder.Text;

                //added by neena
                if (cmbFamilyHistory.SelectedItem != null)
                    BizActionVO.PGDHistoryDetails.FamilyHistory = ((MasterListItem)cmbFamilyHistory.SelectedItem).ID;

                if (cmbAffectedPartner.SelectedItem != null)
                    BizActionVO.PGDHistoryDetails.AffectedPartner = ((MasterListItem)cmbAffectedPartner.SelectedItem).ID;
                //

                //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
                //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        clsAddUpdatePGDGeneralDetailsBizActionVO BizActionObj = new clsAddUpdatePGDGeneralDetailsBizActionVO();
                        BizActionObj.PGDGeneralDetails = new clsPGDGeneralDetailsVO();
                        BizActionObj.PGDFISHList = new List<clsPGDFISHVO>();
                        BizActionObj.PGDKaryotypingList = new List<clsPGDKaryotypingVO>();
                        BizActionObj.PGDGeneralDetails.ID = ((clsPGDGeneralDetailsVO)this.DataContext).ID;
                        BizActionObj.PGDGeneralDetails.SerialEmbNumber = SerialOocyteNumber;
                        BizActionObj.PGDGeneralDetails.OocyteNumber = OocyteNumber;
                        BizActionObj.PGDGeneralDetails.LabDayNo = LabDayNo;
                        BizActionObj.PGDGeneralDetails.LabDayID = LabDayID;
                        BizActionObj.PGDGeneralDetails.LabDayUnitID = LabDayUnitID;
                        BizActionObj.PGDGeneralDetails.PlanTherapyID = PlanTherapyID;
                        BizActionObj.PGDGeneralDetails.PlanTherapyUnitID = PlanTherapyUnitID;
                        BizActionObj.PGDGeneralDetails.FrozenPGDPGS = FrozenPGDPGS;

                        BizActionObj.PGDGeneralDetails.Date = PGDDate.SelectedDate.Value.Date;
                        if (txtFN.Text.Trim() != null)
                        {
                            BizActionObj.PGDGeneralDetails.SourceURL = "LabDayNo" + LabDayNo + "LabDayID" + LabDayID + "LabDayUnitID" + LabDayUnitID + "OocyteNumber" + OocyteNumber + "SerialOocyteNumber" + SerialOocyteNumber + txtFN.Text.Trim();
                            BizActionObj.PGDGeneralDetails.FileName = txtFN.Text.Trim();
                            BizActionObj.PGDGeneralDetails.Report = AttachedFileContents;
                        }

                        #region For ART Flow - PGD

                        BizActionObj.PGDGeneralDetails.SampleReceiveDate = SampleReceiveDate.SelectedDate.Value.Date;
                        BizActionObj.PGDGeneralDetails.SampleReceiveDate = BizActionObj.PGDGeneralDetails.SampleReceiveDate.Value.Add(SampleReceiveTime.Value.Value.TimeOfDay);

                        BizActionObj.PGDGeneralDetails.ResultDate = ResultDate.SelectedDate.Value.Date;
                        BizActionObj.PGDGeneralDetails.ResultDate = BizActionObj.PGDGeneralDetails.ResultDate.Value.Add(TimeOfResult.Value.Value.TimeOfDay);

                        if (cmbReferralDoctor.SelectedItem != null)
                            BizActionObj.PGDGeneralDetails.ReferringID = ((MasterListItem)cmbReferralDoctor.SelectedItem).ID;

                        if (cmbResult.SelectedItem != null)
                            BizActionObj.PGDGeneralDetails.ResultID = ((MasterListItem)cmbResult.SelectedItem).ID;

                        if (cmbSupervisor.SelectedItem != null)
                            BizActionObj.PGDGeneralDetails.SupervisedById = ((MasterListItem)cmbSupervisor.SelectedItem).ID;

                        if (cmbTestordered.SelectedItem != null)
                            BizActionObj.PGDGeneralDetails.TestOrderedID = ((MasterListItem)cmbTestordered.SelectedItem).ID;

                        BizActionObj.PGDGeneralDetails.MainFISHInterpretation = txtMainFSHInterpretation.Text;

                        //if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.PGDIndicationID != null && ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.PGDIndicationID != 0)
                        //{
                        //    cmbPGDIndication.SelectedValue = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.PGDIndicationID;
                        //}

                        if (cmbPGDIndication.SelectedItem != null)
                            BizActionObj.PGDGeneralDetails.PGDIndicationID = ((MasterListItem)cmbPGDIndication.SelectedItem).ID;    // For IVF ADM Changes

                        //if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.PGDIndicationDetails != null)
                        //{
                        //    txtIndicationDetails.Text = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.PGDIndicationDetails;
                        //}

                        if (!string.IsNullOrEmpty(txtIndicationDetails.Text))
                            BizActionObj.PGDGeneralDetails.PGDIndicationDetails = txtIndicationDetails.Text;    // For IVF ADM Changes

                        #endregion

                        BizActionObj.PGDGeneralDetails.MainFISHRemark = txtMainFSHRemark.Text;
                        BizActionObj.PGDGeneralDetails.MainKaryotypingRemark = txtMainKaryotypingRemark.Text;

                        BizActionObj.PGDGeneralDetails.Date = BizActionObj.PGDGeneralDetails.Date.Value.Add(PGDTime.Value.Value.TimeOfDay);

                        if (cmbPhysician.SelectedItem != null)
                            BizActionObj.PGDGeneralDetails.Physician = ((MasterListItem)cmbPhysician.SelectedItem).ID;

                        if (cmbBiopsy.SelectedItem != null)
                            BizActionObj.PGDGeneralDetails.BiospyID = ((MasterListItem)cmbBiopsy.SelectedItem).ID;

                        if (cmbSpecimanUsed.SelectedItem != null)
                            BizActionObj.PGDGeneralDetails.SpecimanUsedID = ((MasterListItem)cmbSpecimanUsed.SelectedItem).ID;                        

                        //BizActionObj.PGDGeneralDetails.ReferringFacility = txtReferringFacitity.Text;

                        if (cmbReferringFacitity.SelectedItem != null)
                            BizActionObj.PGDGeneralDetails.ReferringFacilityID = ((MasterListItem)cmbReferringFacitity.SelectedItem).ID;        // For IVF ADM Changes

                        BizActionObj.PGDGeneralDetails.PGDResult = txtPGDResult.Text.Trim();        // For IVF ADM Changes

                        if (rdbPGD.IsChecked == true)
                            BizActionObj.PGDGeneralDetails.PGDPGSProcedureID = 1;                   // For IVF ADM Changes
                        else if (rdbPGS.IsChecked == true)
                            BizActionObj.PGDGeneralDetails.PGDPGSProcedureID = 2;                   // For IVF ADM Changes

                        BizActionObj.PGDGeneralDetails.ResonOfReferal = txtReasonofreferral.Text;

                        for (int i = 0; i < FISH.Count; i++)
                        {
                            FISH[i].ChromosomeStudiedID = FISH[i].SelectedChromosomeStudiedId.ID;
                            FISH[i].TestOrderedID = FISH[i].SelectedTestOrderedId.ID;
                        }
                        BizActionObj.PGDFISHList = (List<clsPGDFISHVO>)FISH.ToList();

                        for (int i = 0; i < RemoveFISH.Count; i++)
                        {
                            BizActionObj.PGDFISHList.Add(RemoveFISH[i]);
                        }

                        for (int i = 0; i < Karyotyping.Count; i++)
                        {
                            Karyotyping[i].ChromosomeStudiedID = Karyotyping[i].SelectedChromosomeStudiedId.ID;
                            if ((MasterListItem)cmbBindingTechnique.SelectedItem != null)
                                Karyotyping[i].BindingTechnique = ((MasterListItem)cmbBindingTechnique.SelectedItem).ID;
                            if ((MasterListItem)cmbCultureType.SelectedItem != null)
                                Karyotyping[i].CultureTypeID = ((MasterListItem)cmbCultureType.SelectedItem).ID;
                        }

                        BizActionObj.PGDKaryotypingList = (List<clsPGDKaryotypingVO>)Karyotyping.ToList();
                        for (int i = 0; i < RemoveKaryotyping.Count; i++)
                        {
                            BizActionObj.PGDKaryotypingList.Add(RemoveKaryotyping[i]);
                        }

                        //for image 
                        foreach (var item in mylistitem)
                        {
                            ListItems obj = (ListItems)item;
                            clsAddImageVO ObjImg = new clsAddImageVO();
                            ObjImg.PatientID = PatientID;
                            ObjImg.PatientUnitID = PatientUnitID;
                            ObjImg.ID = obj.ID;
                            ObjImg.Photo = obj.Photo;
                            ObjImg.ImagePath = obj.ImagePath;
                            ObjImg.SeqNo = obj.SeqNo;
                            ObjImg.Day = LabDayNo;
                            ObjImg.ServerImageName = obj.OriginalImagePath;
                            ImageList.Add(ObjImg);

                        }

                        BizActionObj.PGDGeneralDetails.ImgList = ImageList;
                        //

                        // Uri address1 = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
                       // PalashServiceClient client1 = new PalashServiceClient("CustomBinding_IPalashService",  Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client1.ProcessCompleted += (s1, arg1) =>
                        {
                            if (arg1.Error == null && arg1.Result != null)
                            {

                                //if (AttachedFileContents !=null)
                                //{
                                //    Uri address2 = new Uri(Application.Current.Host.Source, "../EmailTemplateAttachment");
                                //    string filePath = address2.ToString();

                                //    string file = filePath+"/"+"LabDayNo" + LabDayNo + "LabDayID" + LabDayID + "LabDayUnitID" + LabDayUnitID + txtFN.Text.Trim();
                                //    using (FileStream fs = new FileStream(file, FileMode.Create))
                                //    {
                                //        fs.Write(AttachedFileContents, 0, (int)AttachedFileContents.Length);
                                //    }
                                //}
                                string name = "LabDayNo" + LabDayNo + "LabDayID" + LabDayID + "LabDayUnitID" + LabDayUnitID + "OocyteNumber" + OocyteNumber + "SerialOocyteNumber" + SerialOocyteNumber + txtFN.Text.Trim();
                                obName.Add(Name);
                                // DeleteImage(obName);
                                if (AttachedFileContents != null)
                                    UploadImage(name, AttachedFileContents);

                                MessageBoxControl.MessageBoxChildWindow msgForTemplateSave = new MessageBoxChildWindow("Palash", "Details Saved Sucessfully", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                msgForTemplateSave.OnMessageBoxClosed += new MessageBoxChildWindow.MessageBoxClosedDelegate(msgForTemplateUpdate_OnMessageBoxClosed);
                                msgForTemplateSave.Show();
                            }
                        };
                        client1.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client1.CloseAsync();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }

        }
        private void UploadImage(string name, byte[] AttachedFileContents)
        {
            //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateService.svc"); // this url will work both in dev and after deploy
            //DataTemplateServiceClient client = new DataTemplateServiceClient("CustomBinding_DataTemplateService", address.AbsoluteUri);

            Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
            DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
            client.UploadReportFileCompleted += (s, args) =>
            {
                if (args.Error == null)
                {
                    //if (OnSaveButtonClick != null)
                    //{
                    //    OnSaveButtonClick((clsRadOrderBookingDetailsVO)(this.DataContext), new RoutedEventArgs());
                    //}
                    //this.DialogResult = true;
                }
            };
            client.UploadReportFileAsync(name, AttachedFileContents);
        }
        ObservableCollection<string> obName = new ObservableCollection<string>();
        private void DeleteImage(ObservableCollection<string> name)
        {
            //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateService.svc"); // this url will work both in dev and after deploy
            //DataTemplateServiceClient client = new DataTemplateServiceClient("CustomBinding_DataTemplateService", address.AbsoluteUri);

            Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
            DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
            client.DeleteReportFileCompleted += (s1, args1) =>
            {
                if (args1.Error == null)
                {

                }
            };
            client.DeleteReportFileAsync(name);
        }
        void msgForTemplateUpdate_OnMessageBoxClosed(MessageBoxResult result)
        {
            this.DialogResult = true;
        }
        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            fillChromosomeStudied();
        }
        private List<MasterListItem> _ChromosomeStudied = new List<MasterListItem>();
        public List<MasterListItem> ChromosomeStudied
        {
            get
            {
                return _ChromosomeStudied;
            }
            set
            {
                _ChromosomeStudied = value;
            }
        }
        private List<MasterListItem> _TestOrdered = new List<MasterListItem>();
        public List<MasterListItem> TestOrdered
        {
            get
            {
                return _TestOrdered;
            }
            set
            {
                _TestOrdered = value;
            }
        }
        private void fillChromosomeStudied()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_Chromosome;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc");
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                    ChromosomeStudied = ((clsGetMasterListBizActionVO)args.Result).MasterList;


                    fillTestOrdered();
                }

            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void fillTestOrdered()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_ChromosomeTestOrdered;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc");
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                    TestOrdered = ((clsGetMasterListBizActionVO)args.Result).MasterList;


                    cmbTestordered.ItemsSource = null;
                    cmbTestordered.ItemsSource = TestOrdered.DeepCopy();
                    cmbTestordered.SelectedItem = TestOrdered[0].DeepCopy();

                    fillBiopsy();
                }

            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void fillBiopsy()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVFBiopsy;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc");
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cmbBiopsy.ItemsSource = null;
                    cmbBiopsy.ItemsSource = objList;
                    cmbBiopsy.SelectedItem = objList[0];

                    FillCultureType();
                }


            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillCultureType()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVFPGDCultureType;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc");
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    cmbCultureType.ItemsSource = null;
                    cmbCultureType.ItemsSource = objList;
                    cmbCultureType.SelectedItem = objList[0];

                    FillBindingTechnique();
                }



            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillBindingTechnique()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVFPGDBindingTechnique;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc");
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cmbBindingTechnique.ItemsSource = null;
                    cmbBindingTechnique.ItemsSource = objList;
                    cmbBindingTechnique.SelectedItem = objList[0];
                    //FillSpecimanUsed();
                    fillPhysician();
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void FillSpecimanUsed()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVFPGDSpecimanUsed;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc");
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cmbSpecimanUsed.ItemsSource = null;
                    cmbSpecimanUsed.ItemsSource = objList;
                    cmbSpecimanUsed.SelectedItem = objList[0];
                    fillPhysician();


                }



            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillFamilyHistory()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_FamilyHistory;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc");
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cmbFamilyHistory.ItemsSource = null;
                    cmbFamilyHistory.ItemsSource = objList;
                    cmbFamilyHistory.SelectedItem = objList[0];
                    fillAffectedPerson();
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillAffectedPerson()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_AffectedPartner;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc");
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cmbAffectedPartner.ItemsSource = null;
                    cmbAffectedPartner.ItemsSource = objList;
                    cmbAffectedPartner.SelectedItem = objList[0];                  
                }
                FillHistoryDetails();
               
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillPhysician()
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                    cmbPhysician.ItemsSource = null;
                    cmbPhysician.ItemsSource = objList.DeepCopy();
                    cmbPhysician.SelectedItem = objList[0].DeepCopy();

                    //cmbSupervisor.ItemsSource = null;
                    //cmbSupervisor.ItemsSource = objList.DeepCopy();
                    //cmbSupervisor.SelectedItem = objList[0].DeepCopy();

                    //FillHistoryDetails();

                    cmbReferralDoctor.ItemsSource = null;
                    cmbReferralDoctor.ItemsSource = objList.DeepCopy();
                    cmbReferralDoctor.SelectedItem = objList[0].DeepCopy();

                    cmbSupervisor.ItemsSource = null;
                    cmbSupervisor.ItemsSource = objList.DeepCopy();
                    cmbSupervisor.SelectedItem = objList[0].DeepCopy();

                    FillRefDoctor();
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillRefDoctor()
        {
            try
            {
                clsGetRefernceDoctorBizActionVO BizAction = new clsGetRefernceDoctorBizActionVO();
                BizAction.ComboList = new List<clsComboMasterBizActionVO>();

                //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
                //PalashServiceClient Client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {

                    if (e.Error == null && e.Result != null)
                    {
                        if (((clsGetRefernceDoctorBizActionVO)e.Result).ComboList != null)
                        {

                            clsGetRefernceDoctorBizActionVO result = (clsGetRefernceDoctorBizActionVO)e.Result;
                            List<MasterListItem> objList = new List<MasterListItem>();

                            objList.Add(new MasterListItem(0, "- Select -"));
                            if (result.ComboList != null)
                            {
                                foreach (var item in result.ComboList)
                                {
                                    MasterListItem Objmaster = new MasterListItem();
                                    Objmaster.ID = item.ID;
                                    Objmaster.Description = item.Value;
                                    objList.Add(Objmaster);

                                }
                            }

                            //cmbReferralDoctor.ItemsSource = null;
                            //cmbReferralDoctor.ItemsSource = objList;
                            //cmbReferralDoctor.SelectedItem = objList[0];

                            //if (this.DataContext != null)
                            //{
                            //    cmbReferralDoctor.SelectedValue = ((clsPatientVO)this.DataContext).ReferralDoctorID;

                            //}
                        }
                    }

                    //FillHistoryDetails();
                    //FillWitnessBy();        // For IVF ADM Changes
                    FillPGDIndication();
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                //wait.Close();
                throw;
            }
        }

        public void FillWitnessBy()  //FillLabIncharge() // For IVF ADM Changes
        {
            clsGetUserMasterListBizActionVO BizAction = new clsGetUserMasterListBizActionVO();
            BizAction.IsDecode = false;
            BizAction.MasterList = new List<MasterListItem>();

            BizAction.IsDisplayStaffName = true; // For IVF ADM Changes

            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient Client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "--Select--"));
                    objList.AddRange(((clsGetUserMasterListBizActionVO)e.Result).MasterList);

                    //var result = from item in objList
                    //             where item.ID == ((IApplicationConfiguration)App.Current).CurrentUser.ID
                    //             select item;

                    cmbSupervisor.ItemsSource = null;
                    cmbSupervisor.ItemsSource = objList;
                    cmbSupervisor.SelectedItem = objList[0];  // result.ToList()[0];

                    FillPGDIndication();  // For IVF ADM Changes
                }

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillPGDIndication()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_PGDIndication;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc");
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cmbPGDIndication.ItemsSource = null;
                    cmbPGDIndication.ItemsSource = objList;
                    cmbPGDIndication.SelectedItem = objList[0];

                    //FillHistoryDetails();
                    FillRefferingFacility();
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillRefferingFacility()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_RefferingFacility;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc");
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cmbReferringFacitity.ItemsSource = null;
                    cmbReferringFacitity.ItemsSource = objList;
                    cmbReferringFacitity.SelectedItem = objList[0];

                    FillFamilyHistory();
                  
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        public byte[] AttachedFileContents { get; set; }
        System.Collections.ObjectModel.ObservableCollection<string> AttachedFileNameList = new System.Collections.ObjectModel.ObservableCollection<string>();
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFN.Text))
            {
                if (AttachedFileContents != null)
                {
                    //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateService.svc"); // this url will work both in dev and after deploy
                    //DataTemplateServiceClient client = new DataTemplateServiceClient("CustomBinding_DataTemplateService", address.AbsoluteUri);

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

        private void CmdBrowse1_Click(object sender, RoutedEventArgs e)
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

        private void ImgLink_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFN.Text))
            {
                //{
                //    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateService.svc"); // this url will work both in dev and after deploy
                //    DataTemplateServiceClient client = new DataTemplateServiceClient("CustomBinding_DataTemplateService", address.AbsoluteUri);
                //    client.GlobalUploadFileCompleted += (s, args) =>
                //    {
                //        if (args.Error == null)
                //        {
                //            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + txtFN.Text });
                //            AttachedFileNameList.Add(txtFN.Text);

                //        }
                //    };
                //    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", txtFN.Text, null);
                //}

                //Uri address = new Uri(Application.Current.Host.Source, "EmailTemplateAttachment"); // this url will work both in dev and after deploy
                //DataTemplateServiceClient client = new DataTemplateServiceClient("CustomBinding_DataTemplateService", address.AbsoluteUri);
                //client.UploadReportFileCompleted += (s, args) =>
                //{
                //    if (args.Error == null)
                //    {
                //        HtmlPage.Window.Invoke("OpenReport", new string[] { SourceURL });
                //        AttachedFileNameList.Add(SourceURL);
                //    }
                //};
                //client.UploadReportFileAsync(SourceURL, null);
                //client.CloseAsync();


                Uri address = new Uri(Application.Current.Host.Source, "../PatientPathTestReportDocuments");
                string fileName1 = address.ToString();
                fileName1 = fileName1 + "/" + SourceURL.Trim();  //txtFN.Text.Trim();
                //HtmlPage.Window.Invoke("open", new string[] { fileName1, "", "" });
                HtmlPage.Window.Invoke("Open", fileName1);


            }
        }

        private void cmbTestOrdered_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (FISH == null)
            {
                FISH = new ObservableCollection<clsPGDFISHVO>();
            }
            FISH.Add(new clsPGDFISHVO() { ChromosomeStudiedIdList = ChromosomeStudied, ChromosomeStudiedID = 0, TestOrderedIdList = TestOrdered, TestOrderedID = 0, Status = true });
            dgFISHGrid.ItemsSource = FISH;
            if (Karyotyping == null)
            {
                Karyotyping = new ObservableCollection<clsPGDKaryotypingVO>();
            }
            Karyotyping.Add(new clsPGDKaryotypingVO() { ChromosomeStudiedIdList = ChromosomeStudied, ChromosomeStudiedID = 0, Status = true });
            dgKaryotypingGrid.ItemsSource = Karyotyping;

        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (dgFISHGrid.SelectedItem != null)
            {
                int index;
                if (FISH.Count > 1)
                {
                    if (dgFISHGrid.SelectedIndex > 0)
                    {
                        index = dgFISHGrid.SelectedIndex;
                        if (FISH[index].ID > 0)
                        {

                            FISH[index].Status = false;
                            Karyotyping[index].Status = false;
                            RemoveFISH.Add(FISH[index]);
                            RemoveKaryotyping.Add(Karyotyping[index]);
                        }
                        FISH.RemoveAt(index);
                        Karyotyping.RemoveAt(index);
                    }
                }
            }
            dgFISHGrid.ItemsSource = FISH;
            dgFISHGrid.UpdateLayout();
            dgKaryotypingGrid.ItemsSource = Karyotyping;
            dgKaryotypingGrid.UpdateLayout();
        }

        private void cmbChromosomeStudied_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((AutoCompleteComboBox)sender).Name.Equals("cmbChromosomeStudied"))
            {
                for (int i = 0; i < FISH.Count; i++)
                {
                    if (FISH[i] == ((clsPGDFISHVO)dgFISHGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteComboBox)sender).SelectedItem) != null)
                        {
                            FISH[i].ChromosomeStudiedID = ((MasterListItem)((AutoCompleteComboBox)sender).SelectedItem).ID;
                            if (FISH.Count == Karyotyping.Count)
                                Karyotyping[i].SelectedChromosomeStudiedId = ChromosomeStudied.FirstOrDefault(p => p.ID == ((MasterListItem)((AutoCompleteComboBox)sender).SelectedItem).ID);


                        }
                    }
                }
                dgKaryotypingGrid.ItemsSource = null;
                dgKaryotypingGrid.ItemsSource = Karyotyping;
                dgKaryotypingGrid.UpdateLayout();
            }
        }

        private void cmbKaryotypingChromosomeStudied_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        long HistoryID = 0;
        public void FillHistoryDetails()
        {
            clsGetPGDHistoryBizActionVO BizAction = new clsGetPGDHistoryBizActionVO();
            BizAction.PGDDetails = new clsPGDHistoryVO();
            BizAction.PGDDetails.PlanTherapyID = PlanTherapyID;
            BizAction.PGDDetails.PlanTherapyUnitID = PlanTherapyUnitID;
            BizAction.PGDDetails.PatientID = PatientID;
            BizAction.PGDDetails.PatientUnitID = PatientUnitID;
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetPGDHistoryBizActionVO)arg.Result).SuccessStatus != null)
                    {
                        this.DataContext = null;
                        this.DataContext = ((clsGetPGDHistoryBizActionVO)arg.Result).PGDDetails;
                        HistoryDetails.DataContext = ((clsGetPGDHistoryBizActionVO)arg.Result).PGDDetails;
                        //added by neena
                        if (((clsGetPGDHistoryBizActionVO)arg.Result).PGDDetails.FamilyHistory != null && ((clsGetPGDHistoryBizActionVO)arg.Result).PGDDetails.FamilyHistory != 0)
                        {
                            cmbFamilyHistory.SelectedValue = ((clsGetPGDHistoryBizActionVO)arg.Result).PGDDetails.FamilyHistory;
                        }

                        if (((clsGetPGDHistoryBizActionVO)arg.Result).PGDDetails.AffectedPartner != null && ((clsGetPGDHistoryBizActionVO)arg.Result).PGDDetails.AffectedPartner != 0)
                        {
                            cmbAffectedPartner.SelectedValue = ((clsGetPGDHistoryBizActionVO)arg.Result).PGDDetails.AffectedPartner;
                        }
                        //
                        HistoryID = ((clsGetPGDHistoryBizActionVO)arg.Result).PGDDetails.ID;
                    }
                }
                FillResult();

                //FillDetails();
               
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillResult()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_BiopsyResult;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc");
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                   List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cmbResult.ItemsSource = null;
                    cmbResult.ItemsSource = objList;
                    cmbResult.SelectedItem = objList[0];

                    FillDetails();
                }

            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        string SourceURL = "";
        public void FillDetails()
        {

            clsGetPGDGeneralDetailsBizActionVO BizAction = new clsGetPGDGeneralDetailsBizActionVO();
            BizAction.PGDGeneralDetails = new clsPGDGeneralDetailsVO();
            BizAction.PGDFISHList = new List<clsPGDFISHVO>();
            BizAction.PGDKaryotypingList = new List<clsPGDKaryotypingVO>();
            BizAction.PGDGeneralDetails.LabDayID = LabDayID;
            BizAction.PGDGeneralDetails.LabDayUnitID = LabDayUnitID;
            BizAction.PGDGeneralDetails.LabDayNo = LabDayNo;
            BizAction.PGDGeneralDetails.PlanTherapyID = PlanTherapyID;
            BizAction.PGDGeneralDetails.PlanTherapyUnitID = PlanTherapyUnitID;
            BizAction.PGDGeneralDetails.SerialEmbNumber = SerialOocyteNumber;

            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if ((clsGetPGDGeneralDetailsBizActionVO)arg.Result != null)
                    {
                        this.DataContext = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails;

                        if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.ID > 0 && IsFreezed == true)
                        {
                            cmdSave.IsEnabled = false;
                        }


                        if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.FileName != null)
                        {
                            txtFN.Text = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.FileName;
                        }
                        if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.SourceURL != null)
                        {
                            SourceURL = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.SourceURL;
                        }

                        if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.Physician != null && ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.Physician != 0)
                        {
                            cmbPhysician.SelectedValue = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.Physician;
                        }
                        if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.BiospyID != null && ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.BiospyID != 0)
                        {
                            cmbBiopsy.SelectedValue = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.BiospyID;
                        }
                        if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.SpecimanUsedID != null && ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.SpecimanUsedID != 0)
                        {
                            cmbSpecimanUsed.SelectedValue = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.SpecimanUsedID;
                        }

                        if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.MainFISHRemark != null)
                        {
                            txtMainFSHRemark.Text = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.MainFISHRemark;
                        }

                        if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.MainKaryotypingRemark != null)
                        {
                            txtMainKaryotypingRemark.Text = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.MainKaryotypingRemark;
                        }

                        if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.ResultID != null && ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.ResultID != 0)
                        {
                            cmbResult.SelectedValue = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.ResultID;
                        }

                        #region For ART Flow - PGD

                        //BizActionObj.PGDGeneralDetails.SampleReceiveDate = SampleReceiveDate.SelectedDate.Value.Date;
                        //BizActionObj.PGDGeneralDetails.SampleReceiveDate = BizActionObj.PGDGeneralDetails.SampleReceiveDate.Value.Add(SampleReceiveTime.Value.Value.TimeOfDay);

                        //BizActionObj.PGDGeneralDetails.ResultDate = ResultDate.SelectedDate.Value.Date;
                        //BizActionObj.PGDGeneralDetails.ResultDate = BizActionObj.PGDGeneralDetails.ResultDate.Value.Add(TimeOfResult.Value.Value.TimeOfDay);

                        //if (cmbReferralDoctor.SelectedItem != null)
                        //    BizActionObj.PGDGeneralDetails.ReferringID = ((MasterListItem)cmbReferralDoctor.SelectedItem).ID;

                        if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.ReferringID != null && ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.ReferringID != 0)
                        {
                            cmbReferralDoctor.SelectedValue = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.ReferringID;
                        }

                        //if (cmbSupervisor.SelectedItem != null)
                        //    BizActionObj.PGDGeneralDetails.SupervisedById = ((MasterListItem)cmbSupervisor.SelectedItem).ID;

                        if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.SupervisedById != null && ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.SupervisedById != 0)
                        {
                            cmbSupervisor.SelectedValue = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.SupervisedById;
                        }
                       

                        //if (cmbTestordered.SelectedItem != null)
                        //    BizActionObj.PGDGeneralDetails.TestOrderedID = ((MasterListItem)cmbTestordered.SelectedItem).ID;

                        if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.TestOrderedID != null && ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.TestOrderedID != 0)
                        {
                            cmbTestordered.SelectedValue = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.TestOrderedID;
                        }

                        //BizActionObj.PGDGeneralDetails.MainFISHInterpretation = txtMainFSHInterpretation.Text;

                        if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.MainFISHInterpretation != null)
                        {
                            txtMainFSHInterpretation.Text = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.MainFISHInterpretation;
                        }

                        if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.PGDIndicationID != null && ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.PGDIndicationID != 0)
                        {
                            cmbPGDIndication.SelectedValue = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.PGDIndicationID;
                        }

                        if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.PGDIndicationDetails != null)
                        {
                            txtIndicationDetails.Text = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.PGDIndicationDetails;
                        }

                        if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.ReferringFacilityID != null && ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.ReferringFacilityID != 0)
                        {
                            cmbReferringFacitity.SelectedValue = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.ReferringFacilityID;    // For IVF ADM Changes
                        }

                        // For IVF ADM Changes
                        if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.PGDPGSProcedureID != null && ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.PGDPGSProcedureID != 0)
                        {
                            if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.PGDPGSProcedureID == 1)
                            {
                                rdbPGD.IsChecked = true;
                            }
                            else if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.PGDPGSProcedureID == 2)
                            {
                                rdbPGS.IsChecked = true;
                            }
                            else
                            {
                                rdbPGD.IsChecked = false;
                                rdbPGS.IsChecked = false;
                            }
                        }
                        else
                        {
                            //rdbPGD.IsChecked = false;
                            //rdbPGS.IsChecked = false;
                        }

                        #endregion

                        if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList != null && ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList.Count > 0)
                        {
                            for (int i = 0; i < ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList.Count; i++)
                            {
                                ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList[i].ChromosomeStudiedIdList = ChromosomeStudied;

                                if (Convert.ToInt64(((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList[i].ChromosomeStudiedID) > 0)
                                {
                                    ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList[i].SelectedChromosomeStudiedId = ChromosomeStudied.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList[i].ChromosomeStudiedID));
                                }
                                else
                                {
                                    ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList[i].SelectedChromosomeStudiedId = ChromosomeStudied.FirstOrDefault(p => p.ID == 0);
                                }
                                ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList[i].TestOrderedIdList = TestOrdered;

                                if (Convert.ToInt64(((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList[i].TestOrderedID) > 0)
                                {
                                    ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList[i].SelectedTestOrderedId = TestOrdered.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList[i].TestOrderedID));
                                }
                                else
                                {
                                    ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList[i].SelectedTestOrderedId = TestOrdered.FirstOrDefault(p => p.ID == 0);
                                }

                                FISH.Add(((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDFISHList[i]);

                            }
                            cmdPrint.IsEnabled = true;
                        }
                        else
                        {
                            cmdPrint.IsEnabled = false;
                            fillInitailFISHDetails();
                        }

                        if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList != null && ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList.Count > 0)
                        {
                            if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList[0].BindingTechnique != null)
                                cmbBindingTechnique.SelectedValue = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList[0].BindingTechnique;
                            if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList[0].CultureTypeID != null)
                                cmbCultureType.SelectedValue = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList[0].CultureTypeID;
                            for (int i = 0; i < ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList.Count; i++)
                            {
                                ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList[i].ChromosomeStudiedIdList = ChromosomeStudied;

                                if (Convert.ToInt64(((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList[i].ChromosomeStudiedID) > 0)
                                {
                                    ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList[i].SelectedChromosomeStudiedId = ChromosomeStudied.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList[i].ChromosomeStudiedID));
                                }
                                else
                                {
                                    ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList[i].SelectedChromosomeStudiedId = ChromosomeStudied.FirstOrDefault(p => p.ID == 0);
                                }
                                Karyotyping.Add(((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDKaryotypingList[i]);

                            }
                        }


                        dgFISHGrid.ItemsSource = null;
                        dgFISHGrid.ItemsSource = FISH;

                        dgKaryotypingGrid.ItemsSource = null;
                        dgKaryotypingGrid.ItemsSource = Karyotyping;


                        if (((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.ImgList != null && ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.ImgList.Count > 0)
                        {
                            ImageList = new List<clsAddImageVO>();
                            mylistitem = new List<ListItems>();
                            foreach (var item in ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.ImgList)
                            {
                                //BitmapImage img = new BitmapImage();
                                //img.SetSource(new MemoryStream(item.Photo, false));

                                //clsAddImageVO ObjNew = new clsAddImageVO();
                                //ObjNew.Photo = item.Photo;
                                //ObjNew.ImagePath = item.ImagePath;
                                ////ObjNew.BitImagePath = img;

                                //ImageList.Add(ObjNew);

                                ListItems obj = new ListItems();
                                obj.Image1 = new BitmapImage(new Uri(item.ServerImageName, UriKind.Relative));
                                obj.ImagePath = item.ImagePath;
                                obj.ID = item.ID;
                                obj.SeqNo = item.SeqNo;
                                obj.Photo = item.Photo;
                                obj.Day = item.Day;
                                obj.UnitID = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.UnitID;
                                obj.SerialOocyteNumber = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.SerialEmbNumber;
                                obj.OocyteNumber = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.OocyteNumber;
                                obj.PatientID = item.PatientID;
                                obj.PatientUnitID = item.PatientUnitID;
                                obj.PlanTherapyID = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.PlanTherapyID;
                                obj.PlanTherapyUnitID = ((clsGetPGDGeneralDetailsBizActionVO)arg.Result).PGDGeneralDetails.PlanTherapyUnitID;
                                obj.OriginalImagePath = item.OriginalImagePath;
                                //if (((clsIVFDashboard_GetDay0DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                                //    obj.IsDelete = false;
                                //else
                                //    obj.IsDelete = true;
                                mylistitem.Add(obj);
                                GetMylistitem.Add(obj);
                            }

                            dgImgList.ItemsSource = null;
                            dgImgList.ItemsSource = mylistitem;
                            // GetMylistitem = mylistitem.DeepCopy();
                            txtFN.Text = mylistitem.Count.ToString();
                        }

                    }
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        public void fillInitailFISHDetails()
        {
            FISH.Add(new clsPGDFISHVO() { ChromosomeStudiedIdList = ChromosomeStudied, ChromosomeStudiedID = 0, TestOrderedIdList = TestOrdered, TestOrderedID = 0, Status = true });
            dgFISHGrid.ItemsSource = FISH;
            Karyotyping.Add(new clsPGDKaryotypingVO { ChromosomeStudiedIdList = ChromosomeStudied, ChromosomeStudiedID = 0, Status = true });
            dgKaryotypingGrid.ItemsSource = Karyotyping;

        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/IVFDashboard/IVFDashboard_PatientFISHReport.aspx?TherapyId=" + PlanTherapyID + "&TherapyUnitId=" + PlanTherapyUnitID + "&PatientID=" + PatientID + "&PatientUnitID=" + PatientUnitID + "&OocyteNumber=" + OocyteNumber + "&SerialOocyteNumber=" + SerialOocyteNumber), "_blank");
        }

        private void SampleReceiveDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to delete image", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);
            msgW1.Show();

        }

        List<clsAddImageVO> ImageList = new List<clsAddImageVO>();

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (mylistitem.Count > GetMylistitem.Count)
                {
                    mylistitem.Remove((ListItems)dgImgList.SelectedItem);
                    dgImgList.ItemsSource = null;
                    dgImgList.ItemsSource = mylistitem;
                    txtFN.Text = mylistitem.Count.ToString();
                }
                else
                {
                    clsIVFDashboard_DeleteAndGetLabDayImagesBizActionVO BizAction = new clsIVFDashboard_DeleteAndGetLabDayImagesBizActionVO();
                    BizAction.ImageObj = new clsAddImageVO();
                    ListItems obj = (ListItems)dgImgList.SelectedItem;
                    BizAction.ImageObj.PatientID = obj.PatientID;
                    BizAction.ImageObj.PatientUnitID = obj.PatientUnitID;
                    BizAction.ImageObj.PlanTherapyID = obj.PlanTherapyID;
                    BizAction.ImageObj.PlanTherapyUnitID = obj.PlanTherapyUnitID;
                    BizAction.ImageObj.OocyteNumber = obj.OocyteNumber;
                    BizAction.ImageObj.SerialOocyteNumber = obj.SerialOocyteNumber;
                    BizAction.ImageObj.Day = obj.Day;
                    //BizAction.ImageObj.ServerImageName = obj.ServerImageName;
                    BizAction.ImageObj.OriginalImagePath = obj.OriginalImagePath;


                    //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
                    //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            if (((clsIVFDashboard_DeleteAndGetLabDayImagesBizActionVO)arg.Result).ImageList != null && ((clsIVFDashboard_DeleteAndGetLabDayImagesBizActionVO)arg.Result).ImageList.Count > 0)
                            {
                                ImageList = new List<clsAddImageVO>();
                                foreach (var item in ((clsIVFDashboard_DeleteAndGetLabDayImagesBizActionVO)arg.Result).ImageList)
                                {
                                    mylistitem = new List<ListItems>();
                                    mylistitem.Add(
                                        new ListItems
                                        {
                                            Image1 = new BitmapImage(new Uri(item.ServerImageName, UriKind.Relative)),
                                            ImagePath = item.ImagePath,
                                            ID = item.ID,
                                            SeqNo = item.SeqNo,
                                            Photo = item.Photo,
                                            Day = item.Day,
                                            UnitID = item.UnitID,
                                            SerialOocyteNumber = item.SerialOocyteNumber,
                                            OocyteNumber = item.OocyteNumber,
                                            PatientID = item.PatientID,
                                            PatientUnitID = item.PatientUnitID,
                                            PlanTherapyID = item.PlanTherapyID,
                                            PlanTherapyUnitID = item.PlanTherapyUnitID,
                                            OriginalImagePath = item.OriginalImagePath,
                                            IsDelete = true
                                        });

                                }
                                txtFN.Text = "";
                                dgImgList.ItemsSource = null;
                                dgImgList.ItemsSource = mylistitem;
                                txtFN.Text = mylistitem.Count.ToString();
                            }
                            else
                            {
                                mylistitem = new List<ListItems>();
                                dgImgList.ItemsSource = null;
                                dgImgList.ItemsSource = mylistitem;
                                txtFN.Text = mylistitem.Count.ToString();
                            }
                            ////MessageBoxControl.MessageBoxChildWindow msgW1 =
                            ////       new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            ////msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                            ////{
                            ////    if (res == MessageBoxResult.OK)
                            ////    {

                            ////    }
                            ////};
                            ////msgW1.Show();
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
            }
        }

        byte[] MyPhoto { get; set; }
        private void CmdBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Multiselect = false;
            openDialog.Filter = "(*.jpg, *.png)|*.jpg;*.png;";
            openDialog.FilterIndex = 1;
            if (openDialog.ShowDialog() == true)
            {
                txtFN.Text = openDialog.File.Name;
                WriteableBitmap imageSource = new WriteableBitmap(200, 100);
                try
                {
                    //imageSource.SetSource(openDialog.File.OpenRead());
                    //ImageList.Add(imageSource);
                    //dgImgList.ItemsSource = ImageList;

                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        MyPhoto = new byte[stream.Length];
                        stream.Read(MyPhoto, 0, (int)stream.Length);
                        ShowPhoto(MyPhoto, txtFN.Text);


                    }

                }
                catch (Exception ex)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error while reading file.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
            }
        }

        List<ListItems> mylistitem = new List<ListItems>();
        List<ListItems> GetMylistitem = new List<ListItems>();
        private void ShowPhoto(byte[] MyPhoto, string ImgName)
        {
            BitmapImage img = new BitmapImage();
            img.SetSource(new MemoryStream(MyPhoto, false));

            ListItems obj = new ListItems();
            obj.Image1 = img;
            obj.Photo = MyPhoto;
            obj.ImagePath = ImgName;
            obj.IsDelete = true;

            var item1 = from r in mylistitem.ToList()
                        where r.ImagePath == obj.ImagePath
                        select r;

            if (item1.ToList().Count == 0)
            {
                mylistitem.Add(obj);
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "'" + obj.ImagePath + "'" + " File is already Added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }

            //mylistitem.Add(new ListItems { Image1 = img, Photo = MyPhoto, ImagePath = ImgName });

            dgImgList.ItemsSource = null;
            dgImgList.ItemsSource = mylistitem;

            txtFN.Text = mylistitem.Count.ToString();
            //clsAddImageVO ObjNew = new clsAddImageVO();
            //ObjNew.Photo = MyPhoto;
            //ObjNew.ImagePath = ImgName;
            //ObjNew.BitImagePath = img;

            //ImageList.Add(ObjNew);
            //dgImgList.ItemsSource = null;
            //dgImgList.ItemsSource = ImageList;
        }
    }
}

