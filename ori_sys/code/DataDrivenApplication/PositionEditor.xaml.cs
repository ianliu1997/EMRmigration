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

namespace DataDrivenApplication
{
    public partial class PositionEditor : ChildWindow
    {
        public FormDetail Form { get; set; }
        List<SectionDetail> lstSection = null;
        List<FieldDetail> lstField = null;
        SectionDetail SelectedSection = null;
        public PositionEditor(FormDetail Form)
        {
            if (Form == null)
            {
                throw new ArgumentNullException();
            }
            InitializeComponent();

            this.Form = Form;
            this.Loaded += new RoutedEventHandler(PositionEditor_Loaded);
        }

        void PositionEditor_Loaded(object sender, RoutedEventArgs e)
        {
            SectionOptions.Visibility = Visibility.Collapsed;
            ControlType.Items.Add("Section");
            ControlType.Items.Add("Field");
            lstSection=Form.SectionList;
            cmbSectionList.ItemsSource=Form.SectionList;
        }

        public event RoutedEventHandler OnSaveButtonClick;
        

        private void ControlType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ControlType.SelectedItem != null)
            {
                if (ControlType.SelectedValue == "Section")
                {
                    SectionOptions.Visibility = Visibility.Collapsed;
                    lstElement.ItemsSource = null;
                    lstElement.ItemsSource = lstSection;
                    tblFieldList.Visibility = Visibility.Collapsed;
                    tblSectionList.Visibility = Visibility.Visible;
                }
                else if (ControlType.SelectedValue == "Field")
                {
                    lstElement.ItemsSource = null;
                    cmbSectionList.SelectedItem = null;
                    SectionOptions.Visibility = Visibility.Visible;
                    tblSectionList.Visibility = Visibility.Collapsed;
                    tblFieldList.Visibility = Visibility.Visible;                    
                }
            }
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            if (lstElement.SelectedItem != null)
            {
                if (lstElement.SelectedIndex != 0)
                {
                    int lstart = lstElement.SelectedIndex;

                    List<SectionDetail> sdlist = null;
                    List<SectionDetail> sdlist1 = null;
                    List<FieldDetail> fdlist = null;
                    List<FieldDetail> fdlist1 = null;

                    if (SectionOptions.Visibility == Visibility.Collapsed)
                    {
                        sdlist = lstSection;
                        sdlist1 = sdlist.DeepCopy();

                        sdlist.RemoveAt(lstart);
                        sdlist.Insert(lstart - 1, sdlist1[lstart]);

                        this.lstElement.ItemsSource = null;
                        this.lstElement.ItemsSource = sdlist;
                        this.Form.SectionList = sdlist;
                    }
                    else
                    {
                        fdlist = lstField;
                        fdlist1 = fdlist.DeepCopy();

                        fdlist.RemoveAt(lstart);
                        fdlist.Insert(lstart - 1, fdlist1[lstart]);

                        this.lstElement.ItemsSource = null;
                        this.lstElement.ItemsSource = fdlist;
                        SelectedSection.FieldList = fdlist;
                    }

                    
                }
            }
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            if (lstElement.SelectedItem != null)
            {
                int lstart = lstElement.SelectedIndex;

                if (SectionOptions.Visibility == Visibility.Collapsed)
                {

                    List<SectionDetail> sdlist = lstSection;
                    List<SectionDetail> sdlist1 = null;

                    if (lstElement.SelectedIndex != sdlist.Count - 1)
                    {
                        sdlist1 = sdlist.DeepCopy();

                        sdlist.RemoveAt(lstart);

                        if (lstart == sdlist.Count)
                            sdlist.Add(sdlist1[lstart]);
                        else
                            sdlist.Insert(lstart + 1, sdlist1[lstart]);


                        this.lstElement.ItemsSource = null;
                        this.lstElement.ItemsSource = sdlist;
                        this.Form.SectionList = sdlist;
                    }
                }
                else
                {
                    List<FieldDetail> fdlist = lstField;
                    List<FieldDetail> fdlist1 = null;
                    if (lstElement.SelectedIndex != fdlist.Count - 1)
                    {

                        fdlist1 = fdlist.DeepCopy();

                        fdlist.RemoveAt(lstart);

                        if (lstart == fdlist.Count)
                            fdlist.Add(fdlist1[lstart]);
                        else
                            fdlist.Insert(lstart + 1, fdlist1[lstart]);
                        
                        this.lstElement.ItemsSource = null;
                        this.lstElement.ItemsSource = fdlist;
                        SelectedSection.FieldList = fdlist;
                    }
                }
            }
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnSaveButtonClick != null)
            {
                OnSaveButtonClick((this.Form), e);
            }
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmbSectionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSectionList.SelectedItem != null)
            {
                SelectedSection = (SectionDetail)cmbSectionList.SelectedItem;
                SectionDetail sd = (SectionDetail)cmbSectionList.SelectedItem;
                lstElement.ItemsSource = null;
                lstField = null;
                lstField = sd.FieldList;
                lstElement.ItemsSource = sd.FieldList;

            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
    }
}

