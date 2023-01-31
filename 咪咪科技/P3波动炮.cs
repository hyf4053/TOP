//using System.Windows.Forms;
using System.Collections.Generic;
//Written by 咪咪科技 - P3探测式波动炮轮椅
//注意，在Triggernometry中所有的List起始的index为1

//初始化正常优先级
enum Priority
{
    WAR = 1,
    DRK = 2,
    PLD = 3,
    GNB = 4,
    WHM = 5,
    AST = 6,
    SGE = 7,
    SCH = 8,
    SAM = 9,
    MNK = 10,
    RPR = 11,
    NIN = 12,
    DRG = 13,
    BRD = 14,
    MCH = 15,
    DNC = 16,
    BLM = 17,
    SMN = 18,
    RDM = 19
}

//初始化P3优先级,以B为顺位，D为逆位
//顺位优先级时1最大，逆位优先级时1最小
enum Phase3PriorityN
{
    SCH = 1,
    SGE = 2,
    WHM = 3,
    AST = 4,
    SAM = 15,
    MNK = 14,
    RPR = 13,
    NIN = 12,
    DRG = 11,
    BRD = 10,
    MCH = 9,
    DNC = 8,
    BLM = 7,
    SMN = 6,
    RDM = 5,
    WAR = 19,
    DRK = 18,
    PLD = 17,
    GNB = 16,
}

enum Phase3PriorityR
{
    SCH = 19,
    SGE = 18,
    WHM = 17,
    AST = 16,
    SAM = 5,
    MNK = 6,
    RPR = 7,
    NIN = 8,
    DRG = 9,
    BRD = 10,
    MCH = 11,
    DNC = 8,
    BLM = 7,
    SMN = 6,
    RDM = 5,
    WAR = 1,
    DRK = 2,
    PLD = 3,
    GNB = 4,
}

//加载变量
//P3 被标记的人的名字list
Triggernometry.Variables.VariableList P3MarkList = TriggernometryHelpers.GetListVariable(false,"P3MarkList");
//左点名list
Triggernometry.Variables.VariableList P3LMarkList = TriggernometryHelpers.GetListVariable(false,"P3LMarkList");
//右点名list
Triggernometry.Variables.VariableList P3RMarkList = TriggernometryHelpers.GetListVariable(false,"P3RMarkList");
//欧米茄波动炮BorD
var LR = TriggernometryHelpers.GetScalarVariable(false,"LR");


//游戏内小队顺位数据获取，这两组数据是一一对应的
//第一行是小队队员名称
Triggernometry.Variables.VariableList PartyIDList = TriggernometryHelpers.GetListVariable(false,"PartyIDList");
//第二行是当前小队顺序的职业顺序
Triggernometry.Variables.VariableList PartyJobList = TriggernometryHelpers.GetListVariable(false,"PartyJobList");

