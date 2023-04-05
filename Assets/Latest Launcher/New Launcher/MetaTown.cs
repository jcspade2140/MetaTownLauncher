using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Net;

public class MetaTown : MonoBehaviour
{
    [Header("Panels")]
    public GameObject installStatus;
    public GameObject installNow;

    [Header("Install Status")]
    public TMP_Text textState;
    public TMP_Text textVersion;
    public TMP_Text textProgress;
    public Image imageProgressBar;

    [Header("Main Button Status")]
    public Button buttonMain;
    public Image imageMain;
    public Image imageMainBlur;
    public TMP_Text textMain;


    [Header("Install Now")]
    public Button buttonInstallNow;

    public string directory;
    /*
    private void Awake()
    {
        buttonInstallNow.onClick.AddListener(OnClickInstallNow);
    }

    void OnClickInstallNow()
    {
        Status = GameStatus.downloading;
    }
    
    public enum GameStatus
    {
        notInstalled,
        downloading,
        updating,
        ready,
        failed,
    }

    GameStatus gameStatus;
    internal GameStatus Status
    {
        get => gameStatus;
        set
        {
            gameStatus = value;
            switch (gameStatus)
            {
                case GameStatus.notInstalled:
                    installNow.SetActive(true);
                    installStatus.SetActive(false);
                    break;
                case GameStatus.downloading:
                    installNow.SetActive(false);
                    installStatus.SetActive(true);

                    Color dowloadColor = new Color(248f/255f, 155/255f, 43/255f, 255f/255f) ;
                    imageMain.color = dowloadColor;
                    imageMainBlur.color = dowloadColor;
                    textMain.text = "DOWNLOADING";

                    buttonMain.interactable = false;
                    break;

                case GameStatus.updating:
                    installNow.SetActive(false);
                    installStatus.SetActive(true);

                    Color updateColor = new Color(248f / 255f, 155 / 255f, 43 / 255f, 255f / 255f);
                    imageMain.color = updateColor;
                    imageMainBlur.color = updateColor;
                    textMain.text = "UPDATING";

                    buttonMain.interactable = false;
                    break;
                case GameStatus.failed:

                    break;
                case GameStatus.ready:

                    break;
            }
        }
    }
    */
    private void Start()
    {
        directory = Path.Combine(Directory.GetCurrentDirectory(), "Windows");
        CheckGameFolder();
    }

    void CheckGameFolder()
    {
        if (Directory.Exists(directory))
        {
            Debug.Log("Directory exist!");
            CheckGameVersion();
        }
        else
        {
            Debug.Log("Directory does not exist!");
            //Status = GameStatus.notInstalled;
        }
    }

    void CheckGameVersion()
    {
        Debug.Log("Checking game version!");

        string versionDirectory = Path.Combine(Directory.GetCurrentDirectory(), "version.txt");
        Version localVersion = new Version(0, 0, 0);

        if (File.Exists(versionDirectory))
            localVersion = new Version(File.ReadAllText(versionDirectory));

        WebClient webClient = new WebClient();
        Version onlineVersion = new Version(webClient.DownloadString("https://metaciti-assets.s3.ap-northeast-2.amazonaws.com/MetaTownLauncher/Windows/Version.txt"));



        Debug.Log("Local Version: " + localVersion + "\nOnline Version: " + onlineVersion);
    }

    struct Version
    {
        internal static Version zero = new Version(0, 0, 0);

        private short major;
        private short minor;
        private short subMinor;

        internal Version(short _major, short _minor, short _subMinor)
        {
            major = _major;
            minor = _minor;
            subMinor = _subMinor;
        }
        internal Version(string _version)
        {
            string[] versionStrings = _version.Split('.');
            if (versionStrings.Length != 3)
            {
                major = 0;
                minor = 0;
                subMinor = 0;
                return;
            }

            major = short.Parse(versionStrings[0]);
            minor = short.Parse(versionStrings[1]);
            subMinor = short.Parse(versionStrings[2]);
        }

        internal bool IsDifferentThan(Version _otherVersion)
        {
            if (major != _otherVersion.major)
            {
                return true;
            }
            else
            {
                if (minor != _otherVersion.minor)
                {
                    return true;
                }
                else
                {
                    if (subMinor != _otherVersion.subMinor)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override string ToString()
        {
            return $"{major}.{minor}.{subMinor}";
        }
    }

}
