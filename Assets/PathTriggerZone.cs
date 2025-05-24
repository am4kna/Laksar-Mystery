using UnityEngine;

public class PathTriggerZone : MonoBehaviour
{
    public static bool playerInValidZone = false;
    public static Vector3 lastValidPosition;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInValidZone = true;
            lastValidPosition = other.transform.position;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInValidZone = true;
            lastValidPosition = other.transform.position;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInValidZone = false;
        }
    }
}
