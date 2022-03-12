using UnityEngine;

public class Movement : MonoBehaviour
{
    private Transform _transform;
    private Animator _animator;

    private readonly int _right = Animator.StringToHash("Right");
    private readonly int _left = Animator.StringToHash("Left");
    private readonly int _front = Animator.StringToHash("Front");
    private readonly int _back = Animator.StringToHash("Back");

    public float speed = 1;
    
    // Start is called before the first frame update
    private void Start()
    {
        _transform = transform;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        float moveY = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;

        _transform.position += new Vector3(moveX, moveY, 0);

        if (moveX > 0)
        {
            _animator.Play(_right);
        }
        else if (moveX < 0)
        {
            _animator.Play(_left);
        }
        else if (moveY > 0)
        {
            _animator.Play(_back);
        }
        else if (moveY < 0)
        {
            _animator.Play(_front);
        }
}
}
