using System.Collections.Generic;
using System.Diagnostics;
using System.IO;


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
        public List<string> GetCommands() 
        {
            List<string> config = new List<string>();
            string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            StreamReader reader = new StreamReader(folder + "\\commands.config");
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
            string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            StreamWriter writer = new StreamWriter(folder + "\\commands.config");
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
