using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiController : MonoBehaviour
{
    private Animator animator;
    private bool isDead = false;

    // Pour simuler les inputs / mouvements
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private bool isGrounded = true;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return; // Pas d’animations si mort

        HandleMovement();
        HandleActions();

        // Exemple rapide pour tester la mort
        if (Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }
    }

    void HandleMovement()
    {
        float move = Input.GetAxis("Vertical");
        float strafe = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(strafe, 0, move);

        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        animator.SetFloat("speed", movement.magnitude);
    }

    void HandleActions()
    {
        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            animator.SetTrigger("jump");
            isGrounded = false;
        }

        // Attack
        if (Input.GetMouseButtonDown(0)) // clic gauche
        {
            animator.SetTrigger("attack");
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        animator.SetBool("dead", true);
    }

    // Cette fonction doit être appelée via un Event dans l’animation Jump ou via collision
    public void Land()
    {
        isGrounded = true;
    }
}