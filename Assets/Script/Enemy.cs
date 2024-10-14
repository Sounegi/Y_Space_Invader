using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] bool start;
    [SerializeField] bool moved;
    [SerializeField] bool dir;
    [SerializeField] bool forward;

    [SerializeField] NavMeshAgent agent;
    [SerializeField] private Vector3 target;

    [SerializeField] float bulletSpeed;
    [SerializeField] Transform shootPoint;
    [SerializeField] GameObject bulletPF;
    [SerializeField] int shootAmount;

    public MyIntEvent die = new MyIntEvent();
    public int Id;

    public class MyIntEvent : UnityEvent<int>
    {
    }

    public virtual void TakeDamage()
    {
        die?.Invoke(Id);
        //Destroy(gameObject);
        
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        start = false;
        moved = false;
        dir = false;
        forward = false;
        StartCoroutine(WaitStart());
    }

    IEnumerator WaitStart()
    {
        yield return new WaitForSeconds(0.5f);
        start = true;
    }

    private void FixedUpdate()
    {
        if (start)
        {
            EnterScreen();
        }
        else
        {
            MoveShoot();
        }
    }

    //x1-2, y-0.5
    protected virtual void EnterScreen()
    {
        if (!forward)
        {
            Debug.Log("Set Starting point");
            target = new Vector3(transform.position.x, transform.position.y - 4.0f, 0.0f);
            agent.SetDestination(target);
            //transform.position = new Vector3(transform.position.x, transform.position.y - 4.0f, 0.0f);
            forward = true;
        }
        
        if(agent.remainingDistance <= 0.1f)
        {
            start = false;
        }
        else
        {
            return;
        }
        /*
        if(viewPos.y >= 0.7) //0.7-0.9
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.05f, transform.position.z);
        }
        else
        {
            start = false;
            //MoveShoot();
        }
        */
    }

    protected virtual void MoveShoot()
    {
        var cam = Camera.main;
        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);
        if(viewPos.y <= 0.5)
        {
            forward = false;

        }else if(viewPos.y >= 0.7)
        {
            forward = true;
        }
        if (agent.remainingDistance <= 0.1f && !moved)
        {
            StartCoroutine(Shoot(shootAmount));
            target = new Vector3(transform.position.x + (dir ? 1.5f : -1.5f), transform.position.y - (forward ? 0.7f : -0.7f), 0.0f);
            agent.SetDestination(target);
            //transform.position = new Vector3(transform.position.x + (dir ? 0.5f : -0.5f), transform.position.y - (forward? 0.5f:-0.5f), transform.position.z);
            dir = !dir;
            moved = true;
            StartCoroutine(Wait());
        }
    }

    IEnumerator Shoot(int amount)
    {
        for(int i = 0; i< amount; i++)
        {
            Bullet bullet = ObjectPoolManager.CreatePooled(bulletPF.gameObject, shootPoint.position, shootPoint.rotation).GetComponent<Bullet>();
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -bulletSpeed);
            yield return new WaitForSeconds(0.5f);

        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        moved = false;
    }
}
