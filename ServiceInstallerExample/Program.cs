using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Options;
using ServiceTools;

// author @_wirepair : github.com/wirepair
// date: 03172013 
// copyright: ME AND MINE but i guess you can use it :D.

namespace ServiceInstallerExample
{
    class Program
    {
        static void ShowHelp(OptionSet p, string error)
        {
            if (error != null)
            {
                Console.WriteLine(error);
                Console.WriteLine();
            }
            Console.WriteLine("Usage: ServiceInstallerExample [OPTIONS]");
            Console.WriteLine("Demos how to use this library.");
            Console.WriteLine("Possible commands:");
            Console.WriteLine();
            p.WriteOptionDescriptions(Console.Out);
        }

        static void InstallTheService(string install_string, string serviceName)
        {
            if (ServiceTools.ServiceInstaller.ServiceIsInstalled(serviceName))
            {
                Console.WriteLine("Service is already installed!");
                return;
            }
            try
            {
                ServiceTools.ServiceInstaller.Install(serviceName, serviceName, install_string);
                Console.WriteLine("Service Installed as: ");
                Console.WriteLine(serviceName);

            }
            catch (System.ApplicationException e)
            {
                Console.WriteLine("Error installing service (do you have proper privileges?): {0}", e.Message);
            }
        }
        static void Main(string[] args)
        {
            
            bool show_help = false;
            string filename = null;
            string install_string = null;
            string service_name = null;
            var p = new OptionSet() 
            {
                { "i|install=", 
                   "How service should be installed.",
                    v => install_string = v },
                { "f|filename=", 
                   "Service executable file to be used.",
                    v => install_string = v },
                { "s|servicename=",
                    "Name of the service to be installed as.",
                    v => service_name = v },
                { "h|?|help",  "show this message and exit", 
                   v => show_help = v != null },
            };

            try
            {
                p.Parse(args);
                if (show_help)
                {
                    ShowHelp(p, null);
                    return;
                }
                if (service_name == null || filename == null)
                {
                    ShowHelp(p, "Must have service and filename.");
                    return;
                }

                if (install_string != null)
                {
                    InstallTheService(install_string, service_name);
                }
                else
                {
                    InstallTheService(string.Format("C:\\tools\\pin\\ia32\\bin\\pin.exe -t C:\\tools\\pin\\ia32\\bin\\IDAPinLogger.dll -o C:\\tools\\pin\\ia32\\bin\\pinlog.log -- {0}", filename), service_name);
                }
                ServiceInstaller.StartService(service_name);
                ServiceInstaller.StopService(service_name);
                ServiceInstaller.Uninstall(service_name);
            }
            catch (Mono.Options.OptionException oe)
            {
                ShowHelp(p, oe.Message);
                return;
            }

        }
    }
}
