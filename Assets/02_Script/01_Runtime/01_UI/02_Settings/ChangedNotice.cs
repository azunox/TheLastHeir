using UnityEngine;

public class ChangedNotice : MonoBehaviour
{
    [Header("Notice Settings")]
    [SerializeField] private Sprite img;
    [SerializeField] private string noticeName;

    [SerializeField] private ItemNoti notice;

    public void OnClicked()
    {
        notice.GetItemNoti(img, noticeName);
    }
}
