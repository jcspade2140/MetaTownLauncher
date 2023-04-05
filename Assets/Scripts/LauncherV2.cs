using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows;


public class LauncherV2 : MonoBehaviour
{
    public enum LauncherStatus
    {
        ready,
        failed,
        downloadingGame,
        downloadingUpdate
    }

    string rootPath;
    string versionFile;
    string gameZip;
    string gameExe;
    string patchNotes;

    public TMP_Text btnTxt;
    public TMP_Text versionTxt;
    public TMP_Text progressTxt;
    public Image progressBar;
    public TMP_Text textState;

    [Header("Patch Note Related")]
    public GameObject patchPanel;
    public TMP_Text patchTxt;

    private LauncherStatus _status;
    internal LauncherStatus Status
    {
        get => _status;
        set
        {
            _status = value;
            switch (_status)
            {
                case LauncherStatus.ready:
                    btnTxt.text = "PLAY";
                    textState.text = "Ready";
                    progressTxt.text = "Client is up to date.";
                    break;
                case LauncherStatus.failed:
                    btnTxt.text = "RETRY";
                    textState.text = "Failed";
                    break;
                case LauncherStatus.downloadingGame:
                    btnTxt.text = "DOWNLOADING";
                    textState.text = "Downloading";
                    break;
                case LauncherStatus.downloadingUpdate:
                    btnTxt.text = "UPDATING";
                    textState.text = "Updating";
                    break;
                default:
                    break;
            }
        }
    }

    void Start()
    {
        btnTxt.transform.GetComponentInParent<Button>().onClick.AddListener(PlayButton);

        Screen.SetResolution(1080, 540, false);

        rootPath = Directory.GetCurrentDirectory();
        versionFile = Path.Combine(rootPath, "Version.txt");

#if UNITY_STANDALONE_WIN

        gameZip = Path.Combine(rootPath, "Windows.zip");
        gameExe = Path.Combine(rootPath, "Windows", "MetaTown.exe");

#endif

#if UNITY_STANDALONE_OSX

        gameZip = Path.Combine(rootPath, "MacOS.zip");
        gameExe = Path.Combine(rootPath, "MacOS", "MetaTown.app");

#endif
        CheckForUpdates();
    }

    public void PlayButton()
    {
        if (File.Exists(gameExe) && Status == LauncherStatus.ready)
        {


#if UNITY_STANDALONE_WIN

            ProcessStartInfo startInfo = new ProcessStartInfo(gameExe);
            startInfo.WorkingDirectory = Path.Combine(rootPath, "Windows");
            Process.Start(startInfo);
            Application.Quit();

#endif

#if UNITY_STANDALONE_OSX



            ProcessStartInfo startInfo = new ProcessStartInfo(gameExe);
            startInfo.WorkingDirectory = Path.Combine(rootPath, "MacOS");
            Process.Start(startInfo);
            versionTxt.text = "Opening1 .app";


            System.Diagnostics.Process.Start(Path.Combine(rootPath, "MacOS"));
            versionTxt.text = "Opening2 .app";


            Process p = new Process();
            p.StartInfo.FileName = rootPath;
            versionTxt.text = "Opening3 .app";


            //Application.Quit();
#endif
        }
        else if (Status == LauncherStatus.failed)
        {
            CheckForUpdates();
            versionTxt.text = "Play Button Failed";
        }
    }

    public void PatchButton()
    {
        patchPanel.SetActive(true);
        patchTxt.text = patchNotes;
    }

    private void CheckPatchNotes()
    {
        WebClient webClient = new WebClient();

#if UNITY_STANDALONE_WIN

        patchNotes = webClient.DownloadString("https://metaciti-assets.s3.ap-northeast-2.amazonaws.com/MetaTownLauncher/Windows/PatchNotes.txt");

#endif

#if UNITY_STANDALONE_OSX

        patchNotes = webClient.DownloadString("https://metaciti-assets.s3.ap-northeast-2.amazonaws.com/MetaTownLauncher/MacOS/PatchNotes.txt");

#endif

    }

