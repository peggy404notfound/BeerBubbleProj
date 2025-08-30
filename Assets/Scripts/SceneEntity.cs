using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu()]
public class SceneEntity : ScriptableObject {
    public int max_scenes = 3;
    public int num_scene = 1;
    public int current_scene = 1;
}
