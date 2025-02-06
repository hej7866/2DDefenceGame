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

    private int currentWave;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
        originalColor = spriteRenderer.color;
        animator = GetComponentInChildren<Animator>();

        currentWave = GameManager.Instance.currentWave;
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
            int randomValue = Random.Range(5,21); // 5 ~ 20;

            GameManager.Instance.AddGold(50 + currentWave * randomValue); // 돈 추가 (50 + 현재 웨이브 * 랜덤밸류)
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
        else if (collision.CompareTag("JewelPortal"))
        {
            GameManager.Instance.AddJewel(1); // 보석 추가
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
