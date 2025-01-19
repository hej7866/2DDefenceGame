    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    public class DamageUI : MonoBehaviour
    {
        public static DamageUI Instance;
        public GameObject damageTextPrefab; // 데미지 텍스트 프리팹
        public Transform worldCanvas;      // 월드 공간 캔버스

        private void Awake()
        {
            Instance = this;
        }

        public void ShowDamage(Vector3 position, int damage, bool isCritical)
        {
            // 데미지 텍스트 생성
            GameObject damageText = Instantiate(damageTextPrefab, worldCanvas);
            damageText.GetComponent<Text>().text = damage.ToString();

            if(isCritical) damageText.GetComponent<Text>().color = Color.red;

            // 텍스트 위치를 월드 좌표로 설정
            damageText.transform.position = position;

            // 텍스트 애니메이션 및 제거
            StartCoroutine(FadeOutAndDestroy(damageText));
        }

        public void ShowSkillDamage(Vector3 position, int damage)
        {
            // 데미지 텍스트 생성
            GameObject damageText = Instantiate(damageTextPrefab, worldCanvas);
            damageText.GetComponent<Text>().text = damage.ToString();
            damageText.GetComponent<Text>().color = Color.blue;

            // 텍스트 위치를 월드 좌표로 설정
            damageText.transform.position = position;

            // 텍스트 애니메이션 및 제거
            StartCoroutine(FadeOutAndDestroy(damageText));
        }

        private IEnumerator FadeOutAndDestroy(GameObject damageText)
        {
            Text text = damageText.GetComponent<Text>();
            Color originalColor = text.color;
            float duration = 1f; // 1초 동안 표시

            // 텍스트가 천천히 사라짐
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                float alpha = Mathf.Lerp(1, 0, t / duration);
                text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                damageText.transform.Translate(Vector3.up * Time.deltaTime); // 텍스트가 위로 이동
                yield return null;
            }

            Destroy(damageText); // 텍스트 제거
        }
    }
