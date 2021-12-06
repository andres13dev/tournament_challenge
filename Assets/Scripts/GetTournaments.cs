using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GetTournaments : MonoBehaviour
{
    private string urlTournaments = "https://api.pubg.com/tournaments";
    public GameObject itemPrefab;
    public Transform parentItems;

    [System.Serializable]
    public class GetTournamentsResult
    {
        public TournamentsData[] data;
    }

    [System.Serializable]
    public class Attributes
    {
        public string createdAt;
    }

    [System.Serializable]
    public class TournamentsData
    {
        public string id;
        public Attributes[] attributes;
    }

    private void Start()
    {
        StartCoroutine(GetList());
    }

    IEnumerator GetList()
    {
        WWWForm form = new WWWForm();

        using (UnityWebRequest www = UnityWebRequest.Get(urlTournaments))
        {
            www.SetRequestHeader("Authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJqdGkiOiJkZWI3ZjBkMC0zNjc4LTAxM2EtNzE2Yi0wOWQzZTk3ZDU4YzYiLCJpc3MiOiJnYW1lbG9ja2VyIiwiaWF0IjoxNjM4NTQ0MTQwLCJwdWIiOiJibHVlaG9sZSIsInRpdGxlIjoicHViZyIsImFwcCI6InRvdXJuYW1lbnRfbGlzIn0.vv7M1cnVFpofL3VeEZs2fDfmBzIunRnYIakDS3gG_vA");
            www.SetRequestHeader("Accept", "application/vnd.api+json");

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string nJson = www.downloadHandler.text;
                string nJson1 = nJson.Replace(":{\"cre", ":[{\"cre");
                string nJson2 = nJson1.Replace("}},", "}]},");
                string nJson3 = nJson2.Replace("}}]", "}]}]");
                var myObject = JsonUtility.FromJson<GetTournamentsResult>(nJson3);

                for (int i = 0; i < myObject.data.Length; i++)
                {
                    GameObject item = Instantiate(itemPrefab, parentItems);
                    item.name = "item_" + i.ToString();
                    item.transform.GetChild(0).GetComponent<Text>().text = myObject.data[i].id;
                    item.transform.GetChild(1).GetComponent<Text>().text = myObject.data[i].attributes[0].createdAt.Substring(0, 10);
                }
            }
        }
    }
}
