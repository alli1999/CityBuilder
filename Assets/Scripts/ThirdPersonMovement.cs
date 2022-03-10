using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public Animator animator;

    public float speed = 6f;
    public float gravity = -9.81f;

    public Joystick joystick;

    Vector3 velocity;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR && UNITY_STANDALONE_WIN

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");


        animator.SetFloat("HorizontalSpeed", Mathf.Abs(horizontal));
        animator.SetFloat("VerticalSpeed", Mathf.Abs(vertical));

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Vector3 moveDirection = Quaternion.AngleAxis(cam.rotation.eulerAngles.y, Vector3.up) * direction;
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);

            if(moveDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 200f * Time.deltaTime);
            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
#endif

#if UNITY_ANDROID
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        animator.SetFloat("HorizontalSpeed", Mathf.Abs(horizontal));
        animator.SetFloat("VerticalSpeed", Mathf.Abs(vertical));

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Vector3 moveDirection = Quaternion.AngleAxis(cam.rotation.eulerAngles.y, Vector3.up) * direction;
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);

            if (moveDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 200f * Time.deltaTime);
            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
#endif
    }
}
