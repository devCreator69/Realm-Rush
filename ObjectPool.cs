using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] [Range(0.1f, 30f)] float spawnTimer = 1f;
    [SerializeField] [Range(0, 50)] int poolSize = 5; 
    // range added so that potential errors cant be triggered 
    GameObject[] pool; 
    void Awake()
    {
        PopulatePool();
    }
    void Start()
    {
       StartCoroutine(SpawnEnemy());
    }

    void PopulatePool()
    {
        pool = new GameObject[poolSize];

        for(int i = 0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(enemyPrefab, transform);
            pool[i].SetActive(false);
        } 
    }
    void EnableObjectInPool()
    {
        for(int i = 0; i < pool.Length; i++)
        {
            if(pool[i].activeInHierarchy == false) // check if object is active in hirearchy
            {
                pool[i].SetActive(true); // check and set the first item in pool which is false to true
                return; // return prevents all enemies from being active all the time 
            }
        }
    }    
    IEnumerator SpawnEnemy()
    {
        while(true)
        { 
            EnableObjectInPool();
            yield return new WaitForSeconds(spawnTimer);
        }
    }
}
