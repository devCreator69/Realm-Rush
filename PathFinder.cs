using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    public Vector2Int StartCoordinates { get { return startCoordinates; } }

    [SerializeField] Vector2Int destinationCoordinates;
    public Vector2Int DestinationCoordinates { get { return destinationCoordinates; } }
 
    // creating the nodes at the Vector2 coorinates above 
    Node startNode;
    Node destinationNode;
    
    // Queue - speacil kind of list that enforces a FIFO order(first in firt out)
    // first item will be added and all others will appear after it
    // items are removed from the front first
    Queue<Node> frontier = new Queue<Node>();
    // frontier represents adding unexplored neighbors to tree 
    // nodes can can only be added to tree once 
    // so the nodes which have already been reached need to be kept track of 

    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();
    Node currentSearchNode; // node we are seraching in four directions from to begin BFS

    Vector2Int [] directions = {Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down};
    GridManager gridManager;// contains all the nodes that need to be searched
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    void Awake() 
    {
       
        gridManager = FindObjectOfType<GridManager>();
        if(gridManager != null)
        {
            grid = gridManager.Grid;
            startNode = grid[startCoordinates];
            destinationNode = grid[destinationCoordinates];
        }
        
    }
    void Start()
    {
       
        GetNewPath();
        
    }

    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoordinates);
    }
     // Overloaded method - methods that can have the same name if they have different signatures
     // ex. the method Instantiate can take just an object 
     // Instantiate(object) or Instantiate(object, position, rotation)
    public List<Node> GetNewPath(Vector2Int coordinates)
    {
        gridManager.ResetNodes();
        BreadthFirstSearch(coordinates);  
        return BuildPath();
    }

    void ExploreNeighbors()
    {
        List<Node> neighbors = new List<Node>();

        foreach(Vector2Int direction in directions)
        {
            // taking the coordinates of current search node and finding the coorinates of its neighbors
            Vector2Int neighborCoords = currentSearchNode.coordinates + direction;

            // check if these neighbor coordinates are in the the GridMangers grid or not 
            if(grid.ContainsKey(neighborCoords))
            {
                // take neighbors list and add the node thats in that grid
                neighbors.Add(grid[neighborCoords]);
            }
        }

        // loop through neighbors that are found and add them to frontier
        foreach(Node neighbor in neighbors)
        {
            if(!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
            {
                neighbor.connectedTo = currentSearchNode;
                reached.Add(neighbor.coordinates, neighbor);
                frontier.Enqueue(neighbor);
            }
        }
    } 

    void BreadthFirstSearch(Vector2Int coordinates)
    {
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        frontier.Clear();
        reached.Clear();

        bool isRunning = true;

        // add node to frontier with the coordinates that are passed in
        frontier.Enqueue(grid[coordinates]);
        // now bfs will start wherever rather than defualting to start node
        reached.Add(coordinates, grid[coordinates]);

        // process to start looping though neighbors and exploring them 
        while(frontier.Count > 0 && isRunning)
        {
            // takes currentSearchNode and places it at the front of frontier queue
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;
            ExploreNeighbors();
            if(currentSearchNode.coordinates == destinationCoordinates)
            {
                isRunning = false; 
            }
        }
    }

    List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = destinationNode;

        path.Add(currentNode);
        currentNode.isPath = true;

        while(currentNode.connectedTo != null)
        // while there are still connected nodes to explore keep moving back through tree
        {
            // takes us one step back down path 
            currentNode = currentNode.connectedTo;
           
            path.Add(currentNode);
            currentNode.isPath = true;
        }
        path.Reverse();
        return path;
    }

     public bool WillBlockPath(Vector2Int coordinates)
    {
        if(grid.ContainsKey(coordinates))
        {
            bool previousState = grid[coordinates].isWalkable;

            grid[coordinates].isWalkable = false;
            List<Node> newPath = GetNewPath();
            grid[coordinates].isWalkable = previousState;

            if(newPath.Count <= 1)
            {
                GetNewPath();
                return true;
            }
        }

       return false;
    }

    public void NotifyReceivers()
    {
        // Broadcast message sends specified method to all scipts 
        // those who have applicable data are to run it
        // sendMessageOptions.DontRequireReciever allows message to work without error even if there are no responders
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }

}
