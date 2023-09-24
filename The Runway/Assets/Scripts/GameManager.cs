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
    // Start is called before the first frame update
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScene();
        
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("isMouseClicked");
            Invoke("ChangeCurrentSceneIndex", 2);
        }
    }

    void ChangeCurrentSceneIndex()
    {
        currentSceneIndex = (currentSceneIndex + 1) % 3;
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
        }
    }
    
    
}
