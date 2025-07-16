using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CIMS;
//using PalashDynamics.UserControls;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects;
using System.Windows.Media.Imaging;
using System.IO;

namespace PalashDynamics.SearchResultLists
{
    public partial class DisplayIPDPatientDetails : UserControl
    {
        #region Data Members
        //public WaitIndicator Indicator;
        public PagedSortableCollectionView<clsPatientConsoleHeaderVO> MasterList { get; private set; }
        public string msgText = string.Empty;
        public Boolean IsLoaded { get; set; }
        public RoutedEventHandler EncounterList_Selectionchanged;
        public string Allergies;
        #endregion

        public DisplayIPDPatientDetails()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(DisplayIPDPatientDetails_Loaded);
            MasterList = new PagedSortableCollectionView<clsPatientConsoleHeaderVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 5;
        }

        void DisplayIPDPatientDetails_Loaded(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded)
            {
                //if (((IApplicationConfiguration)App.Current).SelectedPatient.Photo != null)
                //{
                //    byte[] imageBytes = ((IApplicationConfiguration)App.Current).SelectedPatient.Photo;
                //    BitmapImage img = new BitmapImage();
                //    img.SetSource(new MemoryStream(imageBytes, false));
                //    imgPhoto.Source = img;
                //    imgPhoto.Visibility = System.Windows.Visibility.Visible;
                //}
                GetEMRAdmVisitByPatientID();
                IsLoaded = true;
            }
        }
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetEMRAdmVisitByPatientID();
        }
        public int DataListPageSize
        {
            get
            {
                return MasterList.PageSize;
            }
            set
            {
                if (value == MasterList.PageSize) return;
                MasterList.PageSize = value;
            }
        }

        #region Get Patient Visit And admission Details added by santosh Patil 01/11/2012
        private void GetEMRAdmVisitByPatientID()
        {
            clsGetEMRAdmVisitListByPatientIDBizActionVO BizAction = new clsGetEMRAdmVisitListByPatientIDBizActionVO();
            BizAction.EMRList = new List<clsPatientConsoleHeaderVO>();
            BizAction.ID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientId;
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId;
            BizAction.ISOPDIPD = true;
            //Indicator = new WaitIndicator();
            //Indicator.Show();
            BizAction.IsPagingEnabled = true;
            BizAction.MaximumRows = MasterList.PageSize;
            BizAction.StartIndex = MasterList.PageIndex * MasterList.PageSize;
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        BizAction.EMRList = (((clsGetEMRAdmVisitListByPatientIDBizActionVO)args.Result).EMRList);
                        BizAction.PatientDetails = ((clsGetEMRAdmVisitListByPatientIDBizActionVO)args.Result).PatientDetails;

                        if (BizAction.PatientDetails.Gender.ToUpper() == "MALE")
                        {
                            Patient.DataContext = BizAction.PatientDetails;
                            //if (BizAction.PatientDetails.Photo != null)
                            //{
                            //    //WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);
                            //    //bmp.FromByteArray(BizAction.PatientDetails.Photo);
                            //    //imgPhoto.Source = bmp;

                            //    byte[] imageBytes = BizAction.PatientDetails.Photo;
                            //    BitmapImage img = new BitmapImage();
                            //    img.SetSource(new MemoryStream(imageBytes, false));
                            //    imgPhoto.Source = img;
                     
                            //}
                            //else
                            //    imgPhoto.Source = new BitmapImage(new Uri("./Icons/MAle.png", UriKind.Relative));
                            if (BizAction.PatientDetails.ImageName != null && BizAction.PatientDetails.ImageName.Length > 0)
                            {
                                imgPhoto.Source = new BitmapImage(new Uri(BizAction.PatientDetails.ImageName, UriKind.Absolute));
                            }
                            else if (BizAction.PatientDetails.Photo != null)
                            {
                                byte[] imageBytes = BizAction.PatientDetails.Photo;
                                BitmapImage img = new BitmapImage();
                                img.SetSource(new MemoryStream(imageBytes, false));
                                imgPhoto.Source = img;
                            }
                            else
                            {
                                imgPhoto.Source = new BitmapImage(new Uri("./Icons/MAle.png", UriKind.Relative));
                            }
                        }
                        else
                        {
                            Patient.DataContext = BizAction.PatientDetails;
                            //if (BizAction.PatientDetails.Photo != null)
                            //{
                            //    //WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);
                            //    //bmp.FromByteArray(BizAction.PatientDetails.Photo);
                            //    //imgPhoto.Source = bmp;
                            //    byte[] imageBytes = BizAction.PatientDetails.Photo;
                            //    BitmapImage img = new BitmapImage();
                            //    img.SetSource(new MemoryStream(imageBytes, false));
                            //    imgPhoto.Source = img;
                            //}
                            //else
                            //    imgPhoto.Source = new BitmapImage(new Uri("./Icons/images1.jpg", UriKind.Relative));

                            if (BizAction.PatientDetails.ImageName != null && BizAction.PatientDetails.ImageName.Length > 0)
                            {
                                imgPhoto.Source = new BitmapImage(new Uri(BizAction.PatientDetails.ImageName, UriKind.Absolute));
                            }
                            else if (BizAction.PatientDetails.Photo != null)
                            {
                                byte[] imageBytes = BizAction.PatientDetails.Photo;
                                BitmapImage img = new BitmapImage();
                                img.SetSource(new MemoryStream(imageBytes, false));
                                imgPhoto.Source = img;
                            }
                            else
                                imgPhoto.Source = new BitmapImage(new Uri("./Icons/images1.jpg", UriKind.Relative));
                        }
                        if (BizAction.EMRList != null && BizAction.EMRList.Count > 0)
                        {
                            MasterList.Clear();
                            MasterList.TotalItemCount = (int)(((clsGetEMRAdmVisitListByPatientIDBizActionVO)args.Result).TotalRows);
                            foreach (clsPatientConsoleHeaderVO item in BizAction.EMRList)
                            {
                                item.Allergies = BizAction.PatientDetails.Allergies.Replace("\r", "");
                                MasterList.Add(item);
                            }
                            pgrEncounter.Source = null;
                            pgrEncounter.Source = MasterList;
                            dgEncounterList.ItemsSource = null;
                            dgEncounterList.ItemsSource = MasterList;
                        }
                    }
                    //Indicator.Close();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        private void dgEncounterList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgEncounterList.SelectedIndex > -1)
            {
                EncounterList_Selectionchanged(sender, e);
            }
        }
    }
}