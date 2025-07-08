using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public Dictionary<AccessibleUIElementTag, AccessibleUIElement> AccessibleUIElements = new Dictionary<AccessibleUIElementTag, AccessibleUIElement>();

    protected override void Awake()
    {
        base.Awake();

        foreach (AccessibleUIElement uiElement in FindObjectsByType<AccessibleUIElement>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            AccessibleUIElements.Add(uiElement.ElementTag, uiElement);
        }
    }

    public T GetUIElement<T>(AccessibleUIElementTag tag) where T : AccessibleUIElement
    {
        if (AccessibleUIElements.TryGetValue(tag, out var uiElement) && uiElement is T typedUIElement)
        {
            return typedUIElement;
        }
        else
        {
            Debug.LogWarning($"UI Element with tag {tag} and type {typeof(T)} not found.");
            return null;
        }
    }

    public void ShowInfo(string message, PopupStyle? style = null) =>  GetUIElement<InformationPopup>(AccessibleUIElementTag.InformationPopup).Show(message, null, style);

    public async Task<bool> ShowInfoAsync(string message, PopupStyle? style = null) => await GetUIElement<InformationPopup>(AccessibleUIElementTag.InformationPopup).ShowAsync(message, style);
}
