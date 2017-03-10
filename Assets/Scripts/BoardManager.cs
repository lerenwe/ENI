﻿using UnityEngine;
using System.Collections;
using Assets.Scripts.Connexion;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Assets.Scripts.Utility;

public class BoardManager : MonoBehaviour {

    public static bool preventPlayerControl = false;

    GameObject[] Zones;
    ConnexionController connexionController;
    private Waiter _waiter = new Waiter();
    public const string baseURL = "http://vm-web7.ens-lyon.fr/eni"; //Prod
    //public const string baseURL = "http://127.0.0.1/eni"; //Local
    private const string getUserStats = baseURL + "/web/app_dev.php/unity/management/initJeu";
    public Camera mainCamera;
    public Camera LoadingCamera;
    public GameObject mainCanvas;

    public List<string> ActivitiesNames = new List<string>();

    JSONNode userStats;

    void Start ()
    {
        connexionController = GameObject.Find("ConnexionController").GetComponent<ConnexionController>();
        Zones = GameObject.FindGameObjectsWithTag("BoardZone");

        StartCoroutine(getUserStatsAtLogin(_waiter));
    }

    void Update ()
    {
        if (_waiter.waiting)
        {
            mainCamera.depth = -1;
            LoadingCamera.depth = 0;
            mainCanvas.SetActive(false);
        }
        else
        {
            mainCamera.depth = 0;
            LoadingCamera.depth = -1;
            mainCanvas.SetActive(true);
        }
    }

    void PopulateCharacterSkills ()
    {
        CharacterSheetManager charSheet = GameObject.Find("CharacterSheet").GetComponent<CharacterSheetManager>();

        charSheet.competencesList.Clear();
        //charSheet.correspondenceUserCompENIAndMiniGame.Clear();

        Dictionary<int, string> SkillTags = new Dictionary<int, string>();

        foreach (JSONNode value in userStats["listeCompetences"].Children)
        {
            if (!SkillTags.ContainsKey(value["idCompGen"].AsInt))
                SkillTags.Add(value["idCompGen"].AsInt, value["LibCompGen"].Value);
        }

        foreach (JSONNode value in userStats["listeCriteres"].Children)
        {
                charSheet.competencesList.Add(value["idCompEni"].AsInt, new CompetenceENI(SkillTags[value["idCompGen"].AsInt], value["idCompGen"].AsInt, value["point"].AsInt, value["idCritere"].AsInt, value["idJM"].AsInt)); //TODO : Replace RandomName by the real skill name
            //charSheet.correspondenceUserCompENIAndMiniGame.Add(value["idCompENI"].AsInt, value["idJM"].AsInt);
        }

        foreach (JSONNode value in userStats["listeJeux"].Children)
        {
            if (value["jeuNom"].Value == "mini-jeu 01")
                CharacterSheetManager.game1ID = value["idJeu"].AsInt;
            else if (value["jeuNom"].Value == "mini-jeu 02")
                CharacterSheetManager.game2ID = value["idJeu"].AsInt;
        }

        

        GameObject.Find("SkillSpider").GetComponent<Spider_Skill_Displayer>().InitSpider();
    }

    void PopulateMainBoard ()
    {
        foreach (GameObject zone in Zones)
        {
            BoardStep[] steps = zone.GetComponentsInChildren<BoardStep>();

            int gameListIterator = 0;
            foreach (JSONNode value in userStats["listeJeux"].Children)
            {
                if (zone.name == "Zone" + value["idZone"].Value)
                {

                    if (gameListIterator < steps.Length)
                    {
                        steps[gameListIterator].SceneToLoad = value["jeuNom"].Value;
                        steps[gameListIterator].stepContentText.text = value["slogan"].Value;

                        if (value["typeJM"].Value == "Jeux")
                            steps[gameListIterator].currentStepType = BoardStep.StepType.MiniGame;
                        else if (value["typeJM"].Value == "Mission")
                        {
                            steps[gameListIterator].currentStepType = BoardStep.StepType.Mission;
                            steps[gameListIterator].MissionId = value["idJeu"].AsInt;
                        }

                        if (steps[gameListIterator].SceneToLoad == "mini-jeu 01")
                            steps[gameListIterator].SceneToLoad = "IntroLabyrinthe";

                        if (steps[gameListIterator].SceneToLoad == "mini-jeu 02")
                            steps[gameListIterator].SceneToLoad = "Management";
                    }
                    else
                    {
                        Debug.LogError("There's more mini-games referenced in the SQL base than there is steps in " + zone.name + "!");
                        break;
                    }
                    Debug.Log(steps[gameListIterator].SceneToLoad);
                    gameListIterator++;
                }
            }
        }
    }

    public IEnumerator getUserStatsAtLogin(Waiter waiter)
    {
        Debug.Log("Attempting to retrieve the player's stats...");
        waiter.waiting = true;
        string sessionId = PlayerPrefs.GetString("sessionId");
        Dictionary<string, string> headers = new Dictionary<string, string> { { "Cookie", sessionId } };
        string post_url = getUserStats;


        WWW hs_get = new WWW(post_url, null, headers);
        yield return hs_get;
        if (hs_get.error != null)
        {
            Debug.Log("Error while retrieving all the player's stats : " + hs_get.error);
            Debug.Log(hs_get.text);
            SceneManager.LoadScene(0);
        }
        waiter.data = hs_get.text;
        userStats = JSON.Parse(waiter.data);

        foreach (JSONNode activityName in userStats["listeJeux"].Children)
        {
            ActivitiesNames.Add(activityName["jeuNom"].Value);
        }

        waiter.waiting = false;
        Debug.Log("Player stats retrieved successfully =)");

        PopulateMainBoard();
        PopulateCharacterSkills();
    }




}
