using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public float StarSpeed;
    private Transform kirby_Transform;
    public Vector2 StarDirection;
    private float direction;
    [SerializeField]
    private float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        kirby_Transform = GameObject.Find("Kirby").GetComponent<Transform>();
       


        if (kirby_Transform.localScale.x < 0)
        {
            direction = -1f;
        }
        else
        {
            direction = 1f;
        }
        //direction = monsterC.direction;
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
            Debug.Log("È®ÀÎ");
            Destroy(this.gameObject);
        }
    }
}
