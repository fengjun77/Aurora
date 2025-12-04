using UnityEngine;

[System.Serializable]
public class ParallaxLayer
{
    [SerializeField]
    private Transform background;
    [SerializeField]
    private float parallaxMultiplier;

    private float imageFullWidth;
    private float imageHalfWidth;
    private float offsetX = 10f; //为了避免图片边缘露出空白，增加一个偏移量


    public void CalculateImageWidth()
    {
        imageFullWidth = background.GetComponent<SpriteRenderer>().bounds.size.x;
        imageHalfWidth = imageFullWidth / 2;
    }

    public void Move(float distance)
    {
        background.position += Vector3.right * (distance * parallaxMultiplier);
    }

    public void LoopBackground(float cameraLeftEdge, float cameraRightEdge)
    {
        float imageLeftEdge = (background.position.x - imageHalfWidth) + offsetX;
        float imageRightEdge = (background.position.x + imageHalfWidth) - offsetX;

        //如果相机的左边缘已经超过图片的右边缘，说明图片完全离开了相机视野的左侧，需要将图片移动到右侧
        if(cameraLeftEdge > imageRightEdge)
            background.position += Vector3.right * imageFullWidth;
        //如果相机的右边缘已经超过图片的左边缘，说明图片完全离开了相机视野的右侧，需要将图片移动到左侧
        else if(cameraRightEdge < imageLeftEdge)
            background.position += Vector3.left * imageFullWidth;
    }
}
