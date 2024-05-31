using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using UnityEngine;
using Debug = UnityEngine.Debug;

public class CreateGrid : MonoBehaviour
{

    [SerializeField] public static int FinalX=5, FinalY=9;
    public PathfindingData pathfindingData;
    private GameObject[] gm;
    public static GameObject player;
    [HideInInspector] public GameObject selectedObject;
    [SerializeField] private PathfindingDebugStepVisual pathfindingDebugStepVisual;
    [SerializeField] private PathfindingVisual pathfindingVisual;
    [SerializeField] private CharacterPathfindingMovementHandler characterPathfinding;
    public Pathfinding pathfinding;
    [HideInInspector] public int x, y;
    [HideInInspector] public int width, height;
    public GameObject kup;

    public Pathfinding GetPathfinding() { return pathfinding; }
    public void CreateGrids()
    {
        pathfinding = new Pathfinding(x, y);
        pathfindingDebugStepVisual.Setup(pathfinding.GetGrid());
        pathfindingVisual.SetGrid(pathfinding.GetGrid());

        gm = GameObject.FindGameObjectsWithTag("Player");
        pathfindingData.SavePathfinding(pathfinding);
    }

    private void Awake()
    {
        if (pathfindingData != null)
        {
            pathfinding = pathfindingData.LoadPathfinding();
        }
    }
    public void UpdateGrids()
    {
        if (pathfindingData != null)
        {
            pathfindingData.SavePathfinding(pathfinding);
        }

    }
    private void Start()
    {
        if (pathfinding == null)
        {
           // Debug
        }

        if (kup == null)
        {
            //Debug
        }
        GameObject[] grds = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in grds)
        {
            go.GetComponent<CharacterPathfindingMovementHandler>().WalkableCloseInt(out int a, out int b);

            if (go.GetComponent<CharacterController>().characterColor == CharacterColor.Destroy)
            {
                DestroyObject(go.transform.position);
                WalkableClose(a, b);
            }
            else if (go.GetComponent<CharacterController>().characterColor == CharacterColor.Empty)
            {
            }
            else
            {
                WalkableClose(a, b);
               
            }

        } 
        UpdateGrids();
    }
    public void DestroyObject(Vector3 targetPosition, float tolerance = 0.1f)
    {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Grids");
        foreach (GameObject obj in allObjects)
        {
            if (Vector3.Distance(obj.transform.position, targetPosition) < tolerance)
            {
                Destroy(obj);
                return;  
            }
        }

    }
    public void DeleteAll()
    {
        GameObject[] grds = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in grds)
        {
            DestroyImmediate(go);
        }
        GameObject[] mids = GameObject.FindGameObjectsWithTag("Grids");
        foreach (GameObject m in mids)
        {
            DestroyImmediate(m);
        }

    }
    public GameObject CreateCharacter(int x, int y)
    {
        if (pathfinding == null)
        {
            //Debug
            return null;
        }

        if (kup == null)
        {
            //Debug
            return null;
        }

        GameObject spawned = Instantiate(kup, pathfinding.GetGrid().GetWorldPositionReal(x, y), Quaternion.identity);
        return spawned;
    }

    public void CreateCharacterZERO()
    {
        if (pathfinding == null)
        {
            // Debug
            return;
        }

        Instantiate(kup, pathfinding.GetGrid().GetWorldPositionReal(0, 0), Quaternion.identity);
    }

    void SelectObject()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.collider.CompareTag("Player") && hit.collider.GetComponent<CharacterController>().characterColor != CharacterColor.Destroy)
            {
                {
                    selectedObject = hit.collider.gameObject;
                    player = selectedObject;
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.can = true;
            SelectObject();
            if (!player)
            {
                return;
            }
            Vector3 playerWorldPosition = player.transform.position;
            pathfinding.GetGrid().GetXY(playerWorldPosition, out int x, out int y);
  
            PathNode node = pathfinding.GetNode(x, y);
            List<PathNode> path = pathfinding.FindPath(x, y, FinalX, FinalY);
            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {                   
                    node.SetIsWalkable(true);
                }
            }
        }
    }

    public void WalkableClose(int x, int y)
    {
        PathNode node = Pathfinding.Instance.GetNode(x, y);
        if(node != null)
        {
            node.SetIsWalkable(false);
        }
    
    }

    public void CheckAll()
    {
        foreach (GameObject go in gm)
        {
            Vector3 position = go.transform.position;
            pathfinding.GetGrid().GetXY(position, out int x, out int y);
            PathNode node = pathfinding.GetNode(x, y);
            List<PathNode> path = pathfinding.FindPath(x, y, FinalX, FinalY);
            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    go.GetComponent<MeshRenderer>().material.color = Color.blue;
                }
            }
        }
    }
}
