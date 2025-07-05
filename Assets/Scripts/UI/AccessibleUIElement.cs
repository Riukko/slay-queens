using UnityEngine;

public abstract class AccessibleUIElement : MonoBehaviour
{
    public AccessibleUIElementTag ElementTag;
}

public enum AccessibleUIElementTag
{
    ConfirmationPopup,
    InformationPopup,
}

