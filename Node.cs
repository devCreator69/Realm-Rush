using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node 
// pure C# class doesnt inherit from monobehavior 
// can not be attached to game objects  
{
   public Vector2Int coordinates;
   public bool isWalkable; 
   public bool isExplored;
   public bool isPath;
   public Node connectedTo; // holds the parent node that this node branches off of

   public Node(Vector2Int coordinates, bool isWalkable) // can not have methods in pure C# classes
   // constructor - constructs the node object when i want to use it
   // (Vector2Int coordinates, bool isWalkable) are perameters that need to be passed into our variables 
   {
       this.coordinates = coordinates;
       this.isWalkable = isWalkable;
   }
}
