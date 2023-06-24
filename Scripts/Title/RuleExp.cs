using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace U1W.MoonPlant
{
    public class RuleExp : MonoBehaviour
    {
        public Button Rule;
        public Button One;
        public Button Two;
        public Button The;
        public Button Back;
        public CanvasGroup All;
        public CanvasGroup F1;
        public CanvasGroup F2;
        public CanvasGroup F3;
        public int index = 0;

        void Start()
        {

            One.GetComponent<Image>().color = Color.gray;
            Rule.onClick.AsObservable().Subscribe(_ =>
            {
                GameManager.Instance.ButtonSelectSource.Play();
                All.DOFade(1f, 0.1f);
                All.interactable = true;
                All.blocksRaycasts = true;
            }).AddTo(this);
            One.GetComponent<Image>().color = Color.gray;

            Back.onClick.AsObservable().Subscribe(_ =>
            {
                GameManager.Instance.CancellSource.Play();

                All.DOFade(0f, 0.1f);
                All.interactable = false;
                All.blocksRaycasts = false;
            }).AddTo(this);

            One.onClick.AsObservable().Where(_ => index != 0).Subscribe(_ =>
            {
                index = 0;
                GameManager.Instance.ButtonSelectSource.Play();

                One.GetComponent<Image>().color = Color.gray;
                Two.GetComponent<Image>().color = Color.white;
                The.GetComponent<Image>().color = Color.white;
                F1.DOFade(1f, 0.1f);
                F2.DOFade(0f, 0.1f);
                F3.DOFade(0f, 0.1f);
            }).AddTo(this);

            Two.onClick.AsObservable().Where(_ => index != 1).Subscribe(_ =>
            {
                index = 1;

                GameManager.Instance.ButtonSelectSource.Play();

                One.GetComponent<Image>().color = Color.white;
                Two.GetComponent<Image>().color = Color.gray;
                The.GetComponent<Image>().color = Color.white;
                F1.DOFade(0f, 0.1f);
                F2.DOFade(1f, 0.1f);
                F3.DOFade(0f, 0.1f);
            }).AddTo(this);

            The.onClick.AsObservable().Where(_ => index != 2).Subscribe(_ =>
            {
                index = 2;

                GameManager.Instance.ButtonSelectSource.Play();

                One.GetComponent<Image>().color = Color.white;
                Two.GetComponent<Image>().color = Color.white;
                The.GetComponent<Image>().color = Color.gray;
                F1.DOFade(0f, 0.1f);
                F2.DOFade(0f, 0.1f);
                F3.DOFade(1f, 0.1f);
            }).AddTo(this);
        }
    }
}