using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICookbookMenu : MonoBehaviour
{

    public GameObject recipePrefab_;
    public GameObject ingredientPrefab_;

    public GameObject recipeList_;
    public GameObject ingredientsList_;

    public void CreateMenu(Button cookbookButton)
    {
        var summonings = GameManager.Instance.summoningManager_.summonings_;

        List<Button> buttonList = new List<Button>();

        foreach (var item in summonings)
        {
            GameObject summon = Instantiate(recipePrefab_, recipeList_.transform);

            TMP_Text summonText = summon.GetComponentInChildren<TMP_Text>();
            summonText.text = item.description_;

            Button summonButton = summon.GetComponent<Button>();
            int id = buttonList.Count;
            summonButton.onClick.AddListener(() => SelectRecipe(id));

            buttonList.Add(summonButton);
        }

        // Set buttons navigation
        int objectsCount = buttonList.Count;

        for (int i = 0; i < objectsCount; ++i)
        {
            var navigation = buttonList[i].navigation;

            navigation.selectOnLeft = cookbookButton;

            int upButtonIdx = (i + objectsCount - 1) % objectsCount;
            navigation.selectOnUp = buttonList[upButtonIdx];

            int downButtonIdx = (i + 1) % objectsCount;
            navigation.selectOnDown = buttonList[downButtonIdx];

            buttonList[i].navigation = navigation;
        }
    }

    public void SelectRecipe(int id)
    {
        // Clean previous list
        foreach (Transform child in ingredientsList_.transform)
        {
            Destroy(child.gameObject);
        }

        var ingredients = GameManager.Instance.summoningManager_.summonings_[id].resources_;

        int i = 0;
        foreach (var item in ingredients)
        {
            GameObject ingredient = Instantiate(ingredientPrefab_, ingredientsList_.transform);
            UIIngredientInfo ingredientInfo = ingredient.GetComponent<UIIngredientInfo>();

            Sprite ingredientSprite = GameManager.Instance.resourceManager_.GetResourceSprite(item);

            uint ingredientTotalAmount = GameManager.Instance.summoningManager_.summonings_[id].resourceAmounts_[i];
            int currentAmount = GameManager.Instance.inventoryManager_.GetResourceAmount(item);

            string description = currentAmount.ToString() + " / " + ingredientTotalAmount;

            ingredientInfo.SetIngredientInfo(ingredientSprite, description);

            ++i;
        }
    }


    public void OpenMenu()
    {
        recipeList_.transform.GetChild(0).GetComponent<Button>().Select();
        recipeList_.transform.GetChild(0).GetComponent<Button>().onClick.Invoke();

    }

}
