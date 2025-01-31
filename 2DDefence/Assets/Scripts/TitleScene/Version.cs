using UnityEngine;
using TMPro; // TextMeshPro 사용 시 필요

public class Version : MonoBehaviour
{
    public TextMeshProUGUI versionText; // TMP 사용 시
    // public Text versionText; // 일반 UI Text 사용 시

    void Start()
    {
        versionText.text = "Version: " + Application.version;
    }
}
