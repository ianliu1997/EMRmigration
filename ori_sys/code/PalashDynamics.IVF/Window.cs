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

namespace PalashDynamics.IVF.DashBoard
{
        public class Window : ContentControl
        {
            // Fields
            private Button closeButton;
            private bool disableCloseButton;

            public Window()
            {
                this.DefaultStyleKey = typeof(Window);
            }

            public override void OnApplyTemplate()
            {
                base.OnApplyTemplate();
                this.closeButton = base.GetTemplateChild("Part_CloseButton") as Button;
                this.closeButton.IsEnabled = !this.DisableCloseButton;
                this.closeButton.Click += new RoutedEventHandler(this.CloseWindow);
            }

            public event RoutedEventHandler OnCloseButtonClick;

            // Properties
            public bool DisableCloseButton
            {
                get
                {
                    return this.disableCloseButton;
                }
                set
                {
                    this.disableCloseButton = value;
                    if (this.closeButton != null)
                    {
                        this.closeButton.IsEnabled = !this.DisableCloseButton;
                    }
                }
            }


            public void CloseWindow(object sender, RoutedEventArgs e)
            {
                if (this.OnCloseButtonClick != null)
                {
                    this.OnCloseButtonClick(sender, e);
                }
            }

        }
   
}
