using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClipRefsSO audioClipRefsSO;
    public static SoundManager Instance { get; private set; }
    public void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        DeliveryManager.Instance.onRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.onRecipeFailed += DeliveryManager_OnRecipeFailed;
        Player.Instance.onPickedSomething += Player_OnPickedSomething;
        BaseCounter.onAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.onAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;

        PlateKitchenObject.OnIngredientPickedUp += PlateKitchenObject_OnIngredientPickedUp;
        PlateKitchenObject.OnIngredientDroppedOn += PlateKitchenObject_OnIngredientDroppedOn;
    }

    private void PlateKitchenObject_OnIngredientDroppedOn(object sender, EventArgs e)
    {
        PlateKitchenObject plateKitchenObject = sender as PlateKitchenObject;
        PlaySound(audioClipRefsSO.objectDrop, plateKitchenObject.transform.position);
    }

    private void PlateKitchenObject_OnIngredientPickedUp(object sender, EventArgs e)
    {
        PlateKitchenObject plateKitchenObject = sender as PlateKitchenObject;
        PlaySound(audioClipRefsSO.objectPickup, plateKitchenObject.transform.position);
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPickedSomething(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliveryFail, deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliverySuccess, deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 vector3, float volume = 1f)
    {

        AudioSource.PlayClipAtPoint(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], vector3, volume);
    }

    public void PlayWalkingSound(Vector3 vector3, float volume = 1f)
    {

        AudioSource.PlayClipAtPoint(audioClipRefsSO.footstep[UnityEngine.Random.Range(0, audioClipRefsSO.footstep.Length)], vector3, volume);
    }
}
