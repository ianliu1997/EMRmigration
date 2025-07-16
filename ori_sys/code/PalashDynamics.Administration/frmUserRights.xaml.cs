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
using PalashDynamics.ValueObjects.Administration.UserRights;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using MessageBoxControl;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.Administration
{
    public partial class frmUserRights : ChildWindow
    {
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        #region Validation
        private bool CheckValidation()
        {
            bool result = true;
            if (chkOpdBillPercentage.IsChecked == true)
            {
                if (txtOpdBillPercentage.Text.IsValueDouble())
                    txtOpdBillPercentage.ClearValidationError();
                else
                {
                    txtOpdBillPercentage.SetValidation("Please enter valid Percentage");
                    txtOpdBillPercentage.RaiseValidationError();
                    txtOpdBillPercentage.Focus();
                    result = false;
                }
            }

            if (chkOpdBillAmmount.IsChecked == true)
            {
                if (txtOpdBillAmmount.Text.IsValueDouble())
                {
                    txtOpdBillAmmount.ClearValidationError();
                }
                else
                {
                    txtOpdBillAmmount.SetValidation("Please Enter valid Bill Ammount");
                    txtOpdBillAmmount.RaiseValidationError();
                    txtOpdBillAmmount.Focus();
                    result = false;
                }
            }

            if (chkOpdBillSettlementPercentage.IsChecked == true)
            {
                if (txtOpdBillSettlementPercentage.Text.IsValueDouble())
                {
                    txtOpdBillSettlementPercentage.ClearValidationError();

                }
                else
                {
                    txtOpdBillSettlementPercentage.SetValidation("Please Enter Valid Percentage");
                    txtOpdBillSettlementPercentage.RaiseValidationError();
                    txtOpdBillSettlementPercentage.Focus();
                    result = false;
                }
            }

            if (chkOpdBillSettlementAmmount.IsChecked == true)
            {
                if (txtOpdBillSettlementAmmount.Text.IsValueDouble())
                {
                    txtOpdBillSettlementAmmount.ClearValidationError();

                }
                else
                {
                    txtOpdBillSettlementAmmount.SetValidation("Please Enter Valid Ammount");
                    txtOpdBillSettlementAmmount.RaiseValidationError();
                    txtOpdBillSettlementAmmount.Focus();
                    result = false;
                }
            }

            if (chkIpdBillPercentage.IsChecked == true)
            {
                if (txtIpdBillPercentage.Text.IsValueDouble())
                    txtIpdBillPercentage.ClearValidationError();
                else
                {
                    txtIpdBillPercentage.SetValidation("Please enter valid Percentage");
                    txtIpdBillPercentage.RaiseValidationError();
                    txtIpdBillPercentage.Focus();
                    result = false;
                }
            }

            if (chkIpdBillAmmount.IsChecked == true)
            {
                if (txtIpdBillAmmount.Text.IsValueDouble())
                {
                    txtIpdBillAmmount.ClearValidationError();
                }
                else
                {
                    txtIpdBillAmmount.SetValidation("Please Enter valid Bill Ammount");
                    txtIpdBillAmmount.RaiseValidationError();
                    txtIpdBillAmmount.Focus();
                    result = false;
                }
            }

            if (chkIpdBillSettlementPercentage.IsChecked == true)
            {
                if (txtIpdBillSettlementPercentage.Text.IsValueDouble())
                {
                    txtIpdBillSettlementPercentage.ClearValidationError();

                }
                else
                {
                    txtIpdBillSettlementPercentage.SetValidation("Please Enter Valid Percentage");
                    txtIpdBillSettlementPercentage.RaiseValidationError();
                    txtIpdBillSettlementPercentage.Focus();
                    result = false;
                }
            }

            if (chkIpdBillSettlementAmmount.IsChecked == true)
            {
                if (txtIpdBillSettlementAmmount.Text.IsValueDouble())
                {
                    txtIpdBillSettlementAmmount.ClearValidationError();

                }
                else
                {
                    txtIpdBillSettlementAmmount.SetValidation("Please Enter Valid Ammount");
                    txtIpdBillSettlementAmmount.RaiseValidationError();
                    txtIpdBillSettlementAmmount.Focus();
                    result = false;
                }
            }

            if (chkDirectPurchase.IsChecked == true)
            {
                if (txtMaxPurchaseAmtPerMonth.Text.IsValueDouble())
                {
                    txtMaxPurchaseAmtPerMonth.ClearValidationError();

                }
                else
                {
                    txtMaxPurchaseAmtPerMonth.SetValidation("Please Enter Valid Ammount");
                    txtMaxPurchaseAmtPerMonth.RaiseValidationError();
                    txtMaxPurchaseAmtPerMonth.Focus();
                    result = false;
                }
            }

            if (chkDirectPurchase.IsChecked == true)
            {
                if (txtTraFrequencyPerMonth.Text.IsNumberValid())
                {
                    txtTraFrequencyPerMonth.ClearValidationError();

                }
                else
                {
                    txtTraFrequencyPerMonth.SetValidation("Please Enter Valid Ammount");
                    txtTraFrequencyPerMonth.RaiseValidationError();
                    txtTraFrequencyPerMonth.Focus();
                    result = false;
                }
            }
            return result;
        }
        # endregion

        List<MasterListItem> TypeList = new List<MasterListItem>();
        public event RoutedEventHandler OnCloseButton_Click;
        public long UserId { get; set; }
        public bool Status { get; set; }
        public string UserName { get; set; }
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser; //new clsUserVO();
        public frmUserRights()
        {
            InitializeComponent();

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidation())
            {
                bool result = ValidateOnSave();
                if (result)
                {
                    SetCreditLimit();
                    this.DialogResult = true;
                }
                else
                {
                    MessageBoxChildWindow msgW1 = new MessageBoxChildWindow("", "Please Fill Required Information.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgW1.Show();
                }
                //   int id = (int)cmbIpdAuthLvl.SelectedValue;
                //string IpdAuthLvl = cmbIpdAuthLvl.SelectedItem.ToString();
                //long id = ((MasterListItem)cmbIpdAuthLvl.SelectedItem).ID;
                //string OpdAuthLvl = cmbOpdAuthLvl.SelectedItem.ToString();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            if (OnCloseButton_Click != null)
                OnCloseButton_Click(this, new RoutedEventArgs());
        }

        private void ChildWindow_Closed(object sender, EventArgs e)
        {

        }

        //private void chkOpd_Checked(object sender, RoutedEventArgs e)
        //{
        //    if (chkOpd.IsChecked == true)
        //    {
        //        txtOpdCredit.IsEnabled = true;
        //    }
        //}

        //private void chkIpd_Checked(object sender, RoutedEventArgs e)
        //{
        //    if (chkIpd.IsChecked == true)
        //    {
        //        txtIpdCredit.IsEnabled = true;
        //    }
        //}

        private void chkAuthLvlOpd_Checked(object sender, RoutedEventArgs e)
        {
            if (chkAuthlvlOpd.IsChecked == true)
            {
                cmbOpdAuthLvl.IsEnabled = true;
            }
        }

        private void chkAuthLvlIpd_Checked(object sender, RoutedEventArgs e)
        {
            if (chkAuthLvlIpd.IsChecked == true)
            {
                cmbIpdAuthLvl.IsEnabled = true;
            }
        }

        private void chkAuthLvlOpd_UnChecked(object sender, RoutedEventArgs e)
        {
            if (chkAuthlvlOpd.IsChecked == false)
            {
                cmbOpdAuthLvl.IsEnabled = false;
                cmbOpdAuthLvl.SelectedItem = "Select Level";
            }
        }

        private void chkAuthLvlIpd_UnChecked(object sender, RoutedEventArgs e)
        {
            if (chkAuthLvlIpd.IsChecked == false)
            {
                cmbIpdAuthLvl.IsEnabled = false;
                cmbIpdAuthLvl.SelectedItem = "Select Level";
            }
        }

        public Boolean ValidateOnSave()
        {
            Boolean isValid = true;
            if (chkDirectPurchase.IsChecked == true && (Convert.ToDecimal(txtMaxPurchaseAmtPerMonth.Text) < 1))
            {
                //cmbStore.TextBox.SetValidation("Please Select the Store");
                //cmbStore.TextBox.RaiseValidationError();
                //cmbStore.Focus();               
                txtMaxPurchaseAmtPerMonth.SetValidation("Please Enter Purchase Amount");

                //MessageBoxControl.MessageBoxChildWindow msg =
                //    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Purchase Amount", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //msg.Show();
                txtMaxPurchaseAmtPerMonth.Focus();
                isValid = false;
                return false;
            }
            else
                txtMaxPurchaseAmtPerMonth.ClearValidationError();


            if (chkDirectPurchase.IsChecked == true && Convert.ToInt64(txtTraFrequencyPerMonth.Text) < 1)
            {
                txtTraFrequencyPerMonth.SetValidation("Please Enter Transaction Frequency");
                txtTraFrequencyPerMonth.Focus();
                //MessageBoxControl.MessageBoxChildWindow msg =
                //     new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Transaction Frequency", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //msg.Show();
                isValid = false;
                return false;
            }
            else
                txtTraFrequencyPerMonth.ClearValidationError();

            return isValid;
        }

        private void SetCreditLimit()
        {

            if (chkAuthLvlIpd.IsChecked == true || chkAuthlvlOpd.IsChecked == true || chkOpdBillPercentage.IsChecked == true || chkOpdBillAmmount.IsChecked == true
                || chkOpdBillSettlementPercentage.IsChecked == true || chkOpdBillSettlementAmmount.IsChecked == true || chkIpdBillPercentage.IsChecked == true ||
                chkIpdBillAmmount.IsChecked == true || chkIpdBillSettlementPercentage.IsChecked == true || chkIpdBillSettlementAmmount.IsChecked == true || chkCentralPurchase.IsChecked == true || chkCentralPurchase.IsChecked == false || chkDirectPurchase.IsChecked == true || chkIsRCEditOnFreeze.IsChecked == true || chkIsRCEditOnFreeze.IsChecked == false)
            {
                clsSetCreditLimitBizActionVO BizAction = new clsSetCreditLimitBizActionVO();
                BizAction.objUserRight = new clsUserRightsVO();
                //if (chkOpd.IsChecked == true)
                //{
                //    BizAction.objUserRight.CreditLimit = Convert.ToInt64(txtOpdCredit.Text);
                //    BizAction.objUserRight.IsOpd = true;
                //}
                //if (chkIpd.IsChecked == true)
                //{
                //    BizAction.objUserRight.CreditLimit = Convert.ToInt64(txtIpdCredit.Text);
                //    BizAction.objUserRight.IsIpd = true;
                //}

                BizAction.objUserRight.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizAction.objUserRight.UserID = this.UserId;

                if (chkAuthLvlIpd.IsChecked == true)
                {
                    BizAction.objUserRight.IpdAuthLvl = ((MasterListItem)cmbIpdAuthLvl.SelectedItem).ID;
                    BizAction.objUserRight.IsIpd = true;
                }
                else
                {
                    BizAction.objUserRight.IpdAuthLvl = 0;
                    BizAction.objUserRight.IsIpd = false;
                }

                if (chkAuthlvlOpd.IsChecked == true)
                {
                    BizAction.objUserRight.OpdAuthLvl = ((MasterListItem)cmbOpdAuthLvl.SelectedItem).ID;
                    BizAction.objUserRight.IsOpd = true;
                }
                else
                {
                    BizAction.objUserRight.OpdAuthLvl = 0;
                    BizAction.objUserRight.IsOpd = false;
                }

                if (chkOpdBillPercentage.IsChecked == true)
                {
                    //  BizAction.objUserRight.IsOpd = true;
                    BizAction.objUserRight.OpdBillingPercentage = Convert.ToDecimal(txtOpdBillPercentage.Text);
                }
                else
                {
                    // BizAction.objUserRight.IsOpd = false;
                    BizAction.objUserRight.OpdBillingPercentage = 0;
                }

                if (chkOpdBillAmmount.IsChecked == true)
                {
                    //  BizAction.objUserRight.IsOpd = true;
                    BizAction.objUserRight.OpdBillingAmmount = Convert.ToDecimal(txtOpdBillAmmount.Text);
                }
                else
                {
                    //  BizAction.objUserRight.IsOpd = false;
                    BizAction.objUserRight.OpdBillingAmmount = 0;
                }

                if (chkOpdBillSettlementPercentage.IsChecked == true)
                {
                    //  BizAction.objUserRight.IsOpd = true;
                    BizAction.objUserRight.OpdSettlePercentage = Convert.ToDecimal(txtOpdBillSettlementPercentage.Text);
                }
                else
                {
                    //  BizAction.objUserRight.IsOpd = false;
                    BizAction.objUserRight.OpdSettlePercentage = 0;
                }

                if (chkOpdBillSettlementAmmount.IsChecked == true)
                {
                    //  BizAction.objUserRight.IsOpd = true;
                    BizAction.objUserRight.OpdSettleAmmount = Convert.ToDecimal(txtOpdBillSettlementAmmount.Text);
                }
                else
                {
                    //  BizAction.objUserRight.IsOpd = false;
                    BizAction.objUserRight.OpdSettleAmmount = 0;
                }

                if (chkIpdBillPercentage.IsChecked == true)
                {
                    //  BizAction.objUserRight.IsIpd = true;
                    BizAction.objUserRight.IpdBillingPercentage = Convert.ToDecimal(txtIpdBillPercentage.Text);
                }
                else
                {
                    //  BizAction.objUserRight.IsIpd = false;
                    BizAction.objUserRight.IpdBillingPercentage = 0;
                }

                if (chkIpdBillAmmount.IsChecked == true)
                {
                    //  BizAction.objUserRight.IsIpd = true;
                    BizAction.objUserRight.IpdBillingAmmount = Convert.ToDecimal(txtIpdBillAmmount.Text);
                }
                else
                {
                    //  BizAction.objUserRight.IsIpd = false;
                    BizAction.objUserRight.IpdBillingAmmount = 0;
                }

                if (chkIpdBillSettlementPercentage.IsChecked == true)
                {
                    //  BizAction.objUserRight.IsIpd = true;
                    BizAction.objUserRight.IpdSettlePercentage = Convert.ToDecimal(txtIpdBillSettlementPercentage.Text);
                }
                else
                {
                    //  BizAction.objUserRight.IsIpd = false;
                    BizAction.objUserRight.IpdSettlePercentage = 0;
                }

                if (chkIpdBillSettlementAmmount.IsChecked == true)
                {
                    //  BizAction.objUserRight.IsIpd = true;
                    BizAction.objUserRight.IpdSettleAmmount = Convert.ToDecimal(txtIpdBillSettlementAmmount.Text);
                }
                else
                {
                    //  BizAction.objUserRight.IsIpd = false;
                    BizAction.objUserRight.IpdSettleAmmount = 0;
                }

                if (chkDirectPurchase.IsChecked == true)
                {
                    BizAction.objUserRight.IsDirectPurchase = true;
                    BizAction.objUserRight.MaxPurchaseAmtPerTrans = Convert.ToDecimal(txtMaxPurchaseAmtPerMonth.Text);
                    BizAction.objUserRight.FrequencyPerMonth = Convert.ToInt64(txtTraFrequencyPerMonth.Text);
                }
                else
                {
                    BizAction.objUserRight.IsDirectPurchase = false;
                    BizAction.objUserRight.MaxPurchaseAmtPerTrans = 0;
                    BizAction.objUserRight.FrequencyPerMonth = 0;
                }

                if (chkPOApprovalLevel.IsChecked == true)
                {
                    BizAction.objUserRight.POApprovalLvlID = (cmbPOApprovalLevel.SelectedItem as MasterListItem).ID;
                }
                else
                {
                    BizAction.objUserRight.POApprovalLvlID = 0;
                }

                if (chkMRPAdjustmentApprovalLevel.IsChecked == true)
                {
                    BizAction.objUserRight.IsMRPAdjustmentAuth = true;
                    BizAction.objUserRight.MRPAdjustmentAuthLvlID = (cmbMRPAdjustmentApprovalLevel.SelectedItem as MasterListItem).ID;
                }
                else
                {
                    BizAction.objUserRight.MRPAdjustmentAuthLvlID = 0;
                    BizAction.objUserRight.IsMRPAdjustmentAuth = false;
                }

                if (chkRadiology.IsChecked == true)
                {


                }
                #region Addedd By Bhushanp For Advance  Refund Approval
                if (chkPatientAdvRefundAmmount.IsChecked == true)
                {
                    BizAction.objUserRight.PatientAdvRefundAmmount = Convert.ToDecimal(txtPatientAdvRefundAmmount.Text);
                }
                else
                {
                    BizAction.objUserRight.PatientAdvRefundAmmount = 0;
                }
                if (chkCompanyAdvRefundAmmount.IsChecked == true)
                {
                    BizAction.objUserRight.CompanyAdvRefundAmmount = Convert.ToDecimal(txtCompanyAdvRefundAmmount.Text);
                }
                else
                {
                    BizAction.objUserRight.CompanyAdvRefundAmmount = 0;
                }
                //Added By Bhushanp 31052017
                BizAction.objUserRight.PatientAdvRefundAuthLvlID = ((MasterListItem)cmbPatientAdvRefundAuthLvl.SelectedItem).ID;
                BizAction.objUserRight.CompanyAdvRefundAuthLvlID = ((MasterListItem)cmbCompanyAdvRefundAuthLvl.SelectedItem).ID;
                #endregion
                BizAction.objUserRight.IpdBillAuthLvlID = ((MasterListItem)cmbIpdBillAuthLvl.SelectedItem).ID;
                BizAction.objUserRight.OpdBillAuthLvlID = ((MasterListItem)cmbOpdBillAuthLvl.SelectedItem).ID;
                if (chkIsCrossAppointment.IsChecked == true)
                    BizAction.objUserRight.IsCrossAppointment = true;
                else { BizAction.objUserRight.IsCrossAppointment = false; }

                if (chkIsDailyCollection.IsChecked == true) //***//
                    BizAction.objUserRight.IsDailyCollection = true;
                else { BizAction.objUserRight.IsDailyCollection = false; }

                if (chkDirectIndent.IsChecked == true) //***//
                    BizAction.objUserRight.IsDirectIndent = true;
                else { BizAction.objUserRight.IsDirectIndent = false; }

                if (chkInterClinicIndent.IsChecked == true)  //***//
                    BizAction.objUserRight.IsInterClinicIndent = true;
                else { BizAction.objUserRight.IsInterClinicIndent = false; }

                if (chkCentralPurchase.IsChecked == true)
                    BizAction.objUserRight.IsCentarlPurchase = true;
                else { BizAction.objUserRight.IsCentarlPurchase = false; }

                if (chkRadiology.IsChecked == true)
                    BizAction.objUserRight.Isfinalized = true;
                else { BizAction.objUserRight.Isfinalized = false; }// Added By Yogesh K 17 May 16

                if (chkPathology.IsChecked == true)
                    BizAction.objUserRight.IsEditAfterFinalized = true;
                else { BizAction.objUserRight.IsEditAfterFinalized = false; }// Added By Rohinee K 10 jan 17

                if (chkAllowRefundSerAfterSampleCollection.IsChecked == true)
                    BizAction.objUserRight.IsRefundSerAfterSampleCollection = true;
                else { BizAction.objUserRight.IsRefundSerAfterSampleCollection = false; }// Added By Rohinee K 10 jan 17

                if (chkIsRCEditOnFreeze.IsChecked == true)
                    BizAction.objUserRight.IsRCEditOnFreeze = true;
                else { BizAction.objUserRight.IsRCEditOnFreeze = false; } //Added By Prashant Channe 16 Oct 18


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        if (((clsSetCreditLimitBizActionVO)ea.Result).objUserRight.ResultStatus == 1)
                        {
                            MessageBoxChildWindow msgW1 = new MessageBoxChildWindow("", "Record Updated Successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                        else if (((clsSetCreditLimitBizActionVO)ea.Result).objUserRight.ResultStatus == 2)
                        {
                            MessageBoxChildWindow msgW1 = new MessageBoxChildWindow("", "Record Added Successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                        else
                        {

                            MessageBoxChildWindow msgW1 = new MessageBoxChildWindow("", "Error Occured.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser); //user);
                client.CloseAsync();
                client = null;
            }
            else
            {
                MessageBoxChildWindow msgW1 = new MessageBoxChildWindow("", "Please Fill Required Information.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                msgW1.Show();
            }

        }
        //void GetUserRights()
        //{

        //    try
        //    {
        //       // indicator.Show();

        //        this.DataContext = null;

        //        clsGetUserRightsBizActionVO obj = new clsGetUserRightsBizActionVO();
        //        //obj.IsPagingEnabled = true;
        //        //obj.StartRowIndex = DataList.PageIndex * DataList.PageSize;
        //        //obj.MaximumRows = DataList.PageSize;
        //        //obj.SearchExpression = txtSearchCriteria.Text.Trim();

        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //        client.ProcessCompleted += (s, ea) =>
        //        {
        //            if (ea.Result != null && ea.Error == null)
        //            {
        //                clsGetUserRulesBizActionVO result = ea.Result as clsGetUserRulesBizActionVO;
        //                DataList.TotalItemCount = result.TotalRows;
        //                DataList.Clear();
        //                if (result.Details != null)
        //                {
        //                    //  DataList.Clear();
        //                    foreach (var item in result.Details)
        //                    {
        //                        DataList.Add(item);
        //                    }

        //                    dgUserList.ItemsSource = null;
        //                    dgUserList.ItemsSource = DataList;

        //                    dgDataPager.Source = null;
        //                    dgDataPager.PageSize = obj.MaximumRows;
        //                    dgDataPager.Source = DataList;
        //                }
        //                if (DataList.Count == 0)
        //                {
        //                    msgText = "User does not exists.";

        //                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
        //                    msgWindow.Show();
        //                }
        //            }
        //            else
        //            {
        //                msgText = "An Error Occured while Populating User List.";

        //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
        //                msgWindow.Show();
        //            }
        //            indicator.Close();
        //        };
        //        client.ProcessAsync(obj, User);
        //        client.CloseAsync();
        //        client = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        // throw;
        //        indicator.Close();
        //    }
        //}


        //private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    int i = 0;
        //    foreach (var bindingFlag in GetEnumValues<AuthorizationLevel>())
        //    {
        //        TypeList.Add(new MasterListItem(i, bindingFlag.ToString()));
        //        i++;
        //    }
        //    cmbIpdAuthLvl.ItemsSource = null;
        //    cmbIpdAuthLvl.ItemsSource = TypeList;
        //    cmbOpdAuthLvl.ItemsSource = null;
        //    cmbOpdAuthLvl.ItemsSource = TypeList;
        //}

        public static IEnumerable<T> GetEnumValues<T>()
        {
            return typeof(T)
                .GetFields()
                .Where(x => x.IsLiteral)
                .Select(field => (T)field.GetValue(null));
        }

        public enum AuthorizationLevel
        {
            SelectLevel,
            Level1,
            Level2,
            Both
        };

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            //int i = 0;
            //foreach (var bindingFlag in GetEnumValues<AuthorizationLevel>())
            //{
            //    TypeList.Add(new MasterListItem(i, bindingFlag.ToString()));
            //    i++;
            //}
            //cmbIpdAuthLvl.ItemsSource = null;
            //cmbIpdAuthLvl.ItemsSource = TypeList.DeepCopy();
            //cmbOpdAuthLvl.ItemsSource = null;
            //cmbOpdAuthLvl.ItemsSource = TypeList.DeepCopy();
            //cmbIpdAuthLvl.SelectedItem = TypeList[0];
            //cmbOpdAuthLvl.SelectedItem = TypeList[0];

            FillLevelsAuthorization();

        }
        private void FillLevelsAuthorization()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_RequestApprovalMaster;
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

                    cmbIpdAuthLvl.ItemsSource = null;
                    cmbIpdAuthLvl.ItemsSource = objList;
                    cmbIpdAuthLvl.SelectedItem = objList[0];

                    cmbOpdAuthLvl.ItemsSource = null;
                    cmbOpdAuthLvl.ItemsSource = objList;
                    cmbOpdAuthLvl.SelectedItem = objList[0];
                    //   GetUserRights(UserId);

                    // Code to fill IPD & OPD Bill Authorization Level
                    cmbIpdBillAuthLvl.ItemsSource = null;
                    cmbIpdBillAuthLvl.ItemsSource = objList;
                    cmbIpdBillAuthLvl.SelectedItem = objList[0];

                    cmbOpdBillAuthLvl.ItemsSource = null;
                    cmbOpdBillAuthLvl.ItemsSource = objList;
                    cmbOpdBillAuthLvl.SelectedItem = objList[0];

                    //added by Ashish for PO Approval Levels on 04Apr2016
                    cmbPOApprovalLevel.ItemsSource = null;
                    cmbPOApprovalLevel.ItemsSource = objList;
                    cmbPOApprovalLevel.SelectedItem = objList[0];
                    //End

                    cmbMRPAdjustmentApprovalLevel.ItemsSource = null;
                    cmbMRPAdjustmentApprovalLevel.ItemsSource = objList;
                    cmbMRPAdjustmentApprovalLevel.SelectedItem = objList[0];

                    //Added By Bhushan For Advance Refund Approval 31052017
                    cmbPatientAdvRefundAuthLvl.ItemsSource = null;
                    cmbPatientAdvRefundAuthLvl.ItemsSource = objList;
                    cmbPatientAdvRefundAuthLvl.SelectedItem = objList[0];

                    cmbCompanyAdvRefundAuthLvl.ItemsSource = null;
                    cmbCompanyAdvRefundAuthLvl.ItemsSource = objList;
                    cmbCompanyAdvRefundAuthLvl.SelectedItem = objList[0];

                    GetUserRights(UserId);
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        public void GetUserRights(long UserId)
        {
            if (Status == true)
            {
                try
                {
                    clsGetUserRightsBizActionVO objBizVO = new clsGetUserRightsBizActionVO();
                    objBizVO.objUserRight = new clsUserRightsVO();
                    clsUserRightsVO objDetailsVO = new clsUserRightsVO();
                    //   objDetailsVO.UserID = this.UserId;
                    //   objDetailsVO.UnitID = User.ID;
                    objBizVO.objUserRight.UserID = UserId;

                    //grbUnitStore.Visibility = Visibility.Collapsed;


                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, ea) =>
                    {
                        if (ea.Error == null && ea.Result != null)
                        {
                            clsUserRightsVO objUser = new clsUserRightsVO();
                            objUser = ((clsGetUserRightsBizActionVO)ea.Result).objUserRight;

                            if (objUser.IsIpd == true)
                            {
                                chkAuthLvlIpd.IsChecked = true;
                                cmbIpdAuthLvl.SelectedValue = objUser.IpdAuthLvl;
                            }

                            if (objUser.IsOpd == true)
                            {
                                chkAuthlvlOpd.IsChecked = true;
                                cmbOpdAuthLvl.SelectedValue = objUser.OpdAuthLvl;
                            }

                            if (objUser.OpdBillingPercentage > 0)
                            {
                                chkOpdBillPercentage.IsChecked = true;
                                txtOpdBillPercentage.Text = Convert.ToString(objUser.OpdBillingPercentage);
                            }
                            if (objUser.OpdBillingAmmount > 0)
                            {
                                chkOpdBillAmmount.IsChecked = true;
                                txtOpdBillAmmount.Text = Convert.ToString(objUser.OpdBillingAmmount);
                            }
                            if (objUser.OpdSettlePercentage > 0)
                            {
                                chkOpdBillSettlementPercentage.IsChecked = true;
                                txtOpdBillSettlementPercentage.Text = Convert.ToString(objUser.OpdSettlePercentage);
                            }
                            if (objUser.OpdSettleAmmount > 0)
                            {
                                chkOpdBillSettlementAmmount.IsChecked = true;
                                txtOpdBillSettlementAmmount.Text = Convert.ToString(objUser.OpdSettleAmmount);
                            }
                            if (objUser.IpdBillingPercentage > 0)
                            {
                                chkIpdBillPercentage.IsChecked = true;
                                txtIpdBillPercentage.Text = Convert.ToString(objUser.IpdBillingPercentage);
                            }
                            if (objUser.IpdBillingAmmount > 0)
                            {
                                chkIpdBillAmmount.IsChecked = true;
                                txtIpdBillAmmount.Text = Convert.ToString(objUser.IpdBillingAmmount);
                            }
                            if (objUser.IpdSettlePercentage > 0)
                            {
                                chkIpdBillSettlementPercentage.IsChecked = true;
                                txtIpdBillSettlementPercentage.Text = Convert.ToString(objUser.IpdSettlePercentage);
                            }
                            if (objUser.IpdSettleAmmount > 0)
                            {
                                chkIpdBillSettlementAmmount.IsChecked = true;
                                txtIpdBillSettlementAmmount.Text = Convert.ToString(objUser.IpdSettleAmmount);
                            }
                            cmbIpdBillAuthLvl.SelectedValue = objUser.IpdBillAuthLvlID;
                            cmbOpdBillAuthLvl.SelectedValue = objUser.OPDAuthLvtForConcessionID;
                            if (objUser.IsCrossAppointment == true)
                            {
                                chkIsCrossAppointment.IsChecked = true;
                            }

                            if (objUser.IsDailyCollection == true)
                            {
                                chkIsDailyCollection.IsChecked = true;
                            }

                            if (objUser.IsDirectIndent == true) //***//
                            {
                                chkDirectIndent.IsChecked = true;
                            }

                            if (objUser.IsInterClinicIndent == true)//***//
                            {
                                chkInterClinicIndent.IsChecked = true;
                            }
                            if (objUser.IsDirectPurchase == true)
                            {
                                chkDirectPurchase.IsChecked = true;
                                txtMaxPurchaseAmtPerMonth.Text = Convert.ToString(objUser.MaxPurchaseAmtPerTrans);
                                txtTraFrequencyPerMonth.Text = Convert.ToString(objUser.FrequencyPerMonth);
                            }

                            if (objUser.IsCentarlPurchase == true)
                                chkCentralPurchase.IsChecked = true;
                            else
                                chkCentralPurchase.IsChecked = false;

                            if (objUser.POApprovalLvlID > 0)
                            {
                                cmbPOApprovalLevel.SelectedValue = objUser.POApprovalLvlID;
                                chkPOApprovalLevel.IsChecked = true;
                            }
                            else
                            {
                                cmbPOApprovalLevel.SelectedValue = (long)0;
                                chkPOApprovalLevel.IsChecked = false;
                            }

                            if (objUser.IsMRPAdjustmentAuth == true)
                            {
                                chkMRPAdjustmentApprovalLevel.IsChecked = true;
                            }
                            else
                            {
                                chkMRPAdjustmentApprovalLevel.IsChecked = false;
                            }

                            if (objUser.MRPAdjustmentAuthLvlID > 0)
                            {
                                cmbMRPAdjustmentApprovalLevel.SelectedValue = objUser.MRPAdjustmentAuthLvlID;
                                //chkMRPAdjustmentApprovalLevel.IsChecked = true;
                            }
                            else
                            {
                                cmbMRPAdjustmentApprovalLevel.SelectedValue = (long)0;
                                //chkMRPAdjustmentApprovalLevel.IsChecked = false;
                            }

                            if (objUser.Isfinalized == true) //Added By Yogesh K 17 5 16
                            {
                                chkRadiology.IsChecked = true;
                            }
                            else
                            {
                                chkRadiology.IsChecked = false;
                            }

                            if (objUser.IsEditAfterFinalized == true) //Added By Rohinee
                            {
                                chkPathology.IsChecked = true;
                            }
                            else
                            {
                                chkPathology.IsChecked = false;
                            }

                            if (objUser.IsRefundSerAfterSampleCollection == true) //Added By Rohinee
                            {
                                chkAllowRefundSerAfterSampleCollection.IsChecked = true;
                            }
                            else
                            {
                                chkAllowRefundSerAfterSampleCollection.IsChecked = false;
                            }


                            //Added By Bhushanp  31052017
                            if (objUser.PatientAdvRefundAmmount > 0)
                            {
                                chkPatientAdvRefundAmmount.IsChecked = true;
                                txtPatientAdvRefundAmmount.Text = Convert.ToString(objUser.PatientAdvRefundAmmount);
                            }
                            cmbPatientAdvRefundAuthLvl.SelectedValue = objUser.PatientAdvRefundAuthLvlID;
                            //------------------------------------------------------------------------
                            
                            // Begin Added by Prashant Channe on 16/10/2018, to provide rights for Edit On freeze Rate Contract

                            if (objUser.IsRCEditOnFreeze == true)
                            {
                                chkIsRCEditOnFreeze.IsChecked = true;
                            }
                            else
                            {
                                chkIsRCEditOnFreeze.IsChecked = false;
                            }
                            // End Added by Prashant Channe on 16/10/2018, to provide rights for Edit On freeze Rate Contract

                        }
                    };
                    client.ProcessAsync(objBizVO, User);
                    client.CloseAsync();
                    client = null;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            //else
            //{
            //    MessageBoxChildWindow msgW1 = new MessageBoxChildWindow("", "Activate User First.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
            //    msgW1.Show();
            //}
        }

        # region //OPD & IPD Billing

        private void chkOpdBillPercentage_Checked(object sender, RoutedEventArgs e)
        {
            if (chkOpdBillPercentage.IsChecked == true)
            {
                txtOpdBillPercentage.IsEnabled = true;
                //  txtOpdBillPercentage.Text = "";
            }
        }

        private void chkOpdBillPercentage_UnChecked(object sender, RoutedEventArgs e)
        {
            if (chkOpdBillPercentage.IsChecked == false)
            {
                txtOpdBillPercentage.Text = "0.00";
                txtOpdBillPercentage.IsEnabled = false;
                // SetCreditLimit();
            }
        }

        private void chkOpdBillAmmount_Checked(object sender, RoutedEventArgs e)
        {
            if (chkOpdBillAmmount.IsChecked == true)
            {
                txtOpdBillAmmount.IsEnabled = true;
                //   txtOpdBillAmmount.Text = "";
            }
        }

        private void chkOpdBillAmmount_UnChecked(object sender, RoutedEventArgs e)
        {
            if (chkOpdBillAmmount.IsChecked == false)
            {
                txtOpdBillAmmount.Text = "0.00";
                txtOpdBillAmmount.IsEnabled = false;
                //  SetCreditLimit();
            }
        }

        private void chkOpdBillSettlementAmmount_Checked(object sender, RoutedEventArgs e)
        {
            if (chkOpdBillSettlementAmmount.IsChecked == true)
            {
                txtOpdBillSettlementAmmount.IsEnabled = true;
                //  txtOpdBillSettlementAmmount.Text = "";
            }
        }

        private void chkOpdBillSettlementAmmount_UnChecked(object sender, RoutedEventArgs e)
        {
            if (chkOpdBillSettlementAmmount.IsChecked == false)
            {
                txtOpdBillSettlementAmmount.Text = "0.00";
                txtOpdBillSettlementAmmount.IsEnabled = false;
                //  SetCreditLimit();
            }
        }

        private void chkOpdBillSettlementPercentage_Checked(object sender, RoutedEventArgs e)
        {
            if (chkOpdBillSettlementPercentage.IsChecked == true)
            {
                txtOpdBillSettlementPercentage.IsEnabled = true;
                //  txtOpdBillSettlementPercentage.Text = "";
            }
        }

        private void chkOpdBillSettlementPercentage_UnChecked(object sender, RoutedEventArgs e)
        {
            if (chkOpdBillSettlementPercentage.IsChecked == false)
            {
                txtOpdBillSettlementPercentage.Text = "0.00";
                txtOpdBillSettlementPercentage.IsEnabled = false;
                //  SetCreditLimit();
            }
        }

        private void chkIpdBillPercentage_Checked(object sender, RoutedEventArgs e)
        {
            if (chkIpdBillPercentage.IsChecked == true)
            {
                txtIpdBillPercentage.IsEnabled = true;
                //  txtIpdBillPercentage.Text = "";
            }
        }

        private void chkIpdBillPercentage_UnChecked(object sender, RoutedEventArgs e)
        {
            if (chkIpdBillPercentage.IsChecked == false)
            {
                txtIpdBillPercentage.Text = "0.00";
                txtIpdBillPercentage.IsEnabled = false;
                //   SetCreditLimit();
            }
        }

        private void chkIpdBillAmmount_Checked(object sender, RoutedEventArgs e)
        {
            if (chkIpdBillAmmount.IsChecked == true)
            {
                txtIpdBillAmmount.IsEnabled = true;
                //   txtIpdBillAmmount.Text = "";
            }
        }

        private void chkIpdBillAmmount_UnChecked(object sender, RoutedEventArgs e)
        {
            if (chkIpdBillAmmount.IsChecked == false)
            {
                txtIpdBillAmmount.Text = "0.00";
                txtIpdBillAmmount.IsEnabled = false;
                //  SetCreditLimit();
            }
        }

        private void chkIpdBillSettlementAmmount_Checked(object sender, RoutedEventArgs e)
        {
            if (chkIpdBillSettlementAmmount.IsChecked == true)
            {
                txtIpdBillSettlementAmmount.IsEnabled = true;
                //   txtIpdBillSettlementAmmount.Text = "";
            }
        }

        private void chkIpdBillSettlementAmmount_UnChecked(object sender, RoutedEventArgs e)
        {
            if (chkIpdBillSettlementAmmount.IsChecked == false)
            {
                txtIpdBillSettlementAmmount.Text = "0.00";
                txtIpdBillSettlementAmmount.IsEnabled = false;
                //   SetCreditLimit();
            }
        }

        private void chkIpdBillSettlementPercentage_Checked(object sender, RoutedEventArgs e)
        {
            if (chkIpdBillSettlementPercentage.IsChecked == true)
            {
                txtIpdBillSettlementPercentage.IsEnabled = true;
                //    txtIpdBillSettlementPercentage.Text = "";
            }
        }

        private void chkIpdBillSettlementPercentage_UnChecked(object sender, RoutedEventArgs e)
        {
            if (chkIpdBillSettlementPercentage.IsChecked == false)
            {
                txtIpdBillSettlementPercentage.Text = "0.00";
                txtIpdBillSettlementPercentage.IsEnabled = false;
                //   SetCreditLimit();
            }
        }

        # endregion

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = " User Rights : " + UserName;
            //if (Status == false)
            //{
            //    OKButton.IsEnabled = false;            
            //    MessageBoxChildWindow msgW1 = new MessageBoxChildWindow("", "Activate User First.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
            //    msgW1.Show();            
            //}
            //ChildWindow rootPage = Application.Current.RootVisual as ChildWindow;
            //TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            //mElement.Text = " : " + User.LoginName;
            FillLevelsAuthorization();
        }

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
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
        private void txtAmmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValueDouble() || Convert.ToDouble(((TextBox)sender).Text) <= 0 && textBefore != null)
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

        private void txtOpdBillPercentage_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (!txtOpdBillPercentage.Text.Equals("") && (txtOpdBillPercentage.Text != "0"))
                //{
                if (!txtOpdBillPercentage.Text.Equals("") && (txtOpdBillPercentage.Text != "0"))
                {
                    if (Extensions.IsItDecimal(txtOpdBillPercentage.Text) == false)
                    {
                        if (Convert.ToDecimal(txtOpdBillPercentage.Text) > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow("", "Concession percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            //txtOpdBillPercentage.SetValidation("Concession percentage should not be greater than 100");
                            //txtOpdBillPercentage.RaiseValidationError();
                            //txtOpdBillPercentage.Focus();
                            //   txtOpdBillPercentage.Text = "0.00";
                            //    txtSeniorCitizenPerAmount.Text = "0.00";
                            return;
                        }
                        //else
                        //{
                        //    txtOpdBillPercentage.ClearValidationError();
                        //}
                        String str1 = txtOpdBillPercentage.Text.Substring(txtOpdBillPercentage.Text.IndexOf(".") + 1);
                        if (Convert.ToDecimal(str1) > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            //txtOpdBillPercentage.SetValidation("Incorrect Data");
                            //txtOpdBillPercentage.RaiseValidationError();
                            //txtOpdBillPercentage.Focus();
                            //    txtOpdBillPercentage.Text = "0.00";
                            return;
                        }
                        //else
                        //{
                        //    txtOpdBillPercentage.ClearValidationError();
                        //}
                    }
                }
                //  }
            }
            catch
            {

            }
        }

        private void txtIpdBillPercentage_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (!txtOpdBillPercentage.Text.Equals("") && (txtOpdBillPercentage.Text != "0"))
                //{
                if (!txtIpdBillPercentage.Text.Equals("") && (txtIpdBillPercentage.Text != "0"))
                {
                    if (Extensions.IsItDecimal(txtIpdBillPercentage.Text) == false)
                    {
                        if (Convert.ToDecimal(txtIpdBillPercentage.Text) > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow("", "Concession percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            //txtIpdBillPercentage.SetValidation("Concession percentage should not be greater than 100");
                            //txtIpdBillPercentage.RaiseValidationError();
                            //txtIpdBillPercentage.Focus();
                            //   txtIpdBillPercentage.Text = "0.00";
                            //    txtSeniorCitizenPerAmount.Text = "0.00";
                            return;
                        }
                        //else
                        //{
                        //    txtIpdBillPercentage.ClearValidationError();
                        //}
                        String str1 = txtIpdBillPercentage.Text.Substring(txtIpdBillPercentage.Text.IndexOf(".") + 1);
                        if (Convert.ToDecimal(str1) > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            //txtIpdBillPercentage.SetValidation("Incorrect Data");
                            //txtIpdBillPercentage.RaiseValidationError();
                            //txtIpdBillPercentage.Focus();
                            //   txtIpdBillPercentage.Text = "0.00";
                            return;
                        }
                        //else
                        //{
                        //    txtIpdBillPercentage.ClearValidationError();
                        //}
                    }
                }
                //  }
            }
            catch
            {

            }
        }

        private void txtTraFrequencyPerMonth_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text.Trim() != "" && (!((TextBox)sender).Text.IsPositiveNumber()) && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void chkDirectPurchase_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chkDirectPurchase.IsChecked == false)
            {
                txtMaxPurchaseAmtPerMonth.Text = "0.00";
                txtMaxPurchaseAmtPerMonth.IsEnabled = false;
                txtTraFrequencyPerMonth.Text = "0.00";
                txtTraFrequencyPerMonth.IsEnabled = false;
            }
        }

        private void chkDirectPurchase_Checked(object sender, RoutedEventArgs e)
        {
            if (chkDirectPurchase.IsChecked == true)
            {
                txtMaxPurchaseAmtPerMonth.IsEnabled = true;
                txtTraFrequencyPerMonth.IsEnabled = true;
            }
        }

        private void chkCentralPurchase_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chkCentralPurchase_Checked(object sender, RoutedEventArgs e)
        {

        }


        private void chkPOApprovalLevel_Checked(object sender, RoutedEventArgs e)
        {
            if (chkPOApprovalLevel.IsChecked == true)
                cmbPOApprovalLevel.IsEnabled = true;
            else
            {
                cmbPOApprovalLevel.IsEnabled = false;
                cmbPOApprovalLevel.SelectedValue = (long)0;
            }
        }

        private void chkPOApprovalLevel_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chkPOApprovalLevel.IsChecked == true)
                cmbPOApprovalLevel.IsEnabled = true;
            else
            {
                cmbPOApprovalLevel.IsEnabled = false;
                cmbPOApprovalLevel.SelectedValue = (long)0;
            }
        }

        private void chkPatientAdvRefundAmmount_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chkPatientAdvRefundAmmount.IsChecked == false)
            {
                txtPatientAdvRefundAmmount.Text = "0.00";
                txtPatientAdvRefundAmmount.IsEnabled = false;
                //  SetCreditLimit();
            }
        }

        private void chkPatientAdvRefundAmmount_Checked(object sender, RoutedEventArgs e)
        {
            if (chkPatientAdvRefundAmmount.IsChecked == true)
            {
                txtPatientAdvRefundAmmount.IsEnabled = true;
                //   txtOpdBillAmmount.Text = "";
            }
        }

        private void chkCompanyAdvRefundAmmount_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chkCompanyAdvRefundAmmount_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkMRPAdjustmentApprovalLevel_Checked(object sender, RoutedEventArgs e)
        {
            if (chkMRPAdjustmentApprovalLevel.IsChecked == true)
                cmbMRPAdjustmentApprovalLevel.IsEnabled = true;
            else
            {
                cmbMRPAdjustmentApprovalLevel.IsEnabled = false;
                cmbMRPAdjustmentApprovalLevel.SelectedValue = (long)0;
            }
        }

        private void chkMRPAdjustmentApprovalLevel_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chkMRPAdjustmentApprovalLevel.IsChecked == true)
                cmbMRPAdjustmentApprovalLevel.IsEnabled = true;
            else
            {
                cmbMRPAdjustmentApprovalLevel.IsEnabled = false;
                cmbMRPAdjustmentApprovalLevel.SelectedValue = (long)0;
            }
        }

        private void cmbMRPAdjustmentApprovalLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbMRPAdjustmentApprovalLevel.SelectedItem != null && (cmbMRPAdjustmentApprovalLevel.SelectedItem as MasterListItem).ID > 0)
            //{
            //    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForMRPAdjustmentID == 0)
            //    {
            //        MessageBoxControl.MessageBoxChildWindow msgW =
            //                new MessageBoxControl.MessageBoxChildWindow("", "First set Approval Level at Application Configurartion.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //        msgW.Show();
            //    }
            //    else if ((cmbMRPAdjustmentApprovalLevel.SelectedItem as MasterListItem).ID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForMRPAdjustmentID)
            //    {
            //        MessageBoxControl.MessageBoxChildWindow msgW =
            //                new MessageBoxControl.MessageBoxChildWindow("", "Set Proper Level as per Configurartion. Congiguration Level is " + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForMRPAdjustmentID, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //        msgW.Show();
            //    }
            //}
        }       

    }
}