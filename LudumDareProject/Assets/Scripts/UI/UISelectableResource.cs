using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UISelectableResource : MonoBehaviour
{
    public Button button_;
    public TMP_Text text_;

    public Color selectedColor_;
    Color unselectedColor_;

    public ResourceType resourceType_;
    public UnityEvent<ResourceType> resourceSelected_;
    public UnityEvent<ResourceType> resourceUnselected_;
    bool selected_;

    void Start()
    {
        unselectedColor_ = button_.colors.normalColor;
        selected_ = false;
    }

    void SelectResource()
    {
        var newColors = button_.colors;
        newColors.normalColor = selectedColor_;
        button_.colors = newColors;

        resourceSelected_.Invoke(resourceType_);

    }

    void UnselectResource()
    {
        var newColors = button_.colors;
        newColors.normalColor = unselectedColor_;
        button_.colors = newColors;

        resourceUnselected_.Invoke(resourceType_);
    }

    public void OnClickButton()
    {
        if (selected_) 
        {
            UnselectResource();
        }
        else
        {
            SelectResource();
        }

        selected_ = !selected_;
        
    }

    
}
