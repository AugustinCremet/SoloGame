using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatBubble : MonoBehaviour
{
    [SerializeField] GameObject _chatBubbleGO;
    [SerializeField] TextMeshProUGUI _textMeshPro;

    private Queue<string> _lines = new Queue<string>();

    private void Awake()
    {
        _chatBubbleGO.SetActive(false);
        _textMeshPro.enabled = false;
    }

    public void StartDialogue(IEnumerable<string> lines)
    {
        _lines.Clear();
        foreach (string line in lines)
        {
            _lines.Enqueue(line);
        }
        ShowNextLine();
    }

    public void OnContinueDialogue()
    {
        if(_lines.Count > 0)
        {
            ShowNextLine();
        }
        else
        {
            EndDialogue();
        }
    }

    private void ShowNextLine()
    {
        string nextLine = _lines.Dequeue();
        SetText(nextLine);
    }

    private void EndDialogue()
    {
        GetComponentInParent<Player>().EndChat();
        DeactivateText();
    }

    public void SetFailPuzzle()
    {
        _chatBubbleGO.SetActive(true);
        _textMeshPro.enabled = true;
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
