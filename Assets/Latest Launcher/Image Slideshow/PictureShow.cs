using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureShow : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    public Image image;

    public void Awake()
    {
        image = GetComponent<Image>();
    }
}
