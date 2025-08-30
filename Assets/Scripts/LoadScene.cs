using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {
    [SerializeField] int direction = 1; // true for positive
    [SerializeField] SceneEntity scene_entity;
    [SerializeField] TextMeshProUGUI text_mesh;

    public void Awake() {
        text_mesh.text = "Level " + scene_entity.current_scene;
    }

    public void on_click() {
        scene_entity.current_scene = Mathf.Clamp(scene_entity.current_scene + direction, 1, scene_entity.num_scene);
        text_mesh.text = "Level " + scene_entity.current_scene;
        Debug.Log("gonna load scene" + scene_entity.current_scene);
    }
}
