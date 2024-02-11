using UnityEngine;
using TMPro;

public class NumberCubeGenerator : MonoBehaviour
{
    public TMP_InputField inputField; // Reference to the TMP Input Field
    public GameObject cubePrefab; // Reference to the cube prefab
    public float spacing = 0.5f; // Spacing between cubes
    public Color textColor = Color.white;
    public Color swapColor = Color.green; // Color to indicate swapping
    public float fontSize = 1f; // Font size for input numbers

    private GameObject[] cubes; // Array to store references to the generated cubes
    private bool sortingInProgress = false; // Flag to track if sorting is in progress

    public void GenerateCubes()
    {
        if (sortingInProgress)
        {
            // If sorting is already in progress, ignore the button click
            return;
        }

        sortingInProgress = true; // Set flag to indicate sorting is in progress

        if (cubes != null)
        {
            // Clean up previously generated cubes
            foreach (GameObject cube in cubes)
            {
                Destroy(cube);
            }
        }

        string Nos = inputField.text;
        string[] numbers = Nos.Split(',');

        Debug.Log("Number of elements in numbers array: " + numbers.Length); // Debug print

        // Calculate total width
        float totalWidth = (numbers.Length - 1) * spacing;

        // Calculate starting position
        float startX = -totalWidth / 2f;

        // Initialize currentX to starting position
        float currentX = startX;

        cubes = new GameObject[numbers.Length]; // Initialize the array to store cube references

        for (int i = 0; i < numbers.Length; i++)
        {
            // Use the current position for each cube
            Vector3 cubePosition = new Vector3(currentX, 0f, 0f);

            Debug.Log("Position of cube " + (i + 1) + ": " + cubePosition); // Debug print

            GameObject cube = Instantiate(cubePrefab, cubePosition, Quaternion.identity);

            // Update currentX for the next cube
            currentX += spacing * 2; // Double the spacing to ensure even spacing

            cubes[i] = cube; // Store reference to the cube in the array

            // Access the TextMeshPro component inside the canvas of the cube prefab and update its text
            Canvas canvas = cube.GetComponentInChildren<Canvas>();
            if (canvas != null)
            {
                TextMeshProUGUI textMesh = canvas.GetComponentInChildren<TextMeshProUGUI>();
                if (textMesh != null)
                {
                    textMesh.text = numbers[i];
                    textMesh.color = textColor;
                    textMesh.alignment = TextAlignmentOptions.Center;

                    // Set font size based on cube size
                    float cubeSize = 24.2f; // Adjust this value based on your cube size
                    float fontSizeMultiplier = 0.05f; // Adjust this multiplier as needed
                    textMesh.fontSize = Mathf.RoundToInt(cubeSize * fontSizeMultiplier);
                }
                else
                {
                    Debug.LogError("TextMeshProUGUI component not found in the canvas of the cube prefab.");
                }
            }
            else
            {
                Debug.LogError("Canvas component not found in the children of the cube prefab.");
            }
        }

        // Start the bubble sort coroutine
        StartCoroutine(BubbleSortCubes(numbers));
    }

    private System.Collections.IEnumerator BubbleSortCubes(string[] numbers)
    {
        for (int i = 0; i < numbers.Length - 1; i++)
        {
            for (int j = 0; j < numbers.Length - i - 1; j++)
            {
                float currentNumber, nextNumber;
                if (float.TryParse(numbers[j], out currentNumber) &&
                    float.TryParse(numbers[j + 1], out nextNumber))
                {
                    if (currentNumber > nextNumber)
                    {
                        // Swap positions of cubes
                        Vector3 tempPosition = cubes[j].transform.position;
                        cubes[j].transform.position = cubes[j + 1].transform.position;
                        cubes[j + 1].transform.position = tempPosition;

                        // Swap references in the cubes array
                        GameObject tempCube = cubes[j];
                        cubes[j] = cubes[j + 1];
                        cubes[j + 1] = tempCube;

                        // Change color of swapped cubes
                        Renderer renderer1 = cubes[j].GetComponent<Renderer>();
                        Renderer renderer2 = cubes[j + 1].GetComponent<Renderer>();
                        if (renderer1 != null && renderer2 != null)
                        {
                            renderer1.material.color = swapColor;
                            renderer2.material.color = swapColor;
                        }
                        else
                        {
                            Debug.LogError("Renderer component not found in one of the cubes.");
                        }

                        yield return new WaitForSeconds(0.5f); // Wait for a short duration to visualize the swap
                    }
                }
            }
        }

        sortingInProgress = false; // Set flag to indicate sorting is complete
    }
}