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
    }
}