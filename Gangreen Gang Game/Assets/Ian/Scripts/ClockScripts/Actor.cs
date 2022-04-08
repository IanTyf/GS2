using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// takes care of playing the actual animation
public class Actor : MonoBehaviour
{
    [HideInInspector]
    public Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAnim(string state, float normalizedTimeScale)
    {
        anim.speed = 0;
        //Debug.Log("playing anim, norm: " + normalizedTimeScale);
        anim.Play(state, -1, normalizedTimeScale);
        //anim.runtimeAnimatorController.animationClips[0].events[0].
    }

    public void SetAnimToFrame(string state, float normalizedTimeScale)
    {
        anim.speed = 0;
        //Debug.Log("playing anim, norm: "+normalizedTimeScale);
        anim.Play(state, -1, normalizedTimeScale);
    }
}
