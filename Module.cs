using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MonoMod.RuntimeDetour;
using UnityEngine;
using Dungeonator;
using System.IO;
using System.Configuration;

namespace ConsoleToTextAuto
{
    public class CTTModule : ETGModule
    {
        public static readonly string MOD_NAME = "Console To Text Mod (Code By NotABot!)";
        public static readonly string VERSION = "1.1.1 (Automatic Version)";
        public static readonly string TEXT_COLOR = "#8e1cff";

        public static string defaultLog = Path.Combine(ETGMod.ResourcesDirectory, "ConsoleToTextAuto.txt");

        public override void Start()
        {
            //yes this code is ONLY for the auto-generator i built both mods using the same projhect that i modified so it wouldnt clash with each other lol
            if (File.Exists(defaultLog))
            {
                File.Delete(defaultLog);
                Log($"Deleting Old Auto C.T.T File from when previously used and creating new Auto C.T.T file.", TEXT_COLOR);
                File.Create(defaultLog);
            }
            else if (!File.Exists(defaultLog))
            {
                Log($"Creating new Auto C.T.T file.", TEXT_COLOR);
                File.Create(defaultLog);
            }

            //ETGModConsole.Commands.AddUnit("print_debug_console", this.PrintDebugLog);
            //ETGModConsole.Commands.AddUnit("nulls_only_print_debug_console", this.PrintDebugLogNullOnly);

            Application.logMessageReceived += Application_logMessageReceived;
           //   var HookToWriteLogToTxtFile = new Hook(
             //        typeof(GameManager).GetMethod("InvariantUpdate", BindingFlags.NonPublic | BindingFlags.Instance),
               //    typeof(DebugPrinter).GetMethod("LogHook", BindingFlags.Static | BindingFlags.Public));


            Log($"{MOD_NAME} v{VERSION} started successfully..", TEXT_COLOR);
            Log($"Auto C.T.T file should be located in.{defaultLog}", TEXT_COLOR);

        }

        //public static FileStream logfile = new FileStream(defaultLog, FileMode.Open, FileAccess.ReadWrite);
        private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
        {
            using (StreamWriter file2 = File.AppendText(defaultLog))
            {
                file2.WriteLine(condition);
                if (type == LogType.Exception)
                {
                    file2.WriteLine("   " + stackTrace);
                }
            }
        }

        public void PrintDebugLog(string[] args)
        {
            if (File.Exists(defaultLog))
            {
                File.Delete(defaultLog);
                Log($"Deleting Old C.T.T File from when previously used.", TEXT_COLOR);
            }
            File.Create(defaultLog);
            Log($"Creating New C.T.T file.", TEXT_COLOR);
            Log($"C.T.T file should be located in.{defaultLog}", TEXT_COLOR);
            DebugPrinter.ClearDebugLog();
        }
        public void PrintDebugLogNullOnly(string[] args)
        {
            if (File.Exists(defaultLog))
            {
                File.Delete(defaultLog);
                Log($"Deleting Old C.T.T File from when previously used.", TEXT_COLOR);
            }
            File.Create(defaultLog);
            Log($"Creating New C.T.T (Nulls Only) file.", TEXT_COLOR);
            Log($"C.T.T file should be located in.{defaultLog}", TEXT_COLOR);
            DebugPrinter.ClearDebugLogNullOnly();
        }

        public class DebugPrinter : ETGModDebugLogMenu
        {



            public static void LogHook(Application.LogCallback back)
            {
                //orig(man, time);
                List<LoggedText> listOfText = _AllLoggedText;
                foreach (LoggedText txt in listOfText)
                {
                    //ETGModConsole.Log("2");
                    


                    if (!text.Contains(txt))
                    {
                        text.Add(txt);

                        string log = txt.LogMessage;
                        string stacktrace = txt.Stacktace;

                        //ETGModConsole.Log("3");
                        using (StreamWriter file2 = File.AppendText(defaultLog))
                        {
                            file2.WriteLine(log.ToString());
                            if (txt.LogType == LogType.Exception)
                            {
                                file2.WriteLine("   " + stacktrace.ToString());
                            }
                        }
                    }
                    
                }
            }
            protected static List<LoggedText> text = new List<LoggedText>();

            public static void ClearDebugLog()
            {
                List<LoggedText> listOfText = _AllLoggedText;
                foreach (LoggedText txt in listOfText)
                {
                    string log = txt.LogMessage;
                    string stacktrace = txt.Stacktace;

                    using (StreamWriter file2 = File.AppendText(defaultLog))
                    {
                        file2.WriteLine(log.ToString());
                        if (txt.LogType == LogType.Exception)
                        {
                            file2.WriteLine("   " + stacktrace.ToString());
                        }
                        file2.WriteLine("\n");
                    }
                }
            }
            public static void ClearDebugLogNullOnly()
            {
                List<LoggedText> listOfText = _AllLoggedText;
                foreach (LoggedText txt in listOfText)
                {
                    if (txt.LogType == LogType.Exception)
                    {
                        using (StreamWriter file2 = File.AppendText(defaultLog))
                        {
                            string log = txt.LogMessage;
                            string stacktrace = txt.Stacktace;
                            file2.WriteLine(log.ToString());
                            if (txt.LogType == LogType.Exception)
                            {
                                file2.WriteLine("   " + stacktrace.ToString());
                            }
                            file2.WriteLine("\n");
                        }
                    }

                }
            }
        }


        public static void LogHook(Action<string, bool> orig, string text, bool debuglog = false)
        {

            try
            {
                using (StreamWriter file2 = File.AppendText(defaultLog))
                {
                    file2.WriteLine(text);
                }
            }
            catch
            {
                Log($"Something broke so bad that the F2 console can't print it. God pray for you.", TEXT_COLOR);

            }


            orig(text, debuglog);
        }


        public static void LogHookU(Action<string> orig, string str)
        {
            orig(str);
            try
            {
                using (StreamWriter file2 = File.AppendText(defaultLog))
                {
                    //Log($"waa", TEXT_COLOR);
                    file2.WriteLine(str);
                }
            }
            catch
            {
                Log($"Something broke so bad that the F3 console can't print it. God pray for you.", TEXT_COLOR);
            }
        }




        public static void Log(string text, string color = "#FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }

        public override void Exit() { }
        public override void Init() { }
    }
    
}
