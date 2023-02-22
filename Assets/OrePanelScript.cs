using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class OrePanelScript : MonoBehaviour
{
    /////stting variables
    public GameObject OreFolder;
    private bool functionRuning =false;
    public GameObject m_DestroyText;
    private float AmountOfOresToDeleteInfloat;

    IEnumerator DragAndDropingPanel()
    {
        /////making it in same position until you click
        while (!Input.GetMouseButton(0) & !Input.GetKeyDown(KeyCode.Tab))
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(Input.mousePosition.x , Input.mousePosition.y );
            yield return null;
        }
        
        gameObject.GetComponent<RectTransform>().anchoredPosition += new Vector2(500, 500);////making it in diffrent position as it was so sistem can say if is something behind it
        yield return null;////waitng small time
        if (!EventSystem.current.IsPointerOverGameObject() & !Input.GetKeyDown(KeyCode.Tab))////panel is on nothing
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition -= new Vector2(500, 500);////making it in the same position as it was
            /////making and setting Destroy Text
            GameObject m_CopyDestroyText = Instantiate(m_DestroyText);
            m_CopyDestroyText.transform.SetParent(transform);
            Vector2 ThisPanelPosition = gameObject.GetComponent<RectTransform>().anchoredPosition;
            m_CopyDestroyText.GetComponent<RectTransform>().anchoredPosition = new Vector2(130, 0 );

            ////seting Amount Of Ores To Delete to number of ores in panel 
            AmountOfOresToDeleteInfloat = (gameObject.GetComponent<ThingData>().AmountOfOres / 2) + 1;
            gameObject.GetComponent<ThingData>().AmountOfOresToDelete = (int)AmountOfOresToDeleteInfloat;

            transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = gameObject.GetComponent<ThingData>().AmountOfOresToDelete.ToString();

            for (int i = 0; i < 80; i++)
            {
                yield return null;       ////waiting long time
            }

            /////finding if Amount Of Ores To Delete
            while (!Input.GetMouseButton(0) & !Input.GetKeyDown(KeyCode.Tab))
            {
               
                yield return null;
                //if (Input.GetAxis("Mouse ScrollWheel") != 0)
                //{
                //    print(Input.GetAxis("Mouse ScrollWheel"));
                //}
                if (AmountOfOresToDeleteInfloat>20)/////above 20 Amount Of Ores To Delete +Amount Of Ores To Delete
                {
                    AmountOfOresToDeleteInfloat += (Input.GetAxis("Mouse ScrollWheel") * gameObject.GetComponent<ThingData>().AmountOfOresToDelete) / 4;
                    gameObject.GetComponent<ThingData>().AmountOfOresToDelete = (int)Mathf.Clamp(AmountOfOresToDeleteInfloat, 1, gameObject.GetComponent<ThingData>().AmountOfOres);////making it not past number of ores in panel 
                }
                else/////under 20 Amount Of Ores To Delete +1
                {
                    AmountOfOresToDeleteInfloat += Input.GetAxis("Mouse ScrollWheel") * 5;
                    gameObject.GetComponent<ThingData>().AmountOfOresToDelete = (int)Mathf.Clamp(AmountOfOresToDeleteInfloat, 1, gameObject.GetComponent<ThingData>().AmountOfOres);////making it not past number of ores in panel 
                }
                ////seting visuals of Amount Of Ores To Delete
                string StringOfAmountOfOresToDelete = gameObject.GetComponent<ThingData>().AmountOfOresToDelete.ToString();
                transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = StringOfAmountOfOresToDelete;
            }

            /////creating ore 
            GameObject ore = Instantiate(    OreFolder.transform.GetChild(     gameObject.GetComponent<ThingData>().OreNumber       ).gameObject      );
            GameObject player = gameObject.transform.parent.GetComponent<ThingData>().ObjectPlayer;
            ore.transform.position = player.transform.position + (player.transform.GetChild(0).forward * 2);
            ore.transform.parent = player.transform.parent;

            /////seting ore scale
            ore.transform.localScale = ore.transform.localScale * Mathf.Clamp(1 + (gameObject.GetComponent<ThingData>().AmountOfOresToDelete - ore.GetComponent<ThingData>().OreAmountInrock) *0.05f,0,100);

            /////seting ore number in script (in ore)                     
            ore.GetComponent<ThingData>().OreAmountInrock = gameObject.GetComponent<ThingData>().AmountOfOresToDelete;


            ////seting ore number in script(in panel)
            gameObject.GetComponent<ThingData>().AmountOfOres -= gameObject.GetComponent<ThingData>().AmountOfOresToDelete;

            ////destroing panel if panel is empty
            if (gameObject.GetComponent<ThingData>().AmountOfOres == 0)
            {
                Destroy(gameObject);
            }

            ////seting ore number visuali in panel)
            transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = gameObject.GetComponent<ThingData>().AmountOfOres.ToString();

            ////destroing -- Amount Of Ores To Delete 
            Destroy(transform.GetChild(3).gameObject);
        }
        
        
            for (int i = 0; i < 20; i++)////waiting
            {
                yield return null;
            }

        ////geting panel to its start
        
        transform.SetParent(transform.parent.GetChild(0));
        functionRuning = false;
        
        gameObject.GetComponent<RectTransform>().anchoredPosition -= new Vector2(500, 500);

        
    }
    public void StrartCortune()////starts at button click
    {
        if (functionRuning == false)/////checks if function is not running
        {
            
            transform.SetParent(transform.parent.parent);//seting parent so it can move
            gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);////making it in the middle of mouse
            gameObject.GetComponent<RectTransform>().anchorMax= new Vector2(0, 0);
            functionRuning = true;
            StartCoroutine(DragAndDropingPanel());////starting function
        }      
    }
}
