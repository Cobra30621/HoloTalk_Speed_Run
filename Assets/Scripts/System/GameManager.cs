using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Localization;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{

    // 儲存玩家的答題情況
    public List<int> playerAnswers;

    public int now_question; // 現在的題目數
    private int questionCount = 19; // 最多題目數

    // 問題顯示介面
    private const string optionRawKey = "_option";
    private const string questionRawKey = "_question";

    public GridLayoutGroup option_gridLayoutGroup;
    private float moreThanFourOptionsWeight = 1000;
    private float lessThanFourOptionsWeight = 500;
    private float optionsHeight = 130;

    // public GameObject[] options;
    // public Text[] lab_options;
    public GameObject optionPanel;
    public OptionBar[] optionBars;
    
    // 字卡系統
    public TextCardSystem textCardSystem;
    public bool waitClick;

    // 動畫相關
    public Kiara kiara;
    public KiaraResponceData kiaraResponceData;
    public KiaraState[] defaultStateWhenAnswering;
    public KiaraSFX [] defalutSFXWhenAnswering;
    public BGMManager bgm;
    public SpeedRoundAnime speedRoundAnime;

    

    // 流程控制
    private bool needSetQuestion;
    private bool canAnswer;

    // 進度條
    public GameObject progressBarFG;
    private float preProgress;
    private float nowProgress;

    void Start(){
        StartGame();
    }

    void Update(){
        InputNextTextCard();
    }

    [ContextMenu("StartGaming")]
    public void StartGame(){
        now_question = 0;
        GameSettings.ResetToDefaults();
        optionPanel.SetActive(false);
        StartCoroutine(GameCoroutine());
        SetProgressBar(); // 設置進度條
    }

    public void Again(){
        SceneManager.LoadScene("StartScene");
    }

    IEnumerator GameCoroutine()
    {
        yield return new WaitForSeconds(0.4f);
        bgm.PlayBGM(0);
        kiara.SetKiaraAnime(KiaraState.KeepTalking);
        for (int i = 1; i <= 6; i++)
        {
            textCardSystem.SetWaitClick(true);

            string key = $"Kiara/Intro{i}";
            SetTextCardWithKey(key, 1);

            while (textCardSystem.waitClick) yield return null;
        }

        // textCardSystem.SetWaitClick(true);
        // while (textCardSystem.waitClick) yield return null;
        speedRoundAnime.PlayAnime();
        kiara.PlaySFX(0);
        
        SetTextCardWithKey("Kiara/Intro7", 1);
        // 垃圾寫法，等5秒
        for (int i = 0; i < 5; i++)
        {
            if(i < 3)
                kiara.SetKiaraAnime(KiaraState.Exciting);
            yield return new WaitForSeconds(1);
        }
        
        // 設定選項面板
        optionPanel.SetActive(true);
        foreach (OptionBar bar in optionBars)
        {
            bar.Init();
        }
        yield return new WaitForSeconds(0.01f);

        preOptionCount = 0;
        now_question = 0;
        needSetQuestion = true;
        canAnswer = false;
        for (int i = 0; i < questionCount; i++)
        {
            // 等待答題
            while (!needSetQuestion) yield return null;
            SetProgressBar();

            SetQuetionWithId(now_question);
            PlayOptionBarsAnime();

            preOptionCount = optionCount;

            yield return new WaitForSeconds(0.8f); // 等待選項跑完
            
            KiaraResponceWhenQuestioning(now_question);
            needSetQuestion = false;
            canAnswer = true;
        }

        // 等待答完所有題目
        while (now_question < questionCount) yield return null;
        SetProgressBar(); // 設置最後一題的禁毒條
        
        // 結算
        optionPanel.SetActive(false);
        yield return new WaitForSeconds(1);

        kiara.SetKiaraAnime(KiaraState.KeepTalking);
        for (int i = 1; i <= 3; i++)
        {
            if(i != 1){
                textCardSystem.SetWaitClick(true);
                while (textCardSystem.waitClick) yield return null;
            }
            
            string key = $"Kiara/End{i}";
            SetTextCardWithKey(key, 1);
            // yield return new WaitForSeconds(1);
        }
        textCardSystem.SetWaitClick(true);
        while (textCardSystem.waitClick) yield return null;

        kiara.SetKiaraAnime(KiaraState.Except);
        yield return new WaitForSeconds(0.5f);
        // kiara.SetKiaraAnime(KiaraState.Drug);
        
        ResultUI.ShowResult(playerAnswers); 

    }

    private void SetTextCardWithKey(string key , int cardType){
        string info = LeanLocalization.GetTranslationText(key);
        TextCard textCard = new TextCard(info, cardType);
        textCardSystem.SetNextCard(textCard);
    }

    private void InputNextTextCard(){
        if(Input.GetKeyDown(KeyCode.Return)){
            textCardSystem.SetWaitClick(false);
        }
    }

    public void AnswererQuestion(int answer){
        if(!canAnswer){return;}

        playerAnswers[now_question] = answer;
        Debug.Log($"Q{now_question}:{answer}");

        KiaraResponceWhenAnswer(now_question, answer);
        now_question ++;

        needSetQuestion = true;
        canAnswer = false;
    }

    // 回答問題時的反應
    private void KiaraResponceWhenQuestioning(int questionId){
        bool defaults = true; // 預設反應
        foreach (KiaraResponce responce in kiaraResponceData.questioningResponces )
        {
            if(responce.questionId == questionId){
                kiara.SetKiaraAnime(responce.kiaraState);
                kiara.PlaySFX(responce.kiaraSFX);
                defaults = false;
            }
        }

        if(defaults){
            kiara.SetKiaraAnime(KiaraState.Talking);
        }
    }

    private void KiaraResponceWhenAnswer(int questionId, int answer){
        bool defaults = true; // 預設反應
        foreach (KiaraResponce responce in kiaraResponceData.answeredResponces )
        {
            if(responce.questionId == questionId & responce.answer == answer){
                kiara.SetKiaraAnime(responce.kiaraState);
                kiara.PlaySFX(responce.kiaraSFX);
                defaults = false;
            }
        }

        if(defaults){
            int f = Random.Range(0, defaultStateWhenAnswering.Length);
            kiara.SetKiaraAnime(defaultStateWhenAnswering[f]);

            // 有一半機率會發聲 
            int f2 = Random.Range(0, defalutSFXWhenAnswering.Length);
            if(f2 < defalutSFXWhenAnswering.Length){
                // 先暫停播音效
                // kiara.PlaySFX(defalutSFXWhenAnswering[f2]);
            }
        }
        
    }


    private void SetProgressBar(){
        nowProgress = (float)now_question / (float)questionCount;
        if(nowProgress > 1){nowProgress = 1;}
        if(nowProgress < 0){nowProgress = 0;}

        
        progressBarFG.transform.localScale = new Vector3(nowProgress,1,1);

        preProgress = nowProgress;
    }


    public bool oneOptionPerBar;
    private int optionCount;
    private int preOptionCount;

    // 顯示答題介面
    public void SetQuetionWithId(int questionId){
        // 取得題目的選項數
        optionCount = QuestionDataManager.questionOptionCount[questionId];
        
        // 設定題目
        string questionInfo_key = questionId + "_question";
        SetTextCardWithKey(questionInfo_key, 1);

        WhetherOneOptionPerBar(optionCount);

        // 設定選項資訊
        for (int op = 0; op < optionCount; op++)
        {
            string optionKey = questionId + optionRawKey + op ;
            string info = LeanLocalization.GetTranslationText(optionKey);
            if(oneOptionPerBar){
                optionBars[op].SetOption1Info(info);
            }
            else{
                int barID = op / 2;
                int optionID = op % 2;
                Debug.Log($"op{op} index{barID}");
                optionBars[barID].SetOption2Info(optionID, info);
            }   
        }
        
    }

    private bool preQuestionIsTwoOptionPerBar;

    // 宇宙無敵dirty code OAO
    private void PlayOptionBarsAnime(){

        bool optionSub; // 選項變少
        int count; // 取這題與前題選項最多的
        // Debug.Log($"optionCount{optionCount} preOptionCount{preOptionCount}\n count{count} optionSub{optionSub}"); 
        if(oneOptionPerBar){ // 單邊選項
            if(preQuestionIsTwoOptionPerBar){ // 上一題是雙邊選項
                optionSub = optionCount  < (preOptionCount/2); // 選項變少
                count = optionSub ? preOptionCount: optionCount; // 取這題與前題選項最多的
            }
            else{
                optionSub = optionCount < preOptionCount; // 選項變少
                count = optionSub ? preOptionCount: optionCount; // 取這題與前題選項最多的
            }
            for (int op = 0; op < count; op++)
            {   
                // 選項增加與否
                if(op >= preOptionCount && !optionSub){ // 選項增加
                    Debug.Log("Fade In");
                    optionBars[op].FadeIn();
                }
                if(op >= optionCount && optionSub){ // 選項減少
                    Debug.Log("Fade Out");
                    optionBars[op].FadeOut();
                }
                optionBars[op].PlayAnime(true, false);
            }

            preQuestionIsTwoOptionPerBar = false;
        }
        else{ // 雙邊選項
            optionSub = (optionCount / 2) < preOptionCount; // 選項變少
            count = optionSub ? preOptionCount: optionCount; // 取這題與前題選項最多的

            for (int i = 0; i < count; i +=2){
                // 選項增加與否
                if(i >= preOptionCount && !optionSub){ // 選項增加
                    Debug.Log("Fade In");
                    optionBars[i / 2].FadeIn();
                }
                if(i >= optionCount && optionSub){ // 選項減少
                    Debug.Log("Fade Out");
                    optionBars[i / 2].FadeOut();
                }
                // 選項個數為奇數或偶數
                if(i == optionCount -1) // optionCount: 5,7,9 
                    optionBars[i / 2].PlayAnime(false, false);
                else
                    optionBars[i / 2].PlayAnime(false, true);
            }
            preQuestionIsTwoOptionPerBar = true;
        }
    }

    private bool WhetherOneOptionPerBar(int optionsCount){
        oneOptionPerBar = (optionsCount < 5);
        return oneOptionPerBar;
    }

}

