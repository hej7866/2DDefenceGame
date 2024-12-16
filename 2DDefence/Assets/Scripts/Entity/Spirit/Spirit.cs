using UnityEngine;

public class Spirit : Move
{
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    private void OnDestroy()
    {
        EntityController entityController = FindObjectOfType<EntityController>();
        if (entityController != null)
        {
            entityController.RemoveSpirit(this);
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MoneyPortal"))
        {
            GameManager.Instance.AddGold(100); // 돈 추가
            Destroy(gameObject);
        }
        else if (collision.CompareTag("UnitPortal"))
        {
            GameManager.Instance.SpawnRandomUnit();
            Destroy(gameObject);
        }
    }

    public void Select()
    {
        spriteRenderer.color = Color.green; // 선택 시 색상 변경
    }

    public void Deselect()
    {
        spriteRenderer.color = originalColor; // 선택 해제 시 원래 색상 복원
    }
}
