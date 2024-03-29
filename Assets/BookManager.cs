﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 食べ物の名前、入力回数、最後に買った日付を格納するクラス
[System.Serializable]
public class Item{
    public string food;
    public int times;
    public int month;
    public int day;
}

public class BookManager : MonoBehaviour
{
    [SerializeField]
	[Tooltip("発生させるエフェクト(パーティクル)")]
	private ParticleSystem particle;
    [SerializeField]
	[Tooltip("発生させるエフェクト(パーティクル)")]
    private ParticleSystem particle2;
    int push = 0;
    ParticleSystem newParticle;
    ParticleSystem newParticle2;

    bool book_open; // 図鑑のシーンに移った時1回だけListの中のデータを参照
    GameObject RaycastManager;
    RaycastManager script;
    Item[] item = new Item[39];
    public GameObject button; // アイテムのボタン
    public GameObject content; // ボタンの親となるコンテント(UI)
    public Text text_times;
    public Text text_lastday;
    public Text foodnametext;
    public Sprite[] m_Sprite;

    void Start()
    {
     book_open = true;   
     for(int i = 0; i < 39; i++){
         item[i] = new Item();
     }
     item[0].food = "米";
     item[1].food = "パン";
     item[2].food = "麺";
     item[3].food = "餅";
     item[4].food = "牛肉";
     item[5].food = "豚肉";
     item[6].food = "鶏肉";
     item[7].food = "イワシ";
     item[8].food = "タラ";
     item[9].food = "サケ";
     item[10].food = "大豆";
     item[11].food = "豆腐";
     item[12].food = "納豆";
     item[13].food = "卵";
     item[14].food = "牛乳";
     item[15].food = "チーズ";
     item[16].food = "ヨーグルト";
     item[17].food = "トマト";
     item[18].food = "にんじん";
     item[19].food = "ブロッコリー";
     item[20].food = "白菜";
     item[21].food = "玉ねぎ";
     item[22].food = "キャベツ";
     item[23].food = "しめじ";
     item[24].food = "マイタケ";
     item[25].food = "えのき";
     item[26].food = "ジャガイモ";
     item[27].food = "さつまいも";
     item[28].food = "こんにゃく";
     item[29].food = "ワカメ";
     item[30].food = "ヒジキ";
     item[31].food = "りんご";
     item[32].food = "みかん";
     item[33].food = "バナナ";
     item[34].food = "ぶどう";
     item[35].food = "いちご";
     item[36].food = "チョコレート";
     item[37].food = "クッキー";
     item[38].food = "ケーキ";

     RaycastManager = GameObject.Find("RaycastManager"); 
     script = RaycastManager.GetComponent<RaycastManager>(); 
    }

    void Update()
    {
        if(book_open == true){
            Wrapper wrapper = new Wrapper();
            wrapper.List = new List<SaveData>();
            wrapper = script.Load();
            for(int i=0; i< wrapper.List.Count; i++ ){
                for(int j=0; j<39; j++){
                    if(wrapper.List[i].food == item[j].food){
                        item[j].times ++; // セーブしたリストの食材名がitem[]の食材名と一致したら入力回数を表すtimes変数を1増やす
                        item[j].month = wrapper.List[i].month; // データの月も格納(最後に入力した月に上書き)
                        item[j].day = wrapper.List[i].day; // データの日も格納(最後に入力した日に上書き)
                    }
                }
            }
            for(int i=0; i<39; i++){
                // 1回でも入力されていれば
                if(item[i].times > 0){
                    Debug.Log(item[i].food + "を" + item[i].times + "回入力。最後に買った日は" + item[i].month + "月" + item[i].day + "日");
                    // ボタンを作成
                    GameObject cloneObject = Instantiate(button, new Vector3(-2.0f + i * 1.0f, 0.0f, 0.0f), Quaternion.identity);
                    cloneObject.transform.parent = content.transform;
                    cloneObject.name = item[i].food;
                    Text food_text = cloneObject.transform.GetChild(0).GetComponent<Text>();
                    food_text.text = item[i].food;
                    if(item[i].times > 19 && item[i].times < 30){
                        cloneObject.GetComponent<Image>().sprite = m_Sprite[0];
                    }
                    else if(item[i].times > 29){
                        cloneObject.GetComponent<Image>().sprite = m_Sprite[1];
                    }
                }
            }
            // リストに１つでもデータがある（図鑑にボタンが1つでもある）場合
            if(wrapper.List.Count > 0){
                // 最初のボタンをあらかじめ押した状態にしておく
                Debug.Log(content.transform.GetChild(0));
                content.transform.GetChild(0).GetComponent<Button>().onClick.Invoke();
            }
        }
        book_open = false;

        GameObject model = GameObject.Find("model");
        if(model){
            Transform modelTransform = model.transform;
            Vector3 localAngle = modelTransform.localEulerAngles;
            localAngle.y += 0.50f; // ローカル座標を基準にy軸を基準に１度分回転
            modelTransform.localEulerAngles = localAngle; // 回転角度を設定
        }
    }

    // アイテムボタンクリックイベント内容
    public void ItemOnClick(GameObject button) {
         for(int i=0; i<39; i++){
             if(button.name == item[i].food){
                text_times.GetComponent<Text>().text = item[i].times + "回";
                text_lastday.GetComponent<Text>().text = item[i].month + "月" + item[i].day + "日";
                foodnametext.GetComponent<Text>().text = item[i].food;
                
                // 入力回数が多いものならばパーティクルを発生させる
                if(item[i].times > 19 && item[i].times < 30){
                    if(newParticle != null){
                        Destroy(newParticle.gameObject);
                    }
                    if(newParticle2 != null){
                        Destroy(newParticle2.gameObject);
                    }
                    // パーティクルシステムのインスタンスを生成
			        newParticle = Instantiate(particle);
			        // パーティクルの発生場所をこのスクリプトをアタッチしているGameObjectの場所に
			        newParticle.transform.position = this.transform.position;
                    // パーティクルを発生
                    newParticle.Play();
                }
                else if(item[i].times > 29){
                    if(newParticle != null){
                        Destroy(newParticle.gameObject);
                    }
                    if(newParticle2 != null){
                        Destroy(newParticle2.gameObject);
                    }
                    //上記と同様
			        newParticle = Instantiate(particle);
			        newParticle.transform.position = this.transform.position;
                    newParticle.Play();
			        newParticle2 = Instantiate(particle2);
			        newParticle2.transform.position = this.transform.position;
                    newParticle2.Play();   
                }
                // それ以外のボタンを押した時、既にパーティクルが存在していればパーティクルを消す
                else{
                    if(newParticle){
                        Destroy(newParticle.gameObject);
                    }
                    if(newParticle2){
                        Destroy(newParticle2.gameObject);
                    }
                }
                // リストにある食べ物を出現
                GameObject obj = (GameObject)Resources.Load(item[i].food);
                GameObject model = GameObject.Find("model");
                if(model){
                    Destroy(model);
                }
                GameObject cloneObject = Instantiate(obj, new Vector3(0.0f, 1.650f, -5.50f), Quaternion.identity);
                cloneObject.name = "model";
             }
         }
    }

    public void ReturnOnClick(){
        SceneManager.LoadScene("Home");
    }
}
