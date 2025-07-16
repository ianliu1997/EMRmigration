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
using PalashDynamics.ValueObjects.Master.DoctorMaster;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.UserControls;
using MessageBoxControl;

namespace PalashDynamics.Forms.PatientView
{
    public partial class DoctorList : ChildWindow
    {
        public string FemaleName;
        public string MaleName;
        public long GenderID;      
        public long FemaleAge;
        public long MaleAge;
        public string MRNO;
        public string SelectedPatientName;
        public long SelectedAge;
        public long PatientTypeID;

        public DoctorList()
        {
            InitializeComponent();
            FillDoctorCombo();
        }
       

        private void FillDoctorCombo()
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            BizAction.IsNonReferralDoctor = true;
            BizAction.ReferralID = 4;
            BizAction.UnitId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
            BizAction.DepartmentId = 0;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                    cmbReferralDoctor.ItemsSource = null;
                    cmbReferralDoctor.ItemsSource = objList;
                    cmbReferralDoctor.SelectedItem = objList[0];
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

     

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void CmdPrintBarcode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BarCodePrintValidation())
                {
                    BarCodeDoctor win = new BarCodeDoctor();
                    string MRNO1 = "";                 
                    string FemaleName1 = "";
                    string MaleName1 = "";
                    string DoctorName1 = "";                                               
                    string FemaleAge1 = ""; 
                    string MaleAge1 = "";

                    if (MRNO != null)
                    {
                        MRNO1 = MRNO;
                    }

                    win.MyMRNOText.Text = MRNO;
                    win.MyMRNOText1.Text = MRNO;
                    win.MyMRNOText2.Text = MRNO;
                    win.MyMRNOText3.Text = MRNO;
                    win.MyMRNOText4.Text = MRNO;
                    win.MyMRNOText5.Text = MRNO;
                    win.MyMRNOText6.Text = MRNO;
                    win.MyMRNOText7.Text = MRNO;
                    win.MyMRNOText8.Text = MRNO;

                    win.MyMRNOText9.Text = MRNO;
                    win.MyMRNOText10.Text = MRNO;
                    win.MyMRNOText11.Text = MRNO;
                    win.MyMRNOText12.Text = MRNO;
                    win.MyMRNOText13.Text = MRNO;
                    win.MyMRNOText14.Text = MRNO;
                    win.MyMRNOText15.Text = MRNO;
                    win.MyMRNOText16.Text = MRNO;
                    win.MyMRNOText17.Text = MRNO;
                    win.MyMRNOText18.Text = MRNO;
                    win.MyMRNOText19.Text = MRNO;
                    win.MyMRNOText20.Text = MRNO;
                    win.MyMRNOText21.Text = MRNO;
                    win.MyMRNOText22.Text = MRNO;
                    win.MyMRNOText23.Text = MRNO;
                    win.MyMRNOText24.Text = MRNO;
                    win.MyMRNOText25.Text = MRNO;
                    win.MyMRNOText26.Text = MRNO;


                    if (FemaleName.Trim() == "" && GenderID == 2)
                    {
                        FemaleName1 = SelectedPatientName.Substring(4);                       
                    }
                    else if (GenderID == 2)
                    {
                        FemaleName1 = FemaleName;
                    }
                    else if (PatientTypeID == 7)
                    {
                        FemaleName1 = FemaleName;
                    }

                    win.MyFemalNmText.Text = FemaleName1;
                    win.MyFemalNmText1.Text = FemaleName1;
                    win.MyFemalNmText2.Text = FemaleName1;
                    win.MyFemalNmText3.Text = FemaleName1;
                    win.MyFemalNmText4.Text = FemaleName1;
                    win.MyFemalNmText5.Text = FemaleName1;
                    win.MyFemalNmText6.Text = FemaleName1;
                    win.MyFemalNmText7.Text = FemaleName1;
                    win.MyFemalNmText8.Text = FemaleName1;

                    win.MyFemalNmText9.Text = FemaleName1;
                    win.MyFemalNmText10.Text = FemaleName1;
                    win.MyFemalNmText11.Text = FemaleName1;
                    win.MyFemalNmText12.Text = FemaleName1;
                    win.MyFemalNmText13.Text = FemaleName1;
                    win.MyFemalNmText14.Text = FemaleName1;
                    win.MyFemalNmText15.Text = FemaleName1;
                    win.MyFemalNmText16.Text = FemaleName1;
                    win.MyFemalNmText17.Text = FemaleName1;
                    win.MyFemalNmText18.Text = FemaleName1;
                    win.MyFemalNmText19.Text = FemaleName1;
                    win.MyFemalNmText20.Text = FemaleName1;
                    win.MyFemalNmText21.Text = FemaleName1;
                    win.MyFemalNmText22.Text = FemaleName1;
                    win.MyFemalNmText23.Text = FemaleName1;
                    win.MyFemalNmText24.Text = FemaleName1;
                    win.MyFemalNmText25.Text = FemaleName1;
                    win.MyFemalNmText26.Text = FemaleName1;

                    if (FemaleAge <= 0 && GenderID == 2)
                    {
                        FemaleAge1 = Convert.ToString(SelectedAge + "/" + "F");
                        //FemaleAge1 = Convert.ToString(FemaleAge + "/" + "F");
                    }
                    else if (GenderID == 2)
                    {
                        FemaleAge1 = Convert.ToString(FemaleAge + "/" + "F");
                        //FemaleAge1 = Convert.ToString(SelectedAge + "/" + "F");
                    }
                    else if (PatientTypeID == 7)
                    {
                        FemaleAge1 = Convert.ToString(FemaleAge + "/" + "F");
                    }
                    win.MyFAgeText.Text = Convert.ToString(FemaleAge1);
                    win.MyFAgeText1.Text = Convert.ToString(FemaleAge1);
                    win.MyFAgeText2.Text = Convert.ToString(FemaleAge1);
                    win.MyFAgeText3.Text = Convert.ToString(FemaleAge1);
                    win.MyFAgeText4.Text = Convert.ToString(FemaleAge1);
                    win.MyFAgeText5.Text = Convert.ToString(FemaleAge1);
                    win.MyFAgeText6.Text = Convert.ToString(FemaleAge1);
                    win.MyFAgeText7.Text = Convert.ToString(FemaleAge1);
                    win.MyFAgeText8.Text = Convert.ToString(FemaleAge1);

                    win.MyFAgeText9.Text = Convert.ToString(FemaleAge1);
                    win.MyFAgeText10.Text = Convert.ToString(FemaleAge1);
                    win.MyFAgeText11.Text = Convert.ToString(FemaleAge1);
                    win.MyFAgeText12.Text = Convert.ToString(FemaleAge1);
                    win.MyFAgeText13.Text = Convert.ToString(FemaleAge1);
                    win.MyFAgeText14.Text = Convert.ToString(FemaleAge1);
                    win.MyFAgeText15.Text = Convert.ToString(FemaleAge1);
                    win.MyFAgeText16.Text = Convert.ToString(FemaleAge1);
                    win.MyFAgeText17.Text = Convert.ToString(FemaleAge1);
                    win.MyFAgeText18.Text = Convert.ToString(FemaleAge1);
                    win.MyFAgeText19.Text = Convert.ToString(FemaleAge1);
                    win.MyFAgeText20.Text = Convert.ToString(FemaleAge1);
                    win.MyFAgeText21.Text = Convert.ToString(FemaleAge1);
                    win.MyFAgeText22.Text = Convert.ToString(FemaleAge1);
                    win.MyFAgeText23.Text = Convert.ToString(FemaleAge1);
                    win.MyFAgeText24.Text = Convert.ToString(FemaleAge1);
                    win.MyFAgeText25.Text = Convert.ToString(FemaleAge1);
                    win.MyFAgeText26.Text = Convert.ToString(FemaleAge1);

                //------------------------------------------------------------//
                    if (MaleName.Trim() == "" && GenderID == 1)
                    {
                        MaleName1 = SelectedPatientName.Substring(4); 
                    }
                    else if (GenderID == 1)
                    {                       
                        MaleName1 = MaleName;
                    }
                    else if (PatientTypeID == 7)
                    {
                        MaleName1 = MaleName;
                    }

                    win.MyMaleNmText.Text = MaleName1;
                    win.MyMaleNmText1.Text = MaleName1;
                    win.MyMaleNmText2.Text = MaleName1;
                    win.MyMaleNmText3.Text = MaleName1;
                    win.MyMaleNmText4.Text = MaleName1;
                    win.MyMaleNmText5.Text = MaleName1;
                    win.MyMaleNmText6.Text = MaleName1;
                    win.MyMaleNmText7.Text = MaleName1;
                    win.MyMaleNmText8.Text = MaleName1;

                    win.MyMaleNmText9.Text = MaleName1;
                    win.MyMaleNmText10.Text = MaleName1;
                    win.MyMaleNmText11.Text = MaleName1;
                    win.MyMaleNmText12.Text = MaleName1;
                    win.MyMaleNmText13.Text = MaleName1;
                    win.MyMaleNmText14.Text = MaleName1;
                    win.MyMaleNmText15.Text = MaleName1;
                    win.MyMaleNmText16.Text = MaleName1;
                    win.MyMaleNmText17.Text = MaleName1;
                    win.MyMaleNmText18.Text = MaleName1;
                    win.MyMaleNmText19.Text = MaleName1;
                    win.MyMaleNmText20.Text = MaleName1;
                    win.MyMaleNmText21.Text = MaleName1;
                    win.MyMaleNmText22.Text = MaleName1;
                    win.MyMaleNmText23.Text = MaleName1;
                    win.MyMaleNmText24.Text = MaleName1;
                    win.MyMaleNmText25.Text = MaleName1;
                    win.MyMaleNmText26.Text = MaleName1;
                    if (MaleAge <= 0 && GenderID == 1)
                    {
                        MaleAge1 = Convert.ToString(SelectedAge + "/" + "M");
                        //MaleAge1 = Convert.ToString(MaleAge + "/" + "M");
                    }
                    else if (GenderID == 1)
                    {
                        MaleAge1 = Convert.ToString(MaleAge + "/" + "M");
                        //MaleAge1 = Convert.ToString(SelectedAge + "/" + "M");
                    }
                    else if (PatientTypeID == 7)
                    {
                        MaleAge1 = Convert.ToString(MaleAge + "/" + "M");
                    }
                    win.MyMAgeText.Text = Convert.ToString(MaleAge1);
                    win.MyMAgeText1.Text = Convert.ToString(MaleAge1);
                    win.MyMAgeText2.Text = Convert.ToString(MaleAge1);
                    win.MyMAgeText3.Text = Convert.ToString(MaleAge1);
                    win.MyMAgeText4.Text = Convert.ToString(MaleAge1);
                    win.MyMAgeText5.Text = Convert.ToString(MaleAge1);
                    win.MyMAgeText6.Text = Convert.ToString(MaleAge1);
                    win.MyMAgeText7.Text = Convert.ToString(MaleAge1);
                    win.MyMAgeText8.Text = Convert.ToString(MaleAge1);

                    win.MyMAgeText9.Text = Convert.ToString(MaleAge1);
                    win.MyMAgeText10.Text = Convert.ToString(MaleAge1);
                    win.MyMAgeText11.Text = Convert.ToString(MaleAge1);
                    win.MyMAgeText12.Text = Convert.ToString(MaleAge1);
                    win.MyMAgeText13.Text = Convert.ToString(MaleAge1);
                    win.MyMAgeText14.Text = Convert.ToString(MaleAge1);
                    win.MyMAgeText15.Text = Convert.ToString(MaleAge1);
                    win.MyMAgeText16.Text = Convert.ToString(MaleAge1);
                    win.MyMAgeText17.Text = Convert.ToString(MaleAge1);
                    win.MyMAgeText18.Text = Convert.ToString(MaleAge1);
                    win.MyMAgeText19.Text = Convert.ToString(MaleAge1);
                    win.MyMAgeText20.Text = Convert.ToString(MaleAge1);
                    win.MyMAgeText21.Text = Convert.ToString(MaleAge1);
                    win.MyMAgeText22.Text = Convert.ToString(MaleAge1);
                    win.MyMAgeText23.Text = Convert.ToString(MaleAge1);
                    win.MyMAgeText24.Text = Convert.ToString(MaleAge1);
                    win.MyMAgeText25.Text = Convert.ToString(MaleAge1);
                    win.MyMAgeText26.Text = Convert.ToString(MaleAge1);
                    if (cmbReferralDoctor.SelectedItem != null)
                    {
                        DoctorName1 = "Dr." +" " +((MasterListItem)cmbReferralDoctor.SelectedItem).Description;
                    }

                    win.MyDoctorText.Text = DoctorName1;
                    win.MyDoctorText1.Text = DoctorName1;
                    win.MyDoctorText2.Text = DoctorName1;
                    win.MyDoctorText3.Text = DoctorName1;
                    win.MyDoctorText4.Text = DoctorName1;
                    win.MyDoctorText5.Text = DoctorName1;
                    win.MyDoctorText6.Text = DoctorName1;
                    win.MyDoctorText7.Text = DoctorName1;
                    win.MyDoctorText8.Text = DoctorName1;

                    win.MyDoctorText9.Text = DoctorName1;
                    win.MyDoctorText10.Text = DoctorName1;
                    win.MyDoctorText11.Text = DoctorName1;
                    win.MyDoctorText12.Text = DoctorName1;
                    win.MyDoctorText13.Text = DoctorName1;
                    win.MyDoctorText14.Text = DoctorName1;
                    win.MyDoctorText15.Text = DoctorName1;
                    win.MyDoctorText16.Text = DoctorName1;
                    win.MyDoctorText17.Text = DoctorName1;
                    win.MyDoctorText18.Text = DoctorName1;
                    win.MyDoctorText19.Text = DoctorName1;
                    win.MyDoctorText20.Text = DoctorName1;
                    win.MyDoctorText21.Text = DoctorName1;
                    win.MyDoctorText22.Text = DoctorName1;
                    win.MyDoctorText23.Text = DoctorName1;
                    win.MyDoctorText24.Text = DoctorName1;
                    win.MyDoctorText25.Text = DoctorName1;
                    win.MyDoctorText26.Text = DoctorName1;                    

                 

                    win.PrintData = MRNO;
                    //win.PrintItem = ((clsOpeningBalVO)dgOpeningBalanceList.SelectedItem).ItemName;
                    //win.PrintDate = "";// date;
                    //win.PrintFrom = "Opening Balance";
                    win.Show();
                }
            }
            catch (Exception EX)
            {
                throw;
            }
        }

        bool BarCodePrintValidation()
        {
            bool result = true;
          
           if(((MasterListItem)cmbReferralDoctor.SelectedItem).ID == 0)
            {
                result = false;
                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please select the Doctor.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                msgbox.Show();
            }           
            return result;
        }
    }
}

