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
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.DashBoardVO;
using System.Collections.ObjectModel;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.Collections;
using System.Reflection;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmBirthDetails : UserControl
    {

        public clsCoupleVO CoupleDetails;
        public clsPlanTherapyVO PlanTherapyVO;
        public long PlanTherapyID;
        public long PlanTherapyUnitID;
        public long ChildID;
        public DateTime? PregnancyAchivedDate;
        public bool? IsPregnancyAchived;
        public long FetalHeartCount = 0;
        WaitIndicator wait = new WaitIndicator();
        List<clsIVFDashboard_BirthDetailsVO> ChildDetailList = null;
        public bool IsSurrogate = false;   

        private List<MasterListItem> _Activity = new List<MasterListItem>();
        public List<MasterListItem> Activity
        {
            get
            {
                return _Activity;
            }
            set
            {
                _Activity = value;
            }
        }

        private List<MasterListItem> _Pulse = new List<MasterListItem>();
        public List<MasterListItem> Pulse
        {
            get
            {
                return _Pulse;
            }
            set
            {
                _Pulse = value;
            }
        }

        private List<MasterListItem> _Grimace = new List<MasterListItem>();
        public List<MasterListItem> Grimace
        {
            get
            {
                return _Grimace;
            }
            set
            {
                _Grimace = value;
            }
        }

        private List<MasterListItem> _Appearance = new List<MasterListItem>();
        public List<MasterListItem> Appearance
        {
            get
            {
                return _Appearance;
            }
            set
            {
                _Appearance = value;
            }
        }

        private List<MasterListItem> _Respiration = new List<MasterListItem>();
        public List<MasterListItem> Respiration
        {
            get
            {
                return _Respiration;
            }
            set
            {
                _Respiration = value;
            }
        }

        private List<MasterListItem> _APGARScore = new List<MasterListItem>();
        public List<MasterListItem> APGARScore
        {
            get
            {
                return _APGARScore;
            }
            set
            {
                _APGARScore = value;
            }
        }

        private List<MasterListItem> _DeliveryType = new List<MasterListItem>();
        public List<MasterListItem> DeliveryType
        {
            get
            {
                return _DeliveryType;
            }
            set
            {
                _DeliveryType = value;
            }
        }

        public frmBirthDetails()
        {
            InitializeComponent();
            //dgchildDetails.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgchildDetails_CellEditEnded);
        }

        //void dgchildDetails_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        //{
        //    if (e.Column.DisplayIndex == 5)
        //    {
        //        ActivityPoint = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).Point;
        //        ((clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).ActivityPoint = ActivityPoint;


        //        APGARScorePoint = ActivityPoint + PulsePoint + GrimacePOint + AppearancePoint + RespirationPOint;
        //        if (APGARScorePoint >= 0 && APGARScorePoint <= 3)
        //        {
        //            ((clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).APGARScore = APGARScorePoint;
        //            ((clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).SelectedConclusion.Description = "abcd";

        //            //((clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).ConclusionList = APGARScore;
        //            //((clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).SelectedConclusion = APGARScore.FirstOrDefault(p => p.ID == 1);
        //            //((clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).APGARScoreID = 1;
        //        }
        //        if (APGARScorePoint >= 4 && APGARScorePoint <= 6)
        //        {
        //            ((clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).APGARScore = APGARScorePoint;
        //            ((clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).ConclusionList = APGARScore;
        //            ((clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).SelectedConclusion = APGARScore.FirstOrDefault(p => p.ID == 2);
        //            ((clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).APGARScoreID = 2;


        //        }
        //        if (APGARScorePoint >= 7 && APGARScorePoint <= 10)
        //        {
        //            ((clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).APGARScore = APGARScorePoint;
        //            ((clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).ConclusionList = APGARScore;
        //            ((clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).SelectedConclusion = APGARScore.FirstOrDefault(p => p.ID == 3);
        //            ((clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).APGARScoreID = 3;

        //        }
        //    }
        //    //throw new NotImplementedException();
        //}

        private void FillActivity()
        {
            clsIVFDashboard_GetBirthDetailsMasterListBizActionVO BizAction = new clsIVFDashboard_GetBirthDetailsMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVf_BirthActivity;
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
                    objList.AddRange(((clsIVFDashboard_GetBirthDetailsMasterListBizActionVO)args.Result).MasterList);
                    Activity = objList;

                    FillPulse();
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillPulse()
        {
            clsIVFDashboard_GetBirthDetailsMasterListBizActionVO BizAction = new clsIVFDashboard_GetBirthDetailsMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVf_BirthPulse;
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
                    objList.AddRange(((clsIVFDashboard_GetBirthDetailsMasterListBizActionVO)args.Result).MasterList);
                    Pulse = objList;

                    FillGrimace();
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillGrimace()
        {
            clsIVFDashboard_GetBirthDetailsMasterListBizActionVO BizAction = new clsIVFDashboard_GetBirthDetailsMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVf_BirthGrimace;
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
                    objList.AddRange(((clsIVFDashboard_GetBirthDetailsMasterListBizActionVO)args.Result).MasterList);
                    Grimace = objList;

                    FillAppearance();
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillAppearance()
        {
            clsIVFDashboard_GetBirthDetailsMasterListBizActionVO BizAction = new clsIVFDashboard_GetBirthDetailsMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVf_BirthAppearance;
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
                    objList.AddRange(((clsIVFDashboard_GetBirthDetailsMasterListBizActionVO)args.Result).MasterList);
                    Appearance = objList;

                    FillRespiration();
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillRespiration()
        {
            clsIVFDashboard_GetBirthDetailsMasterListBizActionVO BizAction = new clsIVFDashboard_GetBirthDetailsMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVf_BirthRespiration;
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
                    objList.AddRange(((clsIVFDashboard_GetBirthDetailsMasterListBizActionVO)args.Result).MasterList);
                    Respiration = objList;

                    FillAPGARScore();
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillAPGARScore()
        {
            clsIVFDashboard_GetBirthDetailsMasterListBizActionVO BizAction = new clsIVFDashboard_GetBirthDetailsMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVf_BirthAPGARScore;
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
                    objList.AddRange(((clsIVFDashboard_GetBirthDetailsMasterListBizActionVO)args.Result).MasterList);
                    APGARScore = objList;

                    FillDeliveryType();
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillDeliveryType()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVf_BirthDeliveryType;
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
                        DeliveryType = ((clsGetMasterListBizActionVO)args.Result).MasterList;

                        FillDetails();
                    }
                    if (this.DataContext != null)
                    {
                        //cmbSrcTreatmentPlan.SelectedValue = ((clsVO)this.DataContext).;
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

        private void FillDetails()
        {
            try
            {
                wait.Show();
                clsIVFDashboard_GetBirthDetailsListBizActionVO BizAction = new clsIVFDashboard_GetBirthDetailsListBizActionVO();
                BizAction.BirthDetails = new clsIVFDashboard_BirthDetailsVO();
                BizAction.BirthDetails.TherapyID = PlanTherapyID;
                BizAction.BirthDetails.TherapyUnitID = PlanTherapyUnitID;
                if (CoupleDetails != null)
                {
                    BizAction.BirthDetails.PatientID = CoupleDetails.FemalePatient.PatientID;
                    BizAction.BirthDetails.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsIVFDashboard_GetBirthDetailsListBizActionVO)arg.Result).BirthDetailsList != null && ((clsIVFDashboard_GetBirthDetailsListBizActionVO)arg.Result).BirthDetailsList.Count > 0)
                        {
                            ChildDetailList = new List<clsIVFDashboard_BirthDetailsVO>();
                            foreach (var item in ((clsIVFDashboard_GetBirthDetailsListBizActionVO)arg.Result).BirthDetailsList)
                            {
                                item.ActivityList = Activity;
                                if (Convert.ToInt64(item.ActivityID) > 0)
                                {
                                    item.SelectedActivity = Activity.FirstOrDefault(p => p.ID == Convert.ToInt64(item.ActivityID));
                                }
                                else
                                {
                                    item.SelectedActivity = Activity.FirstOrDefault(p => p.ID == 0);
                                }

                                item.PulseList = Pulse;
                                if (Convert.ToInt64(item.Pulse) > 0)
                                {
                                    item.SelectedPulse = Pulse.FirstOrDefault(p => p.ID == Convert.ToInt64(item.Pulse));
                                }
                                else
                                {
                                    item.SelectedPulse = Pulse.FirstOrDefault(p => p.ID == 0);
                                }

                                item.GrimaceList = Grimace;
                                if (Convert.ToInt64(item.Grimace) > 0)
                                {
                                    item.SelectedGrimace = Grimace.FirstOrDefault(p => p.ID == Convert.ToInt64(item.Grimace));
                                }
                                else
                                {
                                    item.SelectedGrimace = Grimace.FirstOrDefault(p => p.ID == 0);
                                }

                                item.AppearanceList = Appearance;
                                if (Convert.ToInt64(item.Appearance) > 0)
                                {
                                    item.SelectedAppearance = Appearance.FirstOrDefault(p => p.ID == Convert.ToInt64(item.Appearance));
                                }
                                else
                                {
                                    item.SelectedAppearance = Appearance.FirstOrDefault(p => p.ID == 0);
                                }

                                item.RespirationList = Respiration;
                                if (Convert.ToInt64(item.Respiration) > 0)
                                {
                                    item.SelectedRespiration = Respiration.FirstOrDefault(p => p.ID == Convert.ToInt64(item.Respiration));
                                }
                                else
                                {
                                    item.SelectedRespiration = Respiration.FirstOrDefault(p => p.ID == 0);
                                }

                                item.ConclusionList = APGARScore;
                                if (Convert.ToInt64(item.APGARScoreID) > 0)
                                {
                                    item.SelectedConclusion = APGARScore.FirstOrDefault(p => p.ID == Convert.ToInt64(item.APGARScoreID));
                                }
                                else
                                {
                                    item.SelectedConclusion = APGARScore.FirstOrDefault(p => p.ID == 0);
                                }

                                item.TypeOfDeliveryList = DeliveryType;
                                if (Convert.ToInt64(item.DeliveryTypeID) > 0)
                                {
                                    item.SelectedTypeOfDelivery = DeliveryType.FirstOrDefault(p => p.ID == Convert.ToInt64(item.DeliveryTypeID));
                                }
                                else
                                {
                                    item.SelectedTypeOfDelivery = DeliveryType.FirstOrDefault(p => p.ID == 0);
                                }


                                ChildDetailList.Add(item);
                            }
                            dgchildDetails.ItemsSource = null;
                            dgchildDetails.ItemsSource = ChildDetailList;
                        }

                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                wait.Close();
            }
            catch (Exception ex)
            {
                wait.Close();
                throw ex;

            }



            //try
            //{
            //    clsIVFDashboard_GetBirthDetailsListBizActionVO BizAction = new clsIVFDashboard_GetBirthDetailsListBizActionVO();
            //    BizAction.BirthDetails = new clsIVFDashboard_BirthDetailsVO();
            //    BizAction.BirthDetails.TherapyID = PlanTherapyID;
            //    BizAction.BirthDetails.TherapyUnitID = PlanTherapyUnitID;
            //    if (CoupleDetails != null)
            //    {
            //        BizAction.BirthDetails.PatientID = CoupleDetails.FemalePatient.PatientID;
            //        BizAction.BirthDetails.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            //    }

            //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            //    client.ProcessCompleted += (s, arg) =>
            //    {
            //        if (arg.Error == null && arg.Result != null)
            //        {
            //            if (((clsIVFDashboard_GetBirthDetailsListBizActionVO)arg.Result).BirthDetailsList != null && ((clsIVFDashboard_GetBirthDetailsListBizActionVO)arg.Result).BirthDetailsList.Count > 0)
            //            {
            //                ChildDetailList = new List<clsIVFDashboard_BirthDetailsVO>();
            //                foreach (var item in ((clsIVFDashboard_GetBirthDetailsListBizActionVO)arg.Result).BirthDetailsList)
            //                {
            //                    item.ActivityList = Activity;
            //                    if (Convert.ToInt64(item.ActivityID) > 0)
            //                    {
            //                        item.SelectedActivity = Activity.FirstOrDefault(p => p.ID == Convert.ToInt64(item.ActivityID));
            //                    }
            //                    else
            //                    {
            //                        item.SelectedActivity = Activity.FirstOrDefault(p => p.ID == 0);
            //                    }

            //                    item.PulseList = Pulse;
            //                    if (Convert.ToInt64(item.Pulse) > 0)
            //                    {
            //                        item.SelectedPulse = Pulse.FirstOrDefault(p => p.ID == Convert.ToInt64(item.Pulse));
            //                    }
            //                    else
            //                    {
            //                        item.SelectedPulse = Pulse.FirstOrDefault(p => p.ID == 0);
            //                    }

            //                    item.GrimaceList = Grimace;
            //                    if (Convert.ToInt64(item.Grimace) > 0)
            //                    {
            //                        item.SelectedGrimace = Grimace.FirstOrDefault(p => p.ID == Convert.ToInt64(item.Grimace));
            //                    }
            //                    else
            //                    {
            //                        item.SelectedGrimace = Grimace.FirstOrDefault(p => p.ID == 0);
            //                    }

            //                    item.AppearanceList = Appearance;
            //                    if (Convert.ToInt64(item.Appearance) > 0)
            //                    {
            //                        item.SelectedAppearance = Appearance.FirstOrDefault(p => p.ID == Convert.ToInt64(item.Appearance));
            //                    }
            //                    else
            //                    {
            //                        item.SelectedAppearance = Appearance.FirstOrDefault(p => p.ID == 0);
            //                    }

            //                    item.RespirationList = Respiration;
            //                    if (Convert.ToInt64(item.Respiration) > 0)
            //                    {
            //                        item.SelectedRespiration = Respiration.FirstOrDefault(p => p.ID == Convert.ToInt64(item.Respiration));
            //                    }
            //                    else
            //                    {
            //                        item.SelectedRespiration = Respiration.FirstOrDefault(p => p.ID == 0);
            //                    }

            //                    item.ConclusionList = APGARScore;
            //                    if (Convert.ToInt64(item.APGARScoreID) > 0)
            //                    {
            //                        item.SelectedConclusion = APGARScore.FirstOrDefault(p => p.ID == Convert.ToInt64(item.APGARScoreID));
            //                    }
            //                    else
            //                    {
            //                        item.SelectedConclusion = APGARScore.FirstOrDefault(p => p.ID == 0);
            //                    }

            //                    item.TypeOfDeliveryList = DeliveryType;
            //                    if (Convert.ToInt64(item.DeliveryTypeID) > 0)
            //                    {
            //                        item.SelectedTypeOfDelivery = DeliveryType.FirstOrDefault(p => p.ID == Convert.ToInt64(item.DeliveryTypeID));
            //                    }
            //                    else
            //                    {
            //                        item.SelectedTypeOfDelivery = DeliveryType.FirstOrDefault(p => p.ID == 0);
            //                    }


            //                    ChildDetailList.Add(item);
            //                }
            //            }

            //            dgchildDetails.ItemsSource = null;
            //            dgchildDetails.ItemsSource = ChildDetailList;


            //            wait.Close();


            //        }
            //    };
            //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //    client.CloseAsync();
            //    wait.Close();
            //}
            //catch (Exception ex)
            //{
            //    wait.Close();
            //}
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillActivity();
            if (IsSurrogate)
                dgchildDetails.Columns[0].Visibility = Visibility.Visible;
            else
                dgchildDetails.Columns[0].Visibility = Visibility.Collapsed;
        }

        //long ActivityPoint;
        //long PulsePoint;
        //long GrimacePOint;
        //long AppearancePoint;
        //long RespirationPOint;
        //long APGARScorePoint;
        private void cmbActivity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < ChildDetailList.Count; i++)
            {
                if (i == dgchildDetails.SelectedIndex)
                {
                    if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                    {
                        ChildDetailList[i].ActivityPoint = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).Point;
                        //ChildDetailList[i].ActivityPoint = ActivityPoint;
                    }

                    ChildDetailList[i].APGARScore = ChildDetailList[i].ActivityPoint + ChildDetailList[i].PulsePoint + ChildDetailList[i].GrimacePoint + ChildDetailList[i].AppearancePoint + ChildDetailList[i].RespirationPoint;
                    if (ChildDetailList[i].APGARScore >= 0 && ChildDetailList[i].APGARScore <= 3)
                    {
                        ChildDetailList[i].APGARScore = ChildDetailList[i].APGARScore;
                        ChildDetailList[i].Conclusion = "Severly Depressed";
                    }
                    else if (ChildDetailList[i].APGARScore >= 4 && ChildDetailList[i].APGARScore <= 6)
                    {
                        ChildDetailList[i].APGARScore = ChildDetailList[i].APGARScore;
                        ChildDetailList[i].Conclusion = "Moderately Depressed";
                    }
                    else if (ChildDetailList[i].APGARScore >= 7 && ChildDetailList[i].APGARScore <= 10)
                    {
                        ChildDetailList[i].APGARScore = ChildDetailList[i].APGARScore;
                        ChildDetailList[i].Conclusion = "Excellent Condition";
                    }
                }
            }



            ////APGARScorePoint = ActivityPoint + PulsePoint + GrimacePOint + AppearancePoint + RespirationPOint;
            //if (APGARScorePoint >= 0 && APGARScorePoint <= 3)
            //{
            //    for (int i = 0; i < ChildDetailList.Count; i++)
            //    {
            //        if (i == dgchildDetails.SelectedIndex)
            //        {
            //            ChildDetailList[i].APGARScore = APGARScorePoint;
            //            ChildDetailList[i].Conclusion = "Severly Depressed";
            //           // ChildDetailList[i].Conclusion = 7;

            //            //ChildDetailList[i].ConclusionList = APGARScore;
            //            //ChildDetailList[i].SelectedConclusion = APGARScore.FirstOrDefault(p => p.ID == 1);
            //            //ChildDetailList[i].APGARScoreID = 1;
            //        }
            //    }
            //}
            //if (APGARScorePoint >= 4 && APGARScorePoint <= 6)
            //{
            //    for (int i = 0; i < ChildDetailList.Count; i++)
            //    {
            //        if (i == dgchildDetails.SelectedIndex)
            //        {
            //            ChildDetailList[i].APGARScore = APGARScorePoint;
            //            ChildDetailList[i].Conclusion = "Moderately Depressed";

            //            //ChildDetailList[i].APGARScore = APGARScorePoint;
            //            //ChildDetailList[i].ConclusionList = APGARScore;
            //            //ChildDetailList[i].SelectedConclusion = APGARScore.FirstOrDefault(p => p.ID == 2);
            //            //ChildDetailList[i].APGARScoreID = 2;
            //        }
            //    }
            //}
            //if (APGARScorePoint >= 7 && APGARScorePoint <= 10)
            //{
            //    for (int i = 0; i < ChildDetailList.Count; i++)
            //    {
            //        if (i == dgchildDetails.SelectedIndex)
            //        {
            //            ChildDetailList[i].APGARScore = APGARScorePoint;
            //            ChildDetailList[i].Conclusion = "Excellent Condition";

            //            //ChildDetailList[i].APGARScore = APGARScorePoint;
            //            //ChildDetailList[i].ConclusionList = APGARScore;
            //            //ChildDetailList[i].SelectedConclusion = APGARScore.FirstOrDefault(p => p.ID == 3);
            //            //ChildDetailList[i].APGARScoreID = 3;
            //        }
            //    }
            //}
            dgchildDetails.ItemsSource = ChildDetailList;
            dgchildDetails.UpdateLayout();
        }

        private void cmbPulse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < ChildDetailList.Count; i++)
            {
                if (i == dgchildDetails.SelectedIndex)
                {
                    if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                    {
                        ChildDetailList[i].PulsePoint = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).Point;
                        //ChildDetailList[i].PulsePoint = PulsePoint;
                    }

                    ChildDetailList[i].APGARScore = ChildDetailList[i].ActivityPoint + ChildDetailList[i].PulsePoint + ChildDetailList[i].GrimacePoint + ChildDetailList[i].AppearancePoint + ChildDetailList[i].RespirationPoint;
                    if (ChildDetailList[i].APGARScore >= 0 && ChildDetailList[i].APGARScore <= 3)
                    {
                        ChildDetailList[i].APGARScore = ChildDetailList[i].APGARScore;
                        ChildDetailList[i].Conclusion = "Severly Depressed";
                    }
                    else if (ChildDetailList[i].APGARScore >= 4 && ChildDetailList[i].APGARScore <= 6)
                    {
                        ChildDetailList[i].APGARScore = ChildDetailList[i].APGARScore;
                        ChildDetailList[i].Conclusion = "Moderately Depressed";
                    }
                    else if (ChildDetailList[i].APGARScore >= 7 && ChildDetailList[i].APGARScore <= 10)
                    {
                        ChildDetailList[i].APGARScore = ChildDetailList[i].APGARScore;
                        ChildDetailList[i].Conclusion = "Excellent Condition";
                    }
                }
            }

            //APGARScorePoint = ActivityPoint + PulsePoint + GrimacePOint + AppearancePoint + RespirationPOint;
            //if (APGARScorePoint >= 0 && APGARScorePoint <= 3)
            //{
            //    for (int i = 0; i < ChildDetailList.Count; i++)
            //    {
            //        if (i == dgchildDetails.SelectedIndex)
            //        {
            //            ChildDetailList[i].APGARScore = APGARScorePoint;
            //            ChildDetailList[i].Conclusion = "Severly Depressed";
            //            // ChildDetailList[i].Conclusion = 7;

            //            //ChildDetailList[i].ConclusionList = APGARScore;
            //            //ChildDetailList[i].SelectedConclusion = APGARScore.FirstOrDefault(p => p.ID == 1);
            //            //ChildDetailList[i].APGARScoreID = 1;
            //        }
            //    }
            //}
            //if (APGARScorePoint >= 4 && APGARScorePoint <= 6)
            //{
            //    for (int i = 0; i < ChildDetailList.Count; i++)
            //    {
            //        if (i == dgchildDetails.SelectedIndex)
            //        {
            //            ChildDetailList[i].APGARScore = APGARScorePoint;
            //            ChildDetailList[i].Conclusion = "Moderately Depressed";

            //            //ChildDetailList[i].APGARScore = APGARScorePoint;
            //            //ChildDetailList[i].ConclusionList = APGARScore;
            //            //ChildDetailList[i].SelectedConclusion = APGARScore.FirstOrDefault(p => p.ID == 2);
            //            //ChildDetailList[i].APGARScoreID = 2;
            //        }
            //    }
            //}
            //if (APGARScorePoint >= 7 && APGARScorePoint <= 10)
            //{
            //    for (int i = 0; i < ChildDetailList.Count; i++)
            //    {
            //        if (i == dgchildDetails.SelectedIndex)
            //        {
            //            ChildDetailList[i].APGARScore = APGARScorePoint;
            //            ChildDetailList[i].Conclusion = "Excellent Condition";

            //            //ChildDetailList[i].APGARScore = APGARScorePoint;
            //            //ChildDetailList[i].ConclusionList = APGARScore;
            //            //ChildDetailList[i].SelectedConclusion = APGARScore.FirstOrDefault(p => p.ID == 3);
            //            //ChildDetailList[i].APGARScoreID = 3;
            //        }
            //    }
            //}
            dgchildDetails.ItemsSource = ChildDetailList;
            dgchildDetails.UpdateLayout();
        }

        private void cmbGrimace_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < ChildDetailList.Count; i++)
            {
                if (i == dgchildDetails.SelectedIndex)
                {
                    if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                    {
                        ChildDetailList[i].GrimacePoint = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).Point;
                        //ChildDetailList[i].GrimacePoint = GrimacePOint;
                    }

                    ChildDetailList[i].APGARScore = ChildDetailList[i].ActivityPoint + ChildDetailList[i].PulsePoint + ChildDetailList[i].GrimacePoint + ChildDetailList[i].AppearancePoint + ChildDetailList[i].RespirationPoint;
                    if (ChildDetailList[i].APGARScore >= 0 && ChildDetailList[i].APGARScore <= 3)
                    {
                        ChildDetailList[i].APGARScore = ChildDetailList[i].APGARScore;
                        ChildDetailList[i].Conclusion = "Severly Depressed";
                    }
                    else if (ChildDetailList[i].APGARScore >= 4 && ChildDetailList[i].APGARScore <= 6)
                    {
                        ChildDetailList[i].APGARScore = ChildDetailList[i].APGARScore;
                        ChildDetailList[i].Conclusion = "Moderately Depressed";
                    }
                    else if (ChildDetailList[i].APGARScore >= 7 && ChildDetailList[i].APGARScore <= 10)
                    {
                        ChildDetailList[i].APGARScore = ChildDetailList[i].APGARScore;
                        ChildDetailList[i].Conclusion = "Excellent Condition";
                    }
                }
            }

            //APGARScorePoint = ActivityPoint + PulsePoint + GrimacePOint + AppearancePoint + RespirationPOint;
            //if (APGARScorePoint >= 0 && APGARScorePoint <= 3)
            //{
            //    for (int i = 0; i < ChildDetailList.Count; i++)
            //    {
            //        if (i == dgchildDetails.SelectedIndex)
            //        {
            //            ChildDetailList[i].APGARScore = APGARScorePoint;
            //            ChildDetailList[i].Conclusion = "Severly Depressed";
            //            // ChildDetailList[i].Conclusion = 7;

            //            //ChildDetailList[i].ConclusionList = APGARScore;
            //            //ChildDetailList[i].SelectedConclusion = APGARScore.FirstOrDefault(p => p.ID == 1);
            //            //ChildDetailList[i].APGARScoreID = 1;
            //        }
            //    }
            //}
            //if (APGARScorePoint >= 4 && APGARScorePoint <= 6)
            //{
            //    for (int i = 0; i < ChildDetailList.Count; i++)
            //    {
            //        if (i == dgchildDetails.SelectedIndex)
            //        {
            //            ChildDetailList[i].APGARScore = APGARScorePoint;
            //            ChildDetailList[i].Conclusion = "Moderately Depressed";

            //            //ChildDetailList[i].APGARScore = APGARScorePoint;
            //            //ChildDetailList[i].ConclusionList = APGARScore;
            //            //ChildDetailList[i].SelectedConclusion = APGARScore.FirstOrDefault(p => p.ID == 2);
            //            //ChildDetailList[i].APGARScoreID = 2;
            //        }
            //    }
            //}
            //if (APGARScorePoint >= 7 && APGARScorePoint <= 10)
            //{
            //    for (int i = 0; i < ChildDetailList.Count; i++)
            //    {
            //        if (i == dgchildDetails.SelectedIndex)
            //        {
            //            ChildDetailList[i].APGARScore = APGARScorePoint;
            //            ChildDetailList[i].Conclusion = "Excellent Condition";

            //            //ChildDetailList[i].APGARScore = APGARScorePoint;
            //            //ChildDetailList[i].ConclusionList = APGARScore;
            //            //ChildDetailList[i].SelectedConclusion = APGARScore.FirstOrDefault(p => p.ID == 3);
            //            //ChildDetailList[i].APGARScoreID = 3;
            //        }
            //    }
            //}
            dgchildDetails.ItemsSource = ChildDetailList;
            dgchildDetails.UpdateLayout();
        }

        private void cmbAppearance_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < ChildDetailList.Count; i++)
            {
                if (i == dgchildDetails.SelectedIndex)
                {
                    if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                    {
                        ChildDetailList[i].AppearancePoint = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).Point;
                        //ChildDetailList[i].AppearancePoint = AppearancePoint;
                    }

                    ChildDetailList[i].APGARScore = ChildDetailList[i].ActivityPoint + ChildDetailList[i].PulsePoint + ChildDetailList[i].GrimacePoint + ChildDetailList[i].AppearancePoint + ChildDetailList[i].RespirationPoint;
                    if (ChildDetailList[i].APGARScore >= 0 && ChildDetailList[i].APGARScore <= 3)
                    {
                        ChildDetailList[i].APGARScore = ChildDetailList[i].APGARScore;
                        ChildDetailList[i].Conclusion = "Severly Depressed";
                    }
                    else if (ChildDetailList[i].APGARScore >= 4 && ChildDetailList[i].APGARScore <= 6)
                    {
                        ChildDetailList[i].APGARScore = ChildDetailList[i].APGARScore;
                        ChildDetailList[i].Conclusion = "Moderately Depressed";
                    }
                    else if (ChildDetailList[i].APGARScore >= 7 && ChildDetailList[i].APGARScore <= 10)
                    {
                        ChildDetailList[i].APGARScore = ChildDetailList[i].APGARScore;
                        ChildDetailList[i].Conclusion = "Excellent Condition";
                    }
                }
            }

            //APGARScorePoint = ActivityPoint + PulsePoint + GrimacePOint + AppearancePoint + RespirationPOint;
            //if (APGARScorePoint >= 0 && APGARScorePoint <= 3)
            //{
            //    for (int i = 0; i < ChildDetailList.Count; i++)
            //    {
            //        if (i == dgchildDetails.SelectedIndex)
            //        {
            //            ChildDetailList[i].APGARScore = APGARScorePoint;
            //            ChildDetailList[i].Conclusion = "Severly Depressed";
            //            // ChildDetailList[i].Conclusion = 7;

            //            //ChildDetailList[i].ConclusionList = APGARScore;
            //            //ChildDetailList[i].SelectedConclusion = APGARScore.FirstOrDefault(p => p.ID == 1);
            //            //ChildDetailList[i].APGARScoreID = 1;
            //        }
            //    }
            //}
            //if (APGARScorePoint >= 4 && APGARScorePoint <= 6)
            //{
            //    for (int i = 0; i < ChildDetailList.Count; i++)
            //    {
            //        if (i == dgchildDetails.SelectedIndex)
            //        {
            //            ChildDetailList[i].APGARScore = APGARScorePoint;
            //            ChildDetailList[i].Conclusion = "Moderately Depressed";

            //            //ChildDetailList[i].APGARScore = APGARScorePoint;
            //            //ChildDetailList[i].ConclusionList = APGARScore;
            //            //ChildDetailList[i].SelectedConclusion = APGARScore.FirstOrDefault(p => p.ID == 2);
            //            //ChildDetailList[i].APGARScoreID = 2;
            //        }
            //    }
            //}
            //if (APGARScorePoint >= 7 && APGARScorePoint <= 10)
            //{
            //    for (int i = 0; i < ChildDetailList.Count; i++)
            //    {
            //        if (i == dgchildDetails.SelectedIndex)
            //        {
            //            ChildDetailList[i].APGARScore = APGARScorePoint;
            //            ChildDetailList[i].Conclusion = "Excellent Condition";

            //            //ChildDetailList[i].APGARScore = APGARScorePoint;
            //            //ChildDetailList[i].ConclusionList = APGARScore;
            //            //ChildDetailList[i].SelectedConclusion = APGARScore.FirstOrDefault(p => p.ID == 3);
            //            //ChildDetailList[i].APGARScoreID = 3;
            //        }
            //    }
            //}
            dgchildDetails.ItemsSource = ChildDetailList;
            dgchildDetails.UpdateLayout();
        }

        private void cmbRespiration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < ChildDetailList.Count; i++)
            {
                if (i == dgchildDetails.SelectedIndex)
                {
                    if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                    {
                        ChildDetailList[i].RespirationPoint = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).Point;
                        //ChildDetailList[i].RespirationPoint = RespirationPOint;
                    }
                    ChildDetailList[i].APGARScore = ChildDetailList[i].ActivityPoint + ChildDetailList[i].PulsePoint + ChildDetailList[i].GrimacePoint + ChildDetailList[i].AppearancePoint + ChildDetailList[i].RespirationPoint;
                    if (ChildDetailList[i].APGARScore >= 0 && ChildDetailList[i].APGARScore <= 3)
                    {
                        ChildDetailList[i].APGARScore = ChildDetailList[i].APGARScore;
                        ChildDetailList[i].Conclusion = "Severly Depressed";
                    }
                    else if (ChildDetailList[i].APGARScore >= 4 && ChildDetailList[i].APGARScore <= 6)
                    {
                        ChildDetailList[i].APGARScore = ChildDetailList[i].APGARScore;
                        ChildDetailList[i].Conclusion = "Moderately Depressed";
                    }
                    else if (ChildDetailList[i].APGARScore >= 7 && ChildDetailList[i].APGARScore <= 10)
                    {
                        ChildDetailList[i].APGARScore = ChildDetailList[i].APGARScore;
                        ChildDetailList[i].Conclusion = "Excellent Condition";
                    }
                }
            }

            //APGARScorePoint = ActivityPoint + PulsePoint + GrimacePOint + AppearancePoint + RespirationPOint;
            //if (APGARScorePoint >= 0 && APGARScorePoint <= 3)
            //{
            //    for (int i = 0; i < ChildDetailList.Count; i++)
            //    {
            //        if (i == dgchildDetails.SelectedIndex)
            //        {
            //            ChildDetailList[i].APGARScore = APGARScorePoint;
            //            ChildDetailList[i].Conclusion = "Severly Depressed";
            //            // ChildDetailList[i].Conclusion = 7;

            //            //ChildDetailList[i].ConclusionList = APGARScore;
            //            //ChildDetailList[i].SelectedConclusion = APGARScore.FirstOrDefault(p => p.ID == 1);
            //            //ChildDetailList[i].APGARScoreID = 1;
            //        }
            //    }
            //}
            //if (APGARScorePoint >= 4 && APGARScorePoint <= 6)
            //{
            //    for (int i = 0; i < ChildDetailList.Count; i++)
            //    {
            //        if (i == dgchildDetails.SelectedIndex)
            //        {
            //            ChildDetailList[i].APGARScore = APGARScorePoint;
            //            ChildDetailList[i].Conclusion = "Moderately Depressed";

            //            //ChildDetailList[i].APGARScore = APGARScorePoint;
            //            //ChildDetailList[i].ConclusionList = APGARScore;
            //            //ChildDetailList[i].SelectedConclusion = APGARScore.FirstOrDefault(p => p.ID == 2);
            //            //ChildDetailList[i].APGARScoreID = 2;
            //        }
            //    }
            //}
            //if (APGARScorePoint >= 7 && APGARScorePoint <= 10)
            //{
            //    for (int i = 0; i < ChildDetailList.Count; i++)
            //    {
            //        if (i == dgchildDetails.SelectedIndex)
            //        {
            //            ChildDetailList[i].APGARScore = APGARScorePoint;
            //            ChildDetailList[i].Conclusion = "Excellent Condition";

            //            //ChildDetailList[i].APGARScore = APGARScorePoint;
            //            //ChildDetailList[i].ConclusionList = APGARScore;
            //            //ChildDetailList[i].SelectedConclusion = APGARScore.FirstOrDefault(p => p.ID == 3);
            //            //ChildDetailList[i].APGARScoreID = 3;
            //        }
            //    }
            //}
            dgchildDetails.ItemsSource = ChildDetailList;
            dgchildDetails.UpdateLayout();
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (ChildDetailList != null && ChildDetailList.Count > 0)
            {
                string msgText = "Are you sure..\n  you want to Save Details ?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();
            }
            else
            {               
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Birth details can not be empty.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsIVFDashboard_AddBirthDetailsListBizActionVO BizAction = new clsIVFDashboard_AddBirthDetailsListBizActionVO();
                BizAction.BirthDetails.TherapyID = PlanTherapyID;
                BizAction.BirthDetails.TherapyUnitID = PlanTherapyUnitID;
                if (CoupleDetails != null)
                {
                    BizAction.BirthDetails.PatientID = CoupleDetails.FemalePatient.PatientID;
                    BizAction.BirthDetails.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                }
                BizAction.BirthDetailsList = new List<clsIVFDashboard_BirthDetailsVO>();

                for (int i = 0; i < ChildDetailList.Count; i++)
                {
                    BizAction.BirthDetails.ID = ChildDetailList[i].BirthDetailsID;
                    BizAction.BirthDetails.FetalHeartNo = ChildDetailList[i].FetalHeartNo;
                    BizAction.BirthDetails.SurrogateID = ChildDetailList[i].SurrogateID;
                    BizAction.BirthDetails.SurrogateUnitID = ChildDetailList[i].SurrogateUnitID;
                    BizAction.BirthDetails.SurrogatePatientMrNo = ChildDetailList[i].SurrogatePatientMrNo;

                    ChildDetailList[i].ActivityID = ChildDetailList[i].SelectedActivity.ID;
                    ChildDetailList[i].Pulse = ChildDetailList[i].SelectedPulse.ID;
                    ChildDetailList[i].Grimace = ChildDetailList[i].SelectedGrimace.ID;
                    ChildDetailList[i].Appearance = ChildDetailList[i].SelectedAppearance.ID;
                    ChildDetailList[i].Respiration = ChildDetailList[i].SelectedRespiration.ID;
                    ChildDetailList[i].APGARScoreID = ChildDetailList[i].SelectedConclusion.ID;
                    ChildDetailList[i].DeliveryTypeID = ChildDetailList[i].SelectedTypeOfDelivery.ID;
                }

                BizAction.BirthDetailsList = ChildDetailList;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        FillDetails();
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
}
