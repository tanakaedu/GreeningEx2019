using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GreeningEx2019
{
    /// <summary>
    /// <para>シーンの切り替えを制御するクラスです。</para>
    /// <list type="bullet">
    /// <item>最初の起動時は、IsBootingフラグがtrueかどうかで分かります。falseなら、何らかのシーンが起動したことがあることを示します。</item>
    /// <item>シーンを切り替えたい時は、SceneChanger.ChangeScene(切り替えたいシーン)を呼び出します。</item>
    /// <item>シーンの切り替えが要求されているかどうかは、NextSceneがNone以外かどうかで判定できます。</item>
    /// <item>シーンが切り替え中なら、IsChangingがtrueになります。</item>
    /// <item>通常は、enabledをfalseにして、ライフサイクルを停止します。</item>
    /// </list>
    ///
    /// <para>シーンの切り替え処理の流れは以下の通り。</para>
    ///
    /// <list type="number">
    /// <item>ChangeScene(シーン)内でenabledをtrueにして、切り替えたいシーンをNextSceneに記録</item>
    /// <item>LateUpdate()でNextSceneをチェックして、読み込みたいシーンを非同期でアクティブにせずに読み込み開始</item>
    /// <item>フェードアウト</item>
    /// <item>読み込んだシーンをアクティブにする</item>
    /// <item>シーンに仕込んであるSceneManagerBaseのAwakeの完了が全て呼ばれるまで待つ</item>
    /// <item>SceneManagerBaseのOnFadeOutDone()を呼び出す</item>
    /// <item>フェードイン</item>
    /// <item>SceneManagerBaseのOnFadeInDone()を呼び出す</item>
    /// <item>シーン切り替え処理を終了</item>
    /// </list>
    ///
    /// </summary>
    public class SceneChanger : MonoBehaviour
    {
        /// <summary>
        /// インスタンス
        /// </summary>
        public static SceneChanger Instance { get; private set; }

        /// <summary>
        /// シーン定義
        /// </summary>
        public enum SceneType
        {
            None=-1,
            Title,
            StageSelect,
            Game,
            Ending,
        }

        [Tooltip("最初に起動するシーン"), SerializeField]
        SceneType startScene = SceneType.Title;
        [Tooltip("シーン切り替えにかかる秒数"), SerializeField]
        float fadeTime = 0.5f;

        /// <summary>
        /// 現在のシーン
        /// </summary>
        public static SceneType NowScene { get; private set; }

        /// <summary>
        /// 切り替え予定のシーン
        /// </summary>
        public static SceneType NextScene { get; private set; }

        /// <summary>
        /// シーン切り替え処理中
        /// </summary>
        public static bool IsChanging { get; private set; }

        /// <summary>
        /// 起動中フラグ
        /// </summary>
        static bool isBooting = true;
        /// <summary>
        /// 起動中フラグ
        /// </summary>
        public static bool IsBooting { get { return isBooting; } }

        /// <summary>
        /// 現在、読み込み中のシーン管理クラスのリスト
        /// </summary>
        static SceneManagerBase[] loadingSceneManagers = null;

        /// <summary>
        /// 現在、読み込み中のシーンのAsyncOperation
        /// </summary>
        static AsyncOperation[] loadingSceneOperations = null;

        /// <summary>
        /// 解放するシーンのAsyncOperation
        /// </summary>
        static AsyncOperation[] unloadSceneOperations = null;

        /// <summary>
        /// 登録したシーンのAsyncOperationの数
        /// </summary>
        static int loadingSceneOperationCount = 0;

        /// <summary>
        /// Awakeが完了したシーンのSceneManagerBaseの数
        /// </summary>
        static int loadingSceneManagerCount = 0;

        /// <summary>
        /// 解放するシーンの数
        /// </summary>
        static int unloadSceneCount = 0;

        private void Awake()
        {
            Instance = this;
            loadingSceneManagers = new SceneManagerBase[SceneManager.sceneCountInBuildSettings];
            loadingSceneManagerCount = 0;
            loadingSceneOperations = new AsyncOperation[SceneManager.sceneCountInBuildSettings];
            loadingSceneOperationCount = 0;
            unloadSceneOperations = new AsyncOperation[SceneManager.sceneCountInBuildSettings];
            unloadSceneCount = 0;
            IsChanging = false;
            isBooting = true;
            NowScene = SceneType.None;
            NextScene = startScene;
        }

#if UNITY_EDITOR
        /// <summary>
        /// 開始時に、System以外のシーンは解放しておく
        /// </summary>
        static IEnumerator ReleaseNotSystemScenes()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name != "System")
                {
                    unloadSceneOperations[unloadSceneCount] = SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
                    unloadSceneCount++;
                }
            }

            for (int i = 0; i < unloadSceneCount; i++)
            {
                if (!unloadSceneOperations[i].isDone)
                {
                    yield return unloadSceneOperations[i];
                }
            }
            unloadSceneCount = 0;
            loadingSceneManagerCount = 0;
            IsChanging = false;
        }
