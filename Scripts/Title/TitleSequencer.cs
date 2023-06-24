using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace U1W.MoonPlant
{
    public class TitleSequencer : MonoBehaviour
    {
        [SerializeField] private Button StartInGameButton;
        [SerializeField] private Button ViewRankingButton;

        void Start()
        {
            GameManager.Instance.TitleBGMSource.volume = 1f;
            GameManager.Instance.TitleBGMSource.Play();
            GameManager.Instance.RankingLoadedFrom = SceneName.Title;
            StartInGameButton.onClick.AsObservable()
                .First()
                .Subscribe(async _ =>
                {
                    GameManager.Instance.PlantSelectSource.Play();
                    await GameManager.Instance.TitleBGMSource.DOFade(0f, 0.3f);
                    GameManager.Instance.TitleBGMSource.Stop();
                    GameManager.Instance.OnMainStartSubject.OnNext(Unit.Default);
                }).AddTo(this);

            ViewRankingButton.onClick.AsObservable().ThrottleFirst(TimeSpan.FromMilliseconds(2000))
                .Subscribe(_ =>
                {
                    GameManager.Instance.ButtonSelectSource.Play();
                    naichilab.RankingLoader.Instance.SendScoreAndShowRanking(0);

                }).AddTo(this);
        }
    }
}