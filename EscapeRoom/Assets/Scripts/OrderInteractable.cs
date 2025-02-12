using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OrderInteractable : MonoBehaviour
{
    private Renderer objRenderer;
    
    private void Awake()
    {
        objRenderer = GetComponent<Renderer>(); // Get object's material
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand")) // Ensure the hand has this tag
        {
            Debug.Log("Object touched by hand!");
            objRenderer.material.color = Color.green; // Change color on touch

            // Add haptic feedback
            var controller = other.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor>();
            if (controller != null)
            {
                controller.SendHapticImpulse(0.5f, 0.2f); // Intensity, Duration
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerHand")) 
        {
            Debug.Log("Hand removed from object!");
            objRenderer.material.color = Color.white; // Reset color
        }
    }
}
