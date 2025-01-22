using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeletePortal : MonoBehaviour
{
    public static DeletePortal Instance;

    [SerializeField] Sprite onSprite;
    [SerializeField] Sprite offSprite;

    public bool onPortal = true;

    SpriteRenderer spriteRenderer;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 왼쪽 마우스 클릭 감지
        if (Input.GetMouseButtonDown(0))
        {
            // UI가 아닌 월드 상의 오브젝트를 클릭한 경우 처리
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                // 클릭한 오브젝트가 업그레이드 팩토리인지 확인
                if (hit.collider.gameObject == this.gameObject)
                {
                    ActiveDeletePortar();
                }
            }
        }
    }

    void ActiveDeletePortar()
    {
        if(onPortal)
        {
            onPortal = false;
            spriteRenderer.sprite = offSprite;
        }
        else if(!onPortal)
        {
            onPortal = true;
            spriteRenderer.sprite = onSprite;
        }
    }
}
