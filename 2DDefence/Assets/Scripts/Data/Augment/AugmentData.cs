using UnityEngine;

[CreateAssetMenu(fileName = "AugmentCard", menuName = "Game/Augment")]
public class AugmentData : ScriptableObject
{
    public int augmentId;                 // 증강 고유번호 
    public string augmentName;           // 증강 이름

    [TextArea(3, 5)] // 최소 3줄, 최대 5줄
    public string augmentDesc;

    public Sprite augmentIcon;           // 증강 아이콘
    public AugmentType augmentType; // 증강 효과 유형


    public enum AugmentType
    {  
        SpecialAbility,    
    }
}
