using System.Windows;
using System.Windows.Browser;

namespace PalashDynamics
{
    public class ScriptableClass
    {
        [ScriptableMember]
        public void ShowAlertPopup(string message)
        {
            MessageBox.Show(message, "Message From JavaScript", MessageBoxButton.OK);
        }
    }
}
