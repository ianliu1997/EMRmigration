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
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Administration.StaffMaster;
using PalashDynamics.Animations;
using System.Text;
using System.Windows.Browser;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class SpermThawingForDashboard : ChildWindow
    {
        public clsCoupleVO CoupleDetails;
        public event RoutedEventHandler OKButtonCode_Click;
        public SpermThawingForDashboard SpermThaw { get; set; }
        public string spermFrezingCode;
        public long ThawingID;
        public long SemenWashID;
        public bool IsView = false;

        //public List<cls_NewGetSpremThawingBizActionVO> spermFrezingCode;  
        //public cls_NewGetSpremThawingBizActionVO SpermThaw { get; set; }

        public SpermThawingForDashboard()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            SetCommandButtonState("Load");

        }

        private ObservableCollection<cls_NewGetSpremThawingBizActionVO> _ThawDeList = new ObservableCollection<cls_NewGetSpremThawingBizActionVO>();
        public ObservableCollection<cls_NewGetSpremThawingBizActionVO> ThawDeList
        {
            get { return _ThawDeList; }
            set { _ThawDeList = value; }
        }

        private SwivelAnimation objAnimation;

        private void dgVitrificationDetilsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (dgVitrificationDetilsGrid.SelectedItem != null)
            //{
            //    dgThawingDetilsGrid.ItemsSource = null;
            //    ThawDeList = new ObservableCollection<cls_NewGetSpremThawingBizActionVO>();
            //    ThawDeList.Add(new cls_NewGetSpremThawingBizActionVO()
            //    {
            //        SpremNo = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).SpremNostr,
            //        UintID = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).UnitID,
            //        ThawingDate = DateTime.Now,
            //        ThawingTime = DateTime.Now,
            //        TotalSpearmCount = 0,
            //        Motility = 0,
            //        ProgressionID = 0,
            //        ProgressionListVO = ProgressionList,
            //        AbnormalSperm = 0,
            //        RoundCells = 0,
            //        AgglutinationID = 0,
            //        AgglutinationListVO = AgglutinationList,
            //        Comments = "",
            //        DoctorID = 0,
            //        DoctorListVO = DoctorList,
            //        WitnessedID = 0,
            //        WitnessedListVO = WitnessedList
            //    });

            //    dgThawingDetilsGrid.ItemsSource = ThawDeList;
            //    dgThawingDetilsGrid.UpdateLayout();
            //}

            //if(ThawDeList.Count==0)
            //dgThawingDetilsGrid.ItemsSource = null;
            //     ThawDeList = new ObservableCollection<cls_NewGetSpremThawingBizActionVO>();
            //else 
            //ThawDeList.Add(new cls_NewGetSpremThawingBizActionVO()
            //{
            //    SpremNo = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).SpremNostr,
            //    UintID = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).UnitID,
            //    ThawingDate = DateTime.Now,
            //    ThawingTime = DateTime.Now,
            //    TotalSpearmCount = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).SpermCount,
            //    Motility = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).Motility,
            //    ProgressionID = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).ProgressionID,
            //    ProgressionListVO = ProgressionList,
            //    AbnormalSperm = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).AbnormalSperm,
            //    RoundCells =Convert.ToInt64( ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).RoundCell),
            //    AgglutinationID = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).AgglutinationID,                
            //    AgglutinationListVO = AgglutinationList,
            //    Comments = "",
            //    DoctorID = 0,
            //    DoctorListVO = DoctorList,
            //    WitnessedID = 0,
            //    WitnessedListVO = WitnessedList
            //});

            //dgThawingDetilsGrid.ItemsSource = ThawDeList;
            //dgThawingDetilsGrid.UpdateLayout();
        }

        #region Screp Code
        //ThawDeList.Add(new cls_NewGetSpremThawingBizActionVO()
        //{
        //    SpremNo = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).SpremNostr,
        //    UintID = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).UnitID,
        //    ThawingDate = DateTime.Now,
        //    ThawingTime = DateTime.Now,
        //    TotalSpearmCount = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).SpermCount,
        //    Motility = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).Motility,
        //    ProgressionListVO = ProgressionList,
        //    ProgressionID = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).ProgressionID,                                
        //    AbnormalSperm = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).AbnormalSperm,
        //    RoundCells = Convert.ToInt64(((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).RoundCell),
        //    AgglutinationListVO = AgglutinationList,
        //    AgglutinationID = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).AgglutinationID,                                
        //    Comments = "",
        //    DoctorID = 0,
        //    DoctorListVO = DoctorList,
        //    WitnessedID = 0,
        //    WitnessedListVO = WitnessedList
        //});

        //ThawDeList.Add(new cls_NewGetSpremThawingBizActionVO()
        //{
        //    SpremNo = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).SpremNostr,
        //    UintID = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).UnitID,
        //    ThawingDate = DateTime.Now,
        //    ThawingTime = DateTime.Now,
        //    TotalSpearmCount = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).SpermCount,
        //    Motility = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).Motility,
        //    ProgressionListVO = ProgressionList,
        //    ProgressionID = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).ProgressionID,

        //    selectedProgressionListVO.ID=ProgressionID,
        //    //ProgressionListVO.ID = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).ProgressionID,
        //    //selectedProgressionListVO.ID = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).ProgressionID,

        //    AbnormalSperm = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).AbnormalSperm,
        //    RoundCells = Convert.ToInt64(((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).RoundCell),
        //    AgglutinationListVO = AgglutinationList,
        //    AgglutinationID = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).AgglutinationID,

        //    Comments = "",
        //    DoctorID = 0,
        //    DoctorListVO = DoctorList,
        //    WitnessedID = 0,
        //    WitnessedListVO = WitnessedList
        //});
        #endregion

        private void chkSelect_Click(object sender, RoutedEventArgs e)
        {


            if (dgVitrificationDetilsGrid.SelectedItem != null)
            {
                if (ThawDeList == null)
                    ThawDeList = new ObservableCollection<cls_NewGetSpremThawingBizActionVO>();
                CheckBox chk = (CheckBox)sender;
                StringBuilder strError = new StringBuilder();
                if (chk.IsChecked == true)
                {
                    if (ThawDeList.Count > 0)
                    {
                        var item = from r in ThawDeList
                                   where r.ID == ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).ID
                                   select new cls_NewGetSpremThawingBizActionVO
                                   {
                                       //Status = r.Status,
                                       ID = r.ID
                                       //,ServiceName = r.ServiceName
                                   };
                        if (item.ToList().Count > 0)
                        {
                            if (strError.ToString().Length > 0)
                                strError.Append(",");
                            strError.Append(((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem));

                            if (!string.IsNullOrEmpty(strError.ToString()))
                            {
                                string strMsg = "Freezing Details already Selected : " + strError.ToString();

                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                        }
                        else
                        {
                            cls_NewGetSpremThawingBizActionVO obj = new cls_NewGetSpremThawingBizActionVO();

                            obj.SpremNo = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).SpremNostr;
                            obj.UintID = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).UnitID;
                            obj.ThawingDate = DateTime.Now;
                            obj.ThawingTime = DateTime.Now;
                            obj.TotalSpearmCount = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).SpermCount;
                            obj.Motility = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).Motility;
                            obj.ProgressionListVO = ProgressionList;
                            obj.ProgressionID = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).ProgressionID;
                            obj.selectedProgressionListVO.ID = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).ProgressionID;
                            obj.selectedProgressionListVO = ProgressionList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).ProgressionID));
                            obj.AbnormalSperm = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).AbnormalSperm;
                            obj.RoundCells = Convert.ToInt64(((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).RoundCell);
                            obj.AgglutinationListVO = AgglutinationList;
                            obj.AgglutinationID = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).AgglutinationID;
                            obj.selectedAgglutinationListVO.ID = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).AgglutinationID;
                            obj.selectedAgglutinationListVO = AgglutinationList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).AgglutinationID));
                            obj.Comments = "";
                            obj.DoctorID = 0;
                            obj.DoctorListVO = DoctorList;
                            obj.WitnessedID = 0;
                            obj.WitnessedListVO = WitnessedList;
                            obj.InchargeIdList = SelectedLabIncharge;
                            obj.IsEnabled = true;
                            ThawDeList.Add(obj);
                        }
                    }
                    else
                    {
                        cls_NewGetSpremThawingBizActionVO obj = new cls_NewGetSpremThawingBizActionVO();

                        obj.SpremNo = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).SpremNostr;
                        obj.UintID = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).UnitID;
                        obj.ThawingDate = DateTime.Now;
                        obj.ThawingTime = DateTime.Now;
                        obj.TotalSpearmCount = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).SpermCount;
                        obj.Motility = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).Motility;
                        obj.ProgressionListVO = ProgressionList;
                        obj.ProgressionID = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).ProgressionID;
                        obj.selectedProgressionListVO.ID = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).ProgressionID;
                        obj.selectedProgressionListVO = ProgressionList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).ProgressionID));
                        obj.AbnormalSperm = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).AbnormalSperm;
                        obj.RoundCells = Convert.ToInt64(((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).RoundCell);
                        obj.AgglutinationListVO = AgglutinationList;
                        obj.AgglutinationID = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).AgglutinationID;
                        obj.selectedAgglutinationListVO.ID = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).AgglutinationID;

                        obj.selectedAgglutinationListVO = AgglutinationList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).AgglutinationID));

                        obj.Comments = "";
                        obj.DoctorID = 0;
                        obj.DoctorListVO = DoctorList;
                        obj.WitnessedID = 0;
                        obj.WitnessedListVO = WitnessedList;
                        obj.InchargeIdList = SelectedLabIncharge;
                        obj.IsEnabled = true;
                        ThawDeList.Add(obj);
                    }
                }
                else
                {
                    string spno = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).SpremNostr;

                    var authors = ThawDeList.Where(a => a.SpremNo == spno).ToList();

                    foreach (var author in authors)
                    {
                        ThawDeList.Remove(author);
                    }
                }


                foreach (var item in ThawDeList)
                {
                    item.SelectFreezed = true;
                }
                dgThawingDetilsGrid.ItemsSource = null;
                dgThawingDetilsGrid.ItemsSource = ThawDeList;
                dgThawingDetilsGrid.UpdateLayout();
                dgThawingDetilsGrid.Focus();
            }


        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            //if (ThawDetailsList.Count > 0)
            if (ThawDeList.Count > 0)
            {
                bool IsFreezed = false;
                var item = ThawDeList.Where(x => x.IsFreezed == false).ToList();
                if (item.Count > 0)
                {
                    string msgTitle = "Palash";
                    string msgText = "Please freeze thawing";
                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgWin.Show();
                }
                else
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
                //BizAction.ThawingList = ThawList;
                BizAction.ThawDeList = ThawDeList;

                for (int i = 0; i < ThawDeList.Count; i++)
                {
                    if (ThawDeList[i].ID != 0)
                    {
                        BizAction.ThawDeList[i].ID = ThawDeList[i].ID;
                    }
                    else
                    {
                        BizAction.ThawDeList[i].ID = 0;
                    }
                    //BizAction.ThawDeList[i].ProgressionID = ThawDeList[i].selectedProgressionListVO.ID;
                    //BizAction.ThawDeList[i].AgglutinationID = ThawDeList[i].selectedAgglutinationListVO.ID;
                    BizAction.ThawDeList[i].DoctorID = ThawDeList[i].selectedDoctorListVO.ID;
                    BizAction.ThawDeList[i].WitnessedID = ThawDeList[i].selectedWitnessedListVO.ID;
                }


                BizAction.CoupleID = CoupleDetails.CoupleId;
                BizAction.PatientId = CoupleDetails.MalePatient.PatientID;
                BizAction.CoupleUintID = CoupleDetails.MalePatient.UnitId;

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
                            FillSpermFreezingDetails();
                            FillThawingDetails();
                            //FillFrontGrid();
                            //objAnimation.Invoke(RotationType.Backward);
                            //this.SetCommandButtonState("Load");
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

        public bool IsCancel = true;
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "New":
                    cmdNew.IsEnabled = false;
                    CmdSave.IsEnabled = true;
                    CmdClose.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Save":
                    cmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = false;
                    CmdSave.IsEnabled = true;
                    CmdClose.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = true;

                    break;
                default:
                    break;
            }
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = false;
            if (IsCancel == true)
            {
                this.DialogResult = false;
                if (OKButtonCode_Click != null)
                    OKButtonCode_Click(this, new RoutedEventArgs());
            }
            else
            {
                objAnimation.Invoke(RotationType.Backward);
                IsCancel = true;
                CmdSave.Content = "Save";
                //  FetchData();
            }
            SetCommandButtonState("Cancel");
        }
        private bool IsPatientExist;
        public string Action { get; set; }
        public string ModuleName { get; set; }
        UIElement myData = null;
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        public bool IsFromSemenPreparation = false;

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //FillSpermFreezingDetails();
            fillCanID();
            ThawDetailsList = new PagedSortableCollectionView<cls_NewThawingDetailsVO>();
            ThawDetailsListPageSize = 15;

            if (IsFromSemenPreparation == true)
            {
                //cmdNew.Visibility = Visibility.Collapsed;
                //CmdSave.Visibility = Visibility.Collapsed;
                //cmdOk.Visibility = Visibility.Visible;

                FrontPanel.Visibility = Visibility.Collapsed;
                BackPanel.Visibility = Visibility.Visible;
                cmdNew.Visibility = Visibility.Collapsed;
                CmdSave.Visibility = Visibility.Visible;
                CmdSave.IsEnabled = true;
                if (IsView)
                    ViewSpermFreezingDetails();
                else
                    FillSpermFreezingDetails();
                ThawDeList.Clear();
                dgThawingDetilsGrid.ItemsSource = null;
                dgThawingDetilsGrid.UpdateLayout();
                CmdSave.Content = "Save";

            }

            if (IsFromSemenPreparation == false)
            {
                FrontPanel.Visibility = Visibility.Visible;
                BackPanel.Visibility = Visibility.Collapsed;
                cmdNew.Visibility = Visibility.Collapsed;
                CmdSave.Visibility = Visibility.Collapsed;
                //cmdOk.Visibility = Visibility.Visible;
            }

            this.Title = "Semen Thawing  :-(Name- " + CoupleDetails.MalePatient.FirstName +
                   " " + CoupleDetails.MalePatient.LastName + ")";
        }

        public StringBuilder SampleID = new StringBuilder();
        List<cls_NewThawingDetailsVO> ThawItemList = new List<cls_NewThawingDetailsVO>();
        private void FillSpermFreezingDetails()
        {
            cls_GetSpremFreezingDetilsForThawingBizActionVO bizAction = new cls_GetSpremFreezingDetilsForThawingBizActionVO();

            bizAction.MalePatientID = CoupleDetails.MalePatient.PatientID;
            bizAction.MalePatientUnitID = CoupleDetails.MalePatient.UnitId;

            //bizAction.ID = ((cls_NewThawingDetailsVO)dgThawingDetilsGrid.SelectedItem).FreezingID;
            //bizAction.UnitID = ((cls_NewThawingDetailsVO)dgThawingDetilsGrid.SelectedItem).FreezingUnitID;
            //bizAction.SpremFreezingDetailsVO.SpremNo = ((cls_NewThawingDetailsVO)dgThawingDetilsGrid.SelectedItem).SpremNo;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null && ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremFreezingDetails != null)
                {
                    if (((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList != null && ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList.Count > 0)
                    {
                        (from a1 in ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremFreezingDetails
                         join a2 in ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList on a1.ID equals a2.SpremNo
                         select new { A1 = a1, A2 = a2 }).ToList().ForEach(x => { x.A1.IsEnabled = false; x.A1.SelectFreezed = true; });


                        var theRest = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremFreezingDetails.Where(x => !((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList.Any(b => b.SpremNo == x.ID));
                        theRest.ToList().ForEach(x => { x.IsEnabled = true; });

                    }
                    else
                    {
                        foreach (var item in ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremFreezingDetails)
                        {
                            item.IsEnabled = true;
                        }
                    }
                    dgVitrificationDetilsGrid.ItemsSource = null;
                    dgVitrificationDetilsGrid.ItemsSource = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremFreezingDetails;
                    dgVitrificationDetilsGrid.UpdateLayout();


                    ThawDeList.Clear();
                    for (int i = 0; i < ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList.Count; i++)
                    {
                        ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].InchargeIdList = SelectedLabIncharge;
                        if (((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].LabPersonId > 0)
                        {
                            ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].SelectedIncharge = SelectedLabIncharge.FirstOrDefault(p => p.ID == ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].LabPersonId);
                        }
                        else
                        {
                            ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].SelectedIncharge = SelectedLabIncharge.FirstOrDefault(p => p.ID == 0);
                        }

                        //if (((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].PlanTherapyID == PlanTherapyID && ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].PlanTherapyUnitID == PlanTherapyUnitID)
                        //{
                        ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].IsEnabled = false;

                        if (SampleID.ToString().Length > 0)
                            SampleID.Append(",");
                        if (((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].IsFreezed == true)
                            SampleID.Append(((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].SpremNo.ToString());

                        cls_NewGetSpremThawingBizActionVO obj = new cls_NewGetSpremThawingBizActionVO();
                        obj.ID = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].ID;
                        obj.UintID = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].UnitID;
                        obj.ThawingDate = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].ThawingDate;
                        obj.ThawingTime = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].ThawingTime;
                        obj.SpremNo = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].SpremNo.ToString();
                        obj.LabPersonId = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].LabPersonId;
                        obj.PlanTherapyID = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].PlanTherapyID;
                        obj.PlanTherapyUnitID = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].PlanTherapyUnitID;
                        obj.IsFreezed = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].IsFreezed;
                        obj.SemenWashID = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].SemenWashID;

                        obj.InchargeIdList = SelectedLabIncharge;
                        if (((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].LabPersonId > 0)
                        {
                            obj.SelectedIncharge = SelectedLabIncharge.FirstOrDefault(p => p.ID == ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].LabPersonId);
                        }
                        else
                        {
                            obj.SelectedIncharge = SelectedLabIncharge.FirstOrDefault(p => p.ID == 0);
                        }

                        obj.IsEnabled = false;
                        //ThawList.Add(((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i]);

                        if (obj.SemenWashID == 0 || obj.SemenWashID == SemenWashID || obj.SemenWashID == null)
                        {
                            ThawDeList.Add(obj);
                            ThawItemList.Add(((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i]);
                        }


                        //var result = from x in ThawDeList
                        //             where x.ThawingDate == ThawDate 
                        //             select x.ThawingTime;
                        //ThawTime = (DateTime?)result;

                        //}
                    }

                    dgThawingDetilsGrid.ItemsSource = null;
                    dgThawingDetilsGrid.ItemsSource = ThawDeList;
                    dgThawingDetilsGrid.UpdateLayout();



                    //if (ThawDeList.Count == 0)
                    //{
                    //    var theList = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremFreezingDetails.Where(x => !((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList.Any(b => b.SpremNo == x.ID));
                    //    theList.ToList().ForEach(x => { x.IsEnabled = true; });
                    //    dgVitrificationDetilsGrid.ItemsSource = null;
                    //    dgVitrificationDetilsGrid.ItemsSource = theList;
                    //    dgVitrificationDetilsGrid.UpdateLayout();

                    //}
                    //else if (ThawDeList != null && ThawDeList.Count > 0)
                    //{
                    //    (from a1 in ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremFreezingDetails
                    //     join a2 in ThawItemList on a1.ID equals a2.SpremNo
                    //     select new { A1 = a1, A2 = a2 }).ToList().ForEach(x => { x.A1.IsEnabled = false; x.A1.SelectFreezed = true; });

                    //    var theRest = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremFreezingDetails.Where(x => !ThawItemList.Any(b => b.SpremNo == x.ID));
                    //    theRest.ToList().ForEach(x => { x.IsEnabled = true; });

                    //    dgVitrificationDetilsGrid.ItemsSource = null;
                    //    dgVitrificationDetilsGrid.ItemsSource = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremFreezingDetails;
                    //    dgVitrificationDetilsGrid.UpdateLayout();
                    //}





                    //dgVitrificationDetilsGrid.ItemsSource = null;
                    //dgVitrificationDetilsGrid.ItemsSource = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremFreezingDetails;
                    //dgVitrificationDetilsGrid.UpdateLayout();
                }
            };
            client.ProcessAsync(bizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void ViewSpermFreezingDetails()
        {
            cls_GetSpremFreezingDetilsForThawingBizActionVO bizAction = new cls_GetSpremFreezingDetilsForThawingBizActionVO();
            bizAction.IsView = true;
            bizAction.SemenWashID = SemenWashID;
            bizAction.MalePatientID = CoupleDetails.MalePatient.PatientID;
            bizAction.MalePatientUnitID = CoupleDetails.MalePatient.UnitId;

            //bizAction.ID = ((cls_NewThawingDetailsVO)dgThawingDetilsGrid.SelectedItem).FreezingID;
            //bizAction.UnitID = ((cls_NewThawingDetailsVO)dgThawingDetilsGrid.SelectedItem).FreezingUnitID;
            //bizAction.SpremFreezingDetailsVO.SpremNo = ((cls_NewThawingDetailsVO)dgThawingDetilsGrid.SelectedItem).SpremNo;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null && ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremFreezingDetails != null)
                {
                    if (((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList != null && ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList.Count > 0)
                    {
                        (from a1 in ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremFreezingDetails
                         join a2 in ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList on a1.ID equals a2.SpremNo
                         select new { A1 = a1, A2 = a2 }).ToList().ForEach(x => { x.A1.IsEnabled = false; x.A1.SelectFreezed = true; });


                        var theRest = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremFreezingDetails.Where(x => !((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList.Any(b => b.SpremNo == x.ID));
                        theRest.ToList().ForEach(x => { x.IsEnabled = true; });

                    }
                    else
                    {
                        foreach (var item in ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremFreezingDetails)
                        {
                            item.IsEnabled = true;
                        }
                    }
                    dgVitrificationDetilsGrid.ItemsSource = null;
                    dgVitrificationDetilsGrid.ItemsSource = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremFreezingDetails;
                    dgVitrificationDetilsGrid.UpdateLayout();

                    ThawDeList.Clear();
                    for (int i = 0; i < ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList.Count; i++)
                    {
                        ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].InchargeIdList = SelectedLabIncharge;
                        if (((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].LabPersonId > 0)
                        {
                            ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].SelectedIncharge = SelectedLabIncharge.FirstOrDefault(p => p.ID == ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].LabPersonId);
                        }
                        else
                        {
                            ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].SelectedIncharge = SelectedLabIncharge.FirstOrDefault(p => p.ID == 0);
                        }

                        //if (((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].PlanTherapyID == PlanTherapyID && ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].PlanTherapyUnitID == PlanTherapyUnitID)
                        //{
                        ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].IsEnabled = false;

                        if (SampleID.ToString().Length > 0)
                            SampleID.Append(",");
                        if (((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].IsFreezed == true)
                            SampleID.Append(((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].SpremNo.ToString());

                        cls_NewGetSpremThawingBizActionVO obj = new cls_NewGetSpremThawingBizActionVO();
                        obj.ID = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].ID;
                        obj.UintID = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].UnitID;
                        obj.ThawingDate = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].ThawingDate;
                        obj.ThawingTime = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].ThawingTime;
                        obj.SpremNo = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].SpremNo.ToString();
                        obj.LabPersonId = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].LabPersonId;
                        obj.PlanTherapyID = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].PlanTherapyID;
                        obj.PlanTherapyUnitID = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].PlanTherapyUnitID;
                        obj.IsFreezed = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].IsFreezed;
                        obj.SemenWashID = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].SemenWashID;

                        obj.InchargeIdList = SelectedLabIncharge;
                        if (((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].LabPersonId > 0)
                        {
                            obj.SelectedIncharge = SelectedLabIncharge.FirstOrDefault(p => p.ID == ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].LabPersonId);
                        }
                        else
                        {
                            obj.SelectedIncharge = SelectedLabIncharge.FirstOrDefault(p => p.ID == 0);
                        }

                        obj.IsEnabled = false;
                        //ThawList.Add(((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i]);

                        if (obj.SemenWashID == 0 || obj.SemenWashID == SemenWashID || obj.SemenWashID == null)
                        {
                            ThawDeList.Add(obj);
                            ThawItemList.Add(((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i]);
                        }


                        //var result = from x in ThawDeList
                        //             where x.ThawingDate == ThawDate 
                        //             select x.ThawingTime;
                        //ThawTime = (DateTime?)result;

                        //}
                    }

                    dgThawingDetilsGrid.ItemsSource = null;
                    dgThawingDetilsGrid.ItemsSource = ThawDeList;
                    dgThawingDetilsGrid.UpdateLayout();







                    //dgVitrificationDetilsGrid.ItemsSource = null;
                    //dgVitrificationDetilsGrid.ItemsSource = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremFreezingDetails;
                    //dgVitrificationDetilsGrid.UpdateLayout();
                }
            };
            client.ProcessAsync(bizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

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


        private List<MasterListItem> _ProgressionList = new List<MasterListItem>();
        public List<MasterListItem> ProgressionList
        {
            get
            {
                return _ProgressionList;
            }
            set
            {
                _ProgressionList = value;
            }
        }

        private List<MasterListItem> _AgglutinationList = new List<MasterListItem>();
        public List<MasterListItem> AgglutinationList
        {
            get
            {
                return _AgglutinationList;
            }
            set
            {
                _AgglutinationList = value;
            }
        }

        private List<MasterListItem> _DoctorList = new List<MasterListItem>();
        public List<MasterListItem> DoctorList
        {
            get
            {
                return _DoctorList;
            }
            set
            {
                _DoctorList = value;
            }
        }

        private List<MasterListItem> _WitnessedList = new List<MasterListItem>();
        public List<MasterListItem> WitnessedList
        {
            get
            {
                return _WitnessedList;
            }
            set
            {
                _WitnessedList = value;
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
                        FillLabIncharge();
                        //FillLabPerson();
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

        private void FillLabIncharge()
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
                    SelectedLabIncharge = objList;
                    fillPlan();


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
                        //FillThawingDetails();
                        //FillProgression();
                        FillEmbryologist();
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

        //private void FillProgression()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();

        //    BizAction.MasterTable = MasterTableNameList.M_IVF_Progression;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, args) =>
        //    {
        //        if (args.Error == null && args.Result != null)
        //        {
        //            ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
        //            ProgressionList = ((clsGetMasterListBizActionVO)args.Result).MasterList;

        //            //List<MasterListItem> objList = new List<MasterListItem>();
        //            //objList.Add(new MasterListItem(0, "- Select -"));
        //            //objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
        //            //cmbProgression.ItemsSource = null;
        //            //cmbProgression.ItemsSource = objList;
        //            //cmbProgression.SelectedItem = objList[0];
        //        }

        //        if (this.DataContext != null)
        //        {
        //            //cmbAbstience.SelectedValue = ((cls_IVFDashboard_SemenVO)this.DataContext).AbstinenceID;
        //        }
        //        fillAgglutination();
        //    };
        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}
        //private void fillAgglutination()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.MasterTable = MasterTableNameList.M_IVF_Agglutination;
        //    BizAction.Parent = new KeyValue();
        //    BizAction.Parent.Key = "1";
        //    BizAction.Parent.Value = "Status";
        //    BizAction.MasterList = new List<MasterListItem>();
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, args) =>
        //    {
        //        if (args.Error == null && args.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();

        //            ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
        //            AgglutinationList = ((clsGetMasterListBizActionVO)args.Result).MasterList;

        //            //objList.Add(new MasterListItem(0, "-- Select --"));
        //            //objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
        //            //cmbAgglutination.ItemsSource = null;
        //            //cmbAgglutination.ItemsSource = objList;
        //            //cmbAgglutination.SelectedItem = objList[0];
        //        }

        //        if (this.DataContext != null)
        //        {
        //            //cmbSrcTreatmentPlan.SelectedValue = ((clsVO)this.DataContext).;
        //        }
        //        FillEmbryologist();

        //    };

        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}
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
                    DoctorList = objList;

                    //((clsGetMasterListBizActionVO)arg.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                    //DoctorList = ((clsGetMasterListBizActionVO)arg.Result).MasterList;

                    //List<MasterListItem> objList = new List<MasterListItem>();
                    //objList.Add(new MasterListItem(0, "-- Select --"));
                    //objList.AddRange(((clsGetEmbryologistBizActionVO)arg.Result).MasterList);
                    //cmbPreparedBy.ItemsSource = null;
                    //cmbPreparedBy.ItemsSource = objList;
                    //cmbPreparedBy.SelectedItem = objList[0];
                }

                FillWitnessed();
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillWitnessed()
        {
            clsGetStaffMasterDetailsBizActionVO BizAction = new clsGetStaffMasterDetailsBizActionVO();
            BizAction.StaffMasterList = new List<clsStaffMasterVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    if (((clsGetStaffMasterDetailsBizActionVO)e.Result).StaffMasterList != null)
                    {
                        clsGetStaffMasterDetailsBizActionVO result = e.Result as clsGetStaffMasterDetailsBizActionVO;

                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        foreach (var item in result.StaffMasterList)
                        {
                            MasterListItem Obj = new MasterListItem();
                            Obj.ID = item.ID;
                            Obj.Description = (item.FirstName + " " + item.MiddleName + " " + item.LastName);
                            Obj.Status = item.Status;
                            objList.Add(Obj);
                        }

                        WitnessedList = objList;

                        //cmbWitnessedBy.ItemsSource = null;
                        //cmbWitnessedBy.ItemsSource = objList;
                        //cmbWitnessedBy.SelectedItem = objList[0];
                    }
                }
                //FillSpermFreezingDetails();
                FillThawingDetails();
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //Client.ProcessAsync(BizAction, App.SessionUser);
            Client.CloseAsync();
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
            wait.Show();
            try
            {
                cls_NewGetSpremThawingBizActionVO bizAction = new cls_NewGetSpremThawingBizActionVO();
                bizAction.MalePatientID = CoupleDetails.MalePatient.PatientID;
                bizAction.MalePatientUnitID = CoupleDetails.MalePatient.UnitId;
                //bizAction.IsThawDetails = true;
                bizAction.IsFromIUI = true;
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

                        foreach (var item in ThawList)
                        {
                            if (item.PlanTherapyID > 0 && item.PlanTherapyUnitID > 0)
                                item.ThawedFrom = "IUI";
                            else
                                item.ThawedFrom = "Semen Preparation";
                        }

                        dgFreezingThawingDetils.ItemsSource = null;
                        dgFreezingThawingDetils.ItemsSource = ThawList;
                        dgFreezingThawingDetils.UpdateLayout();

                        //for (int i = 0; i < ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList.Count; i++)
                        //{
                        //    if (((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].IsFreezed == true)
                        //    {
                        //        ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].IsEnabled = false;
                        //    }
                        //    else
                        //    {
                        //        ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].IsEnabled = true;
                        //    }
                        //    if (((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].LabPersonId > 0)
                        //    {
                        //        ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].SelectedIncharge = SelectedLabIncharge.FirstOrDefault(p => p.ID == ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].LabPersonId);
                        //    }
                        //    else
                        //    {
                        //        ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].SelectedIncharge = SelectedLabIncharge.FirstOrDefault(p => p.ID == 0);
                        //    }
                        //    ThawDetailsList.Add(((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i]);
                        //}
                        //dgThawingDetilsGrid.ItemsSource = null;
                        //dgThawingDetilsGrid.ItemsSource = ThawDetailsList;
                        //dgThawingDetilsGrid.UpdateLayout();

                        wait.Close();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();

                        wait.Close();
                    }
                };
                client.ProcessAsync(bizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
                wait.Close();
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
                //cls_GetSpremFreezingDetilsForThawingBizActionVO bizAction = new cls_GetSpremFreezingDetilsForThawingBizActionVO();
                //bizAction.ID = ((cls_NewThawingDetailsVO)dgThawingDetilsGrid.SelectedItem).FreezingID;
                //bizAction.UnitID = ((cls_NewThawingDetailsVO)dgThawingDetilsGrid.SelectedItem).FreezingUnitID;
                //bizAction.SpremFreezingDetailsVO.SpremNo = ((cls_NewThawingDetailsVO)dgThawingDetilsGrid.SelectedItem).SpremNo;
                //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //client.ProcessCompleted += (s, arg) =>
                //{
                //    if (arg.Error == null && arg.Result != null && ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremFreezingDetails != null)
                //    {
                //        dgVitrificationDetilsGrid.ItemsSource = null;
                //        dgVitrificationDetilsGrid.ItemsSource = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremFreezingDetails;
                //        dgVitrificationDetilsGrid.UpdateLayout();
                //    }
                //};
                //client.ProcessAsync(bizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                //client.CloseAsync();
            }

        }



        private void cmbProgression_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text.Trim() != "" && (!((TextBox)sender).Text.IsItNumber()) && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtFloatNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValueDouble() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtFloatNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            this.SetCommandButtonState("New");
            //objSparmVO = new cls_NewSpremFreezingMainVO();
            //// CmbCycleCode.IsEnabled = true;
            //txtNoofVials.IsEnabled = true;
            //clearUI();
            //cyclecode = string.Empty;
            // getCycleCode();
            //COMENTED BY ROHINI DATED 29.2.16
            // FillGrid();
            //ISEdit = false;
            //isnew = true;

            //SpremNo = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).SpremNostr,
            //   UintID = ((clsNew_SpremFreezingVO)dgVitrificationDetilsGrid.SelectedItem).UnitID,




            FillSpermFreezingDetails();
            ThawDeList.Clear();
            dgThawingDetilsGrid.ItemsSource = null;
            dgThawingDetilsGrid.UpdateLayout();

            //ThawDeList = new ObservableCollection<cls_NewGetSpremThawingBizActionVO>();
            //ThawDeList.Add(new cls_NewGetSpremThawingBizActionVO()
            //{
            //    SpremNo = "",
            //    UintID = 0,
            //    ThawingDate = DateTime.Now,
            //    ThawingTime = DateTime.Now,
            //    TotalSpearmCount = 0,
            //    Motility = 0,
            //    ProgressionID = 0,
            //    ProgressionListVO = ProgressionList,
            //    AbnormalSperm = 0,
            //    RoundCells = 0,
            //    AgglutinationID = 0,
            //    AgglutinationListVO = AgglutinationList,
            //    Comments = "",
            //    DoctorID = 0,
            //    DoctorListVO = DoctorList,
            //    WitnessedID = 0,
            //    WitnessedListVO = WitnessedList
            //});

            //dgThawingDetilsGrid.ItemsSource = ThawDeList;
            //dgThawingDetilsGrid.UpdateLayout();

            CmdSave.Content = "Save";
            objAnimation.Invoke(RotationType.Forward);
        }

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            if (dgFreezingThawingDetils.SelectedItem != null)
            {
                this.DialogResult = true;

                SpermThaw = new SpermThawingForDashboard();
                //SpermThaw = new cls_NewGetSpremThawingBizActionVO();
                //SpermThaw.dgFreezingThawingDetils= dgFreezingThawingDetils.SelectedItem ;
                //SpermThaw1.ThawList = List< (cls_NewThawingDetailsVO)>dgFreezingThawingDetils.SelectedItem;
                //SpermThaw1.ThawList= (cls_NewGetSpremThawingBizActionVO)dgFreezingThawingDetils.SelectedItem;
                //dgFreezingThawingDetils.ItemsSource
                //SpermThaw.ThawList = (List<cls_NewThawingVO>)((cls_NewGetSpremThawingBizActionVO)dgFreezingThawingDetils.SelectedItem);

                string code = ((cls_NewThawingDetailsVO)dgFreezingThawingDetils.SelectedItem).Code;
                SpermThaw.spermFrezingCode = ((cls_NewThawingDetailsVO)dgFreezingThawingDetils.SelectedItem).Code;
                //SpermThaw.ThawList = ((List<cls_NewThawingDetailsVO>)dgFreezingThawingDetils.SelectedItem);

                SpermThaw.ThawingID = ((cls_NewThawingDetailsVO)dgFreezingThawingDetils.SelectedItem).ID;

                if (OKButtonCode_Click != null)
                    OKButtonCode_Click(this, new RoutedEventArgs());

            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            long ID, UnitID, PatientID, PatientUnitID;
            string CollectionNO;
            if (dgFreezingThawingDetils.SelectedItem != null)
            {
                ID = ((cls_NewThawingDetailsVO)dgFreezingThawingDetils.SelectedItem).ID;
                UnitID = ((cls_NewThawingDetailsVO)dgFreezingThawingDetils.SelectedItem).UnitID;
                CollectionNO = ((cls_NewThawingDetailsVO)dgFreezingThawingDetils.SelectedItem).SpremNo.ToString();
                PatientID = CoupleDetails.MalePatient.PatientID;
                PatientUnitID = CoupleDetails.CoupleUnitId;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/IVFDashboard/IVFDashboard_SpermThawing.aspx?ID=" + ID + "&UnitID=" + UnitID + "&MalePatientID=" + PatientID + "&CollectionNO=" + CollectionNO + "&MalePatientUnitID=" + PatientUnitID + "&PrintFomatID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID), "_blank");
            }
        }

        List<cls_NewThawingDetailsVO> ThawForPrint = new List<cls_NewThawingDetailsVO>();

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (dgFreezingThawingDetils.SelectedItem != null)
            {               
                CheckBox chk = (CheckBox)sender;
                if (chk.IsChecked == true)
                {
                    ThawForPrint.Add(((cls_NewThawingDetailsVO)dgFreezingThawingDetils.SelectedItem));
                }
                else
                {
                    ThawForPrint.Remove(((cls_NewThawingDetailsVO)dgFreezingThawingDetils.SelectedItem));
                }
            }
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder strSperm = new StringBuilder();
            long ID, UnitID=0, PatientID, PatientUnitID;
            string CollectionNO;
            if (ThawForPrint.Count > 0)
            {
                foreach (var item in ThawForPrint)
                {
                    UnitID = item.UnitID;
                    if (strSperm.ToString().Length > 0)
                        strSperm.Append(",");
                    strSperm.Append(item.SpremNo);
                }

                ID = ((cls_NewThawingDetailsVO)dgFreezingThawingDetils.SelectedItem).ID;
                //UnitID = ((cls_NewThawingDetailsVO)dgFreezingThawingDetils.SelectedItem).UnitID;
                CollectionNO = strSperm.ToString();
                PatientID = CoupleDetails.MalePatient.PatientID;
                PatientUnitID = CoupleDetails.CoupleUnitId;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/IVFDashboard/IVFDashboard_SpermThawing.aspx?ID=" + ID + "&UnitID=" + UnitID + "&MalePatientID=" + PatientID + "&CollectionNO=" + CollectionNO + "&MalePatientUnitID=" + PatientUnitID + "&PrintFomatID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID), "_blank");
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select samples.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }


        }


    }
}





