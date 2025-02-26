using System.Collections.Generic;
using UnityEngine;


public class OrderPuzzleManager : MonoBehaviour
{
    public List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable> correctSequence; // Assign cubes in correct order via Inspector
    private List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable> playerSequence = new List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();

    private int currentIndex = 0;
    private AudioSource audioSource;

    public AudioClip successSound; // Assign in Inspector
    public AudioClip failSound; // Assign in Inspector

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component missing on OrderPuzzleManager!");
        }
    }

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
                    PlaySound(successSound); // Play success sound
                }
            }
            else
            {
                Debug.Log("Wrong sequence! Restarting...");
                PlaySound(failSound); // Play failure sound
                ResetPuzzle();
            }
        }
    }

    private void ResetPuzzle()
    {
        playerSequence.Clear();
        currentIndex = 0;
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); // Plays without overriding the AudioSource's main clip
        }
    }
}
