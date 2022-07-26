using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace battler
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this);
            }
        }

        public static GameManager GetInstance()
        {
            return instance;
        }
    }
}