using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    private List<KitchenObjectSO> kitchenObjectSOList;
    public event EventHandler<onIngredientAddedEventArgs> OnIngredientAdded;
    public static event EventHandler OnIngredientDroppedOn;
    public static event EventHandler OnIngredientPickedUp;
    public class onIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;
    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (kitchenObjectSOList.Contains(kitchenObjectSO) || !validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        else
        {
            if (kitchenObjectSO == validKitchenObjectSOList[0] || kitchenObjectSO == validKitchenObjectSOList[1])
            {
                if (kitchenObjectSOList.Contains(validKitchenObjectSOList[0]) || kitchenObjectSOList.Contains(validKitchenObjectSOList[1]))
                {
                    return false;
                }
            }
            kitchenObjectSOList.Add(kitchenObjectSO);
            OnIngredientAdded?.Invoke(this, new onIngredientAddedEventArgs
            {
                kitchenObjectSO = kitchenObjectSO
            });
            if (GetKitchenObjectParent() is Player)
            {
                OnIngredientPickedUp?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                OnIngredientDroppedOn?.Invoke(this, EventArgs.Empty);
            }
            return true;
        }
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
    public bool IsThisObjectValid(KitchenObjectSO kitchenObjectSO)
    {
        foreach (KitchenObjectSO tempKitchenObjectSO in validKitchenObjectSOList)
        {
            if (tempKitchenObjectSO == kitchenObjectSO)
            {
                return true;
            }
        }
        return false;
    }
}
