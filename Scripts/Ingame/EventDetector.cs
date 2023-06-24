using System;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace U1W.MoonPlant
{
    /// <summary>
    /// UIが発火したときの処理を登録する
    /// </summary>
    public class EventDetector : MonoBehaviour
    {
        public bool menuButtonInteractive = false;
        public EffectDistributor EffectDistributor;
        public Subject<bool> OnFreeEnd = new Subject<bool>();

        public PlantImage PlantImage;
        public Subject<bool> OnActionDecidedEnd = new Subject<bool>();

        public ActionType CurrentActionType = ActionType.None;
        [SerializeField] private TouchDetector _touchDetector;
        public Button BlueSheet;
        public Button Zyouro;
        public Button ColorMed;
        public Button Random;
        public Button TurnEnd;
        public Button MoonMed;
        public Button Back;

        public CanvasGroup explPanel;
        public Text expl;
        public Text expl2;
        public void ButtonInteractive()
        {
            BlueSheet.interactable = true;
            Zyouro.interactable = true;
            ColorMed.interactable = true;
            Random.interactable = true;
            MoonMed.interactable = true;
        }

        public AnimationManager AnimationManager;
        [SerializeField] private PlantManager _plantManager;
        private void Start()
        {
            var buttons = new List<Button>
            {
                BlueSheet,
                Zyouro,
                ColorMed,
                Random,
                MoonMed,
                Back,
                TurnEnd
            };
            foreach (var button in buttons)
            {
                
                button.OnPointerEnterAsObservable().Subscribe(_ =>
                {
                    button.transform.DOScale(1.2f, 0.1f).From(1.0f);
                }).AddTo(this);
                button.OnPointerExitAsObservable().Subscribe(_ =>
                {
                    button.transform.DOScale(1.0f, 0.1f).From(1.2f);
                    explPanel.DOFade(0f, 0.2f);
                }).AddTo(this);
                
                
            }
            
            BlueSheet.OnPointerEnterAsObservable().Where(_ => menuButtonInteractive).Subscribe(_ =>
            {
                explPanel.DOFade(1f, 0.2f).From(0f);
                expl.text = $"花のかたちの変化を抑える。\n" +
                            $"月光促進薬と併用すると効果が相殺される。\n";
                expl2.text = $"(花が咲いていないと効果はない)";

            }).AddTo(this);
            
            Zyouro.OnPointerEnterAsObservable().Where(_ => menuButtonInteractive).Subscribe(_ =>
            {
                explPanel.DOFade(1f, 0.2f).From(0f);

                expl.text = $"更地を水をやると芽が出る。\n" +
                            $"芽に水をやると花が咲く。\n";
                expl2.text = $"(すでに咲いている花には効果はない)";


            }).AddTo(this);
            
            ColorMed.OnPointerEnterAsObservable().Where(_ => menuButtonInteractive).Subscribe(_ =>
            {

                explPanel.DOFade(1f, 0.2f).From(0f);
                expl.text = $"花の色を変化させる。\n" +
                            $"白 → 緑 → 紫の順に変化する。\n";
                expl2.text = $"(花が咲いていないと効果はない)";

            }).AddTo(this);
            
            Random.OnPointerEnterAsObservable().Where(_ => menuButtonInteractive).Subscribe(_ =>
            {
                explPanel.DOFade(1f, 0.2f).From(0f);
                expl.text = $"TIPS\n" +
                            $"突然変異を起こす薬。\n" +
                            $"突然花が咲いたり枯れたりする。\n";
                expl2.text = $"(花が咲いていないと効果はない)";

            }).AddTo(this);
            
            MoonMed.OnPointerEnterAsObservable().Where(_ => menuButtonInteractive).Subscribe(_ =>
            {
                explPanel.DOFade(1f, 0.2f).From(0f);
                expl.text = $"花のかたちの変化を1段階早く変化させる。\n"+
                            $"月光抑制薬と併用すると効果が相殺される。\n";
                expl2.text = $"(花が咲いていないと効果はない)";


            }).AddTo(this);
            
            
            BlueSheet.onClick.AsObservable().Subscribe(_ =>
            {
                EffectDistributor.Current = EffectState.Line;
                CurrentActionType = ActionType.BlueSheet;
                GameManager.Instance.ButtonSelectSource.Play();
                OnFreeEnd.OnNext(false);
            }).AddTo(this);
            
            Zyouro.onClick.AsObservable().Subscribe(_ =>
            {
                EffectDistributor.Current = EffectState.Line;
                CurrentActionType = ActionType.Zyouro;
                GameManager.Instance.ButtonSelectSource.Play();

                OnFreeEnd.OnNext(false);

            }).AddTo(this);
            
            ColorMed.onClick.AsObservable().Subscribe(_ =>
            {
                EffectDistributor.Current = EffectState.Line;
                CurrentActionType = ActionType.ColorMed;
                GameManager.Instance.ButtonSelectSource.Play();

                OnFreeEnd.OnNext(false);

            }).AddTo(this);
            
            Random.onClick.AsObservable().Subscribe(_ =>
            {
                EffectDistributor.Current = EffectState.Line;
                CurrentActionType = ActionType.Random;
                GameManager.Instance.ButtonSelectSource.Play();

                
                OnFreeEnd.OnNext(false);

            }).AddTo(this);
            
            TurnEnd.onClick.AsObservable().Subscribe(_ =>
            {
                EffectDistributor.Current = EffectState.None;
                CurrentActionType = ActionType.None;
                GameManager.Instance.PlantSelectSource.Play();

                OnFreeEnd.OnNext(true);

            }).AddTo(this);
            
            MoonMed.onClick.AsObservable().Subscribe(_ =>
            {
                EffectDistributor.Current = EffectState.Line;
                CurrentActionType = ActionType.MoonMed;
                GameManager.Instance.ButtonSelectSource.Play();

                OnFreeEnd.OnNext(false);
            }).AddTo(this);
            
            Back.onClick.AsObservable()
                .Where(_ => CurrentActionType != ActionType.None)
                .Subscribe(_ =>
                {
                    if (CurrentActionType == ActionType.Cut)
                    {
                        AnimationManager.SetNotFilledHarvestBoard();
                    }
                    CurrentActionType = ActionType.None;
                    EffectDistributor.Current = EffectState.None;
                    GameManager.Instance.CancellSource.Play();
                    menuButtonInteractive = true;
                    OnActionDecidedEnd.OnNext(false);
                }).AddTo(this);
            
            _touchDetector.PushSubjectSingle
                .Where(val => CurrentActionType == ActionType.Random)
                .Subscribe(point =>
                {
                    GameManager.Instance.PlantSelectSource.Play();
                    IngameSequencer.AP--;
                    _plantManager.Randomize(point);
                    CurrentActionType = ActionType.None;
                    EffectDistributor.Current = EffectState.None;
                    Random.interactable = false;
                    OnActionDecidedEnd.OnNext(false);

                }).AddTo(this);
            
            _touchDetector.PushSubjectSingle
                .Where(val => CurrentActionType == ActionType.Zyouro)
                .Subscribe(point =>
                {
                    GameManager.Instance.PlantSelectSource.Play();
                    menuButtonInteractive = true;

                    IngameSequencer.AP--;
                    _plantManager.Zyouro(point);
                    CurrentActionType = ActionType.None;
                    EffectDistributor.Current = EffectState.None;
                    Zyouro.interactable = false;

                    OnActionDecidedEnd.OnNext(false);

                }).AddTo(this);
            
            _touchDetector.PushSubjectSingle
                .Where(val => CurrentActionType == ActionType.BlueSheet)
                .Subscribe(point =>
                {
                    GameManager.Instance.PlantSelectSource.Play();
                    menuButtonInteractive = true;

                    IngameSequencer.AP--;
                    _plantManager.TakeBlueSheet(point);
                    CurrentActionType = ActionType.None;
                    EffectDistributor.Current = EffectState.None;
                    BlueSheet.interactable = false;
                    OnActionDecidedEnd.OnNext(false);

                }).AddTo(this);
            
            _touchDetector.PushSubjectSingle
                .Where(val => CurrentActionType == ActionType.MoonMed)
                .Subscribe(point =>
                {
                    GameManager.Instance.PlantSelectSource.Play();
                    menuButtonInteractive = true;

                    IngameSequencer.AP--;
                    _plantManager.MoonMed(point);
                    CurrentActionType = ActionType.None;
                    EffectDistributor.Current = EffectState.None;
                    MoonMed.interactable = false;

                    OnActionDecidedEnd.OnNext(false);

                }).AddTo(this);
            
            _touchDetector.PushSubjectSingle
                .Where(val => CurrentActionType == ActionType.ColorMed)
                .Subscribe(point =>
                {
                    GameManager.Instance.PlantSelectSource.Play();
                    menuButtonInteractive = true;

                    IngameSequencer.AP--;
                    _plantManager.ColorMed(point);
                    CurrentActionType = ActionType.None;
                    EffectDistributor.Current = EffectState.None;
                    ColorMed.interactable = false;

                    OnActionDecidedEnd.OnNext(false);

                }).AddTo(this);
            
            
            _touchDetector.PushSubjectSingle
                .Where(val => CurrentActionType == ActionType.Cut)
                .Subscribe(async point =>
                {
                    GameManager.Instance.PlantSelectSource.Play();

                    IngameSequencer.Score += _plantManager.Cut(point);
                    CurrentActionType = ActionType.None;
                    EffectDistributor.Current = EffectState.None;
                    await PlantImage.SetImage();
                    OnActionDecidedEnd.OnNext(false);
                }).AddTo(this);
        }
    }
    
    public enum ActionType
    {
        None,
        Cut,
        BlueSheet,
        Zyouro,
        ColorMed,
        Random,
        MoonMed,
    }
}
