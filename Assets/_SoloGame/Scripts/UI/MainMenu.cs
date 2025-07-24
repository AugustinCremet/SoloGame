using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject _SpashScreen;
    [SerializeField] GameObject _MainMenu;
    [SerializeField] GameObject _StartMenu;
    [SerializeField] GameObject[] _SaveSlots;
    [SerializeField] GameObject _OptionMenu;


    private void Start()
    {

    }
    public void OnStart()
    {
        _MainMenu.SetActive(false);
        _StartMenu.SetActive(true);

        // AC_TODO temporary to show game exist
        for (int i = 1; i <= 3; i++)
        {
            string path = Application.persistentDataPath + $"/Game{i}.json";
            if (File.Exists(path))
            {
                Debug.Log(_SaveSlots[i - 1]);
                _SaveSlots[i - 1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Continue";
            }
        }
    }

    public void OnSaveSlot(int saveSlotNumber)
    {
        GameManager.Instance.SetSaveSlot(saveSlotNumber);
        GameManager.Instance.LoadGame();
    }

    public void OnOption()
    {

    }

    public void OnExit()
    {
        
    }
}
