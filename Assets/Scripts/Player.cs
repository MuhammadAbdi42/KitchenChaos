using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }
    //Universal Variables
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayer;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter; //Keeping the track of the counter we are interacting
    private KitchenObject kitchenObject;
    public event EventHandler onPickedSomething;
    float footStepTimer = 0;
    float footStepTimerMax = .1f;
    //IsWalking boolean
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player");
        }
        Instance = this;
    }
    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) //Doing the Interacting function if the interacting key is pressed
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            if (selectedCounter != null)
            {
                selectedCounter.Interacting(this);
            }
        }
    }
    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e) //Doing the Interacting function if the alternate interacting key is pressed
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            if (selectedCounter != null)
            {
                selectedCounter.InteractingAlternate(this);
            }
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteraction();

        footStepTimer += Time.deltaTime;
        if (footStepTimer >= footStepTimerMax)
        {
            FootstepSound();
            footStepTimer = 0;
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }
    private void FootstepSound()
    {
        if (IsWalking())
        {
            float volume = 1f;
            SoundManager.Instance.PlayWalkingSound(transform.position, volume);
        }
    }
    private void HandleInteraction()
    {
        //Geting Player Movement
        Vector2 inputVector = gameInput.GetMovementVectorNormalized(); //Calling Inputs through GameInput.cs
        Vector3 moveDir = new(inputVector.x, 0, inputVector.y); //Transforming Inputs into Vector3
        if (moveDir != Vector3.zero)
        { //Keeping the trace of interaction while not moving
            lastInteractDir = moveDir;
        }

        //Detecting Interaction via Raycast
        float maxDistanceInteraction = 2f; //Max Distance for Interacting
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, maxDistanceInteraction, countersLayer))
        { //Raycast for Interacting with objects
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (selectedCounter != baseCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedCounter });
    }
    private void HandleMovement()
    {
        //Geting Player Movement
        Vector2 inputVector = gameInput.GetMovementVectorNormalized(); //Calling Inputs through GameInput.cs
        Vector3 moveDir = new(inputVector.x, 0, inputVector.y); //Transforming Inputs into Vector3

        //Changing Animation State Via IsWalking bool
        isWalking = moveDir != Vector3.zero;

        //Rotating Character
        float moveSpeed = 5f; //Player Move Speed Multiplier
        float rotationSpeed = 15f; //Player Rotation Speed Multiplier
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed); //Slerp Function to Rotate Character

        //Collision Detector
        float playerHeight = 2f; //Player Collison Capsule Height Size
        float playerRadius = 0.7f; //Player Collision Capsule Radius
        float moveDistance = moveSpeed * Time.deltaTime; //Player Collision Max Distance
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance); //Defiening canMove Value to Check If The Player is Colliding
        if (!canMove) //Conditions to Check If The Player Can Still Move in Some Directions While Colliding
        {
            //Cannot move towards MoveDir
            //Attempt to Move only X
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                //Can only move X
                moveDir = moveDirX;
            }
            else
            {
                //Cannot Move on X
                //Attempt to move only Z    \
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    moveDir = moveDirZ;
                }
                else
                {
                    //Can't move at all
                }
            }
        }
        //Condition If Player is Not Colliding At All
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

    }
    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject, bool noSound = false)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            onPickedSomething?.Invoke(this, EventArgs.Empty);
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