using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class User
{
    public uint Id;
    public string Login;
    public uint Cristals;
    public int LanguageId;

    public List<PurchasedHero> PurchasedHeroes;
    public List<Transform> HavingHeroesTransforms;

}
