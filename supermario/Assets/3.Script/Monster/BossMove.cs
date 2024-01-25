using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossMove : MonoBehaviour
{
    private float BossCurHp = 50;
    private float BossMaxHp = 50;
    [SerializeField]
    private Slider HPbar;

    private Animator animator;
    public float moveSpeed = 2.0f; // 몬스터 이동 속도
    public float detectionRadius = 10.0f; // 감지 범위 반지름
    private bool isCanAttack = true;
    private bool isAttackDelay;
    private bool StartBattle;
    private bool isDie;
    private AudioManager audio;

    private float detectionRange;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D Boss_col;
    private Rigidbody2D rb_boss;

    private Transform player;
    private Transform Dedede;
    public GameObject HammerRange;
    public GameObject HammerAtk;
    public GameObject StarRange;
    public GameObject Star;
    public GameObject Box;
   
    private float BossDirection;
    public float GetBossDirection()
    {
        return BossDirection;
    }
    Vector2 currentPosition;
    Vector2 direction;
    
    private void Start()
    {
        audio = FindObjectOfType<AudioManager>();
        rb_boss = GetComponent<Rigidbody2D>();
        Boss_col = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
           animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Dedede = GetComponent<Transform>();

        audio.Play("bossmeet");
    }
    private void Update()
    {
        
        BossHp();
        if(BossCurHp <= 0)
        {
            if (!isDie)
            {
                BossDie();
                isDie = true;
            }

           
        }
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        
        if (direction.x >0)
        {
            Dedede.localScale= new Vector3(-7, 7, 7);
            //오른쪽
            BossDirection = 1;
        }
        else
        {
            Dedede.localScale = new Vector3(7, 7, 7);
            //왼쪽
            BossDirection = -1;
        }
        currentPosition = new Vector2(transform.position.x + direction.x * 2.5f, transform.position.y-0.5f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player") && !isAttackDelay && !isDie)
            {
                StartBattle = true;
                    // 플레이어가 감지 범위 내에 있을 때 이동 로직을 구현
                    direction = (collider.transform.position - transform.position).normalized;
                transform.Translate(direction * moveSpeed * Time.deltaTime);
            }
           
        }
      
        animator.SetBool("isDelay", isAttackDelay);

        if(isCanAttack && !isAttackDelay && StartBattle && !isDie) {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            isCanAttack = false;
            StartCoroutine(CanAttack_co());
            if(distanceToPlayer > 4  )
            {
                Debug.Log(" 패턴1번");
                animator.Play("BossStar");
                isAttackDelay = true;
                StartCoroutine(AttackDelay_co());
            }else if( distanceToPlayer <=4)
            {
                Debug.Log(" 패턴2번");
                animator.Play("Attack");
                isAttackDelay = true;
                StartCoroutine(AttackDelay_co());
            }
            //else if( distanceToPlayer <= 3)
            //{
            //    Debug.Log(" 패턴3번");
            //    isAttackDelay = true;
            //    StartCoroutine(AttackDelay_co());
            //}
        }

    }
    IEnumerator AttackDelay_co()
    {
        yield return new WaitForSeconds(2f);
        isAttackDelay = false;

    }
    IEnumerator CanAttack_co()
    {
        yield return new WaitForSeconds(4);
        isCanAttack = true; 


    }
    private void hammerAtktrue()
    {
        HammerAtk.SetActive(true);
    }
    private void hammerAtkfalse()
    {
        HammerAtk.SetActive(false);
    }
    private void hammerRangetrue()
    {
        HammerRange.SetActive(true);
    }
    private void hammerRangefalse()
    {
        HammerRange.SetActive(false);
    }
    private void StarRangetrue()
    {
        StarRange.SetActive(true);
    }
    private void StarRangefalse()
    {
        StarRange.SetActive(false);
    }
    private void StarInstan()
    {
        GameObject currentStar = Instantiate(Star, currentPosition, Quaternion.identity);
        
    }
    private void Sound_hammer()
    {
        audio.Play("bosshammer");
    }

    private void Sound_Star()
    {
        audio.Play("disfly");
    }
    private void SetRange()
    {
        if (HammerRange.activeSelf == true  || StarRange.activeSelf == true)
        {
            if(HammerRange.activeSelf == true)
            {
                HammerRange.SetActive(false);
            }
            else
            {
                StarRange.SetActive(false);
            }

        }
        return;
    }
    private void BossHp()
    {
        HPbar.value = Mathf.Lerp(HPbar.value, BossCurHp / BossMaxHp, Time.deltaTime * 10);
    }
    private void BossDie()
    {
        Destroy(HPbar);
        animator.Play("BossDie");
        Debug.Log("1번");
         Vector2 Boss_vel = rb_boss.velocity;
        rb_boss.AddForce(new Vector2(0, 2000f));
        Debug.Log("2번 : "+ Boss_vel);
        Boss_col.isTrigger = true;
        Box.SetActive(true);

        //Color a = spriteRenderer.color;
        //spriteRenderer.color = new Color(a.r, a.g, a.b, 0.6f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerSkil"))
        {
            BossCurHp--;
            audio.Play("hitsound");   
            animator.Play("BossHit");
        }
    }

}
