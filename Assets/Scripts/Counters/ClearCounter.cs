using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectParent
{
    public override void Interacting(Player player)
    {
        if (!HasKitchenObject())
        {
            //Counter has no object on it
            if (player.HasKitchenObject())
            {
                //Player has a kitchen object and counter doesn't (giving object to the counter)
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
}
