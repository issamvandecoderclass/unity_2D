using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    public float moveSpeed = 10.0f;
    public float jumpSpeed = 20.0f;
    public float smoothTimeY = 0.1f;
    public float smoothTimeX = 0.1f;
    public float attackCD = 0.3f;
    public int playerLives = 3;
    public int coinPickups = 0;
    public Text livesCounter;
    public Text coinCounter;
    GameObject Camera;
    Vector2 cameraVelocity;
    Vector2 spawnPoint;
    bool facingRight = true;
    bool attacking;
    float attackTime;
    public Collider2D batTrigger;
    Animator anim;
    Rigidbody2D rigid;



    // Use this for initialization
    void Start () {
        UpdateCounter();
        spawnPoint = transform.position;
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
        batTrigger.enabled = false;

    }


	
	// Update is called once per frame
	void Update () {
        float posX = Mathf.SmoothDamp(Camera.transform.position.x, transform.position.x, ref
        cameraVelocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(Camera.transform.position.y, transform.position.y, ref
        cameraVelocity.y, smoothTimeY);
        Camera.transform.position = new Vector3(posX, posY, Camera.transform.position.z);
        if (Input.GetKeyDown("f") && !attacking)
        {
            attacking = true;
            attackTime = attackCD;
            anim.SetTrigger("Attacking");
        }
        if (attacking)
        {
            if (attackTime > 0)
            {
                attackTime -= Time.deltaTime;
                batTrigger.enabled = true;
            }
            else
            {
                batTrigger.enabled = false;
                attacking = false;
            }
        }


    }

    void UpdateCounter()
    {
        livesCounter.text = playerLives.ToString();
        coinCounter.text = coinPickups.ToString();
    }


void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(move));
        rigid.velocity = new Vector2(move * moveSpeed, rigid.velocity.y);
        if (Input.GetButtonDown("Jump") && rigid.velocity.y == 0)
        {
            rigid.AddForce(Vector2.up * jumpSpeed);
        }
        if (Input.GetButtonDown("Jump"))
        {
            rigid.AddForce(Vector2.up * jumpSpeed);
        }
        if (move > 0 && !facingRight)
        {
            FlipFacing();
        }
        else if (move < 0 && facingRight)
        {
            FlipFacing();
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Coin")
        {
            DestroyObject(col.gameObject);
            coinPickups = coinPickups + 1;
            UpdateCounter();
        }
        if (col.gameObject.tag == "Deathzone")
        {
            if (playerLives > 0)
            {
                playerLives = playerLives - 1;
                transform.position = spawnPoint;
            }
            else
            {
                livesCounter.text = "3";
                coinCounter.text = "0";
                transform.position = spawnPoint;
            }
        }
        UpdateCounter();
    }
void FlipFacing()
    {
        facingRight = !facingRight;
        Vector3 charScale = transform.localScale;
        charScale.x = charScale.x * -1;
        transform.localScale = charScale;
    }
}
