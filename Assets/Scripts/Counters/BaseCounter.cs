using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler onAnyObjectPlacedHere;
    private KitchenObject kitchenObject;
    [SerializeField] private Transform counterTopPoint;
    public virtual void Interacting(Player player)
    {
        Debug.Log("BaseCounter.InteractingAlternate();");
    }
    public virtual void InteractingAlternate(Player player)
    {
        Debug.Log("BaseCounter.InteractingAlternate();");
    }
    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject, bool noSound = false)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null && noSound == false)
        {
            onAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
