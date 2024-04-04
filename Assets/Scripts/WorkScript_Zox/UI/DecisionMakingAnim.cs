using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionMakingAnim : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private void OnMouseEnter()
    {   
        anim.SetTrigger("ButtonEnter");
    }
    private void OnMouseExit()
    {
        anim.SetBool("MouseStay", false);
    }

    
}
