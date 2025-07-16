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
using System.IO;
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmDocumentforDashTherapy : UserControl
    {
        public clsCoupleVO CoupleDetails;
        public long PlanTherapyID;
        public long PlanTherapyUnitID;
        byte[] AttachedFileContents;
        string AttachedFileName;
        public bool IsClosed;
        System.Collections.ObjectModel.ObservableCollection<string> AttachedFileNameList = new System.Collections.ObjectModel.ObservableCollection<string>();

        public frmDocumentforDashTherapy()
        {
            InitializeComponent();
        }

        private void cmdAttachedDoc_Click(object sender, RoutedEventArgs e)
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
                        new MessageBoxControl.MessageBoxChildWindow("Palsh", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                    throw ex;
                }
            }

        }

        private bool validation() 
        {
            bool result = true;
            if (txtTitle.Text.Length == 0)
            {
                txtTitle.SetValidation("Please Enter Title");
                txtTitle.RaiseValidationError();
                txtTitle.Focus();
                result = false;
            }
            else if (txtDescription.Text.Length == 0)
            {
                txtTitle.ClearValidationError();
                txtDescription.SetValidation("Please Enter Description");
                txtDescription.RaiseValidationError();
                txtDescription.Focus();
                result = false;
            }
            else if (txtFileName.Text.Length == 0)
            {
                txtTitle.ClearValidationError();
                txtDescription.ClearValidationError();
                txtDescription.SetValidation("Please Browse File");
                txtDescription.RaiseValidationError();
                txtDescription.Focus();
                result = false;
            }
            else if (DocList.Count > 0)
            {
                if (DocList.Where(Items => Items.Title == txtTitle.Text).Any() == true)
                {
                    txtTitle.SetValidation("Title cannot be same");
                    txtTitle.RaiseValidationError();
                    txtTitle.Focus();
                    result = false;
                }
                else if (DocList.Where(Items => Items.Description == txtDescription.Text).Any() == true)
                {
                    txtTitle.ClearValidationError();
                    txtDescription.SetValidation("Description cannot be same");
                    txtDescription.RaiseValidationError();
                    txtDescription.Focus();
                    result = false;
                }
                else
                {
                      txtTitle.ClearValidationError();
                      txtDescription.ClearValidationError();
                    result=true;
                }

            }
            else
            {
                txtTitle.ClearValidationError();
                txtDescription.ClearValidationError();
                txtFileName.ClearValidationError();
                result = true;
 
            }

           
            return result;
        }
        private void CmdAddDocument_Click(object sender, RoutedEventArgs e)
        {
            if (validation())
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure \n  You Want To Save Document";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += (result) =>
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        save();
                        txtTitle.Text = "";
                        txtDescription.Text = "";
                        txtFileName.Text = "";
                    }
                    else
                    {
                        txtTitle.Text = "";
                        txtDescription.Text = "";
                        txtFileName.Text = "";
                    }
                };
                msgWin.Show();
            }
        }
        private void save() 
        {
            clsIVFDashboard_AddTherapyDocumentBizActionVO BizAction = new clsIVFDashboard_AddTherapyDocumentBizActionVO();
            BizAction.Details = new clsIVFDashboard_TherapyDocumentVO();
            BizAction.Details.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.Details.PlanTherapyID = PlanTherapyID;
            BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitID;
             BizAction.Details.Date = DateTime.Now;
            BizAction.Details.Title = txtTitle.Text;
            BizAction.Details.Description = txtDescription.Text;
            BizAction.Details.AttachedFileName = txtFileName.Text;
            BizAction.Details.AttachedFileContent = AttachedFileContents;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_AddTherapyDocumentBizActionVO)arg.Result).SuccessStatus == 1)
                    {
                        string msgTitle = "Palash";
                        string msgText = "Doument Saved Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgWin =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);

                        msgWin.OnMessageBoxClosed += (result) =>
                        {
                            if (result == MessageBoxResult.OK)
                            {
                                FillDocumentGrid();
                            }
                        };
                        msgWin.Show();

                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private List<clsIVFDashboard_TherapyDocumentVO> DocList;
        private void FillDocumentGrid() 
        {
            clsIVFDashboard_GetTherapyDocumentBizActionVO BizAction = new clsIVFDashboard_GetTherapyDocumentBizActionVO();
            BizAction.Details = new clsIVFDashboard_TherapyDocumentVO();
            BizAction.DetailList = new List<clsIVFDashboard_TherapyDocumentVO>();
            BizAction.Details.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.Details.PlanTherapyID = PlanTherapyID;
            BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    DocList = new List<clsIVFDashboard_TherapyDocumentVO>();
                    if (((clsIVFDashboard_GetTherapyDocumentBizActionVO)args.Result).DetailList != null)
                    {
                        clsIVFDashboard_GetTherapyDocumentBizActionVO result = args.Result as clsIVFDashboard_GetTherapyDocumentBizActionVO;
                        if (result.DetailList != null)
                        {
                            foreach (var item in result.DetailList)
                            {
                                DocList.Add(item);
                            }
                        }
                    
                    }
                    dgDocumentGrid.ItemsSource = null;
                    dgDocumentGrid.ItemsSource = DocList;
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("PALASH", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdDeleteDoc_Click(object sender, RoutedEventArgs e)
        {
            if (dgDocumentGrid.SelectedItem != null)
            {
                if (IsClosed != true)
                {
                    try
                    {

                        clsIVFDashboard_DeleteTherapyDocumentBizActionVO BizActionVO = new clsIVFDashboard_DeleteTherapyDocumentBizActionVO();
                        BizActionVO.Details = new clsIVFDashboard_TherapyDocumentVO();
                        BizActionVO.Details.ID = ((clsIVFDashboard_TherapyDocumentVO)dgDocumentGrid.SelectedItem).ID;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null && arg.Result != null)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "Document Details Deleted Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                                FillDocumentGrid();

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

        private void hpyrlinkFileView_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((clsIVFDashboard_TherapyDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileName))
            {
                if (((clsIVFDashboard_TherapyDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileContent != null)
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + ((clsIVFDashboard_TherapyDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileName });
                            AttachedFileNameList.Add(((clsIVFDashboard_TherapyDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileName);
                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", ((clsIVFDashboard_TherapyDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileName, ((clsIVFDashboard_TherapyDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileContent);
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsClosed == true)
            {
                CmdAddDocument.IsEnabled = false;
            }
            DocList = new List<clsIVFDashboard_TherapyDocumentVO>();
            FillDocumentGrid();
        }
    }
}
