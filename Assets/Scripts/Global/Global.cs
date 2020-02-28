using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using UnityEngine;
using System.Xml;
using UnityEngine.Advertisements;
using System.Linq;

public class Global
{
    const int port = 8888;
    const string address = "192.168.1.41";
    public static User CurrentUser;

    public static void GetHavingHeroesTransforms()
    {
        List<Transform> havingHeroes = new List<Transform>();
        var allHeroes = Resources.LoadAll("Prefabs/Hero").OfType<GameObject>().Select(h => h.transform).Where(t => t.GetComponent<Hero>() != null);
        var purchasedHeroes = CurrentUser.PurchasedHeroes;
        foreach (var hero in allHeroes)
        {
            var heroComponent = hero.GetComponent<Hero>();
            var heroComponentId = heroComponent.id;
            var purchasedHero = purchasedHeroes.FirstOrDefault(h => h.Id == heroComponentId);
            if (purchasedHero == null)
                continue;

            heroComponent.level = purchasedHero.Level;
            heroComponent.experience = purchasedHero.Experience;

            havingHeroes.Add(hero);
        }
        CurrentUser.HavingHeroesTransforms = havingHeroes;
    }

    public static void ShowRewardedVideo()
    {
        /*ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;

		Advertisement.Show("rewardedVideo", options);*/
    }

    /*public static void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            Debug.Log("Video completed - Offer a reward to the player");

        }
        else if (result == ShowResult.Skipped)
        {
            Debug.LogWarning("Video was skipped - Do NOT reward the player");

        }
        else if (result == ShowResult.Failed)
        {
            Debug.LogError("Video failed to show");
        }
    }*/

    public static bool Logining(string login)
    {
        XmlDocument loginingXML = GetLoginingXML(login);
        XmlDocument answerXML = GetAnswerXML(loginingXML);

        var answerRoot = answerXML.LastChild;
        var successNode = answerRoot["Success"];
        if (successNode == null)
            return false;
        string success = successNode.InnerText;
        if (success != "ok")
            return false;

        var answerNode = answerRoot["Answer"];
        if (answerNode == null)
            return false;

        string answer = answerNode.InnerText;
        CurrentUser = GetUserFromAnswer(answer);

        return CurrentUser != null;
    }

    public static User GetUserFromAnswer(string answer)
    {
        User user = null;

        XmlDocument xml = new XmlDocument();
        xml.LoadXml(answer);
        var root = xml.LastChild;
        var answerNode = root["Answer"];
        var userNode = answerNode["User"];
        uint userId = uint.Parse(userNode["Id"].InnerText);
        string login = userNode["Login"].InnerText;
        uint cristals = uint.Parse(userNode["Cristals"].InnerText);
        List<PurchasedHero> purchasedHeroes = new List<PurchasedHero>();
        var havingHeroesNode = userNode["Heroes"];
        var havingHeroNodes = havingHeroesNode.ChildNodes;
        foreach (XmlNode havingHeroNode in havingHeroNodes)
        {
            uint heroId = uint.Parse(havingHeroNode["Id"].InnerText);
            byte heroLevel = byte.Parse(havingHeroNode["Level"].InnerText);
            uint heroExperience = uint.Parse(havingHeroNode["Experience"].InnerText);

            PurchasedHero hero = new PurchasedHero()
            {
                Id = heroId,
                Level = heroLevel,
                Experience = heroExperience
            };

            purchasedHeroes.Add(hero);
        }

        user = new User()
        {
            Id = userId,
            Login = login,
            Cristals = cristals,
            PurchasedHeroes = purchasedHeroes
        };

        return user;
    }


    public static XmlDocument GetAnswerXML(XmlDocument xml)
    {
        XmlDocument answerXML = new XmlDocument();
        var rootEl = answerXML.CreateElement("root");
        answerXML.AppendChild(rootEl);

        string success;
        TcpClient client = null;
        try
        {
            client = new TcpClient(address, port);
            NetworkStream stream = client.GetStream();

            //while (true)
            //{
            // сообщение
            string message = xml.InnerXml;
            // преобразуем сообщение в массив байтов
            byte[] data = Encoding.Unicode.GetBytes(message);
            // отправка сообщения
            stream.Write(data, 0, data.Length);

            // получаем ответ
            data = new byte[256]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (stream.DataAvailable);

            message = builder.ToString();

            XmlElement answerEl = CreateElement("Answer", message, answerXML);
            rootEl.AppendChild(answerEl);
            success = "ok";
            //break;
            //}
        }
        catch (Exception ex)
        {
            success = ex.Message.ToString();
        }
        XmlElement successEl = CreateElement("Success", success, answerXML);
        rootEl.AppendChild(successEl);

        return answerXML;
    }

    public static XmlDocument GetLoginingXML(string login)
    {
        XmlDocument xml = new XmlDocument();
        var root = xml.CreateElement("root");
        var actionEl = CreateElement("Action", "Logining", xml);

        var dataEl = xml.CreateElement("Data");
        var loginEl = CreateElement("Login", login, xml);
        dataEl.AppendChild(loginEl);

        root.AppendChild(actionEl);
        root.AppendChild(dataEl);
        xml.AppendChild(root);

        return xml;
    }

    public static XmlElement CreateElement(string xmlName, string xmlValue, XmlDocument doc)
    {
        var element = doc.CreateElement(xmlName);
        var val = doc.CreateTextNode(xmlValue);
        element.AppendChild(val);

        return element;
    }
}
