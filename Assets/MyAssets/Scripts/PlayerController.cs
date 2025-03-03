using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick;
    public CharacterController controller;
    public Animator anim;
    public PlayerManager playerManager;
    public float speed;
    public float gravity;
    Vector3 moveDirection;

    private void Start()
    {
        speed = PlayerPrefs.GetFloat("PlayerSpeedVal", speed);
    }

    void Update()
    {
        Vector2 direction = joystick.direction;

        if (controller.isGrounded)
            moveDirection = new Vector3(direction.x, 0, direction.y);

        moveDirection.y += gravity * Time.deltaTime;
        controller.Move(moveDirection * speed * Time.deltaTime);

        Vector3 rotDirection = new Vector3(direction.x, 0, direction.y);

        Quaternion targetRotation = rotDirection != Vector3.zero ? Quaternion.LookRotation(rotDirection) : transform.rotation;
        transform.rotation = targetRotation;

        if (playerManager.collectedFood.Count > 0)
        {
            if (direction == Vector2.zero)
                anim.Play("CarryIdle");
            else
                anim.Play("CarryRun");
        }
        else
        {
            if (direction == Vector2.zero)
                anim.Play("Idle");
            else
                anim.Play("Run");
        }
    }

    public void IncreaseSpeed(int increaseVal)
    {
        speed += increaseVal;
        PlayerPrefs.SetFloat("PlayerSpeedVal", speed);
    }
}