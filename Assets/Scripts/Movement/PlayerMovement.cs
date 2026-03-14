using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;       
    public float rotationSpeed = 10f; 
    public Animator animator;         

    private Vector3 movement;

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); 
        float moveZ = Input.GetAxis("Vertical");   

        movement = new Vector3(moveX, 0f, moveZ).normalized;

        bool isWalking = movement.magnitude > 0f;
        animator.SetBool("isWalking", isWalking); 

        if (isWalking)
        {
            RotateCharacter();
        }
    }

    void FixedUpdate()
    {
        if (movement.magnitude > 0f)
        {
            transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    void RotateCharacter()
    {
        Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
