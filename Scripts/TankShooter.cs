using UnityEngine;
using UnityEngine.UI;

    public class TankShooter : MonoBehaviour
    {
        public Rigidbody m_Shell;                   // Prefab of the shell.
        private Transform m_FireTransform;           // A child of the tank where the shells are spawned.
        public AudioClip m_FireClip;                // Audio that plays when each shot is fired.
        private AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.

        private string m_FireButton;                // The input axis that is used for launching shells.
        private float m_CurrentLaunchForce=20f;         // The force that will be given to the shell when the fire button is released.
        private float m_ChargeSpeed;                // How fast the launch force increases, based on the max charge time.
        private bool m_Fired;                       // Whether or not the shell has been launched with this button press.
        public bool fireRequest;

        void Awake() {
        m_ShootingAudio = new AudioSource();
        if (m_ShootingAudio != null)
            {
                
                m_ShootingAudio.clip = m_FireClip;
            }
        }

        private void OnEnable()
        {
            
            m_FireTransform = transform.FindChild("FireTransform");
        }


        private void Start()
        {

        }


        private void Update()
        {
            if (fireRequest && !m_Fired)
            {
                Fire();
            }
            if (!fireRequest) {
                m_Fired = false;
            }
        }


        private void Fire()
        {
            // Set the fired flag so only Fire is only called once.
            m_Fired = true;

            // Create an instance of the shell and store a reference to it's rigidbody.
            Rigidbody shellInstance =
                Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

            // Set the shell's velocity to the launch force in the fire position's forward direction.
            shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward; ;
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();
        }
    }