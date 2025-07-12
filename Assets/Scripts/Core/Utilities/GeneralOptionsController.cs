using UnityEngine;

public class GeneralOptionsController : MonoBehaviour
{
    public void QuitToMainMenu()
    {
        UIManager.Instance.GetUIElement<ConfirmationPopup>(AccessibleUIElementTag.ConfirmationPopup)
            .Show("Are you sure you want to quit to the main menu?",
            () => CustomSceneManager.Instance.LoadScene(SceneType.MainMenu));
    }
}
