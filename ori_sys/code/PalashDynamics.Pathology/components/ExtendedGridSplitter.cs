using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PalashDynamics.Pathology.components
{
    [TemplatePart(Name = ExtendedGridSplitter.VerticalCollapseButtonElement, Type = typeof(Button))]
    [TemplatePart(Name = ExtendedGridSplitter.HorizontalCollapseButtonElement, Type = typeof(Button))]

    public class ExtendedGridSplitter : GridSplitter
    {
        private const string VerticalCollapseButtonElement = "VerticalCollapseButton";
        private const string HorizontalCollapseButtonElement = "HorizontalCollapseButton";

        protected Button VerticalCollapseButton;
        protected Button HorizontalCollapseButton;
        public ExtendedGridSplitter()
        {
            this.DefaultStyleKey = typeof(ExtendedGridSplitter);
            VerticalCollapseButton = new Button();
            HorizontalCollapseButton = new Button();
        }

        #region CollapseButtonString

        public static readonly DependencyProperty CollapseButtonStringProperty = DependencyProperty.Register("CollapseButtonString", typeof(string), typeof(ExtendedGridSplitter), null);

        public string CollapseButtonString
        {
            get { return (string)GetValue(CollapseButtonStringProperty); }
            set
            {
                SetValue(CollapseButtonStringProperty, value);
                VerticalCollapseButton.Content = value;
                HorizontalCollapseButton.Content = value;
            }
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            VerticalCollapseButton = this.GetTemplateChild(VerticalCollapseButtonElement) as Button;
            if (VerticalCollapseButton != null)
            {
                VerticalCollapseButton.Click += new RoutedEventHandler(OnCollapseButtonClickEvent);
            }

            HorizontalCollapseButton = this.GetTemplateChild(HorizontalCollapseButtonElement) as Button;
            if (HorizontalCollapseButton != null)
            {
                HorizontalCollapseButton.Click += new RoutedEventHandler(OnCollapseButtonClickEvent);
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            VerticalCollapseButton.Width = availableSize.Width;// -2;
            HorizontalCollapseButton.Height = availableSize.Height;// -2;
            return base.MeasureOverride(availableSize);
        }

        public delegate void CollapseButtonClickEventHandler(object sender, EventArgs e);
        public event CollapseButtonClickEventHandler CollapseButtonClickEvent;
        void OnCollapseButtonClickEvent(object sender, RoutedEventArgs e)
        {
            if (CollapseButtonClickEvent != null) CollapseButtonClickEvent(sender, e);
        }

    }
}

