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

    public void StartTransition(string sceneName)
    {
        //_targetPosition = Camera.main.WorldToScreenPoint(playerWorldPosition);
        GameObject player = GameObject.FindWithTag("Player");
        _targetPosition = Camera.main.WorldToScreenPoint(player.transform.position);
        _holeImage.position = _targetPosition;
        _isShrinking = true;
        StartCoroutine(StartAnimation(sceneName));
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

    public IEnumerator StartAnimation(string sceneName)
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

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while(!async.isDone)
        {
            yield return null;
        }

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
            if (Vector2.Distance(_holeImage.sizeDelta, _originalSize) <= 0.1f)
            {
                _isExpanding = false;
            }
        }
    }
}
