using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public LineRenderer lr;
    AIPath ai;
    KocsiMozogj kocsi;

    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponent<AIPath>();
        kocsi = GetComponent<KocsiMozogj>();
        lr.startColor = kocsi.color;
        lr.endColor = kocsi.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (ai.hasPath)
        {
            List<Vector3> pathh = new List<Vector3>();
            bool stale;
            ai.GetRemainingPath(pathh, out stale);
            lr.positionCount = pathh.Count + 1;
            Vector3 pos = transform.position;
            pos.z = -1;
            lr.SetPosition(0, pos);
            for(int i = 1; i <= pathh.Count; i++)
            {
                pos = pathh[i - 1];
                pos.z = -1;
                lr.SetPosition(i, pos);
            }
        }
    }
}
