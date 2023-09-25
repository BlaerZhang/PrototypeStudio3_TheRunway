using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource ambience;
    public AudioSource dialog;
    public AudioSource choiceVoice;
    
    public AudioClip airportAmbience;
    public AudioClip sushiAmbience;
    public AudioClip runwayAmbience;
    
    public AudioClip airportDialog;
    public AudioClip sushiDialog;
    public AudioClip runwayDialog;
    
    public AudioClip airportChoiceVoice;
    public AudioClip sushiChoiceVoice;
    public AudioClip runwayChoiceVoice;

    private GameManager gameManager;
    void Start()
    {
        gameManager = GetComponent<GameManager>();
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
                    dialog.clip = sushiDialog;
                    dialog.Play();
                }
                break;
            case 2:
                if (ambience.clip != runwayAmbience)
                {
                    ambience.clip = runwayAmbience;
                    ambience.Play();
                    dialog.clip = runwayDialog;
                    dialog.Play();
                }
                break;
            case 3:
                if (ambience.clip != airportAmbience)
                {
                    ambience.clip = airportAmbience;
                    ambience.Play();
                }
                break;
        }

        gameManager.isDialogPlaying = dialog.isPlaying || choiceVoice.isPlaying;
    }

    public void PlayChoiceVoice()
    {
        switch (gameManager.currentSceneIndex)
        {
            case 0:
                if (choiceVoice.clip != airportChoiceVoice)
                {
                    choiceVoice.clip = airportChoiceVoice;
                    choiceVoice.Play();
                }
                break;
            case 1:
                if (choiceVoice.clip != sushiChoiceVoice)
                {
                    choiceVoice.clip = sushiChoiceVoice;
                    choiceVoice.Play();
                }
                break;
            case 2:
                if (choiceVoice.clip != runwayChoiceVoice)
                {
                    choiceVoice.clip = runwayChoiceVoice;
                    choiceVoice.Play();
                }
                break;
            case 3:
                if (choiceVoice.clip != airportChoiceVoice)
                {
                    choiceVoice.clip = airportChoiceVoice;
                    choiceVoice.Play();
                }
                break;
        }
    }
}
