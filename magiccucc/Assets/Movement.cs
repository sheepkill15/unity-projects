using UnityEngine;

public class Movement : MonoBehaviour
{
    private CharacterController _controller;

    private Transform _transform;
    private Transform _camera;
    
    private Vector3 _velocity = Vector3.zero;

    public float speed = 2;
    public float jumpHeight = 2;
    public float mouseSensitivity = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _controller = GetComponent<CharacterController>();
        _camera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        #region Keyboard
        {
            _velocity.y += Physics.gravity.y * Time.deltaTime;

            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            _controller.Move(_transform.TransformDirection(new Vector3(moveX * speed, 0, moveY * speed)) * Time.deltaTime +
                             _velocity * Time.deltaTime);
            if (_controller.isGrounded)
            {
                _velocity.y = 0;
                if (Input.GetButtonDown("Jump"))
                {
                    _velocity.y = CalculateHeight(jumpHeight);
                }
            }
        }
        #endregion

        #region Mouse
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // _camera.RotateAround(_transform.position, _camera.right, mouseY * mouseSensitivity);
            _transform.Rotate(new Vector3(0, mouseX * mouseSensitivity, 0), Space.Self);
        }
        #endregion
    }

    private static float CalculateHeight(float height)
    {
        return Mathf.Sqrt(2 * -Physics.gravity.y * height);
    }
}
