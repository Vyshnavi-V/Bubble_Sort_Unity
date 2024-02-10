using UnityEngine;
using TMPro;

public class NumberCubeGenerator : MonoBehaviour
{
    public TMP_InputField inputField; // Reference to the TMP Input Field
    public GameObject cubePrefab; // Reference to the cube prefab
    public float spacing = 0.5f; // Spacing between cubes
    public Color textColor = Color.white; 
     public float fontSize = 8f; // Font size for input numbers
    private void Start()
    {
        inputField.onEndEdit.AddListener(GenerateCubes);
    }

public void GenerateCubes(string input)
{
    string[] numbers = input.Split(',');

    Debug.Log("Number of elements in numbers array: " + numbers.Length); // Debug print

    // Calculate total width
    float totalWidth = (numbers.Length - 1) * spacing;

    // Calculate starting position
    float startX = -totalWidth / 2f;

    // Initialize currentX to starting position
    float currentX = startX;

    for (int i = 0; i < numbers.Length; i++)
    {
        // Use the current position for each cube
        Vector3 cubePosition = new Vector3(currentX, 0f, 0f);

        Debug.Log("Position of cube " + (i+1) + ": " + cubePosition); // Debug print

        GameObject cube = Instantiate(cubePrefab, cubePosition, Quaternion.identity);
        cube.GetComponent<Renderer>().material.color = Color.black;

        // Update currentX for the next cube
        currentX += spacing * 2; // Double the spacing to ensure even spacing

        // Instantiate TextMeshPro object as child of the cube
            GameObject textObject = new GameObject("Text");
            textObject.transform.parent = cube.transform;
            textObject.transform.localPosition = Vector3.zero;

            // Add TextMeshPro component
            TextMeshPro textMesh = textObject.AddComponent<TextMeshPro>();
            textMesh.text = numbers[i];
            textMesh.color = textColor;
            textMesh.alignment = TextAlignmentOptions.Center;
             // Set font size
            textMesh.fontSize = fontSize;
             // Calculate text size and position it in the center of the cube
            Bounds cubeBounds = cube.GetComponent<Renderer>().bounds;
            Bounds textBounds = textMesh.bounds;

            textObject.transform.localPosition = new Vector3(
                0f,
                cubeBounds.extents.y - textBounds.extents.y, // Offset by half the height of cube and text
                0f
            );
    }

}




}