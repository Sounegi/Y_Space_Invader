using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    SpriteRenderer render;
    [SerializeField] bool player;

    private float screenWidth;
    private float screenHeight;

    private void Start()
    {
        render = gameObject.GetComponent<SpriteRenderer>();

    }

    private void FixedUpdate()
    {
        var cam = Camera.main;
        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
        {
            //Inbound
        }
        else
        {
            ObjectPoolManager.DestroyPooled(gameObject);
            //Destroy(this.gameObject);
        }
        // if(transform.position <)
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(player && collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage();
            
            ObjectPoolManager.DestroyPooled(gameObject);
        }
        else if(!player && collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().TakeDamage();

            ObjectPoolManager.DestroyPooled(gameObject);
        }
    }
}
