using System.Collections.Generic;
using UnityEngine;

namespace LynxCity.Core
{
    public class GameState : MonoBehaviour
    {
        public static GameState Instance { get; private set; }
        public string CurrentDistrict { get; set; }="Shinjuku 1989";
        public int Year { get; set; }=1991;
        public int Yen { get; private set; }=12000;
        public float Heat { get; private set; }
        public string CurrentOutfit { get; set; }="Campus Casual '91";
        public readonly Dictionary<string,int> Inventory=new();
        public string InteractionPrompt { get; set; }="";
        void Awake(){if(Instance!=null&&Instance!=this){Destroy(gameObject);return;}Instance=this;DontDestroyOnLoad(gameObject);}
        public bool Spend(int amount){if(amount<0||Yen<amount)return false;Yen-=amount;return true;}
        public void Earn(int amount)=>Yen+=Mathf.Max(0,amount);public void AddHeat(float amount)=>Heat=Mathf.Clamp(Heat+amount,0f,100f);
        public bool SpendHeat(float amount){if(Heat<amount)return false;Heat-=amount;return true;}
        public void AddItem(string id,int count=1){Inventory.TryGetValue(id,out var current);Inventory[id]=current+Mathf.Max(1,count);}
    }
}
