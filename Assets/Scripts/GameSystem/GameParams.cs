using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreeningEx2019
{
    /// <summary>
    /// クリア済みのステージ数や、プレイヤーが操作が可能な状況かどうかなど、
    /// ゲーム全体を制御するための変数や機能を提供するためのクラス。
    /// </summary>
    public class GameParams : Singleton<GameParams>
    {
        [Tooltip("ステージ選択シーンへ移行する際のモード")]
        public StageSelectManager.ToStageSelectType toStageSelect = StageSelectManager.ToStageSelectType.NewGame;
        [Tooltip("クリア済みのステージ数。0なら全くクリアしていない。1ならStage1をクリア済み。")]
        public int clearedStageCount = 10;

        /// <summary>
        /// クリア済みステージ数
        /// </summary>
        public static int ClearedStageCount { get { return Instance.clearedStageCount; } }

        /// <summary>
        /// 全ステージ数
        /// </summary>
        public const int StageCount = 10;

        /// <summary>
        /// ステージ選択で選ばれたステージ。0=Stage1
        /// </summary>
        public static int SelectedStage { get; private set; }

        public static bool IsActionAndWaterButtonDown {
            get {
                return Input.GetButtonDown("Action") || Input.GetButtonDown("Water");
            }
        }

        private void Awake()
        {
            // TODO: PlayerPrefsから、ClearedStageCountを読み出す

            SelectedStage = Mathf.Min(clearedStageCount, StageCount-1);
            if (SelectedStage == StageCount - 1)
            {
                SelectedStage = 0;
            }
        }

        /// <summary>
        /// 選択しているステージ番号を、次のステージに変更。ClearedStageをオーバーしていたら0に戻す
        /// </summary>
        public static void NextSelectStage()
        {
            int div = Mathf.Min(StageCount, ClearedStageCount + 1);
            SelectedStage = (SelectedStage + 1) % div;
        }

        /// <summary>
        /// 選択しているステージ番号を、前のステージに変更。0を越えていたら、ClearedStageへ。
        /// </summary>
        public static void PrevSelectStage()
        {
            SelectedStage = (SelectedStage == 0) ? ClearedStageCount : SelectedStage-1;
            SelectedStage = Mathf.Min(SelectedStage, StageCount - 1);
        }

        /// <summary>
        /// ステージクリア時に呼び出して、パラメーターを更新する。
        /// </summary>
        public static void StageClear()
        {
            Instance.toStageSelect = StageSelectManager.ToStageSelectType.Clear;
            if (SelectedStage == ClearedStageCount-1)
            {
                Instance.clearedStageCount++;

                // TODO: PlayerPrefsで、ClearedStageCountを保存
            }
        }
    }
}
