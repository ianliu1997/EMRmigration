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
using System.Text;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.UserControls;

namespace PalashDynamics.IVF.DashBoard
{
  public partial class frmReceiveOocyte : ChildWindow
    {

        public clsCoupleVO CoupleDetails;
        public long PlanTherapyID;
        public long PlanTherapyUnitID;
        public string SourceOfOocytesCode;
        public long DonorID;
        public long DonorUnitID;
        public bool IsClosed;
        public long PatientID;
        public long PatientUnitID;


        long ID;
        long UnitID;
        long OpuDateID;
        bool IsEdit = false;
        bool ISFREEZE = false;

        public event RoutedEventHandler OnCloseButton_Click;

        public frmReceiveOocyte()
        {
            InitializeComponent();
        }

        public bool validition()
        {
            bool result = true;
            if (((clsIVFDashboard_OPUVO)cmbOPUDate.SelectedItem).ID == 0)
            {
                cmbOPUDate.TextBox.SetValidation("Please Select OPU Date");
                cmbOPUDate.TextBox.RaiseValidationError();
                cmbOPUDate.Focus();
                result = false;
            }                  
            else
            {
                cmbOPUDate.ClearValidationError();                
                result = true;
            }

            if (Convert.ToInt64(txtConsumedOocytes.Text) > Convert.ToInt64(txtAvailableOocytes.Text))
            {
                txtConsumedOocytes.SetValidation("Enter Consumed Oocytes Number less than Available Balance");
                txtConsumedOocytes.RaiseValidationError();
                txtConsumedOocytes.Focus();
                result = false;
            }                  
            else
            {
                txtConsumedOocytes.ClearValidationError();                
                result = true;
            }   
            return result;
        }

        WaitIndicator wait = new WaitIndicator();
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (validition())
            {
                try
                {
                    string msgTitle = "Palash";
                    string msgText = "Are You Sure \n You Want To Save Donor Details";
                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    msgWin.Show();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
             
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                wait.Show();
                try
                {
                    clsIVFDashboard_AddUpdateRecieceOocytesBizActionVO BizActionObj = new clsIVFDashboard_AddUpdateRecieceOocytesBizActionVO();
                    BizActionObj.OPUDetails = new clsReceiveOocyteVO();                  
                    //UnitID	PatientID	PatientUnitID	TherapyID	TherapyUnitID	DonorID	DonorUnitID	DonorOPUID	DonorOPUUnitID
                    BizActionObj.IsEdit = IsEdit;
                    if (BizActionObj.IsEdit == true)
                    {
                        BizActionObj.OPUDetails.ID = ID;
                    }
                    else
                    {
                        BizActionObj.OPUDetails.ID = 0;
                    }
                    
                    BizActionObj.OPUDetails.PatientID = PatientID;
                    BizActionObj.OPUDetails.PatientUnitID = PatientUnitID;
                    BizActionObj.OPUDetails.TherapyID = PlanTherapyID;
                    BizActionObj.OPUDetails.TherapyUnitID = PlanTherapyUnitID;

                     BizActionObj.OPUDetails.DonorID = DonorID;
                    BizActionObj.OPUDetails.DonorUnitID = DonorUnitID;

                    BizActionObj.OPUDetails.DonorOPUID = ((clsIVFDashboard_OPUVO)cmbOPUDate.SelectedItem).OPUID;
                    BizActionObj.OPUDetails.DonorOPUUnitID = ((clsIVFDashboard_OPUVO)cmbOPUDate.SelectedItem).UnitID;
                    BizActionObj.OPUDetails.DonorOPUDate =Convert.ToDateTime(((clsIVFDashboard_OPUVO)cmbOPUDate.SelectedItem).Description);

                    BizActionObj.OPUDetails.DonorOocyteRetrived=(((clsIVFDashboard_OPUVO)cmbOPUDate.SelectedItem).OocyteRetrived);
                    BizActionObj.OPUDetails.DonorBalancedOocyte=(((clsIVFDashboard_OPUVO)cmbOPUDate.SelectedItem).BalanceOocyte);

                    BizActionObj.OPUDetails.OocyteConsumed=Convert.ToInt64(txtConsumedOocytes.Text);

                    if (chkFreezed.IsChecked == true)
                        BizActionObj.OPUDetails.IsFreezed = true;
                    else
                        BizActionObj.OPUDetails.IsFreezed =false;

                    //txtTotalNoOfOocytes.Text = Convert.ToString(((clsIVFDashboard_OPUVO)cmbOPUDate.SelectedItem).OocyteRetrived);
                    //txtAvailableOocytes.Text = Convert.ToString(((clsIVFDashboard_OPUVO)cmbOPUDate.SelectedItem).BalanceOocyte);

                    //DonorOPUID	DonorOPUUnitID	DonorOPUDate	            DonorOocyteRetrived	DonorBalancedOocyte	OocyteConsumed	IsFreezed

                    //CIMS_IVFDAshBoard_AddDonorDetails............ Sp Name

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Donor Details Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.OK)
                                {
                                    //Getdetails();
                                    //cmdSpermiogram.Visibility = Visibility.Visible;
                                    //cmdSpermiogram.IsEnabled = true;
                                    //fillGrid();
                                    cmdSave.IsEnabled = false;
                                }
                            };
                            msgW1.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                    };
                    client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();

                    wait.Close();
                }
                catch (Exception ex)
                {
                    wait.Show();
                    throw ex;
                }
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

