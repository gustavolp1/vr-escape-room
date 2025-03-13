using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
 
public class PadlockManager : MonoBehaviour
{
    private List<GameObject> childObjects = new List<GameObject>();
    private bool won;

    void Update()
    {   
        if (!won){
            UpdateChildObjectsList();
            if (childObjects.Count == 0)
            {
                won = true;
                HandleEmptyList();
            }
        }
    }

    private void UpdateChildObjectsList()
    {
        childObjects.Clear(); 
        foreach (Transform child in transform)
        {
            childObjects.Add(child.gameObject);
        }
    }

    private async void HandleEmptyList()
    {
        Debug.Log("The list is empty! Waiting for 1 second...");
        await Task.Delay(1000);
        SceneManager.LoadScene("Victory");
    }
}
