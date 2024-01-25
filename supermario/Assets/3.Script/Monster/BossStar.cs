using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStar : MonoBehaviour
{
    public float StarSpeed;
    public BossMove Dedede;
  
    private float PB;
    public Vector2 StarDirection;
    private float direction;
    [SerializeField]
    private float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        Dedede = GameObject.FindGameObjectWithTag("Monster").GetComponent<BossMove>();
        direction = Dedede.GetBossDirection();
        Debug.Log(direction);
        //if (Dedede_Transform.localScale.x > 0)
        //{
        //    Debug.Log("다이랙션 1번");
        //    direction = -1f;
        //}
        //else
        //{
        //    Debug.Log("다이랙션 2번");
        //    direction = 1f;
        //}
        StarDirection = new Vector2(direction, 0);

    }

    private void Update()
    {
        transform.position += (Vector3)StarDirection * StarSpeed * Time.deltaTime;
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);

        if (collision.CompareTag("Map") || collision.CompareTag("Wall"))
        {
            Debug.Log("확인");
            Destroy(this.gameObject);
        }
    }
}