            if (OnCloseButton_Click != null)
                OnCloseButton_Click(this, new RoutedEventArgs());
        }

        private void cmbOPUDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ISFREEZE == false)
            {
                if (((clsIVFDashboard_OPUVO)cmbOPUDate.SelectedItem).OPUID >  0)
                {
                    txtTotalNoOfOocytes.Text = Convert.ToString(((clsIVFDashboard_OPUVO)cmbOPUDate.SelectedItem).OocyteRetrived);
                    txtAvailableOocytes.Text = Convert.ToString(((clsIVFDashboard_OPUVO)cmbOPUDate.SelectedItem).BalanceOocyte);
                }
            }
        }

        private void chkFreezed_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
           txtMrNo.Text =SourceOfOocytesCode;
           FillOPUDate();
           Getdetails();
          
        }

        private void Getdetails()
        {
            clsGetDetailsOfReceivedOocyteBizActionVO BizActionObj = new clsGetDetailsOfReceivedOocyteBizActionVO();

            BizActionObj.Details.PatientID = PatientID;
            BizActionObj.Details.PatientUnitID = PatientUnitID;
            BizActionObj.Details.TherapyID = PlanTherapyID;
            BizActionObj.Details.TherapyUnitID = PlanTherapyUnitID;
            BizActionObj.Details.IsReceiveOocyte = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result) != null)
                    {
                        if (((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details != null && ((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details.ID > 0)
                        {
                            PatientID = Convert.ToInt32(((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details.PatientID);
                            PatientUnitID = Convert.ToInt32(((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details.PatientUnitID);
                            PlanTherapyID = Convert.ToInt32(((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details.TherapyID);
                            PlanTherapyUnitID = Convert.ToInt32(((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details.TherapyUnitID);
                            ID = Convert.ToInt32(((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details.ID);
                            UnitID = Convert.ToInt32(((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details.UnitID);

                            cmdSave.Content = "Modify";                            

                            if (((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details.DonorOPUID > 0 && ((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details.IsFreezed==false)
                            {
                                
                                FillOPUDate();
                                OpuDateID = Convert.ToInt64(((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details.DonorOPUID);
                                //cmbOPUDate.SelectedValue = Convert.ToInt64(((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details.DonorOPUID);

                                //txtTotalNoOfOocytes.Text = Convert.ToString(((clsIVFDashboard_OPUVO)cmbOPUDate.SelectedItem).OocyteRetrived);
                                //txtAvailableOocytes.Text = Convert.ToString(((clsIVFDashboard_OPUVO)cmbOPUDate.SelectedItem).BalanceOocyte);


                               
                            }
                            else
                            {
                                cmbOPUDate.SelectedValue = Convert.ToInt64(((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details.DonorOPUID);
                                txtTotalNoOfOocytes.Text = Convert.ToString(((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details.DonorOocyteRetrived);
                                txtAvailableOocytes.Text = Convert.ToString(((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details.DonorBalancedOocyte);
                            }

                            txtConsumedOocytes.Text = Convert.ToString(((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details.OocyteConsumed);
                            

                            Boolean bValue= Convert.ToBoolean(((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details.IsFreezed);
                            if (bValue == true)
                            {
                                chkFreezed.IsChecked=bValue;
                                cmdSave.IsEnabled = false;
                                ISFREEZE = true;
                                String Str = Convert.ToString(((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details.DonorOPUDate);
                                cmbOPUDate.SelectedItem = Str.Substring(0, 10);

                                txtTotalNoOfOocytes.Text = Convert.ToString(((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details.DonorOocyteRetrived);
                                txtAvailableOocytes.Text = Convert.ToString(((clsGetDetailsOfReceivedOocyteBizActionVO)arg.Result).Details.DonorBalancedOocyte);
                            }
                            else
                            {
                                chkFreezed.IsChecked = bValue;
                                cmdSave.IsEnabled = true;
                            }
                            IsEdit = true;
                        }
                    }
                }
            };
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillOPUDate()
        {
            GetIVFDashboardOPUDateBizActionVO BizAction = new GetIVFDashboardOPUDateBizActionVO();
         
            BizAction.UnitID=((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;            
            BizAction.PatientID=DonorID;
            BizAction.PatientUnitID=DonorUnitID;
                        
            BizAction.List = new List<clsIVFDashboard_OPUVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                     GetIVFDashboardOPUDateBizActionVO DetailsVO = new GetIVFDashboardOPUDateBizActionVO();
                      DetailsVO = (GetIVFDashboardOPUDateBizActionVO)args.Result;

                      if (DetailsVO.List != null)
                      {
                          List<clsIVFDashboard_OPUVO> objList = new List<clsIVFDashboard_OPUVO>();

                          if (DetailsVO.List.Count > 0)
                          {
                              for (int i = 0; i < DetailsVO.List.Count; i++)
                              {
                                  clsIVFDashboard_OPUVO obj = new clsIVFDashboard_OPUVO();
                                  if (i == 0)
                                  {
                                      obj.OPUID = 0;
                                      obj.Description = "-- Select --";
                                      objList.Add(obj);
                                  }
                                  obj = DetailsVO.List[i];
                                  objList.Add(obj);
                              }

                             
                              
                          }
                          else
                          {
                              clsIVFDashboard_OPUVO obj = new clsIVFDashboard_OPUVO();
                              
                                  obj.OPUID = 0;
                                  obj.Description = "-- Select --";
                                  objList.Add(obj);
                              
                          }
                          cmbOPUDate.ItemsSource = null;
                          cmbOPUDate.ItemsSource = objList;
                          cmbOPUDate.SelectedItem = objList[0];                          
                      }
                     
                    if (IsEdit == true)
                          cmbOPUDate.SelectedValue = OpuDateID;         
                }
                
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
    }
}

