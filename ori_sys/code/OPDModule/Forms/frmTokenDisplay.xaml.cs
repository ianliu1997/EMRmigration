//Created Date:06/September/2012
//Created By: Nilesh Raut
//Specification: To Add,Update the Show the Token Display

//Review By:
//Review Date:

//Modified By:
//Modified Date: 

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

using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Inventory;
using CIMS;
using System.ComponentModel;
using PalashDynamics.Collections;
using System.Reflection;
using PalashDynamics.UserControls;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.TokenDisplay;

namespace OPDModule.Forms
{
    public partial class frmTokenDisplay : UserControl
    {
        DispatcherTimer DisplayTimer = new DispatcherTimer();

        ObservableCollection<clsTokenDisplayVO> DispayList;
        bool layUpdate = false;


        public frmTokenDisplay()
        {
            InitializeComponent();
        }
        private void frmTokenDisplay_Loaded(object sender, RoutedEventArgs e)
        {
            MediaAudio.Play();
            DispayList = new ObservableCollection<clsTokenDisplayVO>();


            dgTokenDisp.ItemsSource = null;
            dgTokenDisp.ItemsSource = DispayList;
            dgTokenDisp.UpdateLayout();
            ShowData();

            DisplayTimer.Interval = new TimeSpan(0, 0, 10);
            DisplayTimer.Tick += new EventHandler(DisplayTimer_Tick);
            DisplayTimer.Start();

        }

        protected void DisplayTimer_Tick(object s, EventArgs args)
        {
            // PalashDynamics.ValueObjects.OutPatientDepartment.Registration.clsGetPatientAllVisitBizActionVO
            //Label1.Content = DateTime.Now;
            ShowData();
        }
        void ShowData()
        {
            try
            {
                MediaAudio.Stop();
                clsGetTokenDisplayBizActionVO bizActionVO = new clsGetTokenDisplayBizActionVO();
                bizActionVO.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                bizActionVO.VisitDate = DateTime.Now;
                bizActionVO.ListTokenDisplay = new List<clsTokenDisplayVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {

                        bizActionVO.ListTokenDisplay = (((clsGetTokenDisplayBizActionVO)args.Result).ListTokenDisplay);
                        /////Setup Page Fill DataGrid
                        /////
                        DispayList.Clear();
                        if (bizActionVO.ListTokenDisplay.Count > 0)
                        {
                            MediaAudio.Play();
                            foreach (clsTokenDisplayVO TokVo in bizActionVO.ListTokenDisplay)
                            {
                                clsTokenDisplayVO objInv = new clsTokenDisplayVO();
                                objInv.ID = TokVo.ID;
                                objInv.MrNo = TokVo.MrNo;
                                objInv.PatientName = TokVo.PatientName;
                                objInv.DoctorName = TokVo.DoctorName;
                                objInv.TokenNo = TokVo.TokenNo;
                                objInv.Cabin = TokVo.Cabin;
                                DispayList.Add(objInv);
                            }
                            var item1 = from r in bizActionVO.ListTokenDisplay
                                        select r;

                            
                        }

                    }
                    else
                    {

                        //objProgress.Close();
                        string msgText = "Error occured while processing data.";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                    }

                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
                client.CloseAsync();
                client = null;
            }
            catch (Exception)
            {
                //objProgress.Close();
                string msgText = "Error occured while processing data.";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindow.Show();
            }
        }

        private void dgTokenDisp_LayoutUpdated(object sender, EventArgs e)
        {
            if (layUpdate == false)
            {
                for (int i = 0; i < 5; i++)
                {

                    DataGridColumn col = dgTokenDisp.Columns[i];
                    col.CellStyle = (Style)Resources["DataGridCellStyle"];


                }
                layUpdate = true;
            }
        }

        private void dgTokenDisp_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            FrameworkElement el;
            el = this.dgTokenDisp.Columns[1].GetCellContent(e.Row);
            DataGridCell changeCell=null;// = GetParent(el, typeof(DataGridCell)) as DataGridCell;
            SolidColorBrush brush = new SolidColorBrush(Colors.Black);
            if (changeCell != null)
            {
                //if (clist.value > 0)
                //{
                    brush = new SolidColorBrush(Colors.Green);
                //}
                //else if (clist.value < 0)
                //{
                //    brush = new SolidColorBrush(Colors.Red);
                //}
                changeCell.Foreground = brush;
            }

        }
    }
}

