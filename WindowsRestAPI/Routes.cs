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
using Newtonsoft.Json;
using WindowsRestAPI.Properties;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Linq;
using AudioSwitcher.AudioApi.CoreAudio;
using NAudio.CoreAudioApi;
using System.Net;
using System.Reflection;

namespace WindowsRestAPI
{
    [RestResource]
    public class APIResource
    {       
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/display")]
        public IHttpContext SetMonitor(IHttpContext context)
        {
            if (Authentication.Enabled)
            {
                HttpListenerBasicIdentity identity = (HttpListenerBasicIdentity)context.User.Identity;
                if (!Authentication.VerifyAuthentication(identity)) { context.Response.SendResponse(JsonConvert.SerializeObject(Authentication.FailedAuthentication)); return context; }
            }

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
            context.Response.ContentEncoding = Encoding.Default;
            context.Response.SendResponse(JsonConvert.SerializeObject(Response));
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/getid")]
        public IHttpContext GetSound(IHttpContext context)
        {
            if (Authentication.Enabled)
            {
                HttpListenerBasicIdentity identity = (HttpListenerBasicIdentity)context.User.Identity;
                if (!Authentication.VerifyAuthentication(identity)) { context.Response.SendResponse(JsonConvert.SerializeObject(Authentication.FailedAuthentication)); return context; }
            }

            SoundDevices sd = new SoundDevices();

            CoreAudioController controller = new CoreAudioController();
            var devices = controller.GetPlaybackDevices();

            foreach (CoreAudioDevice device in devices)
            {
                try
                {
                    sd.SoundDevice.Add(new SoundDeviceInfo
                    {
                        ID = device.RealId,
                        FriendlyName = device.FullName,
                        State = device.State.ToString()
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
                    controller.Dispose();
                }
            }

            controller.Dispose();
            context.Response.ContentEncoding = Encoding.Default;
            context.Response.SendResponse(JsonConvert.SerializeObject(sd));
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/set")]
        public IHttpContext SetSound(IHttpContext context)
        {
            if (Authentication.Enabled)
            {
                HttpListenerBasicIdentity identity = (HttpListenerBasicIdentity)context.User.Identity;
                if (!Authentication.VerifyAuthentication(identity)) { context.Response.SendResponse(JsonConvert.SerializeObject(Authentication.FailedAuthentication)); return context; }
            }

            ResponseMessage Response = new ResponseMessage();
            Response.Message = "Please input correct id or name. Get id by using /getid";
            Response.Successful = false;

            var enc = Encoding.GetEncoding(context.Request.ContentEncoding.CodePage);
            var id = Encoding.UTF8.GetString(enc.GetBytes(context.Request.QueryString["id"])) ?? JsonConvert.SerializeObject(Response);
            
            CoreAudioController controller = new CoreAudioController();
            var devices = controller.GetPlaybackDevices();

            foreach(CoreAudioDevice device in devices)
            {
                if (device.RealId == id)
                {
                    controller.SetDefaultDevice(device);
                    Response.Message = $"Switched to audio device: {device.FullName}";
                    Response.Successful = true;
                }
                if(device.FullName == id && device.State == AudioSwitcher.AudioApi.DeviceState.Active)
                {
                    try
                    {
                        controller.SetDefaultDevice(device);
                        Response.Message = $"Switched to audio device: {device.FullName}";
                        Response.Successful = true;
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            controller.Dispose();
            context.Response.ContentEncoding = Encoding.Default;
            context.Response.SendResponse(JsonConvert.SerializeObject(Response));
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/volume")]
        public IHttpContext SetVolume(IHttpContext context)
        {
            if (Authentication.Enabled)
            {
                HttpListenerBasicIdentity identity = (HttpListenerBasicIdentity)context.User.Identity;
                if (!Authentication.VerifyAuthentication(identity)) { context.Response.SendResponse(JsonConvert.SerializeObject(Authentication.FailedAuthentication)); return context; }
            }

            ResponseMessage Response = new ResponseMessage();
            Response.Message = "Please input a correct numeric value.";
            Response.Successful = false;
            var volume = context.Request.QueryString["level"] ?? JsonConvert.SerializeObject(Response);

            CoreAudioController controller = new CoreAudioController();
            CoreAudioDevice device = controller.DefaultPlaybackDevice;
            try
            {
                device.Volume = Convert.ToDouble(volume);
                Response.Message = $"Successfully changed volume level to {volume}.";
                Response.Successful = true;
            }
            catch (Exception e)
            {

            }
            context.Response.ContentEncoding = Encoding.Default;
            context.Response.SendResponse(JsonConvert.SerializeObject(Response));
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/run")]
        public IHttpContext RunCommand(IHttpContext context)
        {
            if (Authentication.Enabled)
            {
                HttpListenerBasicIdentity identity = (HttpListenerBasicIdentity)context.User.Identity;
                if (!Authentication.VerifyAuthentication(identity)) { context.Response.SendResponse(JsonConvert.SerializeObject(Authentication.FailedAuthentication)); return context; }
            }

            ResponseMessage Response = new ResponseMessage();
            Response.Message = "Please input a correct value.";
            Response.Successful = false;
            var name = context.Request.QueryString["name"] ?? JsonConvert.SerializeObject(Response);

            Commands command = new Commands();

            bool ApplicationIsRunning = false;

            foreach (string s in command.GetCommands())
            {
                var decoded = command.Decoded(s.Trim());

                string appName = decoded.Name.ToLower();
                string appPath = decoded.Path;
                string appArguments = decoded.Arguments;
                string executableFilename = Path.GetFileName(appPath).Replace(".exe", "");

                if (name.ToLower() == appName)
                {
                    Process process = new Process();
                    Process[] running = Process.GetProcesses();

                    foreach (Process p in running)
                    {
                        if (p.ProcessName.Contains(executableFilename))
                        {
                            try
                            {
                                ApplicationIsRunning = true; 
                                Response.Message = $"Application is already running. {p.ProcessName}";
                                Response.Successful = true;
                            }
                            catch (Exception e)
                            {
                                Response.Message = $"Failed to start process. {p.ProcessName} " + e.Message.ToString();
                                Response.Successful = false;
                            }
                        }
                    }

                    if (!ApplicationIsRunning)
                    {
                        try
                        {
                            process.StartInfo.FileName = appPath;
                            if (appArguments?.Length > 1)
                            {
                                process.StartInfo.Arguments = appArguments;
                            }
                            process.Start();
                            Response.Message = $"Successfully launched application. {appName}";
                            Response.Successful = true;
                        }
                        catch (Exception e)
                        {
                            Response.Message = $"Failed to start process. {decoded.Name} " + e.Message.ToString();
                            Response.Successful = false;
                        }
                    }
                }
            }
            context.Response.ContentEncoding = Encoding.Default;
            context.Response.SendResponse(JsonConvert.SerializeObject(Response));
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/stop")]
        public IHttpContext StopProcess(IHttpContext context)
        {
            if (Authentication.Enabled)
            {
                HttpListenerBasicIdentity identity = (HttpListenerBasicIdentity)context.User.Identity;
                if (!Authentication.VerifyAuthentication(identity)) { context.Response.SendResponse(JsonConvert.SerializeObject(Authentication.FailedAuthentication)); return context; }
            }

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
            context.Response.ContentEncoding = Encoding.Default;
            context.Response.SendResponse(JsonConvert.SerializeObject(Response));
            return context;
        }
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/activewindow")]
        public IHttpContext ActiveWindow(IHttpContext context)
        {
            if (Authentication.Enabled)
            {
                HttpListenerBasicIdentity identity = (HttpListenerBasicIdentity)context.User.Identity;
                if (!Authentication.VerifyAuthentication(identity)) { context.Response.SendResponse(JsonConvert.SerializeObject(Authentication.FailedAuthentication)); return context; }
            }

            Process[] process = Process.GetProcesses();

            ActiveWindow activeWindow = new ActiveWindow();

            var strTitle = string.Empty;
            var handle = GetForegroundWindow();
            // Obtain the length of the text   
            var intLength = GetWindowTextLength(handle) + 1;
            var stringBuilder = new StringBuilder(intLength);
            if (GetWindowText(handle, stringBuilder, intLength) > 0)
            {
                activeWindow.WindowTitle = stringBuilder.ToString();
            }
            else
            {
                activeWindow.WindowTitle = null;
            }

            foreach (Process p in process)
            {
                if (p.MainWindowTitle.Contains(activeWindow.WindowTitle))
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

            context.Response.ContentEncoding = Encoding.Default;
            context.Response.SendResponse(JsonConvert.SerializeObject(activeWindow));
            return context;
        }
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/sendmessage")]
        public IHttpContext SendMessage(IHttpContext context)
        {
            if (Authentication.Enabled)
            {
                HttpListenerBasicIdentity identity = (HttpListenerBasicIdentity)context.User.Identity;
                if (!Authentication.VerifyAuthentication(identity)) { context.Response.SendResponse(JsonConvert.SerializeObject(Authentication.FailedAuthentication)); return context; }
            }

            ResponseMessage Response = new ResponseMessage();
            Response.Message = "Failed to parse message.";
            Response.Successful = false;
            var message = context.Request.QueryString["message"] ?? JsonConvert.SerializeObject(Response);


            if (message != null || message.Length > 0)
            {
                Response.Message = message;
                Response.Successful = true;

                Messages.Show(message);
            }
            else
            {
                Response.Message = "Failed to parse message.";
                Response.Successful = false;
            }
            context.Response.ContentEncoding = Encoding.Default;
            context.Response.SendResponse(JsonConvert.SerializeObject(Response));
            return context;
        }
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/getprocesses")]
        public IHttpContext GetProcesses(IHttpContext context)
        {
            if (Authentication.Enabled)
            {
                HttpListenerBasicIdentity identity = (HttpListenerBasicIdentity)context.User.Identity;
                if (!Authentication.VerifyAuthentication(identity)) { context.Response.SendResponse(JsonConvert.SerializeObject(Authentication.FailedAuthentication)); return context; }
            }

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
            context.Response.ContentEncoding = Encoding.Default;
            context.Response.SendResponse(JsonConvert.SerializeObject(Processes));
            return context;
        }
        [RestRoute]
        public IHttpContext Main(IHttpContext context)
        {
            if (Authentication.Enabled)
            {
                HttpListenerBasicIdentity identity = (HttpListenerBasicIdentity)context.User.Identity;
                if (!Authentication.VerifyAuthentication(identity)) { context.Response.SendResponse(JsonConvert.SerializeObject(Authentication.FailedAuthentication)); return context; }
            }

            ResponseMessage response = new ResponseMessage();
            response.Message = "Welcome to Windows Rest API.";
            response.Successful = true;

            var _Response = new 
            {
                Reponse = response,
                HostBindIP = Settings.Default.HostIP, 
                Port = Settings.Default.Port, 
                HTTPS = Settings.Default.HTTPS, 
                StartServer = Settings.Default.StartServer, 
                StartMinimized = Settings.Default.StartMinimized, 
                Authentication = Settings.Default.Authentication, 
                Username = Settings.Default.Username,
                DetectedEncoding = Encoding.Default.EncodingName,
                Version = Assembly.GetExecutingAssembly().GetName().Version.ToString()
            };

            context.Response.ContentEncoding = Encoding.Default;
            context.Response.SendResponse(JsonConvert.SerializeObject(_Response));
            return context;
        }
    }
}
