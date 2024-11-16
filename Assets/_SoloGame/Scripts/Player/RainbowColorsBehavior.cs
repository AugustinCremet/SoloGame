using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RainbowColorsBehavior : MonoBehaviour
{
    [SerializeField] float _followSharpness = 0.05f;
    [SerializeField] List<RainbowColorObject> _colors = new List<RainbowColorObject>();
    [SerializeField] float _smooth = 1f;
    private Quaternion _targetRotation;
    private Transform _leader;
    private Transform[] _children;
    private int _currentColorAmount;
    private Player _player;

    private void Awake()
    {
        _targetRotation = transform.rotation;
    }

    private void OnEnable()
    {
        ColorSwitcherController.onNextColorSwitch += SwitchColor;
    }

    private void OnDisable()
    {
        ColorSwitcherController.onNextColorSwitch -= SwitchColor;
    }

    public void SwitchColor(bool isNextColor)
    {
        if(isNextColor)
        {
            _targetRotation *= Quaternion.AngleAxis(360f / _currentColorAmount, Vector3.forward);
        }
        else
        {
            _targetRotation *= Quaternion.AngleAxis(-360f / _currentColorAmount, Vector3.forward);
        }
    }

    private void Start()
    {
        _children = GetComponentsInChildren<Transform>(false);
    }

    public void Initiate(List<RainbowColor> colorsToAdd, Transform leader)
    {
        _leader = leader;
        _player = _leader.GetComponent<Player>();

        List<GameObject> listOfColors = new List<GameObject>();
        foreach(var color in colorsToAdd)
        {
            foreach(var child in _colors)
            {
                if((child.RainbowColor == color))
                {
                    listOfColors.Add(child.RainbowObject);
                    _currentColorAmount++;
                    break;
                }
            }
        }

        PlaceColorsAroundPlayer(listOfColors, _leader.position, 1f);
    }

    public void PlaceColorsAroundPlayer(List<GameObject> colors, Vector3 point, float radius)
    {
        int num = colors.Count;

        for(int i = 0; i < num; i++)
        {
            /* Distance around the circle */
            float radians = (2 * Mathf.PI / num) * i;

            /* Get the vector direction NB: Sin is horizontal, so the circle start up and not right */
            float horizontal = Mathf.Sin(radians);
            float vertical = Mathf.Cos(radians);

            Vector3 spawnDir = new Vector3(horizontal, vertical, 0);

            /* Get the spawn position */
            Vector3 spawnPos = point + spawnDir * radius; // Radius is just the distance away from the point

            /* Spawn */
            GameObject color = Instantiate(colors[i], spawnPos, Quaternion.identity, gameObject.transform);
        }
    }

    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, 10 * _smooth * Time.deltaTime);
        foreach(Transform child in _children)
        {
            if(child != gameObject.transform)
                child.rotation = Quaternion.Euler(0f, 0f, gameObject.transform.rotation.z * 1f);
        }
    }
    private void LateUpdate()
    {
        transform.position += (_leader.position - transform.position) * _followSharpness;
    }
}


