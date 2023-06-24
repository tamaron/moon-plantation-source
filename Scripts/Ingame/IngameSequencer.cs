using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace U1W.MoonPlant
{
    /// <summary>
    /// Ingameのゲームフローを管理する
    /// </summary>
    public class IngameSequencer : MonoBehaviour
    {
        public int day = 0;
        public static int Score = 0;
        public static int AP_MAX = 3;
        public static int AP = 0;
        public readonly int LAST_DAY = 10;
        public bool isDay = true;
        [SerializeField] private AnimationManager _animationManager;
        [SerializeField] private EventDetector eventDetector;
        [SerializeField] private EffectDistributor _effectDistributor;
        [SerializeField] private PlantImage _plantImage;
        [SerializeField] private PlantManager _plantManager;
        [SerializeField] private Transform Camera;
        public static readonly float NIGHTVOL = 0.2f;

        private async void Start()
        {
            GameManager.Instance.RankingLoadedFrom = SceneName.Ingame;
            await DoInGame();

        }
        
        public async UniTask DoInGame()
        {
            _plantImage.SetImage().Forget();
            await Camera.DOLocalMoveY(25f, 0f);            
            await UniTask.Delay(3000);
            _animationManager.RealMoon.transform.DOLocalMoveY(10f, 5f).From(3.5f);
            await Camera.DOLocalMoveY(6.34f, 5f).From(25).SetEase(Ease.OutQuart);            
            day = 1;
            Score = 0;
            AP = AP_MAX;
            _plantManager.InitializePlants();
            _animationManager.SetNotFilledHarvestBoard();
            await UniTask.Delay(100);
            GameManager.Instance.BirdSESource.Play();
            await UniTask.Delay(250);
            GameManager.Instance.BirdSESource.Play();
            
            for (; day <= LAST_DAY; day++)
            {
                AP = AP_MAX;
                eventDetector.menuButtonInteractive = true;
                eventDetector.ButtonInteractive();

                _effectDistributor.Current = EffectState.None;
                eventDetector.CurrentActionType = ActionType.None;
                _animationManager.SetAPText();
                _animationManager.SetTopBoardText(day, Score);
                
                await _animationManager.DoDayStartAnim(day);
                _animationManager.UIShow().Forget();
                await UniTask.Delay(1000);

                while (true)
                {
                    if (AP <= 0) break;
                    eventDetector.menuButtonInteractive = true;
                    var end = await eventDetector.OnFreeEnd.First().ToUniTask();
                    eventDetector.menuButtonInteractive = false;
                    eventDetector.explPanel.DOFade(0f, 0.2f);
                    if (end) break;
                    _animationManager.RotateCamera(27, 0.6f).Forget();
                    _animationManager.BackButtonShow().Forget();

                    end = await eventDetector.OnActionDecidedEnd.First().ToUniTask();
                    _animationManager.SetAPText();
                    if(AP != 0) _animationManager.BackButtonHide().Forget();
                    else _animationManager.BackButtonHideOnly().Forget();
                    await _animationManager.RotateCamera(30, 0.6f);
                    if (end) break;
                    
                }
                
                _animationManager.UIHide().Forget();
                _plantManager.DayStep();

                await _animationManager.DoNightStartAnim(day);
                await _plantImage.SetImage();
                await UniTask.Delay(500);
                
                _animationManager.SetHarvestBoard(0, 0);
                _animationManager.RotateCamera(27, 1f).Forget();
                var text = _animationManager.message.GetComponentInChildren<Text>();
                text.text = "収穫する対象を選んでください";
                text.color = Color.white;
                await _animationManager.BackButtonShow();
                _effectDistributor.Current = EffectState.Line;
                eventDetector.CurrentActionType = ActionType.Cut;
                await eventDetector.OnActionDecidedEnd.First().ToUniTask();
                
                _animationManager.BackButtonHideOnly().Forget();
                await _animationManager.RotateCamera(30, 0.8f);
                text.text = "対象を選んでください";
                text.color = Color.black;
                await UniTask.Delay(1000);
            }
            
            await _animationManager.RotateCamera(20, 2f);
            
            GameManager.Instance.InGameBGMSource.DOFade(1f, 1.2f);
            GameManager.Instance.InGameNightBGMSource.DOFade(0f, 1.2f);

            naichilab.RankingLoader.Instance.SendScoreAndShowRanking(Score);
            DOTween.Clear();
        }
    }
}