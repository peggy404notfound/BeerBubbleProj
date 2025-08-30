using CarterGames.Assets.AudioManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Animator animator;
    [SerializeField] InspectorAudioClipPlayer player;

    private const string clicked = "clicked";

    void Awake()
    {
        animator = image.GetComponent<Animator>();
    }

    public void OnClicked()
    {
        player.Play();
        animator.SetBool(clicked, true);
    }
}
