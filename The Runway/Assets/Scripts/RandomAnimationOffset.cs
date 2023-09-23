using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimationOffset : MonoBehaviour
{
    private Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false; 
        Invoke("EnableAnimator", Random.Range(0f,1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void EnableAnimator() 
    {
        animator.enabled = true;
    }
}
