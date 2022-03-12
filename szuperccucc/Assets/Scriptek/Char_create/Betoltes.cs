using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Betoltes : MonoBehaviour
{
    private KarakterS karakterBeallitas;
    private Tarolo tarolo;

    public SpriteRenderer[] testReszek;

    // Start is called before the first frame update
    void Start()
    {
        karakterBeallitas = GameObject.Find("Vezerlo").GetComponent<vezerlo>().adatok;
        tarolo = GameObject.Find("Tarolo").GetComponent<Tarolo>();

        testReszek = GetComponentsInChildren<SpriteRenderer>(); // Az elso maga a karakter, 1estol kell indexelni

        frissit();
    }


    public void frissit()
    {
        testReszek[1].sprite = tarolo.hajak[karakterBeallitas.fej.haj];  // Haj
        testReszek[2].sprite = tarolo.szemek[karakterBeallitas.fej.szem];   // Szem
        testReszek[3].sprite = tarolo.pupillak[karakterBeallitas.fej.pupilla];   // Bal Pupilla
        testReszek[4].sprite = tarolo.pupillak[karakterBeallitas.fej.pupilla];   // Jobb Pupilla
        testReszek[5].sprite = tarolo.szajak[karakterBeallitas.fej.szaj];   // Szaj
        testReszek[6].sprite = tarolo.testek[karakterBeallitas.test];   // Test
        testReszek[7].sprite = tarolo.karok[karakterBeallitas.kar];   // Bal kar
        testReszek[8].sprite = tarolo.karok[karakterBeallitas.kar]; // Jobb kar
        testReszek[9].sprite = tarolo.labak[karakterBeallitas.lab]; // Bal lab
        testReszek[10].sprite = tarolo.labak[karakterBeallitas.lab]; // Jobb lab
    }

}
