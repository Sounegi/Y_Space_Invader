using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    SpriteRenderer render;
    bool enterScreen;
    // Start is called before the first frame update
    void Start()
    {
        render = transform.GetComponent<SpriteRenderer>();
        enterScreen = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        var cam = Camera.main;
        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);
        if (viewPos.x <= 0 || viewPos.x >= 1 || viewPos.y <= 0 || viewPos.y >= 1)
        {
            if(enterScreen)
                Destroy(this.gameObject);
        }
        else
        {
            enterScreen = true;
        }

    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().TakeDamage();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(collision.gameObject.GetComponent<Rigidbody2D>().velocity/5f, ForceMode2D.Impulse);
            ObjectPoolManager.DestroyPooled(collision.gameObject);

        }
    }
}