//public clsCoupleVO CoupleDetails;
//       public SpermThawingForDashboard()
//       {
//           InitializeComponent();
//       }
//       private void dgVitrificationDetilsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
//       {

//       }

//       private void CmdSave_Click(object sender, RoutedEventArgs e)
//       {
//           if (ThawDetailsList.Count > 0)
//           {
//               string msgTitle = "Palash";
//               string msgText = "Are You Sure \n You Want To Save Semen Thawing Details";
//               MessageBoxControl.MessageBoxChildWindow msgWin =
//                   new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
//               msgWin.OnMessageBoxClosed += (result) =>
//               {
//                   if (result == MessageBoxResult.Yes)
//                   {
//                       SaveThawing();
//                   }
//               };
//               msgWin.Show();
//           }
//           else
//           {
//               string msgTitle = "Palash";
//               string msgText = "No Details Available";
//               MessageBoxControl.MessageBoxChildWindow msgWin =
//                   new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

//               msgWin.Show();
//           }
//       }
//       public Boolean IsEdit { get; set; }
//       private void SaveThawing()
//       {
//           try
//           {
//               wait.Show();
//               clsAddUpdateSpermThawingBizActionVO BizAction = new clsAddUpdateSpermThawingBizActionVO();
//               BizAction.ThawingList = new List<cls_NewThawingDetailsVO>();
//               BizAction.IsNewForm = true;
//               BizAction.ThawingList = ThawList;
//               #region Service Call (Check Validation)
//               Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
//               PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
//               client.ProcessCompleted += (s, args) =>
//               {
//                   if (args.Error == null && args.Result != null)
//                   {
//                       string txtmsg = "";
//                       if (IsEdit == true)
//                       {
//                           txtmsg = "Semen Thawing Details Updated Successfully";
//                       }
//                       else
//                       {
//                           txtmsg = "Semen Thawing Details Saved Successfully";
//                       }

