using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverOutline : MonoBehaviour
{
    private Transform currentSelection;
    void Start()
    {
        
    }
    
    void Update()
    {
        if (currentSelection != null)
        {
            currentSelection.GetComponentInParent<Outline>().enabled = false;
            currentSelection = null;
        }
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Transform selection = hit.transform;
            if (selection.GetComponentInParent<Outline>() != null && hit.distance <= 1)
            {
                currentSelection = selection;
                selection.GetComponentInParent<Outline>().enabled = true;
            }
        }
    }
}
