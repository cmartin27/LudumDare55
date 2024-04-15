using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIIngredientInfo : MonoBehaviour
{
    [SerializeField]
    Image sprite_;

    [SerializeField]
    TMP_Text info_;

    public void SetIngredientInfo(Sprite sprite, string text)
    {
        sprite_.sprite = sprite;
        info_.text = text;
    }

}
