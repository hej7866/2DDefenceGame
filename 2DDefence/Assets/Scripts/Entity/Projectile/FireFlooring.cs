using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlooring : MonoBehaviour
{
    public float fireFlooringDamage; // 불 장판의 데미지    

    // 장판에 들어온 적 목록 관리
    private List<Enemy> enemiesInside = new List<Enemy>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && !enemiesInside.Contains(enemy))
            {
                enemiesInside.Add(enemy);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemiesInside.Remove(enemy);
            }
        }
    }

    private void Start()
    {
        StartCoroutine(DoTickDamage());
    }

    private IEnumerator DoTickDamage()
    {
        float tickInterval = 0.3f;
        float duration = 5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // 1) enemiesInside의 복사본을 만들기
            Enemy[] enemyArray = enemiesInside.ToArray();

            // 2) 복사본을 순회
            foreach (var enemy in enemyArray)
            {
                if (enemy == null || enemy.isDead)
                {
                    enemiesInside.Remove(enemy);
                    continue;
                }

                enemy.TakeDamage(fireFlooringDamage);
            }

            yield return new WaitForSeconds(tickInterval);
            elapsed += tickInterval;
        }

        Destroy(gameObject);
    }



}
