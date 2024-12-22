using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KitchenObjectProgressBar : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private KitchenObject kitchenObject;
    [SerializeField] private GameObject BarUI;

    private void Start()
    {
        kitchenObject.OnParentChange += KitchenObject_OnParentChange;
        HideBarUI();
    }

    private void KitchenObject_OnParentChange(object sender, EventArgs e)
    {
        const string CUTTINGCOUNTER = "CuttingCounter";
        const string STOVECOUNTER = "StoveCounter";
        if (kitchenObject.cuttingProgressMax != 0)
        {
            if (kitchenObject.cuttingProgress != 0 && !kitchenObject.transform.root.CompareTag(CUTTINGCOUNTER) && !kitchenObject.transform.root.CompareTag(STOVECOUNTER))
            {
                ShowBarUI();
                barImage.fillAmount = (float)kitchenObject.cuttingProgress / (float)kitchenObject.cuttingProgressMax;
            }
            else
            {
                HideBarUI();
            }
        }
        if (kitchenObject.fryingProgressMax != 0)
        {
            if (kitchenObject.fryingProgress != 0 && !kitchenObject.transform.root.CompareTag(CUTTINGCOUNTER) && !kitchenObject.transform.root.CompareTag(STOVECOUNTER))
            {
                ShowBarUI();
                barImage.fillAmount = (float)kitchenObject.fryingProgress / (float)kitchenObject.fryingProgressMax;
            }
            else
            {
                HideBarUI();
            }
        }
    }
    private void ShowBarUI()
    {
        BarUI.SetActive(true);
    }
    private void HideBarUI()
    {
        BarUI.SetActive(false);
    }

}
