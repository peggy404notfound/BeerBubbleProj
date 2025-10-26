using UnityEngine;

public class AnyKeyCloseUI : MonoBehaviour
{
    [Header("要关闭的 UI 根节点")]
    public GameObject targetUI;

    [Header("是否在关闭后销毁该对象")]
    public bool destroyAfterClose = false;

    private bool isClosed = false;

    void Update()
    {
        if (isClosed) return;

        // 检测任意键或鼠标点击或触屏
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            CloseUI();
        }
    }

    private void CloseUI()
    {
        if (!targetUI) targetUI = gameObject;

        if (destroyAfterClose)
        {
            Destroy(targetUI);
        }
        else
        {
            targetUI.SetActive(false);
        }

        isClosed = true;
    }
}