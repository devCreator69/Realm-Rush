using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource audio;

    void Awake()
    {
        StartCoroutine(LoopAudio());
        int numMusicPlayers = FindObjectsOfType<AudioSource>().Length;

        if(numMusicPlayers > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            //Stops music from restarting everytime level reloads
            DontDestroyOnLoad(gameObject);
        } 
    }

    IEnumerator LoopAudio()
    {
        audio = GetComponent<AudioSource>();
        float length = audio.clip.length;

        while(true)
        {
            audio.Play();
            yield return new WaitForSeconds(length);
        }
    }

}
