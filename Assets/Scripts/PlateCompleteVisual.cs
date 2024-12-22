using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObejct
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }
    [SerializeField] PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObejct> kitchenObjectSOGameObejctsList;
    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
        foreach (KitchenObjectSO_GameObejct kitchenObjectSOGameObejct in kitchenObjectSOGameObejctsList)
        {
            kitchenObjectSOGameObejct.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.onIngredientAddedEventArgs e)
    {
        foreach (KitchenObjectSO_GameObejct kitchenObjectSOGameObejct in kitchenObjectSOGameObejctsList)
        {
            if (kitchenObjectSOGameObejct.kitchenObjectSO == e.kitchenObjectSO)
            {
                kitchenObjectSOGameObejct.gameObject.SetActive(true);
            }
        }
    }
}
