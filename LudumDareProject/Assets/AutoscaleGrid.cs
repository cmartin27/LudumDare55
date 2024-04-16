using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoscaleGrid : MonoBehaviour
{

    Vector2 baseSize = new Vector2(1920, 1080); // Base size of the screen
    Vector2 baseCellSize; // In editor Cell Size for GridLayoutComponent
    Vector2 baseCellSpacing; // In editor Cell Spacing for GridLayoutComponent
    GridLayoutGroup layoutGroup; //Component
    void Start()
    {
        layoutGroup = GetComponent<GridLayoutGroup>();
        baseCellSize = layoutGroup.cellSize;
        baseCellSpacing = layoutGroup.spacing;

        UpdateSize();
    }

    void UpdateSize()
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height); // Current screen size
        layoutGroup.cellSize = (screenSize / baseSize) * baseCellSize;
        layoutGroup.spacing = (screenSize / baseSize) * baseCellSpacing;
    }
}
