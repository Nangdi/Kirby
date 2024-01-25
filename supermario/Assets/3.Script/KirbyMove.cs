using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

//enum Type
//{
//    Nomal,
//    Cutter
//}
public class KirbyMove : MonoBehaviour
{
    public LayerMask groundLayer;
    public RuntimeAnimatorController[] animators;
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D Kirby_r;
    public BoxCollider2D KirbyCol;
    private List<Collider2D> monsterColliders;
    private List<Collider2D> collidersToRemove;
    public GameObject boomerangObject;
    public GameObject S_Effect;
    Vector2 currentPosition;
    public GameObject Star;
    public GameObject inhaleRange;
    private WaitForSeconds boomerangDelay;
    private AudioManager audio;
    private BgmManager Bgm;
    Vector2 hit;

    

    private float limit;   //�����������¿����� ���ѽð�
    private float StartTime; //�����������۽ð�
    private float airPos;  //�����������¿����� �����Ŀ���
    public float JumpPorce; //�����Ŀ�
    public float movespeed; // ���꽺�ǵ�
    private float highestY; // ĳ������ �ְ� ���� ���� ����(���Ҷ���)
    private float xPos;
    private bool isDoubleJump;  //ǳ�������϶�(�����������´�?)
    private bool isGrounded; //������Ҵ�?
    private bool isInhale; // �������̴�?
    private bool isRun; //�޸����ִ�?
    private bool getBoomerang = true; // �θ޶������϶� �θ޶��� �������ִ�?
    private bool isHitMotion; // ��Ʈ������̳�?
    public bool big; //���Լ����� Ŀ�����´�?
    private int layerA ;
    private int layerB;
    private int jumpCount; //������ �ѹ�������
    public Vector2 direction;
    //private Type currentType;
    private float boomerangDirection ; //�θ޶� ����.
    

    public float GetboomerangDirection()
    {
        return boomerangDirection;
    }


    


    private void Start()
    {
        Bgm = FindObjectOfType<BgmManager>();
        audio = FindObjectOfType<AudioManager>();
        collidersToRemove = new List<Collider2D>();
        monsterColliders = GameObject.FindGameObjectsWithTag("Monster")
    .Select(monster => monster.GetComponent<Collider2D>()).Where(col => col != null)
    .ToList();
        boomerangDelay = new WaitForSeconds(0.3f);
        
        boomerangDirection = 1;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Kirby_r = GetComponent<Rigidbody2D>();
        direction = new Vector2();
        highestY = transform.position.y; // ������ �� �ʱ�ȭ
        layerA = LayerMask.NameToLayer("Player");
        layerB = LayerMask.NameToLayer("Monster");

        switch (GameManager.Instance.currentType)
        {
            case Type.Normal:
                animator.runtimeAnimatorController = animators[0];
              
               

                break;
            case Type.Cutter:
                animator.runtimeAnimatorController = animators[1];
                //animator.Play("Basic");
                break;

        }


    }

