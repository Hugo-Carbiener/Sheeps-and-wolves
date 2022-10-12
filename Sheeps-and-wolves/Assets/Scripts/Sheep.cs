using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SheepState
{
    Idle,
    Fleeing,
    Focused
}

public class Sheep : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Transform player;
    [SerializeField] private SpriteManager spriteManager;

    [Header("Movement variables")]
    [SerializeField] private int playerFleeRange;
    [SerializeField] private int targetAngleSpan;
    [SerializeField] private float maxSpeed;
    private float currentSpeed;
    private Vector2 direction;

    [Header("Idle movement")]
    [SerializeField] private float minIdleMovementDuration;
    [SerializeField] private float maxIdleMovementDuration;
    [SerializeField] private float probabilityIncreaseRate;
    private bool isIdleMoving;
    private int currentRandomEvent;
    private float currentProbability;

    [Header("Enforced movement")]
    [SerializeField] private float destinationReachThreshold;

    private SheepState state;

    void Start()
    {
        if (!rigidBody) rigidBody = GetComponent<Rigidbody2D>();
        if (!player) player = GameObject.FindGameObjectWithTag("Player").transform;
        if (!spriteManager) spriteManager = GetComponentInChildren<SpriteManager>();

        isIdleMoving = false;
        state = SheepState.Idle;
        currentRandomEvent = Random.Range(0, 100);
        currentProbability = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // get movement data
        direction = Utils.Instance.GetDirectionFromAngle(transform.eulerAngles.z + 90);
        currentSpeed = rigidBody.velocity.magnitude;

        // if we do not enforce the sheeps movement
        if (state != SheepState.Focused)
        {
            // process sheep state 
            if ((transform.position - player.position).magnitude <= playerFleeRange && state != SheepState.Fleeing)
            {
                state = SheepState.Fleeing;
            }
            else if ((transform.position - player.position).magnitude > playerFleeRange)
            {
                // stops the sheep at the end of the flee
                if (state != SheepState.Idle)
                {
                    rigidBody.velocity = Vector3.zero;
                    currentSpeed = 0;
                }
                state = SheepState.Idle;
            }
        }

        switch(state)
        {
            case SheepState.Fleeing:

                // stop eventual idle movement
                StopCoroutine("idleMovement");

                float relativePlayerAngularPosition = getPlayerAngularPosition();
                Quaternion targetRotation = getFleeTarget(relativePlayerAngularPosition);

                // rotation
                transform.rotation = targetRotation;

                // go faster if not rotating
                if (transform.rotation.eulerAngles.z - targetRotation.eulerAngles.z <= targetAngleSpan)
                {
                    currentSpeed = maxSpeed;
                }
                else
                {
                    currentSpeed = maxSpeed / 2;
                }

                // apply speed
                rigidBody.velocity = direction * currentSpeed;
                break;

            case SheepState.Idle:
                if (!isIdleMoving)
                {
                    if (currentRandomEvent < currentProbability)
                    {
                        // play the idle movement
                        StartCoroutine("idleMovement");
                        currentProbability = 1;
                    } else {
                        currentProbability += probabilityIncreaseRate * Time.deltaTime;
                    }
                }
                break;
        }

        UpdateAnimations();
    }
    
    public void MoveTo(Vector2 target)
    {
        StartCoroutine(EnforceDeplacement(target));
    }

    public IEnumerator EnforceDeplacement(Vector2 target)
    {
        // set sheep state
        state = SheepState.Focused;
        currentSpeed = maxSpeed;
        
        Vector2 sheepPosition = transform.position;
        Vector2 movementDirection = target - sheepPosition;
        transform.rotation = Quaternion.Euler(0, 0, Utils.Instance.GetAngleFromVector(movementDirection));
        rigidBody.velocity = currentSpeed * movementDirection;

        while (Vector2.Distance(sheepPosition, target) >= destinationReachThreshold)
        {
            yield return null;
        }
        rigidBody.velocity = Vector2.zero;
        state = SheepState.Idle;
    }

    private void UpdateAnimations()
    {
        if (currentSpeed == 0)
        {
            spriteManager.startIdleEvent.Invoke();
        } else
        {
            spriteManager.startWalkEvent.Invoke();
        }
    }
   
    /**
     * Get the angular position of the player relative to the sheep (get the poulet)
     */
    private float getPlayerAngularPosition()
    {
        Vector2 playerDirection = player.position - transform.position;
        float relativePlayerAngularPosition = Mathf.Atan2(playerDirection.y, playerDirection.x) / Mathf.PI * 180;
        relativePlayerAngularPosition = Utils.Instance.normalise(relativePlayerAngularPosition);
        return relativePlayerAngularPosition;
    }

    /**
     * Get the angle pointing in the opposite direction from the player
     */
    private Quaternion getFleeTarget(float relativePlayerAngularPosition)
    {
        float targetAngle = relativePlayerAngularPosition + 180;
        targetAngle = Utils.Instance.normalise(targetAngle);
        return Quaternion.Euler(0, 0, targetAngle - 90);
    }

    private IEnumerator idleMovement()
    {
        isIdleMoving = true;

        int rdAngle = Random.Range(0, 360);
        transform.rotation = Quaternion.Euler(0, 0, rdAngle);
        direction = Utils.Instance.GetDirectionFromAngle(transform.eulerAngles.z + 90);

        float elapsedTime = 0f;
        float idleMovementDuration = Random.Range(minIdleMovementDuration, maxIdleMovementDuration);
        while (elapsedTime < idleMovementDuration)
        {
            rigidBody.velocity = direction * maxSpeed;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // reset values
        rigidBody.velocity = Vector2.zero;
        currentRandomEvent = Random.Range(0, 100);

        isIdleMoving = false;
    }
}
