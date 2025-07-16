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
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Media.Imaging;
using System.IO;
using PalashDynamics.ValueObjects.Inventory.MaterialConsumption;
using PalashDynamics.Collections;
using System.Xml.Linq;
using System.Windows.Resources;
using PalashDynamics.ValueObjects.IPD;
using System.Reflection;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Billing;

namespace PalashDynamics.IPD
{
    public partial class PatientAgainstMaterialConsumptionDetails : UserControl
    {
        WaitIndicator indicator;
        public string ModuleName { get; set; }
        public string PatientName { get; set; }
        public string MRNO { get; set; }
        public double TotalAmount { get; set; }
        private bool ISFreeze { get; set; }
        private bool ISDischarged { get; set; }
        public PagedSortableCollectionView<clsMaterialConsumptionVO> ConsumptionDataList { get; private set; }
        public int DataListPageSize
        {
            get
            {
                return ConsumptionDataList.PageSize;
            }
            set
            {
                if (value == ConsumptionDataList.PageSize) return;
                ConsumptionDataList.PageSize = value;
            }
        }
        public PatientAgainstMaterialConsumptionDetails()
        {
            InitializeComponent();
            indicator = new WaitIndicator();                
            ConsumptionDataList = new PagedSortableCollectionView<clsMaterialConsumptionVO>();
            ConsumptionDataList.OnRefresh += new EventHandler<RefreshEventArgs>(ConsumptionDataList_OnRefresh);
            DataListPageSize = 15;
            dpdgMaterailConsumptionList.PageSize = DataListPageSize;
            dpdgMaterailConsumptionList.Source = ConsumptionDataList;
            LoadPatientHeader();
            FillBillSearchList();
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillMatarialConsumptionList();
            LoadPatientHeader();            
        }      

        void ConsumptionDataList_OnRefresh(object sender, RefreshEventArgs e)
        {

            FillMatarialConsumptionList();
        }

