using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class move : MonoBehaviour
{
    public static float speed = .8f;

    public static bool next = true;
   
    void Update()
    {
        float value = speed * Time.deltaTime;
        if (gameObject.GetComponent<RectTransform>().localScale.x >= 0.5f)
        {
            gameObject.GetComponent<RectTransform>().localScale -= new Vector3(value, value, value);
           
        }
        else
        {
            gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            if (next)
            {
                Level_manager.instance.ver_parents[Level_manager.cur_Page].SetActive(true);
                Level_manager.cur_Page++;
                Level_manager.instance.m_text.text = "Page: " + Level_manager.cur_Page;
            }
            else
            {
                Level_manager.cur_Page--;
                Level_manager.instance.m_text.text = "Page: " + Level_manager.cur_Page;
                Level_manager.instance.ver_parents[Level_manager.cur_Page-1].SetActive(true);
               
            }
            Destroy(gameObject.GetComponent<move>());
           
            gameObject.SetActive(false);




        }
    }

}
