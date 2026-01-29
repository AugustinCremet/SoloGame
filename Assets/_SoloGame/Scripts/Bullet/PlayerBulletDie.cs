using BulletPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBulletDie : BaseBulletBehaviour
{
    [SerializeField] GameObject _slimeBall;
    private Vector2 _lastSafePosition = Vector2.zero;
    private PolygonCollider2D _roomBound;

    public override void OnBulletBirth()
    {
        base.OnBulletBirth();
        GameObject[] allObjWithTag = GameObject.FindGameObjectsWithTag("RoomBound");
        Scene currentScene = SceneManager.GetSceneByName(MySceneManager.Instance.CurrentScene);
        //Debug.Log(currentScene.name);

        foreach (GameObject obj in allObjWithTag)
        {
            if(obj.scene == currentScene)
            {
                _roomBound = obj.GetComponent<PolygonCollider2D>();
            }
        }
    }

    public override void Update()
    {
        base.Update();

        bool insideRoom = _roomBound == null || _roomBound.OverlapPoint(transform.position);
        Collider2D hit = Physics2D.OverlapPoint(transform.position, LayerMask.GetMask("Wall"), 1f);

        if (hit != null)
        {
            Debug.Log("Position set " + _lastSafePosition);
            bullet.Die();
        }
        else
        {
            _lastSafePosition = transform.position;
        }

        if (!insideRoom)
        {
            Debug.Log("Outside room bounds");
            bullet.Die();
            return;
        }
    }

    //public override void Update()
    //{
    //    _lastSafePosition = transform.position;

    //    Vector2 movement = bullet.transform.up;

    //    Debug.Log("Movement vector: " + movement);
    //    Debug.DrawLine(transform.position, transform.position + (Vector3)(movement * 2f), Color.red, 0.05f);

    //    RaycastHit2D hit = Physics2D.Raycast(
    //        transform.position,
    //        movement,
    //        3f,
    //        1 << 9
    //    );

    //    if (hit.collider != null)
    //    {
    //        _lastSafePosition = transform.position;
    //        bullet.Die();
    //        return;
    //    }
    //}
    public override void OnBulletDeath()
    {
        base.OnBulletDeath();

        Vector2 pos = bullet.transform.position;
        Instantiate(_slimeBall, _lastSafePosition, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bullet.Die();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        bullet.Die();
    }
}
