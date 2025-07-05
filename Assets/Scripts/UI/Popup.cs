using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Popup : AccessibleUIElement
{
    [Header("Popup Base")]
    [SerializeField] protected TextMeshProUGUI popupMessage;
    [SerializeField] protected TextMeshProUGUI confirmButtonText;
    [SerializeField] protected Image confirmButtonImg;
    [SerializeField] protected Image popupBackground;

    [SerializeField] protected Color confirmBtnDefaultColor;
    [SerializeField] protected Color btnDefaultTxtColor;
    [SerializeField] protected Color popupDefaultBackgroundColor;

    protected Action onConfirmCallback;

    public virtual void Show(
        string message,
        Action onConfirm,
        PopupStyle? style = null)
    {
        onConfirmCallback = onConfirm;

        var s = style ?? default;

        popupMessage.text = message;
        confirmButtonText.text = string.IsNullOrEmpty(s.ConfirmButtonText) ? "OK" : s.ConfirmButtonText;

        confirmButtonText.color = s.ButtonTextColor ?? btnDefaultTxtColor;
        confirmButtonImg.color = s.ConfirmButtonColor ?? confirmBtnDefaultColor;
        popupBackground.color = s.BackgroundColor ?? popupDefaultBackgroundColor;

        gameObject.SetActive(true);
    }

    public void OnClickConfirm()
    {
        onConfirmCallback?.Invoke();
        gameObject.SetActive(false);
    }
}

public struct PopupStyle
{
    public Color? ConfirmButtonColor;
    public Color? CancelButtonColor;
    public Color? ButtonTextColor;
    public Color? BackgroundColor;
    public string ConfirmButtonText;
    public string CancelButtonText;
}