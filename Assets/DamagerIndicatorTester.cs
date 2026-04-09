using UnityEngine;
using System.Collections;

public class DamageIndicatorTester : MonoBehaviour
{
    public int numberOfIndicatorsToTest = 10;
    public float spawnInterval = 0.2f;
    public Vector3 testWorldPosition = Vector3.zero;

    void Start()
    {
        StartCoroutine(TestIndicators());
    }

    private IEnumerator TestIndicators()
    {
        if (DamageIndicatorManager.Instance == null)
        {
            Debug.LogError("DamageIndicatorManager.Instance가 없습니다. 테스트를 진행할 수 없습니다.");
            yield break;
        }

        for (int i = 0; i < numberOfIndicatorsToTest; i++)
        {
            int damage = Random.Range(10, 100);
            bool isCritical = Random.value > 0.8f; 
            bool isPlayerTarget = Random.value > 0.5f;

            DamageIndicatorManager.Instance.ShowDamage(damage, testWorldPosition, isPlayerTarget, isCritical);
            
            yield return new WaitForSeconds(spawnInterval);
        }

        int healAmount = Random.Range(20, 50);
        DamageIndicatorManager.Instance.ShowDamage(-healAmount, testWorldPosition, true, false);
    }
}