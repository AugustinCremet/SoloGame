using System.Collections;
using Mono.Cecil.Cil;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Splines.Interpolators;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] RectTransform _holeImage;
    [SerializeField] CutoutMaskUI _bgImage;


    private bool _isShrinking = false;
    private bool _isExpanding = false;
    private Vector2 _originalSize;
    private Vector2 _targetPosition;

    private void Start()
    {
        _originalSize = _holeImage.sizeDelta;
    }

    public void StartTransition()
    {
        //_targetPosition = Camera.main.WorldToScreenPoint(playerWorldPosition);
        GameObject player = GameObject.FindWithTag("Player");
        _targetPosition = Camera.main.WorldToScreenPoint(player.transform.position);
        _holeImage.position = _targetPosition;
        _isShrinking = true;
        StartCoroutine(Animation(false, 2f));
    }

    void EndTransition()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            _targetPosition = Camera.main.WorldToScreenPoint(player.transform.position);
            _holeImage.position = _targetPosition;
        }

        _isExpanding = true;
    }

    public IEnumerator Animation(bool isEnding, float duration)
    {
        float elapsedTime = 0f;
        float endValue = 1f;
        Vector2 endSize = new Vector2(0f, 0f);

        if(isEnding)
        {
            endValue = 0f;
            endSize = _originalSize;
        }
        float startingAlpha = _bgImage.color.a;
        Vector2 startingSize = _holeImage.sizeDelta;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float newAlpha = Mathf.Lerp(startingAlpha, endValue, elapsedTime / duration) * 2f;
            _bgImage.color = new Color(_bgImage.color.r, _bgImage.color.g, _bgImage.color.b, newAlpha);

            Vector2 newSize = Vector2.Lerp(startingSize, endSize, elapsedTime / duration);
            _holeImage.sizeDelta = newSize;

            yield return null;
        }

        if(!isEnding)
        {
            StartCoroutine(Animation(true, 2f));
        }
    }

    private void Update()
    {
        if (_isShrinking)
        {
            //_holeImage.sizeDelta = Vector2.MoveTowards(_holeImage.sizeDelta, Vector2.zero, _shrinkSpeed * Time.deltaTime);

            if (_holeImage.sizeDelta.magnitude <= 0.1f)
            {
                _isShrinking = false;
                EndTransition();
            }
        }
        else if (_isExpanding)
        {
            //_holeImage.sizeDelta = Vector2.MoveTowards(_holeImage.sizeDelta, _originalSize, _expandSpeed * Time.deltaTime);

            if (Vector2.Distance(_holeImage.sizeDelta, _originalSize) <= 0.1f)
            {
                _isExpanding = false;
            }
        }
    }
}
