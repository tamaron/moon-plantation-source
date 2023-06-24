using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Singleton;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace U1W.MoonPlant
{
    /// <summary>
    /// シーン遷移の管理を行う
    /// </summary>
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        public AudioSource InGameBGMSource;
        public AudioSource InGameNightBGMSource;
        public AudioSource TitleBGMSource;
        public AudioSource BirdSESource;
        public AudioSource ButtonSelectSource;
        public AudioSource PlantSelectSource;
        public AudioSource CancellSource;
        public SceneName RankingLoadedFrom;
        [SerializeField] private CanvasGroup _group;
        public readonly Subject<Unit> OnTitleStartSubject = new Subject<Unit>();
        public readonly Subject<Unit> OnMainStartSubject = new Subject<Unit>();
        public readonly Subject<Unit> OnRankingStartSubject = new Subject<Unit>();

        private void Start()
        {
            OnTitleStartSubject.Subscribe(StartTitle).AddTo(this);
            OnMainStartSubject.Subscribe(StartInGame).AddTo(this);
            OnRankingStartSubject.Subscribe(StartRanking).AddTo(this);
        }

        private async void StartTitle(Unit _)
        {
            await _group.DOFade(1f, 0.4f);
            await SceneManager.LoadSceneAsync(SceneName.Title.ToString(), LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneName.Title.ToString()));
            await TryUnloadSceneAsync(SceneName.Ingame);
            await TryUnloadSceneAsync(SceneName.Ranking);
            GameManager.Instance.InGameBGMSource.DOFade(0f, 1f);
            GameManager.Instance.InGameNightBGMSource.DOFade(0f, 1f);
            GameManager.Instance.InGameBGMSource.Stop();
            GameManager.Instance.InGameNightBGMSource.Stop();
            await UniTask.Delay(1000);
            await _group.DOFade(0f, 1f);

        }

        private async void StartInGame(Unit _)
        {
            await _group.DOFade(1f, 0.4f);
            await SceneManager.LoadSceneAsync(SceneName.Ingame.ToString(), LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneName.Ingame.ToString()));
            await TryUnloadSceneAsync(SceneName.Title);
            await TryUnloadSceneAsync(SceneName.Ranking);
            _group.DOFade(0f, 2f);
        }

        private async void StartRanking(Unit _)
        {
            await SceneManager.LoadSceneAsync(SceneName.Ranking.ToString(), LoadSceneMode.Additive);
            await TryUnloadSceneAsync(SceneName.Ingame);
            await TryUnloadSceneAsync(SceneName.Title);
        }

        private bool ContainsScene(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name == sceneName) return true;
            }

            return false;
        }

        private async UniTask TryUnloadSceneAsync(SceneName scene)
        {
            if (ContainsScene(scene.ToString()))
                await SceneManager.UnloadSceneAsync(scene.ToString());
        }
    }

    public enum SceneName
    {
        Ingame,
        Title,
        Ranking
    }
}