using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Tower towerPrefab;
    [SerializeField] bool isPlaceable;
    
    GridManager gridManager; 
    PathFinder pathFinder;
    Vector2Int coordinates = new Vector2Int();


    void Awake() 
    {
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinder>();
    }

    void Start() 
    {
        if(gridManager != null)
        {
            // converting the positon of this tile in the world to coorinates that the GridManger can work with
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);

            if(!isPlaceable)
            {
                gridManager.BlockNode(coordinates);

            }
        }
    }
        
    // This is a property of isPlaceable which allows for the method isPlaceable to be called in other scripts
    public bool IsPlaceable { get { return isPlaceable; } }
    // Condensed to one line 
    void OnMouseDown()
    {
        if(gridManager.GetNode(coordinates).isWalkable && !pathFinder.WillBlockPath(coordinates))
        {
            // CreateTower is set up as a bool so that if the player does not have enough money to place the tower 
            // the space that they tried to place it at can still be used in the future 

            bool isPlaced = towerPrefab.CreateTower(towerPrefab, transform.position);
            if(isPlaced)
            {
                gridManager.BlockNode(coordinates);
                pathFinder.NotifyReceivers();
            }
        }
    }
}
