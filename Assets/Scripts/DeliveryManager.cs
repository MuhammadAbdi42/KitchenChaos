using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;
    private List<float> waitingRecipeSOListTimer;
    private float spawnRecipeTimer = 3f;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;
    private int successfulRecipesDelivered = 0;
    public event EventHandler onRecipeSpawned;
    public event EventHandler onRecipeCompleted;
    public event EventHandler onRecipeSuccess;
    public event EventHandler onRecipeFailed;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
        waitingRecipeSOListTimer = new List<float>();
    }

    private void Update()
    {
        spawnRecipeTimer += Time.deltaTime;
        if (spawnRecipeTimer >= spawnRecipeTimerMax)
        {
            spawnRecipeTimer = 0f;
            if (waitingRecipeSOList.Count < waitingRecipeMax && GameManager.Instance.IsGamePlaying())
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                waitingRecipeSOList.Add(waitingRecipeSO);
                waitingRecipeSOListTimer.Add(waitingRecipeSO.timer);
                onRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
        for (int i = 0; i < waitingRecipeSOListTimer.Count; i++)
        {
            if (GameManager.Instance.IsGamePlaying())
            {
                waitingRecipeSOListTimer[i] -= Time.deltaTime;
            }
            if (waitingRecipeSOListTimer[i] <= 0f)
            {
                GameManager.Instance.DeliveryFailedInTime();
                waitingRecipeSOList.RemoveAt(i);
                waitingRecipeSOListTimer.RemoveAt(i);
                onRecipeFailed?.Invoke(this, EventArgs.Empty);
            }
            Debug.Log(i.ToString() + " " + waitingRecipeSOListTimer[i]);
        }
    }

    public void DeliveryRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool plateContentsMatchTheRecipe = true;

                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        plateContentsMatchTheRecipe = false;
                    }
                }

                if (plateContentsMatchTheRecipe)
                {
                    waitingRecipeSOList.RemoveAt(i);
                    waitingRecipeSOListTimer.RemoveAt(i);
                    onRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    onRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    successfulRecipesDelivered++;
                    return;
                }
            }

        }
        onRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }
    public List<float> GetWaitingRecipeSOListTimer()
    {
        return waitingRecipeSOListTimer;
    }

    public int GetSuccessfulRecipesDelivered()
    {
        return successfulRecipesDelivered;
    }
}
