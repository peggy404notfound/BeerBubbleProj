using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterAnim : MonoBehaviour
{
    public GameObject hamster;
    public void AnimOnStop()
    {
        gameObject.SetActive(false);
        hamster.SetActive(true);
    }
}
