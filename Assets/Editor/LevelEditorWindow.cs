using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LevelEditorWindow : EditorWindow
{
    private LevelObstacles currentLevel; // Reference to the current level being edited
    private int moveNumber = 10;
    private List<Grid.PiecePosition> initialPieces = new List<Grid.PiecePosition>();

    [MenuItem("Window/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditorWindow>("Level Editor");
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);

        // Allow the user to assign the current LevelObstacles object to edit
        currentLevel = (LevelObstacles)EditorGUILayout.ObjectField("Level to Edit", currentLevel, typeof(LevelObstacles), true);

        // Input fields for grid dimensions
        if (currentLevel != null && currentLevel.grid != null)
        {
            EditorGUILayout.LabelField("Grid Dimensions", EditorStyles.boldLabel);
            currentLevel.grid.xDim = EditorGUILayout.IntField("Width (X Dimension)", currentLevel.grid.xDim);
            currentLevel.grid.yDim = EditorGUILayout.IntField("Height (Y Dimension)", currentLevel.grid.yDim);
        }

        moveNumber = EditorGUILayout.IntField("Move Number", moveNumber);

        if (GUILayout.Button("Add Initial Piece"))
        {
            initialPieces.Add(new Grid.PiecePosition());
        }

        // Iterate over initialPieces list to display and modify each Grid.PiecePosition
        for (int i = 0; i < initialPieces.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            // Use a local copy to edit properties
            Grid.PiecePosition piecePosition = initialPieces[i];

            // Display and edit the type of the piece
            piecePosition.type = (Grid.PieceType)EditorGUILayout.EnumPopup("Type", piecePosition.type);

            // IMPORTANT: Ensure that the EditorGUILayout.IntField lines are not inside a conditional that could prevent them from executing
            // Display and edit the x-coordinate of the piece
            piecePosition.x = EditorGUILayout.IntField("X", piecePosition.x);

            // Display and edit the y-coordinate of the piece
            piecePosition.y = EditorGUILayout.IntField("Y", piecePosition.y);
            if (piecePosition.type == Grid.PieceType.NORMAL)
            {
                piecePosition.color = (ColorPiece.ColorType)EditorGUILayout.EnumPopup("Color", piecePosition.color);
            }
            else
            {
                piecePosition.color = ColorPiece.ColorType.COUNT;
            }

            // After editing, reassign the modified copy back to the list
            initialPieces[i] = piecePosition;

            // Provide a button to remove this piece from the list
            if (GUILayout.Button("Remove"))
            {
                initialPieces.RemoveAt(i);
                i--; // Decrement the counter to correctly continue the loop
            }

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Apply Configuration"))
        {
            ApplyConfiguration();
        }
    }

    private void ApplyConfiguration()
    {
        if (currentLevel == null)
        {
            Debug.LogError("No level selected for editing.");
            return;
        }

        if (currentLevel.grid == null)
        {
            Debug.LogError("Selected level does not have a grid component.");
            return;
        }

        // Apply the move number to the level
        currentLevel.numMoves = moveNumber;

        // Directly manipulate the initialPieces array if it's publicly accessible
        currentLevel.grid.initialPieces = initialPieces.ToArray();

        Debug.Log("Level configuration applied.");
        EditorUtility.SetDirty(currentLevel); // Mark the level object as dirty to ensure changes are saved
    }
}
