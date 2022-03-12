using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour
{
    public GameObject ut_prefab;
    public GameObject haz_prefab;
    public GameObject keresztezodes_prefab;
    public GameObject bolt_prefab;

    public GameObject hatar_prefab;

    //  public GameObject kocsi;

    public int random;
    public int map_szeltebe = 15;
    public int map_hosszaba = 15;

    private void Start()
    {
    //    kocsi.transform.position = Vector3.zero;
        Epits(Vector2.zero, 0);
        Epits(Vector2.zero, 1);
        Epits(Vector2.zero, 2);
        Epits(Vector2.zero, 3);

        AstarData.active.Scan();
    }

    public void Epits(Vector2 offset, int melyikIrany)
    { 
        if (offset.x > 45 || offset.x < -45 || offset.y > 45 || offset.y < -45)
        {
            return;
        }

       /* if(offset.x > 50)
        {
            Instantiate(hatar_prefab, offset + new Vector2(4.3f, 0), Quaternion.identity, transform);
            return;
        }
        else if(offset.x < -50)
        {
            Instantiate(hatar_prefab, offset - new Vector2(4.3f, 0), Quaternion.identity, transform);
            return;
        }
        else if(offset.y > 50)
        {
            Instantiate(hatar_prefab, offset + new Vector2(0, 4.3f), Quaternion.identity, transform);
            return;
        }
        else if (offset.y < -50)
        {
            Instantiate(hatar_prefab, offset - new Vector2(0, 4.3f), Quaternion.identity, transform);
            return;
        }*/

        int szorzoX;
        int szorzoY;

        int angle;

        switch (melyikIrany)
        {
            case 0:
                szorzoX = 1;
                szorzoY = 1;
                angle = 0;
                break;
            case 1:
                szorzoX = 1;
                szorzoY = -1;
                angle = 180;
                break;
            case 2:
                szorzoX = -1;
                szorzoY = 1;
                angle = 0;
                break;
            case 3:
                szorzoX = -1;
                szorzoY = -1;
                angle = 180;
                break;
            default:
                szorzoX = 1;
                szorzoY = 1;
                angle = 0;
                break;
        }
        if (Physics2D.OverlapPointAll(offset + new Vector2(0, 4.2f * szorzoY)).Length > 0)
        {
            // found something
        }
        else
        {
            // spot is empty, we can spawn
            Instantiate(ut_prefab, offset + new Vector2(0, 4.2f * szorzoY), Quaternion.identity, transform);

        }
        if (Physics2D.OverlapPointAll(offset + new Vector2(0, (4.2f + 7f) * szorzoY)).Length > 0)
        {
            // found something
        }
        else
        {
            // spot is empty, we can spawn
            Instantiate(ut_prefab, offset + new Vector2(0, (4.2f + 7f) * szorzoY), Quaternion.identity, transform);
        }
        if (Physics2D.OverlapPointAll(offset + new Vector2(0, (4.2f + 7f + 4.2f) * szorzoY)).Length > 0)
        {
            // found something
        }
        else
        {
            // spot is empty, we can spawn
            Instantiate(keresztezodes_prefab, offset + new Vector2(0, (4.2f + 7f + 4.2f) * szorzoY), Quaternion.identity, transform);

        }
        int hanyszor = Random.Range(1, 5);
        for (int i = 0; i < hanyszor; i++)
        {
            if (Physics2D.OverlapBoxAll(offset + new Vector2((i * 7 + 4.2f) * szorzoX, 4.2f * szorzoY), new Vector2(6, 6), 0).Length > 0)
            {
                // found something
            }
            else
            {
                random = Random.Range(1,101);
                GameObject gm;
                if ( random > 5)
                  gm = Instantiate(haz_prefab, offset + new Vector2((i * 7 + 4.2f) * szorzoX, 4.2f * szorzoY), Quaternion.identity, transform);//boltttttttttttttttttttttttttt
                else
                  gm = Instantiate(bolt_prefab, offset + new Vector2((i * 7 + 4.2f) * szorzoX, 4.2f * szorzoY), Quaternion.identity, transform);

                gm.transform.Rotate(new Vector3(0, 0, 180-angle));
            }
            if (Physics2D.OverlapBoxAll(offset + new Vector2((i * 7 + 4.2f) * szorzoX, (4.2f + 7f) * szorzoY), new Vector2(6, 6), 0).Length > 0)
            {
                // found something
            }
            else
            {
                random = Random.Range(1, 101);
                GameObject gm;
                if (random > 5)
                   gm = Instantiate(haz_prefab, offset + new Vector2((i * 7 + 4.2f) * szorzoX, (4.2f + 7f) * szorzoY), Quaternion.identity, transform);//boltttttttttt
                else
                   gm = Instantiate(bolt_prefab, offset + new Vector2((i * 7 + 4.2f) * szorzoX, (4.2f + 7f) * szorzoY), Quaternion.identity, transform);
                gm.transform.Rotate(new Vector3(0, 0, angle));

            }
            if (Physics2D.OverlapPointAll(offset + new Vector2((i * 7 + 4.2f) * szorzoX, (4.2f + 7f + 4.2f) * szorzoY)).Length > 0)
            {
                // found something
            }
            else
            {
                // spot is empty, we can spawn
                Instantiate(ut_prefab, offset + new Vector2((i * 7 + 4.2f) * szorzoX, (4.2f + 7f + 4.2f) * szorzoY), Quaternion.Euler(new Vector3(0, 0, 90)), transform);
            }
            if (Physics2D.OverlapPointAll(offset + new Vector2((i * 7 + 4.2f) * szorzoX, 0)).Length > 0)
            {
                // found something
            }
            else
            {
                // spot is empty, we can spawn
                Instantiate(ut_prefab, offset + new Vector2((i * 7 + 4.2f) * szorzoX, 0), Quaternion.Euler(new Vector3(0, 0, 90)), transform);
            }
        }
        if (Physics2D.OverlapPointAll(offset + new Vector2(((hanyszor - 1) * 7 + 2 * 4.2f) * szorzoX, (4.2f + 7f + 4.2f) * szorzoY)).Length > 0)
        {
            // found something
        }
        else
        {
            // spot is empty, we can spawn
            Instantiate(keresztezodes_prefab, offset + new Vector2(((hanyszor - 1) * 7 + 2 * 4.2f) * szorzoX, (4.2f + 7f + 4.2f) * szorzoY), Quaternion.identity, transform);
        }
        if (Physics2D.OverlapPointAll(offset + new Vector2(((hanyszor - 1) * 7 + 2 * 4.2f) * szorzoX, 4.2f * szorzoY)).Length > 0)
        {
            // found something
        }
        else
        {
            // spot is empty, we can spawn
            Instantiate(ut_prefab, offset + new Vector2(((hanyszor - 1) * 7 + 2 * 4.2f) * szorzoX, 4.2f * szorzoY), Quaternion.identity, transform);
        }
        if (Physics2D.OverlapPointAll(offset + new Vector2(((hanyszor - 1) * 7 + 2 * 4.2f) * szorzoX, (4.2f + 7f) * szorzoY)).Length > 0)
        {
            // found something
        }
        else
        {
            // spot is empty, we can spawn
            Instantiate(ut_prefab, offset + new Vector2(((hanyszor - 1) * 7 + 2 * 4.2f) * szorzoX, (4.2f + 7f) * szorzoY), Quaternion.identity, transform);
        }
        if (Physics2D.OverlapPointAll(offset + new Vector2(((hanyszor - 1) * 7 + 2 * 4.2f) * szorzoX, 0)).Length > 0)
        {
            // found something
        }
        else
        {
            // spot is empty, we can spawn
            Instantiate(keresztezodes_prefab, offset + new Vector2(((hanyszor - 1) * 7 + 2 * 4.2f) * szorzoX, 0), Quaternion.identity, transform);
        }
       // Epits(offsetNew, 0);
      //  Epits(offsetNew, 2);
      //  Epits(offsetNew, 3);
    //    AstarPath.active.Scan();
    }

}
