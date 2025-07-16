using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Patient;
using System.ComponentModel;
using System.Windows.Browser;
using PalashDynamics;
using CIMS;
using System.Collections;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects.Master.DoctorMaster;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.CRM;
using System;
using System.Collections.Generic;


namespace PalashDynamics.MIS.CRM
{
    public partial class PatientPRO : UserControl
    {

            #region Paging

        public PagedSortableCollectionView<clsPatientGeneralVO> DataList { get; private set; }

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

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {

            FetchData();

        }



        #endregion
       
        public PatientPRO()
        {
             InitializeComponent();
             DataList = new PagedSortableCollectionView<clsPatientGeneralVO>();
             DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
             DataListPageSize = 15;
             FillRefDoctor();
          
        }

        public PatientPRO(DateTime? FromDate, DateTime? ToDate)
        {
            InitializeComponent();
          
        }


        private void FillRefDoctor()
        {
            clsGetRefernceDoctorBizActionVO BizAction = new clsGetRefernceDoctorBizActionVO();
            BizAction.ComboList = new List<clsComboMasterBizActionVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
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

                        cmbRefDoctor.ItemsSource = null;
                        cmbRefDoctor.ItemsSource = objList;
                        cmbRefDoctor.SelectedItem = objList[0];

                        if (this.DataContext != null)
                        {
                            cmbRefDoctor.SelectedValue = ((clsVisitVO)this.DataContext).ReferredDoctorID;

                        }
                    }
                }


            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        private void PatientSearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (dtpVisitToDate.SelectedDate != null && dtpVisitFromDate.SelectedDate != null)
            {
                if (dtpVisitToDate.SelectedDate.Value.Date.Date < dtpVisitFromDate.SelectedDate.Value.Date.Date)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 = new MessageBoxControl.MessageBoxChildWindow("", "To date can not be less than from date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();

                }
                else
                {
                    FetchData();

                }
            }
            else
            {
                FetchData();
            }
         }
     

        public void FetchData()
        {
            clsGetPROPatientListBizActionVO BizAction = new clsGetPROPatientListBizActionVO();
            if (dtpVisitFromDate.SelectedDate != null)
            {
                BizAction.VisitFromDate = dtpVisitFromDate.SelectedDate.Value.Date.Date;
            }

            if (dtpVisitToDate.SelectedDate != null)
            {
                BizAction.VisitToDate = dtpVisitToDate.SelectedDate.Value.Date.Date;
            }

            if (cmbRefDoctor.SelectedItem != null)
            {
                BizAction.ReferredDoctorId = ((MasterListItem)cmbRefDoctor.SelectedItem).ID;
            }
           
              BizAction.InputPagingEnabled = true;
              BizAction.InputStartRowIndex = DataList.PageIndex * DataList.PageSize;
              BizAction.InputMaximumRows = DataList.PageSize;
              
             Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetPROPatientListBizActionVO)arg.Result).PatientList != null)
                    {
                        clsGetPROPatientListBizActionVO result = arg.Result as clsGetPROPatientListBizActionVO;
                       
                        DataList.TotalItemCount = result.OutputTotalRows;
                        if(result.PatientList !=null)
                        {
                            DataList.Clear();
                      
                            foreach (var item in result.PatientList)
                            {
                                DataList.Add(item);
                            }

                        }
                        dgPatient.ItemsSource = null;
                        dgPatient.ItemsSource = DataList;
                        peopleDataPager.Source = DataList;
                       

                    }
                }

                else
                {

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                         new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }


            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
}

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
          

        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = (clsPatientGeneralVO)dgPatient.SelectedItem;
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null &&((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientID > 0)
            {
                fillCoupleDetails();
            }
        }

        string textBefore = null;
      
        private void txtAutocompleteNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((AutoCompleteBox)sender).Text;
        }

        private void txtAutocompleteNumber_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((AutoCompleteBox)sender).Text))
            {
                if (!((AutoCompleteBox)sender).Text.IsNumberValid())
                {
                    ((AutoCompleteBox)sender).Text = textBefore;
                 
               
                }
            }
        }

        private void SearchButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        #region Fill Couple Details
        public void fillCoupleDetails()
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
                    ((IApplicationConfiguration)App.Current).SelectedCoupleDetails = new clsCoupleVO();
                    ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient = new clsPatientGeneralVO();
                    ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient = new clsPatientGeneralVO();

                    ((IApplicationConfiguration)App.Current).SelectedCoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
                    if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId == 0)
                    {
                        //MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //     new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Create Therapy, Therapy is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //msgW1.Show();

                        //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    }
                    else
                    {
                        GetHeightAndWeight(((IApplicationConfiguration)App.Current).SelectedCoupleDetails);
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
                        ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.Height = BizAction.CoupleDetails.FemalePatient.Height;
                        ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.Weight = BizAction.CoupleDetails.FemalePatient.Weight;
                        ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.BMI = BizAction.CoupleDetails.FemalePatient.BMI;

                         ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient.Height = BizAction.CoupleDetails.MalePatient.Height;
                         ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient.Weight = BizAction.CoupleDetails.MalePatient.Weight;
                         ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient.BMI = BizAction.CoupleDetails.MalePatient.BMI;
                    }

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }
        #endregion

        private void chkRadioButton_Click(object sender, RoutedEventArgs e)
        {

         
           //if(chkVisit.IsChecked.HasValue) ((PatientSearchViewModel)this.DataContext).BizActionObject.VisitWise = chkVisit.IsChecked.Value;
                       


        }

        private void RemarkView_Click(object sender, RoutedEventArgs e)
        {
            //frmRemarkPRO PRO = new frmRemarkPRO();
            
            //PRO.Show();
            //PRO_Remarks = PRO.Remarks;

            frmRemarkPRO PRO = new frmRemarkPRO();          
            PRO.OnSaveButton_Click+=new RoutedEventHandler(PRO_OnSaveButton_Click);
            PRO.Show();

        }
        public void PRO_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((frmRemarkPRO)sender).DialogResult == true)
            {
                clsAddPROPatientBizActionVO BizAction = new clsAddPROPatientBizActionVO();
                BizAction.GeneralDetails = new clsPatientGeneralVO();

                BizAction.GeneralDetails = ((clsPatientGeneralVO)dgPatient.SelectedItem);
                if (cmbRefDoctor.SelectedItem != null)
                {
                    BizAction.ReferredDoctorId = ((MasterListItem)cmbRefDoctor.SelectedItem).ID;
                }
                BizAction.GeneralDetails.PRORemark = ((frmRemarkPRO)sender).txtAlerts.Text;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

           

                client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Remarks Added Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            FetchData();
                        }
                    };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dtpVisitFromDate.SelectedDate = DateTime.Now.Date;
            dtpVisitToDate.SelectedDate = DateTime.Now.Date;
        }
        }
     
    }

