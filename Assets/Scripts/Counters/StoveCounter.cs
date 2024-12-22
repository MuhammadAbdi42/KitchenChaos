using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoveCounter : BaseCounter, IKitchenObjectParent
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    public event EventHandler<OnProgressChangedEventArg> OnProgressChanged;
    public class OnProgressChangedEventArg : EventArgs
    {
        public float progressNormalized;
        public bool canBeCooked;
        public bool isBurned;
        public bool isEmpty;
        public bool isBurning;
    }
    private FryingRecipeSO fryingRecipeSO;
    private void Start()
    {
        HandleEvent();
    }
    private void HandleEvent()
    {
        float tempProgressNormalized;
        bool tempIsEmpty, tempIsBurned, tempCanBeCooked, tempIsBrurning;

        if (GetKitchenObject() == null)
        {
            tempIsEmpty = true;

            tempProgressNormalized = 0f;
            tempIsBurned = false;
            tempCanBeCooked = false;
            tempIsBrurning = false;
        }
        else
        {
            tempIsEmpty = false;

            if (IsBurned(GetKitchenObject().GetKitchenObjectSO()))
            {
                tempIsBurned = true;

                tempProgressNormalized = 0f;
                tempCanBeCooked = false;
                tempIsBrurning = false;
            }
            else
            {
                tempIsBurned = false;

                if (CanBeCooked(GetKitchenObject().GetKitchenObjectSO()))
                {
                    tempCanBeCooked = true;

                    tempProgressNormalized = (float)GetKitchenObject().fryingProgress / (float)GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO()).fryingTimerMax;
                    if (!CanBeCooked(GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO()).output))
                    {
                        tempIsBrurning = true;
                    }
                    else
                    {
                        tempIsBrurning = false;
                    }
                }
                else
                {
                    tempCanBeCooked = false;
                    tempIsBrurning = false;

                    tempProgressNormalized = 0f;
                }
            }
        }
        OnProgressChanged?.Invoke(this, new OnProgressChangedEventArg
        {
            progressNormalized = tempProgressNormalized,
            canBeCooked = tempCanBeCooked,
            isBurned = tempIsBurned,
            isEmpty = tempIsEmpty,
            isBurning = tempIsBrurning
        });
    }
    public override void Interacting(Player player)
    {
        if (!HasKitchenObject())
        {
            //Counter has no object on it
            if (player.HasKitchenObject())
            {
                //Player has a kitchen object and counter doesn't (giving object to the counter)
                //Also Checking if the kitchen Object is cuttable
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                //Player and counter have no object (doing nothing)
            }
        }
        else
        {
            //Counter has an object
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                    else
                    {
                        SwappingItems(player);
                    }
                }
                else
                {
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                        else
                        {
                            SwappingItems(player);
                        }
                    }
                    else
                    {
                        SwappingItems(player);
                    }

                }
            }
            else
            {
                //Counter has a kitchen object and player doesn't (giving object to the player)
                this.GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
        if (HasKitchenObject())
        {
            if (CanBeCooked(GetKitchenObject().GetKitchenObjectSO()))
            {
                fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            }
        }
        HandleEvent();
    }
    private void SwappingItems(Player player)
    {
        //Player and counter have an object (swaping the objects)
        KitchenObject tempKitchenObjectPlayerToCounter, tempKitchenObjectCounterToPlayer;
        tempKitchenObjectCounterToPlayer = GetKitchenObject();
        tempKitchenObjectPlayerToCounter = player.GetKitchenObject();

        this.ClearKitchenObject();
        player.ClearKitchenObject();

        tempKitchenObjectCounterToPlayer.SetKitchenObjectParent(player);
        tempKitchenObjectPlayerToCounter.SetKitchenObjectParent(this);

        this.SetKitchenObject(tempKitchenObjectPlayerToCounter);
        player.SetKitchenObject(tempKitchenObjectCounterToPlayer);
    }
    private void Update()
    {
        if (HasKitchenObject())
        {
            if (CanBeCooked(GetKitchenObject().GetKitchenObjectSO()))
            {
                GetKitchenObject().fryingProgress += Time.deltaTime;
                GetKitchenObject().fryingProgressMax = fryingRecipeSO.fryingTimerMax;
                if (GetKitchenObject().fryingProgress > fryingRecipeSO.fryingTimerMax)
                {
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this, true);
                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                }
                HandleEvent();
            }
        }
    }
    private bool CanBeCooked(KitchenObjectSO InputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(InputKitchenObjectSO);
        return fryingRecipeSO != null;
    }
    private KitchenObjectSO InputToOutputKithcenObjectRecipe(KitchenObjectSO InputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(InputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }
    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO InputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == InputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }
    private bool IsBurned(KitchenObjectSO InputKitchenObjectSO)
    {
        if (fryingRecipeSOArray[1].output == InputKitchenObjectSO)
        {
            return true;
        }
        return false;
    }
}
