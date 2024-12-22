using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlatesCounter : BaseCounter, IKitchenObjectParent
{
    [SerializeField] KitchenObjectSO plateKitchenObjectSO;
    [SerializeField] PlateKitchenObject tempPlate;
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawnedAmount = 0;
    private int platesSpawnedAmountMax = 4;
    public EventHandler onPlatesSpwned;
    public EventHandler onPlatesRemoved;
    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;
            if (platesSpawnedAmount < platesSpawnedAmountMax)
            {
                platesSpawnedAmount++;
                onPlatesSpwned?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    public override void Interacting(Player player)
    {
        if (!player.HasKitchenObject())
        {
            //Player is empty handed
            if (platesSpawnedAmount > 0)
            {
                if (platesSpawnedAmount == platesSpawnedAmountMax)
                {
                    spawnPlateTimer = 0f;
                }
                platesSpawnedAmount--;
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                onPlatesRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {
            if (tempPlate.IsThisObjectValid(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                KitchenObjectSO tempKitchenObjectSO = player.GetKitchenObject().GetKitchenObjectSO();
                player.GetKitchenObject().DestroySelf();

                PlateKitchenObject secondTempPlate = Instantiate(tempPlate, transform);

                tempPlate.TryAddIngredient(tempKitchenObjectSO);
                tempPlate.SetKitchenObjectParent(player);

                tempPlate = secondTempPlate;

                platesSpawnedAmount--;
                onPlatesRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
