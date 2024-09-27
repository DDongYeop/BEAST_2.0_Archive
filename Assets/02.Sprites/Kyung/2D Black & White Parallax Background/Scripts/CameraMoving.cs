using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    public Transform pointR, pointL;
    public float speed;
    private bool movingRight;
    public bool automaticMovement;

    void Update()
    {
        if(automaticMovement)
        {
            Move();
        }
        else
        {
            MoveWithKeys();
        }
       
    }

    void Move()
    {
        if(transform.position.x >= pointR.position.x) movingRight = false;
        else if(transform.position.x <= pointL.position.x) movingRight = true;

        if(movingRight) transform.position += Vector3.right * speed * Time.deltaTime;
        else transform.position += Vector3.left * speed * Time.deltaTime;
    }

    void MoveWithKeys()
    {
        if((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && transform.position.x < pointR.position.x)
        {
           transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else if((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && transform.position.x > pointL.position.x)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }  
    }
}
