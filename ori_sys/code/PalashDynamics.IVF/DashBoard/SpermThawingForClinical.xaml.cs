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
using System.IO;
using System.Xml.Linq;
using System.Windows.Resources;
using System.Reflection;
using CIMS;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Patient;
using System.Windows.Media.Imaging;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.Collections;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class SpermThawingForClinical : UserControl, IInitiateCIMS
    {
        public SpermThawingForClinical()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((IApplicationConfiguration)App.Current).SelectedPatient.Gender != Genders.Female.ToString())
                {
                    if (IsPatientExist == false)
                    {
                        ModuleName = "PalashDynamics";
                        Action = "CIMS.Forms.PatientList";
                        UserControl rootPage = Application.Current.RootVisual as UserControl;

                        WebClient c2 = new WebClient();
                        c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                        c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                    }
                    else
                    {
                        fillCoupleDetails();
                        ThawDetailsList = new PagedSortableCollectionView<cls_NewThawingDetailsVO>();
                        //ThawList = new List<cls_NewThawingDetailsVO>();
                        //ThawDetailsList = new PagedSortableCollectionView<cls_NewThawingDetailsVO>();
                        //ThawDetailsList.OnRefresh += new EventHandler<RefreshEventArgs>(ThawDetailsList_OnRefresh);
                        ThawDetailsListPageSize = 15;
                        //this.ThawingDetilsGridPager.DataContext = ThawDetailsList;
                        //this.dgThawingDetilsGrid.DataContext = ThawDetailsList;
                        //dgThawingDetilsGrid.UpdateLayout();
                    }

                }
                else
                {
                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                    ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Male Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void dgVitrificationDetilsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (ThawDetailsList.Count > 0)
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure \n You Want To Save Semen Thawing Details";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWin.OnMessageBoxClosed += (result) =>
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        SaveThawing();
                    }
                };
                msgWin.Show();
            }
            else
            {
                string msgTitle = "Palash";
                string msgText = "No Details Available";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgWin.Show();
            }
        }
        public Boolean IsEdit { get; set; }
        private void SaveThawing()
        {
            try
            {
                wait.Show();
                clsAddUpdateSpermThawingBizActionVO BizAction = new clsAddUpdateSpermThawingBizActionVO();
                BizAction.ThawingList = new List<cls_NewThawingDetailsVO>();
                BizAction.IsNewForm = true;
                BizAction.ThawingList = ThawList;
                #region Service Call (Check Validation)
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        string txtmsg = "";
                        if (IsEdit == true)
                        {
                            txtmsg = "Semen Thawing Details Updated Successfully";
                        }
                        else
                        {
                            txtmsg = "Semen Thawing Details Saved Successfully";
                        }

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", txtmsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            FillThawingDetails();
                        };
                        msgW1.Show();
                        wait.Close();
                    }
                    else
                    {
                        wait.Close();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                #endregion

            }
            catch (Exception ex)
            {
                wait.Close();
            }

        }
        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {

        }
        private bool IsPatientExist;
        public string Action { get; set; }
        public string ModuleName { get; set; }
        UIElement myData = null;
       
        void c2_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
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
                ((IInitiateCIMS)myData).Initiate("VISIT");
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region Check Patient Is Selected/Not
        public void Initiate(string Mode)
        {
            switch (Mode.ToUpper())
            {
                case "NEW":

                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        return;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        return;
                    }
                    IsPatientExist = true;

                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                    mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;
                    break;
                default:
                    break;
            }

        }

        #endregion
        #region FillCouple Details
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
        private void fillCoupleDetails()
        {
            clsGetCoupleDetailsBizActionVO BizAction = new clsGetCoupleDetailsBizActionVO();
            BizAction.PatientID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
            BizAction.CoupleDetails = new clsCoupleVO();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails != null && ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.CoupleId > 0)
                    {

                        CoupleInfo.Visibility = Visibility.Visible;
                        BizAction.CoupleDetails.MalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient;
                        BizAction.CoupleDetails.FemalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient;
                        CoupleDetails.MalePatient = new clsPatientGeneralVO();
                        CoupleDetails.FemalePatient = new clsPatientGeneralVO();
                        CoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
                        //if (CoupleDetails.CoupleId == 0)
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //         new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Create Examination, Examination is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //    msgW1.Show();
                        //    //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                        //    //((IInitiateCIMS)App.Current).Initiate("VISIT");
                        //    ModuleName = "PalashDynamics";
                        //    Action = "CIMS.Forms.PatientList";
                        //    UserControl rootPage = Application.Current.RootVisual as UserControl;
                        //    WebClient c2 = new WebClient();
                        //    c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                        //    c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                        //}
                        //else
                        //{
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
                        // }
                    }
                    else
                    {

                        #region Commented by Saily P on 260912. purpose, the form is applicable to donor patient
                        //MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //         new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Create Examination, Examination is Only For Active Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //msgW1.Show();
                        ////((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                        ////((IInitiateCIMS)App.Current).Initiate("VISIT");

                        //ModuleName = "PalashDynamics";
                        //Action = "CIMS.Forms.PatientList";
                        //UserControl rootPage = Application.Current.RootVisual as UserControl;

                        //WebClient c2 = new WebClient();
                        //c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                        //c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                        #endregion
                    }
                    fillCanID();
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

        #endregion
        private List<MasterListItem> _CanList = new List<MasterListItem>();
        public List<MasterListItem> CanList
        {
            get
            {
                return _CanList;
            }
            set
            {
                _CanList = value;
            }
        }

        private List<MasterListItem> _Plan = new List<MasterListItem>();
        public List<MasterListItem> SelectedPLan
        {
            get
            {
                return _Plan;
            }
            set
            {
                _Plan = value;
            }
        }

        private List<MasterListItem> _LabIncharge = new List<MasterListItem>();
        public List<MasterListItem> SelectedLabIncharge
        {
            get
            {
                return _LabIncharge;
            }
            set
            {
                _LabIncharge = value;
            }
        }
        WaitIndicator wait = new WaitIndicator();
        #region Fill Master Item
        private void fillCanID()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFCanMaster;
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
                        CanList = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        FillLabPerson();
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        private void FillLabPerson()
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizAction.DepartmentId = 0;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);
                    SelectedLabIncharge = objList;
                    fillPlan();
                    if (this.DataContext != null)
                    {
                        //   cmbLabPerson.SelectedValue = ((cls_NewThawingDetailsVO)this.DataContext).LabInchargeId;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void fillPlan()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_PostThawingPlan;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        SelectedPLan = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        FillThawingDetails();
                    }

                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }
        #endregion
        public PagedSortableCollectionView<cls_NewThawingDetailsVO> ThawDetailsList { get; private set; }
        public int ThawDetailsListPageSize
        {
            get
            {
                return ThawDetailsList.PageSize;
            }
            set
            {
                if (value == ThawDetailsList.PageSize) return;
                ThawDetailsList.PageSize = value;
            }
        }
        public List<cls_NewThawingDetailsVO> ThawList;
        private void FillThawingDetails()
        {
            try
            {
                cls_NewGetSpremThawingBizActionVO bizAction = new cls_NewGetSpremThawingBizActionVO();
                bizAction.MalePatientID = CoupleDetails.MalePatient.PatientID;
                bizAction.MalePatientUnitID = CoupleDetails.MalePatient.UnitId;
                bizAction.IsThawDetails = true;
                bizAction.IsPagingEnabled = true;
                bizAction.StartIndex = ThawDetailsList.PageIndex * ThawDetailsList.PageSize;
                bizAction.MaximumRows = ThawDetailsList.PageSize;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList != null)
                    {
                        ThawDetailsList.Clear();
                        ThawList = ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList;
                        ThawDetailsList.TotalItemCount = (int)((cls_NewGetSpremThawingBizActionVO)arg.Result).TotalRows;
                        for (int i = 0; i < ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList.Count; i++)
                        {
                            if (((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].IsFreezed == true)
                            {
                                ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].IsEnabled = false;
                            }
                            else
                            {
                                ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].IsEnabled = true;
                            }
                            if (((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].LabPersonId > 0)
                            {
                                ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].SelectedIncharge = SelectedLabIncharge.FirstOrDefault(p => p.ID == ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].LabPersonId);
                            }
                            else
                            {
                                ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].SelectedIncharge = SelectedLabIncharge.FirstOrDefault(p => p.ID == 0);
                            }
                            ThawDetailsList.Add(((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i]);
                        }
                        dgThawingDetilsGrid.ItemsSource = null;
                        dgThawingDetilsGrid.ItemsSource = ThawDetailsList;
                        dgThawingDetilsGrid.UpdateLayout();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(bizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void cmbPlanForSperms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbLabIncharge1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < ThawDetailsList.Count; i++)
            {
                if (i == dgThawingDetilsGrid.SelectedIndex)
                {
                    if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                    {
                        ThawDetailsList[i].LabPersonId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                    }
                }
            }
        }

        private void dgThawingDetilsGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {

            //cls_NewThawingDetailsVO ThawRow = (cls_NewThawingDetailsVO)e.Row.DataContext;
            //if (ThawRow.IsFreezed == true)
            //{
            //     e.Row.IsEnabled = false;
            //    //dgThawingDetilsGrid.Columns[4].IsReadOnly = true;
            //    //dgThawingDetilsGrid.Columns[5].IsReadOnly = true;
            //    //dgThawingDetilsGrid.Columns[6].IsReadOnly = true;
            //}
            //else
            //{
            //    e.Row.IsEnabled = true;
            //    //dgThawingDetilsGrid.Columns[4].IsReadOnly = false;
            //    //dgThawingDetilsGrid.Columns[5].IsReadOnly = false;
            //    //dgThawingDetilsGrid.Columns[6].IsReadOnly = false;
            //}
        }

        private void dgThawingDetilsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgThawingDetilsGrid.SelectedItem != null)
            {
                cls_GetSpremFreezingDetilsForThawingBizActionVO bizAction = new cls_GetSpremFreezingDetilsForThawingBizActionVO();
                bizAction.ID = ((cls_NewThawingDetailsVO)dgThawingDetilsGrid.SelectedItem).FreezingID;
                bizAction.UnitID = ((cls_NewThawingDetailsVO)dgThawingDetilsGrid.SelectedItem).FreezingUnitID;
                bizAction.SpremFreezingDetailsVO.SpremNo = ((cls_NewThawingDetailsVO)dgThawingDetilsGrid.SelectedItem).SpremNo;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremFreezingDetails != null)
                    {
                        dgVitrificationDetilsGrid.ItemsSource=null;
                        dgVitrificationDetilsGrid.ItemsSource=((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremFreezingDetails;
                        dgVitrificationDetilsGrid.UpdateLayout();
                    }
                };
                client.ProcessAsync(bizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
           
        }
    }
}
