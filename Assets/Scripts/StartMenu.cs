using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static MainManager;

public class StartMenu : MonoBehaviour
{
    private TMP_InputField Field;
    public static StartMenu Instance; // burda, olu�turudu�umuz StartMenuye ba�l� gameobjectin, yani Menu sahnesindeki MainManager'�n bir kopyas�n� olu�turuyoruz. bu sayede burdaki bilgileri saklayabilicez
    public string playerName;
    public TextMeshProUGUI highScore;
    public string l_name;
    public int l_highScore;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null)  // burda bir kontrol yap�yoruz. e�erki halihaz�rda 1 �rnek olu�turulmu�sa onu yok edicek.
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;  // sonra da burda yenisini olu�turucak
        DontDestroyOnLoad(gameObject);  // sahne de�i�iminde var olan gameobjeyi yoketmemesi i�in bu kodu kullan�yoruz.        
        Field = GameObject.Find("NameHolder").GetComponent<TMP_InputField>(); // isim yzd���m�z alan� daha sonra kullanmak i�in referans al�yoruz.
        LoadData();
        highScore.text = "HighScore : " + l_name + " : " + l_highScore;
        // istedi�imiz bilgileri olu�turulan dosyadan almak istedi�imizde buraya yazabiliriz. yada bunun i�in bir fonksiyon olu�turup o fonksiyonu  burda �a��r�r�z.
    }
    public void ad�ekle() //bunu awake in i�inde field.onvaluechange.addlisiner(ad�ekle) diyerek te koyabiliriz. ama ben uniti inspector de ekledim.
    {
        playerName = Field.text;
    }
    public void LoadData() //ba�ka bir yerde y�kl� olan json dosyas�n� ba�ka herhang bir yerden �ekmek i�in json dosyas�nda kay�tl� clas�n ad�n� kullanarak direk �a��rabiliriz. a�a��da oldu�u gibi.
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            l_highScore = data.m_Points;
            l_name = data.m_Name;
        }
    }
    //normalde ismi kaydedip daha sonra uygulamay� kapat�p a�t���m�zda isim kals�n istersek bu kod ile kay�t i�lemi yap�yoruz. ama bu projede isim kayd� yapm�yaca��z
    //[System.Serializable]
    //public class SaveData // buraya kaydetmek istedi�imiz verileri giriyoruz.
    //{
    //    public string playerName;
    //}
    //public void SaveName()
    //{
    //    SaveData data = new SaveData();
    //    data.playerName = playerName; // burada savedatan�n i�indeki playername ile inputfield daki yazd�klar�m�z ile e�liyoruz.

    //    string json = JsonUtility.ToJson(data); // burada json ad�nda bir Jsonutility olu�turup data ad�n� verdi�imiz savedata class�n� i�ine ekliyoruz. json yerine ba�ka bi�ey de isimlendireviliriz.
    //    File.WriteAllText(Application.persistentDataPath + "savefile.json", json); //burada dosya olu�turuyoruz. bu sayede oyundan ��ksak bile son bilgilerimiz kaydedilecek.
    //                                                                               //Application.persistentDataPath + "savefile.json" nereye kay�t yap�alca�� json ile yaz�lacak �eyin ismini yaz�yoruz.
    //}
    public void GameStart ()
    {
        SceneManager.LoadScene(0);
    }
    public void Exit()
    {
        //MainManager.Instance.SaveData(); burda ��karken ismi kaydetsin diye kod yazd�k ama kay�t yapm�yaca��m�z i�in bunu kullanm�yoruz.
#if UNITY_EDITOR //burda se�enekli exit koyduk. e�er unity de uygulamay� a�arsak ayr� kod. e�er de�ilse kodu ba�ka yazd�k. buradaki ayr�m �u, normal if koysak bilgisayar yine onu kontrol ediyor. burda ise kotrol etmiyor.
        EditorApplication.ExitPlaymode();
#else
        Aplication.Quit();
#endif
    }
}
