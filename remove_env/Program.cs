using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace remove_env
{
    class Program
    {
        static void Main(string[] args)
        {
            //
            // Check arguments
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: remove_env.exe variable value[;value2]");
                Console.WriteLine("Supports multiple test values ie: C:\\test;C:\\test_2");
                Console.WriteLine("Returns 0 on success, -1 on failure, -2 on critical failure");
                Environment.Exit(-2);
            }

            //
            // Get the variables
            var variable = args[0];
            var systemPath = Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.Machine);
            var userPath = Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.User);

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

            //
            // remove from path
            int removed = 0;

            foreach (var sarg in sanitizedArgs)
            {
                Console.WriteLine("Removing " + sarg + " from system path");
                removed += sanitizedSystemPath.RemoveAll(e => e == sarg);
            }

            if (removed != 0)
            {
                var resultSystemPath = string.Join(";", sanitizedSystemPath);
                Environment.SetEnvironmentVariable(variable, resultSystemPath, EnvironmentVariableTarget.Machine);
            }

            removed = 0;

            foreach (var sarg in sanitizedArgs)
            {
                Console.WriteLine("Prepending " + sarg + " to user path");
                removed += sanitizedSystemPath.RemoveAll(e => e == sarg);
            }

            if (removed != 0)
            {
                var resultUserPath = string.Join(";", sanitizedUserPath);
                Environment.SetEnvironmentVariable(variable, resultUserPath, EnvironmentVariableTarget.User);
            }

            Environment.Exit(0);
        }
    }
}
