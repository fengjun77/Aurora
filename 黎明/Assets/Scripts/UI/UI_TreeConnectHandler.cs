using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UI_TreeConnectionDetail
{
    [Header("子节点")]public UI_TreeConnectHandler childNode;
    [Header("方向")]public NodeDirectionType direction;
    [Header("长度")][Range(100f, 350f)]public float length;

}

public class UI_TreeConnectHandler : MonoBehaviour
{
    private RectTransform myRect => GetComponent<RectTransform>();

    [Header("连接详情")]
    [SerializeField] private UI_TreeConnectionDetail[] details;

    [Header("连接组件")]
    [SerializeField] private UI_TreeConnection[] connections;

    private Image connectionImage;
    private Color originalColor;

    void Awake()
    {
        if(connectionImage != null)
            originalColor = connectionImage.color;
    }

    void OnValidate()
    {
        if(details.Length <= 0)
            return;

        UpdateAllConnections();
    }

    private void UpdateConnection()
    {
        for(int i = 0; i < details.Length; i++)
        {
            Vector2 targetPosition = connections[i].GetConnectionPoint(myRect);
            Image connectionImage = connections[i].GetConnectionImage();

            connections[i].DirectConnection(details[i].direction, details[i].length);

            if(details[i].childNode == null)
                continue;

            details[i].childNode.SetPosition(targetPosition);
            details[i].childNode.SetConnectionImage(connectionImage);
        }
    }

    public void UpdateAllConnections()
    {
        UpdateConnection();

        foreach(var node in details)
        {
            if(node.childNode == null)
                continue;
            
            node.childNode.UpdateConnection();
        }
    }

    /// <summary>
    /// 解锁连接图片颜色
    /// </summary>
    /// <param name="unlocked"></param>
    public void UnlockConnectionImage(bool unlocked)
    {
        if(connectionImage == null)
            return;

        connectionImage.color = unlocked ? Color.white : originalColor;
    }

    public void SetConnectionImage(Image image) => connectionImage = image;

    public void SetPosition(Vector2 position)
    {
        myRect.anchoredPosition = position;
    }
}
