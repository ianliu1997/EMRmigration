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

namespace PalashDynamics.IPD
{
    public partial class IVFDischargeDetails : UserControl
    {
        public IVFDischargeDetails()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
        }
        SwivelAnimation objAnimation;

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            objAnimation.Invoke(RotationType.Forward);
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            objAnimation.Invoke(RotationType.Backward);
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HyperlinkButton_Click_Female(object sender, RoutedEventArgs e)
        {

        }

        private void HyperlinkButton_Click_Male(object sender, RoutedEventArgs e)
        {

        }
    }
}
