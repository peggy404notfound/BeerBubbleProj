using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFull : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision!=null && collision.GetComponent<Foam>().pour == true)
        {
            FoamController.instance.countDown = true;
        }
    }
}
