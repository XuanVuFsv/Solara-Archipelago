using UnityEngine;

namespace VitsehLand.Scripts.Ultilities
{
    public static class MyDebug
    {
        static bool canLog = false;

        public static void Log(object message, Object context)
        {
            if (!canLog) return;
            Debug.Log(message, context);
        }

        public static void Log(object message)
        {
            if (!canLog) return;
            Debug.Log(message);
        }

        public static void LogCaller(
       [System.Runtime.CompilerServices.CallerLineNumber] int line = 0
     , [System.Runtime.CompilerServices.CallerMemberName] string memberName = ""
     , [System.Runtime.CompilerServices.CallerFilePath] string filePath = ""
 )
        {
            Debug.Log($"{line} :: {memberName} :: {filePath}");
        }
    }
}