using CarterGames.Assets.AudioManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pouring : MonoBehaviour
{
    public static bool allowedToPour = true;

    [SerializeField] TutorialHints hints;
    bool alreadyShowHint = false;

    public Image bearCan; // 酒瓶sprite
    public RectTransform idleRectTransform;
    public RectTransform startPouringRectTransform;

    public Transform bottle; // 酒瓶对象
    public float minAngle = 60f;   // 最小显示角
    public float maxAngle = 130f;  // 最大显示角
    public float maxHoldTime = 4f; // 保留原字段
    public float liquidRiseRate = 0.5f;

    private float holdTime = 0f;
    public bool isPouring = false;

    public BoxCollider2D boxCollider;
    private float originalHeight;

    [Header("Pouring Settings")]
    public GameObject liquidPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 0.5f;     // 实时被映射覆盖
    public float baseSpawnInterval = 0.5f; // 慢
    public float maxSpawnInterval = 0.05f; // 快
    private Coroutine pouringCoroutine;
    public float maxHeight;
    public bool ifMax;

    // ======= 仅新增/替换：输入与阈值（陀螺优先 + 滞回） =======
    [Header("Tilt Control (Gyro Preferred)")]
    [Tooltip("竖直握持的默认Y值基线，竖直常≈-1")]
    public float neutralY = -1f;

    [Tooltip("开始倒酒的阈值（高一些）")]
    public float startThreshold = 0.12f;

    [Tooltip("停止倒酒的阈值（低一些，形成滞回）")]
    public float stopThreshold = 0.08f;

    [Tooltip("把(startThreshold ~ startThreshold+tiltRange)映射为0~1")]
    public float tiltRange = 0.6f;

    [Tooltip("方向反了就勾上")]
    public bool invertSign = false;

    [Tooltip("输入平滑")]
    public float smooth = 8f;

    private Vector3 gFiltered;

    void Start()
    {
        allowedToPour = true;

        // 开启陀螺仪（若设备支持），优先用融合重力
        Input.gyro.enabled = true;
        gFiltered = SystemInfo.supportsGyroscope ? Input.gyro.gravity : Input.acceleration;
    }

    void Update()
    {
        if (!allowedToPour) return;

        // 读取并平滑重力向量（陀螺优先，回退加速度）
        Vector3 gRaw = SystemInfo.supportsGyroscope ? Input.gyro.gravity : Input.acceleration;
        gFiltered = Vector3.Lerp(gFiltered, gRaw, Time.deltaTime * smooth);

        // 往下倾时，g.y 会从 -1 向 0 增大
        float forwardDelta = (gFiltered.y - neutralY) * (invertSign ? -1f : 1f);

        // —— 滞回：未在倒用“开始阈值”，已在倒用“停止阈值” —— //
        bool shouldPour = isPouring ? (forwardDelta > stopThreshold)
                                    : (forwardDelta > startThreshold);

        // ===== 状态切换：保持你的原结构 =====
        if (shouldPour && !isPouring)
        {
            setBearCanRectTransform(startPouringRectTransform, bearCan.rectTransform);
            if (!alreadyShowHint)
            {
                if (hints != null) hints.displayNextHint();
                alreadyShowHint = true;
            }

            isPouring = true;
            holdTime = 0f;
            GetComponent<InspectorAudioClipPlayer>().Play();
            pouringCoroutine = StartCoroutine(SpawnLiquidCoroutine());
        }

        if (!shouldPour && isPouring)
        {
            isPouring = false;
            holdTime = 0f;
            spawnInterval = baseSpawnInterval;
            GetComponent<InspectorAudioClipPlayer>().Stop();

            if (pouringCoroutine != null)
            {
                StopCoroutine(pouringCoroutine);
                pouringCoroutine = null;
            }

            setBearCanRectTransform(idleRectTransform, bearCan.rectTransform);
        }

        // ===== 倒酒中：把“有效倾斜”映射到角度/流速（保留你的映射） =====
        if (isPouring)
        {
            float effective = Mathf.Clamp01(
                (forwardDelta - startThreshold) / Mathf.Max(tiltRange, 0.0001f)
            );

            float angle = Mathf.Lerp(minAngle, maxAngle, effective);
            bottle.rotation = Quaternion.Euler(0f, 0f, angle);
            bearCan.rectTransform.rotation = Quaternion.Euler(0f, 0f, angle);

            // 倾斜越大 -> 越接近 maxSpawnInterval（更快）
            spawnInterval = Mathf.Lerp(baseSpawnInterval, maxSpawnInterval, effective);

            // ===== 以下为你原来的“液面/碰撞体上升”逻辑，未改 =====
            holdTime += Time.deltaTime;
            if (!ifMax)
            {
                float pourSpeed = liquidRiseRate / (1f + holdTime * 10f);

                if (boxCollider != null)
                {
                    Vector2 currentSize = boxCollider.size;
                    currentSize.y += pourSpeed * Time.deltaTime;
                    boxCollider.size = currentSize;

                    Vector2 currentOffset = boxCollider.offset;
                    boxCollider.offset = new Vector2(currentOffset.x, currentOffset.y + (pourSpeed * Time.deltaTime) / 2);

                    if (currentOffset.y + (pourSpeed * Time.deltaTime) / 2 >= maxHeight)
                    {
                        boxCollider.offset = new Vector2(currentOffset.x, maxHeight);
                        ifMax = true;
                    }
                }
            }
        }
    }

    IEnumerator SpawnLiquidCoroutine()
    {
        while (isPouring)
        {
            Instantiate(liquidPrefab, spawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void RefreshScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void setBearCanRectTransform(RectTransform src, RectTransform dest)
    {
        dest.position = src.position;
        dest.pivot = src.pivot;
        dest.rotation = src.rotation;
    }
}
