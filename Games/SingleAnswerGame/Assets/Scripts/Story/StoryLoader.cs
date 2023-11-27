using System;
using System.Collections;
using Global;
using Global.Localization;
using Global.Sound;
using JetBrains.Annotations;
using Story.Data;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Story
{
    public class StoryLoader : MonoBehaviour
    {
        [SerializeField] private GameObject variantPrefab;
        
        [Header("Parts")]
        [SerializeField] private Transform content;
        [SerializeField] private Image background;
        
        [Header("Images")]
        [SerializeField] private Image left;
        [SerializeField] private Image right;
        [SerializeField] private Image centerUnder;
        [SerializeField] private Image centerOver;
        
        [Header("Texts")]
        [SerializeField] private GameObject textObj;
        [SerializeField] private Text text;
        [SerializeField] private Font font;
        [SerializeField] private FontManager fontManager;

        private StoryController _controller;

        public IEnumerator FetchData()
        {
            using (UnityWebRequest request = UnityWebRequest.Get("https://firebasestorage.googleapis.com/v0/b/the-simple-interactive-story.appspot.com/o/royal-oak-commons.jpg?alt=media&token=e2a4af53-bbfa-48bf-9324-6501923b5f62"))
            {
                yield return request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    Debug.Log(request.downloadHandler.data);
                    byte[] bytes = request.downloadHandler.data;
                    Texture2D texture = new Texture2D(2, 2);
                    texture.LoadImage(bytes);
            
                    left.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), 
                        new Vector2(0.5f, 0.5f), 100.0f);
                }
            }
        }

        private void Start()
        {

            string storyId = LocalStorage.GetValue("storyId", "");
            string language = LocalStorage.GetValue("language", "en_US");
            if(storyId == "") return;
            
            StartCoroutine(Load(storyId, language));
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator Load(string storyId, string language)
        {
            _controller = new StoryController(storyId, language);

            Font frameFont = fontManager.GetFont(_controller.Config.font);
            if (frameFont != null) font = frameFont;
            text.GetComponent<Text>().font = font;
            
            LoadFrame(_controller.GetFrame(_controller.Config.mainFrame));
            yield return null;
        }
        private void LoadFrame(Frame frame)
        {
            if(frame == null) return;

            LoadMusic(frame);
            
            LoadText(frame.textKey);
            LoadAnswers(frame.answers);
            
            LoadImage(background, "Backgrounds",frame.background);
            
            LoadImage(left, "Images",frame.images?.left);
            LoadImage(right, "Images",frame.images?.right);
            LoadImage(centerUnder, "Images",frame.images?.centerUnder);
            LoadImage(centerOver, "Images",frame.images?.centerOver);
        }

        private void LoadMusic(Frame frame)
        {
            if(frame.music == null) return;
            
            MusicManager.ChangeAudioClip(_controller.GetClip(frame.music));
        }
        private void LoadText([CanBeNull] string key)
        {
            textObj.SetActive(true);

            if(key != null) text.GetComponent<Text>().text = _controller.GetWord(key);
            else textObj.SetActive(false);
        }
        
        private void LoadAnswers([CanBeNull] Answer[] answers)
        {
            foreach (Transform child in content) {
                Destroy(child.gameObject);
            }
            if(answers == null) return;

            foreach (Answer answer in answers)
            {
                GameObject obj = Instantiate(variantPrefab, content, false);
                
                answer.textKey = _controller.GetWord(answer.textKey);
                obj.GetComponent<Variant>().SetData(answer, AnswerClick, font);
            }
        }

        private void AnswerClick(string frameId, string action)
        {
            if(frameId == null) return;

            LoadFrame(_controller.GetFrame(frameId));
            
            if(action == null) return;
            Debug.Log("Action");

        }
        private void LoadImage(Image img, string folder, [CanBeNull] string filename)
        {
            if(filename == null) return;

            Sprite sprite = _controller.GetSprite($"{folder}/{filename}");

            if (sprite != null) img.sprite = sprite;
        }
    }
}