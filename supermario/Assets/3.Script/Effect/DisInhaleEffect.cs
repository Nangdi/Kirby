using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisInhaleEffect : MonoBehaviour
{
    public float movespeed;
    Vector3 position;
    private float direction;
    private KirbyMove Kirby;
    private SpriteRenderer spriteRenderer;
    private Transform kirby_Transform;
    // Start is called before the first frame update
  
    void Start()
    {
        Debug.Log(direction);
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameObject.FindGameObjectWithTag("Player").TryGetComponent<KirbyMove>(out Kirby);
        direction = Kirby.GetboomerangDirection();
        position = new Vector3(direction, 0, 0);
        Debug.Log(direction);
    }

    // Update is called once per frame
    void Update()
    {
        if(direction > 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
        transform.position += position * movespeed * Time.deltaTime;
        //transform.Translate(position * movespeed * Time.deltaTime);
    }
    public void Effect_Destroy()
    {
        Destroy(gameObject);
    }
}
