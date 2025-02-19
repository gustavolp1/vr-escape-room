using System.Collections.Generic;
using UnityEngine;


public class OrderPuzzleManager : MonoBehaviour
{
    public List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable> correctSequence; // Assign cubes in correct order via Inspector
    private List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable> playerSequence = new List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
    
    private int currentIndex = 0;

    public void RegisterInteraction(UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactedCube)
    {
        if (currentIndex < correctSequence.Count)
        {
            if (interactedCube == correctSequence[currentIndex])
            {
                playerSequence.Add(interactedCube);
                currentIndex++;

                if (currentIndex == correctSequence.Count)
                {
                    Debug.Log("Puzzle Solved!");
                    // Call a success function here (e.g., unlock door, change scene, etc.)
                }
            }
            else
            {
                Debug.Log("Wrong sequence! Restarting...");
                ResetPuzzle();
            }
        }
    }

    private void ResetPuzzle()
    {
        playerSequence.Clear();
        currentIndex = 0;
    }
}