//                       MessageBoxControl.MessageBoxChildWindow msgW1 =
//                                   new MessageBoxControl.MessageBoxChildWindow("Palash", txtmsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
//                       msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
//                       {
//                           FillThawingDetails();
//                       };
//                       msgW1.Show();
//                       wait.Close();
//                   }
//                   else
//                   {
//                       wait.Close();
//                   }
//               };
//               client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
//               client.CloseAsync();
//               #endregion

//           }
//           catch (Exception ex)
//           {
//               wait.Close();
//           }

//       }
//       private void CmdClose_Click(object sender, RoutedEventArgs e)
//       {
//           this.DialogResult = false;
//       }
//       private bool IsPatientExist;
//       public string Action { get; set; }
//       public string ModuleName { get; set; }
//       UIElement myData = null;
//       protected override void OnClosed(EventArgs e)
//       {
//           base.OnClosed(e);
//           Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
//       }

//       private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
//       {
//           fillCanID();
//           ThawDetailsList = new PagedSortableCollectionView<cls_NewThawingDetailsVO>();
//           ThawDetailsListPageSize = 15;


//           this.Title = "Semen Thawing  :-(Name- " + CoupleDetails.MalePatient.FirstName +
//                  " " + CoupleDetails.MalePatient.LastName + ")";
//       }

