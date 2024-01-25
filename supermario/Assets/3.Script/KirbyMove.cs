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

    

    private float limit;   //더블점프상태에서의 제한시간
    private float StartTime; //더블점프시작시간
    private float airPos;  //더블점프상태에서의 점프파워ㅏ
    public float JumpPorce; //점프파워
    public float movespeed; // 무브스피드
    private float highestY; // 캐릭터의 최고 높이 저장 변수(턴할때씀)
    private float xPos;
    private bool isDoubleJump;  //풍선상태일때(더블점프상태니?)
    private bool isGrounded; //땋에닿았니?
    private bool isInhale; // 흡입중이니?
    private bool isRun; //달리고있니?
    private bool getBoomerang = true; // 부메랑상태일때 부메랑을 가지고있니?
    private bool isHitMotion; // 히트모션중이냐?
    public bool big; //흡입성공후 커진상태니?
    private int layerA ;
    private int layerB;
    private int jumpCount; //점프는 한번만가능
    public Vector2 direction;
    //private Type currentType;
    private float boomerangDirection ; //부메랑 방향.
    

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
        highestY = transform.position.y; // 시작할 때 초기화
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
        //달리기 , 걷기 메소드
        MoveMent(xPos, ref direction);
        //이동하는 코드
        transform.Translate(direction * movespeed * runspeed * Time.deltaTime);

        //점프 더블점프 메소드

        JumpMovement();
        // 더블점프해제 메소드( 땅에닿았을때 , S키를 입력했을때)

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

        //기본상태에서 S 키다운시 흡입모션
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


        //커터 상태일때 S 키
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
        //더블점프상태라면

        animator.SetBool("isGrounded", isGrounded); //땅에 있는가
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
        //    Debug.Log("히트모션");
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
        
    }//업데이트
    private void StarInstan()
    {
        Instantiate(Star, currentPosition, Quaternion.identity);
    }
    private void JumpTrun()
    {//고점까지 위치저장
        if (transform.position.y >= highestY)
        {
            highestY = transform.position.y;
        }
        //점프가 고점찍고 내려올때 실행
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

            //힘든 애니매이션 실행
           
            animator.Play("JumpLimit");

        }
        if (animator.GetBool("doubleJump") && limit >= 8)
        {
            
            //더블점프모드해제

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
                //Debug.Log("점프");
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

        // 레이 시작점과 방향을 그립니다.
        Debug.DrawRay(left, Vector2.down * 0.1f, Color.red);
        Debug.DrawRay(right, Vector2.down * 0.1f, Color.red);
        if (hit.collider != null || hit1.collider != null)
        {
            //Debug.Log("캐릭터가 땅에 닿았습니다.");
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



        if (xPos != 0)//움직일때
        {
            //Debug.Log("걷기");

            animator.SetBool("isWalking", true); // 움직일시 wailking true

            #region 좌우반전메서드
            if (xPos == -1) //왼쪽이동일때
            {
                
                transform.localScale = new Vector3(-5, 5, 5);
            }
            else//오른쪽이동일때
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
    //버그때문에 넣음
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
        //    //Debug.Log("포문 도니?");
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
        Kirby_r.velocity = hit;  //맞으면넉백
        GameManager.Instance.isHit = true;   // 불값 트루로 (깜빡이는모션 , 무적판정)
        animator.Play("Hit"); // 맞는 모션재생
        audio.Play("hitsound");
        StartCoroutine(Hit_co()); //3초뒤 불값 트루
        StartCoroutine(HitMotion_co());//3초간 깜빡임
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
                Debug.Log("획득한 타입 : " + monsterScript.type);
                animator.Play("SuccessInhale");
                Destroy(collision.gameObject);
                big = true;
            }
            else
                {
              
                //Vector2 hit = new Vector2(xPos * 15, 15);

               
                

                PlayerHit();
                GameManager.Instance.CurHP -= 1f;
                Debug.Log("남은HP : " + GameManager.Instance.CurHP);
            }

        }
        
    }
    
    
}