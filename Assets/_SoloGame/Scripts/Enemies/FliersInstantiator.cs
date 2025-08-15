using UnityEngine;

public class FliersInstantiator : MonoBehaviour
{
    [SerializeField] float _flierAmount;
    [SerializeField] GameObject _flierPrefab;
    [SerializeField] float _distanceToCenter;

    [SerializeField] float _radiusOfFlier;
    [SerializeField] float _speedOfFlier;
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
            GameObject obj = new GameObject("Flier" + i);
            obj.transform.SetParent(transform, false);
            obj.transform.localPosition = newPos;
            var comp1 = obj.AddComponent<SwarmWander>();
            comp1.Init(_radiusOfFlier, _speedOfFlier);
        }
    }
}
