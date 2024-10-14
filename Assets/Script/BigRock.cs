using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigRock : Rock
{
    [SerializeField] GameObject rock;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(collision.gameObject.GetComponent<Rigidbody2D>().velocity / 5f, ForceMode2D.Impulse);
            ObjectPoolManager.DestroyPooled(collision.gameObject);
            DestroyByShoot();
        }
    }
    public void DestroyByShoot()
    {
        var BrVelo = transform.GetComponent<Rigidbody2D>().velocity;

        for(int i = 0; i<2; i++)
        {
            GameObject smallRock = Instantiate(rock, transform.position, Quaternion.identity);
            smallRock.GetComponent<Rigidbody2D>().velocity = ((-2f * i)+1f) * BrVelo;
        }

        Destroy(this.gameObject);
        
    }
}
