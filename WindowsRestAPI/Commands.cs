using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace WindowsRestAPI
{
    class DecodedRunString
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Arguments { get; set; }
    }
    class Commands
    {
        public static void CopyCommands()
        {
            //https://techcommunity.microsoft.com/t5/windows-dev-appconsult/msix-how-to-copy-data-outside-the-installation-folder-during-the/ba-p/1133602
            string result = Assembly.GetExecutingAssembly().Location;
            int index = result.LastIndexOf("\\");
            string commands = $"{result.Substring(0, index)}\\commands.config";

            string destinationPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\WindowsRestAPI\\commands.config";
            string destinationFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\WindowsRestAPI\\";

            if (!File.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationFolder);
                File.Copy(commands, destinationPath, true);
            }
        }
        public List<string> GetCommands() 
        {
            string commands = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\WindowsRestAPI\\commands.config";
            List<string> config = new List<string>();
            string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            StreamReader reader = new StreamReader(commands);
            string[] c = reader.ReadToEnd().Trim().Split('\n');
            foreach(string s in c)
            {
                config.Add(s);
            }
            reader.Close();
            reader.Dispose();
            if(config.Count == 0)
            {
                return null;
            }
            return config;
        }
        public void SaveCommands(List<string> commands)
        {
            string cmds = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\WindowsRestAPI\\commands.config";
            StreamWriter writer = new StreamWriter(cmds);
            foreach (string s in commands)
            {
                writer.WriteLine(s.Trim());
            }           
            writer.Close();
            writer.Dispose();
        }
        public void AddCommand(string name, string path, string arguments)
        {
            string command = "";
            if (arguments == "")
            {
                command = "Executable_" + name + "=" + path;
            }
            else
            {
                command = "Executable_" + name + "=" + path + "," + arguments;
            }
            List<string> commands = new List<string>();
            commands = GetCommands();
            commands.Add(command.Trim());
            SaveCommands(commands);
        }
        public void RemoveCommand(string command)
        {
            List<string> commands = GetCommands();
            List<string> updatedCommands = new List<string>();

            commands.TrimExcess();
            foreach (string s in commands)
            {
                DecodedRunString decoded = Decoded(s);
                if (decoded.Name.ToLower() == command.ToLower())
                {
                    continue;
                }
                else
                {
                    updatedCommands.Add(s.Trim());
                }
            }
            SaveCommands(updatedCommands);
        }
        public DecodedRunString Decoded(string command)
        {
            if(command == "")
            {
                return null;
            }
            DecodedRunString decoded = new DecodedRunString();
            decoded.Name = command.Replace("Executable_", "").Split('=')[0].Trim();
            string[] path = command.Split('=');
            decoded.Path = path[1];
            if (command.Contains(","))
            {
                decoded.Path = path[1].Split(',')[0].Trim();
                decoded.Arguments = command.Split(',')[1].Trim();
            }
            
            return decoded;
        }
    }
}
