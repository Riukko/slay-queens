using UnityEngine;

[ExecuteAlways]
public class GameManager : MonoBehaviour
{
    [SerializeField]

    public GridManager GridManager;




    #region Singleton
    private static GameManager instance = null;
    public static GameManager Instance => instance;
    public static bool HasInstance => instance != null;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion
}