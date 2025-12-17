using UnityEngine.UI;
using UnityEngine;

public class UI_TreeConnection : MonoBehaviour
{
    [SerializeField] private RectTransform rotationPoint; // 连接点旋转调整
    [SerializeField] private RectTransform connectionLength; // 连接线长度调整
    [SerializeField] private RectTransform childNodeConnectionPoint; // 预留给子节点连接点使用

    /// <summary>
    /// 连接方向与长度设置
    /// </summary>
    /// <param name="direction">连接方向</param>
    /// <param name="length">连接长度</param>
    public void DirectConnection(NodeDirectionType direction, float length)
    {
        bool shouldBeActive = direction != NodeDirectionType.None;
        float finalLength = shouldBeActive ? length : 0f;
        float angle = GetDirectionAngle(direction);

        rotationPoint.localRotation = Quaternion.Euler(0f, 0f, angle);
        connectionLength.sizeDelta = new Vector2(finalLength, connectionLength.sizeDelta.y);
    }

    public Image GetConnectionImage() => connectionLength.GetComponent<Image>();

    public Vector2 GetConnectionPoint(RectTransform rect)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle
        (
            rect.parent as RectTransform,
            childNodeConnectionPoint.position,
            null,
            out var localPosition
        );

        return localPosition;
    }

    private float GetDirectionAngle(NodeDirectionType type)
    {
        switch (type)
        {
            case NodeDirectionType.UpLeft:
                return 135f;
            case NodeDirectionType.Up:
                return 90f;
            case NodeDirectionType.UpRight:
                return 45f;
            case NodeDirectionType.Left:
                return 180f;
            case NodeDirectionType.Right:
                return 0f;
            case NodeDirectionType.DownLeft:
                return 225f;
            case NodeDirectionType.Down:
                return 270f;
            case NodeDirectionType.DownRight:
                return 315f;
            default:
                return 0f;
        }
    }
}

public enum NodeDirectionType
{
    None,
    UpLeft,
    Up,
    UpRight,
    Left,
    Right,
    DownLeft,
    Down,
    DownRight
}
