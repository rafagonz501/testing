using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class Database : MonoBehaviour
{
    private string secretKey = "mySecretKey"; 
    public string addScoreURL = "http://localhost/epochbash/addscore.php?";
    public string highscoreURL = "http://localhost/epochbash/retrievescore.php";

   
    [SerializeField] Text statusText;       
    
    private void Start()
    {
        StartCoroutine(GetScores());
    }

    
    IEnumerator PostScores(string name, int score)
    {
       
        string hash = Md5Sum(name + score + secretKey);

        string post_url = addScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash;

        
        WWW hs_post = new WWW(post_url);
        yield return hs_post; 

        if (hs_post.error != null)
        {
            print("There was an error establishing connection: " + hs_post.error);
        }
    }

    
    IEnumerator GetScores()
    {
        statusText.text = "Establishing connection...";
        WWW hs_get = new WWW(highscoreURL);
        yield return hs_get;

        if (hs_get.error != null)
        {
            print("There was an error establishig connection: " + hs_get.error);
        }
        else
        {
            statusText.text = hs_get.text; 
        }
    }

    public string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

       
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }
    /* void Start()
     {
         StartCoroutine(GetText());
     }

     IEnumerator GetText()
     {
         UnityWebRequest www = UnityWebRequest.Get("http://epochbash/");
         yield return www.SendWebRequest();

         if (www.result != UnityWebRequest.Result.Success)
         {
             Debug.Log(www.error);
         }
         else
         {
             // Show results as text
             Debug.Log(www.downloadHandler.text);

             // Or retrieve results as binary data
             byte[] results = www.downloadHandler.data;
         }
     }*/
}