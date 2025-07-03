using UnityEngine;

public class SlimeBall : MonoBehaviour, ICollectable
{
    private Rigidbody2D _rb;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Vector2 dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        float magnitude = Random.Range(1f, 2f);
        Vector2 force = dir * magnitude;
        _rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void OnCollect(GameObject collector)
    {
        collector.GetComponent<Player>().Heal(1);
        Destroy(gameObject);
    }
}
