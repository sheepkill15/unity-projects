using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Update is called once per frame
    public float scrollSpeed = 10f;
    public int treshold = 10;
    public Vector2 startpos;
    public Vector2 newpos;
    public bool canmove = true;

    void Update()
    {
        if ( canmove == true )
        {
            Vector2 mousePos = Input.mousePosition;

            if (mousePos.x > Screen.width - treshold)
            {
                if(transform.position.x < 50)
                 transform.position += new Vector3(scrollSpeed, 0, 0);
            }
            else if (mousePos.x < treshold)
            {
                if(transform.position.x > -50)
                    transform.position += new Vector3(-scrollSpeed, 0, 0);
            }
            if (mousePos.y > Screen.height - treshold)
            {
                if(transform.position.y < 50)
                    transform.position += new Vector3(0, scrollSpeed, 0);
            }
            else if (mousePos.y < treshold)
            {
                if(transform.position.y > -50)
                    transform.position += new Vector3(0, -scrollSpeed, 0);
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - Input.GetAxis("Mouse ScrollWheel") * 15, 3, 60);
            }

            if (Input.GetMouseButtonDown(2))
            {
                startpos = mousePos;
            }

            if (Input.GetMouseButton(2))
            {
                newpos = mousePos;

                float angle = Mathf.Atan2(newpos.y - startpos.y, newpos.x - startpos.x);
                Vector2 move = new Vector2(Mathf.Cos(angle) * scrollSpeed, Mathf.Sin(angle) * scrollSpeed);
                Vector3 newMove = transform.position + (Vector3)move;
                if (newMove.x > 50) newMove.x = 49;
                if (newMove.x < -50) newMove.x = -49;
                if (newMove.y > 50) newMove.y = 49;
                if (newMove.y < -50) newMove.y = -49;
                transform.position = newMove;

                /* if (startpos.x - newpos.x > 1)
                     transform.position += new Vector3(-scrollSpeed, 0, 0);
                 if (startpos.x - newpos.x < -1)
                     transform.position += new Vector3(scrollSpeed, 0, 0);
                 if (startpos.y - newpos.y > 1)
                     transform.position += new Vector3(0, -scrollSpeed, 0);
                 if (startpos.y - newpos.y < -1)
                     transform.position += new Vector3(0, scrollSpeed, 0);*/
            }
        }
        


    }
}
