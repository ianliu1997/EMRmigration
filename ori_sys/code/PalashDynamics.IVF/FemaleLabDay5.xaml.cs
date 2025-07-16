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
using System.Collections.ObjectModel;
using CIMS;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;
using System.IO;
using MessageBoxControl;
using PalashDynamics.UserControls;
using System.Windows.Media.Imaging;
using DataDrivenApplication.Forms;
namespace PalashDynamics.IVF
{
    public partial class FemaleLabDay5 : UserControl,IInitiateCIMS
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
    
        public FemaleLabDay5()
        {
            this.DataContext = new clsFemaleLabDay5VO();
            InitializeComponent();
        }

        long ID { get; set; }
        string Impression { get; set; }
        long SummaryID { get; set; }
        long UnitID { get; set; }

        public FemaleLabDay5(clsCoupleVO Couple,clsLabDaysSummaryVO Summary)
        {
            try
            {
                InitializeComponent();
          
                CoupleDetails = Couple;
                this.DataContext = new clsFemaleLabDay5VO() { ID = Summary.OocyteID, UnitID = Summary.UnitID, CoupleID = CoupleDetails.CoupleId, CoupleUnitID = CoupleDetails.CoupleUnitId, LabDaySummary = Summary };
                ID = Summary.OocyteID;
                Impression = Summary.Impressions;
                SummaryID = Summary.ID;
                UnitID = Summary.UnitID;              
                IsUpdate = true;
                
            }
            catch (Exception ex)
            {

            }
        }


        #region Variable Declaration
        
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
       
        public bool IsUpdate = false;
        public bool IsPatientExist = false;
        public bool IsPageLoded = false;

        long StoreID = 0;


        long CoupleId { get; set; }
        long CoupleUnitId { get; set; }

        long SourceOfSemen { get; set; }
        long MethodOfSpermPreparation { get; set; }
        ListBox lstFUBox;

        clsFemaleSemenDetailsVO SemenDetails = new clsFemaleSemenDetailsVO();
        List<FileUpload> FUSetting = new List<FileUpload>();
        ObservableCollection<string> listOfReports = new ObservableCollection<string>();
        ObservableCollection<clsFemaleLabDay5FertilizationAssesmentVO> FertilizationList = new ObservableCollection<clsFemaleLabDay5FertilizationAssesmentVO>();
        clsFemaleLabDay5CalculateDetailsVO CalcDetails = new clsFemaleLabDay5CalculateDetailsVO();
        ObservableCollection<clsFemaleMediaDetailsVO> MediaList = new ObservableCollection<clsFemaleMediaDetailsVO>();

        List<MasterListItem> FertilizationStage = new List<MasterListItem>();
        List<MasterListItem> GradeList = new List<MasterListItem>();
        List<MasterListItem> PlanList = new List<MasterListItem>();
        List<MasterListItem> ICMList = new List<MasterListItem>();
        List<MasterListItem> TrophectodermList = new List<MasterListItem>();
        List<MasterListItem> ExpansionList = new List<MasterListItem>();
        List<MasterListItem> BlastocytsGradeList = new List<MasterListItem>();

        WaitIndicator Indicatior = null;
        #endregion

        #region FillCombobox

