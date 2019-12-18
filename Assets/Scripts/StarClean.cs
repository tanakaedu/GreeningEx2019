using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarClean : MonoBehaviour
{
    [Tooltip("cleanの直下の島の設定用"),SerializeField]
    GameObject[] islands = null;

    //スタート前に設定 → 設定したステージまでの全ステージを緑化
    //スタート後に設定 → 設定したステージを緑化
    [Tooltip("クリアしたステージ"), SerializeField]
    int cleardStageNum = 0;
    [Tooltip("アニメーション速度"), SerializeField]
    float albedoChangeSpeed = 0.025f;

    Renderer[][] islandsRenderer = null;

    float albedoChange = 0f;
    int hozonCleardStageNum;

    void Start()
    {
        hozonCleardStageNum = cleardStageNum;

        islandsRenderer = new Renderer[islands.Length][];
        Color color;

        for(int i = cleardStageNum; i < islands.Length; i++)
        {
            islandsRenderer[i] = islands[i].GetComponentsInChildren<Renderer>();
            for(int j = 0; j<islandsRenderer[i].Length;j++)
            {
                color = islandsRenderer[i][j].material.color;
                color.a = 0f;
                islandsRenderer[i][j].material.color = color;
            }
        }
    }

    void Update()
    {
        if (cleardStageNum != hozonCleardStageNum && albedoChange < 1) 
        {
            Color color;
            albedoChange += albedoChangeSpeed;
            for (int j = 0; j < islandsRenderer[cleardStageNum - 1].Length; j++) 
            {
                color = islandsRenderer[cleardStageNum - 1][j].material.color;
                color.a = albedoChange;
                islandsRenderer[cleardStageNum - 1][j].material.color = color;
            }
        }else if (cleardStageNum != hozonCleardStageNum && albedoChange >= 1)
        {
            hozonCleardStageNum = cleardStageNum;
            albedoChange = 0f;
        }
    }
}
