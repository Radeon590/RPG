using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsTest : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.T))
        animator.Play("anim");
    }
}
