using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace U1W.MoonPlant
{
    public class RankingSequencer : MonoBehaviour
    {
        [SerializeField] private RectTransform CongText;
        [SerializeField] private RectTransform Ranking;
        [SerializeField] private Button TitleButton;
        [SerializeField] private CanvasGroup ToTitleButton;
        [SerializeField] private CanvasGroup UnloadButton;

        async void Start()
        {
            if (GameManager.Instance.RankingLoadedFrom == SceneName.Ingame)
            {
                ToTitleButton.interactable = true;
                ToTitleButton.alpha = 1f;

                TitleButton.onClick.AsObservable().First().Subscribe(_ =>
                {
                    GameManager.Instance.OnTitleStartSubject.OnNext(Unit.Default);
                }).AddTo(this);

                await CongText.DOAnchorPos(Vector2.zero, 1.5f).From(new Vector2(1138, 0));
                await UniTask.Delay(1800);
                CongText.DOAnchorPos(new Vector2(-1138, 0), 1.5f);
                await UniTask.Delay(800);
                await Ranking.DOAnchorPos(Vector2.zero, 1.5f).From(new Vector2(1138, 0));
            }
            else if (GameManager.Instance.RankingLoadedFrom == SceneName.Title)
            {
                UnloadButton.interactable = true;
                UnloadButton.alpha = 1f;
                await Ranking.DOAnchorPos(Vector2.zero, 1.5f).From(new Vector2(1138, 0));
            }
        }
    }
}