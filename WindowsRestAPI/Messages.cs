using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;
using WindowsRestAPI.Properties;

namespace WindowsRestAPI
{
    public class Messages
    {
        public static void Show(string Message)
        {
            if(Settings.Default.MessageBox)
            {
                MessageBox.Show(Message, "WindowsRestAPI", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            
            if(Settings.Default.NotificationCenter)
            {
                // Construct the visuals of the toast (using Notifications library)
                ToastContent toastContent = new ToastContentBuilder()
                    .AddToastActivationInfo("action=viewConversation&conversationId=5", ToastActivationType.Foreground)
                    .AddText(Message)
                    .GetToastContent();

                // And create the toast notification
                var toast = new ToastNotification(toastContent.GetXml());

                // And then show it
                DesktopNotificationManagerCompat.CreateToastNotifier().Show(toast);
            }
        }
    }
}