//       private List<MasterListItem> _CanList = new List<MasterListItem>();
//       public List<MasterListItem> CanList
//       {
//           get
//           {
//               return _CanList;
//           }
//           set
//           {
//               _CanList = value;
//           }
//       }

//       private List<MasterListItem> _Plan = new List<MasterListItem>();
//       public List<MasterListItem> SelectedPLan
//       {
//           get
//           {
//               return _Plan;
//           }
//           set
//           {
//               _Plan = value;
//           }
//       }

//       private List<MasterListItem> _LabIncharge = new List<MasterListItem>();
//       public List<MasterListItem> SelectedLabIncharge
//       {
//           get
//           {
//               return _LabIncharge;
//           }
//           set
//           {
//               _LabIncharge = value;
//           }
//       }
//       WaitIndicator wait = new WaitIndicator();
//       #region Fill Master Item
//       private void fillCanID()
//       {
//           try
//           {
//               clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
//               BizAction.MasterTable = MasterTableNameList.M_IVFCanMaster;
//               BizAction.Parent = new KeyValue();
//               BizAction.Parent.Key = "1";
//               BizAction.Parent.Value = "Status";
//               BizAction.MasterList = new List<MasterListItem>();
//               Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
//               PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
//               client.ProcessCompleted += (s, args) =>
//               {
//                   if (args.Error == null && args.Result != null)
//                   {
//                       CanList = ((clsGetMasterListBizActionVO)args.Result).MasterList;
//                       FillLabPerson();
//                   }
//               };
//               client.ProcessAsync(BizAction, new clsUserVO());
//               client.CloseAsync();
//           }
//           catch (Exception ex)
//           {
//               wait.Close();
//           }
//       }

