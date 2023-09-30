using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverOutline : MonoBehaviour
{
    private Transform currentSelection;
    private GameManager gameManager;
    
    void Start()
    {
        gameManager = GetComponent<GameManager>();
    }
    
    void Update()
    {
        if (currentSelection != null)
        {
            currentSelection.GetComponentInParent<Outline>().enabled = false;
            gameManager.isSelecting = false;
            currentSelection = null;
        }
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && !gameManager.isDialogPlaying)
        {
            Transform selection = hit.transform;
            if (selection.GetComponentInParent<Outline>() != null && hit.distance <= 1)
            {
                currentSelection = selection;
                gameManager.isSelecting = true;
                selection.GetComponentInParent<Outline>().enabled = true;
            }
        }
    }
}
