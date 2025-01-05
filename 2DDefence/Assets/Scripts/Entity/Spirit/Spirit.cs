using UnityEngine;

public class Spirit : Move
{
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private LineRenderer lineRenderer; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
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
            UnitSpawnManager.Instance.ExecuteRandomFunction();
            Destroy(gameObject);
        }
    }

    public void Select()
    {
        //spriteRenderer.color = Color.green;
        lineRenderer.enabled = true;

    }

    public void Deselect()
    {
        //spriteRenderer.color = originalColor;
        lineRenderer.enabled = false;
    }
}
