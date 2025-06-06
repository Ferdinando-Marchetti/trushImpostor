using UnityEngine;

public class UIExplosione : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float durata = 1f;

    private void OnEnable()
    {
        canvasGroup.alpha = 0f;
        StartCoroutine(FadeIn());
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer < durata)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / durata);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}
