using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] [Range(0f,5f)] float speed = 5f;
    // Range used bc negative numbers used in Lerp breaks code
    // Only allows numbers 0-5 to be input
    // Adds range slider in inspector

   List <Node> path = new List <Node>();

    Enemy enemy;
    PathFinder pathFinder;
    GridManager gridManager;
    PlayerLives playerLives;
    void OnEnable() // Called whenever an object is enabled or diabled in the hierarchy
    {
       ReturnToStart();
       RecalculatePath(true);   
        
    }
    void Awake() 
    {
        enemy = GetComponent<Enemy>();
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinder>();
        playerLives = FindObjectOfType<PlayerLives>();

///////////////////////////////////////////////////
      
    }
    void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = new Vector2Int();

        if(resetPath)
        {
            coordinates = pathFinder.StartCoordinates;
        }
        else
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
         
        }
        StopAllCoroutines();
        path.Clear();
        path = pathFinder.GetNewPath(coordinates);
        StartCoroutine(FollowPath());
    }
    void ReturnToStart()
    {
        transform.position = gridManager.GetPositionFromCoordinates(pathFinder.StartCoordinates);
    }
    void FinishPath()
    {

        //////////////////////////////////////////////////
        playerLives.LooseLife();


        enemy.StealGold();
        
        
        gameObject.SetActive(false);
        //disables enemy from hierarchy for the moment then will be free fpr the pool to reuse later
    }

    IEnumerator FollowPath()
    // IEnumerator used for coroutine
    {
        // Lerp (Linear Interpolation) requires 3 peramaters start, end and the travel percent 
        // Stops enemy from jumping from box to box, fluid movemwnt will be created
        
        for(int i = 1; i < path.Count; i++) 
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);
            float travelPercent = 0f;

            
            transform.LookAt(endPosition);
        

            // start position is represented by 0 and end is 1
            // while not at end 
            while(travelPercent < 1f) 
            {
                // update travel percent with time.deltatime using the numbers between 0-1 
                // multiplied by whatever speed I want it to travel
                travelPercent += Time.deltaTime * speed;
                // movement will be made more fluid 
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }

        FinishPath();
    }
}