//       private void FillLabPerson()
//       {
//           clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
//           BizAction.MasterList = new List<MasterListItem>();
//           BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
//           BizAction.DepartmentId = 0;
//           Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
//           PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
//           client.ProcessCompleted += (s, arg) =>
//           {
//               if (arg.Error == null && arg.Result != null)
//               {
//                   List<MasterListItem> objList = new List<MasterListItem>();
//                   objList.Add(new MasterListItem(0, "-- Select --"));
//                   objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);
//                   SelectedLabIncharge = objList;
//                   fillPlan();
//                   if (this.DataContext != null)
//                   {
//                       //   cmbLabPerson.SelectedValue = ((cls_NewThawingDetailsVO)this.DataContext).LabInchargeId;
//                   }
//               }
//           };
//           client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
//           client.CloseAsync();
//       }

//       private void fillPlan()
//       {
//           try
//           {
//               clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
//               BizAction.MasterTable = MasterTableNameList.M_PostThawingPlan;
//               BizAction.Parent = new KeyValue();
//               BizAction.Parent.Key = "1";
//               BizAction.Parent.Value = "Status";
//               BizAction.MasterList = new List<MasterListItem>();
//               Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
//               PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
//               client.ProcessCompleted += (s, args) =>
//               {
//                   if (args.Error == null && args.Result != null)
//                   {
//                       ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
//                       SelectedPLan = ((clsGetMasterListBizActionVO)args.Result).MasterList;
//                       FillThawingDetails();
//                   }

