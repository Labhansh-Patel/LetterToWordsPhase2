#define USELOGS

using UnityEngine;

namespace APICalls
{
    public static class LogSystem
    {
        public static void LogEvent(string log, params object[] overload)
        {
#if USELOGS

//			Debug.LogFormat(log, overload);
#endif
        }

        /// <summary>
        /// Logs the color event.
        /// </summary>
        /// <param name="log">Log.</param>
        /// <param name="color">Color.</param>
        public static void LogColorEvent(string color, string log, params object[] overload)
        {
#if USELOGS

            string logjoin = "<color=" + color + ">" + log + "</color>";
            Debug.LogFormat(logjoin, overload);
#endif
        }

        public static void LogErrorEvent(string log, params object[] overload)
        {
#if USELOGS

            Debug.LogErrorFormat(log, overload);
#endif
        }
    }
}