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
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Patient;
using CIMS;

namespace PalashDynamics.IVF.TherpyExecution
{
    public partial class ChangePartner : ChildWindow
    {
        #region  Variables
        public event RoutedEventHandler OnSaveButton_Click;
        #endregion

        #region Constructor
        public ChangePartner()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ChangePartner_Loaded);
        }
        #endregion

        #region Loaded Event
        void ChangePartner_Loaded(object sender, RoutedEventArgs e)
        {
            fillAllCoupleDetails();
        }
        #endregion

        #region Properties
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

        private List<clsCoupleVO> _AllCoupleDetails = new List<clsCoupleVO>();
        public List<clsCoupleVO> AllCoupleDetails
        {
            get
            {
                return _AllCoupleDetails;
            }
            set
            {
                _AllCoupleDetails = value;
            }
        }
        #endregion

        #region Fill All Couple Details
        private void fillAllCoupleDetails()
        {
            try
            {
                clsGetCoupleDetailsBizActionVO BizAction = new clsGetCoupleDetailsBizActionVO();
                BizAction.PatientID = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
                BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
                BizAction.IsAllCouple = true;
                BizAction.CoupleDetails = new clsCoupleVO();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        //BizAction.CoupleDetails.MalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient;
                        //BizAction.CoupleDetails.FemalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient;
                        //AllCoupleDetails = new List<clsCoupleVO>();
                        AllCoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).AllCoupleDetails;
                        if (((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).Gender.Equals("Male"))
                        {
                            List<clsPatientGeneralVO> Partner = new List<clsPatientGeneralVO>();
                            for (int i = 0; i < AllCoupleDetails.Count; i++)
                            {
                                Partner.Add(AllCoupleDetails[i].FemalePatient);
                            }
                            dgChangeCouple.ItemsSource = Partner;
                        }
                        else
                        {
                            dgChangeCouple.ItemsSource = BizAction.AllCoupleDetails;
                            List<clsPatientGeneralVO> Partner = new List<clsPatientGeneralVO>();
                            for (int i = 0; i < AllCoupleDetails.Count; i++)
                            {
                                Partner.Add(AllCoupleDetails[i].MalePatient);
                            }
                            dgChangeCouple.ItemsSource = Partner;

                        }
                       //dgChangeCouple.ItemSource=BizAction.AllCoupleDetails
                    }


                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
               
            }
        }
        #endregion

        #region Save/Cancel
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            if (OnSaveButton_Click != null)
                OnSaveButton_Click(this, new RoutedEventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        #endregion

        #region Grid Selection Changed Event
        private void dgChangeCouple_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgChangeCouple.SelectedItem != null)
            {
                if (((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).Gender.Equals("Male"))
                {
                    CoupleDetails = AllCoupleDetails.FirstOrDefault(p => p.FemalePatient.PatientID == ((clsPatientGeneralVO)dgChangeCouple.SelectedItem).PatientID);
                }
                else
                {
                    CoupleDetails = AllCoupleDetails.FirstOrDefault(p => p.MalePatient.PatientID == ((clsPatientGeneralVO)dgChangeCouple.SelectedItem).PatientID);
                }
            }
        }
        #endregion
    }
}

