using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject _ceiling;

    public static List<Room> AllRooms = new List<Room>();

    private void Awake()
    {
        AllRooms.Add(this);
    }

    private void OnDestroy()
    {
        AllRooms.Remove(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            foreach(Room room in AllRooms)
            {
                room._ceiling.SetActive(room == this);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _ceiling.SetActive(false);
        }
    }
}
