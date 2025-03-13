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

    void Start()
    {

        keyPrefab.SetActive(false);

        // Initialize counters
        for (int i = 0; i < counters.Length; i++)
        {
            values[i] = 0;
            UpdateCounterText(i);
        }
    }

    public void Increment(int index)
    {
        values[index] = (values[index] + 1) % 10; // Loops 0-9
        UpdateCounterText(index);
    }

    public void Decrement(int index)
    {
        values[index] = (values[index] - 1 + 10) % 10; // Loops 9-0
        UpdateCounterText(index);
    }

    private void UpdateCounterText(int index)
    {
        counters[index].text = values[index].ToString();
    }

    public void ConfirmPuzzle()
    {
        if (CheckSolution()) {
            Debug.Log("Correct! Puzzle Solved!");
            keyPrefab.SetActive(true);
        }
        else {
            Debug.Log("Wrong combination. Try again.");
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
}
