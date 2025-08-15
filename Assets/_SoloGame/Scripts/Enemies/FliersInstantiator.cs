using UnityEngine;

public class FliersInstantiator : MonoBehaviour
{
    [SerializeField] float _flierAmount;
    [SerializeField] GameObject _flierPrefab;
    [SerializeField] float _distanceToCenter;
    private void Awake()
    {
        DistributeFliers();
    }

    private void DistributeFliers()
    {
        float angleAmount = 360f / _flierAmount;

        for(int i = 0; i < _flierAmount; ++i)
        {
            float angle = i * angleAmount * Mathf.Deg2Rad;
            Vector3 newPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * _distanceToCenter;

            var obj = Instantiate(_flierPrefab, transform);
            obj.GetComponent<SwarmWander>().Init(newPos);
        }
    }

    private void Update()
    {
        if (transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }
}
