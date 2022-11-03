using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wichtel {
//https://4experience.co/how-to-start-unity-application-from-command-prompt-with-initial-arguments/
public class CommandLineExample : MonoBehaviour
{
    private const string WindowModeArg = "isWindowedMode";
    private const string ResolutionWidthArg = "width";
    private const string ResolutionHeightArg = "height";

    [SerializeField]
    private int defaultWidth = 1920;

    [SerializeField]
    private int defaultHeight = 1080;

    void Start() => ParseCommandLineArguments();

    private void ParseCommandLineArguments()
    {
        Dictionary<string, string> args = CommandLineController.CommandLineArguments;
        
        int screenWidth = defaultWidth;
        int screenHeight = defaultHeight;
        bool isWindowsMode = false;
        
        //parse IsWindowedMode value
        if (args.ContainsKey(WindowModeArg))
            bool.TryParse(args[WindowModeArg], out isWindowsMode);
        
        //parse width value
        if(args.ContainsKey(ResolutionWidthArg))
            int.TryParse(args[ResolutionWidthArg], out screenWidth);
        
        //parse height value
        if(args.ContainsKey(ResolutionHeightArg))
            int.TryParse(args[ResolutionHeightArg], out screenHeight);

        //set resolution according to command line arguments
        Screen.SetResolution(screenWidth, screenHeight, isWindowsMode == false);
    }
}
}