﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Management
{
    //Cette classe définit un personnage, et définit ses indices de satisfaction
    public class Personnage : MonoBehaviour
    {
        public GameManager gameManager;
        public CharacterGrid saveChildren;

        public int persoId;
        public int serviceId;
        public string role = "[Service Name]";
        public float surfaceSalarie = 5;
        public int luminosite = 3;
        public bool accesExterieur = false;
        public int distanceSallePause = 4;
        public float distanceToilette = 2f;

        public SpriteRenderer sr;
        public SatisfactionPersonnage Satisfaction { get; private set; }
        public Personnage copain;
        public Personnage bienAimePar;
        public Personnage myProductiveLink;
        public Personnage charIMakeProductive;
        public Piece piece;

        void Start()
        {
            if (!gameManager)
                gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

            sr = GetComponent<SpriteRenderer>();
            Satisfaction = GetComponent<SatisfactionPersonnage>();
        }

        //Permet de selectionner le personnage
        void OnMouseDown()
        {
            gameManager.SelectedGameObject = this.gameObject;
        }

        //Sets a sprite as this character's avatar
        public void setAvatar(int persoId)
        {
            sr = gameObject.GetComponent<SpriteRenderer>();
            saveChildren = GameObject.Find("GrillePersonnages").GetComponent<CharacterGrid>();
            sr.sprite = saveChildren.avatarsSprites[persoId - 1];
        }

        //Calcule la satisfaction du personnage
        public void CalculSatisfaction()
        {
            StartCoroutine(Satisfaction.CalculSatisfaction());
        }

        //Réinitialise la satisfaction du personnage
        public void ResetSatisfaction()
        {
            this.Satisfaction.Reset();
        }
    }
}
