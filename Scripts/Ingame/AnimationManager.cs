using System;
using System.Net.Mime;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace U1W.MoonPlant
{
    /// <summary>
    /// UI、カメラ移動、BGMなどの演出を行う
    /// </summary>
    public class AnimationManager : MonoBehaviour
    {
        public AudioMixer audioMixer;
        [Header("Main UI")]
        [SerializeField] private RectTransform MenuBoard;
        [SerializeField] private RectTransform TopBoard;
        [SerializeField] private RectTransform SuggestBoard;

        private Vector2 menuVector2 = new Vector2(0, 0);
        private Vector2 topVector2 = new Vector2(0, 84);
        private Vector2 suggestVector2 = new Vector2(20, 0);
        private float uiShowTime = 0.45f;
        private float uiHideTime = 0.6f;


        [Header("DaySwitch")] 
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private RectTransform subjectRectTransform;
        [SerializeField] private RectTransform numberRectTransform;
        private Material skybox;

        
        [Header("BackPanel")]
        [SerializeField] private RectTransform backRectTransform;
        public RectTransform message;

        [Header("topboad text")]
        public Text TopBoardText;

        [Header("Harvest Board")] 
        public Text ColorText;
        public Text Moon;
        public Text Total;

        [Header("AP")] public Text AP;

        public Transform RealMoon;

        public Volume DayVolume;
        public Volume NightVolume;


        public void SetDayVolume()
        {
            DOTween.To(() => 0f,
                x =>
                {
                    DayVolume.weight = x;
                    NightVolume.weight = 1 - x;
                },
                1f,
                2f);
        }
        
        public void SetNightVolume()
        {
            DOTween.To(() => 0f,
                x =>
                {
                    DayVolume.weight = 1 - x;
                    NightVolume.weight = x;
                },
                1f,
                2f);
        }
        
        public void SetAPText()
        {
            AP.text = $"残り{IngameSequencer.AP}/{IngameSequencer.AP_MAX}";
        }
        

        public void SetHarvestBoard(int color, int moon)
        {
            ColorText.text = $"{color}";
            Moon.text = $"{moon}";
            Total.text = $"{moon} x {color}\n" +
                         $"$ {color * moon}";
        }
        public void SetNotFilledHarvestBoard()
        {
            ColorText.text = $"---";
            Moon.text = $"---";
            Total.text = $"---";
        }
        
        private void Start()
        {
            skybox = RenderSettings.skybox;
            skybox.SetFloat("_Exposure", 0.4f);
        }

        // Menuを隠して戻るをだす
        public async UniTask BackButtonShow()
        {
            message.DOAnchorPos(new Vector2(0, 210), 0.4f).SetEase(Ease.InQuad);
            MenuBoard.DOAnchorPos(new Vector2(300, 0), 0.2f).SetEase(Ease.InQuad);
            backRectTransform.DOAnchorPos(new Vector2(-50, 50), 0.4f).SetEase(Ease.OutBack);
        }

        public void SetTopBoardText(int day, int money)
        {
            TopBoardText.text = $"day {day}\n$ {money}";
        }
        
        public async UniTask BackButtonHide()
        {
            message.DOAnchorPos(new Vector2(0, 500), 0.4f).SetEase(Ease.InQuad);
            MenuBoard.DOAnchorPos(menuVector2, 0.2f).SetEase(Ease.InQuad);
            backRectTransform.DOAnchorPos(new Vector2(300, 0), uiShowTime).SetEase(Ease.OutBack);
        }
        
        public async UniTask BackButtonHideOnly()
        {
            message.DOAnchorPos(new Vector2(0, 500), 0.4f).SetEase(Ease.InQuad);
            backRectTransform.DOAnchorPos(new Vector2(300, 0), uiShowTime).SetEase(Ease.OutBack);
        }
        
        public async UniTask UIShow()
        {
            MenuBoard.DOAnchorPos(menuVector2, uiShowTime).SetEase(Ease.OutBack);
            TopBoard.DOAnchorPos(topVector2, uiShowTime).SetEase(Ease.OutBack);
            SuggestBoard.DOAnchorPos(suggestVector2, uiShowTime).SetEase(Ease.OutBack);
        }
        
        public async UniTask UIHide()
        {
            MenuBoard.DOAnchorPos(new Vector2(300, 0), uiHideTime).SetEase(Ease.InQuad);
            TopBoard.DOAnchorPos(new Vector2(0, 400), uiHideTime).SetEase(Ease.InQuad);
            SuggestBoard.DOAnchorPos(new Vector2(-200, 0), uiHideTime).SetEase(Ease.InQuad);
   
        }

        public async UniTask DoDayStartAnim(int day)
        {
            GameManager.Instance.InGameBGMSource.DOFade(1f, 0.5f);
            GameManager.Instance.InGameNightBGMSource.DOFade(0f, 0.5f);

            DOTween.To(() =>
                    0.4f,
                x => skybox.SetFloat("_Exposure", x),
                7f,
                2f
            );
            SetDayVolume();
            await UniTask.Delay(500);
            await RotateCamera(12, 1f);
            if (day == 1)
            {
                GameManager.Instance.InGameBGMSource.volume = 1f;
                GameManager.Instance.InGameNightBGMSource.volume = 0f;
                GameManager.Instance.InGameBGMSource.Play();
                GameManager.Instance.InGameNightBGMSource.Play();
            }
            await DoTextAnimation("Hello!", "Day " + day.ToString(), Color.white);
            await RotateCamera(30, 1f);

        }
        
        public async UniTask DoNightStartAnim(int day)
        {
            GameManager.Instance.InGameBGMSource.DOFade(0f, 0.5f);
            GameManager.Instance.InGameNightBGMSource.DOFade(1f, 0.5f);

            DOTween.To(() =>
                    7f,
                x => skybox.SetFloat("_Exposure", x),
                0.4f,
                2f
            );
            SetNightVolume();

            RealMoon.transform.DOLocalMoveY(3.5f, 1.5f).From(10f);        

            await UniTask.Delay(500);
            await RotateCamera(12, 1f);
            await DoTextAnimation("Welcome to", "Night " + day.ToString(), Color.white);
            RealMoon.transform.DOLocalMoveY(10f, 1.5f).From(3.5f).SetEase(Ease.InOutSine);
            await RotateCamera(30, 1f);

        }

        public async UniTask RotateCamera(float degree, float time)
        {
            await cameraTransform.DORotate(new Vector3(degree, -45, 0), time).SetEase(Ease.InOutSine);
        }

        private async UniTask DoTextAnimation(string subject, string message, Color color)
        {
            var subjectText = subjectRectTransform.GetComponent<Text>();
            var numberText = numberRectTransform.GetComponent<Text>();
            subjectText.text = subject;
            numberText.text = message;
            // subjectText.color = color;
            // numberText.color = color;

            subjectRectTransform.anchoredPosition = new Vector2(700, 150);
            numberRectTransform.anchoredPosition = new Vector2(-700, 50);

            subjectRectTransform.DOAnchorPos(new Vector2(0, 150), 0.5f);
            await numberRectTransform.DOAnchorPos(new Vector2(0, 50), 0.5f);

            await UniTask.Delay(1000);
            
            subjectRectTransform.DOAnchorPos(new Vector2(-700, 150), 0.5f);
            await numberRectTransform.DOAnchorPos(new Vector2(700, 50), 0.5f);
        }
    }
}