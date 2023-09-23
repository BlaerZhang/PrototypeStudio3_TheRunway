using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera cam;
    
    public GameObject body;

    public int currentSceneIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        currentSceneIndex = 0;
        cam.cullingMask = LayerMask.GetMask("Airport");
        body.layer = 6;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            switch (currentSceneIndex)
            {
                case 0:
                    cam.cullingMask = LayerMask.GetMask("Rovolving Sushi");
                    body.layer = 7;
                    break;
                case 1:
                    cam.cullingMask = LayerMask.GetMask("Runway");
                    body.layer = 8;
                    break;
                case 2:
                    cam.cullingMask = LayerMask.GetMask("Airport");
                    body.layer = 6;
                    break;
            }

            currentSceneIndex = (currentSceneIndex + 1) % 3;
        }
    }
    
    
}
