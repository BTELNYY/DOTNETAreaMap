using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DOTNETAreaMap
{
    public class Program
    {
        public static Assembly assembly;

        public static StreamWriter writer;

        public static string filter;

        static void Main(string[] args)
        {
            List<string> argsList = new List<string>(args);
            if(argsList.Count < 6)
            {
                PrintHelp();
                return;
            }
            string[] input = GetKeyPair("-i", argsList);
            if(input == null)
            {
                PrintHelp();
                return;
            }
            string[] filter = GetKeyPair("-a", argsList);
            if(filter == null)
            {
                PrintHelp();
                return;
            }
            else
            {
                Program.filter = filter[1];
            }
            try
            {
                foreach(string file in Directory.EnumerateFiles(input[1]))
                {
                    try
                    {
                        if (file.Contains(filter[1]))
                        {
                            continue;
                        }
                        Console.WriteLine("Load extra assembly: " + file);
                        Assembly.LoadFrom(file);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"Can't load ${file}: {ex.Message}");
                    }
                }
                assembly = Assembly.LoadFrom(Path.Combine(input[1], filter[1]));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }
            string[] output = GetKeyPair("-o", argsList);
            if(output == null)
            {
                PrintHelp();
                return;
            }
            writer = new StreamWriter(new FileStream(output[1], FileMode.OpenOrCreate));
            StageOne();
            StageTwo();
            writer.Flush();
        }


        public static List<Type> ToProcess = new List<Type>();

        static void StageOne()
        {
            try
            {
                int total = assembly.GetTypes().Length;
                int actual = total;
                foreach (Type t in assembly.GetTypes())
                {
                    try
                    {
                        Console.WriteLine("Discovering Type: " + t.FullName);
                        ToProcess.Add(t);
                    }
                    catch
                    {
                        actual--;
                    }
                }
                ConsoleColor color = actual == total ? ConsoleColor.Green : ConsoleColor.Yellow;
                Utils.WriteLineColor($"Discovered {actual} types out of {total} types. There were/was {total - actual} error(s).", color);
            }
            catch(ReflectionTypeLoadException ex)
            {
                Utils.WriteLineColor(string.Join("\n", ex.LoaderExceptions.Select(x => x.ToString())), ConsoleColor.Red);
            }
        }

        public static List<IProcessor> processors = new List<IProcessor>();

        static void StageTwo()
        {
            writer.Write("classDiagram\n");
            ClassProcessor processor = new ClassProcessor();
            foreach (Type t in ToProcess.Where(x => x.Assembly.FullName == assembly.FullName && x.IsPublic))
            {
                processor.Process(t, writer);
            }
        }

        static string[] GetKeyPair(string key, List<string> args)
        {
            string[] keys = new string[2];
            int index = args.IndexOf(key);
            if(index == -1 || args.Count == index + 1)
            {
                return null;
            }
            keys[0] = args[index];
            keys[1] = args[index + 1];
            return keys;
        }

        static void PrintHelp()
        {
            string help = $@"Invalid syntax. Syntax: ./DOTNETAreaMap.exe -i (path) -o (path)
-i (path): folder path to DLLs. (Required)
-a (assembly name): filter results by assembly filename, external types will not be mapped. (Required)
-o (path): output path for UML diagram. (Required)
            ";
            Console.WriteLine(help);
        }
    }
}
