using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slideshow : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    public Image picture;
    public int pictureCount = 0;

    public GameObject markerPrefab;
    public RectTransform markersPosition;
    public List<Image> markers = new List<Image>();
    private void Awake()
    {
        SpawnMarkers();
    }

    private void Start()
    {
        StartCoroutine(ShowPicture());
    }

    void SpawnMarkers()
    {
        for (int i = 0; i < sprites.Count; i++)
        {
            Image marker = Instantiate(markerPrefab, markersPosition).GetComponent<Image>();
            Color color = marker.color;
            color.a = 0.25f;
            marker.color = color;
            markers.Add(marker);
        }
    }

    void NextPicture()
    {
        if (pictureCount == sprites.Count - 1)
            pictureCount = 0;
        else
            pictureCount++;

        picture.sprite = sprites[pictureCount];
        StartCoroutine(ShowPicture());
    }

    IEnumerator ShowPicture()
    {
        for (int i = 0; i < markers.Count; i++)
        {
            if (i != pictureCount)
            {
                Color markerColor = markers[i].color;
                markerColor.a = 0.1f;
                markers[i].color = markerColor;
            }
            else
            {
                Color markerColor = markers[i].color;
                markerColor.a = 0.5f;
                markers[i].color = markerColor;
            }
        }

        Color color = picture.color;
        color.a = 0;

        while (color.a < 1f)
        {
            color.a += Time.deltaTime;
            picture.color = color;
            yield return null;
        }

        

        StartCoroutine(HidePicture());
    }

    IEnumerator HidePicture()
    {
        yield return new WaitForSeconds(5f);
        Color color = picture.color;
        color.a = 1;

        while (color.a > 0f)
        {
            color.a -= Time.deltaTime;
            picture.color = color;
            yield return null;
        }

        NextPicture();
    }
   
}
