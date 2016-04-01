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


        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            //Check horizontal speed based on inputs
            bool left = CrossPlatformInputManager.GetButton("Left");
            bool right = CrossPlatformInputManager.GetButton("Right");
            //hsp is the horizontal speed given as 1 or -1 depending on the buttons pressed
            float hsp = (left ? -1 : 0) + (right ? 1 : 0);
            // Pass all parameters to the character control script.
            m_Character.Move(hsp, m_Jump);
            m_Jump = false;
        }
    }
}
