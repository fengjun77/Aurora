using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    public int skillPoints;

    public bool EnoughSkillPoints(int cost) => skillPoints >= cost;

    public void RemoveSkillPoints(int cost) => skillPoints -= cost;

    public void AddSkillPoints(int amount) => skillPoints += amount;

    [ContextMenu("Reset All Skills")]
    public void RefundAllSkills()
    {
        UI_TreeNode[] skillNodes = GetComponentsInChildren<UI_TreeNode>();

        foreach (UI_TreeNode node in skillNodes)
        {
            node.Refund();
        }
    }
}
