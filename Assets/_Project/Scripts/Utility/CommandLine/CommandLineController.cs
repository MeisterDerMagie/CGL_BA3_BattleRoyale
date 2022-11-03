//(c) copyright by Martin M. Klöckener
using System.Collections.Generic;
using UnityEngine;

namespace Wichtel {
//https://4experience.co/how-to-start-unity-application-from-command-prompt-with-initial-arguments/
public static class CommandLineController
{
    //command line arguments need to be in this syntax:
    //arguments without value: bla.exe startInWindowMode
    //arguments with value: bla.exe isWindowed=true
    //multiple arguments are simply written one after the other, without separator (separator is white space): bla.exe isWindowed=true width=1280 height=720

    private static Dictionary<string, string> commandLineArguments;
    
    public static Dictionary<string, string> CommandLineArguments
    {
        get
        {
            //lazy initialization
            if(!isInitialized)
                Initialize();

            return commandLineArguments;
        }
    }

    private static bool isInitialized = false;


    private static void Initialize()
    {
        GetCommandLineArguments();
        isInitialized = true;
    }

    private static void GetCommandLineArguments()
    {
        commandLineArguments = new Dictionary<string, string>();
        
        string[] args = System.Environment.GetCommandLineArgs();

        foreach (string s in args)
        {
            string[] split = s.Split("=", 2);

            //if an argument without value was found
            if (split.Length == 1)
            {
                if(!commandLineArguments.ContainsKey(split[0]))
                    commandLineArguments.Add(split[0], string.Empty);
            }
            
            //if an argument with value was found
            else if (split.Length == 2)
            {
                if(!commandLineArguments.ContainsKey(split[0]))
                    commandLineArguments.Add(split[0], split[1]);
            }
        }


        Debug.Log("Command line arguments:");
        foreach (KeyValuePair<string,string> keyValuePair in commandLineArguments)
        {
            Debug.Log($"argument: \"{keyValuePair.Key}\": \"{keyValuePair.Value}\"");
        }
    }
}
}