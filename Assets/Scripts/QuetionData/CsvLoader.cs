using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsvLoader : MonoBehaviour
{
    public string [][] VTuberAnswers;   

    // 注意:檔案類型要txt
    public void Init(){
        VTuberAnswers = LoadData("holotalk");
        // options = LoadData("options");
    }



    private string [][] LoadData(string filename){
        //读取csv二进制文件  
        TextAsset binAsset = Resources.Load (filename, typeof(TextAsset)) as TextAsset;         
            
        //读取每一行的内容  
        string [] lineArray = binAsset.text.Split ("\r"[0]);  
            
        //创建二维数组  
        string [][] array;
        array = new string [lineArray.Length][];  
            
        //把csv中的数据储存在二位数组中  
        for(int i =0;i < lineArray.Length; i++)  
        {  
            array[i] = lineArray[i].Split (',');  
            
        }

        return array;
    }

    public int GetQuetionCount(){
        return VTuberAnswers[0].Length - 1; // 減去標題
    }

    public int GetVTuberCount(){
        return VTuberAnswers.Length - 1; // 減去標題
    }
    public string GetVTuberAnswersDataByRowAndColFrom(int nRow, int nCol)  
    {  
        if (VTuberAnswers.Length <= 0 || nRow >= VTuberAnswers.Length)  
            return"";  
        if (nCol >= VTuberAnswers[0].Length)  
            return"";  
        return VTuberAnswers[nRow][nCol];  
    }  


}
