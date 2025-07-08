using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationPopup : Popup
{
    [SerializeField] private TextMeshProUGUI cancelButtonText;
    [SerializeField] private Image cancelButtonImg;
    [SerializeField] private Color cancelBtnDefaultColor;

    private Action onCancelCallback;

    public void Show(
        string message,
        Action onConfirm,
        Action onCancel = null,
        PopupStyle? style = null)
    {
        SetupPopupBase(message, style);
        SetupCancel(style);

        onConfirmCallback = onConfirm;
        onCancelCallback = onCancel;

        gameObject.SetActive(true);
    }

    public override Task<bool> ShowAsync(string message, PopupStyle? style = null)
    {
        tcs = new TaskCompletionSource<bool>();
        SetupPopupBase(message, style);
        SetupCancel(style);

        gameObject.SetActive(true);
        return tcs.Task;
    }

    private void SetupCancel(PopupStyle? style)
    {
        var s = style ?? default;

        cancelButtonText.text = string.IsNullOrEmpty(s.CancelButtonText) ? "Cancel" : s.CancelButtonText;
        cancelButtonText.color = s.ButtonTextColor ?? btnDefaultTxtColor;
        cancelButtonImg.color = s.CancelButtonColor ?? cancelBtnDefaultColor;
    }

    public void OnClickCancel()
    {
        onCancelCallback?.Invoke();
        tcs?.TrySetResult(false);
        gameObject.SetActive(false);
    }
}
