using UnityEngine;

public class Padlock : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Key"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
