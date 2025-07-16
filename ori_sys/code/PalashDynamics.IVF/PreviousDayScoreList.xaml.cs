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
using CIMS;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.IVF
{
    public partial class PreviousDayScoreList : ChildWindow
    {
        public long DetailID { get; set; }
        public int DayID { get; set; }
        public long CoupleID { get; set; }
        public long CoupleUnitID { get; set; }
        public PreviousDayScoreList()
        {
            InitializeComponent();
        }

       

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (DayID == 1)
            {
                GetDay0Score();
            }
            else if (DayID == 3)
            {
                GetDay2Score();
            }
            else if (DayID == 4)
            {
                GetDay3Score();
            }
            else if (DayID == 5)
            {
                GetDay4Score();
            }
            else if (DayID == 6)
            {
                GetDay5Score();
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void GetDay0Score()
        {
            clsGetDay1ScoreBizActionVO BizAction = new clsGetDay1ScoreBizActionVO();
            BizAction.Day0Score = new List<clsFemaleLabDay1FertilizationAssesmentVO>();
            BizAction.CoupleID = CoupleID;
            BizAction.CoupleUnitID = CoupleUnitID;
            
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetDay1ScoreBizActionVO)arg.Result).Day0Score != null)
                    {
                        dgScoreList.ItemsSource = ((clsGetDay1ScoreBizActionVO)arg.Result).Day0Score;
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

        private void GetDay2Score()
        {
            clsGetDay3ScoreBizActionVO BizAction = new clsGetDay3ScoreBizActionVO();
            BizAction.Day3Score = new List<clsFemaleLabDay4FertilizationAssesmentVO>();
            BizAction.CoupleID = CoupleID;
            BizAction.CoupleUnitID = CoupleUnitID;
            BizAction.Day = 2;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetDay3ScoreBizActionVO)arg.Result).Day3Score != null)
                    {
                        dgScoreList.ItemsSource = ((clsGetDay3ScoreBizActionVO)arg.Result).Day3Score;
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

        private void GetDay3Score()
        {
            clsGetDay3ScoreBizActionVO BizAction = new clsGetDay3ScoreBizActionVO();
            BizAction.Day3Score = new List<clsFemaleLabDay4FertilizationAssesmentVO>();
            BizAction.CoupleID = CoupleID;
            BizAction.CoupleUnitID = CoupleUnitID;
            BizAction.Day = 3;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetDay3ScoreBizActionVO)arg.Result).Day3Score != null)
                    {
                        dgScoreList.ItemsSource = ((clsGetDay3ScoreBizActionVO)arg.Result).Day3Score;
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

        private void GetDay4Score()
        {
            clsGetDay4ScoreBizActionVO BizAction = new clsGetDay4ScoreBizActionVO();
            BizAction.Day4Score = new List<clsFemaleLabDay5FertilizationAssesmentVO>();
            BizAction.CoupleID = CoupleID;
            BizAction.CoupleUnitID = CoupleUnitID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetDay4ScoreBizActionVO)arg.Result).Day4Score != null)
                    {
                        dgScoreList.ItemsSource = ((clsGetDay4ScoreBizActionVO)arg.Result).Day4Score;
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

        private void GetDay5Score()
        {
            clsGetDay5ScoreBizActionVO BizAction = new clsGetDay5ScoreBizActionVO();
            BizAction.Day5Score = new List<clsFemaleLabDay6FertilizationAssesmentVO>();
            BizAction.CoupleID = CoupleID;
            BizAction.CoupleUnitID = CoupleUnitID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetDay5ScoreBizActionVO)arg.Result).Day5Score != null)
                    {
                        dgScoreList.ItemsSource = ((clsGetDay5ScoreBizActionVO)arg.Result).Day5Score;
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

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
    }
}

