using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer player;
    public GameObject videoCanvas;

    void Awake()
    {
        player.loopPointReached += CloseCanvas;
    }

    void CloseCanvas(VideoPlayer player)
    {
        videoCanvas.SetActive(false);
    }
}
