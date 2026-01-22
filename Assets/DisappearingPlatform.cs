using UnityEngine;
using System.Collections;

public class DisappearingPlatform : MonoBehaviour
{
    public float fadeDuration = 0.3f; // длительность затухания

    private SpriteRenderer spriteRenderer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            StartCoroutine(FadeToTransparent());
            StartCoroutine(WaitForDestroy());
        }
    }

    IEnumerator FadeToTransparent()
    {
        Color originalColor = spriteRenderer.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Убедимся, что объект полностью прозрачен в конце
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }

    private IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }
}