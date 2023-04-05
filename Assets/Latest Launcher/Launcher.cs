using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Diagnostics;

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class Launcher : MonoBehaviour
{
    public bool koreanLanguage;
    public Button buttonKorea;
    public Button buttonAmerica;
    public List<TMP_Text> staticText = new List<TMP_Text>();
    public List<string> krLanguage = new List<string>();
    public List<string> enLanguage = new List<string>();
    public List<string> krProgress = new List<string>();
    public List<string> enProgress = new List<string>();

    [Header("Panel Reference")]
    public GameObject panelInstall;
    public GameObject panelMainButton;
    public GameObject panelStatus;

    public Button buttonInstall;
    public Button buttonMain;

    public TMP_Text textMainButton;
    public TMP_Text textState;
    public TMP_Text textProgress;
    public TMP_Text textVersion;

    public Image imageProgressbar;
    public Image imageMainButton;
    public Image imageMainButtonBlur;

    public string directory;
    public string gameDirectory;
    public string exeDirectory;

    public string localVersionDirectory;
    public string onlineVersionURL;
    public string localVersion;
    public string onlineVersion;

    public string downloadDirectory;
    public string downloadGameURL;

    [Header("Launcher State")]
    public string launcherState = "";


    private void Awake()
    {
        string[] enLang = new string[] { "Meta Town", "An Extension of Reality", "News", "Event", "World", "Shop", "Notice", "My Profile", "INSTALL"};
        enLanguage.AddRange(enLang);

        string[] krLang = new string[] { "Meta Town", "현실 너머의 연장선", "소식", "이벤트", "세계", "쇼핑", "공지", "내 프로필", "설치" };
        krLanguage.AddRange(krLang);



        directory = Directory.GetCurrentDirectory();

        gameDirectory = Path.Combine(directory, "Windows");
        exeDirectory = Path.Combine(gameDirectory, "MetaTown.exe");

        localVersionDirectory = Path.Combine(directory, "Version.txt");
        onlineVersionURL = "https://metaciti-assets.s3.ap-northeast-2.amazonaws.com/MetaTownLauncher/Windows/Version.txt";

        downloadGameURL = "https://metaciti-assets.s3.ap-northeast-2.amazonaws.com/MetaTownLauncher/Windows/Windows.zip";
        downloadDirectory = Path.Combine(directory, "Windows.zip");

        buttonMain.onClick.AddListener(OnClickMainButton);
        buttonInstall.onClick.AddListener(OnClickInstallButton);

        buttonMain.interactable = false;

        buttonKorea.onClick.AddListener(delegate { StaticLabelTranslate("kr"); });
        buttonAmerica.onClick.AddListener(delegate { StaticLabelTranslate("en"); });
    }

    void StaticLabelTranslate(string language)
    {
        
        switch (language)
        {
            case "kr":
                koreanLanguage = true;
                if(launcherState == "needUpdate")
                {
                    textState.text = "새 업데이트 가능";
                    textMainButton.text = $"업데이트<size=15>\n버전 {GetOnlineVersion()}</size>";

                    textProgress.text = $"클라이언트 버전 {GetOnlineVersion()} 를 다운로드할 수 있습니다.";
                    textVersion.text = $"버전 {GetLocalVersion()}";
                }

                if(launcherState == "ready")
                {
                    textProgress.text = "클라이언트 버전이 최신 버전입니다.";
                    textVersion.text = $"버전 {GetLocalVersion()}";

                    textState.text = "준비 완료";
                    textMainButton.text = "시작";
                }

        

                for (int i = 0; i < staticText.Count; i++)
                {
                    staticText[i].text = krLanguage[i];
                }
                
                break;

            case "en":
                koreanLanguage = false;
                if (launcherState == "needUpdate")
                {
                    textState.text = "New Update Available";
                    textMainButton.text = $"UPDATE<size=15>\nversion {GetOnlineVersion()}</size>";

                    textProgress.text = $"Client version {GetOnlineVersion()} is available for download.";
                    textVersion.text = $"version {GetLocalVersion()}";
                }

                if (launcherState == "ready")
                {
                    textProgress.text = "Client is up to date.";
                    textVersion.text = $"version {GetLocalVersion()}";

                    textState.text = "Ready";
                    textMainButton.text = "PLAY";
                }


                for (int i = 0; i < staticText.Count; i++)
                {
                    staticText[i].text = enLanguage[i];
                }
                break;
        }
    }

    private void Start()
    {
        StaticLabelTranslate("en");
        koreanLanguage = false;

        localVersion = GetLocalVersion();
        onlineVersion = GetOnlineVersion();

        if(!GameInstalled())
        {
            panelInstall.SetActive(true);
            panelMainButton.SetActive(false);
            panelStatus.SetActive(false);
        }
        else
        {
            panelInstall.SetActive(false);
            panelMainButton.SetActive(true);
            panelStatus.SetActive(true);
           
            if(localVersion != onlineVersion)
            {
                LauncherNeedUpdate();
            }
            else
            {
                LauncherReady();
            }
        }
    }

    void LauncherNeedUpdate()
    {
        buttonMain.interactable = true;

        if(!koreanLanguage)
        {
            textState.text = "New Update Available";
            textMainButton.text = $"UPDATE<size=15>\nversion {GetOnlineVersion()}</size>";

            textProgress.text = $"Client version {GetOnlineVersion()} is available for download.";
            textVersion.text = $"version {GetLocalVersion()}";
        }
        else
        {
            textState.text = "새 업데이트 가능";
            textMainButton.text = $"업데이트<size=15>\n버전 {GetOnlineVersion()}</size>";

            textProgress.text = $"클라이언트 버전 {GetOnlineVersion()} 를 다운로드할 수 있습니다.";
            textVersion.text = $"버전 {GetLocalVersion()}";
        }

        Color color = new Color(60f / 255f, 200f / 255f, 110f / 255f, 255f / 255f);
        imageMainButton.color = color;
        imageMainButtonBlur.color = color;

        Outline outline = buttonMain.GetComponent<Outline>();
        outline.effectColor = color;

        launcherState = "needUpdate";

    }
    void LauncherReady()
    {
        buttonMain.interactable = true;

        if(!koreanLanguage)
        {
            textProgress.text = "Client is up to date.";
            textVersion.text = $"version {GetLocalVersion()}";

            textState.text = "Ready";
            textMainButton.text = "PLAY";
        }
        else
        {
            textProgress.text = "클라이언트 버전이 최신 버전입니다.";
            textVersion.text = $"버전 {GetLocalVersion()}";

            textState.text = "준비 완료";
            textMainButton.text = "시작";
        }

        Color color = new Color(50f / 255f, 150f / 255f, 200f / 255f, 255f / 255f);
        imageMainButton.color = color;
        imageMainButtonBlur.color = color;

        Outline outline = buttonMain.GetComponent<Outline>();
        outline.effectColor = color;

        launcherState = "ready";
    }

    void LauncherPlay()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo(exeDirectory);
        startInfo.WorkingDirectory = gameDirectory;
        Process.Start(startInfo);
        Application.Quit();
    }

    public void OnClickInstallButton()
    {
        buttonMain.interactable = false;

        panelInstall.SetActive(false);
        panelMainButton.SetActive(true);
        panelStatus.SetActive(true);

        DownloadGame();
    }

    public void OnClickMainButton()
    {
        buttonMain.interactable = false;

        switch (launcherState)
        {
            case "ready":
                LauncherPlay();
                break;

            case "needUpdate":
                DownloadGame();
                break;
        }
    }

    bool GameInstalled()
    {
        bool gameInstalled = false;

        if(Directory.Exists(gameDirectory))
        {
            if(File.Exists(exeDirectory))
            {
                gameInstalled = true;
            }
            else
            {
                gameInstalled = false;
            }
        }
        else
        {
            gameInstalled = false;
        }

        return gameInstalled;
    }

    
    void DownloadGame()
    {
        WebClient webClient = new WebClient();
        try
        {
            
            webClient.DownloadFileAsync(new Uri(downloadGameURL), downloadDirectory);

            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);
        }
        catch (Exception)
        {
            webClient.DownloadProgressChanged -= new DownloadProgressChangedEventHandler(DownloadProgress);
        }
    }

    private void DownloadProgress(object sender, DownloadProgressChangedEventArgs args)
    {
        string fileSizeDownloaded;
        if (!koreanLanguage)
            fileSizeDownloaded = (args.BytesReceived * 0.000001).ToString("0") + " MB" + " out of " + (args.TotalBytesToReceive * 0.000001).ToString("0") + " MB downloaded.";
        else
            fileSizeDownloaded = (args.BytesReceived * 0.000001).ToString("0") + "mb" + " 중 " + (args.TotalBytesToReceive * 0.000001).ToString("0") + "mb 가 다운로드 되었습니다.";

        float progressBar = args.ProgressPercentage;

        textProgress.text = fileSizeDownloaded;
        textVersion.text = $"<size=20><b>{progressBar}%</b></size>";
        imageProgressbar.fillAmount = progressBar / 100f;

        if(!koreanLanguage)
        {
            textState.text = "Downloading";
            textMainButton.text = $"DOWNLOADING";
        }
        else
        {
            textState.text = "다운로드 중";
            textMainButton.text = $"다운로드 중";
        }

        Color color = new Color(250f/255f, 155f/255f, 40f/255f, 255f/255f);
        imageMainButton.color = color;
        imageMainButtonBlur.color = color;

        Outline outline = buttonMain.GetComponent<Outline>();
        outline.effectColor = color;

        if (args.BytesReceived == args.TotalBytesToReceive)
        {
            if(!koreanLanguage)
            {
                textProgress.text = "Installing please wait. . . .";

                if (launcherState == "needUpdate")
                {
                    textState.text = "Updating";
                    textMainButton.text = $"UPDATING<size=15>\nversion {onlineVersion}</size>";
                }
                else
                {
                    textState.text = "Installing";
                    textMainButton.text = $"INSTALLING<size=15>\nversion {onlineVersion}</size>";
                }
            }
            else
            {
                textProgress.text = "설치중입니다....";

                if (launcherState == "needUpdate")
                {
                    textState.text = "업데이트 중";
                    textMainButton.text = $"업데이트 중<size=15>\n버전 {onlineVersion}</size>";
                }
                else
                {
                    textState.text = "설치 중";
                    textMainButton.text = $"설치 중<size=15>\n버전 {onlineVersion}</size>";
                }
            }

            imageProgressbar.fillAmount = 1f;
            StartCoroutine(Install());
        }
    }

    IEnumerator Install()
    {
        yield return new WaitForSeconds(1f);
        
        try
        {
            ZipFile.ExtractToDirectory(downloadDirectory, directory, true);
            File.Delete(downloadDirectory);

            File.WriteAllText(localVersionDirectory, onlineVersion);
        }
        catch (Exception)
        {

        }

        LauncherReady();
    }

    string GetLocalVersion()
    {
        string version = "";
        if (File.Exists(localVersionDirectory))
            version = File.ReadAllText(localVersionDirectory);
        else
            version = "0.0.0";
        return version;
    }
    string GetOnlineVersion()
    {
        string version = "";
        try
        {
            WebClient webClient = new WebClient();
            version = webClient.DownloadString(onlineVersionURL);
        }
        catch (Exception)
        {
            if (!koreanLanguage)
                version = "Server connection failed.";
            else
                version = "서버 연결에 실패했습니다.";
        }

        return version;
    }

    private void OnDisable()
    {
        
    }



}
