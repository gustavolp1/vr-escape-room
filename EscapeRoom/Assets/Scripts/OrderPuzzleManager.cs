using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrderPuzzleManager : MonoBehaviour
{
    [SerializeField] private GameObject keyPrefab;
    public List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable> correctSequence;
    private List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable> playerSequence = new List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();

    private AudioSource audioSource;
    private bool puzzleSolved = false;

    public AudioClip successSound;
    public AudioClip failSound;
    public TextMeshPro feedbackText;
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;
    public List<Color> inputColors;

    private SceneAudioManager sceneAudioManager;

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

        // Find the SceneAudioManager in the scene
        sceneAudioManager = FindFirstObjectByType<SceneAudioManager>();
        if (sceneAudioManager == null)
        {
            Debug.LogError("SceneAudioManager not found in the scene!");
        }
    }

    public void RegisterInteraction(UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactedCube)
    {
        if (puzzleSolved) return;

        playerSequence.Add(interactedCube);
        UpdateFeedbackText();

        if (playerSequence.Count == correctSequence.Count)
        {
            if (IsSequenceCorrect())
            {
                Debug.Log("Puzzle Solved!");
                PlaySound(successSound);
                keyPrefab.SetActive(true);
                puzzleSolved = true;

                if (feedbackText != null)
                {
                    feedbackText.text = "<color=#" + ColorUtility.ToHtmlStringRGB(correctColor) + ">SOLVED!</color>";
                }

                // Switch background music after puzzle is solved
                if (sceneAudioManager != null)
                {
                    sceneAudioManager.SwitchToAlternateLoop();
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
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
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
        if (playerSequence.Count == 0 && feedbackText != null)
        {
            feedbackText.text = "";
        }
    }
}
