using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoMainMenu : MonoBehaviour
{
    void Start()
    {
        LoadSceneAwait();
    }

    private async void LoadSceneAwait()
    {
        Debug.Log("Waiting for 1 second...");
        await Task.Delay(1000);
        SceneManager.LoadScene("MainMenu");
    }
}
