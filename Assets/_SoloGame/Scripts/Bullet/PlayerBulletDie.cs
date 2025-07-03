using BulletPro;
using UnityEngine;

public class PlayerBulletDie : BaseBulletBehaviour
{
    [SerializeField] GameObject _slimeBall;
    
    public override void OnBulletDeath()
    {
        base.OnBulletDeath();
        Instantiate(_slimeBall, transform.position, Quaternion.identity);
    }
}
