using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackGroundRolling : MonoBehaviour
{
    private float width;
    public Vector3 rolling_direction;
    public float moveSpeed;
    private void Start()
    {

        width = GetComponent<BoxCollider2D>().size.x * transform.localScale.x;
    }
    void Update()
    {
        //transform.position += rolling_direction * moveSpeed * Time.deltaTime;
        transform.Translate(rolling_direction * moveSpeed * Time.deltaTime);

        if (transform.position.x <= -width)
        {
            Vector2 offset = new Vector2(width * 2f, 0);
            transform.position = (Vector2)transform.position + offset;
        }

        if (Input.anyKey)
        {
            SceneManager.LoadScene("Stage1_1");
        }
    }
}
