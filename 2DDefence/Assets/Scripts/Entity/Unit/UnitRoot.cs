using UnityEngine;
public class UnitRoot : MonoBehaviour
{
    private Unit parentUnit;

    void Start()
    {
        parentUnit = GetComponentInParent<Unit>();
    }

    public void ApplyDamageEvent()
    {
        if (parentUnit != null)
        {
            parentUnit.ApplyDamage();
        }
    }

    public void ApplyCriticalDamageEvent()
    {
        if (parentUnit != null)
        {
            parentUnit.ApplyCriticalDamage();
        }
    }

    public void ResetTargetEvent()
    {
        if (parentUnit != null)
        {
            parentUnit.ResetTarget();
        }
    }
}