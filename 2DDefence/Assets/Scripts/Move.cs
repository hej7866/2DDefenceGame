using UnityEngine;

public class Move : MonoBehaviour
{
    protected Vector3 targetPosition;
    protected bool isMoving;

    [SerializeField] protected float speed = 5f;

    protected virtual void Update()
    {
        HandleMovement();
    }

    public virtual void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
        isMoving = true;
    }

    protected virtual void HandleMovement()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // 목표 위치에 도달하면 멈춤
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
            }
        }
    }
}
