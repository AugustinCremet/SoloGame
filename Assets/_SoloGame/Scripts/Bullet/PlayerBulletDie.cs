using BulletPro;
using UnityEngine;

public class PlayerBulletDie : BaseBulletBehaviour
{
    [SerializeField] GameObject _slimeBall;
    private Vector2 _position = Vector2.zero;

    public override void Update()
    {
        base.Update();

        Collider2D hit = Physics2D.OverlapPoint(transform.position, LayerMask.GetMask("Wall"), 1f);
        if(hit != null )
        {
            Debug.Log("Position set " + _position);
        }
        else
        {
            Debug.Log("Outside of wall");
            _position = transform.position;
        }
    }
    public override void OnBulletDeath()
    {
        base.OnBulletDeath();

        Vector2 pos = bullet.transform.position;
        Debug.Log(_position);
        Instantiate(_slimeBall, _position, Quaternion.identity);
    }
}
