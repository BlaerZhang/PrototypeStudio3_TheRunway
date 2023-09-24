using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource ambience;
    public AudioSource dialog;
    
    public AudioClip airportAmbience;
    public AudioClip sushiAmbience;
    public AudioClip runwayAmbience;

    private GameManager gameManager;
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        ambience.clip = airportAmbience;
        ambience.Play();
    }
    
    void Update()
    {
        switch (gameManager.currentSceneIndex)
        {
            case 0:
                if (ambience.clip != airportAmbience)
                {
                    ambience.clip = airportAmbience;
                    ambience.Play();
                }
                break;
            case 1:
                if (ambience.clip != sushiAmbience)
                {
                    ambience.clip = sushiAmbience;
                    ambience.Play();
                }
                break;
            case 2:
                if (ambience.clip != runwayAmbience)
                {
                    ambience.clip = runwayAmbience;
                    ambience.Play();
                }
                break;
        }
    }
}
