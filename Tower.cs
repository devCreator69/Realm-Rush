using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tower : MonoBehaviour
{
    [SerializeField] int cost = 75; 
     [SerializeField] float buildDelay = 1f; 

    void Start() 
    {
        StartCoroutine(Build());
    }

    public bool CreateTower(Tower tower, Vector3 position)
    {
        Bank bank = FindObjectOfType<Bank>();

        if (bank == null)
        {
            return false; 
        }

        if (bank.CurrentBalance >= cost)
        {
            Instantiate(tower.gameObject, position, Quaternion.identity);
            bank.Withdraw(cost);
           
           // UpdateDisplay();
            return true;
        }

        return false;
        // clears error messages about the bool not returning anything 

    }
    IEnumerator Build()
    {
        foreach (Transform child in transform)
        {
           child.gameObject.SetActive(false);
           foreach(Transform grandchild in child)
           {
               grandchild.gameObject.SetActive(false);
           }
        }
        // made so players cant shoot right after placing tower
        // loops through tower children and grandchildren to turn on each component individually
        // after x amount of seconds the nect part will be enabled
        // only grandchild is the particle system which shoots the bolts

        foreach (Transform child in transform)
        {
           child.gameObject.SetActive(true);
           yield return new WaitForSeconds(buildDelay);
           foreach(Transform grandchild in child)
           {
               grandchild.gameObject.SetActive(true);
           }   
        }
    }
}
