using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : Monster
{

    private KirbyMove kirby;
    //private BoxCollider2D Kirby_col;
    //private Animator animator;
    public GameObject boomerang;

    protected AudioManager audio;
    public SpriteRenderer spriteRenderer;
    private bool isInRange;
    private bool Attack;
    private bool aniPlay;
  
    
    public float detectionRange = 5f; // 몬스터의 감지 범위
    public float direction;
    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ 몬스터 이동 변수들
    private Rigidbody2D rb;
    private bool isMoving;
    private float moveDirection; // -1: 왼쪽, 0: 정지, 1: 오른쪽
  
    private bool shouldStopRandomMovement;
    public float moveSpeed;
  




   
    private void OnDrawGizmosSelected()
    {
        // 감지 범위를 그리기 위한 색상 설정
        Gizmos.color = Color.red;

        // 몬스터의 위치에서 감지 범위 그리기
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
    private Transform player;

    private void Start()
    {
        switch (typeIndex)
        {
            case 0:
                type = Type.Normal;
                break;
            case 1:
                type = Type.Cutter;
                break;
        }
        audio = FindObjectOfType<AudioManager>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Kirby_col = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        isMoving = false;
         StartCoroutine(RandomMovement());
        Attack = true;

    }

    private void Update()
    {
      
       
        animator.SetBool("inhaling", inhaling);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        // 플레이어와 몬스터 간의 거리 계산
        if (isInRange == true && Attack)
        {
            StartCoroutine(Attack_co());
            animator.SetTrigger("Attack");
           

        }



        // 플레이어가 감지 범위 내에 들어온 경우
        if (distanceToPlayer <= detectionRange)
        {
            shouldStopRandomMovement = true;
            if (player.position.x - transform.position.x <0)
            {
                spriteRenderer.flipX = true;
                direction = -1;
            }
            else
            {
                spriteRenderer.flipX = false;
                direction = 1;
            }
            if (isInRange == false && aniPlay ==false)
            {
                aniPlay = true;
                StartCoroutine(FindDelay());
                //animator.Play("FindPlayer");
            }

            
            
            // 원하는 반응을 여기에 추가하십시오 (예: 몬스터의 공격 또는 추격 로직)
            //Debug.Log("플레이어 감지!");
        }else
        {
           
            isInRange = false;
            aniPlay = false;
        }
        //Vector2 StartPositon = new Vector2(direction * 2f + transform.position.x, transform.position.y);
        if (isDie && !DieEffectisOn)
        {

            StartCoroutine(DieEffect_co());
        }
        if (isMoving && !isInRange && !isDie)
        {
           
            // 움직임을 적용합니다.
            rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

            if(moveDirection < 0)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
        else 
        {
            // 정지합니다.
            rb.velocity = Vector2.zero;
        }
        animator.SetBool("IsMoving", isMoving);
    }
    IEnumerator Attack_co()
    {
        Attack = false;
        yield return new WaitForSeconds(4f);
        Attack = true;
    }
    IEnumerator FindDelay()
    {
        animator.Play("FindPlayer");
        yield return new WaitForSeconds(2f);
        isInRange = true;
    }
    protected virtual IEnumerator Die_co()
    {
        //Rigidbody2D thisrigidbody2D = GetComponent<Rigidbody2D>();
        //thisrigidbody2D.velocity = Vector2.zero;
        
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    protected virtual IEnumerator DieEffect_co()
    {
        Color monsterA = spriteRenderer.color;

        DieEffectisOn = true;
        spriteRenderer.color = new Color(monsterA.r, monsterA.g, monsterA.b, 0.7f);
       
        yield return new WaitForSeconds(0.2f);
        DieEffectisOn = false;
        spriteRenderer.color = monsterA;
       
       
    }
   
    private IEnumerator RandomMovement()
    {
            Queue<int> Action = new Queue<int>();
        while (true) { 
        
            // 랜덤한 시간 동안 대기합니다.
            

            int[] randDirection = { -1, 0, 1 };
            // 랜덤하게 움직일 방향을 설정합니다.
            for (int i = 0; i < 10; i++)
            {
                int RandIndex = Random.Range(0, 2);
                int temp = randDirection[RandIndex];
                randDirection[RandIndex] = randDirection[RandIndex + 1];
                randDirection[RandIndex + 1] = temp;
            }
            for (int i = 0; i < randDirection.Length; i++)
            {
                Action.Enqueue(randDirection[i]);
            }


        while (Action.Count > 0 )
        {
                if (shouldStopRandomMovement)
                {
                    yield break; // 코루틴 종료
                }
                yield return new WaitForSeconds(1f);

            moveDirection = Action.Dequeue();
            
            // 움직임 상태를 활성화합니다.
            if(moveDirection != 0) { 
            isMoving = true;
            }

            // 랜덤한 시간 동안 움직입니다.
            yield return new WaitForSeconds(1f);

            // 움직임 상태를 비활성화하여 정지합니다.
            isMoving = false;
        }
        }
       
    }

    private void AttacktoPlayer()
    {
        Vector2 StartPositon = new Vector2(direction * 1.7f + transform.position.x, transform.position.y);
        Instantiate(boomerang,StartPositon, Quaternion.identity);
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D thisCollider = GetComponent<Collider2D>();
        if (collision.CompareTag("PlayerSkil"))
        {
            isDie = true;
            
            animator.Play("Die");
            
            
            Physics2D.IgnoreCollision(thisCollider, Kirby_col);

            StartCoroutine(Die_co());

        }
    }

}
