using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResearchScript : MonoBehaviour
{
    public Transform BuildMenu;
    public Building BuildingScript;
    public ThingData ThingDataScript;
    public Player PlayerScript;
    public TextMeshProUGUI PlanetDataText;
    public TextMeshProUGUI RockDataText;

    public bool BuildRecepi;
    public GameObject AddedBuildThing;

    public int[] ResorseAmount;
    public string[] ResordeType;

    public float DataRockNeeded;
    public float DataPlanetNeeded;

    public bool researched = false;

    public GameObject[] ShownThings;

    public bool ChangeMaxSpeed = false;
    public float ChangeMaxSpeedTo;

    public bool ChangeDamage = false;
    public float ChangeDamageTo;

    public bool BuildModeResearch;


    // Start is called before the first frame update
    public void Clicked()
    {

        if (researched == false)
        {
            if (transform.GetChild(1).gameObject.active == true)
            {
                if (BuildingScript.TakeRecorses(ResorseAmount, ResordeType) == true & DataPlanetNeeded <= ThingDataScript.PlanetData & DataRockNeeded <= ThingDataScript.RockData)
                {
                    ThingDataScript.PlanetData -= DataPlanetNeeded;
                    ThingDataScript.RockData -= DataRockNeeded;

                    PlanetDataText.text = ThingDataScript.PlanetData.ToString();
                    RockDataText.text = ThingDataScript.RockData.ToString();

                    for (int i = 0; i < ShownThings.Length; i++)
                    {
                        ShownThings[i].SetActive(true);
                    }

                    if (ChangeMaxSpeed == true)
                    {
                        PlayerScript.MaxSpeed = ChangeMaxSpeedTo;
                    }

                    if (ChangeDamage == true)
                    {
                        PlayerScript.Damage = ChangeDamageTo;
                    }
                    if (BuildModeResearch == true)
                    {
                        PlayerScript.BuildModeResearched = true;
                    }
                    if (BuildRecepi == true)
                    {
                        Instantiate(AddedBuildThing).transform.parent = BuildMenu;
                    }
                    GetComponent<Image>().color -= new Color(0, 0, 0, 0.5f);
                    researched = true;
                }
                else
                {
                    transform.GetChild(1).gameObject.SetActive(false);
                    BuildingScript.PublicPrintNotEnoughtMaterials();
                }
            }
            else
            {
                StartCoroutine(ExplainText());
            }

        }
        else if (transform.GetChild(1).gameObject.active == true)
        {
            transform.GetChild(1).gameObject.SetActive(false);
        }
        
    }
    IEnumerator ExplainText()
    {
        transform.GetChild(1).gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        transform.GetChild(1).gameObject.SetActive(false);
    }
}
