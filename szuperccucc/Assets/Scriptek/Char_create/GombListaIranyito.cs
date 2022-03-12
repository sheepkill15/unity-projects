using UnityEngine;
using UnityEngine.UI;

public class GombListaIranyito : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonTemplate;
    private KarakterS karakterBeallitas;
    private int a;
    public Tarolo tarolo;

    private void Start()
    {
        karakterBeallitas = GameObject.Find("Vezerlo").GetComponent<vezerlo>().adatok;

        Activate(0);
    }

    public void Activate(int a)
    {
        gameObject.SetActive(true);
        for (int i = 0; i < tarolo.hajak.Length; i++)
        {
            GameObject button = Instantiate(buttonTemplate) as GameObject;
            button.SetActive(true);
            button.GetComponent<GombListaGomb>().SetImage(tarolo.hajak[i]);
            button.transform.SetParent(buttonTemplate.transform.parent, false);
            button.name = i.ToString();
            button.GetComponent<Button>().onClick.AddListener(delegate { OnClick(button); });
        }
    }

    public void OnClick(GameObject button)
    {
        ushort.TryParse(button.name, out karakterBeallitas.fej.haj);
        karakterBeallitas.Valtas();
    }
     
}
