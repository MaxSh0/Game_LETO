using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hero : MonoBehaviour
{
    private Rigidbody2D rd;
    Animator anim;

    public bool isGround;
    Transform grounded;
    public LayerMask layerMask;
    public int Gold = 0;

    public GameObject money1;
    public GameObject money2;
    public GameObject money3;

    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        grounded = GameObject.Find(this.name + "/grounded").transform;

        money1 = GameObject.Find("coin_gold");
        money2 = GameObject.Find("coin_gold (2)");
        money3 = GameObject.Find("coin_gold (1)");


        money1.SetActive(false);
        money2.SetActive(false);
        money3.SetActive(false);
    }


    void Update()
    {
     
        isGround = Physics2D.Linecast(transform.position, grounded.position, layerMask);
        anim.SetBool("jump", !isGround);

        //прыжок
        if (Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.UpArrow))
        {
            anim.SetBool("jump", true);
            Jump();
        }

        //анимации бега и поворота
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            Flip();
            anim.SetInteger("numAnimation", 2);
        }
        else
        {
            anim.SetInteger("numAnimation", 1);
        }

        //значки сверху
        Money(Gold);
    }

    void FixedUpdate()
    {
        rd.velocity = new Vector2(Input.GetAxis("Horizontal") * 10f, rd.velocity.y);
    }

    void Jump()
    {
        if (isGround) { 
        rd.velocity = new Vector3(0, 8, 0);
        }
    }

    void Flip()
    {
        if (Input.GetAxis("Horizontal") > 0)
            transform.localRotation = Quaternion.Euler(0, 0, 0);

        if (Input.GetAxis("Horizontal") < 0)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
    }


    void OnTriggerEnter2D(Collider2D shit)
    {
        if (shit.gameObject.tag == "gold") {//если триггерный объект с тегом gold
            Destroy(shit.gameObject); //уничтожаем монету при соприкосновении
            Gold++;
        }

        if (shit.gameObject.tag == "Finish")
        {
            Application.LoadLevel("Scene2");
        }
    }



    void Money(int Gold) {
        if (Gold == 1)
        {
            money1.SetActive(true);
        }
        else if (Gold == 2)
        {
            money2.SetActive(true);
        }
        else if (Gold == 3)
        {
            money3.SetActive(true);
        }
    }


    void OnCollisionEnter2D(Collision2D shit) {
        if (shit.gameObject.tag == "Enemy") {
            anim.SetBool("death", true);
            Invoke("ReloadLevel", 1);
        }
    }

    void ReloadLevel() {
        Application.LoadLevel(Application.loadedLevel);
    }
}

