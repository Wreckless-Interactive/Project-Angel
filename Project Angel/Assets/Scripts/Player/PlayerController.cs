using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : BaseCharacter
{

    public float moveSpeed;
    public float gravity;

    private Vector3 velocity;
    private CharacterController Controller => GetComponent<CharacterController>();

    private void Start()
    {
        
    }

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {

        float h = Input.GetAxis("Horizontal") * moveSpeed;
        float v = Input.GetAxis("Vertical") * moveSpeed;

        velocity = new Vector3(h, velocity.y, v);
        velocity = transform.TransformDirection(velocity);
        velocity.y -= gravity * Time.deltaTime;

        Controller.Move(velocity * Time.deltaTime);

    }

}
