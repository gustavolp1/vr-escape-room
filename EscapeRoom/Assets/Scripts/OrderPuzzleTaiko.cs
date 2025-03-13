using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OrderPuzzleTaiko : MonoBehaviour
{
    private OrderPuzzleManager orderPuzzleManager;

    void Start()
    {
        orderPuzzleManager = FindFirstObjectByType<OrderPuzzleManager>(); // Find the manager in the scene
        if (orderPuzzleManager == null)
            Debug.LogError("OrderPuzzleManager not found in the scene!");
    }

    public void OnCubeSelected(SelectEnterEventArgs args)
    {
        Debug.Log(gameObject.name + " selected!");
        orderPuzzleManager.RegisterInteraction(GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>());
    }
}
