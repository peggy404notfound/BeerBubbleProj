using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foam : MonoBehaviour
{
    public CircleCollider2D coll; // Բ����ײ��
    public SpriteRenderer sp;     // ������Ⱦ��
    public bool foam;             // ��ǰ��ĭ״̬
    public bool pour;

    private Coroutine destroyCoroutine; // ���ڴ洢Э������

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
            sp.color = Color.yellow; // ��ײʱ��ʾ��ɫ
            pour = true;
            sp.sortingOrder = 14;

            // �����ʱЭ�������У�ֹͣ��
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
            sp.color = Color.white; // �뿪��ײʱ�ָ�Ϊ��ɫ

            // �������Ƿ��ǻ״̬
            if (gameObject.activeSelf && destroyCoroutine == null)
            {
                destroyCoroutine = StartCoroutine(DestroyAfterDelay(30f));
            }
        }
    }


    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // ȷ���ڼ�ʱ�ڼ�״̬û�иı�
        if (foam)
        {
            Destroy(gameObject);
        }
    }
}

