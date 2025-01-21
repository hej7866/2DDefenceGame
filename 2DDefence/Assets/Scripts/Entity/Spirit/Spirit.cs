using UnityEngine;
using UnityEngine.UI;

public class Spirit : Move
{
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Animator animator;

    private LineRenderer lineRenderer; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
        originalColor = spriteRenderer.color;
        animator = GetComponentInChildren<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        animator.SetBool("1_Move", isMoving); // 이동 애니메이션 구현
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
            UnitManager unitManager = UnitManager.Instance;
            if(unitManager.unitPopulation < unitManager.populationLimit)
            {    
                UnitSpawnManager.Instance.ExecuteRandomFunction();
            }
            else if(unitManager.unitPopulation >= unitManager.populationLimit)
            {
                string log = "최대 인구수에 도달하여 유닛을 생산할 수 없습니다.";
                LogManager.Instance.Log(log);
                return;
            }
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
