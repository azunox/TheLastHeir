using UnityEngine;
using System.Collections;

public class TeleportTrigger : MonoBehaviour
{
    [Header("텔레포트 목적지")]
    public GameObject targetDestination;

    [Header("플레이어 Transform (루트)")]
    public GameObject player;

    [Header("페이드 스크립트")]
    public ScreenFader screenFader;

    [Header("텔레포트 시 비활성화할 스크립트(선택 사항)")]
    public MonoBehaviour[] movementScripts;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(TeleportWithFade(player));
        }
    }

    private IEnumerator TeleportWithFade(GameObject player)
    {
        // 1. 이동 관련 스크립트 비활성화
        foreach (var script in movementScripts)
        {
            if (script != null) script.enabled = false;
        }

        // 2. 화면 어둡게
        yield return StartCoroutine(screenFader.FadeOut());

        // 3. 캐릭터 이동
        var cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
            player.transform.position = targetDestination.transform.position;
            cc.enabled = true;
        }
        else
        {
            var rb = player.GetComponent<Rigidbody>();
            if (rb != null)
                rb.MovePosition(targetDestination.transform.position);
            else
                player.transform.position = targetDestination.transform.position;
        }

        // 4. 로딩 애니메이션
        if (screenFader.loadingAnimImage != null)
            screenFader.loadingAnimImage.gameObject.SetActive(true);

        Coroutine animCoroutine = StartCoroutine(screenFader.PlayLoadingSpriteAnim());
        yield return new WaitForSeconds(screenFader.blackScreenDuration);

        if (animCoroutine != null)
            StopCoroutine(animCoroutine);

        if (screenFader.loadingAnimImage != null)
            screenFader.loadingAnimImage.gameObject.SetActive(false);

        // 5. 화면 밝게
        yield return StartCoroutine(screenFader.FadeIn());

        // 6. 이동 관련 스크립트 다시 활성화
        foreach (var script in movementScripts)
        {
            if (script != null) script.enabled = true;
        }
    }
}
