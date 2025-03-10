using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class GameManager : MonoBehaviour {
  [SerializeField] private Transform gameTransform;
  [SerializeField] private Transform piecePrefab;
  [SerializeField] private GameObject keyPrefab;

  private List<Transform> pieces;
  private int emptyLocation;
  private int size;
  private bool shuffling = true;
  private bool won = false;
  private bool currentlyInteracting = false; 
  public UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor rightRayInteractor; // Reference to the XRRayInteractor on the right hand controller
  public UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor leftRayInteractor;  // Reference to the XRRayInteractor on the left hand controller

  // Create the game setup with size x size pieces.
  private void CreateGamePieces(float gapThickness) {
    // This is the width of each tile.
    float width = 1 / (float)size;
    for (int row = 0; row < size; row++) {
        for (int col = 0; col < size; col++) {
            Transform piece = Instantiate(piecePrefab, gameTransform);
            pieces.Add(piece);

            // In this case, the board is on a wall, and we want the top-left corner to be the starting point.
            piece.localPosition = new Vector3(
                0,                                    
                1 - (2 * width * row) - width,     
                -1 + (2 * width * col) + width   
            );
            piece.localScale = ((2 * width) - gapThickness) * Vector3.one;
            piece.name = $"{(row * size) + col}";

            TextMeshPro textComponent = piece.GetComponentInChildren<TextMeshPro>();
            textComponent.text = piece.name;

            // We want an empty space in the bottom right.
            if ((row == size - 1) && (col == size - 1)) {
                emptyLocation = (size * size) - 1;
                piece.gameObject.SetActive(false);
            } else {
                // UV Calculation based on the grid position
                Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                Vector2[] uv = new Vector2[4];
                // UV coord order: (0, 1), (1, 1), (0, 0), (1, 0)
                uv[0] = new Vector2((float)col / size, 1 - (float)(row + 1) / size);  // Top-left corner
                uv[1] = new Vector2((float)(col + 1) / size, 1 - (float)(row + 1) / size);  // Top-right corner
                uv[2] = new Vector2((float)col / size, 1 - (float)row / size);  // Bottom-left corner
                uv[3] = new Vector2((float)(col + 1) / size, 1 - (float)row / size);  // Bottom-right corner

                // Assign our new UVs to the mesh.
                mesh.uv = uv;
            }
        }
    }
  }



  // Start is called before the first frame update
  void Start() {

    keyPrefab.SetActive(false);

    pieces = new List<Transform>();
    size = 3;
    CreateGamePieces(0.01f);

    foreach (var piece in pieces)
    {
        // Make sure each piece has an XRSimpleInteractable
        var interactable = piece.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnPieceSelected);
            interactable.selectExited.AddListener(OnPieceDeselected); 
        }
    }

    StartCoroutine(WaitShuffle(0.5f));
  }

  // Update is called once per frame
  void Update() {

    // Check for completion.
    if (!shuffling && !won) {
      if (CheckCompletion()){
        print("You gosh darn did it! - Respectuflly, the sliding puzzle");
        keyPrefab.SetActive(true);
        won = true;
      }
    }

  }

  private void OnPieceSelected(SelectEnterEventArgs args){
      for (int i = 0; i < pieces.Count; i++)
      {
          var piece = pieces[i];
          if (piece.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>().isSelected && !currentlyInteracting)
          {
              // Check each direction to see if valid move
              if (SwapIfValid(i, -size, size)) { break; }
              if (SwapIfValid(i, +size, size)) { break; }
              if (SwapIfValid(i, -1, 0)) { break; }
              if (SwapIfValid(i, +1, size - 1)) { break; }
              currentlyInteracting = true;
          }
      }
  }


  private void OnPieceDeselected(SelectExitEventArgs args) {
      currentlyInteracting = false;
  }

  // colCheck is used to stop horizontal moves wrapping.
  private bool SwapIfValid(int i, int offset, int colCheck) {
    // Ensure the swap does not wrap around horizontally
    if (((i % size) != colCheck) && ((i + offset) == emptyLocation)) {
        // Swap in game state
        (pieces[i], pieces[i + offset]) = (pieces[i + offset], pieces[i]);

        // Swap transforms
        (pieces[i].localPosition, pieces[i + offset].localPosition) = (pieces[i + offset].localPosition, pieces[i].localPosition);

        // Update empty location
        emptyLocation = i;
        return true;
    }

    return false;
}


  // We name the pieces in order so we can use this to check completion.
  private bool CheckCompletion() {
    if (pieces.Count - 1 != emptyLocation) {
        return false;
    }

    for (int i = 0; i < pieces.Count; i++) {
        if (pieces[i].name != $"{i}") {
          return false;
        }
    }

    return true;
  }

  private IEnumerator WaitShuffle(float duration) {
    yield return new WaitForSeconds(duration);
    Shuffle(31);
    shuffling = false;
  }

  // Brute force shuffling.
  private void Shuffle(int shuffleAmount) {
    int count = 0;
    int last = 0;
    while (count < shuffleAmount) {
      // Pick a random location.
      int rnd = Random.Range(0, size * size);
      // Only thing we forbid is undoing the last move.
      if (rnd == last) { continue; }
      last = emptyLocation;
      // Try surrounding spaces looking for valid move.
      if (SwapIfValid(rnd, -size, size)) {
        count++;
      } else if (SwapIfValid(rnd, +size, size)) {
        count++;
      } else if (SwapIfValid(rnd, -1, 0)) {
        count++;
      } else if (SwapIfValid(rnd, +1, size - 1)) {
        count++;
      }
    }
  }
}


