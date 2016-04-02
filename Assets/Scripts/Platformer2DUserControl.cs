using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
        private int warpDest = -1;  // warpDest is -1 until pressed


        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            // Check horizontal speed based on inputs
            float hsp = Input.GetAxis("Horizontal");

            m_Jump = Input.GetButtonDown("Jump");

            // Check if any warp buttons are pressed
            if (Input.GetButtonDown("Past"))
                warpDest = 0;
            else if (Input.GetButtonDown("Medieval"))
                warpDest = 1;
            else if (Input.GetButtonDown("Future"))
                warpDest = 2;

            // Pass all parameters to the character control script.
            m_Character.Move(hsp, m_Jump, warpDest);
            warpDest = -1;
            m_Jump = false;
        }
    }
}
