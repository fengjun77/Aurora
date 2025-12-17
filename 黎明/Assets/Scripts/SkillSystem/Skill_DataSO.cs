using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Skill Data", fileName = "Skill Data - ")]
public class Skill_DataSO : ScriptableObject
{
    public int cost;

    [Header("技能信息")]
    public string skillName;
    [TextArea]
    public string description;
    public Sprite icon;
}
