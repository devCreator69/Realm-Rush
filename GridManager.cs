using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // Dictionary
    // they store a key-value pair, keys link to values
    // coordinates of the node will be the key (a unique value to somewhere in the world)
    // values will be the node objects themselves (including position, isWalkable, is path etc.)

    // This dictionary will store all of the nodes that make up the world 
    // must tell what data type the key will be and of the value it will hold

    [SerializeField] Vector2Int gridSize; // using Vector2Int will allow me to specify the x hight and y width of grid
    // world grid size should match unity editor snap settings
    [SerializeField] int unityGridSize = 10;

    // making a property out of the varaible unityGridSize
    public int UnityGridSize {get { return unityGridSize; } }


    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>(); // 

    public Dictionary<Vector2Int, Node> Grid { get{ return grid; } } // created property for entire grid

    void Awake()
    {
        CreateGrid();
    }

    // when elements within grid need to be accessed use the GetNode method and pass in the coordinates 
    public Node GetNode(Vector2Int coordinates)
    {
        if(grid.ContainsKey(coordinates))
        {
            return grid[coordinates];
        }
        return null;
    }

   public void BlockNode(Vector2Int coordinates) 
   {
       // Primary method to use in Tiles script to talk to GridManger to block nodes where the isPlaceable flag is false 
       if(grid.ContainsKey(coordinates))
       {
           grid[coordinates].isWalkable = false;
       }
   }

   public void ResetNodes()
   {
       foreach(KeyValuePair<Vector2Int, Node> entry in grid)
       {
           entry.Value.connectedTo = null;
           entry.Value.isExplored = false;
           entry.Value.isPath = false;
       }
   }
    // Convert world positions to grid coordinates and back again in GetPositionFromCoorinates
    public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
        Vector2Int coordinates = new Vector2Int();
        coordinates.x = Mathf.RoundToInt(position.x / unityGridSize);
        coordinates.y = Mathf.RoundToInt(position.z / unityGridSize);

        return coordinates;
    } 

    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates)
    {
        Vector3 position = new Vector3();
        position.x = coordinates.x * unityGridSize;
        position.z = coordinates.y * unityGridSize;

       return position;

    }
    

    void CreateGrid()
    // from 0,0 this method will loop thru every element in the grid and add new node object for that position 
    // for everytime x is incrimented by 1 it will loop through every valur for y 
    // if grid was 5x5 x would incriment once then the nested for loop for y would go up 5 times
    {
        for(int x  = 0; x < gridSize.x; x++)
        {
            for(int y = 0; y < gridSize.y; y++)
            { 
                Vector2Int coordinates = new Vector2Int(x,y);
                grid.Add(coordinates, new Node(coordinates, true));
            }
        }

    }
}
