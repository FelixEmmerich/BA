﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMechanism
{
    public class QuitGame : MonoBehaviour
    {
        public void Quit()
        {
            Debug.Log("Quitting");
            Application.Quit();
        }
    }

}