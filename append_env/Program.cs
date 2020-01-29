using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace append_env
{
    class Program
    {
        static void Main(string[] args)
        {
            //
            // Check arguments
            if (args.Length <= 2)
            {
                Console.WriteLine("Usage: append_env.exe [user/system] variable value[;value2]");
                Console.WriteLine("Supports multiple values ie: C:\\test;C:\\test_2");
                Console.WriteLine("Returns 0 on success, -1 on failure, -2 on critical failure");
                Environment.Exit(-2);
            }

            //
            // Get the variables
            var type = args[0];
            var variable = args[1];
            var values = args[2];

            //
            // Get environment variables
            var systemEnv = Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.Machine);
            var userEnv = Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.User);

            var sanitizedSystemEnv = new List<string>();
            var sanitizedUserEnv = new List<string>();

            if (systemEnv != null)
            {
                sanitizedSystemEnv = systemEnv.Split(';').Select(path =>
                {
                    path = path.Replace("\\", "/");
                    path = path.TrimEnd('/');
                    return path;
                }).ToList();
            }

            if (userEnv != null)
            {
                sanitizedUserEnv = userEnv.Split(';').Select(path =>
                {
                    path = path.Replace("\\", "/");
                    path = path.TrimEnd('/');
                    return path;
                }).ToList();
            }

            var sanitizedArgs = values.Split(';').Select(path =>
            {
                path = path.Replace("\\", "/");
                path = path.TrimEnd('/');
                return path;
            }).ToList();

            var sanitizedType = type.ToLower();

            //
            // Prepend to path
            if (sanitizedType == "machine" || sanitizedType == "system")
            {
                foreach (var sarg in sanitizedArgs)
                {
                    Console.WriteLine("Prepending " + sarg + " to system path");
                    sanitizedSystemEnv.Add(sarg);
                }

                var resultSystemEnv = string.Join(";", sanitizedSystemEnv);
                Environment.SetEnvironmentVariable(variable, resultSystemEnv, EnvironmentVariableTarget.Machine);
            }
            else if (sanitizedType == "user")
            {
                foreach (var sarg in sanitizedArgs)
                {
                    Console.WriteLine("Prepending " + sarg + " to user path");
                    sanitizedUserEnv.Add(sarg);
                }

                var resultUserEnv = string.Join(";", sanitizedUserEnv);
                Environment.SetEnvironmentVariable(variable, resultUserEnv, EnvironmentVariableTarget.User);
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
