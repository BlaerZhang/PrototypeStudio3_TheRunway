using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SubtitleManager : MonoBehaviour
{
    public TimelineAsset sushiSubtitle;
    public TimelineAsset runwaySubtitle;
    public TimelineAsset endingSubtitle;

    public PlayableDirector playableDirector;

    private GameManager gameManager;
    
    void Start()
    {
        gameManager = GetComponent<GameManager>();
    }
    
    void Update()
    {
        
    }

    public void SwitchSubtitleTimeline()
    {
        switch (gameManager.currentSceneIndex)
        {
            case 0:
                playableDirector.playableAsset = sushiSubtitle;
                playableDirector.Play();
                break;
            case 1:
                playableDirector.playableAsset = runwaySubtitle;
                playableDirector.Play();
                break;
            case 2:
                playableDirector.playableAsset = endingSubtitle;
                playableDirector.Play();
                break;
        }
    }
}
