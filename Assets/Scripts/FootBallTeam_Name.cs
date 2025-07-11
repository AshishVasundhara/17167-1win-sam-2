using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FootBallTeam_Name : MonoBehaviour
{
    public static FootBallTeam_Name instance { get; set; }
    public GameObject FootBall_Level;
    public Transform Content;


    private void Awake()
    {
        instance = this;
    }

    [System.Obsolete]
    public void FootBall_Team()
    {
        var fixtures = ApiResponce.instacne.brazilResponse;

        for (int j = 0; j < Content.transform.childCount; j++)
        {
            Destroy(Content.transform.GetChild(j).gameObject);
        }


        for (int i = 0; i < fixtures.Count; i++)
        {
            GameObject game = Instantiate(FootBall_Level, Content.transform.position, Quaternion.identity, Content);
            TeamInDeta teamIn = game.GetComponent<TeamInDeta>();


            teamIn.Home_Text.text = fixtures[i].teams.home.name.ToString();
            teamIn.Away_Text.text = fixtures[i].teams.away.name.ToString();


            if (fixtures[i].teams.home.winner == "true" || fixtures[i].teams.home.winner == "false")
            {
                teamIn.Result.text = fixtures[i].goals.home + " : " + fixtures[i].goals.away;

                teamIn.ResultObject.SetActive(true);
                teamIn.Match_TimeOnject.SetActive(false);
            }
            else
            {
                teamIn.ResultObject.SetActive(false);
                teamIn.Match_TimeOnject.SetActive(true);

                string time = fixtures[i].fixture.date;
                var set_Time = time.Split('-', 'T', ':', '+');

                teamIn.Match_Time.text = int.Parse(set_Time[3]) + ":" + int.Parse(set_Time[4]).ToString("00");
            }
            //print("--Match Time----- " + int.Parse(set_Time[0]) + ":" + int.Parse(set_Time[1]) + ":" + int.Parse(set_Time[2]) + ":" +
            //    int.Parse(set_Time[3]) + ":" + int.Parse(set_Time[4]) + ":" + int.Parse(set_Time[5]) + ":" + int.Parse(set_Time[6]));



            teamIn.season = fixtures[i].league.season;
            teamIn.league = fixtures[i].league.id;
            teamIn.fixture_id = fixtures[i].fixture.id;

            // --->  set Logo Image --------
            Davinci.get().load(fixtures[i].teams.home.logo).into(teamIn.Home_Logo).start();
            Davinci.get().load(fixtures[i].teams.away.logo).into(teamIn.Away_Logo).start();

            //StartCoroutine(GetTexture(fixtures[i].teams.home.logo, teamIn.Home_Logo));
            //StartCoroutine(GetTexture(fixtures[i].teams.away.logo, teamIn.Away_Logo));

            //StartCoroutine(GetPredictions.instance.Get_predictions(GetPredictions.instance.URL + fixtures[i].fixture.id, teamIn));
            //print("----fixture.id-   " + GetPredictions.instance.URL + fixtures[i].fixture.id);

        }
    }
    int num;
    IEnumerator GetTexture(string url, Image yourImg)
    {
        //print(“GetTexture------------>>“+ IconImgUrl);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            print("error "+www.error);
        }
        else
        {
            num++;
            print("---" + num);
            Texture2D tex = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
            yourImg.sprite = sprite;
            
        }
    }
}
