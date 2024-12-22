using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] CuttingCounter cuttingCounter;
    [SerializeField] StoveCounter stoveCounter;
    [SerializeField] private GameObject BarUI;
    [SerializeField] private GameObject BarUIColor;
    [SerializeField] private GameObject ForbiddonUI;
    [SerializeField] private GameObject DoneUI;

    private void Start()
    {
        if (cuttingCounter != null)
        {
            cuttingCounter.OnProgressChanged += CuttingCounter_OnProgressChanged;

        }
        if (stoveCounter != null)
        {
            stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
        }
        barImage.fillAmount = 0f;
        HideBarUI();
        HideForbiddonUI();
        HideDoneUI();
    }

    private void CuttingCounter_OnProgressChanged(object sender, CuttingCounter.OnProgressChangedEventArg e)
    {
        barImage.fillAmount = e.progressNormalized;
        HideBarUI();
        HideForbiddonUI();
        HideDoneUI();
        if (e.isEmpty == false)
        {
            if (e.canBeCut == true)
            {
                if (!(e.progressNormalized == 0f || e.progressNormalized == 1f))
                {
                    ShowBarUI();
                }
            }
            else
            {
                if (e.isCut)
                {
                    ShowDoneUI();
                }
                else
                {
                    ShowForbiddonUI();
                }
            }
        }
    }
    private void StoveCounter_OnProgressChanged(object sender, StoveCounter.OnProgressChangedEventArg e)
    {
        barImage.fillAmount = e.progressNormalized;
        HideBarUI();
        HideForbiddonUI();
        HideDoneUI();
        if (e.isEmpty == false)
        {
            if (e.canBeCooked == true)
            {
                if (!(e.progressNormalized == 0f || e.progressNormalized == 1f))
                {
                    ShowBarUI();
                    if (e.isBurning)
                    {
                        ShowForbiddonUI();
                        BarUIColor.GetComponent<Image>().color = new Color32(165, 000, 025, 255);
                    }
                    else
                    {
                        HideForbiddonUI();
                        BarUIColor.GetComponent<Image>().color = new Color32(255, 177, 000, 255);
                    }
                }
            }
            else
            {
                if (e.isBurned)
                {
                    ShowDoneUI();
                }
                else
                {
                    ShowForbiddonUI();
                }
            }
        }
    }
    private void ShowForbiddonUI()
    {
        ForbiddonUI.SetActive(true);
    }
    private void HideForbiddonUI()
    {
        ForbiddonUI.SetActive(false);
    }
    private void ShowDoneUI()
    {
        DoneUI.SetActive(true);
    }
    private void HideDoneUI()
    {
        DoneUI.SetActive(false);
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
