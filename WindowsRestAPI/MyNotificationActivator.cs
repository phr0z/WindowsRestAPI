using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Toolkit.Uwp.Notifications.NotificationActivator;

namespace WindowsRestAPI
{
    class MyNotificationActivator
    {
        // The GUID CLSID must be unique to your app. Create a new GUID if copying this code.
        [ClassInterface(ClassInterfaceType.None)]
        [ComSourceInterfaces(typeof(INotificationActivationCallback))]
        [Guid("b11690b2-55a9-4339-a2ea-8ada1c99e203"), ComVisible(true)]
        public class ThisNotificationActivator : NotificationActivator
        {
            public override void OnActivated(string invokedArgs, NotificationUserInput userInput, string appUserModelId)
            {
                // TODO: Handle activation
            }
        }
    }
}
