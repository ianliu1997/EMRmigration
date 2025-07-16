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
using CIMS;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.Service.PalashTestServiceReference;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmBirth : UserControl
    {
        public clsCoupleVO CoupleDetails;
        public long PlanTherapyID;
        public long PlanTherapyUnitID;
        public long ChildID;
        WaitIndicator wait = new WaitIndicator();      
        public DateTime? PregnancyAchivedDate;
        public bool? IsPregnancyAchived;
        private List<clsIVFDashboard_BirthDetailsVO> BirthList;
        public frmBirth()
        {
            InitializeComponent();

        }

       private void FillChild()
        {
            try
            {
                wait.Show();
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();

                BizAction.MasterTable = MasterTableNameList.M_IVF_Child;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                        cmbChild.ItemsSource = null;
                        cmbChild.ItemsSource = objList;
                        cmbChild.SelectedItem = objList[0];
                    }
                    FillGender();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

       private void FillGender()
        {
           try
            {
                wait.Show();
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_GenderMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbGender.ItemsSource = null;
                    cmbGender.ItemsSource = objList;
                    cmbGender.SelectedItem = objList[0];
                    
                }
                FillCountry();
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
              }
           catch (Exception ex)
            {
                wait.Close();
            }
        }

       private void FillCountry()
        {
           try
            {
                wait.Show();
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "Country";
            BizAction.IsDecode = true;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtCountry.ItemsSource = null;
                    txtCountry.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List.DeepCopy();                 
                }
                FillCondition();
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
            }
           catch (Exception ex)
            {
                wait.Close();
            }
        }

       private void FillCondition()
        {
           try
            {
                wait.Show();
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_Condition;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbCondition.ItemsSource = null;
                    cmbCondition.ItemsSource = objList;
                    cmbCondition.SelectedItem = objList[0];
                   
                }
                FillDeliveryMethod();
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
             }
           catch (Exception ex)
            {
                wait.Close();
            }
        }

       private void FillDeliveryMethod()
        {
           try
            {
                wait.Show();
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_DeliveryMethod;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbDeliveryMethod.ItemsSource = null;
                    cmbDeliveryMethod.ItemsSource = objList;
                    cmbDeliveryMethod.SelectedItem = objList[0];
               
                }
                FillDealthPastPortman();
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
            }
           catch (Exception ex)
            {
                wait.Close();
            }
        }

       private void FillDealthPastPortman()
        {
           try
            {
                wait.Show();
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_IVF_DealthPastPortMan;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbDeathPostportumon.ItemsSource = null;
                    cmbDeathPostportumon.ItemsSource = objList;
                    cmbDeathPostportumon.SelectedItem = objList[0];
                   
                }
                FillDiedParinatallyOn();
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
             }
           catch (Exception ex)
            {
                wait.Close();
            }
        }

       private void FillDiedParinatallyOn()
        {
            try
            {
                wait.Show();
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_IVF_DiedParinatallyOn;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbDiedperinatallyon.ItemsSource = null;
                    cmbDiedperinatallyon.ItemsSource = objList;
                    cmbDiedperinatallyon.SelectedItem = objList[0];
                  
                }
                fillDetails();
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
      
            wait.Close();
             }
           catch (Exception ex)
            {
                wait.Close();
            }
        } 
        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DateofBirth.SelectedDate = DateTime.Now.Date;
            FillChild();
            txtCountry.Text = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Country;
            txtSurname.Text = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient.LastName;
            txtTownofBirth.Text = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.City;
            DateofBirth.DisplayDateEnd = DateTime.Now.Date;
        }

     
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                if (IsNew == true)
                {
                    var item = from r in BirthList
                               where (r.ChildID == ((MasterListItem)cmbChild.SelectedItem).ID)
                               select r;
                    if (item.ToList().Count > 0)
                    {
                        string msgText = "Details of Child already exists";

                        MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.Show();
                    }
                    else
                    {
                        string msgText = "Are you sure..\n  you want to Save Details ?";

                        MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                        msgW.Show();
                    }
                }
                else
                {
                    string msgText = "Are you sure..\n  you want to Save Details ?";

                    MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    msgW.Show();
                }

            }
        }
        private void fillDetails()
        {
            try
            {
                wait.Show();
                ValueObjects.DashBoardVO.clsIVFDashboard_GetBirthDetailsBizActionVO BizAction = new ValueObjects.DashBoardVO.clsIVFDashboard_GetBirthDetailsBizActionVO();
                BizAction.BirthDetails = new ValueObjects.DashBoardVO.clsIVFDashboard_BirthDetailsVO();
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
                        if (((clsIVFDashboard_GetBirthDetailsBizActionVO)arg.Result).BirthDetailsList != null)
                        {
                            BirthList = new List<clsIVFDashboard_BirthDetailsVO>();
                            if (((clsIVFDashboard_GetBirthDetailsBizActionVO)arg.Result).BirthDetailsList.Count > 0)
                            {
                                BirthList=((clsIVFDashboard_GetBirthDetailsBizActionVO)arg.Result).BirthDetailsList;
                                dgchildDetails.ItemsSource = null;
                                dgchildDetails.ItemsSource = BirthList;

                            }
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                wait.Close();
            }
            catch (Exception ex)
            {
                throw ex;
                wait.Close();
            }
        }
        private void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    PalashDynamics.ValueObjects.DashBoardVO.clsIVFDashboard_AddBirthDetailsBizActionVO bizactionVO=new PalashDynamics.ValueObjects.DashBoardVO.clsIVFDashboard_AddBirthDetailsBizActionVO();
                    bizactionVO.BirthDetails = new PalashDynamics.ValueObjects.DashBoardVO.clsIVFDashboard_BirthDetailsVO();
                    if (IsNew == false)
                    {
                        bizactionVO.BirthDetails.ID = ((PalashDynamics.ValueObjects.DashBoardVO.clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).ID;
                    }
                    else
                    {
                        bizactionVO.BirthDetails.ID = 0;
                    }
                    bizactionVO.BirthDetails.PatientID= CoupleDetails.FemalePatient.PatientID;
                    bizactionVO.BirthDetails.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                    bizactionVO.BirthDetails.TherapyID= PlanTherapyID;
                    bizactionVO.BirthDetails.TherapyUnitID = PlanTherapyUnitID;
                    bizactionVO.BirthDetails.DateOfBirth = DateofBirth.SelectedDate;
                    bizactionVO.BirthDetails.Week =Convert.ToInt64(txtWeek.Text);
                    if (((MasterListItem)cmbDeliveryMethod.SelectedItem).ID !=null)
                    bizactionVO.BirthDetails.DeliveryMethodID = ((MasterListItem)cmbDeliveryMethod.SelectedItem).ID;
                    if (((MasterListItem)cmbChild.SelectedItem).ID != null)
                    bizactionVO.BirthDetails.ChildID = ((MasterListItem)cmbChild.SelectedItem).ID;
                    if (((MasterListItem)cmbGender.SelectedItem).ID != null)
                    bizactionVO.BirthDetails.GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;
                    if (((MasterListItem)cmbDeathPostportumon.SelectedItem).ID != null)
                    bizactionVO.BirthDetails.DeathPostportumID = ((MasterListItem)cmbDeathPostportumon.SelectedItem).ID;
                    if (((MasterListItem)cmbDiedperinatallyon.SelectedItem).ID != null)
                    bizactionVO.BirthDetails.DiedPerinatallyID = ((MasterListItem)cmbDiedperinatallyon.SelectedItem).ID;
                    if (((MasterListItem)cmbCondition.SelectedItem).ID != null)
                    bizactionVO.BirthDetails.ConditionID = ((MasterListItem)cmbCondition.SelectedItem).ID;
                    bizactionVO.BirthDetails.Country = txtCountry.Text;
                    bizactionVO.BirthDetails.TownOfBirth = txtTownofBirth.Text;
                    bizactionVO.BirthDetails.WeightAtBirth =Convert.ToSingle(txtWeight.Text);
                    bizactionVO.BirthDetails.LengthAtBirth = Convert.ToSingle(txtLength.Text);
                    bizactionVO.BirthDetails.FirstName = txtFirstName.Text;
                    bizactionVO.BirthDetails.Surname = txtSurname.Text;
           
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.OK)
                                    cmbChild.IsEnabled = true;
                                fillDetails();
                                ClearUI();
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
                    client.ProcessAsync(bizactionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtCountry_LostFocus(object sender, RoutedEventArgs e)
        {
            txtCountry.Text = txtCountry.Text.ToTitleCase();        
        }

        private void txtCountry_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((AutoCompleteBox)sender).Text;
        }

        private void txtCountry_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((AutoCompleteBox)sender).Text))
            {
                if (!((AutoCompleteBox)sender).Text.IsOnlyCharacters())
                {
                    ((AutoCompleteBox)sender).Text = textBefore;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }

        }

        void ClearUI()
        {
            cmbChild.SelectedValue = (long)0;
            cmbGender.SelectedValue = (long)0;
            cmbCondition.SelectedValue = (long)0;
            txtWeight.Text = string.Empty;
            txtLength.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtIdentityNumber.Text = string.Empty;
            cmbDeathPostportumon.SelectedValue = (long)0;
            cmbDiedperinatallyon.SelectedValue = (long)0;
            cmbDeliveryMethod.SelectedValue = (long)0;

        }

        public bool Validation()
        {
            if (DateofBirth.SelectedDate == null )
            {
             
                DateofBirth.SetValidation("Please Select Date of Birth");
                DateofBirth.RaiseValidationError();
                DateofBirth.Focus();
                return false;
            }
            else   if (cmbChild.SelectedItem == null || ((MasterListItem)cmbChild.SelectedItem).ID == 0)
            {
                DateofBirth.ClearValidationError();
                
                cmbChild.TextBox.SetValidation("Please Select Child");
                cmbChild.TextBox.RaiseValidationError();
                cmbChild.Focus();
                return false;
            }
            else if (cmbGender.SelectedItem == null || ((MasterListItem)cmbGender.SelectedItem).ID == 0)
            {
                DateofBirth.ClearValidationError();
                cmbChild.ClearValidationError();
               
                cmbGender.TextBox.SetValidation("Please Select Gender");
                cmbGender.TextBox.RaiseValidationError();
                cmbGender.Focus();
                return false;
            }
            else if (cmbDeliveryMethod.SelectedItem == null || ((MasterListItem)cmbDeliveryMethod.SelectedItem).ID == 0)
            {
                DateofBirth.ClearValidationError();
                cmbChild.ClearValidationError();
                cmbGender.ClearValidationError();

                cmbDeliveryMethod.TextBox.SetValidation("Please Select Delivery Method");
                cmbDeliveryMethod.TextBox.RaiseValidationError();
                cmbDeliveryMethod.Focus();
                return false;
            }
            else if (txtFirstName.Text == null)
            {
                DateofBirth.ClearValidationError();
                cmbChild.ClearValidationError();
                cmbGender.ClearValidationError();
                cmbDeliveryMethod.ClearValidationError();

                txtFirstName.SetValidation("Please Enter First Name .");
                txtFirstName.RaiseValidationError();
                txtFirstName.Focus();
                return false;
            }
            else 
            {
                DateofBirth.ClearValidationError();
                cmbChild.ClearValidationError();
                cmbGender.ClearValidationError();
                cmbDeliveryMethod.ClearValidationError();
                txtFirstName.ClearValidationError();
                return true;
            }

        }

        private void cmbChild_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ClearUI();
        }

        private void txtFirstName_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsOnlyCharacters() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtWeight_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtWeight_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtLength_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtLength_TextChanged(object sender, TextChangedEventArgs e)
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

        private void DateofBirth_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsPregnancyAchived == true)
            {
                if (DateofBirth.SelectedDate != null)
                {
                    if (PregnancyAchivedDate.Value != null)
                    {
                        DateTime? startDateTime = DateofBirth.SelectedDate;
                        TimeSpan? ts = (startDateTime - PregnancyAchivedDate);
                        int dateDiff = ts.Value.Days;
                        txtWeek.Text = Convert.ToString(dateDiff / 7);
                    }
                    else
                    {
                        txtWeek.Text = "";
                    }
                }
            }
            else
            {
                txtWeek.Text = "";
            }
        }
        bool IsNew = true;
        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            if (dgchildDetails.SelectedItem != null)
            {
                IsNew = false;
                cmbChild.IsEnabled = false;
                DateofBirth.SelectedDate=Convert.ToDateTime(((PalashDynamics.ValueObjects.DashBoardVO.clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).DateOfBirth);
                txtWeek.Text=Convert.ToString(((PalashDynamics.ValueObjects.DashBoardVO.clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).Week);
                cmbChild.SelectedValue=(((PalashDynamics.ValueObjects.DashBoardVO.clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).ChildID);
                cmbGender.SelectedValue=(((PalashDynamics.ValueObjects.DashBoardVO.clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).GenderID);
                cmbDeathPostportumon.SelectedValue=(((PalashDynamics.ValueObjects.DashBoardVO.clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).DeathPostportumID);
                cmbDiedperinatallyon.SelectedValue=(((PalashDynamics.ValueObjects.DashBoardVO.clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).DiedPerinatallyID);
                cmbCondition.SelectedValue=(((PalashDynamics.ValueObjects.DashBoardVO.clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).ConditionID);
                txtCountry.Text=(((PalashDynamics.ValueObjects.DashBoardVO.clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).Country);
                txtTownofBirth.Text=Convert.ToString(((PalashDynamics.ValueObjects.DashBoardVO.clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).TownOfBirth);
                txtWeight.Text=Convert.ToString(((PalashDynamics.ValueObjects.DashBoardVO.clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).WeightAtBirth);
                txtLength.Text=Convert.ToString(((PalashDynamics.ValueObjects.DashBoardVO.clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).LengthAtBirth);
                txtFirstName.Text=Convert.ToString(((PalashDynamics.ValueObjects.DashBoardVO.clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).FirstName);
                txtSurname.Text = Convert.ToString(((PalashDynamics.ValueObjects.DashBoardVO.clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).Surname);
                txtIdentityNumber.Text = Convert.ToString(((PalashDynamics.ValueObjects.DashBoardVO.clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).IdentityNumber);
                cmbDeliveryMethod.SelectedValue = (((PalashDynamics.ValueObjects.DashBoardVO.clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).DeliveryMethodID);
            }
           
        }

        private void DeleteChild_Click(object sender, RoutedEventArgs e)
        {
            if (dgchildDetails.SelectedItem != null)
            {
                try
                {

                clsIVFDashboard_DeleteBirthDetailsBizActionVO BizActionVO = new clsIVFDashboard_DeleteBirthDetailsBizActionVO();
                BizActionVO.BirthDetails = new clsIVFDashboard_BirthDetailsVO();
                BizActionVO.BirthDetails.ID = ((clsIVFDashboard_BirthDetailsVO)dgchildDetails.SelectedItem).ID;

                     Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Child Details Deleted Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.OK)
                                    fillDetails();
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
                    client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
      
    }
}
