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
        Debug.Log(currentScene.name);

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

        if(hit != null )
        {
            Debug.Log("Position set " + _lastSafePosition);
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
    public override void OnBulletDeath()
    {
        base.OnBulletDeath();

        Vector2 pos = bullet.transform.position;
        Instantiate(_slimeBall, _lastSafePosition, Quaternion.identity);
    }
}
