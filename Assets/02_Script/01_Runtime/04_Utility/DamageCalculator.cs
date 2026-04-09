using UnityEngine;
using TheLastHeir.Runtime.Structs;
using TheLastHeir.Utility;

namespace TheLastHeir.Runtime.Utility
{
    /// <summary>
    /// 스킬의 속성과 피격자의 경감률을 기반으로 최종 피해량을 계산하는 헬퍼 클래스
    /// </summary>
    public static class DamageCalculator
    {
        /// <summary>
        /// 최종 피해량 계산
        /// </summary>
        /// <param name="dmg">스킬이 전달하는 데미지 (속성 + 수치)</param>
        /// <param name="neg">피격자의 속성별 데미지 경감률</param>
        /// <param name="multiplierData">속성별 배율 데이터 (ElementMultiplier ScriptableObject)</param>
        /// <returns>최종 피해량</returns>
        public static float CalculateFinal(in Damage dmg, in DamageNegation neg, ElementMultiplier multiplierData)
        {
            Debug.Log($"[BeforeCalc] Damage.Amount = {dmg.Amount}");

            // Holy, Dark는 일반 배율 시스템에 포함되지 않음 (별도 처리 예정)
            if (dmg.Type == ElementType.Holy || dmg.Type == ElementType.Dark)
            {
                DebugUtility.LogCombat($"[DamageCalculator] {dmg.Type} 타입은 별도 처리 대상입니다.");
                return dmg.Amount; // 그대로 반환
            }

            // 속성 배율
            float multiplier = multiplierData?.GetMultiplier(dmg.Type) ?? 1f;

            // 속성별 경감률
            float reduction = 1f - neg.GetNegation(dmg.Type);

            // 최종 피해량
            float final = dmg.Amount * multiplier * reduction;

            return Mathf.Max(final, 1f);
        }
    }
}