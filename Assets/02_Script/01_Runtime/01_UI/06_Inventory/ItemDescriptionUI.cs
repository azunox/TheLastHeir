using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using TheLastHeir.Runtime.Entity;

public class ItemDescriptionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private Image itemIconImage;
    [SerializeField] private Button actionButton;
    [SerializeField] private TextMeshProUGUI actionButtonText;

    public void UpdateDescription(Item item, string actionName, Action onClickAction)
    {
        if (item == null) { ClearDescription(); return; }

        gameObject.SetActive(true);
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.description;

        if (item.icon != null) { itemIconImage.sprite = item.icon; itemIconImage.enabled = true; }
        else itemIconImage.enabled = false;

        if (onClickAction != null)
        {
            actionButton.gameObject.SetActive(true);
            actionButtonText.text = actionName;
            actionButton.onClick.RemoveAllListeners();
            actionButton.onClick.AddListener(() => onClickAction.Invoke());
        }
        else actionButton.gameObject.SetActive(false);
    }

    public void ClearDescription()
    {
        itemNameText.text = "";
        itemDescriptionText.text = "";
        itemIconImage.enabled = false;
        if (actionButton != null) actionButton.gameObject.SetActive(false);
    }
}