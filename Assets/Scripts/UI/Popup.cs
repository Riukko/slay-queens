using System;
using System.Threading.Tasks;
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
    protected TaskCompletionSource<bool> tcs;

    public virtual void Show(string message, Action onConfirm, PopupStyle? style = null)
    {
        SetupPopupBase(message, style);
        onConfirmCallback = onConfirm;
        gameObject.SetActive(true);
    }

    public virtual Task<bool> ShowAsync(string message, PopupStyle? style = null)
    {
        tcs = new TaskCompletionSource<bool>();
        SetupPopupBase(message, style);
        gameObject.SetActive(true);
        return tcs.Task;
    }

    protected virtual void SetupPopupBase(string message, PopupStyle? style)
    {
        var s = style ?? default;

        popupMessage.text = message;

        confirmButtonText.text = string.IsNullOrEmpty(s.ConfirmButtonText) ? "OK" : s.ConfirmButtonText;
        confirmButtonText.color = s.ButtonTextColor ?? btnDefaultTxtColor;
        confirmButtonImg.color = s.ConfirmButtonColor ?? confirmBtnDefaultColor;
        popupBackground.color = s.BackgroundColor ?? popupDefaultBackgroundColor;
    }

    public virtual void OnClickConfirm()
    {
        onConfirmCallback?.Invoke();
        tcs?.TrySetResult(true);
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