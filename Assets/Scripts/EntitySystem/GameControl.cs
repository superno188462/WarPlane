/***********************************************
 * \file        GameControl.cs
 * \author      
 * \date        
 * \version     
 * \brief       游戏控制器
 * \note        
 * \remarks     
 ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : Singleton<GameControl>
{
    //总波数、当前波数
    public const int waves = 3;
    public int wave = 0;

    //下一波需要创建的单位
    public Dictionary<string, int> nextWaveCreate;
    

    public List<EntityBase> list;
    //new => list.add()
    //die => list,remove()

    //波数显示的文本
    public Text text;

    //剩余总敌人数
    public int sum;

    //判断玩家是否被创建
    public bool changePlayer ;

    //是否要执行下一波
    //刷怪状态
    public bool IscRefresh ;
    //清怪状态
    public bool IscDie ;
    //增益状态，暂停状态
    public bool IscPause ;

    //增益
    public int gainAttack = 0;//倍数
    public float gainAttackAdd = 0;//值
    

    protected override void Awake()
    {
        base.Awake();
        this.gameObject.AddComponent<Mathc>(); 
        this.gameObject.AddComponent<ResourceSystem>();
        this.gameObject.AddComponent<WebLoad>();
        this.gameObject.AddComponent<Order>();
        this.gameObject.AddComponent<PoolSystem>();
        this.gameObject.AddComponent<AudioSystem>();
        this.gameObject.AddComponent<ParticleSystemSystem>();
        this.gameObject.AddComponent<UISystem>();
        this.gameObject.AddComponent<EntitySystem>();
        this.gameObject.AddComponent<PlayerControl>();
        nextWaveCreate = new Dictionary<string, int>();

        changePlayer = true;
        IscRefresh = true;
        IscDie = false;
        IscPause = false;
    }

    //创建玩家，绘制武器表ui
    public void SetPlayer()
    {
        EntityBase unit = EntitySystem.CreateEntityInCondition("ID001", transform.position, 0);
        PlayerControl.Instance.UnitEnter(unit);
        PlayerControl.Instance.unit.SetGroup(EntityGroup.Player);
        }

    //检测不同状态
    private void Update()
    {
        if (changePlayer == true)
        {
            changePlayer = !changePlayer;
            SetPlayer();
        }

        if (IscRefresh == true)
        {
            NextWave();
        } 
        if(IscDie == true)
        {
           // Debug.Log("CreateGainOptionUI");
            CreateGainOptionUI();
        }
        if(IscPause ==true)
        {
            
        }
    }

    //改变波数提示
    public void changeText(int a,int b)
    {
        string str = $"剩余波数：{a }\n剩余敌人：{b}";
        text.text = str;
    }

    //将下一波的数据传入字典中
    public void FillDict(string str)//ID002 * 5 + ID003 * 1
    {
        str.Trim();
        string[] strs = str.Split('+');
        for (int i = 0; i < strs.Length; i++)
        {
            if (strs[i].Trim().Length < 3) continue;
            string[] strs2 = strs[i].Split('*');
            string id = strs2[0];
            int num = int.Parse(strs2[1]);

            nextWaveCreate[id] = num;
        }
        
    }

    //生成敌人
    public void  CreateEnemy()
    {
        sum = 0;
        foreach (KeyValuePair<string ,int> p in nextWaveCreate)
        {
            sum += p.Value;
            Vector3 pos = PlayerControl.Instance.unit.transform.position;
            for(int i=0;i<p.Value;i++)
            {
                EntitySystem.CreateEntityInArea(p.Key, pos, 20);
            }
        }
        IscDie = false;
    }

    //展示增益ui界面,进入停止状态
    public void CreateGainOptionUI()
    {
        IscDie = false;
        IscPause = true;
        UISystem.Instance.FindChildByName("GainOptionPanel").GetComponentInChildren<GainOptionPanel>().ShowGainOption();
    }

    //下一波函数，写字典，创建敌人，修改文本
    public void NextWave()
    {
        IscRefresh = false;
        string w = wave.ToString();
        if (EntitySystem.waveInfo.ContainsKey(w))
        {
            string info = EntitySystem.waveInfo[w];
            FillDict(info);
        }
        else
        {
            return;
        }

        CreateEnemy();

        wave += 1;

        changeText(waves - wave+1, sum);
        
    }

    //当敌人阵亡，需要在字典中减去该单位，判断敌人是否全部阵亡
    public void Push(string id)
    {
        //Debug.Log(id);
        if (nextWaveCreate.ContainsKey(id))
        {
            nextWaveCreate[id] -= 1;
            if (nextWaveCreate[id] == 0)
                nextWaveCreate.Remove(id);
        }
        sum -= 1;
        if (sum == 0)
        {
            ClearAllEnemy();
        }
        //Debug.Log($"当前波数：{wave }\n剩余敌人：{sum}");
        changeText(waves-wave+1, sum);

        //combatSystem
        // Is  Controol  逻辑is 控制器c  名字Refresh
        //IscRefresh=true
        //enemyNumber=12
        //enemyNumber--   =>  IscRefresh  = enemyNumber>0 

        //IscPause  //暂停

        //小怪死亡判断是否杀死所有小怪                             <=
        //enemy =>die()  => combatsystem=> UpdateCombatState()  // 判断
        //返回是否通关波数
        //【小怪是否全部刷新完成】  =》 【小怪是否全部死亡】=》【拉去增益进行下一波】
        //  IscRefresh                         IscDie/next            IscPause// ClearAllEnemy  
        //IscRefresh =true   =return false ;
        //IscRefresh =false  => 在场小怪的存货数量


    }


    //清理掉所有小怪调用的事件
    public void ClearAllEnemy()
    {
        IscDie = true;
    }
}
