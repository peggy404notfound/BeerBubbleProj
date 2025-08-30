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

    public List<string> animalCustomers; // ����˿͵��б�
    public TextMeshProUGUI tmpText; // TextMeshPro ������ʾ�ı�
    public TextMeshProUGUI feedbackText; // ������ʾ������Ϣ���ı�

    private float randomPercentage; // ������ɵı���
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
        // ��ʼ�������б�������Inspector�б༭���ڴ�����ֱ�Ӹ�ֵ��
        animalCustomers = new List<string> { "����", "����¹", "����" };

        if (canvasGroup != null) {
            canvasGroup.alpha = 0f;
        }

        /*sceneEntity.max_scenes = SceneManager.sceneCount;
        sceneEntity.num_scene = 1;
        sceneEntity.current_scene = 1;*/

        // ���ѡ��˿Ͳ���ʾ���
        DisplayRandomCustomer();
    }

    void DisplayRandomCustomer()
    {
        // ���б������ѡ��һ���˿�
        string randomCustomer = animalCustomers[Random.Range(0, animalCustomers.Count)];

        int random_index = Random.Range(0, 3);
        randomPercentage = percentages[random_index];
        Debug.Log("random percentage: " + randomPercentage);
        if (target_image != null) {
            target_image.sprite = sprites[random_index];
        }

        // randomPercentage = Random.Range(0f, 60f);

        // �������ʾ�� tmpText ��
        tmpText.text = $"{randomCustomer}: {randomPercentage:F0}%";

        // ���֮ǰ�ķ����ı�
        feedbackText.text = "";
    }

    public void CheckRate(float rate)
    {
        // ����Ŀ�귶Χ
        float lowerBound = randomPercentage - 5f;
        float upperBound = randomPercentage + 5f;
        print(rate);

        
        // ��������Ƿ��ڷ�Χ��
        if (rate >= lowerBound && rate <= upperBound)
        {
            if (sceneEntity != null) {
                
            }
            feedbackText.text = "������!";
            sceneEntity.num_scene = Mathf.Min(sceneEntity.max_scenes, sceneEntity.num_scene + 1);
            if (gameClearBg != null) {
                gameClearBg.sprite = success;
            }
        }
        else
        {
            feedbackText.text = "����һ��!";
            if (gameClearBg != null) {
                gameClearBg.sprite = failure;
            }
        }

        Pouring.allowedToPour = false;
        if (hints != null) hints.displayNextHint();
        game_clear = true;
    }
}
