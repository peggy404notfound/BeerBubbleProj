using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using CarterGames.Assets.AudioManager;

public class RandomCustomer : MonoBehaviour
{
    [SerializeField] TutorialHints hints;

    public List<string> animalCustomers; // 动物顾客的列表
    public TextMeshProUGUI tmpText; // TextMeshPro 用于显示文本
    public TextMeshProUGUI feedbackText; // 用于显示反馈信息的文本

    private float randomPercentage; // 随机生成的比例
    private float[] percentages = { 5f, 20f, 40f, 60f };
    [SerializeField] Sprite[] sprites;
    [SerializeField] Image target_image;
    [SerializeField] SceneEntity sceneEntity;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Image gameClearBg;
    [SerializeField] Sprite success;
    [SerializeField] Sprite failure;
    bool game_clear = false;
    float speed = 1f;

    void Update()
    {
        if (game_clear) {
            if (canvasGroup.alpha < 0.99f) {
                canvasGroup.alpha += speed * Time.deltaTime;
            } else {
                game_clear = false;
            }
        }
    }
    void Start()
    {
        // 初始化动物列表（可以在Inspector中编辑或在代码中直接赋值）
        animalCustomers = new List<string> { "鹈鹕", "长颈鹿", "仓鼠" };

        if (canvasGroup != null) {
            canvasGroup.alpha = 0f;
        }

        /*sceneEntity.max_scenes = SceneManager.sceneCount;
        sceneEntity.num_scene = 1;
        sceneEntity.current_scene = 1;*/

        // 随机选择顾客并显示结果
        DisplayRandomCustomer();
    }

    void DisplayRandomCustomer()
    {
        // 从列表中随机选择一个顾客
        string randomCustomer = animalCustomers[Random.Range(0, animalCustomers.Count)];

        int random_index = Random.Range(0, 3);
        randomPercentage = percentages[random_index];
        Debug.Log("random percentage: " + randomPercentage);
        if (target_image != null) {
            target_image.sprite = sprites[random_index];
        }

        // randomPercentage = Random.Range(0f, 60f);

        // 将结果显示在 tmpText 上
        tmpText.text = $"{randomCustomer}: {randomPercentage:F0}%";

        // 清除之前的反馈文本
        feedbackText.text = "";
    }

    public void CheckRate(float rate)
    {
        // 计算目标范围
        float lowerBound = randomPercentage - 5f;
        float upperBound = randomPercentage + 5f;
        print(rate);

        
        // 检查输入是否在范围内
        if (rate >= lowerBound && rate <= upperBound)
        {
            if (sceneEntity != null) {
                
            }
            feedbackText.text = "好样的!";
            sceneEntity.num_scene = Mathf.Min(sceneEntity.max_scenes, sceneEntity.num_scene + 1);
            if (gameClearBg != null) {
                gameClearBg.sprite = success;
            }
        }
        else
        {
            feedbackText.text = "再试一次!";
            if (gameClearBg != null) {
                gameClearBg.sprite = failure;
            }
        }

        Pouring.allowedToPour = false;
        if (hints != null) hints.displayNextHint();
        game_clear = true;
    }
}
