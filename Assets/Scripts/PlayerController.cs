using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private float axisH;
    public float speed;


    // Start is called before the first frame update
    void Start()
    {
        this.rigidBody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update() {
        this.axisH = Input.GetAxisRaw("Horizontal");
    }


    private void FixedUpdate() {
        rigidBody.velocity = new Vector2(axisH * speed, rigidBody.velocity.y);
    }
}
