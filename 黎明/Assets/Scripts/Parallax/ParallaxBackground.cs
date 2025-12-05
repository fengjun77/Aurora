using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Camera mainCamera;
    private float lastCameraPostionX;

    private float cameraHalfWidth;

    [SerializeField]
    private ParallaxLayer[] parallaxLayers;

    void Awake()
    {
        mainCamera = Camera.main;
        cameraHalfWidth = mainCamera.orthographicSize  * mainCamera.aspect;
        InitLayers();
    }

    void FixedUpdate()
    {
        float currentCameraPositionX = mainCamera.transform.position.x;
        //需要移动的距离
        float distanceToMove = currentCameraPositionX - lastCameraPostionX;
        //记录移动到的距离
        lastCameraPostionX = currentCameraPositionX;

        float cameraLeftEdge = currentCameraPositionX - cameraHalfWidth;
        float cameraRightEdge = currentCameraPositionX + cameraHalfWidth;

        foreach (var layer in parallaxLayers)
        {
            layer.Move(distanceToMove);
            layer.LoopBackground(cameraLeftEdge, cameraRightEdge);
        }
    }

    /// <summary>
    /// 初始化图层信息
    /// </summary>
    private void InitLayers()
    {
        foreach (var layer in parallaxLayers)
        {
            layer.CalculateImageWidth();
        }
    }
}
