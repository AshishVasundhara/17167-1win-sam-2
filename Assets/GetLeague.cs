using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetLeague : MonoBehaviour
{
    public static GetLeague instance { get; set; }
    public string URL = GameManger.BaseURL + "standings?season=2024&league=71";
    public GetData getDats;
    public GameObject LeaguePositonPrefab;
    public GameObject NoDataFoundPrefab;
    public Transform LeaguePositonPrefabParent;

    private void Awake()
    {
        instance = this;
    }

    public void Get_LeagueApiCall(int LeagueID, int Season, string HomeName, string AwayName)
    {
        StartCoroutine(Get_League(LeagueID, Season, HomeName, AwayName));

        for (int i = 0; i < LeaguePositonPrefabParent.childCount; i++)
        {
            Destroy(LeaguePositonPrefabParent.GetChild(i).gameObject);
        }
    }

    public IEnumerator Get_League(int LeagueID, int Season, string HomeName, string AwayName)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(GameManger.BaseURL + "standings?season=" + Season + "&league=" + LeagueID))
        {
            webRequest.SetRequestHeader("X-RapidAPI-Key", GameManger.RapidAPI_Key);
            webRequest.SetRequestHeader("X-RapidAPI-Host", GameManger.RapidAPI_Host);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;                

                if (jsonResponse.Contains("[["))
                {
                    jsonResponse = jsonResponse.Replace("[[", "[");
                }
                if (jsonResponse.Contains("]]"))
                {
                    jsonResponse = jsonResponse.Replace("]]", "]");
                }

                if (jsonResponse.Contains("],["))
                {
                    jsonResponse = jsonResponse.Replace("],[", ",");
                }
                getDats = JsonUtility.FromJson<GetData>(jsonResponse);
                if (getDats.response.Count > 0)
                {
                    //int rank = getDats.response[0].league.standings[0][0].rank;                   

                    for (int i = 0; i < getDats.response[0].league.standings.Count; i++)
                    {
                        GameObject game = Instantiate(LeaguePositonPrefab);
                        game.transform.SetParent(LeaguePositonPrefabParent);
                        game.transform.localScale = Vector3.one;
                        game.GetComponent<RectTransform>().localPosition = Vector3.zero;
                        game.GetComponent<LeagueInValue>().SetupDataFromApiReceived(getDats.response[0].league.standings[i].rank,
                           getDats.response[0].league.standings[i].team.logo,
                           getDats.response[0].league.standings[i].team.name,
                           getDats.response[0].league.standings[i].all.played,
                           getDats.response[0].league.standings[i].all.win,
                           getDats.response[0].league.standings[i].all.draw,
                           getDats.response[0].league.standings[i].all.lose,
                           getDats.response[0].league.standings[i].points);


                        if (HomeName.ToLower().Trim() == getDats.response[0].league.standings[i].team.name.ToLower().Trim())
                        {
                            if (getDats.response[0].league.standings[i].rank % 10 == 1)
                            {
                                GameManger.instance.HomeTeamRank.text = getDats.response[0].league.standings[i].rank + " st";
                            }
                            else if (getDats.response[0].league.standings[i].rank % 10 == 2)
                            {
                                GameManger.instance.HomeTeamRank.text = getDats.response[0].league.standings[i].rank + " nd";
                            }
                            else if (getDats.response[0].league.standings[i].rank % 10 == 3)
                            {
                                GameManger.instance.HomeTeamRank.text = getDats.response[0].league.standings[i].rank + " rd";
                            }
                            else
                            {
                                GameManger.instance.HomeTeamRank.text = getDats.response[0].league.standings[i].rank + " th";
                            }
                        }
                        else if (AwayName.ToLower().Trim() == getDats.response[0].league.standings[i].team.name.ToLower().Trim())
                        {
                            if (getDats.response[0].league.standings[i].rank % 10 == 1)
                            {
                                GameManger.instance.awayTeamRank.text = getDats.response[0].league.standings[i].rank + " st";
                            }
                            else if (getDats.response[0].league.standings[i].rank % 10 == 2)
                            {
                                GameManger.instance.awayTeamRank.text = getDats.response[0].league.standings[i].rank + " nd";
                            }
                            else if (getDats.response[0].league.standings[i].rank % 10 == 3)
                            {
                                GameManger.instance.awayTeamRank.text = getDats.response[0].league.standings[i].rank + " rd";
                            }
                            else
                            {
                                GameManger.instance.awayTeamRank.text = getDats.response[0].league.standings[i].rank + " th";
                            }
                        }
                    }
                }
                else
                {
                    GameObject game = Instantiate(NoDataFoundPrefab);
                    game.transform.SetParent(LeaguePositonPrefabParent);
                    game.transform.localScale = Vector3.one;
                    game.GetComponent<RectTransform>().localPosition = Vector3.zero;

                    GameManger.instance.HomeTeamRank.text = "00";
                    GameManger.instance.awayTeamRank.text = "00";
                }
            }
        }
    }

    [System.Serializable]
    public class GetData
    {
        public string get;
        public List<Response> response;
        public int results;
    }

    [System.Serializable]
    public class Response
    {
        public League league;
    }

    [System.Serializable]
    public class League
    {
        public int id;
        public string name;
        public string country;
        public string logo;
        public string flag;
        public int season;
        public List<Standing> standings;
        //public List<List<StandingObj>> standings; 
        //public Root root;
    }

    [System.Serializable]
    public class Standing
    {
        public int rank;
        public Team team;
        public int points;
        public AllObj all;
    }

    [System.Serializable]
    public class Team
    {
        public int id;
        public string name;
        public string logo;

    }

    [System.Serializable]
    public class AllObj
    {
        public int played;
        public int win;
        public int draw;
        public int lose;
    }

    [System.Serializable]
    public class Paging
    {
        public int current;
        public int total;
    }

    [System.Serializable]
    public class Parameters
    {
        public string season;
        public string league;
    }

    [System.Serializable]
    public class Root
    {
        //public string get;
        public Parameters parameters;
        //public List<object> errors;
        public int results;
        //public Paging paging;
        //public Response[] response;
    }



}
