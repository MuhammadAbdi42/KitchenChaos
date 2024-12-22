using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public event EventHandler OnParentChange;
    private IKitchenObjectParent kitchenObjectParent;
    public int cuttingProgress;
    public int cuttingProgressMax;
    public float fryingProgress;
    public float fryingProgressMax;
    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }
    private void Start()
    {
        cuttingProgress = 0;
        cuttingProgressMax = 0;
        fryingProgress = 0f;
        fryingProgressMax = 0f;
    }
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent, bool noSound = false)
    {
        if (this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }
        this.kitchenObjectParent = kitchenObjectParent;
        if (kitchenObjectParent.HasKitchenObject() == true)
        {
            Debug.LogError("kitchenObjectParent already has a kitchen object");
        }
        kitchenObjectParent.SetKitchenObject(this, noSound);
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
        OnParentChange?.Invoke(this, EventArgs.Empty);
    }
    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent, bool noSound = false)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject spawnedKitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        spawnedKitchenObject.SetKitchenObjectParent(kitchenObjectParent, noSound);
        return spawnedKitchenObject;
    }
}
