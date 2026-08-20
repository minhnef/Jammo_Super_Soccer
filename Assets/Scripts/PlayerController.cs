using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [SerializeField] private float moveSpeed = 5f;
    public float currentMoveSpeed;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Rigidbody rb;

    private Vector3 moveDirection;

    [SerializeField] private Animator animator;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (animator == null) animator = GetComponent<Animator>();
    }


    private void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        rb.MovePosition(rb.position + moveDirection * currentMoveSpeed * Time.fixedDeltaTime);
        currentMoveSpeed = moveDirection.magnitude * moveSpeed;
        animator.SetFloat("Blend", currentMoveSpeed);
    }

    private void RotatePlayer()
    {
        if (moveDirection != Vector3.zero)
        {
            Quaternion lookDirection = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(
                Quaternion.Slerp(
                    rb.rotation,
                    lookDirection,
                    rotationSpeed * Time.fixedDeltaTime
                    ));
        }
        else return;
    }

    public void PlayHappy()
    {
        animator.SetTrigger("happy");
    }
}
