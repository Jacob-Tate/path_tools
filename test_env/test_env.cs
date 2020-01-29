using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_env
{
    class test_env
    {
        static void Main(string[] args)
        {
            //
            // Check arguments
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: test_env.exe variable value[;value2]");
                Console.WriteLine("Supports multiple test values ie: C:\\test;C:\\test_2");
                Console.WriteLine("Returns 0 on found, -1 on not found");
                Environment.Exit(-1);
            }

            var variable = args[0];

            //
            // Get the variables
            var systemEnv = Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.Machine);
            var userEnv = Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.User);

            var sanitizedSystemEnv = new List<string>();
            var sanitizedUserEnv = new List<string>();

            if (systemEnv != null)
            {
                sanitizedSystemEnv = systemEnv.Split(';').Select(path =>
                {
                    path = path.Replace("/", "\\");
                    path = path.TrimEnd('\\');
                    return path;
                }).ToList();
            }

            if (userEnv != null)
            {
                sanitizedUserEnv = userEnv.Split(';').Select(path =>
                {
                    path = path.Replace("/", "\\");
                    path = path.TrimEnd('\\');
                    return path;
                }).ToList();
            }

            var sanitizedArgs = args[1].Split(';').Select(path =>
            {
                path = path.Replace("/", "\\");
                path = path.TrimEnd('\\');
                return path;
            }).ToList();

            // Test the values
            var result = sanitizedArgs.All(testPath =>
            {
                if (sanitizedSystemEnv.Contains(testPath))
                {
                    Console.WriteLine(testPath + " found in System " + variable);
                    return true;
                }

                if (sanitizedUserEnv.Contains(testPath))
                {
                    Console.WriteLine(testPath + " found in User " + variable);
                    return true;
                }

                Console.WriteLine(testPath + " not found");
                return false;
            });

            if (result)
            {
                Environment.Exit(0);
            }

            Environment.Exit(-1);
        }
    }
}
