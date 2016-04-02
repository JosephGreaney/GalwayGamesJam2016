using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UnityStandardAssets._2D
{
    public class Entity : MonoBehaviour
    {

        private static int ID;

        private int id = ID++;
        public int health;
        public int damage;


        private EntityType type;
        private enum EntityType
        {
            PLAYER, WIZARD, DINOSAUR, CAVEMAN, ROBOT, ALIEN
        };
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        protected void Attack()
        {
            print(type + " attacked\n");
        }
    }
}
