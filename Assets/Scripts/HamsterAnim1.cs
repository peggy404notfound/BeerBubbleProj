using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterAnim1 : MonoBehaviour
{
    public Animator anim;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("up", true);
        }
    }



}
