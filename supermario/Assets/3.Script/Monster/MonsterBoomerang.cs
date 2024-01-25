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
        //�θ޶��� �����¹��� ,�ӵ�
        // �����ִ� �������� ���ǵ尪��ŭ �̵�
        transform.Translate(boomerangDirection * BoomerangSpeed * Time.deltaTime);

        //������ ���� �ӵ��� �پ��

        BoomerangSpeed -= direction* direction * Time.deltaTime *45f ;
        // �θ޶��� ���⶧�� Ŀ�� ��ġ ����
        //if (BoomerangSpeed < 1 && BoomerangSpeed > -1)
        //{
        //    monsterCPosition = monsterC.transform.position;
        //}
        //// ���ƿö� ������ ��ġ�� �̵�
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
