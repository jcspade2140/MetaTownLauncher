using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Links : MonoBehaviour
{
    public Button news, events, world, shop, notice, myProfile;
    public List<Button> others = new List<Button>();

    private void Start()
    {
        news.onClick.AddListener(OnClickNews);
        events.onClick.AddListener(OnClickEvents);
        world.onClick.AddListener(OnClickWorld);
        shop.onClick.AddListener(OnClickShop);
        notice.onClick.AddListener(OnClickNotice);
        myProfile.onClick.AddListener(OnClickMyProfile);

        for (int i = 0; i < others.Count; i++)
        {
            others[i].onClick.AddListener(OnClickOthers);
        }
    }

    private void OnClickMyProfile()
    {
        Debug.Log("My Profile Clicked!");
        Application.OpenURL("http://www.meta-town.io/");
    }

    private void OnClickNotice()
    {
        Debug.Log("Notice Clicked!");
        Application.OpenURL("http://www.meta-town.io/");
    }

    private void OnClickShop()
    {
        Debug.Log("Shop Clicked!");
        Application.OpenURL("http://www.meta-town.io/");
    }

    private void OnClickWorld()
    {
        Debug.Log("World Clicked!");
        Application.OpenURL("http://www.meta-town.io/");
    }

    private void OnClickEvents()
    {
        Debug.Log("Events Clicked!");
        Application.OpenURL("http://www.meta-town.io/");
    }

    private void OnClickNews()
    {
        Debug.Log("News Clicked!");
        Application.OpenURL("http://www.meta-town.io/");
    }

    public void OnClickOthers()
    {
        Debug.Log("Others Clicked!");
        Application.OpenURL("http://www.meta-town.io/");
    }
}
