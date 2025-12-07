using System.Runtime.InteropServices;
using UnityEngine;

public class VFX_AutoController : MonoBehaviour
{
    [SerializeField] private bool canDestory = true;
    [SerializeField] private float destoryDelay = 1f;
    [SerializeField] private bool canRandomOffset = true;

    [Header("随机位置")]
    [SerializeField] private float xMinOffset = -.3f;
    [SerializeField] private float xMaxOffset = .3f;
    [Space]
    [SerializeField] private float yMinOffset = -.3f;
    [SerializeField] private float yMaxOffset = .3f;

    void Start()
    {
        if(canRandomOffset)
            ApplyRandomOffset();

        if(canDestory)
            Destroy(gameObject, destoryDelay);
    }

    private void ApplyRandomOffset()
    {
        float xOffset = Random.Range(xMinOffset, xMaxOffset);
        float yOffset = Random.Range(yMinOffset, yMaxOffset);

        transform.position += new Vector3(xOffset,yOffset);
    }
}
