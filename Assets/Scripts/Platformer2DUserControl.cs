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
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            //Check horizontal speed based on inputs
            float hsp = Input.GetAxis("Horizontal");

            m_Jump = Input.GetButtonDown("Jump");
            // Pass all parameters to the character control script.
            m_Character.Move(hsp, m_Jump);
            m_Jump = false;
        }
    }
}
