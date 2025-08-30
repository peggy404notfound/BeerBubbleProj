using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    public void on_click() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
