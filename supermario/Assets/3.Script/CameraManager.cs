using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public BoxCollider2D mapBound;
    public GameObject kirby;
    private Camera camera;
    [SerializeField] private float movespeed;
    private Vector3 kirbyPosition;
    private Vector3 maxbound;
    private Vector3 minbound;

    private float halfwidth;
    private float halfheight;

    private void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        camera = GetComponent<Camera>();
        
        minbound = mapBound.bounds.min;
        maxbound = mapBound.bounds.max;

        halfheight = camera.orthographicSize;
        halfwidth = halfheight * Screen.width / Screen.height;
    }
    private void Update()
    {
        if (kirby != null)
        {
            kirbyPosition.Set(kirby.transform.position.x, kirby.transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, kirbyPosition, movespeed * Time.deltaTime);
        }
        //transform.position =new Vector3(kirby.transform.position.x, kirby.transform.position.y, transform.position.z);

        float clampedX = Mathf.Clamp(transform.position.x, minbound.x + halfwidth, maxbound.x- halfwidth);
        float clampedY = Mathf.Clamp(transform.position.y, minbound.y + halfheight, maxbound.y - halfheight);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        
        
    }
}
