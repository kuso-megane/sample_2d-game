using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rbody;
    private float axisH;
    public float speed;
    public float jump = 9.0f;

    public LayerMask groundLayer;
    private bool goJump = false;
    private bool onGround = false;


    // Start is called before the first frame update
    void Start()
    {
       this.rbody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update() {
        this.axisH = Input.GetAxisRaw("Horizontal"); //1 or -1


        //ジャンプボタン判定
        if (Input.GetButtonDown("Jump")) {
            Jump();
        }
    }


    private void FixedUpdate() {
        
        onGround = Physics2D.Linecast(
            transform.position,
            transform.position - (transform.up * 0.1f),
            this.groundLayer
        );

        //向き調整
        if (onGround) {
            if (this.axisH > 0.0f) {
                transform.localScale = new Vector2(1, 1);
            }
            else if(this.axisH < 0.0f) {
                transform.localScale = new Vector2(-1, 1);
            }
        }
        

        //地上の速度更
        if (onGround) {
            this.rbody.velocity = new Vector2(this.axisH * speed, this.rbody.velocity.y);
        }

        //jump
        if (onGround && goJump) {
            Vector2 jumpPw = new Vector2(0, this.jump);
            this.rbody.AddForce(jumpPw, ForceMode2D.Impulse);
            this.goJump = false;
        }
    }



    void Jump() {
        this.goJump = true;

    }
}
