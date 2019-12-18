using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GreeningEx2019
{
    public class TitleManager : SceneManagerBase
    {
        public override void OnFadeOutDone()
        {
            SoundController.PlayBGM(SoundController.BgmType.Title, true);
            SceneManager.SetActiveScene(gameObject.scene);
        }

        private void Update()
        {
            if (Fade.IsFading) return;

            if (GameParams.IsActionAndWaterButtonDown)
            {
                SoundController.Play(SoundController.SeType.Click);
                SceneChanger.ChangeScene(SceneChanger.SceneType.StageSelect);
            }
        }
    }
}
