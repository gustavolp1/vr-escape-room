using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountingPuzzleManager : MonoBehaviour
{
    public TextMeshPro[] counters; // Drag and drop CounterBlasters, CounterChairs, etc.
    private int[] values = new int[4]; // Holds current values of the counters
    public int[] correctCombination = {3, 7, 1, 5}; // Set the correct answer here
    [SerializeField] private GameObject keyPrefab;
    
    // Sound Effects
    private AudioSource audioSource;
    public AudioClip buttonSound;  // Sound for button presses
    public AudioClip successSound; // Sound for correct answer
    public AudioClip failSound;    // Sound for incorrect answer

    void Start()
    {
        keyPrefab.SetActive(false);

        // Initialize counters
        for (int i = 0; i < counters.Length; i++)
        {
            values[i] = 0;
            UpdateCounterText(i);
        }

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component missing on CountingPuzzleManager!");
        }
    }

    public void Increment(int index)
    {
        values[index] = (values[index] + 1) % 10; // Loops 0-9
        UpdateCounterText(index);
        PlaySound(buttonSound);
    }

    public void Decrement(int index)
    {
        values[index] = (values[index] - 1 + 10) % 10; // Loops 9-0
        UpdateCounterText(index);
        PlaySound(buttonSound);
    }

    public void ConfirmPuzzle()
    {
        if (CheckSolution()) 
        {
            Debug.Log("Correct! Puzzle Solved!");
            keyPrefab.SetActive(true);
            PlaySound(successSound); // Play correct sound
        }
        else 
        {
            Debug.Log("Wrong combination. Try again.");
            PlaySound(failSound); // Play wrong sound
        }
    }

    private bool CheckSolution()
    {
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i] != correctCombination[i])
                return false;
        }
        return true;
    }

    private void UpdateCounterText(int index)
    {
        counters[index].text = values[index].ToString();
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
