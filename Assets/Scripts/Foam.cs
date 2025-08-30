using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foam : MonoBehaviour
{
    public CircleCollider2D coll; // 圆形碰撞体
    public SpriteRenderer sp;     // 精灵渲染器
    public bool foam;             // 当前泡沫状态
    public bool pour;

    private Coroutine destroyCoroutine; // 用于存储协程引用

    private void Start()
    {
        foam = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Foam")
        {
            foam = false;
            coll.isTrigger = false;
            sp.color = Color.yellow; // 碰撞时显示黄色
            pour = true;
            sp.sortingOrder = 14;

            // 如果计时协程在运行，停止它
            if (destroyCoroutine != null)
            {
                StopCoroutine(destroyCoroutine);
                destroyCoroutine = null;
            }
        }

        if (collision.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Foam")
        {
            foam = true;
            sp.color = Color.white; // 离开碰撞时恢复为白色

            // 检查对象是否是活动状态
            if (gameObject.activeSelf && destroyCoroutine == null)
            {
                destroyCoroutine = StartCoroutine(DestroyAfterDelay(30f));
            }
        }
    }


    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 确保在计时期间状态没有改变
        if (foam)
        {
            Destroy(gameObject);
        }
    }
}