//               };
//               client.ProcessAsync(BizAction, new clsUserVO());
//               client.CloseAsync();
//           }
//           catch (Exception ex)
//           {
//               wait.Close();
//           }
//       }
//       #endregion
//       public PagedSortableCollectionView<cls_NewThawingDetailsVO> ThawDetailsList { get; private set; }
//       public int ThawDetailsListPageSize
//       {
//           get
//           {
//               return ThawDetailsList.PageSize;
//           }
//           set
//           {
//               if (value == ThawDetailsList.PageSize) return;
//               ThawDetailsList.PageSize = value;
//           }
//       }
//       public List<cls_NewThawingDetailsVO> ThawList;
//       private void FillThawingDetails()
//       {
//           try
//           {
//               cls_NewGetSpremThawingBizActionVO bizAction = new cls_NewGetSpremThawingBizActionVO();
//               bizAction.MalePatientID = CoupleDetails.MalePatient.PatientID;
//               bizAction.MalePatientUnitID = CoupleDetails.MalePatient.UnitId;
//               bizAction.IsThawDetails = true;
//               bizAction.IsPagingEnabled = true;
//               bizAction.StartIndex = ThawDetailsList.PageIndex * ThawDetailsList.PageSize;
//               bizAction.MaximumRows = ThawDetailsList.PageSize;
//               Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
//               PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
//               client.ProcessCompleted += (s, arg) =>
//               {
//                   if (arg.Error == null && arg.Result != null && ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList != null)
//                   {
//                       ThawDetailsList.Clear();
//                       ThawList = ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList;
//                       ThawDetailsList.TotalItemCount = (int)((cls_NewGetSpremThawingBizActionVO)arg.Result).TotalRows;
//                       for (int i = 0; i < ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList.Count; i++)
//                       {
//                           if (((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].IsFreezed == true)
//                           {
//                               ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].IsEnabled = false;
//                           }
//                           else
//                           {
//                               ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].IsEnabled = true;
//                           }
//                           if (((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].LabPersonId > 0)
//                           {
//                               ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].SelectedIncharge = SelectedLabIncharge.FirstOrDefault(p => p.ID == ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].LabPersonId);
//                           }
//                           else
//                           {
//                               ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].SelectedIncharge = SelectedLabIncharge.FirstOrDefault(p => p.ID == 0);
//                           }
//                           ThawDetailsList.Add(((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i]);
//                       }
//                       dgThawingDetilsGrid.ItemsSource = null;
//                       dgThawingDetilsGrid.ItemsSource = ThawDetailsList;
//                       dgThawingDetilsGrid.UpdateLayout();
//                   }
//                   else
//                   {
//                       MessageBoxControl.MessageBoxChildWindow msgW1 =
//                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
//                       msgW1.Show();
//                   }
//               };
//               client.ProcessAsync(bizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
//               client.CloseAsync();
//           }
//           catch (Exception)
//           {
//               throw;
//           }
//       }
//       private void cmbPlanForSperms_SelectionChanged(object sender, SelectionChangedEventArgs e)
//       {

