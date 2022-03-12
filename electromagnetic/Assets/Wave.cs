using UnityEngine;

public class Wave : MonoBehaviour
{
    private LineRenderer[] _lineRenderers;
    private Transform _transform;
    
    [Header("Simulation settings")]
    public float stepSize = 0.1f;

    public float inverseTimeScale = 5f;

    public float inverseSpaceScale = 5f;

    public enum Type
    {
        HullamEgyenlete,
        Maxwell
    }

    public Type type;
    
    [Header("Wave Parameters")]
    [Tooltip("In nanometers")]
    public float wavelength = 400; // nm
    [Tooltip("In nanometers")]
    public float amplitude = 5; // nm
    [Tooltip("In hertz / 10^14")]
    public float frequency = 50; // hz

    [SerializeField, ShowOnly] private float szogsebesseg;

    [SerializeField, ShowOnly] private float periodus;

    [SerializeField, ShowOnly] private float wavenumber;

    [SerializeField, ShowOnly] private float velocity = 1;

    private const float LightSpeed = 299_792_458f;

    public Vector3 initialElectricField = -Vector3.right;

    private Vector3 _currentElectricField;
    
    private void Start()
    {
        _lineRenderers = GetComponentsInChildren<LineRenderer>();
        _transform = transform;
    }
    
    private float HullamEgyenlete(float x, float t)
    {
        return amplitude * Mathf.Sin(2 * Mathf.PI * (t / periodus - x / wavelength));
    }

    private Vector3 Maxwell(Vector3 pos, float t, int wave)
    {
        if (wave != 0) return Vector3.Cross(Vector3.right, _currentElectricField);
        _currentElectricField = initialElectricField * Mathf.Cos(wavenumber * pos.z - szogsebesseg * t);
        return _currentElectricField;
    }
    
    // Update is called once per frame
    private void Update()
    {
        RecalculateVelocity();
        Move();
        Generate();
    }
    private void Generate()
    {
        int size = Mathf.FloorToInt(wavelength / inverseSpaceScale / stepSize);
        foreach (LineRenderer lineRenderer in _lineRenderers)
        {
            lineRenderer.positionCount = size;
        }

        {
            int i = 0;
            var position = _transform.position;
            float ownZ = position.z;
            float ownY = position.y;
            float ownX = position.x;
            for (float k = 0; i < size; k += stepSize)
            {
                Vector3 point;
                switch (type)
                {
                    case Type.HullamEgyenlete:
                        Vector3 ifMoved = _transform.right * k;
                        point = new Vector3(ownX, ownY + HullamEgyenlete(k, Time.time / inverseTimeScale), ownZ) + ifMoved;
                        break;
                    case Type.Maxwell:
                        point = new Vector3(ownX, ownY, ownZ) + _transform.TransformPoint(Maxwell(new Vector3(0, 0, k), Time.time / inverseTimeScale, 0) + new Vector3(k, 0, 0));
                        break;
                    default:
                        point = Vector3.zero;
                        break;
                }
               _lineRenderers[0].SetPosition(i, point);
               switch (type)
               {
                   case Type.HullamEgyenlete:
                       Vector3 ifMoved = _transform.right * k;
                       point = new Vector3(ownX, ownY, ownZ + HullamEgyenlete(k, Time.time / inverseTimeScale)) + ifMoved;
                       break;
                   case Type.Maxwell:
                       point = new Vector3(ownX, ownY, ownZ) + _transform.TransformPoint(Maxwell(new Vector3(0, 0, k), Time.time / inverseTimeScale, 1) + new Vector3(k, 0, 0));
                       break;
                   default:
                       point = Vector3.zero;
                       break;
               }
               _lineRenderers[1].SetPosition(i++, point);
            }
        }
    }

    private void Move()
    {
        _transform.position += -_transform.right * (velocity * Time.deltaTime);
    }

    private void RecalculateVelocity()
    {
        periodus = 1 / frequency;
        wavenumber = 1 / wavelength;
        velocity = wavelength * frequency / LightSpeed * 100000;
        szogsebesseg = 2 * Mathf.PI * frequency;
    }

    private void OnCollisionEnter(Collision other)
    {
        var forward = _transform.right;
        float angle = Vector3.Angle(other.contacts[0].normal, forward);
        _transform.Rotate(forward, 2 * angle * Mathf.Deg2Rad);
    }
}
