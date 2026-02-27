using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
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
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float attack_range = 2f;
    [SerializeField] private float MaxHP = 100f;
    private float HP;
    private bool IsDead = false;

    [Header("Miscellaneous")]
    [SerializeField] private Animator animator;
    [SerializeField] private TMP_Text uiCooldownText;
    [SerializeField] private Image progressBar;
    [SerializeField] private Image JumpBackground;

    [SerializeField] private GameObject coneAttack;
    [SerializeField] private GameObject jumpAttack; 
    [SerializeField] private GameObject attackZone;
    [SerializeField] private GameObject jumpAttackZone;

    private CharacterController controller;
    private GameObject frontCam;
    private Vector2 moveInput;
    private float verticalRotation = 0f;
    private float horizontalRotation = 0f;
    private Vector3 playerVelocity;
    private bool isGrounded;

    public CameraController camCtrl;
    public Transform cameraTopPos;

    private bool canMove = false;
    
    private float cooldown = 10f;
    private float currentCooldown = 0f;
    private string redJump = "#AE0000";
    private string greenJump = "#299400";


    private bool canAttack = true; 

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (frontCam == null) frontCam = GameObject.Find("CameraYeti").gameObject;
        Cursor.lockState = CursorLockMode.Locked;
        HP = MaxHP;

        // Bloquer les inputs 5 secondes au démarrage
        StartCoroutine(EnableInputAfterDelay(5f));
    }

    private IEnumerator EnableInputAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canMove = true;
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
            if (ColorUtility.TryParseHtmlString(redJump, out Color newColor))
            {
                if (JumpBackground)
                    JumpBackground.color = newColor;
            }
            if (uiCooldownText)
                uiCooldownText.text = currentCooldown.ToString("F0");
        }
        else
        {
            if (ColorUtility.TryParseHtmlString(greenJump, out Color newColor))
            {
                if (JumpBackground)
                    JumpBackground.color = newColor;
            }
            if (uiCooldownText)
                uiCooldownText.text = "";
        }

        //lifebar update
        HP -= Time.deltaTime * 3f; 
        float fillValue = HP / 100f;
        if (progressBar)
            progressBar.fillAmount = fillValue;
        
        if (canMove)
        {
            MovePlayer();
            LookAround();
        }
        ApplyGravity();
    }

    void OnMove(InputValue inputVal)
    {
        if (!canMove) return;

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
        if (!canMove || !isGrounded || currentCooldown > 0f) return;

        animator.SetTrigger("jump");
        playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        isGrounded = false;
        currentCooldown = cooldown;

        StartCoroutine(WaitForJumpState());
    }

    IEnumerator WaitForJumpState()
    {
        // attendre que l'Animator entre vraiment dans l'état "Jump"
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            yield return null;

        // ici l'animation Jump est active
        StartCoroutine(JumpAttack());
        StartCoroutine(
            camCtrl.JumpQTECamera(
                cameraTopPos,
                10f,  //Duration
                0.15f //Slow
            )
        );
    }

    IEnumerator JumpAttack()
    {
        yield return new WaitForSeconds(1.0f);
        jumpAttackZone.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        jumpAttackZone.SetActive(false);
    }

    void OnLook(InputValue inputVal)
    {
        if (!canMove) return;

        Vector2 mouseDelta = inputVal.Get<Vector2>();
        horizontalRotation += mouseDelta.x * mouseSensitivity * Time.deltaTime;
        verticalRotation -= mouseDelta.y * mouseSensitivity * Time.deltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, lowestAngle, highestAngle);
    }

    void OnAttack(InputValue inputVal)
    {
        if (!canMove) return;

        animator.SetTrigger("attack");
        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        attackZone.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        attackZone.SetActive(false);
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
        frontCam.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    public void IsAttack(float damage)
    {
        HP -= damage;
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
