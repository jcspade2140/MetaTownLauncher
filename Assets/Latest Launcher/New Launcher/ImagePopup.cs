using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ImagePopup : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    public Image image;

    public int imageCount = 0;

    public Button nextButton, prevButton;
    public bool transitioning;

    public GameObject markerPrefab;
    public RectTransform markerPosition;
    public List<Image> markers = new List<Image>();

    private void Start()
    {
        nextButton.onClick.AddListener(NextImage);
        prevButton.onClick.AddListener(PreviousImage);

        for (int i = 0; i < sprites.Count; i++)
        {
            markers.Add(Instantiate(markerPrefab, markerPosition).GetComponent<Image>());

            if(i != imageCount)
            {
                Color color = markers[i].color;
                color.a = 0.2f;
                markers[i].color = color;
            }
        }

        transitioning = false;
        StartCoroutine(Loop());
    }
    void NextImage()
    {
        if (transitioning)
            return;
        Debug.Log("Next Image!");
        if (imageCount == sprites.Count - 1)
            imageCount = 0;
        else
            imageCount++;
        StartCoroutine(FadeOut());
    }

    void PreviousImage()
    {
        if (transitioning)
            return;

        Debug.Log("Next Image!");

        if (imageCount == 0)
        {
            imageCount = sprites.Count;
        }
        imageCount--;
        StartCoroutine(FadeOut());
    }


    IEnumerator Loop()
    {
        yield return new WaitForSeconds(5f);

        if (transitioning)
            yield return null;
        else
            NextImage();
    }

    IEnumerator FadeOut()
    {
        transitioning = true;
        Color color = image.color;
        color.a = 1f;

        while (color.a > 0)
        {
            color.a -= Time.deltaTime;
            image.color = color;
            yield return null;
        }
        image.sprite = sprites[imageCount];
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        for (int i = 0; i < markers.Count; i++)
        {
            if (i != imageCount)
            {
                Color markerColor = markers[i].color;
                markerColor.a = 0.2f;
                markers[i].color = markerColor;
            }
            else
            {
                Color markerColor = markers[i].color;
                markerColor.a = 1f;
                markers[i].color = markerColor;
            }
        }

        Color color = image.color;
        color.a = 0f;

        while (color.a < 1f)
        {
            color.a += Time.deltaTime;
            image.color = color;
            yield return null;
        }
        transitioning = false;

        StartCoroutine(Loop());
    }
}
