// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;


// #region AnimationAndMovementController

//     int isRunningHash;
//     int isJumpingHash;
//     int jumpCountHash;

//     int zero = 0;

//     * ########## Gravity ########## * //
//     float gravity = -9.8f;
//     float groundedGravity = -.05f;

//     * ########## Jumping ########## * //
//     bool isJumpPressed = false;
//     float initialJumpVelocity;
//     float maxJumpHeight = 3.0f;
//     float maxJumpTime = 0.75f;
//     bool isJumping = false;
//     bool isJumpAnimating = false;
//     int jumpCount = 0;
//     Dictionary<int, float> initialJumpVelocities = new Dictionary<int, float>();
//     Dictionary<int, float> jumpGravities = new Dictionary<int, float>();
//     Coroutine currentJumpResetRoutine = null;


//     void Awake()
//     {
//         isRunningHash = Animator.StringToHash("isRunning");
//         isJumpingHash = Animator.StringToHash("isJumping");
//         jumpCountHash = Animator.StringToHash("jumpCount");

//         playerInput.CharacterControls.Jump.started += onJump;
//         playerInput.CharacterControls.Jump.canceled += onJump;
//         setupJumpVariables();
//     }

//     void setupJumpVariables()
//     {
//         // * calculating the initial jump velocity and gravity
//         float timeToApex = maxJumpTime / 2;
//         gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
//         initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
//         float secondJumpGravity = (-2 * (maxJumpHeight + 2)) / Mathf.Pow((timeToApex * 1.25f), 2);
//         float secondJumpInitialVelocity = (2 * (maxJumpHeight + 2)) / (timeToApex * 1.25f);
//         float thirdJumpGravity = (-2 * (maxJumpHeight + 4)) / Mathf.Pow((timeToApex * 1.5f), 2);
//         float thirdJumpInitialVelocity = (2 * (maxJumpHeight + 4)) / (timeToApex * 1.5f);

//         initialJumpVelocities.Add(1, initialJumpVelocity);
//         initialJumpVelocities.Add(2, secondJumpInitialVelocity);
//         initialJumpVelocities.Add(3, thirdJumpInitialVelocity);

//         jumpGravities.Add(0, gravity);      // * when jump counts resets
//         jumpGravities.Add(1, gravity);
//         jumpGravities.Add(2, secondJumpGravity);
//         jumpGravities.Add(3, thirdJumpGravity);     // * when jump count is 3

//     }

//     void handleJump()
//     {
//         if (!isJumping && characterController.isGrounded && isJumpPressed)
//         {
//             if (jumpCount < 3 && currentJumpResetRoutine != null)
//             {
//                 StopCoroutine(currentJumpResetRoutine);
//             }
//             animator.SetBool(isJumpingHash, true);
//             isJumpAnimating = true;
//             isJumping = true;
//             jumpCount += 1;
//             animator.SetInteger(jumpCountHash, jumpCount);
//             currentMovement.y = initialJumpVelocities[jumpCount];
//             appliedMovement.y = initialJumpVelocities[jumpCount];
//         }
//         else if (!isJumpPressed && isJumping && characterController.isGrounded)
//         {
//             isJumping = false;
//         }
//     }

//     // * Co Routine for handling jump
//     IEnumerator jumpResetRoutine()
//     {
//         yield return new WaitForSeconds(0.5f);
//         jumpCount = 0;
//     }


//     void onJump (InputAction.CallbackContext context)
//     {
//         isJumpPressed = context.ReadValueAsButton();

//     }

//     void handleAnimation()
//     {
//     bool isRunning = animator.GetBool(isRunningHash);
//     bool isJumping = animator.GetBool(isJumpingHash);

//     if (isRunPressed && isMovementPressed && !isRunning)
//     {
//         animator.SetBool(isRunningHash, true);
//     }
//     else if ((!isMovementPressed || !isRunPressed) && isRunning)
//     {
//         animator.SetBool(isRunningHash, false);
//     }
//     }
//     void handleGravity()
//     {
//         bool isFalling = currentMovement.y <= 0.0f || !isJumpPressed;
//         float fallMultiplier = 2.0f;

//         if (characterController.isGrounded)
//         {
//             if (isJumpAnimating)
//             {
//                 animator.SetBool(isJumpingHash, false);
//                 isJumpAnimating = false;
//                 currentJumpResetRoutine = StartCoroutine(jumpResetRoutine());
//                 if (jumpCount == 3)
//                 {
//                     jumpCount = 0;
//                     animator.SetInteger(jumpCountHash, jumpCount);
//                 }
//             }
//             currentMovement.y = groundedGravity;
//             appliedMovement.y = groundedGravity;
//         } 
//         else if (isFalling)
//         {
//             float previousYVelocity = currentMovement.y;
//             currentMovement.y = currentMovement.y + (jumpGravities[jumpCount] * fallMultiplier * Time.deltaTime);
//             appliedMovement.y = Mathf.Max((previousYVelocity + currentMovement.y) * .5f, -20.0f);
//         }
//         else
//         {
//             float previousYVelocity = currentMovement.y;
//             currentMovement.y = currentMovement.y + (jumpGravities[jumpCount] * Time.deltaTime);
//             appliedMovement.y = (previousYVelocity + currentMovement.y) * .5f;
//         }

//     void Update(){
//     //  calling handler after currentMovement is updated because we need to update the y value of currentMovement to
//     //  know what gravity variable is to be applied
//     handleGravity();
//     handleJump();
//     }
// #endregion