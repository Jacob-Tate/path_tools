using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace append_path
{
    class Program
    {
        static void Main(string[] args)
        {
            //
            // Check arguments
            if (args.Length <= 1)
            {
                Console.WriteLine("Usage: append_path.exe [user/system] value[;value2]");
                Console.WriteLine("Supports multiple test values ie: C:\\test;C:\\test_2");
                Console.WriteLine("Returns 0 on success, -1 on failure, -2 on critical failure");
                Environment.Exit(-2);
            }

            //
            // Get the variables
            var systemPath = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine);
            var userPath = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User);

            if (systemPath == null)
            {
                Console.WriteLine("SystemPath was null aborting");
                Environment.Exit(-2);
            }

            if (userPath == null)
            {
                Console.WriteLine("UserPath was null aborting");
                Environment.Exit(-2);
            }

            //
            // Sanitize the inputs
            var sanitizedSystemPath = systemPath.Split(';').Select(path =>
            {
                path = path.Replace("\\", "/");
                path = path.TrimEnd('/');
                return path;
            }).ToList();

            var sanitizedUserPath = userPath.Split(';').Select(path =>
            {
                path = path.Replace("\\", "/");
                path = path.TrimEnd('/');
                return path;
            }).ToList();

            var sanitizedArgs = args[1].Split(';').Select(path =>
            {
                path = path.Replace("\\", "/");
                path = path.TrimEnd('/');
                return path;
            }).ToList();

            var sanitizedType = args[0].ToLower();

            //
            // Prepend to path
            if (sanitizedType == "machine" || sanitizedType == "system")
            {
                foreach (var sarg in sanitizedArgs)
                {
                    Console.WriteLine("Prepending " + sarg + " to system path");
                    sanitizedSystemPath.Add(sarg);
                }

                var resultSystemPath = string.Join(";", sanitizedSystemPath);
                Environment.SetEnvironmentVariable("Path", resultSystemPath, EnvironmentVariableTarget.Machine);
            }
            else if (sanitizedType == "user")
            {
                foreach (var sarg in sanitizedArgs)
                {
                    Console.WriteLine("Prepending " + sarg + " to user path");
                    sanitizedSystemPath.Add(sarg);
                }

                var resultUserPath = string.Join(";", sanitizedUserPath);
                Environment.SetEnvironmentVariable("Path", resultUserPath, EnvironmentVariableTarget.User);
            }
            else
            {
                Console.WriteLine(sanitizedType + " is not a valid path type");
                Environment.Exit(-2);
            }

            Environment.Exit(0);
        }
    }
}
