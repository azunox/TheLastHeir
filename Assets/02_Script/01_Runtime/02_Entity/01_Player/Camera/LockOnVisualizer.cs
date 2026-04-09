using UnityEngine;
using UnityEngine.UI;

namespace TheLastHeir.Runtime.Combat
{
    [System.Serializable]
    public class LockOnVisualizer
    {
        [SerializeField] private Image lockOnIcon;
        [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0);

        public void SetVisible(bool visible)
        {
            if (lockOnIcon != null)
                lockOnIcon.gameObject.SetActive(visible);
        }

        public void UpdatePosition(Transform target, UnityEngine.Camera cam)
        {
            if (target == null || lockOnIcon == null || cam == null) return;
            
            Vector3 screenPos = cam.WorldToScreenPoint(target.position + offset);
            
            if (screenPos.z < 0)
            {
                SetVisible(false);
                return;
            }

            if (!lockOnIcon.gameObject.activeSelf)
                SetVisible(true);

            lockOnIcon.transform.position = screenPos;
        }
    }
}