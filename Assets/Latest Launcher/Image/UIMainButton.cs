using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMainButton : MonoBehaviour
{
    [Header("Panels")]
    public GameObject installPanel;
    public GameObject statusPanel;
    public GameObject mainButtonPanel;

    [Header("Main Button")]
    public TMP_Text mainButtonLabel;
    public Image mainButtonImage;
    public Image mainButtonImageBlur;

    [Header("Color States")]
    public Color installColor = new Color(60f / 255f, 200f / 255f, 40f / 110f, 255f / 255f);
    public Color installingColor = new Color(250f / 255f, 155f / 255f, 40f / 255f, 255f / 255f);
  
    public Color updateColor = new Color(60f / 255f, 200f / 255f, 40f / 110f, 255f / 255f);
    public Color updatingColor = new Color(250f / 255f, 155f / 255f, 40f / 255f, 255f / 255f);
    public Color readyColor = new Color(50f / 255f, 150f / 255f, 200f / 255f, 255f / 255f);

    private void Start()
    {
        //installColor = new Color(60f / 255f, 200f / 255f, 40f / 110f, 255f / 255f);
        //installingColor = new Color(250f / 255f, 155f / 255f, 40f / 255f, 255f / 255f);
        //updateColor = new Color(60f / 255f, 200f / 255f, 40f / 110f, 255f / 255f);
        //updatingColor = new Color(250f / 255f, 155f / 255f, 40f / 255f, 255f / 255f);
        //readyColor = new Color(50f / 255f, 150f / 255f, 200f / 255f, 255f / 255f);
        State("Install");
    }

    public void State(string state)
    {
        switch (state)
        {
            case "Install":
                installPanel.SetActive(true);
                mainButtonPanel.SetActive(false);
                statusPanel.SetActive(false);


                break;
            case "Installing":
                installPanel.SetActive(false);
                mainButtonPanel.SetActive(true);
                statusPanel.SetActive(true);

                ChangeMainButtonColor(installingColor);



                break;
            case "Download":

                break;
            case "Downloading":

                break;
            case "Update":

                break;
            case "Updating":

                break;
            case "Ready":

                break;
            default:
                break;
        }
    }

    void ChangeMainButtonColor(Color color)
    {
        mainButtonImage.color = color;
        mainButtonImageBlur.color = color;
    }
}
