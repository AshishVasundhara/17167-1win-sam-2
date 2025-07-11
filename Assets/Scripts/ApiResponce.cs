using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ApiResponce : MonoBehaviour
{
    public static ApiResponce instacne { get; set; }
    public string Date;
    public string CurrentDate;
    public GameObject NoMatchesObj;
    public GameObject LoadingScreen;
    public List<Response> brazilResponse = new List<Response>();
    public  string apiURL = "https://v3.football.api-sports.io/fixtures?date=";
    

    private void Awake()
    {
        instacne = this;
    }

    [System.Obsolete]
    private void Start()
    {
      //  StartCoroutine(GetRequest(apiURL + Date));

    }
   
    [System.Obsolete]
    public IEnumerator GetRequest(string uri)
    {
        print("---URL--- " + uri);
        LoadingScreen.SetActive(true);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Set the request header with the API key
            webRequest.SetRequestHeader("X-RapidAPI-Key", "497fc34e1de0969c721f48c9a9465e6d");
            webRequest.SetRequestHeader("X-RapidAPI-Host", "http://api-football-v1.p.rapidapi.com/");

            // Send the request and wait for a response
            yield return webRequest.SendWebRequest();

            // Check for errors

            LoadingScreen.SetActive(false);
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
               
            }
            else
            {
                print("-------FootBall Team webRequest----");
                string jsonResponse = webRequest.downloadHandler.text;
                DisplayFixtures(jsonResponse);
                FootBallTeam_Name.instance.FootBall_Team();

            }
        }

        if(brazilResponse.Count==0)
        {
            NoMatchesObj.SetActive(true);
            print("NomatchesObj");
        }
        else
        {
            NoMatchesObj.SetActive(false);

        }
    }

    FixturesResponse fixturesResponse;
    [System.Obsolete]
    void DisplayFixtures(string jsonResponse)
    {
        // Parse the JSON response and display the fixtures
        print("Jsomnn------------------>>" + jsonResponse);
        fixturesResponse = null;
        if (brazilResponse != null)
            brazilResponse.Clear();
        fixturesResponse = JsonUtility.FromJson<FixturesResponse>(jsonResponse);


        for (int i=0; i< fixturesResponse.response.Length; i++)
        {
            if(fixturesResponse.response[i].league.country.Contains("Brazil"))
            {
                brazilResponse.Add(fixturesResponse.response[i]);
            }
        }

        //foreach (var fixture in fixtures.response)
        //{
        //    if (fixture.league.country == "Brazil")
        //    {
        //        Debug.Log($"Match--: {fixture.teams.home.name} vs {fixture.teams.away.name}");
        //        Debug.Log($"Date--: {fixture.fixture.date}");
        //        if (fixture != null)
        //            brazilFixtures.Add(fixture);
        //    }
        //}
    }

    [System.Serializable]
    public class FixturesResponse
    {
        public Response[] response;
    }

    [System.Serializable]
    public class Response
    {
        public League league;
        public Teams teams;
        public FixtureInfo fixture;
        public Goals goals;
    }

    [System.Serializable]
    public class Goals
    {
        public int home;
        public int away;     
    }

    [System.Serializable]
    public class League
    {
        public string country;
        public int id;
        public int season;        
    }

    [System.Serializable]
    public class Teams
    {
        public Team home;
        public Team away;
    }

    [System.Serializable]
    public class Team
    {
        public string name;
        public string logo;
        public string winner;

    }

    [System.Serializable]
    public class FixtureInfo
    {
        public string date;
        public int id;
    }

    internal IEnumerator GetRequest(object p)
    {
        throw new NotImplementedException();
    }


}


