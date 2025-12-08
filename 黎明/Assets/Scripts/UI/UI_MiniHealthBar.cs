using UnityEngine;

public class UI_MiniHealthBar : MonoBehaviour
{
    private Entity entity;

    void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    void OnEnable()
    {
        entity.OnFilpped += HandleFilp;
    }

    void OnDisable()
    {
        entity.OnFilpped -= HandleFilp;
    }

    private void HandleFilp()
    {
        transform.rotation = Quaternion.identity;
    }
}
