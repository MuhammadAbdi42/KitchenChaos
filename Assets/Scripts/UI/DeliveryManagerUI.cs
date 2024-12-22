using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;
    [SerializeField] private Transform title;
    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        DeliveryManager.Instance.onRecipeSpawned += DeliveryManager_OnRecipeSpawned;
        DeliveryManager.Instance.onRecipeCompleted += DeliveryManager_OnRecipeCompleted;
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeCompleted(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeSpawned(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (DeliveryManager.Instance.GetWaitingRecipeSOList().Count == 0)
        {
            title.gameObject.SetActive(false);
        }
        else
        {
            title.gameObject.SetActive(true);

        }
        foreach (Transform child in container)
        {
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }
        List<RecipeSO> recipeSOList = DeliveryManager.Instance.GetWaitingRecipeSOList();
        List<float> recipeSOListTimer = DeliveryManager.Instance.GetWaitingRecipeSOListTimer();
        for (int i = 0; i < recipeSOList.Count; i++)
        {
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSOList[i], recipeSOListTimer[i]);
        }

    }
}
