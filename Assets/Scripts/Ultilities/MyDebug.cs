using UnityEngine;

namespace VitsehLand.Scripts.Ultilities
{
    public static class MyDebug
    {
        public static bool enableDebugLog = false;

        public static void Log(object message, Object context)
        {
            if (!enableDebugLog) return;
            Debug.Log(message, context);
        }

        public static void Log(object message)
        {
            if (!enableDebugLog) return;
            Debug.Log(message);
        }

        public static void LogCaller(
       [System.Runtime.CompilerServices.CallerLineNumber] int line = 0
     , [System.Runtime.CompilerServices.CallerMemberName] string memberName = ""
     , [System.Runtime.CompilerServices.CallerFilePath] string filePath = ""
 )
        {
            if (!enableDebugLog) return;
            Debug.Log($"{line} :: {memberName} :: {filePath}");
        }
    }
}