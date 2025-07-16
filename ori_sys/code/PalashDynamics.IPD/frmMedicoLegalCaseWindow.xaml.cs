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
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.Controls;
using PalashDynamics.ValueObjects;
using System.IO;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using MessageBoxControl;
using PalashDynamics.ValueObjects.Patient;
using System.Windows.Browser;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;

namespace PalashDynamics.IPD
{
    public partial class frmMedicoLegalCaseWindow : ChildWindow, IInitiateCIMS
    {
        public event RoutedEventHandler OnSaveButton_Click;
        public event RoutedEventHandler OnClosebutton_Click;
        private clsIPDAdmMLCDetailsVO _MedicoLegalCase = null;
        private clsIPDAdmMLCDetailsVO _MedicoLegalCaseVO;
        bool IsPatientExist = true;
        long newVisitAdmID, newVisitAdmUnitID;
        byte[] AttachedFileContents;
        string AttachedFileName;
        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "NEW":
                    if (((IApplicationConfiguration)App.Current).SelectedIPDPatient == null)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW5.Show();
                        IsPatientExist = false;
                        break;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedIPDPatient.ID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW5.Show();

                        IsPatientExist = false;
                        break;
                    }
                    if (((IApplicationConfiguration)App.Current).SelectedIPDPatient.IsDischarge == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient Already Discharged.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW5.Show();
                        IsPatientExist = false;
                        break;
                    }
                    IsPatientExist = true;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                    // mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;
                    break;
            }
        }

        #endregion
        public frmMedicoLegalCaseWindow()
        {
            InitializeComponent();
            if (((IApplicationConfiguration)App.Current).SelectedIPDPatient != null)
            {
                // ((IApplicationConfiguration)App.Current).SelectedIPDPatient = new clsIPDAdmissionVO();
                newVisitAdmID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.ID;
                newVisitAdmUnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId;

            }
            this.Loaded += new RoutedEventHandler(frmMedicoLegalCaseWindow_Loaded);
        }
        void frmMedicoLegalCaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsPatientExist == false)
                this.DialogResult = false;
            else
            {
                GetMedicoLegalCase();
                SetPatientDetails();
            }

            // this.DataContext = this._MedicoLegalCase;
        }
        private void GetMedicoLegalCase()
        {
            clsGetIPDAdmissionListBizActionVO BizAction = new clsGetIPDAdmissionListBizActionVO();
            BizAction.AdmDetails = ((IApplicationConfiguration)App.Current).SelectedIPDPatient;
            BizAction.IsMedicoLegalCase = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (((clsGetIPDAdmissionListBizActionVO)args.Result).AdmMLDCDetails != null)
                    {
                        //clsIPDAdmMLCDetailsVO _MedicoLegalCase = ((clsGetIPDAdmissionListBizActionVO)args.Result).AdmMLDCDetails;
                        clsIPDAdmMLCDetailsVO _MedicoLegalCaseVO = null;
                        this._MedicoLegalCaseVO = ((clsGetIPDAdmissionListBizActionVO)args.Result).AdmMLDCDetails;
                        AttachedFileContents = ((clsGetIPDAdmissionListBizActionVO)args.Result).AdmMLDCDetails.AttachedFileContent;
                        txtAddress.Text = ((clsGetIPDAdmissionListBizActionVO)args.Result).AdmMLDCDetails.Address;
                        txtNumber.Text = ((clsGetIPDAdmissionListBizActionVO)args.Result).AdmMLDCDetails.Number;
                        txtPoliceStation.Text = ((clsGetIPDAdmissionListBizActionVO)args.Result).AdmMLDCDetails.PoliceStation;
                        txtNameOfAutority.Text = ((clsGetIPDAdmissionListBizActionVO)args.Result).AdmMLDCDetails.Authority;
                        txtMedicoLegalCaseRemark.Text = ((clsGetIPDAdmissionListBizActionVO)args.Result).AdmMLDCDetails.Remark;
                        txtTitle.Text = ((clsGetIPDAdmissionListBizActionVO)args.Result).AdmMLDCDetails.Title;
                        txtFileName.Text = ((clsGetIPDAdmissionListBizActionVO)args.Result).AdmMLDCDetails.AttachedFileName;
                        cmdSave.IsEnabled = false;
                        hlkPreview.IsEnabled = true;
                    }
                    else
                    {
                        hlkPreview.IsEnabled = false;
                    }

                }
                else
                {
                    clsIPDAdmMLCDetailsVO _MedicoLegalCase = new clsIPDAdmMLCDetailsVO();

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser); // new clsUserVO());
            client.CloseAsync();
        }
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            string strError = Validate();
            if (strError == string.Empty)
            {
                msgText = "Do you want to save Record?.";
                MessageBoxChildWindow msg = new MessageBoxChildWindow("", msgText, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                msg.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        SaveMedicoLegalCase((clsIPDAdmMLCDetailsVO)this.DataContext);
                    }
                };
                msg.Show();
            }
            else
            {
                string msgText = "Required Fields " + strError;
                MessageBoxControl.MessageBoxChildWindow ErrormsgW =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                ErrormsgW.Show();
            }
        }
        public string Validate()
        {
            string strError = string.Empty;
            List<clsControlsValue> ManadatoryFieldList = ListControls();
            if (ManadatoryFieldList != null)
            {
                foreach (clsControlsValue item in ListControls())
                {
                    if (item.ControlType == (int)PalashDynamics.ValueObjects.Controls.TextBox)
                    {
                        TextBox txt = (TextBox)this.FindName(item.ControlName);
                        if (txt.IsEnabled)
                        {
                            if (txt.Text.Length == 0)
                            {
                                if (strError == string.Empty)
                                {
                                    strError += item.ErrorFieldName;
                                }
                                else
                                {
                                    strError += ", " + item.ErrorFieldName;
                                }
                                txt.BorderBrush = new SolidColorBrush(Colors.Red);
                            }
                            else
                            {
                                txt.BorderBrush = new SolidColorBrush(Colors.Gray);
                            }
                        }
                        else
                        {
                            txt.BorderBrush = new SolidColorBrush(Colors.Gray);
                        }
                    }
                    if (item.ControlType == (int)PalashDynamics.ValueObjects.Controls.AutoCompleteComboBox)
                    {
                        AutoCompleteComboBox txt = (AutoCompleteComboBox)this.FindName(item.ControlName);
                        if (txt.IsEnabled)
                        {
                            if (txt.SelectedItem != null)
                            {
                                if (((MasterListItem)txt.SelectedItem).ID < 1)
                                {
                                    if (strError == string.Empty)
                                    {
                                        strError += item.ErrorFieldName;
                                    }
                                    else
                                    {
                                        strError += ", " + item.ErrorFieldName;
                                    }
                                    txt.BorderBrush = new SolidColorBrush(Colors.Red);
                                }
                                else
                                {
                                    txt.BorderBrush = new SolidColorBrush(Colors.Gray);
                                }

                            }
                        }
                        else
                        {
                            txt.BorderBrush = new SolidColorBrush(Colors.Gray);
                        }
                    }
                    if (item.ControlType == (int)PalashDynamics.ValueObjects.Controls.TimePicker)
                    {
                        TimePicker txt = (TimePicker)this.FindName(item.ControlName);
                        if (txt.IsEnabled)
                        {
                            if (txt.Value == null)
                            {
                                if (strError == string.Empty)
                                {
                                    strError += item.ErrorFieldName;
                                }
                                else
                                {
                                    strError += ", " + item.ErrorFieldName;
                                }
                                txt.BorderBrush = new SolidColorBrush(Colors.Red);
                            }
                            else
                            {
                                txt.BorderBrush = new SolidColorBrush(Colors.Gray);
                            }
                        }
                        else
                        {
                            txt.BorderBrush = new SolidColorBrush(Colors.Gray);
                        }
                    }
                    if (item.ControlType == (int)PalashDynamics.ValueObjects.Controls.DatePicker)
                    {
                        DatePicker txt = (DatePicker)this.FindName(item.ControlName);
                        if (txt.IsEnabled)
                        {
                            if (txt.SelectedDate == null)
                            {
                                if (strError == string.Empty)
                                {
                                    strError += item.ErrorFieldName;
                                }
                                else
                                {
                                    strError += ", " + item.ErrorFieldName;
                                }
                                txt.BorderBrush = new SolidColorBrush(Colors.Red);
                            }
                            else
                            {
                                txt.BorderBrush = new SolidColorBrush(Colors.Gray);
                            }
                        }
                        else
                        {
                            txt.BorderBrush = new SolidColorBrush(Colors.Gray);
                        }
                    }
                }
            }
            return strError;
        }

        string msgText = "";
        private void SaveMedicoLegalCase(clsIPDAdmMLCDetailsVO _MedicoLegalCase)
        {

            clsSaveIPDAdmissionBizActionVO BizAction = new clsSaveIPDAdmissionBizActionVO();
            BizAction.Details = ((IApplicationConfiguration)App.Current).SelectedIPDPatient;

            BizAction.AdmMLDCDetails = new clsIPDAdmMLCDetailsVO();
            BizAction.AdmMLDCDetails.PoliceStation = txtPoliceStation.Text;
            BizAction.AdmMLDCDetails.Number = txtNumber.Text;
            BizAction.AdmMLDCDetails.Authority = txtNameOfAutority.Text;
            BizAction.AdmMLDCDetails.Remark = txtMedicoLegalCaseRemark.Text;
            BizAction.AdmMLDCDetails.Address = txtAddress.Text;

            BizAction.AdmMLDCDetails.AttachedFileContent = AttachedFileContents;
            if (AttachedFileName == null)
                BizAction.AdmMLDCDetails.AttachedFileName = txtFileName.Text.Trim();
            else
                BizAction.AdmMLDCDetails.AttachedFileName = AttachedFileName;
            BizAction.AdmMLDCDetails.Title = txtTitle.Text.Trim();

            BizAction.IsMedicoLegalCase = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (((clsSaveIPDAdmissionBizActionVO)args.Result).Details != null)
                    {
                        msgText = "Record saved successfully.";
                        MessageBoxChildWindow msg = new MessageBoxChildWindow("", msgText, MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        msg.Show();
                        this.DialogResult = false;
                    }
                    else
                    {
                        msgText = "Error occurred while processing.";

                        MessageBoxChildWindow msg = new MessageBoxChildWindow("", msgText, MessageBoxButtons.Ok, MessageBoxIcon.Error);
                        msg.Show();
                    }
                }
                else
                {
                    msgText = "Error occurred while processing.";
                    MessageBoxChildWindow msg = new MessageBoxChildWindow("", msgText, MessageBoxButtons.Ok, MessageBoxIcon.Error);
                    msg.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser); // new clsUserVO());
            client.CloseAsync();

        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {

            this.DialogResult = false;

        }
        private void TextBoxNumber_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = CIMS.Comman.HandleNumber(sender, e);
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create an instance of the open file dialog box.
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                // Set filter options and filter index.
                openFileDialog1.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.Multiselect = true;
                // Call the ShowDialog method to show the dialog box.
                bool? userClickedOK = openFileDialog1.ShowDialog();
                // Process input if the user clicked OK.
                if (userClickedOK == true)
                {
                    // Open the selected file to read.
                    System.IO.Stream fileStream = openFileDialog1.File.OpenRead();
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                    {
                        // Read the first line from the file and write it the textbox.
                        //TxtReportPath.Text = reader.ReadLine();
                        TxtReportPath.Text = openFileDialog1.File.Name;
                    }
                    fileStream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgWindow =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Error while opening File Browser", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
            }
        }

        private void ViewLink_Click(object sender, RoutedEventArgs e)
        {

        }

        public class clsControlsValue
        {
            private long _ControlID;
            public long ControlID
            {
                get { return _ControlID; }
                set { _ControlID = value; }
            }

            private string _controlName;
            public string ControlName
            {
                get { return _controlName; }
                set { _controlName = value; }
            }

            private int _controlType;
            public int ControlType
            {
                get { return _controlType; }
                set { _controlType = value; }
            }

            private string _ErrorFieldName;
            public string ErrorFieldName
            {
                get { return _ErrorFieldName; }
                set { _ErrorFieldName = value; }
            }
        }

        private void txtFileName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void cmdAttachedDoc_Click(object sender, RoutedEventArgs e)
        {
            if (txtTitle.Text.Length == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Title", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
            }
            else
            {
                OpenFileDialog openDialog = new OpenFileDialog();
                if (openDialog.ShowDialog() == true)
                {
                    AttachedFileName = openDialog.File.Name;
                    txtFileName.Text = openDialog.File.Name;
                    try
                    {
                        using (Stream stream = openDialog.File.OpenRead())
                        {
                            AttachedFileContents = new byte[stream.Length];
                            stream.Read(AttachedFileContents, 0, (int)stream.Length);
                        }
                    }
                    catch (Exception ex)
                    {
                        string msgText = "Error while reading file.";

                        MessageBoxControl.MessageBoxChildWindow msgWindow =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                    }
                }
                if (txtFileName.Text != "")
                {
                    txtFileName.IsReadOnly = true;
                    CmdAddDocument.IsEnabled = true;
                    //CmdAddDocument_Click(sender, e);
                }
            }

        }

        private void CmdAddDocument_Click(object sender, RoutedEventArgs e)
        {

            if (txtTitle.Text.Length == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Title", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
            }
            //else if (txtDescription.Text.Length == 0)
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Description", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    msgW1.Show();

            //}
            else if (txtFileName.Text.Length == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Browse File", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
            }
        }


        #region private Methods
        private void SetPatientDetails()
        {
            if (IsPatientExist == true)
            {
                lblPatientName1.Text = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientName;
                lblAdmissionDate1.Text = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmissionDate.ToString().Substring(0, 10);
                lblAdmissionNo1.Text = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.IPDNO;
                lblMrNo1.Text = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.MRNo;
                lblPatientGender1.Text = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.GenderName;
            }
        }
        private List<clsControlsValue> ListControls()
        {
            List<clsControlsValue> TempList = new List<clsControlsValue>();
            clsControlsValue objItem = new clsControlsValue();
            TempList.Add(new clsControlsValue() { ControlName = "txtPoliceStation", ControlType = (int)PalashDynamics.ValueObjects.Controls.TextBox, ErrorFieldName = "Police Station" });
            TempList.Add(new clsControlsValue() { ControlName = "txtNameOfAutority", ControlType = (int)PalashDynamics.ValueObjects.Controls.TextBox, ErrorFieldName = "Name of Authority" });
            TempList.Add(new clsControlsValue() { ControlName = "txtNumber", ControlType = (int)PalashDynamics.ValueObjects.Controls.TextBox, ErrorFieldName = "Number" });
            return TempList;
        }
        #endregion

        private void hlkPreview_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsIPDAdmMLCDetailsVO MLCDetailsVO = this._MedicoLegalCaseVO;
                if (MLCDetailsVO.IsMLC == true)
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.UploadReportFileForMLCaseCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            Uri address1 = new Uri(Application.Current.Host.Source, "../PatientMlCaseDocuments");
                            string strFileName = address1.ToString();
                            strFileName = strFileName + "/" + MLCDetailsVO.AttachedFileName;
                            HtmlPage.Window.Invoke("open", new object[] { strFileName, "", "" });
                        }
                    };
                    client.UploadReportFileForMLCaseAsync(MLCDetailsVO.AttachedFileName, MLCDetailsVO.AttachedFileContent);
                    client.CloseAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}

