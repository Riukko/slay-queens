using UnityEngine;

public class GridDataManager : MonoBehaviour
{
    public Cell[,] CellTable;

    public int GridSize;

    [SerializeField]
    private GridGenerator gridGenerator;

    public void Start()
    {
        if(gridGenerator == null)
        {
            Debug.LogError("The Grid Generator object should be passed to the Grid Data Manager");
            return;
        }

        GridSize = gridGenerator.GridSize;
        GridHelpers.HighlightGridOuterLines();
    }

    #region Singleton
    private static GridDataManager instance = null;
    public static GridDataManager Instance => instance;
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
