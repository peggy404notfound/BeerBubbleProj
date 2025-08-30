using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterCheckCheek : MonoBehaviour
{
    public GameObject hamster1;
    public GameObject hameter2;
    public bool isTouch;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.GetComponent<Foam>().pour == true)
        {
            if (!isTouch)
            {
                hamster1.SetActive(false);
                hameter2.SetActive(true);
                isTouch = true;
            }
          
        }
    }
}
