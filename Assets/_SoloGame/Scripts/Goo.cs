using UnityEngine;

public class Goo : MonoBehaviour
{
    void Update()
    {
        float scale = Mathf.Lerp(transform.localScale.x, 0f, Time.deltaTime / 2f);
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
