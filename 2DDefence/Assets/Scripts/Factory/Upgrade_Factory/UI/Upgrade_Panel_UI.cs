using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade_Panel_UI : MonoBehaviour
{
    public Text upgrade_ad_count_txt;
    public Text upgrade_as_count_txt;

    void Start()
    {
        UpgradeADCount();
        UpgradeASCount();
    }

    public void UpgradeADCount()
    {
        upgrade_ad_count_txt.text = $"+{UnitUpgrade.Instance.adUpgradeCount}강";
    }

    public void UpgradeASCount()
    {
        upgrade_as_count_txt.text = $"+{UnitUpgrade.Instance.asUpgradeCount}강";
    }
}