//       }

//       private void cmbLabIncharge1_SelectionChanged(object sender, SelectionChangedEventArgs e)
//       {
//           for (int i = 0; i < ThawDetailsList.Count; i++)
//           {
//               if (i == dgThawingDetilsGrid.SelectedIndex)
//               {
//                   if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
//                   {
//                       ThawDetailsList[i].LabPersonId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
//                   }
//               }
//           }
//       }

//       private void dgThawingDetilsGrid_LoadingRow(object sender, DataGridRowEventArgs e)
//       {

//           //cls_NewThawingDetailsVO ThawRow = (cls_NewThawingDetailsVO)e.Row.DataContext;
//           //if (ThawRow.IsFreezed == true)
//           //{
//           //     e.Row.IsEnabled = false;
//           //    //dgThawingDetilsGrid.Columns[4].IsReadOnly = true;
//           //    //dgThawingDetilsGrid.Columns[5].IsReadOnly = true;
//           //    //dgThawingDetilsGrid.Columns[6].IsReadOnly = true;
//           //}
//           //else
//           //{
//           //    e.Row.IsEnabled = true;
//           //    //dgThawingDetilsGrid.Columns[4].IsReadOnly = false;
//           //    //dgThawingDetilsGrid.Columns[5].IsReadOnly = false;
//           //    //dgThawingDetilsGrid.Columns[6].IsReadOnly = false;
//           //}
//       }

//       private void dgThawingDetilsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
//       {
//           if (dgThawingDetilsGrid.SelectedItem != null)
//           {
//               cls_GetSpremFreezingDetilsForThawingBizActionVO bizAction = new cls_GetSpremFreezingDetilsForThawingBizActionVO();
//               bizAction.ID = ((cls_NewThawingDetailsVO)dgThawingDetilsGrid.SelectedItem).FreezingID;
//               bizAction.UnitID = ((cls_NewThawingDetailsVO)dgThawingDetilsGrid.SelectedItem).FreezingUnitID;
//               bizAction.SpremFreezingDetailsVO.SpremNo = ((cls_NewThawingDetailsVO)dgThawingDetilsGrid.SelectedItem).SpremNo;
//               Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
//               PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
//               client.ProcessCompleted += (s, arg) =>
//               {
//                   if (arg.Error == null && arg.Result != null && ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremFreezingDetails != null)
//                   {
//                       dgVitrificationDetilsGrid.ItemsSource = null;
//                       dgVitrificationDetilsGrid.ItemsSource = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremFreezingDetails;
//                       dgVitrificationDetilsGrid.UpdateLayout();
//                   }
//               };
//               client.ProcessAsync(bizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
//               client.CloseAsync();
//           }

//       }