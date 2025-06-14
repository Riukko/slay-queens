using UnityEngine;

public class GeneralOptionsController : MonoBehaviour
{
    public void QuitToMainMenu()
    {
        CustomSceneManager.Instance.LoadScene(SceneType.MainMenu);
    }
}
