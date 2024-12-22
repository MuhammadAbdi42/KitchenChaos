using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;
    [SerializeField] private UnityEngine.UI.Image timerClock;
    private float recipeTimeMax;
    private float recipeTime;
    public void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }
    public void SetRecipeSO(RecipeSO recipeSO, float timer)
    {
        recipeTimeMax = recipeSO.timer;
        recipeTime = timer;
        recipeNameText.text = recipeSO.recipeName;
        timerClock.fillAmount = recipeTime / recipeTimeMax;

        foreach (Transform child in iconContainer)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList)
        {
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<UnityEngine.UI.Image>().sprite = kitchenObjectSO.sprite;
        }
    }
    public void Update()
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            recipeTime -= Time.deltaTime;
        }
        timerClock.fillAmount = recipeTime / recipeTimeMax;
    }
}
