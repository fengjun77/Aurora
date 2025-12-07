using UnityEngine;

public class Enemy_VFX : Entity_VFX
{
    [SerializeField] private GameObject attackWindow;
    

    public void EnableAttackWindow(bool enable) => attackWindow.SetActive(enable);
}
