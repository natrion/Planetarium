using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class OrePanelScript : MonoBehaviour
{
    public GameObject OreFolder;
    private bool functionRuning =false;
    public GameObject m_DestroyText;
    private float AmountOfOresToDeleteInfloat;
    IEnumerator DragAndDropingPanel()
    {
        while (!Input.GetMouseButton(0) & !Input.GetKeyDown(KeyCode.Tab))
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(Input.mousePosition.x , Input.mousePosition.y );
            yield return null;
        }
        
        gameObject.GetComponent<RectTransform>().anchoredPosition += new Vector2(500, 500);
        yield return null;
        if (!EventSystem.current.IsPointerOverGameObject() & !Input.GetKeyDown(KeyCode.Tab))
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition -= new Vector2(500, 500);
            GameObject m_CopyDestroyText = Instantiate(m_DestroyText);
            m_CopyDestroyText.transform.SetParent(transform);
            Vector2 ThisPanelPosition = gameObject.GetComponent<RectTransform>().anchoredPosition;
            m_CopyDestroyText.GetComponent<RectTransform>().anchoredPosition = new Vector2(130, 0 );

            AmountOfOresToDeleteInfloat = (gameObject.GetComponent<ThingData>().AmountOfOres / 2) + 1;
            gameObject.GetComponent<ThingData>().AmountOfOresToDelete = (int)AmountOfOresToDeleteInfloat;

            transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = gameObject.GetComponent<ThingData>().AmountOfOresToDelete.ToString();

            for (int i = 0; i < 80; i++)
            {
                yield return null;
            }
            
            
            while (!Input.GetMouseButton(0) & !Input.GetKeyDown(KeyCode.Tab))
            {
               
                yield return null;
                //if (Input.GetAxis("Mouse ScrollWheel") != 0)
                //{
                //    print(Input.GetAxis("Mouse ScrollWheel"));
                //}
                if (AmountOfOresToDeleteInfloat>20)
                {
                    AmountOfOresToDeleteInfloat += (Input.GetAxis("Mouse ScrollWheel") * gameObject.GetComponent<ThingData>().AmountOfOresToDelete) / 4;
                    gameObject.GetComponent<ThingData>().AmountOfOresToDelete = (int)Mathf.Clamp(AmountOfOresToDeleteInfloat, 1, gameObject.GetComponent<ThingData>().AmountOfOres);
                }
                else
                {
                    AmountOfOresToDeleteInfloat += Input.GetAxis("Mouse ScrollWheel") * 5;
                    gameObject.GetComponent<ThingData>().AmountOfOresToDelete = (int)Mathf.Clamp(AmountOfOresToDeleteInfloat, 1, gameObject.GetComponent<ThingData>().AmountOfOres);
                }
                string StringOfAmountOfOresToDelete = gameObject.GetComponent<ThingData>().AmountOfOresToDelete.ToString();
                transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = StringOfAmountOfOresToDelete;
            }
            GameObject ore = Instantiate(    OreFolder.transform.GetChild(     gameObject.GetComponent<ThingData>().OreNumber       ).gameObject      );
            GameObject player = gameObject.transform.parent.GetComponent<ThingData>().ObjectPlayer;
            ore.transform.position = player.transform.position + (player.transform.GetChild(0).forward * 2);
            ore.transform.parent = player.transform.parent;
            
            ore.transform.localScale = ore.transform.localScale * Mathf.Clamp(1 + (gameObject.GetComponent<ThingData>().AmountOfOresToDelete - ore.GetComponent<ThingData>().OreAmountInrock) *0.05f,0,100);
                                    
            ore.GetComponent<ThingData>().OreAmountInrock = gameObject.GetComponent<ThingData>().AmountOfOresToDelete;


            gameObject.GetComponent<ThingData>().AmountOfOres -= gameObject.GetComponent<ThingData>().AmountOfOresToDelete;
            if (gameObject.GetComponent<ThingData>().AmountOfOres == 0)
            {
                Destroy(gameObject);
            }
            transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = gameObject.GetComponent<ThingData>().AmountOfOres.ToString();
            Destroy(transform.GetChild(3).gameObject);
        }
        
        
            for (int i = 0; i < 20; i++)
            {
                yield return null;
            }
            transform.SetParent(transform.parent.GetChild(0));
            functionRuning = false;
        
        gameObject.GetComponent<RectTransform>().anchoredPosition -= new Vector2(500, 500);

        
    }
    public void StrartCortune()
    {
        if (functionRuning == false)
        {
            
            transform.SetParent(transform.parent.parent);
            gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            gameObject.GetComponent<RectTransform>().anchorMax= new Vector2(0, 0);
            functionRuning = true;
            StartCoroutine(DragAndDropingPanel());
        }      
    }
}
