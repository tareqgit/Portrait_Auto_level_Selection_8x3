using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Level_manager : MonoBehaviour
{
    public static Level_manager instance;


    public Font my_font;

    public int Total_No_of_level;
    private int total_no_of_level;
    public int No_of_Rows;
    public int No_of_columns;



    public static int cur_Page = 0;
    public Sprite backGround_Sprite;

    public Sprite layout_Element_Frame;

    public Sprite bedge_spr;

    public List<Sprite> stars;
    public List<Sprite> locks;
    public Sprite LockedPanel_Spr;

    // public static int[] stars_Store = new int[] { };
    [HideInInspector]
    public List<GameObject> ver_parents;
    [HideInInspector]
    GameObject LevelSelection_parent;

    [HideInInspector] //i don't wanna show him on inspector
    public Text m_text;

    GameObject Locked_panel;

    void Star_playerPref()
    {
        for (int i = 1; i <= Total_No_of_level; i++)
        {
            PlayerPrefs.SetInt("Lvl_" + i + "_Star", 0);
            // Here 0 is Locked and 1 is Unlocked
            if (i >= 2) PlayerPrefs.SetInt("Lock_" + i, 0);  //as 1 is Automitacally Unlocked..
        }
    }

    void Awake()
    {
        #region Should run only One time in a device
        if (PlayerPrefs.GetInt("FIRSTTIMEOPENING", 1) == 1)
        {
            Debug.Log("First Time Opening");

            //Set first time opening to false
            PlayerPrefs.SetInt("FIRSTTIMEOPENING", 0);

            //Do your stuff here
            Star_playerPref();
        }
        else
        {
            Debug.Log("NOT First Time Opening");

            //Do your stuff here

            ////Random Check
            //for (int i = 1; i <= Total_No_of_level; i++)
            //{
            //    PlayerPrefs.SetInt("Lvl_" + i + "_Star", Random.Range(0,3));
            //}

            ////Reset
            //Star_playerPref();
        }

        #endregion

        instance = this; //SimgleTon

        cur_Page = 1;

        total_no_of_level = Total_No_of_level; //for local item reduction calculation

        /// First we need a Universal Parrent for Storing all other elements       
        LevelSelection_parent = _CreateRect_GameObject("Level_Selection", 0, 0, 0, 0, gameObject.transform);
        LevelSelection_parent.AddComponent<Image>().color = new Color(1, 1, 1, 0);
        ///Now we need a Background 
        _CreateRect_GameObject("backGround", 0, 0, 0, 0, LevelSelection_parent.transform).AddComponent<Image>().sprite = backGround_Sprite;

        ///Now we need to Create Pages
        for (int page = 1; page <= _page_Calculator(); page++)

        {

            GameObject ver_parent = _CreateRect_GameObject("Vertical_Parent_" + page + "", 0, 0, 45, 40, LevelSelection_parent.transform);
            ver_parent.AddComponent<Image>().color = new Color(1, 1, 1, 0);

            ver_parent.AddComponent<VerticalLayoutGroup>();
            ver_parents.Add(ver_parent);
            if (page >= 2) { ver_parent.SetActive(false); }

            ///now we need the Rows
            for (int row = 1; row <= No_of_Rows; row++)
            {
                GameObject Horz_parent = _CreateRect_GameObject("Horizontal_Parent_" + row + "", 0, 0, 0, 0, ver_parent.transform);
                Horz_parent.AddComponent<Image>().color = new Color(1, 1, 1, 0);
                Horz_parent.AddComponent<HorizontalLayoutGroup>();

                ///Now columns with Item
                for (int col = 1; col <= No_of_columns; col++)
                {

                    GameObject layout_element = _CreateRect_GameObject("Layout_Element_" + col + "", 0, 0, 0, 0, Horz_parent.transform);

                    {
                        Image lay_img = layout_element.AddComponent<Image>();


                        if ((total_no_of_level >= 1)) //if we have enough item to show
                        {
                            total_no_of_level--;
                            lay_img.sprite = layout_Element_Frame;
                            lay_img.preserveAspect = true;

                            //***************for Loacking****************
                            GameObject lock_obj = _CreateRect_GameObject("Lock_" + col + "", 0, 0, 1.5f, 0, layout_element.transform);
                            lock_obj.GetComponent<RectTransform>().localScale = new Vector3(.8f, .8f, .8f);
                            Image lock_img = lock_obj.AddComponent<Image>();
                            ///0 is Lock, 1 is Unlock
                            if ((Total_No_of_level - total_no_of_level) != 1)
                            {
                                lock_img.sprite = locks[PlayerPrefs.GetInt("Lock_" + (Total_No_of_level - total_no_of_level))];//need to be changed
                            }
                            else
                            {
                                lock_img.sprite = locks[1]; //as Level no  1 is always Unlocked
                            }

                            lock_img.preserveAspect = true;
                            //**************for Bedge***************************
                            GameObject bedge_obj = _CreateRect_GameObject("bedge" + col + "", 60, 0, -50f, 0, layout_element.transform);
                            bedge_obj.GetComponent<RectTransform>().localScale = new Vector3(.35f, .35f, .3f);
                            Image bedge_img = bedge_obj.AddComponent<Image>();

                            bedge_img.sprite = bedge_spr;

                            bedge_img.preserveAspect = true;
                            GameObject bedge_text_obj = _CreateRect_GameObject("bedge_Text" + col + "", 0, 0, 0.1f, 0, bedge_obj.transform);
                            bedge_text_obj.GetComponent<RectTransform>().localScale = new Vector3(3f, 3f, 3f);
                            Text b_text = bedge_text_obj.AddComponent<Text>();
                            b_text.text = (Total_No_of_level - total_no_of_level)+"";
                            b_text.font = my_font;
                            b_text.resizeTextForBestFit = true;
                            b_text.resizeTextMaxSize = 20;
                            b_text.alignment = TextAnchor.MiddleCenter;
                            b_text.fontStyle = FontStyle.BoldAndItalic;
                            

                            //*************for Stars**************
                            GameObject stars_obj = _CreateRect_GameObject("Star_" + col + "", 0, 0, 47.45f, 0, layout_element.transform);
                            stars_obj.GetComponent<RectTransform>().localScale = new Vector3(.8f, .8f, .8f);
                            Image str_img = stars_obj.AddComponent<Image>();

                            str_img.sprite = stars[PlayerPrefs.GetInt("Lvl_" + (Total_No_of_level - total_no_of_level) + "_Star")];//need to be changed
                            str_img.preserveAspect = true;

                            //***************ON_Click*****************
                            Button L_b = layout_element.AddComponent<Button>();

                            if (PlayerPrefs.GetInt("Lock_" + (Total_No_of_level - total_no_of_level)) == 0)
                            {
                                if ((Total_No_of_level - total_no_of_level) != 1) //for first level cheaking
                                {
                                    L_b.onClick.AddListener(Locked);
                                }else
                                {
                                    L_b.onClick.AddListener(Unlocked); //for first Level
                                }
                            }else
                            {
                                
                                L_b.onClick.AddListener(Unlocked);
                            }
                        }
                        else //if we don't have enough item to show the show empty box
                        {
                            lay_img.sprite = layout_Element_Frame;
                            lay_img.preserveAspect = true;
                            lay_img.color = new Color(1, 1, 1, 0);
                        }
                    }
                    layout_element.AddComponent<LayoutElement>();

                }
            }
        }

        GameObject m_next = _createButton("next", LevelSelection_parent.transform, 5, 0, 3.28f, 0, 0.6568f, 0, 1, 0.0601f);
        m_next.AddComponent<Button>().onClick.AddListener(_Next);

        GameObject m_prev = _createButton("prev", LevelSelection_parent.transform, 0, -2, -0.5f, 0, 0, 0, .329f, 0.0601f);
        m_prev.AddComponent<Button>().onClick.AddListener(_Prev);

        GameObject m_back = _createButton("Back", LevelSelection_parent.transform, -8, -2, -3.051758f,3.95993f, 0.0166f, 0.932f, .2085f, 1f);
        m_back.AddComponent<Button>().onClick.AddListener(_Back);

        GameObject m_Page = _createText("Page_no", LevelSelection_parent.transform, 2.7f, 1.29995f, -1f, 0, .3485f, 0, 0.6457f, 0.0656f);
        m_text = m_Page.GetComponent<Text>();
        m_text.text = "Page: " + cur_Page;
        m_text.font = my_font;
        m_text.resizeTextForBestFit = true;
        m_text.resizeTextMaxSize = 20;
        m_text.alignment = TextAnchor.MiddleCenter;
        m_text.fontStyle = FontStyle.BoldAndItalic;



        ////**********************Extra Things Like Locked Panel************************
                Locked_panel = _CreateRect_GameObject("locked_Panel", 0, 0, 0, 0, LevelSelection_parent.transform);
        Image locked_panel_img = Locked_panel.AddComponent<Image>();
        locked_panel_img.sprite = LockedPanel_Spr;
        locked_panel_img.preserveAspect = true;

        Button NextTime = _createButton("NextTime", Locked_panel.transform, 7.99985f, 1.525f, 2.98086f, 1.525879f, .6478f, 0.3097f, .912f, .413f).AddComponent<Button>();
        NextTime.onClick.AddListener(_NextTime);

        Button BuyNow= _createButton("BuyNow", Locked_panel.transform, -4f, 0, 2.11f, 1.5258f, 0.0825f, 0.3097f, 0.339f, 0.4137f).AddComponent<Button>();
        BuyNow.onClick.AddListener(_Buy_Now);


        //Now deActive LockPanel
        Locked_panel.SetActive(false);

    }





    GameObject _createButton(string name, Transform m_parent, float left, float right, float top, float bottom, float anc_Min_X, float anc_Min_y, float anc_Max_X, float anc_Max_Y)
    {
        GameObject next = new GameObject(name);
        next.transform.SetParent(m_parent.transform);
        RectTransform trans = next.gameObject.AddComponent<RectTransform>();
        trans.pivot = new Vector2(1, 0);

        trans.offsetMin = new Vector2(left, bottom);
        trans.offsetMax = -new Vector2(right, top);
        trans.anchorMin = new Vector2(anc_Min_X, anc_Min_y);
        trans.anchorMax = new Vector2(anc_Max_X, anc_Max_Y);
        trans.localScale = new Vector3(1, 1, 1);


        next.AddComponent<Image>().color = new Color(1, 1, 1, 0);
        return next;

    }

    GameObject _createText(string name, Transform m_parent, float left, float right, float top, float bottom, float anc_Min_X, float anc_Min_y, float anc_Max_X, float anc_Max_Y)
    {
        GameObject next = new GameObject(name);
        next.transform.SetParent(m_parent.transform);
        RectTransform trans = next.gameObject.AddComponent<RectTransform>();
        trans.pivot = new Vector2(.5f, 0);

        trans.offsetMin = new Vector2(left, bottom);
        trans.offsetMax = -new Vector2(right, top);
        trans.anchorMin = new Vector2(anc_Min_X, anc_Min_y);
        trans.anchorMax = new Vector2(anc_Max_X, anc_Max_Y);
        trans.localScale = new Vector3(1, 1, 1);


        next.AddComponent<Text>();
        return next;

    }








    GameObject _CreateRect_GameObject(string name, float left, float right, float top, float bottom, Transform m_parent_Transform)
    {
        GameObject verticalPrent = new GameObject(name);
        verticalPrent.transform.SetParent(m_parent_Transform);
        RectTransform trans = verticalPrent.gameObject.AddComponent<RectTransform>();
        // trans.anchoredPosition = new Vector2(1, 1);

        trans.offsetMin = new Vector2(left, bottom);
        trans.offsetMax = -new Vector2(right, top);

        trans.anchorMin = new Vector2(0, 0);
        trans.anchorMax = new Vector2(1, 1);
        trans.localScale = new Vector3(1, 1, 1);

        return verticalPrent;

    }

    private int _page_Calculator()
    {
        if ((Total_No_of_level % (No_of_columns * No_of_Rows)) != 0)

        {
            return Total_No_of_level / (No_of_columns * No_of_Rows) + 1;

        }
        else
        {
            return Total_No_of_level / (No_of_columns * No_of_Rows);
        }
    }




















    void _Next()
    {
        if (_page_Calculator() > cur_Page)
        {
            if(ver_parents[cur_Page-1].gameObject.GetComponent<move>()==null)
                             ver_parents[cur_Page - 1].gameObject.AddComponent<move>();
            move.next = true;

            Debug.Log("Next");
        }
    }

    void _Prev()
    {
        if (cur_Page > 1)
        {
            if (ver_parents[cur_Page - 1].gameObject.GetComponent<move>() == null)
                                ver_parents[cur_Page - 1].gameObject.AddComponent<move>();
            move.next = false;

            Debug.Log("prev");
        }
    }

    void _Back()
    {
        LevelSelection_parent.SetActive(false);
        Debug.Log("Back");
    }

    void Locked()
    {
        Locked_panel.SetActive(true);
        Locked_panel.GetComponent<RectTransform>().localScale =new Vector3(.5f,.5f,.5f);
        if (Locked_panel.GetComponent<Pop_up>() == null)
                        Locked_panel.AddComponent<Pop_up>();
        Pop_up.open = true;
        Debug.Log("Locked");
    }
    void Unlocked()
    {
        LevelSelection_parent.SetActive(false); //may be there should be another Panel open instead of this
        
        Debug.Log("Unlocked");
    }
    void _NextTime()
    {
        if (Locked_panel.GetComponent<Pop_up>() == null)
        {
            Locked_panel.AddComponent<Pop_up>();
        }
        Pop_up.open = false;
        //Locked_panel.SetActive(false);//as this Line is under PopUp class so don't need of that anymore
        Debug.Log("Next Time");
    }
    void _Buy_Now()
    {
        Debug.Log("BuyNow");
    }
}
