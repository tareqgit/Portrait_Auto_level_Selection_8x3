using UnityEngine;
using System.Collections;

public class Pop_up : MonoBehaviour {
    public static float speed = 1.2f;

    public static bool open=true;
	
	// Update is called once per frame
	void Update () {
        float value = speed * Time.deltaTime;
        if (open)
        {
            if (gameObject.GetComponent<RectTransform>().localScale.x <= 1f)
            {
                gameObject.GetComponent<RectTransform>().localScale += new Vector3(value, value, value);

            }
            else
            {
                Destroy(gameObject.GetComponent<Pop_up>());
            }
        }else
        {
            if (gameObject.GetComponent<RectTransform>().localScale.x >= .5f)
            {
                gameObject.GetComponent<RectTransform>().localScale -= new Vector3(value, value, value);

            }
            else
            {
                Destroy(gameObject.GetComponent<Pop_up>());
                gameObject.SetActive(false);
            }
        }
    }
}
