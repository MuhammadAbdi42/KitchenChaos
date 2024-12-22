using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject particlesGameObject;
    [SerializeField] private StoveCounter stoveCounter;
    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }
    private void StoveCounter_OnProgressChanged(object sender, StoveCounter.OnProgressChangedEventArg e)
    {
        if (e.isEmpty == false)
        {
            if (e.canBeCooked == true)
            {
                stoveOnGameObject.SetActive(true);
                particlesGameObject.SetActive(true);
            }
            else
            {
                stoveOnGameObject.SetActive(false);
                particlesGameObject.SetActive(false);
            }
        }
        else
        {
            stoveOnGameObject.SetActive(false);
            particlesGameObject.SetActive(false);
        }
    }
}
