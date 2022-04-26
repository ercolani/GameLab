using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class CSVDownloader
{
    const string k_googleSheetDocID = "14NBwpw8VwjJN6HReTucol7sAJLopHx_bwhtuopBSp-Q";
    const string url = "https://docs.google.com/spreadsheets/d/" + k_googleSheetDocID + "/export?format=csv";

    internal static IEnumerator DownloadData(System.Action<string> onCompleted)
    {
        yield return new WaitForEndOfFrame();

        string downloadData = null;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Download Error: " + webRequest.error);
                downloadData = PlayerPrefs.GetString("LastDataDownloaded", null);
                string versionText = PlayerPrefs.GetString("LastDataDownloaded", null);
                Debug.Log("Using stale data version: " + versionText);
            }
            else
            {
                //version number will be in the very first cell
                string versionSection = webRequest.downloadHandler.text.Substring(0, 15);
                int spaceIndex = versionSection.IndexOf(' ');
                UnityEngine.Assertions.Assert.IsFalse(spaceIndex == -1, "Could not find a space at the start of the CVS");

                string versionText = webRequest.downloadHandler.text.Substring(spaceIndex + 1, 3);
                Debug.Log("Downloaded Data Version: " + versionText);

                PlayerPrefs.SetString("LastDataDownloaded", webRequest.downloadHandler.text);
                PlayerPrefs.SetString("LastDataDownloadedVersion", versionText);

                downloadData = webRequest.downloadHandler.text;
            }
        }
        onCompleted(downloadData);
    }
}

