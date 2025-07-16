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
using System.Windows.Controls.Primitives;
using PalashDynamics.Controls;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.IVF
{
    public class IVFTreatmentPlanRepeterControlItem : ListBoxItem
    {
        public IVFTreatmentPlanRepeterControlItem()
        {
            this.DefaultStyleKey = typeof(IVFTreatmentPlanRepeterControlItem);
        }
        
        private TextBox txtOocyteNO;
        private AutoCompleteComboBox cmbCumulus;
        private AutoCompleteComboBox cmbGrade;        
        //private ToggleButton btnDetail;
        private HyperlinkButton CmdMediaDetails;
        //private AutoCompleteComboBox cmbMedia;
        private AutoCompleteComboBox cmbMOI;
        private TextBox txtScore;
        private CheckBox chkProceedToDay;
        private AutoCompleteComboBox cmbPlan        ;

        private HyperlinkButton cmdView ;
        private TextBox txtFileName;
        private ToggleButton cmdBrowse;

        private HyperlinkButton AddRemoveClick;


        public event RoutedEventHandler OnAddRemoveClick;
        public event RoutedEventHandler OnCmdMediaDetailClick;
        public event RoutedEventHandler OnchkProceedToDayClick;
        public event SelectionChangedEventHandler OnSelectionChanged;
        public event RoutedEventHandler OnBrowseClick;
        public event RoutedEventHandler OnViewClick;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.txtOocyteNO = base.GetTemplateChild("txtOocyteNO") as TextBox;
            this.cmbCumulus = base.GetTemplateChild("cmbCumulus") as AutoCompleteComboBox;
            this.cmbGrade = base.GetTemplateChild("cmbGrade") as AutoCompleteComboBox;
            //this.btnDetail = base.GetTemplateChild("btnDetail") as ToggleButton;
            this.CmdMediaDetails = base.GetTemplateChild("CmdMediaDetails") as HyperlinkButton;
            //this.cmbMedia = base.GetTemplateChild("cmbMedia") as AutoCompleteComboBox;
            this.cmbMOI = base.GetTemplateChild("cmbMOI") as AutoCompleteComboBox;
            this.txtScore = base.GetTemplateChild("txtScore") as TextBox;
            this.chkProceedToDay = base.GetTemplateChild("chkProceedToDay") as CheckBox;
            this.cmbPlan = base.GetTemplateChild("cmbPlan") as AutoCompleteComboBox;
            this.cmdView = base.GetTemplateChild("cmdView") as HyperlinkButton;
            this.txtFileName = base.GetTemplateChild("txtFileName") as TextBox;
            this.cmdBrowse = base.GetTemplateChild("cmdBrowse") as ToggleButton;
            
            this.AddRemoveClick = base.GetTemplateChild("CmdAddRemove") as HyperlinkButton;
           // this.txtOocyteNO.Tag = "OocyteNO";

            //By Anjali.........
            this.txtOocyteNO.Tag = "SerialOccyteNo";
            //......................
            this.cmbCumulus.Tag = "Cumulus";
            this.cmbGrade.Tag = "Grade";
            //this.btnDetail.Tag = "Detail";
            this.CmdMediaDetails.Tag = "Media";
            //this.cmbMedia.Tag = "Media";
            this.cmbMOI.Tag = "MOI";
            this.txtScore.Tag = "Score";
            this.chkProceedToDay.Tag = "ProceedToDay";
            this.cmbPlan.Tag = "Plan";
            this.txtOocyteNO.Text = "Auto Generated";            
            this.txtOocyteNO.IsReadOnly = true;
            this.txtFileName.IsReadOnly = true;
            this.txtFileName.Tag = "FileName";
            this.cmdView.Tag = "ViewFile";
            this.cmdBrowse.Tag = "BrowseFile";

            this.AddRemoveClick.Click += new RoutedEventHandler(this.CmdAddRemoveClick);
            this.chkProceedToDay.Click += new RoutedEventHandler(chkProceedToDay_Click);
            this.CmdMediaDetails.Click += new RoutedEventHandler(CmdMediaDetails_Click);
            //this.cmbMedia.SelectionChanged += new SelectionChangedEventHandler(cmb_SelectionChanged);
            this.cmbCumulus.SelectionChanged += new SelectionChangedEventHandler(cmb_SelectionChanged);
            this.cmbGrade.SelectionChanged += new SelectionChangedEventHandler(cmb_SelectionChanged);
            this.cmbMOI.SelectionChanged += new SelectionChangedEventHandler(cmb_SelectionChanged);
            this.cmbPlan.SelectionChanged += new SelectionChangedEventHandler(cmb_SelectionChanged);
            this.cmdView.Click += new RoutedEventHandler(View_Click);
            this.cmdBrowse.Click += new RoutedEventHandler(Browse_Click);
            
            if (this.DataContext != null)
            {
                //if(((IVFTreatment)this.DataContext).OocyteNO >0)
                //{
                //    this.txtOocyteNO.Text = ((IVFTreatment)this.DataContext).OocyteNO.ToString();
                //}


                //By Anjali..........

                if (((IVFTreatment)this.DataContext).SerialOccyteNo > 0)
                {
                    this.txtOocyteNO.Text = ((IVFTreatment)this.DataContext).SerialOccyteNo.ToString();
                }
                //.....................
                if (((IVFTreatment)this.DataContext).Cumulus != null)
                {
                    this.cmbCumulus.SelectedValue = ((IVFTreatment)this.DataContext).Cumulus.ID;
                }
                if (((IVFTreatment)this.DataContext).Grade != null)
                {
                    this.cmbGrade.SelectedValue = ((IVFTreatment)this.DataContext).Grade.ID;
                }
                if (((IVFTreatment)this.DataContext).MOI != null)
                {
                    this.cmbMOI.SelectedValue = ((IVFTreatment)this.DataContext).MOI.ID;
                }
                if (((IVFTreatment)this.DataContext).Plan != null)
                {
                    this.cmbPlan.SelectedValue = ((IVFTreatment)this.DataContext).Plan.ID;
                }
                //if (((IVFTreatment)this.DataContext).Media != null)
                //{
                //    this.cmbMedia.SelectedValue = ((IVFTreatment)this.DataContext).Media.ID;
                //}
                this.chkProceedToDay.IsChecked = ((IVFTreatment)this.DataContext).ProceedToDay;
            }
        }

        void View_Click(object sender, RoutedEventArgs e)
        {
            if (this.OnViewClick != null)
            {
                this.OnViewClick(sender, e);
            }
        }

        void Browse_Click(object sender, RoutedEventArgs e)
        {
            if (this.OnBrowseClick != null)
            {
                this.OnBrowseClick(sender, e);
            }
        }

        void CmdMediaDetails_Click(object sender, RoutedEventArgs e)
        {
            if (this.OnCmdMediaDetailClick != null)
            {
                this.OnCmdMediaDetailClick(sender, e);             
            }
        }

        void cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.OnSelectionChanged != null)
            {
                this.OnSelectionChanged(sender, e);
            }
        }

        void chkProceedToDay_Click(object sender, RoutedEventArgs e)
        {
            if (this.OnchkProceedToDayClick != null)
            {
                this.OnchkProceedToDayClick(sender, e);
            }
        }

        public void CmdAddRemoveClick(object sender, RoutedEventArgs e)
        {
            if (this.OnAddRemoveClick != null)
            {
                this.OnAddRemoveClick(sender, e);
            }
        }
    }

    public class ICSITreatmentPlanRepeterControlItem : ListBoxItem
    {
        public ICSITreatmentPlanRepeterControlItem()
        {
            this.DefaultStyleKey = typeof(ICSITreatmentPlanRepeterControlItem);
        }

        private TextBox txtOocyteNO;
        private TextBox txtMBD;
        private AutoCompleteComboBox cmbDOS;
        private TextBox txtComment;
        private AutoCompleteComboBox cmbPIC;
        private HyperlinkButton CmdMediaDetails;       
        //private AutoCompleteComboBox cmbMedia;
        private TextBox txtIC;
        private CheckBox chkProceedToDay;
        private AutoCompleteComboBox cmbPlan;
        private HyperlinkButton AddRemoveClick;
        private HyperlinkButton cmdView;
        private TextBox txtFileName;
        private ToggleButton cmdBrowse;
        public event RoutedEventHandler OnAddRemoveClick;
        public event RoutedEventHandler OnCmdMediaDetailClick;
        public event RoutedEventHandler OnchkProceedToDayClick;
        public event SelectionChangedEventHandler OnSelectionChanged;
        public event RoutedEventHandler OnBrowseClick;
        public event RoutedEventHandler OnViewClick;


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.txtOocyteNO = base.GetTemplateChild("txtOocyteNO") as TextBox;
            this.txtMBD = base.GetTemplateChild("txtMBD") as TextBox;
            this.cmbDOS = base.GetTemplateChild("cmbDOS") as AutoCompleteComboBox;
            this.txtComment = base.GetTemplateChild("txtComment") as TextBox;
            this.cmbPIC = base.GetTemplateChild("cmbPIC") as AutoCompleteComboBox;
            this.CmdMediaDetails = base.GetTemplateChild("CmdMediaDetails") as HyperlinkButton;            
            //this.cmbMedia = base.GetTemplateChild("cmbMedia") as AutoCompleteComboBox;
            this.txtIC = base.GetTemplateChild("txtIC") as TextBox;
            this.chkProceedToDay = base.GetTemplateChild("chkProceedToDay") as CheckBox;
            this.cmbPlan = base.GetTemplateChild("cmbPlan") as AutoCompleteComboBox;
            this.AddRemoveClick = base.GetTemplateChild("CmdAddRemove") as HyperlinkButton;
            this.cmdView = base.GetTemplateChild("cmdView") as HyperlinkButton;
            this.txtFileName = base.GetTemplateChild("txtFileName") as TextBox;
            this.cmdBrowse = base.GetTemplateChild("cmdBrowse") as ToggleButton;

            this.txtOocyteNO.Tag = "OocyteNO";
            this.txtMBD.Tag = "MBD";
            this.cmbDOS.Tag = "DOS";
            this.txtComment.Tag = "Comment";
            this.cmbPIC.Tag = "PIC";
            this.CmdMediaDetails.Tag = "Media";
            //this.cmbMedia.Tag = "Media";
            this.txtIC.Tag = "IC";
            this.chkProceedToDay.Tag = "ProceedToDay";
            this.cmbPlan.Tag = "Plan";
            this.txtOocyteNO.Text = "Auto Generated";
            this.txtOocyteNO.IsReadOnly = true;
            this.txtFileName.IsReadOnly = true;
            this.txtFileName.Tag = "FileName";
            this.cmdView.Tag = "ViewFile";
            this.cmdBrowse.Tag = "BrowseFile";

            this.AddRemoveClick.Click += new RoutedEventHandler(this.CmdAddRemoveClick);
            this.chkProceedToDay.Click+=new RoutedEventHandler(chkProceedToDay_Click);
            this.CmdMediaDetails.Click += new RoutedEventHandler(CmdMediaDetails_Click);
            //this.cmbMedia.SelectionChanged += new SelectionChangedEventHandler(cmb_SelectionChanged);
            this.cmbDOS.SelectionChanged += new SelectionChangedEventHandler(cmb_SelectionChanged);
            this.cmbPIC.SelectionChanged += new SelectionChangedEventHandler(cmb_SelectionChanged);
            this.cmbPlan.SelectionChanged += new SelectionChangedEventHandler(cmb_SelectionChanged);
            this.cmdBrowse.Click += new RoutedEventHandler(Browse_Click);
            this.cmdView.Click += new RoutedEventHandler(View_Click);

            if (this.DataContext != null)
            {
                if (((ICSITreatment)this.DataContext).OocyteNO >0)
                {
                    this.txtOocyteNO.Text = ((ICSITreatment)this.DataContext).OocyteNO.ToString();
                }
                if (((ICSITreatment)this.DataContext).DOS != null)
                {
                    this.cmbDOS.SelectedValue = ((ICSITreatment)this.DataContext).DOS.ID;
                }
                if (((ICSITreatment)this.DataContext).PIC != null)
                {
                    this.cmbPIC.SelectedValue = ((ICSITreatment)this.DataContext).PIC.ID;
                }                
                if (((ICSITreatment)this.DataContext).Plan != null)
                {
                    this.cmbPlan.SelectedValue = ((ICSITreatment)this.DataContext).Plan.ID;
                }
                //if (((ICSITreatment)this.DataContext).Media != null)
                //{
                //    this.cmbMedia.SelectedValue = ((ICSITreatment)this.DataContext).Media.ID;
                //}
                this.chkProceedToDay.IsChecked = ((ICSITreatment)this.DataContext).ProceedToDay;
            }
        }

        void View_Click(object sender, RoutedEventArgs e)
        {
            if (this.OnViewClick != null)
            {
                this.OnViewClick(sender, e);
            }
        }

        void Browse_Click(object sender, RoutedEventArgs e)
        {
            if (this.OnBrowseClick != null)
            {
                this.OnBrowseClick(sender, e);
            }
        }
        void CmdMediaDetails_Click(object sender, RoutedEventArgs e)
        {
            if (this.OnCmdMediaDetailClick != null)
            {
                this.OnCmdMediaDetailClick(sender, e);
            }
        }

        void cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.OnSelectionChanged != null)
            {
                this.OnSelectionChanged(sender, e);
            }
        }

        void  chkProceedToDay_Click(object sender, RoutedEventArgs e)
        {
            if (this.OnchkProceedToDayClick != null)
            {
                this.OnchkProceedToDayClick(sender, e);
            }
        }

        public void CmdAddRemoveClick(object sender, RoutedEventArgs e)
        {
            if (this.OnAddRemoveClick != null)
            {
                this.OnAddRemoveClick(sender, e);
            }
        }
    }

    public class FileUploadRepeterControlItem : ListBoxItem
    {
        public FileUploadRepeterControlItem()
        {
            this.DefaultStyleKey = typeof(FileUploadRepeterControlItem);
        }

        private HyperlinkButton View;
        private HyperlinkButton AddRemoveClick;

        private TextBox FileName;
        private Button Browse;

        public event RoutedEventHandler OnAddRemoveClick;
        public event RoutedEventHandler OnViewClick;
        public event RoutedEventHandler OnBrowseClick;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.AddRemoveClick = base.GetTemplateChild("CmdAddRemove") as HyperlinkButton;
            this.View = base.GetTemplateChild("View") as HyperlinkButton;

            this.Browse = base.GetTemplateChild("Browse") as Button;
            this.FileName = base.GetTemplateChild("FileName") as TextBox;

            this.AddRemoveClick.Click += new RoutedEventHandler(this.CmdAddRemoveClick);
            this.View.Click += new RoutedEventHandler(View_Click);
            this.Browse.Click += new RoutedEventHandler(Browse_Click);

            ToolTip tt = new ToolTip();
            TextBlock tbl = new TextBlock();
            tbl.TextWrapping = TextWrapping.Wrap;
            tbl.Text = "";
            if (this.DataContext != null)
            {
                if (((FileUpload)this.DataContext).FileName != null)
                {
                    this.FileName.Text = ((FileUpload)this.DataContext).FileName.ToString();
                }
            }
        }

        void Browse_Click(object sender, RoutedEventArgs e)
        {
            if (this.OnBrowseClick != null)
            {
                this.OnBrowseClick(sender, e);
            }
        }

        void View_Click(object sender, RoutedEventArgs e)
        {
            if (this.OnViewClick != null)
            {
                this.OnViewClick(sender, e);
            }
        }

        public void CmdAddRemoveClick(object sender, RoutedEventArgs e)
        {
            if (this.OnAddRemoveClick != null)
            {
                this.OnAddRemoveClick(sender, e);
            }
        }
    }
}
