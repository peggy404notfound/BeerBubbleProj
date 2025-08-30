using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimStop : MonoBehaviour
{
    public GameObject thisObject;

    public void OnStop()
    {
        thisObject.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
