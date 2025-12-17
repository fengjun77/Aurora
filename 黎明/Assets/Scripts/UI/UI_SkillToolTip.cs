using System.Runtime.InteropServices;
using System.Text;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    private UI ui;
    private UI_SkillTree skillTree;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillRequirements;

    [Space]
    [SerializeField] private string meConditionHex;
    [SerializeField] private string notMeConditionHex;
    [SerializeField] private string conflictNodesHex;
    [SerializeField] private Color exampleColor;

    protected override void Awake()
    {
        base.Awake();

        ui = GetComponentInParent<UI>();
        skillTree = ui.GetComponentInChildren<UI_SkillTree>(true);
    }

    public override void ShowToolTip(bool show, RectTransform targetRect)
    {
        base.ShowToolTip(show, targetRect);
    }

    public void ShowToolTip(bool show, RectTransform targetRect, UI_TreeNode node)
    {
        base.ShowToolTip(show, targetRect);

        if(!show)
            return;

        skillName.text = node.skillData.skillName;
        skillDescription.text = node.skillData.description;
        if(node.isLocked)
            skillRequirements.text = "该技能已被锁定，无法解锁。";
        else
            skillRequirements.text = node.isUnlocked ? "" : GetRequirements(node.skillData.cost, node.neededNodes, node.conflictNodes);
    }

    private string GetRequirements(int skillCost, UI_TreeNode[] neededNodes, UI_TreeNode[] conflictNodes)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("解锁需求：");

        string costColor = skillTree.EnoughSkillPoints(skillCost) ? meConditionHex : notMeConditionHex;

        sb.AppendLine($" - <color=#{costColor}>需要 {skillCost} 点技能点</color>");

        foreach(var node in neededNodes)
        {
            string nodeColor = node.isUnlocked ? meConditionHex : notMeConditionHex;
            sb.AppendLine($" - <color=#{nodeColor}>需要解锁 {node.skillData.skillName}</color>");
        }

        if(conflictNodes.Length > 0)
        {
            foreach(var node in conflictNodes)
            {
                string nodeColor = conflictNodesHex;
                sb.AppendLine($" - <color=#{nodeColor}>不能与 {node.skillData.skillName} 同时解锁</color>");
            }
        }

        return sb.ToString();
    }
}
