using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Testing : MonoBehaviour
{
    // * ########## Variables ########## * //
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;

    // * ########## Hashes ########## * //
    int isWalkingHash;
    int berserkTriggerHash;
    int skill1TriggerHash;
    int skill2TriggerHash;
    int skill3TriggerHash;

    // * ########## Input Values ########## * //
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 appliedMovement;
    bool isMovementPressed;
    //bool isDashPressed;
    bool skill1Pressed = false;

    // * ########## Constants movement ########## * //
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

    // * ########## Skill Variables ########## * //
    private float skill1TimeLeft = 0;
    private float skill2TimeLeft = 0;
    private float skill3TimeLeft = 0;
    private int skill1Stage = 0;
    private int skill2Stage = 0;
    private int skill3Stage = 0;
    private float skillStageDuration = 1.5f;

    // * ########## UI ELEMENTS ########## * //
    public SkillCooldownUI berserkCooldownUI;

    // * ########## Weapon Aiming ########## * //
    [SerializeField] Transform weaponSlot;

    // * ########## Camera ########## * //
    [SerializeField] Camera mainCamera;

    // * ########## Functions ########## * //
    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        berserkTriggerHash = Animator.StringToHash("Berserk");
        skill1TriggerHash = Animator.StringToHash("Skill1Stage");
        skill2TriggerHash = Animator.StringToHash("Skill2Stage");
        skill3TriggerHash = Animator.StringToHash("Skill3Stage");

        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Dash.performed += onDash;
        playerInput.CharacterControls.Berserk.performed += onBerserk;
        playerInput.CharacterControls.Skill1.performed += onSkill1;
        playerInput.CharacterControls.Skill2.performed += onSkill2;
        playerInput.CharacterControls.Skill3.performed += onSkill3;
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
                //isDashPressed = false;  // * Reset dash press
                speed = isBerserk ? normalSpeed * 1.5f : normalSpeed;
            }
        }

        if (isBerserk && Time.time > berserkEndTime)
        {
            DeactivateBerserk();
        }

        UpdateSkillTimers();
        HandleMovement();
        HandleRotation();
        HandleWeaponAiming();
        HandleAnimation();

        appliedMovement.x = currentMovement.x;
        appliedMovement.z = currentMovement.z;

        characterController.Move(appliedMovement * Time.deltaTime);
    }

    void UpdateSkillTimers()
    {
        if (skill1TimeLeft > 0)
        {
            skill1TimeLeft -= Time.deltaTime;
            if (skill1TimeLeft <= 0)
            {
                skill1Stage = 0;
                animator.ResetTrigger(skill1TriggerHash);
                Debug.Log("Skill1 Timer Expired: Resetting to Stage 0");
            }
        }
        if (skill2TimeLeft > 0)
        {
            skill2TimeLeft -= Time.deltaTime;
            if (skill2TimeLeft <= 0)
            {
                skill2Stage = 0;
                animator.ResetTrigger(skill2TriggerHash);
                Debug.Log("Skill2 Timer Expired: Resetting to Stage 0");
            }
        }
        if (skill3TimeLeft > 0)
        {
            skill3TimeLeft -= Time.deltaTime;
            if (skill3TimeLeft <= 0)
            {
                skill3Stage = 0;
                animator.ResetTrigger(skill3TriggerHash);
                Debug.Log("Skill3 Timer Expired: Resetting to Stage 0");
            }
        }
    }

    void onDash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && dashCooldownLeft <= 0)
        {
            //isDashPressed = true;
            dashTimeLeft = dashDuration;
            dashCooldownLeft = dashCooldown;
            speed = dashSpeed;
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

    void HandleRotation()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 directionToLookAt = (mousePosition - transform.position).normalized;
        directionToLookAt.y = 0; // Ensure the character only rotates on the Y axis

        Quaternion targetRotation = Quaternion.LookRotation(directionToLookAt);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 15f); // Smooth rotation
    }

    void HandleWeaponAiming()
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        Vector3 playerToMouseDir = mouseWorldPosition - transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(playerToMouseDir);
        weaponSlot.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
    }

    Vector3 GetMouseWorldPosition()
    {
        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (plane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }
        return Vector3.zero;
    }

    void HandleAnimation()
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

        if (berserkCooldownUI != null)
        {
            berserkCooldownUI.StartCooldown(berserkDuration);
        }
    }

    void onSkill1(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && !skill1Pressed)
        {
            skill1Pressed = true; 
            skill1Stage = Mathf.Clamp(skill1Stage + 1, 1, 3);
            skill1TimeLeft = skillStageDuration;
            animator.SetTrigger(skill1TriggerHash);
            Debug.Log("Skill1 Stage: " + skill1Stage);

            StartCoroutine(ResetSkill1Pressed());
        }
    }

    IEnumerator ResetSkill1Pressed()
    {
        yield return new WaitForSeconds(0.4f);
        skill1Pressed = false;
    }

    void onSkill2(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            skill2Stage++;
            skill2TimeLeft = skillStageDuration;
            animator.SetTrigger(skill2TriggerHash);
            Debug.Log("Skill2 Stage: " + skill2Stage);
        }
    }

    void onSkill3(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            skill3Stage++;
            skill3TimeLeft = skillStageDuration;
            animator.SetTrigger(skill3TriggerHash);
            Debug.Log("Skill3 Stage: " + skill3Stage);
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
