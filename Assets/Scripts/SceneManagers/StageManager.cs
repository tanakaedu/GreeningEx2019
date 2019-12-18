using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GreeningEx2019
{
    public class StageManager : SceneManagerBase
    {
        [Tooltip("ゲーム用プレイヤープレハブ"), SerializeField]
        GameObject stellaPrefab = null;

        /// <summary>
        /// 操作可能な状態かどうかを返します。
        /// </summary>
        public static bool CanMove
        {
            get
            {
                return !Fade.IsFading;
            }
        }

        public override void OnFadeOutDone()
        {
            SoundController.PlayBGM(SoundController.BgmType.Title, true);
            SceneManager.SetActiveScene(gameObject.scene);

            // プレイヤーを入れ替える
            GameObject stabPlayer = GameObject.FindGameObjectWithTag("Player");
            GameObject myp = Instantiate(stellaPrefab, stabPlayer.transform.position, stabPlayer.transform.rotation);
            Destroy(stabPlayer);

            // カメラにターゲットを設定
            FollowCamera fcam = Camera.main.gameObject.GetComponent<FollowCamera>();
            fcam.SetTarget(myp.transform);
        }
    }
}
