using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private float axisH;
    public Rigidbody2D rbody;
    
    public float speed;
    public float jump;
    

    public LayerMask groundLayer;
    private bool goJump = false;
    private bool onGround = false;

    private Animator animator;

    private Dictionary<string, string> animes = new Dictionary<string, string>() {
        {"stopAnime", "PlayerStop"},
        {"runAnime", "PlayerRun"},
        {"jumpAnime", "PlayerJump"},
        {"goalAnime", "PlayerGoal"},
        {"deadAnime", "PlayerOver"}
    };

    private string nowAnime = "";
    private string oldAnime = "";


    // Start is called before the first frame update
    void Start()
    {
        this.rbody = this.GetComponent<Rigidbody2D>();
        this.animator = this.GetComponent<Animator>();
        this.nowAnime = this.animes["stopAnime"];
        this.oldAnime = this.animes["stopAnime"];

        GameManager.gameState_static = GameManager.state_playing;
    }

    // Update is called once per frame
    void Update() {

        if (GameManager.gameState_static != GameManager.state_playing) {
            return;
        }


        this.axisH = Input.GetAxisRaw("Horizontal"); //1 or -1

        //ジャンプボタン判定
        if (Input.GetButtonDown("Jump")) {
            Jump();
        }
    }


    void FixedUpdate() {

        if (GameManager.gameState_static != GameManager.state_playing) {
            return;
        }
        
        this.onGround = Physics2D.Linecast(
            transform.position,
            transform.position - (transform.up * 0.1f),
            this.groundLayer
        );

        //向き調整
        if (this.onGround) {
            if (this.axisH > 0.0f) {
                transform.localScale = new Vector2(1, 1);
            }
            else if(this.axisH < 0.0f) {
                transform.localScale = new Vector2(-1, 1);
            }
        }
        

        //速度更新
        if (this.onGround || this.axisH != 0) {
            this.rbody.velocity = new Vector2(this.axisH * speed, this.rbody.velocity.y);
        }

        //jump
        if (this.onGround && this.goJump) {
            
            Vector2 jumpPw = new Vector2(0, this.jump);
            this.rbody.AddForce(jumpPw, ForceMode2D.Impulse);
            
            this.goJump = false;
        }

        //空中機動
        if (!this.onGround) {
            //this.rbody.velocity = new Vector2(this.rbody.velocity.x * 0.999f, this.rbody.velocity.y);
        }

        //animation
        if (this.onGround) {

            if (axisH == 0) {
                this.nowAnime = this.animes["stopAnime"];
            }
            else {
                this.nowAnime = this.animes["runAnime"];
            }
        }
        else {
            this.nowAnime = this.animes["jumpAnime"];
        }

        if (this.nowAnime != this.oldAnime) {
            this.oldAnime = this.nowAnime;
            this.animator.Play(this.nowAnime);
        }
        
    }



    void Jump() {
        this.goJump = true;
    }


    void OnTriggerEnter2D(Collider2D other) {

        if (GameManager.gameState_static != GameManager.state_playing) {
            return;
        }
        
        GameObject collidedObj = other.gameObject;
        if (collidedObj.tag == "Goal") {
            this.Goal();
        }
        else if (collidedObj.tag == "Dead"){
            this.GameOver();
        }

    }

    public void Goal() {

        GameManager.gameState_static = GameManager.state_clear;
        this.animator.Play(this.animes["goalAnime"]);

        this.GameStop();
    }


    public void GameOver() {

        GameManager.gameState_static = GameManager.state_over;
        this.animator.Play(this.animes["deadAnime"]);

        this.GameStop();

        //ゲームオーバー演出
        this.GetComponent<CapsuleCollider2D>().enabled = false;
        this.rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }


    void GameStop() {
        this.rbody.velocity = new Vector2(0, 0);
    }
}