    private void CheckForUpdates()
    {
        CheckPatchNotes();

        if (File.Exists(versionFile))
        {
            Version localVersion = new Version(File.ReadAllText(versionFile));
            versionTxt.text = localVersion.ToString();

            UnityEngine.Debug.Log("Version is: " + localVersion);

            try
            {
                WebClient webClient = new WebClient();

#if UNITY_STANDALONE_WIN

                Version onlineVersion = new Version(webClient.DownloadString("https://metaciti-assets.s3.ap-northeast-2.amazonaws.com/MetaTownLauncher/Windows/Version.txt"));

#endif

#if UNITY_STANDALONE_OSX

                Version onlineVersion = new Version(webClient.DownloadString("https://metaciti-assets.s3.ap-northeast-2.amazonaws.com/MetaTownLauncher/MacOS/Version.txt"));

#endif


                if (onlineVersion.IsDifferentThan(localVersion))
                {
                    InstallGameFiles(false, onlineVersion);
                }
                else
                {
                    Status = LauncherStatus.ready;
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogException(ex, this);


                versionTxt.text = "Download Version File Failed";
            }
        }
        else
        {
            InstallGameFiles(false, Version.zero);
        }
    }

    private void InstallGameFiles(bool _isUpdate, Version _onlineVersion)
    {
        try
        {
            WebClient webClient = new WebClient();
            if (_isUpdate)
            {
                Status = LauncherStatus.downloadingUpdate;
            }
            else
            {
                Status = LauncherStatus.downloadingGame;

#if UNITY_STANDALONE_WIN

                _onlineVersion = new Version(webClient.DownloadString("https://metaciti-assets.s3.ap-northeast-2.amazonaws.com/MetaTownLauncher/Windows/Version.txt"));

#endif

#if UNITY_STANDALONE_OSX

                _onlineVersion = new Version(webClient.DownloadString("https://metaciti-assets.s3.ap-northeast-2.amazonaws.com/MetaTownLauncher/MacOS/Version.txt"));

#endif


            }


#if UNITY_STANDALONE_WIN

            webClient.DownloadFileAsync(new Uri("https://metaciti-assets.s3.ap-northeast-2.amazonaws.com/MetaTownLauncher/Windows/Windows.zip"), gameZip, _onlineVersion);

#endif

#if UNITY_STANDALONE_OSX

            webClient.DownloadFileAsync(new Uri("https://www.dropbox.com/s/x9zj5bxmqhrpzk4/MacOS.zip?dl=1"), gameZip, _onlineVersion);

#endif


            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);

            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadGameCompletedCallback);


        }
        catch
        {
            Status = LauncherStatus.failed;
            versionTxt.text = "Install Failed";
        }
    }

    private void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
    {


        //progressTxt.text = e.BytesReceived.ToString() + " Bytes" + " / " + e.TotalBytesToReceive.ToString() + " Bytes" + " Percent: " + e.ProgressPercentage.ToString() + "%";
        //progressTxt.text =  String.Format("{0:0.##}", e.BytesReceived * 0.000001) + " MB" + " / " + String.Format("{0:0.##}", e.TotalBytesToReceive * 0.000001) + " MB"; 
        progressTxt.text = (e.BytesReceived * 0.000001).ToString("0") + " MB" + " out of " + (e.TotalBytesToReceive * 0.000001).ToString("0") + " MB downloaded.";
        progressBar.fillAmount = e.ProgressPercentage / 100f;

        if(e.ProgressPercentage >= 99f)
        {
            textState.text = "Installing";
            progressTxt.text = "Please Wait . . .";
            progressBar.fillAmount = 1f;
            btnTxt.text = "INSTALLING";
        }

    }

    private void DownloadGameCompletedCallback(object sender, AsyncCompletedEventArgs e)
    {
        
        try
        {
            

            string onlineVersion = ((Version)e.UserState).ToString();

            ZipFile.ExtractToDirectory(gameZip, rootPath, true); //Supposed to have true

            File.Delete(gameZip);

            File.WriteAllText(versionFile, onlineVersion);

            progressTxt.text = "";
            versionTxt.text = onlineVersion;
            Status = LauncherStatus.ready;

        }
        catch (Exception ex)
        {
            Status = LauncherStatus.failed;
            versionTxt.text = "Unzipping Failed";
        }
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
