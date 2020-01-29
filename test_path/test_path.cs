using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_path
{
    class test_path
    {
        static void Main(string[] args)
        {
            //
            // Check arguments
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: test_path.exe test_value");
                Console.WriteLine("Supports multiple test values ie: C:\\test;C:\\test_2");
                Console.WriteLine("Returns 0 on found, -1 on not found");
                Environment.Exit(-1);
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
                path = path.Replace("/", "\\");
                path = path.TrimEnd('\\');
                return path;
            }).ToList();

            var sanitizedUserPath = userPath.Split(';').Select(path =>
            {
                path = path.Replace("/", "\\");
                path = path.TrimEnd('\\');
                return path;
            }).ToList();

            var sanitizedArgs = args[0].Split(';').Select(path =>
            {
                path = path.Replace("/", "\\");
                path = path.TrimEnd('\\');
                return path;
            }).ToList();

            // Test the values
            var result = sanitizedArgs.All(testPath =>
            {
                if (sanitizedSystemPath.Contains(testPath))
                {
                    Console.WriteLine(testPath + " found in System Path");
                    return true;
                }

                if (sanitizedUserPath.Contains(testPath))
                {
                    Console.WriteLine(testPath + " found in User Path");
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
