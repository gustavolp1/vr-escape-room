using UnityEngine;

public class Padlock : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Key"))
        {
            AudioSource audioData = GetComponentInParent<AudioSource>();
            audioData.Play();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
