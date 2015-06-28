using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;

namespace Anna.TvShows.Service
{
    class InstallService
    {
        private readonly string _serviceName;

        public InstallService(string serviceName)
        {
            if (serviceName == null)
                throw new ArgumentNullException("serviceName");

            _serviceName = serviceName;
        }

        public void Install()
        {
            var services = ServiceController.GetServices();

            var service = services.FirstOrDefault(s => s.ServiceName == _serviceName);

            if (service != null)
            {
                Console.WriteLine("Service is already installed.");

                return;
            }

            string installServiceCommand = string.Format("create \"{0}\" binPath= {1} start= auto", _serviceName, System.Reflection.Assembly.GetEntryAssembly().Location);

            ExecuteServiceCommand(installServiceCommand);
        }

        private void ExecuteServiceCommand(string command)
        {
            var p = new Process
            {
                StartInfo =
                {
                    Arguments = command,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    FileName = "sc.exe"
                }
            };

            Console.WriteLine("Executing: " + p.StartInfo.FileName + " " + p.StartInfo.Arguments);
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            Console.WriteLine(output);

            if (p.ExitCode != 0)
            {
                Console.WriteLine("Failed to install service");

                Environment.Exit(p.ExitCode);
            }

            p.Close();
        }
    }
}