using UnityEngine;

// 공격 애니메이션 길이 따오는 로직
// 필요한 이유 : 공속에 비례해서 빨라지는 애니메이션을 구현하기 위함
public class AnimationLengthFetcher : MonoBehaviour
{
    public static AnimationLengthFetcher Instance;

    private Animator _animator;
    public float normalAttackLength = 1.0f;
    public float criticalAttackLength = 1.0f;
    public string normalClipName;
    public string criticalClipName;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        // 현재 재생 중인 애니메이션의 길이 가져오기
        normalAttackLength = GetAnimationLength(normalClipName); // 애니메이션 클립 이름 입력
        criticalAttackLength = GetAnimationLength(criticalClipName);
    }

    private float GetAnimationLength(string clipName)
    {
        if (_animator == null)
        {
            Debug.LogError("Animator가 설정되지 않았습니다.");
            return -1f;
        }

        // AnimatorController에서 모든 애니메이션 클립 가져오기
        RuntimeAnimatorController controller = _animator.runtimeAnimatorController;
        if (controller != null)
        {
            foreach (AnimationClip clip in controller.animationClips)
            {
                if (clip.name == clipName)
                {
                    return clip.length; // 애니메이션 길이 반환
                }
            }
        }

        return -1f; // 해당 클립을 찾지 못했을 경우
    }
}
