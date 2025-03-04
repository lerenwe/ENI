﻿using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Management
{
    //Cette classe définit une pièce ainsi que ses propriétés
    public class Room : MonoBehaviour
    {

        #region External Components & objects
        private GameManager gameManager;
        public ManagementCharacter managementCharacter; //The management character associated with this room, if any.
        private SpriteRenderer imagePiece;
        #endregion

        #region Room stats
        public int id;
        public float surface = 55f;
        public int ouvertureExterieur = 4;
        public bool accesHandicape = false;
        public bool accesExterieur = true;
        public float distanceSallePause = 5f;
        public float distanceToilette = 4f;
        public List<int> roomDistancesid = new List<int>();
        #endregion


        void Start()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            imagePiece = GetComponentInChildren<SpriteRenderer>();
            gameManager.roomDescriptionPanel.SetActive(false);
        }

        void OnMouseOver()
        {
            if (!gameManager.draggingAnyCharacter)
            {
                imagePiece.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);
                gameManager.roomDescriptionPanel.SetActive(true);
                gameManager.highlightedRoom = this.gameObject;
                gameManager.UpdateRoomDescription();
            }
        }

        void OnMouseExit()
        {
            if (!gameManager.draggingAnyCharacter)
            {
                imagePiece.color = new Color(58, 52, 34, 0);
                gameManager.roomDescriptionPanel.SetActive(false);
            }
        }
    }
}
