using UnityEngine;

namespace Global.Sound
{
    /// <summary>
    /// Manage saving same music on different scenes. Also manages volume changing and saving. 
    /// </summary>
    public class MusicManager : MonoBehaviour {
        [SerializeField]
        private AudioSource audioSource;
        public static MusicManager Instance { get; private set; }

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else if (Instance != this) {
                Destroy(gameObject);
            }

            LoadManager();
            DontDestroyOnLoad(gameObject);
        }
        private void LoadManager() 
        {
            Instance.Volume(LocalStorage.GetValue("music", 0.1f));
            if (!Instance.audioSource.isPlaying) {
                Instance.audioSource.Play();
            }
        }

        public static void Stop()
        {
            if (Instance != null)
            {
                Instance.audioSource.Stop();
            }
        }
        public static void Play()
        {
            if (Instance != null && !Instance.audioSource.isPlaying)
            {
                Instance.audioSource.Play();
            }
        }

        public void ChangeClip(AudioClip clip)
        {
            if(audioSource.isPlaying && audioSource.clip.name == clip.name) return;
            
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
        }

        public static void ChangeAudioClip(AudioClip clip)
        {
            if (Instance != null)
            {
                Instance.ChangeClip(clip);
            }
        }
        private void Start()
        {
            Play();
        }
        public void Volume(float value) {
            Instance.audioSource.volume = value;
        }
        public static void VolumeSound(float value) {
            if(Instance == null) return;
            
            Instance.Volume(value);
        }
    }
}