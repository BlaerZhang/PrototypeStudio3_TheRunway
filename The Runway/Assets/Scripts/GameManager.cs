using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public Camera cam;
    
    public GameObject body;

    public int currentSceneIndex = 0;

    public VolumeProfile globalVolumeProfile;

    public Animator animator;

    private DepthOfField depthOfField;
    
    private AudioManager audioManager;

    private SubtitleManager subtitleManager;

    public bool isDialogPlaying;

    public Animation endingAnimation;

    void Start()
    {
        Screen.SetResolution(Screen.width, Mathf.RoundToInt(Screen.width * (1f / 2.35f)), true);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 24;
        currentSceneIndex = 0;
        cam.cullingMask = LayerMask.GetMask("Airport");
        body.layer = 6;
        if (globalVolumeProfile.TryGet<DepthOfField>(out depthOfField))
        {
            depthOfField.focalLength.overrideState = true;
            depthOfField.focusDistance.overrideState = true;
            depthOfField.focalLength.value = 11;
            depthOfField.focusDistance.value = 0.35f;
        }

        audioManager = GetComponent<AudioManager>();
        subtitleManager = GetComponent<SubtitleManager>();
    }


    void Update()
    {
        UpdateScene();

        if (Input.GetMouseButtonDown(0) && !isDialogPlaying && currentSceneIndex < 3) 
        {
            animator.SetTrigger("isMouseClicked");
            Invoke("ChangeCurrentSceneIndex", 2);
            audioManager.PlayChoiceVoice();
            subtitleManager.SwitchSubtitleTimeline();
        }
    }

    void ChangeCurrentSceneIndex()
    {
        currentSceneIndex += 1;
    }
    
    void UpdateScene()
    {
        switch (currentSceneIndex)
        {
            case 0:
                cam.cullingMask = LayerMask.GetMask("Airport");
                body.layer = 6;
                if (globalVolumeProfile.TryGet<DepthOfField>(out depthOfField))
                {
                    depthOfField.focalLength.overrideState = true;
                    depthOfField.focusDistance.overrideState = true;
                    depthOfField.focalLength.value = 11;
                    depthOfField.focusDistance.value = 0.35f;
                }
                break;
            case 1:
                cam.cullingMask = LayerMask.GetMask("Rovolving Sushi");
                body.layer = 7;
                if (globalVolumeProfile.TryGet<DepthOfField>(out depthOfField))
                {
                    depthOfField.focalLength.overrideState = true;
                    depthOfField.focusDistance.overrideState = true;
                    depthOfField.focalLength.value = 12;
                    depthOfField.focusDistance.value = 0.3f;
                }
                break;
            case 2:
                cam.cullingMask = LayerMask.GetMask("Runway");
                body.layer = 8;
                if (globalVolumeProfile.TryGet<DepthOfField>(out depthOfField))
                {
                    depthOfField.focalLength.overrideState = true;
                    depthOfField.focusDistance.overrideState = true;
                    depthOfField.focalLength.value = 9;
                    depthOfField.focusDistance.value = 0.25f;
                }
                break;
            case 3:
                cam.cullingMask = LayerMask.GetMask("Airport");
                body.layer = 6;
                if (globalVolumeProfile.TryGet<DepthOfField>(out depthOfField))
                {
                    depthOfField.focalLength.overrideState = true;
                    depthOfField.focusDistance.overrideState = true;
                    depthOfField.focalLength.value = 11;
                    depthOfField.focusDistance.value = 0.35f;
                }

                endingAnimation.Play();
                break;
        }
    }
    
    
}
