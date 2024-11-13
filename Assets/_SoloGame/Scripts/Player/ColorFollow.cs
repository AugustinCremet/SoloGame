using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFollow : MonoBehaviour
{
    [SerializeField] Transform _leader;
    [SerializeField] float _followSharpness = 0.05f;
    [SerializeField] List<GameObject> _colors = new List<GameObject>();
    [SerializeField] float _smooth = 1f;
    private Quaternion _targetRotation;

    private void Awake()
    {
        CreateColorsAroundPlayer(_colors, _leader.transform.position, 1);
        _targetRotation = transform.rotation;
    }

    public void CreateColorsAroundPlayer(List<GameObject> colors, Vector3 point, float radius)
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
        if (Input.GetKeyDown("e"))
        {
            _targetRotation *= Quaternion.AngleAxis(360f / _colors.Count, Vector3.forward);
        }
        if(Input.GetKeyDown("q"))
        {
            _targetRotation *= Quaternion.AngleAxis(-360f / _colors.Count, Vector3.forward);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, 10 * _smooth * Time.deltaTime);
    }
    private void LateUpdate()
    {
        transform.position += (_leader.position - transform.position) * _followSharpness;
    }
}


