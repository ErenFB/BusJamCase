using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    public List<Vector3> gridPositions;
    private Pathfinding pathfinding;
    [SerializeField] private PathfindingDebugStepVisual pathfindingDebugStepVisual;
    [SerializeField] private PathfindingVisual pathfindingVisual;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            gridPositions = new List<Vector3>();
        }
    }

    public void CreateGrids(int x, int y, PathfindingDebugStepVisual debugVisual, PathfindingVisual visual)
    {
        pathfinding = new Pathfinding(x, y);
        pathfindingDebugStepVisual = debugVisual;
        pathfindingVisual = visual;

        pathfindingDebugStepVisual.Setup(pathfinding.GetGrid());
        pathfindingVisual.SetGrid(pathfinding.GetGrid());

        gridPositions.Clear();
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                gridPositions.Add(new Vector3(i, 0, j));
            }
        }
    }

    public List<Vector3> LoadGridPositions()
    {
        return gridPositions;
    }

    public Pathfinding GetPathfinding()
    {
        return pathfinding;
    }
}
