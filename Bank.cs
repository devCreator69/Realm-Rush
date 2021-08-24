using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro;

public class Bank : MonoBehaviour
{
    [SerializeField] int startingBalance = 150;
    [SerializeField] int currentBalance;
    public int CurrentBalance { get { return currentBalance; } } 
    // created property to give access to currentBalance outside of the script 

    [SerializeField] TextMeshProUGUI displayBalance;
    [SerializeField] TextMeshProUGUI winningText;
    [SerializeField] TextMeshProUGUI loosingText;
    [SerializeField] int winningBalance;
    [SerializeField] float loadDelay = 1f;

    void Awake() 
    {
        currentBalance = startingBalance;
        UpdateDisplay();
        winningText.enabled = false;
        loosingText.enabled = false;
       
   
    }

    public void Deposit(int amount)
    {
        currentBalance += Mathf.Abs (amount); 
        //Mathf.Abs passes in the absolute value of the amount so that there can not be a negative amount added to the balance
        UpdateDisplay();
    }

    public void Withdraw(int amount)
    {
        currentBalance -= Mathf.Abs(amount);
        UpdateDisplay();

        if(currentBalance < 0)
        {
            //LOOSE THE GAME
            loosingText.enabled = true;
            Invoke("ReloadScene", loadDelay);

            
        }
        if(currentBalance == winningBalance)
        {
           //WIN THE GAME
          winningText.enabled = true;
          Invoke("ReloadScene", loadDelay);
          
        }
    }  
    
    void UpdateDisplay()
    {
        displayBalance.text = "Gold: " + currentBalance; 
    }

    void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
    
}

