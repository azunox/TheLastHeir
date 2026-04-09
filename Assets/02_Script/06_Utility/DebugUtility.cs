using UnityEngine;

namespace TheLastHeir.Utility
{
    public static class DebugUtility
    {
        /// <summary>
        /// 로그 태그 활성 여부 — 필요 시 특정 시스템 로그만 켜서 볼 수 있음.
        /// </summary>
        public static bool EnableGeneralLogs = true;
        public static bool EnableEntityLogs = false;
        public static bool EnableCombatLogs = false;

        /// <summary>
        /// 일반 로그 출력 (빌드에서는 제거됨)
        /// </summary>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void Log(object message, Object context = null)
        {
            if (!EnableGeneralLogs) return;
            Debug.Log($"<color=#B3E5FC>[LOG]</color> {message}", context);
        }

        /// <summary>
        /// 경고 로그 출력
        /// </summary>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogWarning(object message, Object context = null)
        {
            Debug.LogWarning($"<color=#FFF176>[WARN]</color> {message}", context);
        }

        /// <summary>
        /// 오류 로그 출력
        /// </summary>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogError(object message, Object context = null)
        {
            Debug.LogError($"<color=#EF9A9A>[ERROR]</color> {message}", context);
        }

        /// <summary>
        /// 엔티티 관련 로깅 (활성화 시에만 출력)
        /// </summary>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogEntity(object message, Object context = null)
        {
            if (EnableEntityLogs)
                Debug.Log($"<color=#AED581>[ENTITY]</color> {message}", context);
        }

        /// <summary>
        /// 전투/피격 관련 로깅 (활성화 시에만 출력)
        /// </summary>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogCombat(object message, Object context = null)
        {
            if (EnableCombatLogs)
                Debug.Log($"<color=#F48FB1>[COMBAT]</color> {message}", context);
        }
    }
}
