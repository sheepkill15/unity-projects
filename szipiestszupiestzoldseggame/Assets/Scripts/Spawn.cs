using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawn : MonoBehaviour
{
    public static int number=0;
    public static int number2 = 0;
    public static int wave = 0;

    public static int maxwave = 2;
    public static int minnum = 7;
    public static int maxnum = 12;
    public static int mint = 10;
    public static int maxt = 20;
    public int hulla2;


    public static float dinyechance =5;


    public int time;
    public float delay;
    public GameObject apple;
    public GameObject mellon;
    private Transform _transform;

    public Text text;
    public Text text2;
    public Text text3;

    public GameObject buton;

    public bool left;
    public static int hulla = 0;





    private void Start()
    {
        _transform = transform;
    }

    public void nextround()
    {
        Player.cango = 10;
        maxwave++;
        wave = maxwave;
        maxnum += 3;
        minnum += 3;
        maxt += 5;
        mint += 5;
        if (dinyechance < 50)
             dinyechance += 5;

        hulla = 0;
        fakestart();
    }


    void fakestart()
    {
        hulla = 0;
        buton.SetActive(false);
        text3.text = wave.ToString();
        number = Random.Range(maxnum,minnum);
        number2 = number;
        time = Random.Range(mint ,maxt);
        StartCoroutine(Wait());
    }


    IEnumerator Wait()
    {
        while(time > 0 )
        {
        yield return new WaitForSeconds(1);
            time--;
            text.text = time.ToString();
        }

        StartCoroutine(spawn());
    }

    IEnumerator spawn()
    {
        while ( number > 0 )
        {

            int i;
            i = Random.Range(1, 100);
            if ( i >= dinyechance)
            Instantiate(apple, new Vector3(_transform.position.x, -3.04f, 0), Quaternion.Euler(_transform.eulerAngles.x, left ? 180 : 0, 0), GridManager.Instance.map);
            else
            Instantiate(mellon, new Vector3(_transform.position.x, -0.49f, 0), Quaternion.Euler(_transform.eulerAngles.x, left ? 180 : 0, 0), GridManager.Instance.map);


            float delay;
            delay = Random.Range(5, 15);
            delay = delay / 10;
            number--;
                 yield return new WaitForSeconds(delay);
        }

    }

    private void Update()
    {
        hulla2 = hulla;
        if (hulla == number2)
        {
            wave--;
            text3.text = wave.ToString();
            if (wave > 0)
            {
                hulla = 0;
                fakestart();
                
            }
            else
            {
                number2 = 0;
                number = -1;
                hulla = 1;
                buton.SetActive(true);
                Player.cango = 0;
            }
        }

        text2.text = (number2 - number - hulla).ToString();
        


    }



}
