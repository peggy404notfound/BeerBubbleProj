using UnityEngine;

public class CheckFull : MonoBehaviour
{
    private float stayTime = 0f;
    [SerializeField] float needStay = 0.2f;   // 需要持续接触的时间

    private void OnTriggerStay2D(Collider2D collision)
    {
        var foam = collision.GetComponent<Foam>();
        if (foam != null && foam.pour)        // 只认真正的泡沫
        {
            stayTime += Time.deltaTime;
            if (stayTime >= needStay)
                FoamController.instance.countDown = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        stayTime = 0f;                        // 一离开就重置，防抖
    }
}