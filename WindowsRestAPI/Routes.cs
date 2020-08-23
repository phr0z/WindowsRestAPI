using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Shared;
using NAudio.CoreAudioApi;
using Newtonsoft.Json;
using SoundSwitch.Framework.Audio.Device;
using SoundSwitch.Framework.DeviceCyclerManager;
using WindowsRestAPI.Properties;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WindowsRestAPI
{
    [RestResource]
    public class APIResource
    {
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/display")]
        public IHttpContext SetMonitor(IHttpContext context)
        {
            responseMessage response = new responseMessage();
            int sNumber;
            response.message = "Please input windows display number.";
            response.successful = false;
            var monitor = context.Request.QueryString["primary"] ?? JsonConvert.SerializeObject(response);
            try
            {
                sNumber = Convert.ToInt16(monitor);
                switch (sNumber)
                {
                    case 1:
                        MonitorChanger.SetAsPrimaryMonitor(0);
                        break;
                    case 2:
                        MonitorChanger.SetAsPrimaryMonitor(1);
                        break;
                    case 3:
                        MonitorChanger.SetAsPrimaryMonitor(2);
                        break;
                    case 4:
                        MonitorChanger.SetAsPrimaryMonitor(3);
                        break;
                }
            }
            catch (Exception)
            {
                response.message = "Are you using a number as input?";
                response.successful = false;
                context.Response.SendResponse(JsonConvert.SerializeObject(response));
                return context;
            }

            response.message = $"Setting #{sNumber} as primary display.";
            response.successful = true;
            context.Response.SendResponse(JsonConvert.SerializeObject(response));
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/getid")]
        public IHttpContext GetSound(IHttpContext context)
        {
            responseMessage response = new responseMessage();
            response.message = "Something went wrong.";
            response.successful = false;
            List<soundDeviceInfo> sd = new List<soundDeviceInfo>();
            var enumerator = new MMDeviceEnumerator();

            foreach (var wasapi in enumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active))
            {
                try
                {
                    soundDeviceInfo sdi = new soundDeviceInfo();
                    sdi.ID = wasapi.ID;
                    sdi.FriendlyName = wasapi.FriendlyName;
                    sdi.State = wasapi.State.ToString();
                    sd.Add(sdi);
                    response.message = "Successfully got active audio device(s).";
                    response.successful = true;
                }
                catch (Exception)
                {
                    //avoid null exceptions
                    response.successful = false;
                }
            }

            context.Response.SendResponse(JsonConvert.SerializeObject(sd) + JsonConvert.SerializeObject(response));
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/set")]
        public IHttpContext SetSound(IHttpContext context)
        {
            responseMessage response = new responseMessage();
            response.message = "Please input correct id or name. Get id by using /getid";
            response.successful = false;
            var id = context.Request.QueryString["id"] ?? JsonConvert.SerializeObject(response);
            
            var enumerator = new MMDeviceEnumerator();
            bool changeResult = false;

            foreach (var wasapi in enumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active))
            {

                string wasapi_device_name = Regex.Replace(wasapi.FriendlyName, @"[^\u0041-\u007A]", string.Empty);
                string device_name = Regex.Replace(id, @"[^\u0041-\u007A]", string.Empty);

                try
                {
                    if (wasapi.ID == id)
                    {
                        DeviceFullInfo device = new DeviceFullInfo(wasapi.FriendlyName, wasapi.ID, wasapi.DataFlow, wasapi.IconPath, wasapi.State);
                        DeviceCyclerManager manager = new DeviceCyclerManager();
                        changeResult = manager.SetAsDefault(device);
                        response.message = $"Switched to audio device: {wasapi.FriendlyName}";
                    }
                    else if (wasapi_device_name == device_name)
                    {
                        DeviceFullInfo device = new DeviceFullInfo(wasapi.FriendlyName, wasapi.ID, wasapi.DataFlow, wasapi.IconPath, wasapi.State);
                        DeviceCyclerManager manager = new DeviceCyclerManager();
                        changeResult = manager.SetAsDefault(device);
                        response.message = $"Switched to audio device: {wasapi.FriendlyName}";
                    }
                }
                catch (Exception e)
                {
                    response.message = $"Failed to switch to audio device by name: {id}. Try using ID instead. {e}";
                    continue;
                }
            }

            response.successful = changeResult;
            context.Response.SendResponse(JsonConvert.SerializeObject(response));
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/run")]
        public IHttpContext RunCommand(IHttpContext context)
        {
            responseMessage response = new responseMessage();
            response.message = "Please input a correct value.";
            response.successful = false;
            var name = context.Request.QueryString["name"] ?? JsonConvert.SerializeObject(response);

            Commands command = new Commands();

            foreach (string s in command.GetCommands())
            {
                var decoded = command.Decoded(s.Trim());

                string appName = decoded.Name.ToLower();
                string appPath = decoded.Path;
                string appArguments = decoded.Arguments;
                
                if (name.ToLower() == appName)
                {
                    Process process = new Process();
                    process.StartInfo.FileName = appPath;
                    if(appArguments?.Length > 1)
                    {
                        process.StartInfo.Arguments = appArguments;
                    }
                    process.Start();
                    response.message = $"Successfully launched application. {appName}";
                    response.successful = true;
                }
            }
            context.Response.SendResponse(JsonConvert.SerializeObject(response));
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/stop")]
        public IHttpContext StopProcess(IHttpContext context)
        {
            List<responseMessage> responseList = new List<responseMessage>();
            responseMessage response = new responseMessage();
            response.message = "Please input a correct value.";
            response.successful = false;
            var name = context.Request.QueryString["name"] ?? JsonConvert.SerializeObject(response);

            Commands command = new Commands();

            foreach (string s in command.GetCommands())
            {
                var decoded = command.Decoded(s.Trim());

                string appName = decoded.Name.ToLower();
                string appPath = decoded.Path;
                string appArguments = decoded.Arguments;
                string executableFilename = Path.GetFileName(appPath).Replace(".exe","");

                if (name.ToLower() == appName)
                {
                    Process[] process = Process.GetProcesses(); 
                    foreach(Process p in process) 
                    { 
                        if (p.ProcessName.Contains(executableFilename)) 
                        {
                            try 
                            {
                                p.Kill();
                                response.message = $"Successfully stopped process. {p.ProcessName}";
                                response.successful = true;
                                responseList.Add(response);
                            }
                            catch (Exception e) 
                            {
                                response.message = $"Failed to stop process. {p.ProcessName} " + e.Message.ToString();
                                response.successful = false;
                                responseList.Add(response);
                            }
                        } 
                    }
                }
            }
            if (responseList.Count == 0)
            {
                response.message = $"Couldn't find any process with name {name}";
                response.successful = false;
                responseList.Add(response);
                context.Response.SendResponse(JsonConvert.SerializeObject(responseList));
                return context;
            }
            else 
            {
                context.Response.SendResponse(JsonConvert.SerializeObject(responseList));
                return context;
            }
        }
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/activewindow")]
        public IHttpContext ActiveWindow(IHttpContext context)
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            static extern IntPtr GetForegroundWindow();

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]            
            static extern int GetWindowTextLength(IntPtr hWnd);
         
            Process[] process = Process.GetProcesses();

            ActiveWindow activeWindow = new ActiveWindow();

            responseMessage response = new responseMessage();
            responseMessage response2 = new responseMessage();
            List<responseMessage> responseList = new List<responseMessage>();

            var strTitle = string.Empty;
            var handle = GetForegroundWindow();
            // Obtain the length of the text   
            var intLength = GetWindowTextLength(handle) + 1;
            var stringBuilder = new StringBuilder(intLength);
            if (GetWindowText(handle, stringBuilder, intLength) > 0)
            {
                activeWindow.WindowName = stringBuilder.ToString();
                response.message = "windowname";
                response.successful = true;
            }
            else
            {
                activeWindow.WindowName = null;
                response.message = "windowname";
                response.successful = false;
            }

            responseList.Add(response);

            foreach(Process p in process)
            {
                if (p.MainWindowTitle.Contains(activeWindow.WindowName))
                {
                    activeWindow.ProcessName = p.ProcessName;
                    activeWindow.StartTime = p.StartTime.ToString();
                    response2.message = "processname";
                    response2.successful = true;
                    break;
                }
                else
                {
                    activeWindow.ProcessName = null;
                    activeWindow.StartTime = null;
                    response2.message = "processname";
                    response2.successful = false;
                }
            }
            
            responseList.Add(response2);

            context.Response.SendResponse(JsonConvert.SerializeObject(activeWindow) + JsonConvert.SerializeObject(responseList));
            return context;
        }
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/sendmessage")]
        public IHttpContext SendMessage(IHttpContext context)
        {
            responseMessage response = new responseMessage();
            response.message = "Failed to parse message.";
            response.successful = false;
            var message = context.Request.QueryString["message"] ?? JsonConvert.SerializeObject(response);

            if (message != null || message.Length > 0)
            {
                response.message = message;
                response.successful = true;
                MessageBox.Show(message, "WindowsRestAPI", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            else
            {
                response.message = "Failed to parse message.";
                response.successful = false;
            }

            context.Response.SendResponse(JsonConvert.SerializeObject(response));
            return context;
        }
        [RestRoute]
        public IHttpContext Main(IHttpContext context)
        {
            responseMessage response = new responseMessage();
            response.message = "Welcome to Windows Rest API.";
            response.successful = true;
            context.Response.SendResponse(JsonConvert.SerializeObject(response));
            return context;
        }
    }
}
