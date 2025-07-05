using System.Collections.Generic;
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
}
