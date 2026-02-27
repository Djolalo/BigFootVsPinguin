using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

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
    [Header("Miscellaneous")]
    [SerializeField] private Animator animator;
    [SerializeField] private TMP_Text uiCooldownText;
    
    [SerializeField] private float MaxHP = 100f;
    [SerializeField] private Image progressBar;

    
    

    
    private CharacterController controller;
    private GameObject cam;
    private Vector2 moveInput;
    private float verticalRotation = 0f;
    private float horizontalRotation = 0f;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private float cooldown = 10f;
    private float currentCooldown = 0f;
    private float HP = 1f;

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

        //Jump cooldown 
        
        if (currentCooldown > 0f){
            currentCooldown -= Time.deltaTime;
        }
        
        if(currentCooldown < 0f)
        {
            currentCooldown = 0f;
        }

        if(currentCooldown > 0f)
        {
                uiCooldownText.text = currentCooldown.ToString("F0");
        }else{
            uiCooldownText.text = "";
        }

        //lifebar update
        HP -= Time.deltaTime * 1f; 
        float fillValue = HP/100f;
        progressBar.fillAmount = fillValue;
        

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
            if (currentCooldown <= 0f)
            {
                animator.SetTrigger("jump");
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                currentCooldown = cooldown;
            }
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
}
