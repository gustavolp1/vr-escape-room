using UnityEngine;

public class SceneAudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip defaultLoop;
    public AudioClip alternateLoop;

    private bool isAlternatePlaying = false;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        audioSource.clip = defaultLoop;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void SwitchToAlternateLoop()
    {
        if (!isAlternatePlaying)
        {
            audioSource.Stop();
            audioSource.clip = alternateLoop;
            audioSource.Play();
            isAlternatePlaying = true;
        }
    }
}
