using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class KarakterS : ScriptableObject
{
    [System.Serializable]
    public struct Fej
    {
        public ushort haj;
        public ushort szem;
        public ushort pupilla;
        public ushort szaj;
    }
    public Fej fej;
    public ushort test;
    public ushort kar;
    public ushort lab;

    public void Valtas()
    {
        GameObject.Find("Karakter").GetComponent<Betoltes>().frissit();
    }

}

