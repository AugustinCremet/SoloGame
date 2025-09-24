using TMPro;
using UnityEngine;

public class ChatBubble : MonoBehaviour
{
    [SerializeField] GameObject _chatBubbleGO;
    [SerializeField] TextMeshProUGUI _textMeshPro;

    private void Awake()
    {
        _chatBubbleGO.SetActive(false);
        _textMeshPro.enabled = false;
    }

    public void SetFailPuzzle()
    {
        _chatBubbleGO.SetActive(true);
        _textMeshPro.enabled = true;
        SetText("Oh no! I must retry.");
    }

    public void DeactivateText()
    {
        _chatBubbleGO.SetActive(false);
        _textMeshPro.enabled = false;
    }
    public void SetText(string text)
    {
        _chatBubbleGO.SetActive(true);
        _textMeshPro.enabled = true;
        _textMeshPro.SetText(text);
        _textMeshPro.ForceMeshUpdate();
        Vector2 textSize = _textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(.3f, 0.6f);
        //_bubbleSpriteRenderer.size = textSize + padding;
    }

    private void Update()
    {
        transform.eulerAngles = Vector3.zero;
    }
}
