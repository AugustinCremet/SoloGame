using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.Cinemachine;

public class ScreenRadialFade : MonoBehaviour
{
    [SerializeField] float _fadeDuration = 0.25f;

    Material _mat;
    Camera _cam;

    private void Awake()
    {
        _mat = GetComponent<Image>().material;
        _cam = Camera.main;
    }

    private Vector2 WorldToUV(Vector3 worldPos)
    {
        Vector3 screen = _cam.WorldToScreenPoint(worldPos);
        return new Vector2(
            screen.x / Screen.width,
            screen.y / Screen.height
        );
    }

    public IEnumerator FadeOut(Vector3 worldPos)
    {
        Vector2 uv = WorldToUV(worldPos);
        _mat.SetVector("_Center", uv);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / _fadeDuration;
            _mat.SetFloat("_Radius", Mathf.Lerp(1.4f, 0f, t));
            yield return null;
        }
    }

    public IEnumerator FadeIn(Vector3 worldPos)
    {
        Vector2 uv = WorldToUV(worldPos);
        _mat.SetVector("_Center", uv);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / _fadeDuration;
            _mat.SetFloat("_Radius", Mathf.Lerp(0f, 1.4f, t));
            yield return null;
        }
    }
}
