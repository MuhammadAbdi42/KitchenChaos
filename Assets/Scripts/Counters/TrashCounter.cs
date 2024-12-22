using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler onAnyObjectTrashed;
    public override void Interacting(Player player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            onAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}
