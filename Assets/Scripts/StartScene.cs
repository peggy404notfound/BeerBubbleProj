using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    [SerializeField] SceneEntity scene_entity;

    public void on_click() {
        SceneManager.LoadScene(scene_entity.current_scene);
    }
}
