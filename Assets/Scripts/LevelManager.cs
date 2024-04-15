using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public string Creator;
    public string Name;
    public bool UseJSON = false;
    public JSONCreator Ruleset;
    public List<PlayerSpawnController> PSpawns;
    public List<ItemSpawnController> ISpawns;
    public List<WeaponSpawnController> WSpawns;
    public List<NPCSpawner> NPCSpawns;
    public PlayerSpawnController LastPS;
    public List<string> Announces;
    public Dictionary<string, JSONActor> Actors = new Dictionary<string, JSONActor>();
    public Dictionary<string, JSONItem> Items = new Dictionary<string, JSONItem>();
    public Dictionary<string, JSONWeapon> Weapons = new Dictionary<string, JSONWeapon>();
    public List<FirstPersonController> AlivePlayers = new List<FirstPersonController>();
    public List<GameObject> Spawned;
    public bool RoundComplete;
    public Dictionary<IColors,List<FirstPersonController>> Teams = new Dictionary<IColors, List<FirstPersonController>>();
    public int CurrentWave = 0;
    public int Points = 0;
    public Image Overlay;
    public TextMeshProUGUI CenterText;
    bool Won = false;
    public static bool MidCutscene = false;
    public Image Crosshair;
    public Sprite CrosshairPlus;
    Sprite CrosshairBasic;

    void Awake()
    {
        God.LM = this;
        MidCutscene = false;
        CrosshairBasic = Crosshair.sprite;
    }

    void Start()
    {
        if (!UseJSON) return;
        string cr = Creator != "" ? Creator : "Misha";
        if (!God.LS.Rulesets.ContainsKey(cr)) cr = "Misha";
        Ruleset = God.LS.Rulesets[cr];
        if (Ruleset.Gravity > 0)
        {
            Physics.gravity = new Vector3(0,-9.81f,0) * Ruleset.Gravity;
        }
        //Debug.Log("WEAPONS: " + Ruleset.Weapons.Count + " / " + cr);
        foreach(JSONActor i in Ruleset.Actors)
            Actors.Add(i.Name,i);
        foreach(JSONItem i in Ruleset.Items)
            Items.Add(i.Text,i);
        foreach(JSONWeapon i in Ruleset.Weapons)
            Weapons.Add(i.Text,i);
        foreach (FirstPersonController pc in God.Players)
        {
            //pc.ImprintRules(Ruleset);
            AlivePlayers.Add(pc);
        }
            
    }

    void Update()
    {
        string txt = "";
        foreach(string a in Announces)
        {
            if (txt != "") txt += "\n";
            txt += a;
        }
        God.UpdateText.text = txt;

        if (Won) return;
        if (Ruleset.Waves > 0 && CurrentWave != -1)
        {
            bool any = false;
            foreach (NPCSpawner sp in NPCSpawns)
            {
                if (sp.Children.Count > 0)
                {
                    any = true;
                    break;
                }
            }
            if (!any)
            {
                CurrentWave++;
                if (CurrentWave >= Ruleset.Waves)
                {
                    StartCoroutine(YouWin());
                    // MakeAnnounce("YOU WIN");
                    CurrentWave = -1;
                }
                else
                {
                    foreach (NPCSpawner sp in NPCSpawns)
                    {
                        sp.Spawn();
                    }
                }
            }
        }
    }

    public IEnumerator YouWin(bool skipStart=false)
    {
        if (Won) yield break;
        Won = true;
        Overlay.gameObject.SetActive(true);
        Overlay.color = new Color(0,0,0,0);
        CenterText.text = "";
        float c = 0;
        if (skipStart) c = 1;
        while (c < 1)
        {
            c = Mathf.Lerp(c, 1.01f, Time.unscaledDeltaTime);
            c = Mathf.MoveTowards(c, 1.01f, Time.unscaledDeltaTime / 2);
            Overlay.color = new Color(0,0,0,c);
            yield return null;
        }
        Overlay.color = new Color(0,0,0,1);
        yield return StartCoroutine(TextReveal("YOU WIN",0.15f));
        // CenterText.text = "YOU WIN";
        while (!Input.GetKeyDown(KeyCode.Space) && !Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
        EndCutscene();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //MakeAnnounce("YOU WIN");
    }

    public IEnumerator Cutscene(string text, int points)
    {
        yield return StartCoroutine(Cutscene(new List<string>() { text }, points));
    }
    
    public IEnumerator Cutscene(List<string> text, int points)
    {
        if (MidCutscene) yield break;
        StartCutscene();
        Overlay.gameObject.SetActive(true);
        Overlay.color = new Color(0,0,0,0);
        CenterText.text = "";
        float c = 0;
        while (c < 1)
        {
            c = Mathf.Lerp(c, 1.01f, Time.unscaledDeltaTime);
            c = Mathf.MoveTowards(c, 1.01f, Time.unscaledDeltaTime);
            Overlay.color = new Color(0,0,0,c);
            yield return null;
        }
        Overlay.color = new Color(0,0,0,1);
        // if (bg != null)
        // {
        //     Overlay.color = new Color(1,1,1);
        //     Overlay.sprite = bg;
        // }
        foreach(string txt in text)
        {
            yield return StartCoroutine(TextReveal(txt,Ruleset.TextSpeed));
            // CenterText.text = txt;
            while (!Input.GetKeyDown(KeyCode.Space) && !Input.GetMouseButtonDown(0))
            {
                yield return null;
            }

            yield return null;
        }
        CenterText.text = "";
        if(points != 0)
            GetPoint(points,false);
        if (Points >= Ruleset.PointsToWin)
        {
            
            yield return StartCoroutine(YouWin(true));
            yield break;
        }
        Overlay.color = new Color(0,0,0,1);
        c = 1;
        while (c > 0)
        {
            c = Mathf.Lerp(c, -0.01f, Time.unscaledDeltaTime);
            c = Mathf.MoveTowards(c, -0.01f, Time.unscaledDeltaTime);
            Overlay.color = new Color(0,0,0,c);
            yield return null;
        }

        EndCutscene();
    }

    public IEnumerator TextReveal(string txt,float speed=0.05f)
    {
        float time = 0;
        for (int n = 0; n <= txt.Length; n++)
        {
            if (n < txt.Length && txt.Substring(n, 1) == "<")
            {
                while (txt.Substring(n, 1) != ">" && n <= txt.Length)
                    n++;
                n++;
            }
            CenterText.text = txt.Substring(0, n);
            time = speed;
            while (time > 0)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    speed = 0;
                    time = 0;
                }
                time -= Time.unscaledDeltaTime;
                yield return null;
            }
        }
    }

    public PlayerSpawnController GetPSpawn(FirstPersonController pc)
    {
        if (PSpawns.Count == 0) return null;
        PlayerSpawnController r = PSpawns[Random.Range(0, PSpawns.Count)];
        if (LastPS != null) PSpawns.Add(LastPS);
        if(PSpawns.Count > 1) PSpawns.Remove(r);
        LastPS = r;
        return r;
    }

    public void AwardPoint(FirstPersonController who, int amt = 1, string targ="")
    {
//         if (who.Team != IColors.None)
//         {
//             IColors team = who.Team;
//             if (!God.RM.TeamScores.ContainsKey(team)) God.RM.TeamScores.Add(team, amt);
//             else God.RM.TeamScores[team] += amt;
//             if(God.RM.TeamScores[team] >= Ruleset.PointsToWin) SetWinner(team);
//             string teamtxt = who.Name.Value.ToString()  + " <"+team.ToString()+"> ";
//             if (targ != "") teamtxt += " > " + targ;
//             teamtxt += " ("+God.RM.TeamScores[team]+")";
//             MakeAnnounce(teamtxt);
// //            StartCoroutine(Announce(teamtxt));
//             return;
//         }
//         if (!God.RM.Scores.ContainsKey(who)) God.RM.Scores.Add(who, amt);
//         else God.RM.Scores[who] += amt;
//         if(God.RM.Scores[who] >= Ruleset.PointsToWin) SetWinner(who);
//         string txt = who.Name.Value.ToString();
//         if (targ != "") txt += " > " + targ;
//         txt += " ("+God.RM.Scores[who]+")";
//         MakeAnnounce(txt);
    }

    public void MakeAnnounce(string txt, bool big = false)
    {
        if (big)
        {
            God.LS.StartCoroutine(Winner(txt));
        }
        else
            StartCoroutine(Announce(txt));

        God.RM?.AlertClientRPC(txt, big);
    }
    
    public IEnumerator Announce(string txt)
    {
        Announces.Add(txt);
        yield return new WaitForSeconds(3);
        Announces.Remove(txt);
    }

    public void SetWinner(FirstPersonController who)
    {
//        Debug.Log(who.Name.Value + " Wins!");
//        God.LS.StartCoroutine(Winner(who.Name.Value.ToString()));
        // MakeAnnounce(who.Name.Value.ToString() + " WINS!", true);
        RoundComplete = true;
    }
    
    public void SetWinner(IColors team)
    {
//        Debug.Log(who.Name.Value + " Wins!");
//        God.LS.StartCoroutine(Winner(team.ToString()));
        MakeAnnounce(team.ToString() + " WINS!", true);
        RoundComplete = true;
    }
    
    public IEnumerator Winner(string who)
    {
        God.AnnounceText.text = who;
        if (NetworkManager.Singleton.IsServer)
            God.LS.PickNextLevel();
        yield return new WaitForSeconds(3);
        God.RM.Scores.Clear();
        God.AnnounceText.text = "";
        God.LS.StartLevel();
    }

    public JSONItem GetItem(string n)
    {
        if (Items.ContainsKey(n)) return Items[n];
        if (n == "" && Ruleset.Items.Count > 0) return Ruleset.Items[Random.Range(0, Ruleset.Items.Count)];
        JSONTempItem r = new JSONTempItem();
        r.Text = "Useless Item";
        return new JSONItem(r);
    }

    public JSONActor GetActor(string n)
    {
        if (Actors.ContainsKey(n)) return Actors[n];
        Debug.Log("ACTOR NOT FOUND: " + n + " / " + Ruleset.Actors.Count + " / " + Actors.Keys.Count);
        if (Ruleset.Actors.Count > 0) return Ruleset.Actors[0];
        JSONTempActor a = new JSONTempActor();
        a.HP = 100;
        a.MoveSpeed = 10;
        return new JSONActor(a);
    }
    
    public JSONWeapon GetWeapon(string n)
    {
        if (Weapons.ContainsKey(n)) return Weapons[n];
        return null;
        // if (Ruleset.Weapons.Count > 0) return Ruleset.Weapons[0];
        // JSONTempWeapon wpn = new JSONTempWeapon();
        // wpn.Damage = 10;
        // wpn.Text = "GENERIC WEAPON";
        // return new JSONWeapon(wpn);
    }

    public bool Respawn(FirstPersonController pc)
    {
        if (Ruleset.Mode == GameModes.Elim) return false;
        return true;
    }

    public void NoticeDeath(FirstPersonController pc,FirstPersonController source=null)
    {
        if(source != null && Ruleset.Mode == GameModes.Deathmatch)AwardPoint(source,1);
        AlivePlayers.Remove(pc);
        if (Ruleset.Mode == GameModes.Elim)
        {
            if (AlivePlayers.Count == 1)
            {
                FirstPersonController winner = AlivePlayers[0];
                AwardPoint(winner);
                if (RoundComplete) return;
            }
            if (AlivePlayers.Count <= 1)
            {
                AlivePlayers.Clear();
                foreach (FirstPersonController dead in God.Players)
                {
                    dead.Reset();
                    AlivePlayers.Add(dead);
                }
            }
        }
    }

    // public IColors PickTeam(FirstPersonController pc)
    // {
    //     if (Ruleset.Teams.Count <= 1) return IColors.None;
    //     int amt = 999;
    //     IColors best = IColors.None;
    //     foreach (IColors c in Ruleset.Teams)
    //     {
    //         if(!Teams.ContainsKey(c)) Teams.Add(c,new List<FirstPersonController>());
    //         if (Teams[c].Contains(pc)) return c;
    //         int mem = Teams[c].Count;
    //         if (mem < amt)
    //         {
    //             best = c;
    //             amt = mem;
    //         }
    //     }
    //
    //     if (Teams.ContainsKey(best))
    //         Teams[best].Add(pc);
    //     return best;
    // }
    public void StartCutscene()
    {
        MidCutscene = true;
        Time.timeScale = 0;
    }

    public void EndCutscene()
    {
        MidCutscene = false;
        Time.timeScale = 1;
    }

    public void RemovePlayer(FirstPersonController pc)
    {
        AlivePlayers.Remove(pc);
        foreach (IColors c in Teams.Keys)
            Teams[c].Remove(pc);
    }

    public void GetPoint(int amt,bool checkWin=true)
    {
        Points += amt;
        if (Points >= Ruleset.PointsToWin && !Won && checkWin)
        {
            StartCoroutine(YouWin());
            // MakeAnnounce("YOU WIN");
        }
    }

    public void SetCrosshair(bool fancy)
    {
        Crosshair.sprite = fancy ? CrosshairPlus : CrosshairBasic;
    }
}