        private void FillEmbryologist()
        {
            clsGetEmbryologistBizActionVO BizAction = new clsGetEmbryologistBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            BizAction.UnitId = 0;
            BizAction.DepartmentId = 0;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetEmbryologistBizActionVO)arg.Result).MasterList);

                    cmbEmbryologist.ItemsSource = null;
                    cmbEmbryologist.ItemsSource = objList;
                    cmbEmbryologist.SelectedItem = objList[0];



                    cmbAssistantEmbryologist.ItemsSource = null;
                    cmbAssistantEmbryologist.ItemsSource = objList;
                    cmbAssistantEmbryologist.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {
                        cmbEmbryologist.SelectedValue = ((clsFemaleLabDay5VO)this.DataContext).EmbryologistID;
                        cmbAssistantEmbryologist.SelectedValue = ((clsFemaleLabDay5VO)this.DataContext).AssEmbryologistID;


                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillAnesthetist()
        {
            clsGetAnesthetistBizActionVO BizAction = new clsGetAnesthetistBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            BizAction.UnitId = 0;
            BizAction.DepartmentId = 0;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetAnesthetistBizActionVO)arg.Result).MasterList);

                    cmbAnesthetist.ItemsSource = null;
                    cmbAnesthetist.ItemsSource = objList;
                    cmbAnesthetist.SelectedItem = objList[0];



                    cmbAssistantAnesthetist.ItemsSource = null;
                    cmbAssistantAnesthetist.ItemsSource = objList;
                    cmbAssistantAnesthetist.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {
                        cmbAnesthetist.SelectedValue = ((clsFemaleLabDay5VO)this.DataContext).AnesthetistID;
                        cmbAssistantAnesthetist.SelectedValue = ((clsFemaleLabDay5VO)this.DataContext).AssAnesthetistID;


                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillNeedleSource()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_SourceNeedleMaster;
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

                    cmbSourceNeedle.ItemsSource = null;
                    cmbSourceNeedle.ItemsSource = objList;
                    cmbSourceNeedle.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {
                        cmbSourceNeedle.SelectedValue = ((clsFemaleLabDay5VO)this.DataContext).SourceNeedleID;


                    }
                }



            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }


        private void FillFertilization()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_FertilizationStageMaster;
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

                    FertilizationStage = objList;

                    FillGrade();

                }



            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillGrade()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_GradeMaster;
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

                    GradeList = objList;
                    //FillPlan();
                    FillICM();

                }



            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillPlan()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_PlanMaster;
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

                    PlanList = objList;
                    
                    if (IsUpdate == true)
                    {
                        FetchData(ID, UnitID, CoupleDetails.CoupleId, CoupleDetails.CoupleUnitId);
                    }
                    else
                    {
                        if (CoupleId > 0)
                        {
                            GetDay4Details();
                        }
                    }

                }



            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillICM()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_ICMMaster;
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

                    ICMList = objList;
                    //FillPlan();
                    FillExpansion();

                }



            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillExpansion()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_ExpansionMaster;
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

                    ExpansionList = objList;
                    //FillPlan();
                    FillBlastocytsGrade();

                }



            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillBlastocytsGrade()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_BlastocytsGradeMaster;
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

                    BlastocytsGradeList = objList;
                    //FillPlan();
                    FillTrophectoderm();

                }



            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillTrophectoderm()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_TrophectodermMaster;
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
                    TrophectodermList = objList;
                  
                    FillPlan();

                }



            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        #endregion

        #region Couple Details
       
        private void fillCoupleDetails()
        {
            clsGetCoupleDetailsBizActionVO BizAction = new clsGetCoupleDetailsBizActionVO();
            BizAction.PatientID = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
            BizAction.CoupleDetails = new clsCoupleVO();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.CoupleDetails.MalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient;
                    BizAction.CoupleDetails.FemalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient;
                    CoupleDetails.MalePatient = new clsPatientGeneralVO();
                    CoupleDetails.FemalePatient = new clsPatientGeneralVO();
                    CoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
                    if (CoupleDetails.CoupleId == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Create Therapy, Therapy is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();

                        ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    }
                    else
                    {
                        CoupleId = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.CoupleId;
                        CoupleUnitId = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.CoupleUnitId;
                        FillFertilization();
                        //getEMRDetails(BizAction.CoupleDetails.FemalePatient, "F");
                        //getEMRDetails(BizAction.CoupleDetails.MalePatient, "M");
                        GetHeightAndWeight(BizAction.CoupleDetails);

                      
                        if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.Photo != null)
                        {

                            WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto13.Width, (int)imgPhoto13.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                            bmp.FromByteArray(((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.Photo);
                            imgPhoto13.Source = bmp;
                            imgPhoto13.Visibility = System.Windows.Visibility.Visible;
                           
                        }

                        else
                        {
                            imgP1.Visibility = System.Windows.Visibility.Visible;
                        }

                        if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.Photo != null)
                        {
                            WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto12.Width, (int)imgPhoto12.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                            bmp.FromByteArray(((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.Photo);

                            imgPhoto12.Source = bmp;

                            imgPhoto12.Visibility = System.Windows.Visibility.Visible;

                        }

                        else
                        {
                            imgP2.Visibility = System.Windows.Visibility.Visible;
                        }
                        
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        
        private void GetHeightAndWeight(clsCoupleVO CoupleDetails)
        {
            clsGetGetCoupleHeightAndWeightBizActionVO BizAction = new clsGetGetCoupleHeightAndWeightBizActionVO();
            BizAction.CoupleDetails = new clsCoupleVO();
            BizAction.FemalePatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.MalePatientID = CoupleDetails.MalePatient.PatientID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (((clsGetGetCoupleHeightAndWeightBizActionVO)args.Result).CoupleDetails != null)
                        BizAction.CoupleDetails = ((clsGetGetCoupleHeightAndWeightBizActionVO)args.Result).CoupleDetails;
                    if (BizAction.CoupleDetails != null)
                    {
                        clsPatientGeneralVO FemalePatientDetails = new clsPatientGeneralVO();

                        FemalePatientDetails = CoupleDetails.FemalePatient;
                        FemalePatientDetails.Height = BizAction.CoupleDetails.FemalePatient.Height;
                        FemalePatientDetails.Weight = BizAction.CoupleDetails.FemalePatient.Weight;
                        FemalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", BizAction.CoupleDetails.FemalePatient.BMI));
                        FemalePatientDetails.Alerts = BizAction.CoupleDetails.FemalePatient.Alerts;
                        Female.DataContext = FemalePatientDetails;

                        clsPatientGeneralVO MalePatientDetails = new clsPatientGeneralVO();
                        MalePatientDetails = CoupleDetails.MalePatient;
                        MalePatientDetails.Height = BizAction.CoupleDetails.MalePatient.Height;
                        MalePatientDetails.Weight = BizAction.CoupleDetails.MalePatient.Weight;
                        MalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", BizAction.CoupleDetails.MalePatient.BMI));
                        MalePatientDetails.Alerts = BizAction.CoupleDetails.MalePatient.Alerts;
                        Male.DataContext = MalePatientDetails;


                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();





        }

        private void getEMRDetails(clsPatientGeneralVO PatientDetails, string Gender)
        {
            clsGetEMRDetailsBizActionVO BizAction = new clsGetEMRDetailsBizActionVO();
            BizAction.PatientID = PatientDetails.PatientID;
            BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
            BizAction.TemplateID = 8;//Using For Getting Height Wight Of Patient 
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Double height = 0;
            Double weight = 0;

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.EMRDetailsList = ((clsGetEMRDetailsBizActionVO)args.Result).EMRDetailsList;

                    if (BizAction.EMRDetailsList != null || BizAction.EMRDetailsList.Count > 0)
                    {
                        for (int i = 0; i < BizAction.EMRDetailsList.Count; i++)
                        {
                            if (BizAction.EMRDetailsList[i].ControlCaption.Equals("Height"))
                            {
                                if (!string.IsNullOrEmpty(BizAction.EMRDetailsList[i].Value))
                                {
                                    height = Convert.ToDouble(BizAction.EMRDetailsList[i].Value);
                                    if (height != 0 && weight != 0)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (BizAction.EMRDetailsList[i].ControlCaption.Equals("Weight"))
                            {
                                if (!string.IsNullOrEmpty(BizAction.EMRDetailsList[i].Value))
                                {
                                    weight = Convert.ToDouble(BizAction.EMRDetailsList[i].Value);
                                    if (height != 0 && weight != 0)
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        if (height != 0 && weight != 0)
                        {
                            if (Gender.Equals("F"))
                            {
                                PatientDetails.Height = height;
                                PatientDetails.Weight = weight;
                                PatientDetails.BMI = Math.Round(CalculateBMI(height, weight), 2);
                                Female.DataContext = PatientDetails;
                            }
                            else
                            {
                                PatientDetails.Height = height;
                                PatientDetails.Weight = weight;
                                PatientDetails.BMI = Math.Round(CalculateBMI(height, weight), 2);
                                Male.DataContext = PatientDetails;
                            }
                        }
                        else
                        {
                            if (Gender.Equals("F"))
                            {
                                Female.DataContext = PatientDetails;
                            }
                            else
                            {
                                Male.DataContext = PatientDetails;
                            }
                        }
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private double CalculateBMI(Double Height, Double Weight)
        {
            try
            {
                if (Weight == 0)
                {
                    return 0.0;

                }
                else if (Height == 0)
                {

                    return 0.0;
                }
                else
                {
                    double weight = Convert.ToDouble(Weight);
                    double height = Convert.ToDouble(Height);
                    double TotalBMI = weight / height;
                    TotalBMI = TotalBMI / height;
                    TotalBMI = TotalBMI * 10000;
                    return TotalBMI;

                }
            }
            catch (Exception ex)
            {
                return 0.0;
            }
        }

        #endregion
       
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsUpdate == false)
            {
                if (IsPatientExist == false)
                {
                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                    ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");

                }
            }
            if (!IsPageLoded && ((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
            {
                
                FillEmbryologist();
                FillAnesthetist();
                FillNeedleSource();
                fillCoupleDetails();
                if (IsUpdate == false)
                {
                    ProcDate.SelectedDate = DateTime.Now.Date.Date;
                    ProcTime.Value = DateTime.Now;
                    LoadFURepeaterControl();

                }
                
            }

            IsPageLoded = true;

        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ChkValidation())
                {

                    ImpressionWindow winImp = new ImpressionWindow();
                    winImp.Day = true;
                    if (IsUpdate == true)
                    {
                        winImp.Impression = Impression;
                    }
                    winImp.OnSaveClick += new RoutedEventHandler(winImp_OnSaveClick);
                    winImp.Show();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        void winImp_OnSaveClick(object sender, RoutedEventArgs e)
        {
            ImpressionWindow ObjImp = (ImpressionWindow)sender;
            if (ObjImp.DialogResult == true)
            {
                if (ObjImp.Impression != null)
                {
                    if (IsUpdate == false)
                    {
                        Save(ObjImp.Impression);
                    }
                    else
                    {
                        Update(ObjImp.Impression);
                    }
                }

            }
        }

        private void Save(string Imp)
        {
            clsAddLabDay5BizActionVO BizAction = new clsAddLabDay5BizActionVO();
            BizAction.Day5Details = (clsFemaleLabDay5VO)this.DataContext;

            BizAction.Day5Details.LabDaySummary = new clsLabDaysSummaryVO();
            BizAction.Day5Details.LabDaySummary.Impressions = Imp;

            BizAction.Day5Details.CoupleID = CoupleId;
            BizAction.Day5Details.CoupleUnitID = CoupleUnitId;


            if (cmbEmbryologist.SelectedItem != null)
                BizAction.Day5Details.EmbryologistID = ((MasterListItem)cmbEmbryologist.SelectedItem).ID;

            if (cmbAssistantEmbryologist.SelectedItem != null)
                BizAction.Day5Details.AssEmbryologistID = ((MasterListItem)cmbAssistantEmbryologist.SelectedItem).ID;

            if (cmbAnesthetist.SelectedItem != null)
                BizAction.Day5Details.AnesthetistID = ((MasterListItem)cmbAnesthetist.SelectedItem).ID;

            if (cmbAssistantAnesthetist.SelectedItem != null)
                BizAction.Day5Details.AssAnesthetistID = ((MasterListItem)cmbAssistantAnesthetist.SelectedItem).ID;

            if (cmbSourceNeedle.SelectedItem != null)
                BizAction.Day5Details.SourceNeedleID = ((MasterListItem)cmbSourceNeedle.SelectedItem).ID;


            BizAction.Day5Details.FertilizationAssesmentDetails = FertilizationList.ToList();

            BizAction.Day5Details.SemenDetails = SemenDetails;
            BizAction.Day5Details.SemenDetails.MethodOfSpermPreparation = MethodOfSpermPreparation;
            BizAction.Day5Details.SemenDetails.SourceOfSemen = SourceOfSemen;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("", "Day 5 details added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();

                    //((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                    //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    //((IApplicationConfiguration)App.Current).FillMenu("IVF Lab Work");

                    LabDaysSummary Lab = new LabDaysSummary();
                    Lab.IsPatientExist = true;
                    ((IApplicationConfiguration)App.Current).OpenMainContent(Lab);
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

        private void Update(string Imp)
        {

            try
            {
                clsAddLabDay5BizActionVO BizAction = new clsAddLabDay5BizActionVO();
                BizAction.Day5Details = (clsFemaleLabDay5VO)this.DataContext;
                BizAction.Day = 5;
                BizAction.Day5Details.ID = ID;
                BizAction.Day5Details.CoupleID = CoupleId;
                BizAction.Day5Details.CoupleUnitID = CoupleUnitId;
                BizAction.Day5Details.LabDaySummary = new clsLabDaysSummaryVO();
                BizAction.Day5Details.LabDaySummary.Impressions = Imp;
                BizAction.Day5Details.LabDaySummary.ID = SummaryID;


                if (cmbEmbryologist.SelectedItem != null)
                    BizAction.Day5Details.EmbryologistID = ((MasterListItem)cmbEmbryologist.SelectedItem).ID;

                if (cmbAssistantEmbryologist.SelectedItem != null)
                    BizAction.Day5Details.AssEmbryologistID = ((MasterListItem)cmbAssistantEmbryologist.SelectedItem).ID;

                if (cmbAnesthetist.SelectedItem != null)
                    BizAction.Day5Details.AnesthetistID = ((MasterListItem)cmbAnesthetist.SelectedItem).ID;

                if (cmbAssistantAnesthetist.SelectedItem != null)
                    BizAction.Day5Details.AssAnesthetistID = ((MasterListItem)cmbAssistantAnesthetist.SelectedItem).ID;

                if (cmbSourceNeedle.SelectedItem != null)
                    BizAction.Day5Details.SourceNeedleID = ((MasterListItem)cmbSourceNeedle.SelectedItem).ID;

                BizAction.Day5Details.FertilizationAssesmentDetails = FertilizationList.ToList();

                BizAction.Day5Details.SemenDetails = SemenDetails;
                BizAction.Day5Details.SemenDetails.MethodOfSpermPreparation = MethodOfSpermPreparation;
                BizAction.Day5Details.SemenDetails.SourceOfSemen = SourceOfSemen;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("", "Day 5 details Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();

                        //((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                        //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                        //((IApplicationConfiguration)App.Current).FillMenu("IVF Lab Work");
                        LabDaysSummary Lab = new LabDaysSummary();
                        Lab.IsPatientExist = true;
                        ((IApplicationConfiguration)App.Current).OpenMainContent(Lab);
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

        private void Day4Scoring_Click(object sender, RoutedEventArgs e)
        {
            PreviousDayScoreList WinDay = new PreviousDayScoreList();
            WinDay.DayID = (int)(IVFLabDay.Day5);
            WinDay.CoupleID = CoupleId;
            WinDay.CoupleUnitID = CoupleUnitId;
            WinDay.Show();

        }
        
        private bool ChkValidation()
        {
            bool result = true;
            if (ProcDate.SelectedDate == null)
            {
                ProcDate.SetValidation("Please select the Procedure Date");
                ProcDate.RaiseValidationError();
                ProcDate.Focus();
                result = false;

            }
            else
                ProcDate.ClearValidationError();

            if (ProcTime.Value == null)
            {
                ProcTime.SetValidation("Please select the Procedure Time");
                ProcTime.RaiseValidationError();
                ProcTime.Focus();
                result = false;

            }
            else
                ProcTime.ClearValidationError();



            //if (cmbSourceNeedle.SelectedItem == null)
            //{
            //    cmbSourceNeedle.TextBox.SetValidation("Please select Source of Needle");
            //    cmbSourceNeedle.TextBox.RaiseValidationError();
            //    cmbSourceNeedle.Focus();
            //    result = false;

            //}
            //else if (((MasterListItem)cmbSourceNeedle.SelectedItem).ID == 0)
            //{
            //    cmbSourceNeedle.TextBox.SetValidation("Please select the Source of Needle");
            //    cmbSourceNeedle.TextBox.RaiseValidationError();
            //    cmbSourceNeedle.Focus();
            //    result = false;

            //}
            //else
            //    cmbSourceNeedle.TextBox.ClearValidationError();



            if (cmbEmbryologist.SelectedItem == null)
            {
                cmbEmbryologist.TextBox.SetValidation("Please select Embryologist");
                cmbEmbryologist.TextBox.RaiseValidationError();
                cmbEmbryologist.Focus();
                result = false;

            }
            else if (((MasterListItem)cmbEmbryologist.SelectedItem).ID == 0)
            {
                cmbEmbryologist.TextBox.SetValidation("Please select the Embryologist");
                cmbEmbryologist.TextBox.RaiseValidationError();
                cmbEmbryologist.Focus();
                result = false;

            }
            else
                cmbEmbryologist.TextBox.ClearValidationError();

            //if (cmbAnesthetist.SelectedItem == null)
            //{
            //    cmbAnesthetist.TextBox.SetValidation("Please select Anesthetist");
            //    cmbAnesthetist.TextBox.RaiseValidationError();
            //    cmbAnesthetist.Focus();
            //    result = false;

            //}
            //else if (((MasterListItem)cmbAnesthetist.SelectedItem).ID == 0)
            //{
            //    cmbAnesthetist.TextBox.SetValidation("Please select the Anesthetist");
            //    cmbAnesthetist.TextBox.RaiseValidationError();
            //    cmbAnesthetist.Focus();
            //    result = false;

            //}
            //else
            //    cmbAnesthetist.TextBox.ClearValidationError();


            if (FertilizationList.Count==0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "You can not save day 5 without Fertilization Assesment.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgW1.Show();
                result = false;
            }
            if (FertilizationList.Where(Items => Items.SelectedPlan.ID == 0).Any() == true)
            {
                MessageBoxControl.MessageBoxChildWindow msgW13 =
                                                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select Plan.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgW13.Show();
                result = false;

            }


            return result;
        }

        private void GetDay4Details()
        {
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                try
                {

                    clsGetLabDay4ForLabDay5BizActionVO BizAction = new clsGetLabDay4ForLabDay5BizActionVO();


                    BizAction.Day5Details = new List<clsFemaleLabDay5FertilizationAssesmentVO>();
                    BizAction.CoupleID = CoupleId;
                    BizAction.CoupleUnitID = CoupleUnitId;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            if (((clsGetLabDay4ForLabDay5BizActionVO)arg.Result).Day5Details != null)
                            {

                                List<clsFemaleLabDay5FertilizationAssesmentVO> ObjItem;
                                ObjItem = ((clsGetLabDay4ForLabDay5BizActionVO)arg.Result).Day5Details; ;

                                foreach (var item4 in ObjItem)
                                {
                                    item4.FerPlan = FertilizationStage;
                                    item4.Grade = GradeList;
                                    item4.Plan = PlanList;
                                    item4.BlastocytsGrade = BlastocytsGradeList;
                                    item4.Expansion = ExpansionList;
                                    item4.ICM = ICMList;
                                    item4.Trophectoderm = TrophectodermList;
                                    FertilizationList.Add(item4);
                                }
                                dgFertilizationAssesmentList.ItemsSource = null;
                                dgFertilizationAssesmentList.ItemsSource = FertilizationList;

                                cmbEmbryologist.SelectedValue = ((clsGetLabDay4ForLabDay5BizActionVO)arg.Result).EmbryologistID;
                                cmbAnesthetist.SelectedValue = ((clsGetLabDay4ForLabDay5BizActionVO)arg.Result).AnaesthetistID;
                                cmbAssistantAnesthetist.SelectedValue = ((clsGetLabDay4ForLabDay5BizActionVO)arg.Result).AssAnaesthetistID;
                                cmbAssistantEmbryologist.SelectedValue = ((clsGetLabDay4ForLabDay5BizActionVO)arg.Result).AssEmbryologistID;


                                Indicatior.Close();

                            }
                            Indicatior.Close();
                        }     
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                            msgW1.Show();
                            Indicatior.Close();
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                catch (Exception)
                {
                    throw;
                    Indicatior.Close();

                }
                finally
                {
                 
                   
                }

        }
       
        #region Upload File
        
        private void LoadFURepeaterControl()
        {

            lstFUBox = new ListBox();
            if (IsUpdate == false)
                FUSetting = ((clsFemaleLabDay5VO)this.DataContext).FUSetting = new List<FileUpload>();
            else
                FUSetting = ((clsFemaleLabDay5VO)this.DataContext).FUSetting;

            lstFUBox.DataContext = ((clsFemaleLabDay5VO)this.DataContext).FUSetting;

            if (IsUpdate == false)
            {
                FUSetting.Add(new PalashDynamics.ValueObjects.IVFPlanTherapy.FileUpload());
            }
            if (FUSetting != null)
            {

                for (int i = 0; i < FUSetting.Count; i++)
                {
                    FileUploadRepeterControlItem FUrci = new FileUploadRepeterControlItem();
                    FUrci.OnAddRemoveClick += new RoutedEventHandler(FUrci_OnAddRemoveClick);
                    FUrci.OnBrowseClick += new RoutedEventHandler(FUrci_OnBrowseClick);
                    FUrci.OnViewClick += new RoutedEventHandler(FUrci_OnViewClick);

                    FUSetting[i].Index = i;
                    FUSetting[i].Command = ((i == FUSetting.Count - 1) ? "Add" : "Remove");

                    FUrci.DataContext = FUSetting[i];
                    lstFUBox.Items.Add(FUrci);
                }
            }
            Grid.SetRow(lstFUBox, 0);
            Grid.SetColumn(lstFUBox, 0);
            GridUploadFile.Children.Add(lstFUBox);
        }

        void FUrci_OnViewClick(object sender, RoutedEventArgs e)
        {
            if (((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).FileName != null && ((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).FileName != "")
            {
                if (((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).Data != null)
                {
                    string FullFile = "Lab Day 5 " + ((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).Index + ((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).FileName;

                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + FullFile });
                            listOfReports.Add(FullFile);
                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", FullFile, ((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).Data);
                }
                else
                {

                }
            }
            else
            {
                MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This File is not uploaded. Please upload the File then click on preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                mgbx.Show();
            }
        }
        
        void FUrci_OnBrowseClick(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == true)
            {
                ((ValueObjects.IVFPlanTherapy.FileUpload)((Button)sender).DataContext).FileName = openDialog.File.Name;
                ((TextBox)((Grid)((Button)sender).Parent).FindName("FileName")).Text = openDialog.File.Name;
                try
                {
                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        ((ValueObjects.IVFPlanTherapy.FileUpload)((Button)sender).DataContext).Data = new byte[stream.Length];
                        stream.Read(((ValueObjects.IVFPlanTherapy.FileUpload)((Button)sender).DataContext).Data, 0, (int)stream.Length);

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

        void FUrci_OnAddRemoveClick(object sender, RoutedEventArgs e)
        {
            if (((HyperlinkButton)sender).TargetName.ToString() == "Remove")
            {
                FUSetting.RemoveAt(((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).Index);
            }
            if (((HyperlinkButton)sender).TargetName.ToString() == "Add")
            {
                if (FUSetting.Where(Items => Items.FileName == null).Any() == true)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW13 =
                                                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select File.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW13.Show();

                }
                else
                {
                    FUSetting.Add(new ValueObjects.IVFPlanTherapy.FileUpload());
                }
            }
            lstFUBox.Items.Clear();
            for (int i = 0; i < FUSetting.Count; i++)
            {
                FileUploadRepeterControlItem FUrci = new FileUploadRepeterControlItem();
                FUrci.OnAddRemoveClick += new RoutedEventHandler(FUrci_OnAddRemoveClick);
                FUrci.OnBrowseClick += new RoutedEventHandler(FUrci_OnBrowseClick);
                FUrci.OnViewClick += new RoutedEventHandler(FUrci_OnViewClick);


                FUSetting[i].Index = i;
                FUSetting[i].Command = ((i == FUSetting.Count - 1) ? "Add" : "Remove");

                FUrci.DataContext = FUSetting[i];
                lstFUBox.Items.Add(FUrci);
            }
        }
       
        #endregion

        #region Semen Details
        private void cmdSemenDetails_Click(object sender, RoutedEventArgs e)
        {
            SemenDetails WinSemen = new SemenDetails();
            if (IsUpdate == false)
            {
                if (MethodOfSpermPreparation > 0)
                {
                    WinSemen.Details = SemenDetails;
                    WinSemen.Details.SourceOfSemen = SourceOfSemen;
                    WinSemen.Details.MethodOfSpermPreparation = MethodOfSpermPreparation;
                }
            }
            else
            {
                WinSemen.IsUpdate = true;
                WinSemen.Details = SemenDetails;
                WinSemen.cmbSourceOfSemen.SelectedValue = SourceOfSemen;
                WinSemen.cmbMethodOfSpermpreparation.SelectedValue = MethodOfSpermPreparation;
            }
            WinSemen.OnSaveButton_Click += new RoutedEventHandler(WinSemen_OnSaveButton_Click);
            WinSemen.Show();

        }
        void WinSemen_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            SemenDetails ObjSemen = (SemenDetails)sender;
            if (ObjSemen.DialogResult == true)
            {
                if (ObjSemen.Details != null)
                {
                    SourceOfSemen = ((MasterListItem)(ObjSemen.cmbSourceOfSemen.SelectedItem)).ID;
                    MethodOfSpermPreparation = ((MasterListItem)(ObjSemen.cmbMethodOfSpermpreparation.SelectedItem)).ID;
                    SemenDetails = ObjSemen.Details;
                }
            }

        }

        #endregion

        #region Calculate Detail
        private void AddDetail_Click(object sender, RoutedEventArgs e)
        {
            Lab5Detail WinDetail = new Lab5Detail();
            foreach (var item in FertilizationList)
                {
                    if (item.ID == ((clsFemaleLabDay5FertilizationAssesmentVO)dgFertilizationAssesmentList.SelectedItem).ID)
                    {
                        if (item.Day5CalculateDetails != null)
                        {
                            WinDetail.Details = item.Day5CalculateDetails;
                        }

                    }

                }
            WinDetail.OnSaveButton_Click += new RoutedEventHandler(WinDetail_OnSaveButton_Click);
            WinDetail.Show();
        }
        void WinDetail_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {

            Lab5Detail ObjCal = (Lab5Detail)sender;
            if (ObjCal.DialogResult == true)
            {
                if (ObjCal.Details != null)
                {
                    CalcDetails = ObjCal.Details;
                    CalcDetails.FertilizationID = ((clsFemaleLabDay5FertilizationAssesmentVO)dgFertilizationAssesmentList.SelectedItem).ID;
                    ((clsFemaleLabDay5FertilizationAssesmentVO)dgFertilizationAssesmentList.SelectedItem).Day5CalculateDetails = CalcDetails;
                }
            }
        }



        #endregion

        #region Media Details

        private void MediaDetails_Click(object sender, RoutedEventArgs e)
        {
            MediaDetails Win = new MediaDetails();

            if (StoreID != 0)
            {
                Win.cmbStore.IsEnabled = false;
                Win.StoreID = StoreID;
            }
            foreach (var item in FertilizationList)
            {
                if (item.ID == ((clsFemaleLabDay5FertilizationAssesmentVO)dgFertilizationAssesmentList.SelectedItem).ID)
                {
                    if (item.MediaDetails != null && item.MediaDetails.Count > 0)
                    {
                        Win.StoreID = item.MediaDetails[0].StoreID;
                        Win.ItemList = new ObservableCollection<clsFemaleMediaDetailsVO>(item.MediaDetails);
                    }
                }
            }
           
            Win.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            Win.Show();

        }

        void Win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            MediaList.Clear();
            MediaDetails ObjWin = (MediaDetails)sender;
            if (ObjWin.DialogResult == true)
            {
                if (ObjWin.ItemList != null)
                {
                    StoreID = ObjWin.StoreID;
                    foreach (var item in ObjWin.ItemList)
                    {
                        clsFemaleMediaDetailsVO objItem = new clsFemaleMediaDetailsVO();
                        objItem.ItemID = item.ItemID;
                        objItem.ItemName = item.ItemName;
                        objItem.BatchID = item.BatchID;
                        objItem.BatchCode = item.BatchCode;
                        objItem.ExpiryDate = item.ExpiryDate;
                        objItem.StoreID = item.StoreID;
                        objItem.PH = item.PH;
                        objItem.OSM = item.OSM;
                        objItem.SelectedStatus.ID = item.SelectedStatus.ID;
                        objItem.SelectedStatus.Description = item.SelectedStatus.Description;
                        objItem.SelectedStatus = item.Status.FirstOrDefault(q => q.ID == item.SelectedStatus.ID);
                        objItem.VolumeUsed = item.VolumeUsed;
                        objItem.Date = item.Date;
                        objItem.FertilizationID = ((clsFemaleLabDay5FertilizationAssesmentVO)dgFertilizationAssesmentList.SelectedItem).ID;

                        MediaList.Add(objItem);
                        ((clsFemaleLabDay5FertilizationAssesmentVO)dgFertilizationAssesmentList.SelectedItem).MediaDetails = MediaList.ToList();

                    }

                }
            }





        }

        #endregion

        private void FetchData(long id, long unitid, long coupleid, long coupleunitid)
        {
            clsGetDay5DetailsBizActionVO BizAction = new clsGetDay5DetailsBizActionVO();
            BizAction.LabDay5 = new clsFemaleLabDay5VO();

            BizAction.ID = id;
            BizAction.UnitID = unitid;
            BizAction.CoupleID = coupleid;
            BizAction.CoupleUnitID = coupleunitid;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {

                    if (((clsGetDay5DetailsBizActionVO)arg.Result).LabDay5 != null)
                    {
                        clsGetDay5DetailsBizActionVO LabDay5Details = new clsGetDay5DetailsBizActionVO();
                        LabDay5Details = (clsGetDay5DetailsBizActionVO)arg.Result;

                        this.DataContext = LabDay5Details.LabDay5;


                        cmbEmbryologist.SelectedValue = LabDay5Details.LabDay5.EmbryologistID;
                        cmbAssistantEmbryologist.SelectedValue = LabDay5Details.LabDay5.AssEmbryologistID;
                        cmbAnesthetist.SelectedValue = LabDay5Details.LabDay5.AnesthetistID;
                        cmbAssistantAnesthetist.SelectedValue = LabDay5Details.LabDay5.AssAnesthetistID;
                        cmbSourceNeedle.SelectedValue = LabDay5Details.LabDay5.SourceNeedleID;


                        if (LabDay5Details.LabDay5.FertilizationAssesmentDetails != null)
                        {
                            List<clsFemaleLabDay5FertilizationAssesmentVO> Obj;
                            Obj = ((clsGetDay5DetailsBizActionVO)arg.Result).LabDay5.FertilizationAssesmentDetails;
                            foreach (var item4 in Obj)
                            {
                                item4.FerPlan = FertilizationStage;
                                item4.Grade = GradeList;
                                item4.Plan = PlanList;
                                item4.ICM = ICMList;
                                item4.Trophectoderm = TrophectodermList;
                                item4.Expansion = ExpansionList;
                                item4.BlastocytsGrade = BlastocytsGradeList;
                                FertilizationList.Add(item4);

                                //----For Getting Summary
                                MasterListItem objUpdate = new MasterListItem();
                                objUpdate.ID = item4.ID;
                                objUpdate.Description = item4.SelectedFePlan.ID.ToString();

                                if (objUpdate.Description == "16")
                                { Oocytes = 1; }
                                else if (objUpdate.Description == "12")
                                { TwoPN = 1; }
                                else if (objUpdate.Description == "13")
                                { ThreePN = 1; }
                                else if (objUpdate.Description == "2")
                                { Only2PB = 1; }
                                else if (objUpdate.Description == "8")
                                { MI = 1; }
                                else if (objUpdate.Description == "11")
                                { MII = 1; }
                                else if (objUpdate.Description == "7")
                                { GV = 1; }
                                else if (objUpdate.Description == "14")
                                { Degenerated = 1; }
                                else if (objUpdate.Description == "15")
                                { Lost = 1; }

                                OBjList.Add(objUpdate);
                            }
                            dgFertilizationAssesmentList.ItemsSource = null;
                            dgFertilizationAssesmentList.ItemsSource = FertilizationList;



                        }

                        if (LabDay5Details.LabDay5.SemenDetails != null)
                        {
                            SemenDetails = ((clsGetDay5DetailsBizActionVO)arg.Result).LabDay5.SemenDetails;
                            SourceOfSemen = ((clsGetDay5DetailsBizActionVO)arg.Result).LabDay5.SemenDetails.SourceOfSemen;
                            MethodOfSpermPreparation = ((clsGetDay5DetailsBizActionVO)arg.Result).LabDay5.SemenDetails.MethodOfSpermPreparation;
                        }

                        LoadFURepeaterControl();
                        if (LabDay5Details.LabDay5.IsFreezed == true)
                        {
                            CmdSave.IsEnabled = false;

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

        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            ((IApplicationConfiguration)App.Current).FillMenu("IVF Lab Work");
        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((clsFemaleLabDay5FertilizationAssesmentVO)dgFertilizationAssesmentList.SelectedItem).FileName))
            {
                if (((clsFemaleLabDay5FertilizationAssesmentVO)dgFertilizationAssesmentList.SelectedItem).FileContents != null)
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + ((clsFemaleLabDay5FertilizationAssesmentVO)dgFertilizationAssesmentList.SelectedItem).FileName });
                            listOfReports.Add(((clsFemaleLabDay5FertilizationAssesmentVO)dgFertilizationAssesmentList.SelectedItem).FileName);
                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", ((clsFemaleLabDay5FertilizationAssesmentVO)dgFertilizationAssesmentList.SelectedItem).FileName, ((clsFemaleLabDay5FertilizationAssesmentVO)dgFertilizationAssesmentList.SelectedItem).FileContents);
                }
            }
        }

        private void cmdBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == true)
            {

                ((clsFemaleLabDay5FertilizationAssesmentVO)dgFertilizationAssesmentList.SelectedItem).FileName = openDialog.File.Name;
                try
                {
                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        ((clsFemaleLabDay5FertilizationAssesmentVO)dgFertilizationAssesmentList.SelectedItem).FileContents = new byte[stream.Length];
                        stream.Read(((clsFemaleLabDay5FertilizationAssesmentVO)dgFertilizationAssesmentList.SelectedItem).FileContents, 0, (int)stream.Length);

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


        #region  Calculate Summary
        long Oocytes = 0;
        long TwoPN = 0;
        long ThreePN = 0;
        long Only2PB = 0;
        long MI = 0;
        long MII = 0;
        long GV = 0;
        long Degenerated = 0;
        long Lost = 0;
        List<MasterListItem> OBjList = new List<MasterListItem>();

        private void cmbFertilizationStage_LostFocus(object sender, RoutedEventArgs e)
        {

            if ((clsFemaleLabDay5FertilizationAssesmentVO)dgFertilizationAssesmentList.SelectedItem != null)
            {
                if (((clsFemaleLabDay5FertilizationAssesmentVO)dgFertilizationAssesmentList.SelectedItem).SelectedFePlan != null && ((clsFemaleLabDay5FertilizationAssesmentVO)dgFertilizationAssesmentList.SelectedItem).SelectedFePlan.ID != 0)
                {
                    if (((clsFemaleLabDay5FertilizationAssesmentVO)dgFertilizationAssesmentList.SelectedItem).SelectedFePlan.Description != null)
                    {
                        if (OBjList.Count > 0)
                        {
                            RemoveFromSummary();
                        }
                    }
                    CalculateSummary();

                }
            }
        }

        private void CalculateSummary()
        {

            MasterListItem Fertilization = ((clsFemaleLabDay5FertilizationAssesmentVO)dgFertilizationAssesmentList.SelectedItem).SelectedFePlan;

            MasterListItem OBjCalc = new MasterListItem();

            OBjCalc.ID = ((clsFemaleLabDay5FertilizationAssesmentVO)dgFertilizationAssesmentList.SelectedItem).ID;
            OBjCalc.Description = ((clsFemaleLabDay5FertilizationAssesmentVO)dgFertilizationAssesmentList.SelectedItem).SelectedFePlan.ID.ToString();

            if (Fertilization.ID == 16)
            {

                Oocytes = Oocytes + 1;
            }

            if (Fertilization.ID == 12)
            {
                TwoPN = TwoPN + 1;
            }
            if (Fertilization.ID == 13)
            {
                ThreePN = ThreePN + 1;

            }
            if (Fertilization.ID == 2)
            {
                Only2PB = Only2PB + 1;

            }
            if (Fertilization.ID == 8)
            {
                MI = MI + 1;

            }
            if (Fertilization.ID == 11)
            {
                MII = MII + 1;

            }
            if (Fertilization.ID == 7)
            {
                GV = GV + 1;

            }
            if (Fertilization.ID == 14)
            {
                Degenerated = Degenerated + 1;

            }
            if (Fertilization.ID == 15)
            {
                Lost = Lost + 1;

            }

            OBjList.Add(OBjCalc);

            txtTotOocytes.Text = Oocytes.ToString();
            txtTot2PN.Text = TwoPN.ToString();
            txtTot3PN.Text = ThreePN.ToString();
            txtTot2PB.Text = Only2PB.ToString();
            txtTotMI.Text = MI.ToString();
            txtTotMII.Text = MII.ToString();
            txtTotGV.Text = GV.ToString();
            txtTotLost.Text = Lost.ToString();
            txtTotDegenerated.Text = Degenerated.ToString();



        }

        private void RemoveFromSummary()
        {
            bool Flag = false;
            MasterListItem Select = new MasterListItem();
            foreach (var item in OBjList)
            {
                if (item.ID == ((clsFemaleLabDay5FertilizationAssesmentVO)dgFertilizationAssesmentList.SelectedItem).ID)
                {
                    if (item.Description != ((clsFemaleLabDay5FertilizationAssesmentVO)dgFertilizationAssesmentList.SelectedItem).SelectedFePlan.ID.ToString())
                    {

                        if (item.Description == "16")
                        { Oocytes = Oocytes - 1; }
                        else if (item.Description == "12")
                        { TwoPN = TwoPN - 1; }
                        else if (item.Description == "13")
                        { ThreePN = ThreePN - 1; }
                        else if (item.Description == "2")
                        { Only2PB = Only2PB - 1; }
                        else if (item.Description == "8")
                        { MI = MI - 1; }
                        else if (item.Description == "11")
                        { MII = MII - 1; }
                        else if (item.Description == "7")
                        { GV = GV - 1; }
                        else if (item.Description == "14")
                        { Degenerated = Degenerated - 1; }
                        else if (item.Description == "15")
                        { Lost = Lost - 1; }

                        Flag = true;
                        Select = item;
                    }
                }
            }
            if (Flag == true)
            { OBjList.Remove(Select); }
        }
        #endregion

        private void HyperlinkButton_Click_Male(object sender, RoutedEventArgs e)
        {
            if (MaleAlert.Text.Trim().Length > 0)
            {
                frmAttention PatientAlert = new frmAttention();
                PatientAlert.AlertsExpanded = CoupleDetails.MalePatient.Alerts;
                PatientAlert.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Attention not entered.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

            }
        }

        private void HyperlinkButton_Click_Female(object sender, RoutedEventArgs e)
        {
            if (FemaleAlert.Text.Trim().Length > 0)
            {
                frmAttention PatientAlert = new frmAttention();
                PatientAlert.AlertsExpanded = CoupleDetails.FemalePatient.Alerts;
                PatientAlert.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Attention not entered.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

            }
        }

    }
}
