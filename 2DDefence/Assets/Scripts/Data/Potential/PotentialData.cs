using UnityEngine;

[CreateAssetMenu(fileName = "Potential", menuName = "Game/Potential")]
public class PotentialData : ScriptableObject
{
    public int potentialId;             // 잠재능력 고유번호 
    public int valudId;                 // 수치 고유번호 

    public string potential;            // 잠재능력
    public string potentialText;        // UI에 표시될 내용
    public float value;                 // 수치
}
