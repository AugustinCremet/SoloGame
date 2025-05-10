using UnityEngine;
using UnityEngine.SceneManagement;

public class HamiltonianSwitch : MonoBehaviour
{
    [SerializeField] string _puzzleName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.OnInteract += Interact;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.OnInteract -= Interact;
        }
    }

    private void Interact()
    {
        Debug.Log("Player interact (Start Hamiltonian puzzle)");
        GameManager.Instance.SwitchScene(_puzzleName);
    }
}
