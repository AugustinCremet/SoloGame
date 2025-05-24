using TMPro;
using UnityEngine;

public class ChatBubble : MonoBehaviour
{
    [SerializeField] SpriteRenderer _bubbleSpriteRenderer;
    [SerializeField] TextMeshPro _textMeshPro;

    private void Awake()
    {
        _bubbleSpriteRenderer.enabled = false;
        _textMeshPro.enabled = false;
    }

    public void SetFailPuzzle()
    {
        _bubbleSpriteRenderer.enabled = true;
        _textMeshPro.enabled = true;
        SetText("Oh no! I must retry.");
    }
    public void SetText(string text)
    {
        _textMeshPro.SetText(text);
        _textMeshPro.ForceMeshUpdate();
        Vector2 textSize = _textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(.3f, 0.6f);
        _bubbleSpriteRenderer.size = textSize + padding;
    }

    private void Update()
    {
        transform.eulerAngles = Vector3.zero;
    }
}
