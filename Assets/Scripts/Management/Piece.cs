﻿using UnityEngine;

namespace Assets.Scripts.Management
{
    //Cette classe définit une pièce ainsi que ses propriétés
    public class Piece : MonoBehaviour
    {
        private GameManager gameManager;
        private SpriteRenderer imagePiece;

        public int id;
        public float surface = 55f;
        public int ouvertureExterieur = 4;
        public bool accesHandicape = false;
        public bool accesExterieur = true;
        public float distanceSallePause = 5f;
        public float distanceToilette = 4f;
        public Piece[] nextTo;
        public Personnage personnage;


        void Start()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            imagePiece = GetComponentInChildren<SpriteRenderer>();
        }
        void OnMouseDown()
        {
            gameManager.SelectedGameObject = this.gameObject;
        }
        void OnMouseOver()
        {
            if(!gameManager.isDragging)
                imagePiece.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);
        }

        void OnMouseExit()
        {
            if(!gameManager.isDragging)
                imagePiece.color = new Color(58, 52, 34, 0);
        }
    }
}
