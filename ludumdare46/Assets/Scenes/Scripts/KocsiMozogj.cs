using Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class KocsiMozogj : MonoBehaviour
{
    private Seeker seeker;
    private AIPath AI;
    public MoveCar movecar;
    public Color color;

    public GameObject waypoint;

    public List<Vector2> path = new List<Vector2>();
    List<GameObject> waypoints = new List<GameObject>();

    public bool chosen = false;
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        AI = GetComponent<AIPath>();
        color = new Color(Random.value, Random.value, Random.value, 1);
        GetComponent<SpriteRenderer>().color = color;
        movecar = GameObject.Find("GameManager").GetComponent<MoveCar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (chosen)
        {
            if (Input.GetMouseButtonDown(1))
            {
                bool ok = false;
                path.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if(Input.GetKey(KeyCode.LeftShift))
                {
                    waypoints.Add(Instantiate(waypoint, path[path.Count - 1], Quaternion.identity));
                    waypoints[waypoints.Count - 1].GetComponent<SpriteShapeRenderer>().color = color;
                }
                else
                {
                    path.Clear();
                    foreach(GameObject game in waypoints)
                    {
                        Destroy(game);
                    }
                    waypoints.Clear();
                    path.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    ok = true;
                }
                if (ok)
                {
                    seeker.StartPath(transform.position, path[0]);
                    path.RemoveAt(0);
                }
            }
            /*     if(Input.GetMouseButtonDown(0))
                 {
                     chosen = false;
                 } */
        }
        if (AI.hasPath)
        {
            if (AI.reachedEndOfPath)
            {
                if (path.Count > 0)
                {
                    seeker.StartPath(transform.position, path[0]);
                    path.RemoveAt(0);
                    if (waypoints.Count > 0)
                    {
                        Destroy(waypoints[0]);
                        waypoints.RemoveAt(0);
                    }
                }
            }
        }
    }
    private void OnMouseDown()
    {
        movecar.ResetChosen();
        chosen = true;
    }
}
