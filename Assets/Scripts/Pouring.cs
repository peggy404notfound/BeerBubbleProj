using CarterGames.Assets.AudioManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pouring : MonoBehaviour {
    public static bool allowedToPour = true;

    [SerializeField] TutorialHints hints;
    bool alreadyShowHint = false;

    public Image bearCan; // 酒瓶sprite
    // public RectTransform bottleRectTransform; // 酒瓶sprite
    public RectTransform idleRectTransform;
    public RectTransform startPouringRectTransform;

    public Transform bottle; // 酒瓶对象
    public float minAngle = 60f; // 最小倾斜角度
    public float maxAngle = 130f; // 最大倾斜角度
    public float maxHoldTime = 4f; // 按住时间到达最大角度的时长
    public float liquidRiseRate = 0.5f; // 液面上升初始速率系数

    private float holdTime = 0f; // 按住时间
    public bool isPouring = false; // 是否在倒酒

    public BoxCollider2D boxCollider;
    private float originalHeight;

    [Header("Pouring Settings")]
    public GameObject liquidPrefab; // 酒液预制体
    public Transform spawnPoint;   // 生成位置
    public float spawnInterval = 0.5f; // 生成间隔
    public float baseSpawnInterval = 0.5f;
    public float maxSpawnInterval = 0.05f;
    private Coroutine pouringCoroutine;
    public float maxHeight;
    public bool ifMax;
    void Start() {
        allowedToPour = true;
    }

    void Update() {
        if (!allowedToPour) return;

        // 检测空格按下
        // if (Input.GetMouseButtonDown(0))
        if (Input.GetKeyDown(KeyCode.Space)) {
            setBearCanRectTransform(startPouringRectTransform, bearCan.rectTransform);
            if (!alreadyShowHint) {
                if (hints != null) hints.displayNextHint();
                alreadyShowHint = true;
            }

            isPouring = true;
            holdTime = 0f;
            GetComponent<InspectorAudioClipPlayer>().Play();
            pouringCoroutine = StartCoroutine(SpawnLiquidCoroutine());
        }

        // 检测空格松开
        if (Input.GetKeyUp(KeyCode.Space)) {
            isPouring = false;
            holdTime = 0f;
            spawnInterval = baseSpawnInterval;
            GetComponent<InspectorAudioClipPlayer>().Stop();

            if (pouringCoroutine != null) {
                StopCoroutine(pouringCoroutine);
                pouringCoroutine = null;
            }

            setBearCanRectTransform(idleRectTransform, bearCan.rectTransform);
        }
        // 计算倾斜角度
        if (isPouring) {
            holdTime += Time.deltaTime;
            float t = Mathf.Clamp01(holdTime / maxHoldTime);
            float angle = Mathf.Lerp(minAngle, maxAngle, t);
            bottle.rotation = Quaternion.Euler(0f, 0f, angle);

            bearCan.rectTransform.rotation = Quaternion.Euler(0f, 0f, angle);
            spawnInterval -= 0.1f * Time.deltaTime;
            if (spawnInterval <= maxSpawnInterval)
                spawnInterval = maxSpawnInterval;

            if (!ifMax) {
                float pourSpeed = liquidRiseRate / (1f + holdTime * 10f);

                if (boxCollider != null) {
                    // 获取当前的大小
                    Vector2 currentSize = boxCollider.size;
                    // 增加高度
                    currentSize.y += pourSpeed * Time.deltaTime;

                    // 更新碰撞体的大小
                    boxCollider.size = currentSize;

                    // 保证下边缘不动，只调整上边缘
                    Vector2 currentOffset = boxCollider.offset;
                    boxCollider.offset = new Vector2(currentOffset.x, currentOffset.y + (pourSpeed * Time.deltaTime) / 2);
                    if (currentOffset.y + (pourSpeed * Time.deltaTime) / 2 >= maxHeight) {
                        boxCollider.offset = new Vector2(currentOffset.x, maxHeight);
                        ifMax = true;
                    }
                }
            }

        }
    }

    IEnumerator SpawnLiquidCoroutine() {
        while (isPouring) {
            Instantiate(liquidPrefab, spawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void RefreshScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void setBearCanRectTransform(RectTransform src, RectTransform dest) {
        dest.position = src.position;
        dest.pivot = src.pivot;
        dest.rotation = src.rotation;
    }
}