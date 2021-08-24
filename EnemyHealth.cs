using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
//Used so that any component which has the EnemyHealth script will automatically be given the enemy script
//this is crucial bc without the enemy script being attached to the object this script will not work

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHitPoints = 5;
    [Tooltip("Adds amount to maxHitPoints when enemy dies.")]
    [SerializeField] int difficultyRamp = 1;
    int currenthitPoints = 0;

    Enemy enemy; 

    void OnEnable()
    {
        currenthitPoints = maxHitPoints;
    }

    void Start() 
    {
        enemy = GetComponent<Enemy>();
        // Use GetComponent instead of GetObjectOfType bc 
        // the Enemy and Enemy health scripts are both on the root of the enemy object
    }
    
    void OnParticleCollision(GameObject other) 
    {
        ProcessHit();
    }

    void ProcessHit()
    {
        currenthitPoints--;
        if(currenthitPoints <= 0)
        {
            gameObject.SetActive(false);
            maxHitPoints += difficultyRamp;
            enemy.RewardGold();
        }
    }   
}
