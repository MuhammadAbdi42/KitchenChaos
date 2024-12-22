using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CuttingCounter : BaseCounter, IKitchenObjectParent
{
    public static event EventHandler OnAnyCut;
    [SerializeField] CuttingRecipeSO[] cuttingRecipeSOArray;
    public event EventHandler<OnProgressChangedEventArg> OnProgressChanged;
    public event EventHandler OnCut;
    public class OnProgressChangedEventArg : EventArgs
    {
        public float progressNormalized;
        public bool canBeCut;
        public bool isCut;
        public bool isEmpty;
    }
    private void Start()
    {
        HandleEvent();
    }
    private void HandleEvent()
    {
        float tempProgressNormalized;
        bool tempIsEmpty, tempIsCut, tempCanBeCut;

        if (GetKitchenObject() == null)
        {
            tempIsEmpty = true;

            tempProgressNormalized = 0f;
            tempIsCut = false;
            tempCanBeCut = false;
        }
        else
        {
            tempIsEmpty = false;

            if (IsCut(GetKitchenObject().GetKitchenObjectSO()))
            {
                tempIsCut = true;

                tempProgressNormalized = 0f;
                tempCanBeCut = false;
            }
            else
            {
                tempIsCut = false;

                if (CanBeCut(GetKitchenObject().GetKitchenObjectSO()))
                {
                    tempCanBeCut = true;

                    tempProgressNormalized = (float)GetKitchenObject().cuttingProgress / (float)GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO()).cuttingProgressMax;
                }
                else
                {
                    tempCanBeCut = false;

                    tempProgressNormalized = 0f;
                }
            }
        }
        OnProgressChanged?.Invoke(this, new OnProgressChangedEventArg
        {
            progressNormalized = tempProgressNormalized,
            canBeCut = tempCanBeCut,
            isCut = tempIsCut,
            isEmpty = tempIsEmpty
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
        HandleEvent();
    }
    public void SwappingItems(Player player)
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
    public override void InteractingAlternate(Player player)
    {
        if (GetKitchenObject() != null && CanBeCut(GetKitchenObject().GetKitchenObjectSO()))
        {
            if (HasKitchenObject() && CanBeCut(GetKitchenObject().GetKitchenObjectSO()))
            {
                GetKitchenObject().cuttingProgress++;
                int cuttingRecipeSOMax = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO()).cuttingProgressMax;
                GetKitchenObject().cuttingProgressMax = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO()).cuttingProgressMax;
                HandleEvent();
                OnCut?.Invoke(this, EventArgs.Empty);
                OnAnyCut?.Invoke(this, EventArgs.Empty);
                if (GetKitchenObject().cuttingProgress >= GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO()).cuttingProgressMax)
                {
                    KitchenObjectSO OutputkitchenObjectSO = InputToOutputKithcenObjectRecipe(GetKitchenObject().GetKitchenObjectSO());
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(OutputkitchenObjectSO, this, true);
                    HandleEvent();
                }
            }
        }
    }
    private bool CanBeCut(KitchenObjectSO InputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(InputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }
    private KitchenObjectSO InputToOutputKithcenObjectRecipe(KitchenObjectSO InputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(InputKitchenObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }
    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO InputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == InputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
    private bool IsCut(KitchenObjectSO InputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.output == InputKitchenObjectSO)
            {
                return true;
            }
        }
        return false;
    }
}
