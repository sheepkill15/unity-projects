using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCar : MonoBehaviour
{
    public List<KocsiMozogj> cars = new List<KocsiMozogj>();

    public GameObject carPrefab;

    public GameObject carsPanel;
    public GameObject content;
    public GameObject carButtonPrefab;

    private void Start()
    {
        AddCar(carPrefab);   
    }



    public void AddCar(GameObject carprefab)
    {
        cars.Add(Instantiate(carprefab, transform.position, Quaternion.identity, transform).GetComponent<KocsiMozogj>());
        DisplayCars();
    }
    public void DisplayCars()
    {
        foreach (Transform transform in content.transform)
        {
            Destroy(transform.gameObject);
        }

        foreach (KocsiMozogj car in cars)
        {
            Button button = Instantiate(carButtonPrefab, content.transform).GetComponentInChildren<Button>();
            button.GetComponent<Image>().sprite = car.gameObject.GetComponent<SpriteRenderer>().sprite;
            button.GetComponent<CheckClickUIGOmb>().car = car.gameObject;
            button.onClick.AddListener(delegate ()
            {
                Vector3 pos = car.gameObject.transform.position;
                pos.z = -10;
                Camera.main.transform.position = pos;
                ResetChosen();
                car.chosen = true;
            });
        }
    }

    public void ResetChosen()
    {
        foreach (KocsiMozogj car in cars)
        {
            car.chosen = false;
        }
    }

    public void newCar()
    {
        cars.Add(Instantiate(carPrefab, transform.position, Quaternion.identity, transform).GetComponent<KocsiMozogj>());
        DisplayCars();
    }


}
