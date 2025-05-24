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
    private float _animationDuration = 2f;

    private void Start()
    {
        _originalSize = _holeImage.sizeDelta;
    }

    public void StartTransition(string sceneName, bool isSceneAdded)
    {
        ChangeAnimationPosition(isSceneAdded);
        StartCoroutine(StartAnimation(sceneName, isSceneAdded));
    }

    public IEnumerator StartAnimation(string sceneName, bool isSceneAdded)
    {
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

        AsyncOperation async = null;
        if(isSceneAdded)
        {
            async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
        else
        {
            async = SceneManager.UnloadSceneAsync(sceneName);
        }

        while(!async.isDone)
        {
            yield return null;
        }

        ChangeAnimationPosition(!isSceneAdded);
        StartCoroutine(EndAnimation(sceneName));
    }

    public IEnumerator EndAnimation(string sceneName)
    {
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
