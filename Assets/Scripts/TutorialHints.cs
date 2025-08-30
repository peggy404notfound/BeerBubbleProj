using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TutorialHints : MonoBehaviour
{
    public GameObject[] hints;
    [SerializeField] int current_hint = 1;

    public void displayNextHint() {
        if (current_hint >= hints.Length) return;
        
        if (current_hint - 1 >= 0) {
            hints[current_hint - 1].SetActive(false);
        }

        hints[current_hint].SetActive(true);
        ++current_hint;
    }
}
