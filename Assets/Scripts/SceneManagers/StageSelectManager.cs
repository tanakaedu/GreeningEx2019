using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GreeningEx2019
{
    public class StageSelectManager : SceneManagerBase
    {
        [Tooltip("島のオブジェクト"), SerializeField]
        Transform[] islands = null;
        [Tooltip("ステージの星を表示する高さ"), SerializeField]
        float stageStarHeight = 4.5f;

        /// <summary>
        /// 操作が可能になったら、trueを返します。
        /// </summary>
        public static bool CanMove
        {
            get
            {
                return state == StateType.PlayerControl;
            }
        }

        /// <summary>
        /// ステージ選択シーンに来る時の状況を表す。
        /// </summary>
        public enum ToStageSelectType
        {
            NewGame,    // 新規にゲームを開始
            Clear,
            Back,
        }

        enum StateType
        {
            OpeningMovie,
            NewGame,
            PlayerControl,
            Clear,
            Back,
            StoryMovie,
        }

        /// <summary>
        /// 現在の状態
        /// </summary>
        static StateType state = StateType.OpeningMovie;

        // リピートを防ぐための前のカーソル
        static float lastCursor = 0;

        private new void Awake()
        {
            switch (GameParams.Instance.toStageSelect)
            {
                case ToStageSelectType.NewGame:
                    state = StateType.OpeningMovie;
                    break;
                case ToStageSelectType.Clear:
                    // 途中の動画チェック
                    state = StateType.Clear;
                    break;
                case ToStageSelectType.Back:
                    state = StateType.Back;
                    break;
            }

            base.Awake();
        }

        private void Update()
        {
            switch (state)
            {
                case StateType.PlayerControl:
                    UpdatePlayerControl();
                    break;

                default:
                    // デバッグコード。未実装の状態は、すぐに操作に移行
                    if (Fade.IsFading) return;
                    state = StateType.PlayerControl;
                    break;
            }
        }

        public override void OnFadeOutDone()
        {
            base.OnFadeOutDone();
            SceneManager.SetActiveScene(gameObject.scene);
        }


        void UpdatePlayerControl()
        {
            float cursor = Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical");
            if ((cursor < -0.5f) && (lastCursor > -0.5f))
            {
                GameParams.PrevSelectStage();
            }
            else if ((cursor > 0.5f) && (lastCursor < 0.5f))
            {
                GameParams.NextSelectStage();
            }
            lastCursor = cursor;

            if (GameParams.IsActionAndWaterButtonDown)
            {
                SceneChanger.ChangeScene(SceneChanger.SceneType.Game);
            }
        }
    }
}