//TODO:这里的1在最后要改成3
if(P3MarkList!=null && P3MarkList.Size() == 3){
    // && P3MarkList.Size()
     //TriggernometryHelpers.PlayTTS(P3MarkList.ToString());
     //int x = (int)Phase3Priority.SCH;
     //Phase3Priority p = (Phase3Priority)1;
     //TriggernometryHelpers.PlayTTS(p.ToString());
     //MessageBox.Show(((int)Phase3Priority.SCH).ToString());
    //TriggernometryHelpers.PlayTTS(P3MarkList.Peek(1).ToString());
    //TriggernometryHelpers.PlayTTS(P3MarkList.Size().ToString());
    //TriggernometryHelpers.PlayTTS(LR);
    //TriggernometryHelpers.PlayTTS(P3MarkList.Peek(i).ToString());

    //排序结果, 这个结果会按照优先级从最高到最低进行排序，里面的int指的是实际玩家在gamepartylist里面的顺序
    int[] order = new int[3];

    //对点名的list进行排序
    for (int i = 1; i < 3+1; i++)
    {
        //玩家名字
        string playerName = P3MarkList.Peek(i).ToString();
        //玩家在list中的位置
        int index = PartyIDList.IndexOf(playerName);
        //保存至待排序的数组
        order[i-1] = index;
        //TriggernometryHelpers.PlayTTS(index.ToString());
    }
    //获取对应的玩家的职业
    string[] jobOrder = new string[3];
    jobOrder[0] = PartyJobList.Peek(order[0]).ToString();
    jobOrder[1] = PartyJobList.Peek(order[1]).ToString();
    jobOrder[2] = PartyJobList.Peek(order[2]).ToString();
    //打B边，使用顺序优先级
    if(LR == "B")
    {
        //将DNC转换成对应的优先级
        Phase3PriorityN EJob0, EJob1, EJob2 =  new Phase3PriorityN();
        Phase3PriorityN.TryParse(jobOrder[0],out EJob0);
        Phase3PriorityN.TryParse(jobOrder[1],out EJob1);
        Phase3PriorityN.TryParse(jobOrder[2],out EJob2);

        // TriggernometryHelpers.PlayTTS(((int)EJob0).ToString());
        // TriggernometryHelpers.PlayTTS(((int)EJob1).ToString());
        // TriggernometryHelpers.PlayTTS(((int)EJob2).ToString());
        //TriggernometryHelpers.PlayTTS("B");

        //原始DNC的index
        Dictionary<string,int> Original = new Dictionary<string,int>();
        //根据优先级排序的结果
        SortedDictionary<int,string> DicSort = new SortedDictionary<int,string>();
        Original.Add(jobOrder[0],0);
        Original.Add(jobOrder[1],1);
        Original.Add(jobOrder[2],2);
        DicSort.Add((int)EJob0,jobOrder[0]);
        DicSort.Add((int)EJob1,jobOrder[1]);
        DicSort.Add((int)EJob2,jobOrder[2]);

        int counter = 0;
        foreach (var item in DicSort)
        {
            if(TriggernometryHelpers.EvaluateStringExpression("${_ffxivplayer}")==P3MarkList.Peek(Original[item.Value]+1).ToString())
            {
                if(P3LMarkList.IndexOf(P3MarkList.Peek(Original[item.Value]+1).ToString())!=0)
                {
                    if(counter==0)
                    {
                        TriggernometryHelpers.PlayTTS("左刀去1点朝向Boy");
                    }
                    else if(counter==1)
                    {
                        TriggernometryHelpers.PlayTTS("左刀去2点朝向Dog");
                    }
                    else if(counter==2)
                    {
                        TriggernometryHelpers.PlayTTS("左刀去3点朝向A");
                    }
                    
                }
                else if(P3RMarkList.IndexOf(P3MarkList.Peek(Original[item.Value]+1).ToString())!=0)
                {
                    if(counter==0)
                    {
                        TriggernometryHelpers.PlayTTS("右刀去1点朝向Dog");
                    }
                    else if(counter==1)
                    {
                        TriggernometryHelpers.PlayTTS("右刀去2点朝向Boy");
                    }
                    else if(counter==2)
                    {
                        TriggernometryHelpers.PlayTTS("右刀去3点朝向C");
                    }
                }
            }
            counter++;
        }
        
    }
    else if(LR == "D")
    {
         //TriggernometryHelpers.PlayTTS(TriggernometryHelpers.EvaluateStringExpression("${_ffxivplayer}"));
         //将DNC转换成对应的优先级
        Phase3PriorityR EJob0, EJob1, EJob2 =  new Phase3PriorityR();
        Phase3PriorityR.TryParse(jobOrder[0],out EJob0);
        Phase3PriorityR.TryParse(jobOrder[1],out EJob1);
        Phase3PriorityR.TryParse(jobOrder[2],out EJob2);

        //TriggernometryHelpers.PlayTTS("D");

        //原始DNC的index
        Dictionary<string,int> Original = new Dictionary<string,int>();
        //根据优先级排序的结果
        SortedDictionary<int,string> DicSort = new SortedDictionary<int,string>();
        Original.Add(jobOrder[0],0);
        Original.Add(jobOrder[1],1);
        Original.Add(jobOrder[2],2);
        DicSort.Add((int)EJob0,jobOrder[0]);
        DicSort.Add((int)EJob1,jobOrder[1]);
        DicSort.Add((int)EJob2,jobOrder[2]);

        int counter = 0;
        foreach (var item in DicSort)
        {
           
            if(TriggernometryHelpers.EvaluateStringExpression("${_ffxivplayer}")==P3MarkList.Peek(Original[item.Value]+1).ToString())
            {
                if(P3LMarkList.IndexOf(P3MarkList.Peek(Original[item.Value]+1).ToString())!=0)
                {
                    if(counter==0)
                    {
                        TriggernometryHelpers.PlayTTS("左刀去3点朝向Dog");
                    }
                    else if(counter==1)
                    {
                        TriggernometryHelpers.PlayTTS("左刀去4点朝向Boy");
                    }
                    else if(counter==2)
                    {
                        TriggernometryHelpers.PlayTTS("左刀去1点朝向C");
                    }
                    
                }
                else if(P3RMarkList.IndexOf(P3MarkList.Peek(Original[item.Value]+1).ToString())!=0)
                {
                    if(counter==0)
                    {
                        TriggernometryHelpers.PlayTTS("右刀去3点朝向Boy");
                    }
                    else if(counter==1)
                    {
                        TriggernometryHelpers.PlayTTS("右刀去4点朝向Dog");
                    }
                    else if(counter==2)
                    {
                        TriggernometryHelpers.PlayTTS("右刀去1点朝向A");
                    }
                }
                
            }
            counter++;
        }
    }

}else{
    TriggernometryHelpers.PlayTTS("空");
}
