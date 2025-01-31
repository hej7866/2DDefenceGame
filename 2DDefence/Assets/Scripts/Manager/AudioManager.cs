using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    AudioSource audioSource;

    [Header("사운드 설정 패널")]
    [SerializeField] GameObject setting_panel;

    [Header("슬라이더")]
    [SerializeField] Slider BGM_slider;

    bool setting = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        audioSource.volume = BGM_slider.value;
    }

}