#endif


        private void LateUpdate()
        {
            // すでに切り替えを開始していたら何もしない
            if (IsChanging) return;

#if UNITY_EDITOR
            if (IsBooting)
            {
                IsChanging = true;
                StartCoroutine(ReleaseNotSystemScenes());
                if (IsChanging) return;
            }
#endif

            IsChanging = true;
            StartCoroutine(SceneChange());
        }


        /// <summary>
        /// シーン切り替え処理
        /// </summary>
        /// <returns></returns>
        IEnumerator SceneChange()
        {
            // シーンを非アクティブで読み込み開始
            loadingSceneOperations[loadingSceneOperationCount] = SceneManager.LoadSceneAsync(NextScene.ToString(), LoadSceneMode.Additive);
            loadingSceneOperations[loadingSceneOperationCount].allowSceneActivation = false;
            loadingSceneOperationCount++;

            // フェードアウト
            yield return Fade.StartFade(Fade.FadeStateType.Out, fadeTime, true);

            // 不要になったシーンを解放する
            if (NowScene != SceneType.None)
            {
                unloadSceneOperations[unloadSceneCount] = SceneManager.UnloadSceneAsync(NowScene.ToString());
                unloadSceneCount++;
            }

            // シーン切り替え
            NowScene = NextScene;
            NextScene = SceneType.None;

            // シーンをアクティブにする
            loadingSceneOperations[0].allowSceneActivation = true;

            // シーンの読み込みと解放の完了を待つ
            for (int i=0;i<loadingSceneOperationCount;i++)
            {
                if (!loadingSceneOperations[i].isDone)
                {
                    yield return loadingSceneOperations[i];
                }
            }
            for (int i = 0; i < unloadSceneCount; i++)
            {
                if (!unloadSceneOperations[i].isDone)
                {
                    yield return unloadSceneOperations[i];
                }
            }
            // シーンの初期化を待つ
            while (loadingSceneManagerCount < loadingSceneOperationCount)
            {
                yield return null;
            }
            // フェードアウト完了処理を呼び出す
            for (int i=0; i<loadingSceneManagerCount;i++)
            {
                loadingSceneManagers[i].OnFadeOutDone();
            }

            // フェードイン
            yield return Fade.StartFade(Fade.FadeStateType.In, fadeTime);

            // フェードイン完了処理を呼び出す
            for (int i = 0; i < loadingSceneManagerCount; i++)
            {
                loadingSceneManagers[i].OnFadeInDone();
            }

            // シーン切り替え完了
            loadingSceneOperationCount = 0;
            loadingSceneManagerCount = 0;
            unloadSceneCount = 0;

            IsChanging = false;
            isBooting = false;
            enabled = false;
        }

        /// <summary>
        /// 切り替えたいシーンを設定します。
        /// </summary>
        /// <param name="nextScene"></param>
        public static void ChangeScene(SceneType nextScene)
        {
            NextScene = nextScene;
            Instance.enabled = true;
        }

        /// <summary>
        /// シーンのAwakeを完了したら、このメソッドを呼び出します。
        /// </summary>
        /// <param name=""></param>
        public static void AwakeDone(SceneManagerBase smb)
        {
            loadingSceneManagers[loadingSceneManagerCount] = smb;
            loadingSceneManagerCount++;
        }
    }
}
