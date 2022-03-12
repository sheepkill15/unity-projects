using UnityEngine;

public class AttackIndicatorUpdater : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    public Transform lineBegin;
    public Transform lineEnd;
    
    // Start is called before the first frame update
    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        var position1 = lineBegin.position;
        _lineRenderer.SetPosition(0, position1);
        var position = lineEnd.position;
        _lineRenderer.SetPosition(1, position);
        lineEnd.eulerAngles = new Vector3(0, 0, Mathf.Atan2((position - position1).y, (position - position1).x) * Mathf.Rad2Deg + -90);
    }
}
