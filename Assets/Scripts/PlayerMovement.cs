using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float mouseSensitivity = 10f;
    [SerializeField] private float lowestAngle = -90f;
    [SerializeField] private float highestAngle = 90f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float attack_range = 2f;
    [SerializeField] private float MaxHP = 100;
    private float HP;
    private bool IsDead = false;
    [Header("Miscellaneous")]
    [SerializeField] private Animator animator;


    private CharacterController controller;
    private GameObject cam;
    private Vector2 moveInput;
    private float verticalRotation = 0f;
    private float horizontalRotation = 0f;
    private Vector3 playerVelocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (cam == null) cam = GetComponentInChildren<Camera>().gameObject;
        Cursor.lockState = CursorLockMode.Locked;
        HP = MaxHP;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f; // Petite force pour coller au sol
        }

        MovePlayer();
        ApplyGravity();
        LookAround();
    }

    void OnMove(InputValue inputVal)
    {
        moveInput = inputVal.Get<Vector2>();

        if (moveInput == Vector2.zero)
        {
            animator.SetFloat("speed", 0);    
        }
        else
        {
            animator.SetFloat("speed", 1);
        }
    }

    void OnJump(InputValue inputVal)
    {
        if (isGrounded)
        {
            animator.SetTrigger("jump");
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        
    }

    void OnLook(InputValue inputVal)
    {
        Vector2 mouseDelta = inputVal.Get<Vector2>();
        horizontalRotation += mouseDelta.x * mouseSensitivity * Time.deltaTime;
        verticalRotation -= mouseDelta.y * mouseSensitivity * Time.deltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, lowestAngle, highestAngle);
    }

    void OnAttack(InputValue inputVal)
    {
        animator.SetTrigger("attack");
        //si les pinguins sont à la portées d'attaque du yeti, les tuers.
    }

    private void MovePlayer()
    {
        Vector3 moveDir = transform.forward * moveInput.y + transform.right * moveInput.x;
        controller.Move(moveDir * moveSpeed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void LookAround()
    {
        transform.rotation = Quaternion.Euler(0, horizontalRotation, 0);
        cam.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    public void IsAttack(float damage)
    {
        HP -= damage;
        if(HP % 10 == 0 && HP != MaxHP)
        {
            animator.SetTrigger("damage");
        }
        if(HP <= 0)
        {
            DeadPlayer();
        }
    }

    private void DeadPlayer()
    {
        IsDead = true;
        animator.SetBool("dead", true);
    }
}
