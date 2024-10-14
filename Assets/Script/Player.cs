using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    private Rigidbody2D playerRB;
    [SerializeField] int maxHP;
    [SerializeField] int curHP;
    [SerializeField] List<GameObject> playerHeart;

    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float moveSpeed;

    [SerializeField] Transform shootPoint;
    private bool isShooting;
    [SerializeField] float shootSpeed;
    float nextShootTime;
    [SerializeField] float bulletSpeed;
    [SerializeField] GameObject bulletPF;
    Bullet bullet;

    public UnityEvent playerDied;

    public Vector2 MoveInput { get; private set; }
    public bool ShootHold { get; private set; }

    [SerializeField] private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _shootAction;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = this.transform.GetComponent<Rigidbody2D>();
        _playerInput = this.transform.GetComponent<PlayerInput>();
        SetupInputs();
        isShooting = false;
        maxHP = 3;
        curHP = maxHP;
        foreach(GameObject heart in playerHeart)
        {
            heart.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInputs();

        if (ShootHold)
        {
            //Debug.Log("ShottingHold");
            ShootBullet();
        }

    }

    private void FixedUpdate()
    {
        //Debug.Log("x " + MoveInput.x + " y " + MoveInput.y);
        transform.position = new Vector3(transform.position.x + MoveInput.x*moveSpeed, transform.position.y + MoveInput.y*moveSpeed, transform.position.z);
        //playerRB.AddForce(new Vector2(MoveInput.x* acceleration, MoveInput.y* acceleration), ForceMode2D.Force);
        //playerRB.velocity = Vector2.ClampMagnitude(playerRB.velocity , maxSpeed);
    }

    public void TakeDamage()
    {
        curHP -= 1;
        UpdateHeart();
        if(curHP <= 0)
        {
            playerDied?.Invoke();
        }
    }

    private void UpdateHeart()
    {
        playerHeart[maxHP - curHP - 1].SetActive(false);
    }

    private void ShootBullet()
    {
        if (Time.time < nextShootTime)
            return;
        nextShootTime = Time.time + 1f / shootSpeed;
        Debug.Log("Shoot!");
        Bullet bullet = ObjectPoolManager.CreatePooled(bulletPF.gameObject, shootPoint.position, shootPoint.rotation).GetComponent<Bullet>();
        //GameObject bullet = Instantiate(bulletPF, position: shootPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, bulletSpeed);
        //bullet.hit.AddListener(DestroyBullet);
    }


    private void SetupInputs()
    {
        _moveAction = _playerInput.actions["Move"];
        _shootAction = _playerInput.actions["Fire"];

    }

    private void UpdateInputs()
    {
        MoveInput = _moveAction.ReadValue<Vector2>();
        ShootHold = _shootAction.IsInProgress();

    }

}