        private void FillBillSearchList()
        {
            clsGetBillSearchListBizActionVO BizAction = new clsGetBillSearchListBizActionVO();
            BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            BizAction.Opd_Ipd_External_Id = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmID;
            BizAction.Opd_Ipd_External_UnitId = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmissionUnitID;
            BizAction.Opd_Ipd_External = 1;
           // BizAction.BillType = (BillTypes)(2);

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
            {
                BizAction.UnitID = BizAction.PatientUnitID.Value;
            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
            else
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IPDCounterID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    clsGetBillSearchListBizActionVO result = e.Result as clsGetBillSearchListBizActionVO;
                    if (result.List != null)
                    {
                        foreach (var item in result.List)
                        {
                            if (item.ISDischarged == true && item.Opd_Ipd_External_Id == BizAction.Opd_Ipd_External_Id && item.Opd_Ipd_External_UnitId == BizAction.Opd_Ipd_External_UnitId && item.Opd_Ipd_External == 1)
                            {
                                ISDischarged = true;
                            }
                            else
                            {
                                ISDischarged = false;
                            }                         
                        }
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void LoadPatientHeader()
        {
            if (indicator == null || indicator.Visibility == Visibility.Collapsed)
            {
                indicator = new WaitIndicator();
                indicator.Show();
            }
            try
            {
                clsGetPatientConsoleHeaderDeailsBizActionVO BizAction = new clsGetPatientConsoleHeaderDeailsBizActionVO();
                BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                BizAction.ISOPDIPD = false;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        BizAction.PatientDetails = ((clsGetPatientConsoleHeaderDeailsBizActionVO)args.Result).PatientDetails;
                        BizAction.PatientDetails.UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                        PatientName = ((clsGetPatientConsoleHeaderDeailsBizActionVO)args.Result).PatientDetails.PatientName;
                        MRNO = ((clsGetPatientConsoleHeaderDeailsBizActionVO)args.Result).PatientDetails.MRNo;

                        if (BizAction.PatientDetails.Gender.ToUpper() == "MALE")
                        {
                            Patient.DataContext = BizAction.PatientDetails;
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
                                imgPhoto.Source = new BitmapImage(new Uri("./Icons/MAle.png", UriKind.Relative));
                        }
                        else
                        {
                            Patient.DataContext = BizAction.PatientDetails;

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
                    }
                    indicator.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                indicator.Close();
                throw;
            }
        }


        private void FillMatarialConsumptionList()
        {
            indicator.Show();
            try
            {
                clsGetMatarialConsumptionListBizActionVO BizAction = new clsGetMatarialConsumptionListBizActionVO();
                BizAction.ConsumptionList = new List<clsMaterialConsumptionVO>();

                BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
                BizAction.AdmID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.ID;
                BizAction.AdmissionUnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId;
                BizAction.IsPatientAgainstMaterialConsumption = true;
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }
                else
                {
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }

                BizAction.IsPagingEnabled = true;
                BizAction.StartIndex = ConsumptionDataList.PageIndex * ConsumptionDataList.PageSize;
                BizAction.MaximumRows = ConsumptionDataList.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        BizAction = (clsGetMatarialConsumptionListBizActionVO)e.Result;
                        BizAction.ConsumptionList = ((clsGetMatarialConsumptionListBizActionVO)e.Result).ConsumptionList;                       
                        ConsumptionDataList.TotalItemCount = BizAction.TotalRows;
                        ConsumptionDataList.Clear();
                        foreach (var item in BizAction.ConsumptionList)
                        {
                            if (item.MRP > 0)
                            {
                                if (item.UsedQty > 1)
                                {
                                    TotalAmount = TotalAmount + (item.MRP * Convert.ToDouble(item.UsedQty));
                                }
                                else
                                {
                                    TotalAmount = TotalAmount + item.MRP;
                                }
                                txtTotalAmount.Text = Convert.ToString(TotalAmount);
                            }
                            ConsumptionDataList.Add(item);
                        }
                        dgMaterailConsumptionList.ItemsSource = null;
                        dgMaterailConsumptionList.ItemsSource = ConsumptionDataList;
                        dpdgMaterailConsumptionList.Source = null;
                        dpdgMaterailConsumptionList.PageSize = BizAction.MaximumRows;
                        dpdgMaterailConsumptionList.Source = ConsumptionDataList;                      
                    }
                    indicator.Close();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                indicator.Close();
                throw;
            }
        }

        public string Action { get; set; }
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            if (ISDischarged == false)
            {
                ModuleName = "PalashDynamics.Pharmacy";
                Action = "PalashDynamics.Pharmacy.MaterialConsumption";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_ConsumptionOpenReadCompleted);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {              
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                new MessageBoxControl.MessageBoxChildWindow("", "Patient is not Admitted.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                return;
            }
        }

        void c_ConsumptionOpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
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
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
                clsIPDAdmissionVO objAdmission = new clsIPDAdmissionVO();

                objAdmission.PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                objAdmission.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
                objAdmission.AdmID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.ID;
                objAdmission.AdmissionUnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId;
                objAdmission.PatientName = PatientName;
                objAdmission.MRNo = MRNO;
                objAdmission.TariffID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.TariffID;
                objAdmission.CompanyID = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                objAdmission.PatientSourceID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientSourceID;
                objAdmission.DoctorName = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.DoctorName;
                objAdmission.DoctorID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.DoctorID;
                objAdmission.PatientCategoryID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientCategoryID;
                ((IInitiateMaterialConsumption)myData).InitiateMaterialConsumption(objAdmission);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {

            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/InventoryPharmacy/MaterialConsumptionReport.aspx?PatientID=" + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID + "&PatientUnitID=" + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID + "&AdmID=" + ((IApplicationConfiguration)App.Current).SelectedIPDPatient.ID + "&AdmissionUnitID=" + ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId + "&ISPatientAgainst=" + 1), "_blank");

        }

        private void cmdPrintPackage_Click(object sender, RoutedEventArgs e)
        {
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/InventoryPharmacy/PackageConsumption.aspx?PatientID=" + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID + "&PatientUnitID=" + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID + "&AdmID=" + ((IApplicationConfiguration)App.Current).SelectedIPDPatient.ID + "&AdmissionUnitID=" + ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId ), "_blank");
        }

        private void cmdCloseM_Click(object sender, RoutedEventArgs e)
        {
            ModuleName = "PalashDynamics.IPD";
            Action = "PalashDynamics.IPD.Forms.frmAdmissionList";
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            WebClient c = new WebClient();
            c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_AdmissionListOpenReadCompleted);
            c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
        }

        void c_AdmissionListOpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
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
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
