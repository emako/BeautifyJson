using Fclp;
using System;
using System.IO;

namespace BeautifyJson;

internal class Program
{
    private static void Main(string[] args)
    {
        FluentCommandLineParser p = new();

        p.Setup<string>('f')
           .Callback(files =>
           {
               foreach (string file in files.Split('|'))
               {
                   BeautifyJsonFile(file);
               }
               Environment.Exit(0);
           })
        .Required();

        p.Parse(args);

        foreach (string file in args)
        {
            BeautifyJsonFile(file);
        }
        Environment.Exit(0);
    }

    private static void BeautifyJsonFile(string file)
    {
        Console.WriteLine(file);

        if (File.Exists(file))
        {
            string jsonString = File.ReadAllText(file);
            string beautifiedJsonString = JsonBeautifier.Beautify(jsonString);

            File.WriteAllText(file, beautifiedJsonString);
        }
    }
}
