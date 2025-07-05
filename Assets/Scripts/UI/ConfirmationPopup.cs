using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ConfirmationPopup : Popup
{
    [Header("Confirmation Popup")]
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
        base.Show(message, onConfirm, style);

        onCancelCallback = onCancel;

        var s = style ?? default;

        cancelButtonText.text = string.IsNullOrEmpty(s.CancelButtonText) ? "Cancel" : s.CancelButtonText;
        cancelButtonText.color = s.ButtonTextColor ?? btnDefaultTxtColor;
        cancelButtonImg.color = s.CancelButtonColor ?? cancelBtnDefaultColor;
    }

    public void OnClickCancel()
    {
        onCancelCallback?.Invoke();
        gameObject.SetActive(false);
    }
}