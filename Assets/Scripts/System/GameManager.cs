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

    public GameObject[] options;
    public Text[] lab_options;
    public GameObject optionPanel;
    
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

    public float optionsChangeTime = 0.5f;
    public int lastOptionCount = 0;

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
        
        optionPanel.SetActive(true);
        now_question = 0;

        needSetQuestion = true;
        canAnswer = false;
        for (int i = 0; i < questionCount; i++)
        {
            // 等待答題
            while (!needSetQuestion) yield return null;
            SetProgressBar();

            yield return SetQuetionWithId(now_question);
            // PlayNextQuestionAnime();
            // if(i != 0){
            //     yield return new WaitForSeconds(0.4f);
            // }
            
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

        // switch (questionId)
        // {
        //     case 18:
        //         // kiara.SetKiaraAnime(KiaraState.Exciting);
        //         kiara.PlaySFX(3);
        //         break;
        //     default:
        //         break;
        // }
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
        
        // switch (questionId)
        // {
        //     case 8:
        //         if(answer == 1) {kiara.PlaySFX(2);}
        //         break;
        //     case 12:
        //         if(answer == 0) {kiara.PlaySFX(1);}
        //         break;
        //     case 18:
        //         if(answer == 7) {kiara.PlaySFX(1);}
        //         break;
        //     default:
        //         break;
        // }
    }


    private void SetProgressBar(){
        nowProgress = (float)now_question / (float)questionCount;
        if(nowProgress > 1){nowProgress = 1;}
        if(nowProgress < 0){nowProgress = 0;}

        progressBarFG.transform.localScale = new Vector3(nowProgress,1,1);

        preProgress = nowProgress;
    }

    private void PlayNextQuestionAnime(){

        for (int op = 0; op < lab_options.Length; op++)
        {
            // lab_options[op].text = "";
        }
    }


    public bool waitSetting = false;
    // 顯示答題介面
    public IEnumerator SetQuetionWithId(int questionId){
        // 取得題目的選項數
        int optionCount = QuestionDataManager.questionOptionCount[questionId];
        
        // 設定題目
        string questionInfo_key = questionId + "_question";
        SetTextCardWithKey(questionInfo_key, 1);

        // 設定選項
        SetOptionsLayout(optionCount);
        SetOptionsActive(optionCount);
        yield return new WaitForSeconds(0.005f); // 等待設定完畢

        SetOptionsImageLayout(optionCount);
        // 撥放按鈕切換動畫
        for (int op = 0; op < optionCount; op++)
        {
            Debug.Log($"lastOptionCount{lastOptionCount} op{op}");
            if(op >= lastOptionCount || optionCount > 4){ // 新出現的選項
                options[op].GetComponent<OptionButton>().PlayAnime(optionsChangeTime, true, false);
            }
            else if(optionCount < lastOptionCount && op >= optionCount){ // 消失的選項
                options[op].GetComponent<OptionButton>().PlayAnime(optionsChangeTime, false, true);
            }
            else{ // 其他選項
                options[op].GetComponent<OptionButton>().PlayAnime(optionsChangeTime, false, false);
            }
        }
        lastOptionCount = optionCount; // 設定上一題選項數

        yield return new WaitForSeconds(optionsChangeTime); // 等待選項動畫跑完一半，在改變選項
        for (int op = 0; op < optionCount; op++)
        {
            string optionKey = questionId + optionRawKey + op ;
            lab_options[op].text = LeanLocalization.GetTranslationText(optionKey);
        }
        yield return new WaitForSeconds(optionsChangeTime); // 等待選項動畫跑完，才開放作答
    }

    

    private void SetOptionsLayout(int optionCount){
        if(optionCount > 4){
            option_gridLayoutGroup.constraintCount = 2;
            option_gridLayoutGroup.cellSize = new Vector2(lessThanFourOptionsWeight, optionsHeight);
        }
        else{
            option_gridLayoutGroup.constraintCount = 1;
            option_gridLayoutGroup.cellSize = new Vector2(moreThanFourOptionsWeight, optionsHeight);
        }
    }

    
    private void SetOptionsActive(int optionCount){
        // waitSetting = true;
        foreach (GameObject option in options)
        {
            option.SetActive(false);
        }

        for (int i = 0; i < optionCount; i++)
        {
            // 當此題比上一題多選項、切換版面，該選像文字設為空白
            if(i >= lastOptionCount || optionCount > 4){
                lab_options[i].text = "";
                // 爛寫法:將選項變成透明
                options[i].GetComponent<OptionButton>().SetImageUnActive(); 
            }
            options[i].SetActive(true);
        }
        
    } 

    private void SetOptionsImageLayout(int optionCount){
        for (int i = 0; i < optionCount; i++)
        {
            options[i].GetComponent<OptionButton>().SetLayout();
        }
    }
}

