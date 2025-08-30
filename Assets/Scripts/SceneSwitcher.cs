using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] bool next_level = false;
    [SerializeField] Button retry_button;
    [SerializeField] Button next_level_button;
    [SerializeField] CanvasGroup game_clear_bg;
    [SerializeField] float fade_in_speed = 0.05f;
    public bool required_fade_in = false;

    void Update() {
        next_level_button.gameObject.SetActive(next_level);

        if (required_fade_in) {
            if (game_clear_bg.alpha < 0.99) {
                game_clear_bg.alpha += fade_in_speed * Time.deltaTime;
            } else {
                required_fade_in = false;
            }
        }
    }

    void GameClearBgFadeIn() {
        required_fade_in = true;
    }

    void Retry() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void NextLevel() {
        if (!next_level) return;

        int next = SceneManager.GetActiveScene().buildIndex + 1;
        if (next > SceneManager.sceneCount) return;

        SceneManager.LoadScene(next);
    }
}
