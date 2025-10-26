using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using CarterGames.Assets.AudioManager; // 主BGM用的 Audio Clip Player

public class RandomCustomer : MonoBehaviour
{
    [SerializeField] TutorialHints hints;

    public List<string> animalCustomers;              // 动物顾客的列表
    public TextMeshProUGUI tmpText;                   // 显示顾客与目标百分比
    public TextMeshProUGUI feedbackText;             // 结果反馈文本

    private float randomPercentage;                   // 随机目标百分比
    private float[] percentages = { 5f, 20f, 40f, 60f };
    [SerializeField] Sprite[] sprites;
    [SerializeField] Image target_image;
    [SerializeField] SceneEntity sceneEntity;

    [Header("Result UI")]
    [SerializeField] CanvasGroup canvasGroup;         // 结算层淡入
    [SerializeField] Image gameClearBg;               // 成功/失败背景图
    [SerializeField] Sprite success;                  // 成功图
    [SerializeField] Sprite failure;                  // 失败图

    // ================== 新增：音频 & 锁定 sprite ==================
    [Header("BGM")]
    [Tooltip("场景里播放主BGM的 InspectorAudioClipPlayer（拖这个组件的对象）")]
    [SerializeField] InspectorAudioClipPlayer mainBgmPlayer;

    [Tooltip("成功结算BGM（拖 .wav/.mp3）")]
    [SerializeField] AudioClip bgmWin;

    [Tooltip("失败结算BGM（拖 .wav/.mp3）")]
    [SerializeField] AudioClip bgmLose;

    // 新增：可调命中容差
    [Header("判定参数")]
    [Tooltip("命中目标的容差（单位：百分比），例如 8 表示 ±8%")]
    [SerializeField] float passTolerance = 8f;

    // 用于播放胜/负结算BGM（一次性）
    private AudioSource resultBgmPlayer;

    // 锁定当前结果图，防止被其它地方改回默认
    private Sprite lockedResultSprite;

    // 防重复触发（只播一次）
    private bool resultHandled = false;
    // ============================================================

    bool game_clear = false;
    float speed = 1f;

    void Update()
    {
        if (game_clear)
        {
            if (canvasGroup != null && canvasGroup.alpha < 0.99f)
            {
                canvasGroup.alpha += speed * Time.deltaTime;
            }
            else
            {
                game_clear = false;
            }
        }

        // 持续锁定：若有脚本/动画把 sprite 改回默认，立刻改回结果图
        if (lockedResultSprite != null && gameClearBg != null && gameClearBg.sprite != lockedResultSprite)
            gameClearBg.sprite = lockedResultSprite;
    }

    void Start()
    {
        // 初始化动物列表（可在 Inspector 改）
        animalCustomers = new List<string> { "鹈鹕", "长颈鹿", "仓鼠" };

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        // 内部一次性BGM播放器
        resultBgmPlayer = gameObject.GetComponent<AudioSource>();
        if (resultBgmPlayer == null) resultBgmPlayer = gameObject.AddComponent<AudioSource>();
        resultBgmPlayer.playOnAwake = false;
        resultBgmPlayer.loop = false;   // 只播一次
        resultBgmPlayer.volume = 1f;

        DisplayRandomCustomer();
    }

    void DisplayRandomCustomer()
    {
        string randomCustomer = animalCustomers[Random.Range(0, animalCustomers.Count)];

        int random_index = Random.Range(0, 3);
        randomPercentage = percentages[random_index];
        Debug.Log("random percentage: " + randomPercentage);

        if (target_image != null)
            target_image.sprite = sprites[random_index];

        tmpText.text = $"{randomCustomer}: {randomPercentage:F0}%";
        feedbackText.text = "";
    }

    // 外部调用：把当前泡沫/比例传入
    public void CheckRate(float rate)
    {
        if (resultHandled) return; // 只处理一次

        // 改为使用可调容差参数
        float lowerBound = randomPercentage - passTolerance;
        float upperBound = randomPercentage + passTolerance;
        bool isWin = (rate >= lowerBound && rate <= upperBound);

        // 1) 暂停/停止主BGM
        if (mainBgmPlayer != null)
            mainBgmPlayer.Stop();

        // 2) 播放一次性结算BGM
        AudioClip clip = isWin ? bgmWin : bgmLose;
        if (clip != null)
            resultBgmPlayer.PlayOneShot(clip);

        // 3) 切图 & 锁定当前结果 sprite
        if (isWin)
        {
            feedbackText.text = "好样的!";
            if (sceneEntity != null)
                sceneEntity.num_scene = Mathf.Min(sceneEntity.max_scenes, sceneEntity.num_scene + 1);

            if (gameClearBg != null)
                gameClearBg.sprite = success;
        }
        else
        {
            feedbackText.text = "再试一次!";
            if (gameClearBg != null)
                gameClearBg.sprite = failure;
        }

        lockedResultSprite = gameClearBg != null ? gameClearBg.sprite : null;

        // ===== 原有流程保持不变 =====
        Pouring.allowedToPour = false;
        if (hints != null) hints.displayNextHint();
        game_clear = true;

        resultHandled = true;
    }
}