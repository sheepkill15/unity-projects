using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class Movement : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Transform _transform;
    private Transform _camera;

    public float speed = 1;
    public float jumpHeight = 2;
    
    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = transform;
        Debug.Assert(Camera.main != null, "Camera.main != null");
        _camera = Camera.main.transform;
    }

    // Update is called once per frame
    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        _transform.position += ( moveX * speed * Time.deltaTime * _transform.right + 
                              moveY  * speed * Time.deltaTime * _transform.forward);

        if (Input.GetButtonDown("Jump"))
        {
            Vector3 vel = _rigidbody.velocity;
            vel.y = CalculateJumpForce(jumpHeight);
            _rigidbody.velocity = vel;
        }

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        _transform.localEulerAngles += new Vector3(0, mouseX, 0);
        _camera.localEulerAngles += new Vector3(-mouseY, 0, 0);
    }

    private static float CalculateJumpForce(float height)
    {
        return Mathf.Sqrt(2 * height * -Physics.gravity.y);
    }
}
