using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoLevel : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("EscapeRoom");
    }
}
