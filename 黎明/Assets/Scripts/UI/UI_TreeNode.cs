using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI ui;
    private RectTransform rect;
    private UI_SkillTree skillTree;
    private UI_TreeConnectHandler connectHandler;

    public UI_TreeNode[] neededNodes; //解锁此节点所需的节点
    public UI_TreeNode[] conflictNodes; //与此节点冲突的节点
    public bool isUnlocked; //是否已解锁
    public bool isLocked; //是否被锁定

    [Header("技能数据")]
    public Skill_DataSO skillData;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    private string lockedColorHex = "#929090ff";
    private Color lastColor;

    void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        skillTree = GetComponentInParent<UI_SkillTree>();
        connectHandler = GetComponent<UI_TreeConnectHandler>();

        UpdateIconColor(GetColorByHex(lockedColorHex));
    }

    public void Refund()
    {
        if(isUnlocked)
            skillTree.AddSkillPoints(skillData.cost);
        
        isUnlocked = false;
        isLocked = false;
        UpdateIconColor(GetColorByHex(lockedColorHex));

        connectHandler.UnlockConnectionImage(false);

        //在技能管理器中取消该技能的学习状态
    }

    void OnValidate()
    {
        if(skillData == null)
            return;

        skillName = skillData.skillName;
        skillIcon.sprite = skillData.icon;
        gameObject.name = "UI_TreeNode - " + skillName;
    }

    private void Unlock()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
        LockConflictNodes();

        skillTree.RemoveSkillPoints(skillData.cost);
        connectHandler.UnlockConnectionImage(true);
    }

    /// <summary>
    /// 检查此节点是否可以被解锁
    /// </summary>
    /// <returns></returns>
    private bool CanBeUnlocked()
    {
        if(isLocked || isUnlocked)
            return false;

        if(skillTree.EnoughSkillPoints(skillData.cost) == false)
            return false;

        //遍历所有前置节点，检查是否都已解锁
        foreach(var node in neededNodes)
        {
            if(!node.isUnlocked)
                return false;
        }

        //遍历所有冲突节点，检查是否有已解锁的节点
        foreach(var node in conflictNodes)
        {
            if(node.isUnlocked)
                return false;
        }

        return true;
    }

    private void UpdateIconColor(Color color)
    {
        if(skillIcon == null)
            return;

        lastColor = skillIcon.color;
        skillIcon.color = color;
    }

    private void LockConflictNodes()
    {
        foreach(var node in conflictNodes)
            node.isLocked = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(CanBeUnlocked())
            Unlock();
        else
            Debug.Log("技能无法解锁");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(true, rect, this);

        if(isUnlocked || isLocked)
            return;
        
        Color color = Color.white * .9f;
        color.a = 1f;
        UpdateIconColor(color);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(false, rect);

        if(isUnlocked || isLocked)
            return;
        
        UpdateIconColor(lastColor);
    }

    private Color GetColorByHex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color color);

        return color;
    }

    void OnDisable()
    {
        if(isLocked)
            UpdateIconColor(GetColorByHex(lockedColorHex));
        
        if(isUnlocked)
            UpdateIconColor(Color.white);
    }
}
