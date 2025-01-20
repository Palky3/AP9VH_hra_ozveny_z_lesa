using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class Character : MonoBehaviour
{
    [Header("Controls")]
    public float playerSpeed = 4.0f;
    public float crouchSpeed = 2.0f;
    public float sprintSpeed = 5.0f;
    public float jumpHeight = 0.8f; 
    public float gravityMultiplier = 2;
    public float rotationSpeed = 5f;
    public float crouchColliderHeight = 1.35f;
    public float jumpingDelayAfterLanding = 0.2f;

    [Header("Animation Smoothing")]
    [Range(0, 1)]
    public float speedDampTime = 0.1f;
    [Range(0, 1)]
    public float velocityDampTime = 0.9f;
    [Range(0, 1)]
    public float rotationDampTime = 0.2f;
    [Range(0, 1)]
    public float airControl = 0.5f;

    public StateMachine movementSM;
    public StandingState standing;
    public JumpingState jumping;
    public CrouchingState crouching;
    public LandingState landing;
    public SprintState sprinting;
    public SprintJumpState sprintjumping;
    public CombatState combatting;
    public AttackState attacking;

    public bool isDead = false;

    [HideInInspector]
    public float gravityValue = -9.81f;
    [HideInInspector]
    public bool canJump = true;
    [HideInInspector]
    public float normalColliderHeight;
    [HideInInspector]
    public CharacterController controller;
    [HideInInspector]
    public PlayerInput playerInput;
    [HideInInspector]
    public Transform cameraTransform;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Vector3 playerVelocity;

    //bool is_introCutscene = true;


    // Start is called before the first frame update
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        movementSM = new StateMachine();
        standing = new StandingState(this, movementSM);
        jumping = new JumpingState(this, movementSM);
        crouching = new CrouchingState(this, movementSM);
        landing = new LandingState(this, movementSM);
        sprinting = new SprintState(this, movementSM);
        sprintjumping = new SprintJumpState(this, movementSM);
        combatting = new CombatState(this, movementSM);
        attacking = new AttackState(this, movementSM);

        movementSM.Initialize(standing);

        normalColliderHeight = controller.height;
        gravityValue *= gravityMultiplier;

        //StartCoroutine(WaitForCutsceneEnd());
    }

    IEnumerator WaitForCutsceneEnd()
    {
        yield return new WaitForSeconds(18f);
        //is_introCutscene = false;
        
    }


    private void Update()
    {
        if (!isDead && !GameManager.isPaused/* && !is_introCutscene*/)
        {
            movementSM.currentState.HandleInput();

            movementSM.currentState.LogicUpdate();
        }
        
    }

    private void FixedUpdate()
    {
        if (!isDead && !GameManager.isPaused/* && !is_introCutscene*/)
        {
            movementSM.currentState.PhysicsUpdate();
        }
    }

    void OnLandingComplete()
    {

        StartCoroutine(EnableJumpWithDelay(jumpingDelayAfterLanding));
    }

    void OnSprintJumpComplete()
    {

        StartCoroutine(EnableJumpWithDelay(jumpingDelayAfterLanding));
    }

    private IEnumerator EnableJumpWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canJump = true;
    }
}
