using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 5;
    public float jumpHeight = 2;

    private Rigidbody2D _rb;
    private Transform _transform;
    private Animator _animator;

    private bool _grounded;

    public GameObject bullet;

    private bool _pressedShoot;

    public GameObject kuka;

    private bool _left;
    private static readonly int Running = Animator.StringToHash("Running");
    private static readonly int Shoot = Animator.StringToHash("Shoot");

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(pew());
        _rb = GetComponent<Rigidbody2D>();
        _transform = transform;
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        float moveHoriz = Input.GetAxisRaw("Horizontal");
        _transform.position += new Vector3(moveHoriz * speed * Time.deltaTime, 0, 0);
        
        _animator.SetBool(Running, moveHoriz != 0);
        
        if (moveHoriz > 0)
        {
            _left = false;
            Vector3 scale = _transform.localScale;
            scale.x = -1;
            _transform.localScale = scale;

        }
        else if (moveHoriz < 0)
        {
            _left = true;
            Vector3 scale = _transform.localScale;
            scale.x = 1;
            _transform.localScale = scale;
        }
        
        if (_grounded && Input.GetButtonDown("Jump"))
        {
            Vector3 vel = _rb.velocity;
            vel.y = CalculateJumpVelocity(jumpHeight);
            _rb.velocity = vel;
            _grounded = false;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            _pressedShoot = true;
        }

        if (_transform.position.y < -50)
        {
          kuka.GetComponent<Player>().takedmg();
          _transform.position = new Vector3(0, 5, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Map"))
        {
            _grounded = true;
        }
    }

    private static float CalculateJumpVelocity(float height)
    {
        return Mathf.Sqrt(-2 * Physics2D.gravity.y * height);
    }

    IEnumerator pew()
    {
        while (true)
        {
            if(_pressedShoot)
              {
                Instantiate(bullet , 
                    _transform.position, 
                    Quaternion.Euler(_transform.eulerAngles.x, _left ? 180 : 0, 0));
                _pressedShoot = false;
                _animator.SetTrigger(Shoot);
              }
         yield return new WaitForSeconds(0.5f);
        }

    }



}
