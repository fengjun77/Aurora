using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    private RectTransform rect;
    [SerializeField] private Vector2 offset = new Vector2(300, 20);

    protected virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    /// <summary>
    /// 显示或隐藏提示框
    /// </summary>
    /// <param name="show"></param>
    /// <param name="targetRect"></param>
    public virtual void ShowToolTip(bool show, RectTransform targetRect)
    {
        if(!show)
        {
            rect.position = new Vector2(9999,9999);
            return;
        }

        UpdatePosition(targetRect);
    }

    private void UpdatePosition(RectTransform targetRect)
    {
        float screenCenterX = Screen.width / 2f;
        float screenTop = Screen.height;
        float screenBottom = 0f;

        Vector2 targetPosition = targetRect.position;

        targetPosition.x = targetPosition.x > screenCenterX ? targetPosition.x - offset.x : targetPosition.x + offset.x;

        float toolTipHalf = rect.sizeDelta.y / 2f;
        //提示框的上边界
        float topY = targetPosition.y + toolTipHalf;
        //提示框的下边界
        float bottomY = targetPosition.y - toolTipHalf;

        //调整提示框位置，防止超出屏幕
        if(topY > screenTop)
            targetPosition.y = screenTop - toolTipHalf;
        else if(bottomY < screenBottom)
            targetPosition.y = screenBottom + toolTipHalf;

        rect.position = targetPosition;
    }

}
