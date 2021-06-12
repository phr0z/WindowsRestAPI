using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsRestAPI
{
    public class ResponseMessage
    {
        public string Message { get; set; }
        public bool Successful { get; set; }
    }
    public class ActiveWindow
    {
        public string WindowTitle { get; set; }
        public string ProcessName { get; set; }
        public string StartTime { get; set; }
        public ResponseMessage Response { get; set; }
    }
    public class SoundDevices
    {
        public List<SoundDeviceInfo> SoundDevice = new List<SoundDeviceInfo>();
        public ResponseMessage Response { get; set; }
    }
    public class SoundDeviceInfo
    {
        public string ID { get; set; }
        public string FriendlyName { get; set; }
        public string State { get; set; }
    }
    public class Processes
    {
        public List<ProcessInfo> Process = new List<ProcessInfo>();
        public ResponseMessage Response { get; set; }
    }
    public class ProcessInfo
    {
        public string ProcessName { get; set; }
        public string StartTime { get; set; }
    }
}
