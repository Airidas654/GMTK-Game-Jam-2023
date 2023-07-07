using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMovement : MonoBehaviour
{
    [Header("Horizontal movement")]
    [Min(0)]
    [SerializeField] public float movingSpeed = 1;
    [Min(0)]
    [SerializeField] float timeToMaxAcceleration = 0.1f;
    [SerializeField] AnimationCurve accelerationCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] AnimationCurve stopCurve = AnimationCurve.Linear(0, 1, 1, 0);

    private Rigidbody2D rigid;
    private Animator anim;
    private Shooting shoot;
    private float horizontalMovementTime = 0;
    private float verticalMovementTime = 0;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        shoot = GetComponent<Shooting>();
    }


    void Update()
    {
        ////////////// Horizontal movement //////////////
        float horizontal;
        float vertical;
        if (shoot.shooting)
        {
            horizontal = 0;
            vertical = 0;
        }
        else
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }
        float horizontalSpeed = 0;
        float verticalSpeed = 0;
        if (horizontal > 0)
        {
            horizontalMovementTime = Mathf.Min(horizontalMovementTime + Time.deltaTime, timeToMaxAcceleration);
            if (horizontalMovementTime < 0)
            {
                horizontalSpeed = stopCurve.Evaluate(Mathf.Abs(horizontalMovementTime / timeToMaxAcceleration));
            }
            else
            {
                horizontalSpeed = accelerationCurve.Evaluate(Mathf.Abs(horizontalMovementTime / timeToMaxAcceleration));
            }
        }
        else if (horizontal < 0)
        {
            horizontalMovementTime = Mathf.Max(horizontalMovementTime - Time.deltaTime, -timeToMaxAcceleration);
            if (horizontalMovementTime > 0)
            {
                horizontalSpeed = -stopCurve.Evaluate(Mathf.Abs(horizontalMovementTime / timeToMaxAcceleration));
            }
            else
            {
                horizontalSpeed = -accelerationCurve.Evaluate(Mathf.Abs(horizontalMovementTime / timeToMaxAcceleration));
            }
        }
        else
        {
            if (horizontalMovementTime > 0)
            {
                horizontalMovementTime = Mathf.Max(0, horizontalMovementTime - Time.deltaTime);
                horizontalSpeed = stopCurve.Evaluate(Mathf.Abs(horizontalMovementTime / timeToMaxAcceleration));
            }
            else
            {
                horizontalMovementTime = Mathf.Min(0, horizontalMovementTime + Time.deltaTime);
                horizontalSpeed = -stopCurve.Evaluate(Mathf.Abs(horizontalMovementTime / timeToMaxAcceleration));
            }
        }


        if (vertical > 0)
        {
            verticalMovementTime = Mathf.Min(verticalMovementTime + Time.deltaTime, timeToMaxAcceleration);
            if (verticalMovementTime < 0)
            {
                verticalSpeed = stopCurve.Evaluate(Mathf.Abs(verticalMovementTime / timeToMaxAcceleration));
            }
            else
            {
                verticalSpeed = accelerationCurve.Evaluate(Mathf.Abs(verticalMovementTime / timeToMaxAcceleration));
            }
        }
        else if (vertical < 0)
        {
            verticalMovementTime = Mathf.Max(verticalMovementTime - Time.deltaTime, -timeToMaxAcceleration);
            if (verticalMovementTime > 0)
            {
                verticalSpeed = -stopCurve.Evaluate(Mathf.Abs(verticalMovementTime / timeToMaxAcceleration));
            }
            else
            {
                verticalSpeed = -accelerationCurve.Evaluate(Mathf.Abs(verticalMovementTime / timeToMaxAcceleration));
            }
        }
        else
        {
            if (verticalMovementTime > 0)
            {
                verticalMovementTime = Mathf.Max(0, verticalMovementTime - Time.deltaTime);
                verticalSpeed = stopCurve.Evaluate(Mathf.Abs(verticalMovementTime / timeToMaxAcceleration));
            }
            else
            {
                verticalMovementTime = Mathf.Min(0, verticalMovementTime + Time.deltaTime);
                verticalSpeed = -stopCurve.Evaluate(Mathf.Abs(verticalMovementTime / timeToMaxAcceleration));
            }
        }



        rigid.velocity = new Vector2(horizontalSpeed, verticalSpeed).normalized * movingSpeed;

        if (rigid.velocity.magnitude <= float.Epsilon)
        {
            anim.SetBool("Moving", false);
        }
        else
        {
            anim.SetBool("Moving", true);
        }


        if (horizontal > 0)
        {
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else if (horizontal < 0)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }


    }
}
