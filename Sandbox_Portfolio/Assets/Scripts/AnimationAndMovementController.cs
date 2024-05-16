using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationAndMovementController : MonoBehaviour
{
    // * ########## Variables ########## * //
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;

    // * ########## Hashes ########## * //
    int isWalkingHash;
    int berserkTriggerHash;
    int skillTriggerHash;

    // * ########## Input Values ########## * //
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 appliedMovement;
    bool isMovementPressed;
    bool isDashPressed;

    // * ########## Constants movement ########## * //
    float rotationFactorPerFrame = 15.0f;
    float normalSpeed = 4.5f;
    public float dashSpeed = 100f;
    public float dashDuration = 0.05f;
    float dashCooldown = 1.5f;
    float dashTimeLeft = 0;
    float dashCooldownLeft = 0;
    float speed = 4.5f;

    // * ########## Berserk ########## * //
    public float berserkDuration = 10.0f;
    private bool isBerserk = false;
    private float berserkEndTime;

    // * ########## Functions ########## * //
    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        berserkTriggerHash = Animator.StringToHash("Berserk");
        skillTriggerHash = Animator.StringToHash("Skill");

        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Dash.performed += onDash;
        playerInput.CharacterControls.Berserk.performed += onBerserk;
        playerInput.CharacterControls.Skill.performed += onSkill;
    }

    void Update()
    {
        if (dashCooldownLeft > 0)
        {
            dashCooldownLeft -= Time.deltaTime;
        }

        if (dashTimeLeft > 0)
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0)
            {
                isDashPressed = false;  // * Reset dash press
                speed = isBerserk ? normalSpeed * 1.5f : normalSpeed;
                HandleMovement();
            }
        }

        if (isBerserk && Time.time > berserkEndTime)
        {
            DeactivateBerserk();
        }

        handleRotation();
        handleAnimation();
        appliedMovement.x = currentMovement.x;
        appliedMovement.z = currentMovement.z;

        characterController.Move(appliedMovement * Time.deltaTime);
    }

    void onDash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && dashCooldownLeft <= 0)
        {
            isDashPressed = true;
            dashTimeLeft = dashDuration;
            dashCooldownLeft = dashCooldown;
            speed = dashSpeed;
            HandleMovement();
        }
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 isoMovementInput = RotateInput(currentMovementInput, -45);
        currentMovement.x = isoMovementInput.x * speed;
        currentMovement.z = isoMovementInput.y * speed;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    void handleRotation()
    {
        Vector3 positionToLookAt;
        // * the change in pos our character should point to
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;
        // * the current rotation of our character
        Quaternion currentRotation = transform.rotation;
        if (isMovementPressed)
        {
            // * the rotation we want to have
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            // * the rotation we want to have
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    Vector2 RotateInput(Vector2 input, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float tx = input.x;
        float ty = input.y;
        input.x = (cos * tx) - (sin * ty);
        input.y = (sin * tx) + (cos * ty);

        return input;
    }

    void handleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);

        if (isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        else if (!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }
    }

    void onBerserk(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && !isBerserk)
        {
            ActivateBerserk();
        }
    }

    void ActivateBerserk()
    {
        isBerserk = true;
        berserkEndTime = Time.time + berserkDuration;

        dashCooldown = 0.5f;
        speed = normalSpeed * 1.9f;
        animator.SetTrigger(berserkTriggerHash);

    }

    void onSkill(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            animator.SetTrigger(skillTriggerHash);
        }
    }

    void DeactivateBerserk()
    {
        isBerserk = false;

        dashCooldown = 1.5f;
        speed = normalSpeed;
    }

    void OnEnable()
    {
        // * Enabling the PlayerInput
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        // * Disabling the PlayerInput
        playerInput.CharacterControls.Disable();
    }
}