    private void Update()
    {
        xPos = 0;
       
        

        isRun = animator.GetBool("isRun");
        isDoubleJump = animator.GetBool("doubleJump");
        //animator.SetFloat("xPos", xPos);
        float runspeed = isRun ? 1.5f : 1f;

        currentPosition = new Vector2(transform.position.x + boomerangDirection * 2f, transform.position.y);

        Kirbyfloar();
        
        if (!GameManager.Instance.isDie ) { 
        //�޸��� , �ȱ� �޼ҵ�
        MoveMent(xPos, ref direction);
        //�̵��ϴ� �ڵ�
        transform.Translate(direction * movespeed * runspeed * Time.deltaTime);

        //���� �������� �޼ҵ�

        JumpMovement();
        // ������������ �޼ҵ�( ����������� , SŰ�� �Է�������)

        JumpTrun();
        JumpTimeLimit();
       
        if (Input.GetKeyDown(KeyCode.S))
        {
            if ( isDoubleJump)
            {

                StopCoroutine(JumpDisInhale_co());
                StartCoroutine(JumpDisInhale_co());
            }


            if (getBoomerang && !isDoubleJump && GameManager.Instance.currentType == Type.Cutter && !big)
            {

                getBoomerang = false;
                animator.Play("S_Attack");
                Instantiate(boomerangObject, currentPosition, Quaternion.identity);
                StartCoroutine(BoomerangDelay_co());


            }
        }
        }

        else
        {
            
            
            animator.Play("Die");
          
            Vector2 Boss_vel = Kirby_r.velocity;
            KirbyCol.isTrigger = true;
            Kirby_r.AddForce(new Vector2(0, 2f));
          
            
               if(GameManager.Instance.lifecount > 1) { 
            StartCoroutine(ReStart());
            }
            else
            {
                StartCoroutine(GameOver());
            }

        }
        
        //if (Input.GetKeyDown(KeyCode.S) && isDoubleJump)
        //{

        //    StopCoroutine(JumpDisInhale_co());
        //    StartCoroutine(JumpDisInhale_co());
        //}

        //�⺻���¿��� S Ű�ٿ�� ���Ը��
        if (!big && Input.GetKey(KeyCode.S) && 
            isGrounded && animator.GetBool("isWalking") == false &&
            GameManager.Instance.currentType == Type.Normal &&
            !GameManager.Instance.isHit)
        {

            if (isInhale == false)
            {

                animator.Play("Inhale");
                audio.Play("inhale");
            }
            isInhale = true;
            inhaleRange.SetActive(true);
        }
        else
        {
            audio.Stop("inhale");
            isInhale = false;
            inhaleRange.SetActive(false);
        }


        //Ŀ�� �����϶� S Ű
        //if (Input.GetKeyDown(KeyCode.S) && getBoomerang && !isDoubleJump && gameManager.currentType == Type.Cutter && !big)
        //{

        //    getBoomerang = false;
        //    animator.Play("S_Attack");
        //    Instantiate(boomerangObject, currentPosition, Quaternion.identity);
        //    StartCoroutine(BoomerangDelay_co());


        //}
        //if (Input.GetKey(KeyCode.S))
        //{

        //}
        //�����������¶��

        animator.SetBool("isGrounded", isGrounded); //���� �ִ°�
        animator.SetBool("isInhale", isInhale);
        //animator.SetBool("", isInhale);



        if (big)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                switch (GameManager.Instance.currentType)
                {
                    case Type.Normal:
                        //animator.runtimeAnimatorController = animators[0];
                        animator.Play("RDisInhale");
                        GameManager.Instance.UITpye();

                        break;
                    case Type.Cutter:
                        animator.runtimeAnimatorController = animators[1];
                        GameManager.Instance.UITpye();
                        audio.Play("transform");
                        break;

                }
                StartCoroutine(BigDelay());
            }

        }

        //if (gameManager.isHit )
        //{
        //    Debug.Log("��Ʈ���");
        //    StartCoroutine(HitMotion_co());

        //}

        if (GameManager.Instance.gameClear)
        {
            animator.Play("Clear");
            GameManager.Instance.gameClear = false;
        }

        if ((!isGrounded || !animator.GetBool("isWalking"))  && audio.IsPlay("move"))
        {
          
            MoveSoundStop();
        }
        
    }//������Ʈ
    private void StarInstan()
    {
        Instantiate(Star, currentPosition, Quaternion.identity);
    }
    private void JumpTrun()
    {//�������� ��ġ����
        if (transform.position.y >= highestY)
        {
            highestY = transform.position.y;
        }
        //������ ������� �����ö� ����
        else if (transform.position.y < highestY && !isDoubleJump && animator.GetBool("Jump"))
        {

            animator.SetBool("Jump", false);
            if (!big) { 
            animator.Play("Jump");
            }
        }
    }
    private void JumpTimeLimit()
    {
        limit = Time.time - StartTime;

        if (animator.GetBool("doubleJump") && limit >= 5)
        {

            //���� �ִϸ��̼� ����
           
            animator.Play("JumpLimit");

        }
        if (animator.GetBool("doubleJump") && limit >= 8)
        {
            
            //���������������

            StopCoroutine(JumpDisInhale_co());
            StartCoroutine(JumpDisInhale_co());


        }
    }
    private void JumpMovement()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {


            if (isGrounded)
            {
                audio.Play("jump");
                //Debug.Log("����");
                animator.SetBool("Jump", true);
                Kirby_r.AddForce(new Vector3(0, JumpPorce, 0));
                jumpCount = 1;


            }
            else if (/*animator.GetBool("Jump")*/!isGrounded && !isDoubleJump && jumpCount == 1 && !big && !GameManager.Instance.isHit)
            {
                jumpCount--;
                
                animator.SetBool("Jump", false);
                animator.SetBool("doubleJump", true);
                Kirby_r.velocity = Vector2.zero;
                
                Kirby_r.gravityScale = 0.1f;
                StartTime = Time.time;

                airPos = 1500f;



            }
            else if (animator.GetBool("doubleJump") /*&&Kirby_r.velocity.y <=0*/ && Kirby_r.gravityScale < 0.2f)
            {
                audio.Play("fly");

                //if (airPos > 500f)
                //{
                //    airPos -= 100f;
                //}
                //else
                //{
                //    airPos = 500f;
                //}






                StopCoroutine(Jump_co());
                StartCoroutine(Jump_co());
            }
        }
    }
    private void MoveSoundPlay()
    {
        audio.Play("move");
    }
    private void MoveSoundStop()
    {
        audio.Stop("move");
    }
    private void disInhalesound()
    {
        audio.Play("disfly");
    }
    public void BoomerangSound()
    {
        audio.Play("boomerang");
    }
   private void DieBgm()
    {
        Bgm.BgmStop();
        Bgm.BgmPlay(4);
    }

    private void Kirbyfloar()
    {
        Bounds bounds = KirbyCol.bounds;
        Vector2 left = new Vector2(bounds.min.x + 0.15f, bounds.min.y);
        Vector2 right = new Vector2(bounds.max.x - 0.15f, bounds.min.y);
        RaycastHit2D hit = Physics2D.Raycast(left, Vector2.down, 0.1f, groundLayer);
        RaycastHit2D hit1 = Physics2D.Raycast(right, Vector2.down, 0.1f, groundLayer);

        // ���� �������� ������ �׸��ϴ�.
        Debug.DrawRay(left, Vector2.down * 0.1f, Color.red);
        Debug.DrawRay(right, Vector2.down * 0.1f, Color.red);
        if (hit.collider != null || hit1.collider != null)
        {
            //Debug.Log("ĳ���Ͱ� ���� ��ҽ��ϴ�.");
            isGrounded = true;
            highestY = transform.position.y;
            JumpDisInhale();
           
        }
        else
        {
            isGrounded = false;
        }


    }
    private void MoveMent(float xPos, ref Vector2 direction)
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            xPos = -1f;
            //animator.SetFloat("xPos", xPos);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            xPos = 1f;
            //animator.SetFloat("xPos", xPos);
        }
        if (xPos != 0)
        {
            boomerangDirection = xPos;
           
        }
       
        direction = new Vector2(xPos, 0f);

        animator.SetBool("isWalking", false);



        if (xPos != 0)//�����϶�
        {
            //Debug.Log("�ȱ�");

            animator.SetBool("isWalking", true); // �����Ͻ� wailking true

            #region �¿�����޼���
            if (xPos == -1) //�����̵��϶�
            {
                
                transform.localScale = new Vector3(-5, 5, 5);
            }
            else//�������̵��϶�
            {
             
                transform.localScale = new Vector3(5, 5, 5);

            }
            #endregion
            if (Input.GetKey(KeyCode.A))
            {
                
                animator.SetBool("isRun", true);
            }
            else
            {
                animator.SetBool("isRun", false);
            }
        }
        else
        {
            
            animator.SetBool("isRun", false);
        }
    }
    //���׶����� ����
    private IEnumerator JumpDisInhale_co()
    {
        animator.SetBool("doubleJump", false);
        yield return new WaitUntil(() => Kirby_r.gravityScale <= 0.2f);

        Kirby_r.gravityScale = 3f;
        Kirby_r.mass = 2f;
        yield break;

    }
    private void JumpDisInhale()
    {
        animator.SetBool("doubleJump", false);

        Kirby_r.gravityScale = 3f;
        Kirby_r.mass = 2f;
    }
    IEnumerator BigDelay()
    {
        yield return new WaitForSeconds(1f);
        big = false;
    }
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    isGrounded = false;
    //}

    IEnumerator Jump_co()
    {

        Kirby_r.mass = 3;
        Kirby_r.gravityScale = 3;


        Kirby_r.AddForce(new Vector2(0, airPos));

        yield return new WaitForSeconds(0.1f);


        yield return new WaitUntil(() => Kirby_r.velocity.y < 0);
        
        Kirby_r.mass = 2;
        Kirby_r.gravityScale = 0.1f;
        //for (float i = 3f; i >= 0.1f; i -= 0.1f)
        //{
        //    //Debug.Log("���� ����?");
        //    Kirby_r.gravityScale = i;
        //    yield return null;

        //}
    }
    IEnumerator BoomerangDelay_co()
    {
        
        yield return boomerangDelay;
        getBoomerang = true;
    }
    public bool GetIsDoubleJump()
    {
        return isDoubleJump;
    }
    IEnumerator ReStart()
    {
       
        yield return new WaitForSeconds(3.0f);
        GameManager.Instance.lifecount--;
        SceneManager.LoadScene("Stage1_1");
        GameManager.Instance.isDie = false;
        GameManager.Instance.CurHP = 3;
        GameManager.Instance.currentType = Type.Normal;
        KirbyCol.isTrigger = true;
        //animator.Play("basic");

    }
    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("GameOver");
    }

    public void Effect()
    {
        Instantiate(S_Effect, currentPosition, Quaternion.identity);
    }
    IEnumerator HitMotion_co()
    {
        while (GameManager.Instance.isHit)
        {


            Color KirbyA = spriteRenderer.color;
            spriteRenderer.color = new Color(KirbyA.r, KirbyA.g, KirbyA.b, 0.7f);

            yield return new WaitForSeconds(0.3f);


            spriteRenderer.color = KirbyA;

            yield return new WaitForSeconds(0.3f);
        }
    }
    IEnumerator Hit_co()
    {
        Physics2D.IgnoreLayerCollision(layerA, layerB, true);
        yield return new WaitForSeconds(3f);
        GameManager.Instance.isHit = false;
        Physics2D.IgnoreLayerCollision(layerA, layerB, false);
        //foreach (Collider2D colliderToRemove in collidersToRemove)
        //{
        //    monsterColliders.Remove(colliderToRemove);
        //}
    }
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    MonsterController monsterScript = collision.gameObject.GetComponent<MonsterController>();
    //    monsterScript.inhaling = false;
    //}
   private void PlayerHit()
    {
        hit = new Vector2(-boomerangDirection * 3, 10);
        Kirby_r.velocity = hit;  //������˹�
        GameManager.Instance.isHit = true;   // �Ұ� Ʈ��� (�����̴¸�� , ��������)
        animator.Play("Hit"); // �´� ������
        audio.Play("hitsound");
        StartCoroutine(Hit_co()); //3�ʵ� �Ұ� Ʈ��
        StartCoroutine(HitMotion_co());//3�ʰ� ������
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Boomerang" && !isDoubleJump)
        {
            animator.Play("Catch");

        }
        if (collision.CompareTag("MonsterWeapon") && !GameManager.Instance.isHit)
        {
           
            PlayerHit();
         
            GameManager.Instance.CurHP -= 1f;
            Debug.Log(GameManager.Instance.CurHP);
        }
    }
   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Monster monsterScript = collision.gameObject.GetComponent<Monster>();
       

        if (collision.gameObject.CompareTag("Monster") && !GameManager.Instance.isHit )
        {



            if (isInhale)
            {
                GameManager.Instance.currentType = monsterScript.type;
                Debug.Log("ȹ���� Ÿ�� : " + monsterScript.type);
                animator.Play("SuccessInhale");
                Destroy(collision.gameObject);
                big = true;
            }
            else
                {
              
                //Vector2 hit = new Vector2(xPos * 15, 15);

               
                

                PlayerHit();
                GameManager.Instance.CurHP -= 1f;
                Debug.Log("����HP : " + GameManager.Instance.CurHP);
            }

        }
        
    }
    
    
}