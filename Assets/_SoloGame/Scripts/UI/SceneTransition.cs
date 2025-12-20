using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] RectTransform _holeImage;
    [SerializeField] CutoutMaskUI _bgImage;

    private Vector2 _originalSize;
    private Vector2 _targetPosition;
    private float _animationDuration = 2f;

    private void Awake()
    {
        _originalSize = _holeImage.sizeDelta;
    }

    //public IEnumerator StartFadeIn(Action onComplete)
    //{
    //    Debug.Log("StartFadeIn");
    //    StartCoroutine(FadeIn(onComplete));
    //}

    public void StartTransition(string sceneName, bool isSceneAdded)
    {
        ChangeAnimationPosition(isSceneAdded);
        StartCoroutine(StartAnimation(sceneName, isSceneAdded));
    }

    public IEnumerator StartAnimation(string sceneName, bool isSceneAdded)
    {
        yield return StartCoroutine(FadeIn());

        AsyncOperation async;
        if (isSceneAdded)
        {
            async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
        else
        {
            async = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
        }

        while (!async.isDone)
        {
            yield return null;
        }

        ChangeAnimationPosition(!isSceneAdded);
        StartCoroutine(FadeOut(false));
    }

    public IEnumerator FadeIn(Action onComplete = null)
    {
        ChangeAnimationPosition(true);
        float elapsedTime = 0f;
        Vector2 endSize = new Vector2(0f, 0f);
        Vector2 startingSize = _holeImage.sizeDelta;

        while (elapsedTime < _animationDuration)
        {
            elapsedTime += Time.deltaTime;

            Vector2 newSize = Vector2.Lerp(startingSize, endSize, elapsedTime / _animationDuration);
            _holeImage.sizeDelta = newSize;

            yield return null;
        }

        onComplete?.Invoke();
    }

    public IEnumerator FadeOut(bool focusOnMainPlayer)
    {
        ChangeAnimationPosition(focusOnMainPlayer);
        float elapsedTime = 0f;
        Vector2 endSize = _originalSize;
        Vector2 startingSize = _holeImage.sizeDelta;

        while (elapsedTime < _animationDuration)
        {
            elapsedTime += Time.deltaTime;

            Vector2 newSize = Vector2.Lerp(startingSize, endSize, elapsedTime / _animationDuration);
            _holeImage.sizeDelta = newSize;

            yield return null;
        }
    }

    public void ResetTransition()
    {
        Debug.Log("ResetTransition " + _originalSize);
        _holeImage.sizeDelta = _originalSize;
    }

    private void ChangeAnimationPosition(bool isMainPlayer)
    {
        GameObject player = null;
        Camera camera = null;

        if (isMainPlayer)
        {
            player = GameObject.FindWithTag("Player");
            camera = Camera.main;
        }
        else
        {
            player = GameObject.FindWithTag("PlayerGoo");
            camera = GameObject.FindWithTag("PuzzleCamera").GetComponent<Camera>();
        }

        _targetPosition = camera.WorldToScreenPoint(player.transform.position);
        _holeImage.position = _targetPosition;
    }
}
