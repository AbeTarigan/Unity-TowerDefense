using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    [SerializeField] Vector2Int destinateCoordinates;

    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    Queue<Node> frontire = new Queue<Node>();
    Dictionary<Vector2Int ,Node> reached = new Dictionary<Vector2Int ,Node>();

    Vector2Int[] directions = { Vector2Int.right ,Vector2Int.left ,Vector2Int.up ,Vector2Int.down };
    GridManager m_gridManager;
    Dictionary<Vector2Int ,Node> grid = new Dictionary<Vector2Int ,Node>();


    private void Awake()
    {
        m_gridManager = FindObjectOfType<GridManager>();
        if(m_gridManager != null)
        {
            grid = m_gridManager.Grid;
        }

        startNode = new Node(startCoordinates ,true);
        destinationNode = new Node(destinateCoordinates ,true);
    }

    // Start is called before the first frame update
    void Start()
    {
        BreadthFristSearch();
    }

    void ExploreNeighbors()
    {
        List<Node> neighbors = new List<Node>();

        foreach(Vector2Int direction in directions)
        {
            Vector2Int neighborCoords = currentSearchNode.coordinates + direction;
            if(grid.ContainsKey(neighborCoords))
            {
                neighbors.Add(grid[neighborCoords]);
            }
        }

        foreach(Node neighbor in neighbors)
        {
            if(!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
            {
                reached.Add(neighbor.coordinates ,neighbor);
                frontire.Enqueue(neighbor);
            }
        }
    }

    void BreadthFristSearch()
    {
        bool isRunning = true;

        frontire.Enqueue(startNode);
        reached.Add(startCoordinates ,startNode);

        while(frontire.Count > 0 && isRunning)
        {
            currentSearchNode = frontire.Dequeue();
            currentSearchNode.isExplored = true;
            ExploreNeighbors();
            if(currentSearchNode.coordinates == destinateCoordinates)
            {
                isRunning = false;
            }
        }
    }
}
