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
using System.Linq;

namespace WindowsRestAPI
{
    [RestResource]
    public class APIResource
    {
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/display")]
        public IHttpContext SetMonitor(IHttpContext context)
        {
            ResponseMessage Response = new ResponseMessage();
            int sNumber;
            Response.Message = "Please input windows display number.";
            Response.Successful = false;
            var monitor = context.Request.QueryString["primary"] ?? JsonConvert.SerializeObject(Response);
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
                Response.Message = "Are you using a number as input?";
                Response.Successful = false;
                context.Response.SendResponse(JsonConvert.SerializeObject(Response));
                return context;
            }

            Response.Message = $"Setting #{sNumber} as primary display.";
            Response.Successful = true;
            context.Response.SendResponse(JsonConvert.SerializeObject(Response));
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/getid")]
        public IHttpContext GetSound(IHttpContext context)
        {
            SoundDevices sd = new SoundDevices();
            var enumerator = new MMDeviceEnumerator();

            foreach (var wasapi in enumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active))
            {
                try
                {
                    sd.SoundDevice.Add(new SoundDeviceInfo
                    {
                        ID = wasapi.ID,
                        FriendlyName = wasapi.FriendlyName,
                        State = wasapi.State.ToString()
                    });

                    sd.Response = new ResponseMessage
                    {
                        Message = "Successfully got active audio device(s).",
                        Successful = true
                    };
                }
                catch (Exception)
                {
                    sd.Response = new ResponseMessage
                    {
                        Message = "Failed to get audio device(s).",
                        Successful = false
                    };
                }
            }

            context.Response.SendResponse(JsonConvert.SerializeObject(sd));
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/set")]
        public IHttpContext SetSound(IHttpContext context)
        {
            ResponseMessage Response = new ResponseMessage();
            Response.Message = "Please input correct id or name. Get id by using /getid";
            Response.Successful = false;
            var id = context.Request.QueryString["id"] ?? JsonConvert.SerializeObject(Response);
            
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
                        Response.Message = $"Switched to audio device: {wasapi.FriendlyName}";
                    }
                    else if (wasapi_device_name == device_name)
                    {
                        DeviceFullInfo device = new DeviceFullInfo(wasapi.FriendlyName, wasapi.ID, wasapi.DataFlow, wasapi.IconPath, wasapi.State);
                        DeviceCyclerManager manager = new DeviceCyclerManager();
                        changeResult = manager.SetAsDefault(device);
                        Response.Message = $"Switched to audio device: {wasapi.FriendlyName}";
                    }
                }
                catch (Exception e)
                {
                    Response.Message = $"Failed to switch to audio device by name: {id}. Try using ID instead. {e}";
                    continue;
                }
            }

            Response.Successful = changeResult;
            context.Response.SendResponse(JsonConvert.SerializeObject(Response));
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/run")]
        public IHttpContext RunCommand(IHttpContext context)
        {
            ResponseMessage Response = new ResponseMessage();
            Response.Message = "Please input a correct value.";
            Response.Successful = false;
            var name = context.Request.QueryString["name"] ?? JsonConvert.SerializeObject(Response);

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
                    Response.Message = $"Successfully launched application. {appName}";
                    Response.Successful = true;
                }
            }
            context.Response.SendResponse(JsonConvert.SerializeObject(Response));
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/stop")]
        public IHttpContext StopProcess(IHttpContext context)
        {
            ResponseMessage Response = new ResponseMessage();
            Response.Message = "Please input a correct value.";
            Response.Successful = false;
            var name = context.Request.QueryString["name"] ?? JsonConvert.SerializeObject(Response);

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
                                Response.Message = $"Successfully stopped process. {p.ProcessName}";
                                Response.Successful = true;
                            }
                            catch (Exception e) 
                            {
                                Response.Message = $"Failed to stop process. {p.ProcessName} " + e.Message.ToString();
                                Response.Successful = false;
                            }
                        } 
                    }
                }
            }
 
            context.Response.SendResponse(JsonConvert.SerializeObject(Response));
            return context;
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

            var strTitle = string.Empty;
            var handle = GetForegroundWindow();
            // Obtain the length of the text   
            var intLength = GetWindowTextLength(handle) + 1;
            var stringBuilder = new StringBuilder(intLength);
            if (GetWindowText(handle, stringBuilder, intLength) > 0)
            {
                activeWindow.WindowName = stringBuilder.ToString();
            }
            else
            {
                activeWindow.WindowName = null;
            }

            foreach (Process p in process)
            {
                if (p.MainWindowTitle.Contains(activeWindow.WindowName))
                {
                    activeWindow.ProcessName = p.ProcessName;
                    activeWindow.StartTime = p.StartTime.ToString();
                    activeWindow.Response = new ResponseMessage
                    {
                        Message = "Successfully found window.",
                        Successful = true
                    };
                    break;
                }
                else
                {
                    activeWindow.ProcessName = null;
                    activeWindow.StartTime = null;
                    activeWindow.Response = new ResponseMessage
                    {
                        Message = "Failed to find window.",
                        Successful = false
                    };
                }
            }

            context.Response.SendResponse(JsonConvert.SerializeObject(activeWindow));
            return context;
        }
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/sendmessage")]
        public IHttpContext SendMessage(IHttpContext context)
        {
            ResponseMessage Response = new ResponseMessage();
            Response.Message = "Failed to parse message.";
            Response.Successful = false;
            var message = context.Request.QueryString["message"] ?? JsonConvert.SerializeObject(Response);

            if (message != null || message.Length > 0)
            {
                Response.Message = message;
                Response.Successful = true;
                MessageBox.Show(message, "WindowsRestAPI", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            else
            {
                Response.Message = "Failed to parse message.";
                Response.Successful = false;
            }

            context.Response.SendResponse(JsonConvert.SerializeObject(Response));
            return context;
        }
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/getprocesses")]
        public IHttpContext GetProcesses(IHttpContext context)
        {
            Processes Processes = new Processes();
            Process[] process = Process.GetProcesses();
            foreach (Process p in process)
            {
                try
                {
                    Processes.Process.Add(
                    new ProcessInfo
                    {
                        ProcessName = p.ProcessName,
                        StartTime = p.StartTime.ToString()
                    });
                }
                catch (Exception)
                {
                    // where priveleges arent sufficient.
                }
            }
            if(Processes.Process.Count > 0)
            {
                Processes.Response = new ResponseMessage
                {
                    Message = "Successfully got processes.",
                    Successful = true
                };
            }
            else
            {
                Processes.Response = new ResponseMessage
                {
                    Message = "Failed to get processes.",
                    Successful = false
                };
            }
            context.Response.SendResponse(JsonConvert.SerializeObject(Processes));
            return context;
        }
        [RestRoute]
        public IHttpContext Main(IHttpContext context)
        {
            ResponseMessage Response = new ResponseMessage();
            Response.Message = "Welcome to Windows Rest API.";
            Response.Successful = true;
            context.Response.SendResponse(JsonConvert.SerializeObject(Response));
            return context;
        }
    }
}
