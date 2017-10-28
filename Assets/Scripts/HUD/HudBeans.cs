using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudBeans : MonoBehaviour {

    public PlayerController playerScript;
    public GameObject beansImageObj;
    public int visibleTime;
    private Image beansImage;
    private Text beansText;

    void Start() {
    	beansImage = beansImageObj.GetComponent<Image>();
    	beansText = GetComponent<Text>();
    	
    	Color tmp = beansImage.color;
        tmp.a = 0f;
        beansImage.color = tmp;
        beansText.color = tmp;
    }

    void Update() {
        beansText.text = playerScript.GetBeans().ToString();
    }

    public IEnumerator VisiblityTimer() {
        Color tmp = beansImage.color;
        tmp.a = 1f;
        beansImage.color = tmp;
        beansText.color = tmp;

        yield return new WaitForSeconds(visibleTime);

        while (beansImage.color.a > 0f) {
	        tmp = beansImage.color;
	        tmp.a -= 0.1f;
	        beansImage.color = tmp;
	        beansText.color = tmp;
	        yield return new WaitForSeconds(0.01f);
	    }
    }
}
