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
    public Image sprite_;
    public bool setSelectedColor_ = true;
    public Color selectedColor_;
    Color unselectedColor_;

    public EResourceType EResourceType_;
    public UnityEvent<EResourceType> resourceSelected_;
    public UnityEvent<EResourceType> resourceUnselected_;
    bool selected_;

    void Start()
    {
        unselectedColor_ = button_.colors.normalColor;
        selected_ = false;
    }

    void SelectResource()
    {
        if(setSelectedColor_){
            var newColors = button_.colors;
            newColors.normalColor = selectedColor_;
            button_.colors = newColors;
        }

        resourceSelected_.Invoke(EResourceType_);

    }

    void UnselectResource()
    {
        if (setSelectedColor_)
        {
            var newColors = button_.colors;
            newColors.normalColor = unselectedColor_;
            button_.colors = newColors;
        }


        resourceUnselected_.Invoke(EResourceType_);
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
