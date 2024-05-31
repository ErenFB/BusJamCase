using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PathfindingData", menuName = "ScriptableObjects/PathfindingData", order = 1)]
public class PathfindingData : ScriptableObject
{
    public int width;
    public int height;
    public List<PathNodeData> nodes = new List<PathNodeData>();

    [System.Serializable]
    public class PathNodeData
    {
        public int x;
        public int y;
        public bool isWalkable;
    }

    public void SavePathfinding(Pathfinding pathfinding)
    {
        width = pathfinding.GetGrid().GetWidth();
        height = pathfinding.GetGrid().GetHeight();
        nodes.Clear();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                PathNode node = pathfinding.GetNode(x, y);
                nodes.Add(new PathNodeData { x = x, y = y, isWalkable = node.isWalkable });
            }
        }
    }

    public Pathfinding LoadPathfinding()
    {
        Pathfinding pathfinding = new Pathfinding(width, height);

        foreach (PathNodeData nodeData in nodes)
        {
            PathNode node = pathfinding.GetNode(nodeData.x, nodeData.y);
            node.SetIsWalkable(nodeData.isWalkable);
        }

        return pathfinding;
    }
}
