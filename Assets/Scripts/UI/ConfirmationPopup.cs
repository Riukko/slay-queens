using System;
using TMPro;
using UnityEngine;

public class ConfirmationPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI popupMessage;

    [SerializeField] private TextMeshProUGUI confirmButtonText;

    [SerializeField] private TextMeshProUGUI cancelButtonText;
    private Action onConfirmCallback;

    public void Show(string message, Action onConfirm, string confirmBtnTxt = "Yes", string cancelBtnTxt = "No")
    {
        onConfirmCallback = onConfirm;
        popupMessage.text = message;
        confirmButtonText.text = confirmBtnTxt;
        cancelButtonText.text = cancelBtnTxt;
        gameObject.SetActive(true);
    }

    public void OnClickConfirm()
    {
        onConfirmCallback?.Invoke();
        gameObject.SetActive(false);
    }

    public void OnClickCancel()
    {
        gameObject.SetActive(false);
    }
}