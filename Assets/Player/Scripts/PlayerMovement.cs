using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    #region inputs
    float horizontalInput, verticalInput;
    private Rigidbody2D rb2D;

    #endregion

    
    [Header("Settings")]
    public float acceleration = 2f;
    public float baseSpeed = 5f;
    private float timeInMotion = 0f;

    [Header("Visual")]
    public ParticleSystem trail;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f);
        movement.Normalize();
        //transform.Translate(movement * Speed * Time.deltaTime);
        //rb2D.AddForce(movement * baseSpeed * Time.deltaTime,ForceMode2D.Force);
        rb2D.velocity = movement * getSpeed();


        if (movement != Vector3.zero)
        {
            if(trail.isPlaying==false)
                trail.Play();
            timeInMotion += Time.deltaTime;
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg - 90f;
            transform.GetChild(0).transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {
            trail.Stop();
            timeInMotion = 0;
        }

    }
    float getSpeed()
    {
        return (baseSpeed + acceleration) * Mathf.Clamp(timeInMotion, 0.1f, 2);
    }

    public Vector3 getPosition(){
        return transform.position;
    }
}
