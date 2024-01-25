using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBoomerang : MonoBehaviour
{
    [SerializeField]
    private MonsterController monsterC;
    private KirbyMove Kirby;
    private GameObject monster;
    public float BoomerangSpeed;
    private SpriteRenderer spriteRenderer;
    private AudioManager audio;
    public Vector2 boomerangDirection;
    private float boomerangDirection_float;
    //public Transform returnOb;
    private bool isDoublejump;
    private Vector2 monsterCPosition;
    float direction ;

    // Update is called once per frame
    private void Awake()
    {
        audio = FindObjectOfType<AudioManager>();
         monsterC =GameObject.FindGameObjectWithTag("Monster").GetComponent<MonsterController>();
        //spriteRenderer = monsterC.GetComponent<SpriteRenderer>();

        if (monsterC.spriteRenderer.flipX == true)
        {
            direction = -1f;
        }
        else
        {
            direction = 1f;
        }
        //direction = monsterC.direction;
        boomerangDirection = new Vector2(direction, 0);
     


    }
    private void Start()
    {

        audio.Play("boomerang");
    }
    void Update()
    {
        
       
      
        //transform.position += (Vector3)boomerangDirection * BoomerangSpeed * Time.deltaTime;
        //부메랑의 나가는방향 ,속도
        // 보고있는 방향으로 스피드값만큼 이동
        transform.Translate(boomerangDirection * BoomerangSpeed * Time.deltaTime);

        //나가고 점점 속도가 줄어듬

        BoomerangSpeed -= direction* direction * Time.deltaTime *45f ;
        // 부메랑이 멈출때쯤 커비 위치 저장
        //if (BoomerangSpeed < 1 && BoomerangSpeed > -1)
        //{
        //    monsterCPosition = monsterC.transform.position;
        //}
        //// 돌아올땐 저장한 위치로 이동
        //if (BoomerangSpeed < 0)
        //{

        //    transform.position = Vector2.MoveTowards(transform.position, monsterCPosition, direction * 4 * Time.deltaTime);
        //}
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            Destroy(gameObject);
        }
        
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Monster") )
    //    {
    //        Destroy(gameObject);
    //    }
    //}
}
