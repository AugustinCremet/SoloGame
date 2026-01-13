using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SceneFade : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private float _fadeDuration = 1f;

    public IEnumerator FadeOut()
    {
        yield return StartCoroutine(FadeRoutine(0f, 1f));
    }

    public IEnumerator FadeIn()
    {
        yield return StartCoroutine(FadeRoutine(1f, 0f));
    }

    private IEnumerator FadeRoutine(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / _fadeDuration);
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, alpha);
            yield return null;
        }
        
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, endAlpha);
    }
}
