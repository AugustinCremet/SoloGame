using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            
        }
    }

    private void Interact()
    {
        Debug.Log("Player interact (save the game)");
        GameManager.Instance.SaveGame();
    }
}
