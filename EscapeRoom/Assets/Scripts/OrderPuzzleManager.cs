using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class OrderPuzzleManager : MonoBehaviour
{
    [SerializeField] private GameObject keyPrefab;
    public List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable> correctSequence; // Assign cubes in correct order via Inspector
    private List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable> playerSequence = new List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();

    private int currentIndex = 0;
    private AudioSource audioSource;
    private bool puzzleSolved = false;

    public AudioClip successSound; // Assign in Inspector
    public AudioClip failSound; // Assign in Inspector
    public TextMeshPro feedbackText; // Assign in Inspector
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;
    public List<Color> inputColors; // Assign different colors for inputs in Inspector

    void Start()
    {

        keyPrefab.SetActive(false);
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component missing on OrderPuzzleManager!");
        }
        
        if (feedbackText != null)
        {
            feedbackText.text = "";
        }
    }

    public void RegisterInteraction(UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactedCube)
    {
        if (puzzleSolved) return; // Ignore inputs if the puzzle is already solved

        playerSequence.Add(interactedCube);
        UpdateFeedbackText();

        if (playerSequence.Count == correctSequence.Count)
        {
            if (IsSequenceCorrect())
            {
                Debug.Log("Puzzle Solved!");
                PlaySound(successSound);
                keyPrefab.SetActive(true);
                puzzleSolved = true; // Disable further inputs
                if (feedbackText != null)
                {
                    feedbackText.text = "<color=#" + ColorUtility.ToHtmlStringRGB(correctColor) + ">SOLVED!</color>";
                }
            }
            else
            {
                Debug.Log("Wrong sequence! Restarting...");
                PlaySound(failSound);
                if (feedbackText != null)
                {
                    feedbackText.text = "<color=#" + ColorUtility.ToHtmlStringRGB(wrongColor) + ">WRONG!</color>";
                    StartCoroutine(ClearFeedbackAfterDelay(2f));
                }
                ResetPuzzle();
            }
        }
    }

    private bool IsSequenceCorrect()
    {
        for (int i = 0; i < correctSequence.Count; i++)
        {
            if (playerSequence[i] != correctSequence[i])
            {
                return false;
            }
        }
        return true;
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

    private void UpdateFeedbackText()
    {
        if (feedbackText != null)
        {
            feedbackText.text = GetColoredInputSequence();
        }
    }

    private string GetColoredInputSequence()
    {
        List<string> sequence = new List<string>();
        for (int i = 0; i < playerSequence.Count; i++)
        {
            int index = correctSequence.IndexOf(playerSequence[i]);
            string colorCode = (index >= 0 && index < inputColors.Count) ? ColorUtility.ToHtmlStringRGB(inputColors[index]) : "FFFFFF";
            sequence.Add("<color=#" + colorCode + ">" + (index + 1).ToString() + "</color>");
        }
        return string.Join(" ", sequence);
    }

    private System.Collections.IEnumerator ClearFeedbackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (playerSequence.Count == 0 && feedbackText != null) // Only clear if no new inputs were made
        {
            feedbackText.text = "";
        }
    }
}