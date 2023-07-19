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
    public static StartMenu Instance; // burda, oluþturuduðumuz StartMenuye baðlý gameobjectin, yani Menu sahnesindeki MainManager'ýn bir kopyasýný oluþturuyoruz. bu sayede burdaki bilgileri saklayabilicez
    public string playerName;
    public TextMeshProUGUI highScore;
    public string l_name;
    public int l_highScore;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null)  // burda bir kontrol yapýyoruz. eðerki halihazýrda 1 örnek oluþturulmuþsa onu yok edicek.
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;  // sonra da burda yenisini oluþturucak
        DontDestroyOnLoad(gameObject);  // sahne deðiþiminde var olan gameobjeyi yoketmemesi için bu kodu kullanýyoruz.        
        Field = GameObject.Find("NameHolder").GetComponent<TMP_InputField>(); // isim yzdýðýmýz alaný daha sonra kullanmak için referans alýyoruz.
        LoadData();
        highScore.text = "HighScore : " + l_name + " : " + l_highScore;
        // istediðimiz bilgileri oluþturulan dosyadan almak istediðimizde buraya yazabiliriz. yada bunun için bir fonksiyon oluþturup o fonksiyonu  burda çaðýrýrýz.
    }
    public void adýekle() //bunu awake in içinde field.onvaluechange.addlisiner(adýekle) diyerek te koyabiliriz. ama ben uniti inspector de ekledim.
    {
        playerName = Field.text;
    }
    public void LoadData() //baþka bir yerde yüklü olan json dosyasýný baþka herhang bir yerden çekmek için json dosyasýnda kayýtlý clasýn adýný kullanarak direk çaðýrabiliriz. aþaðýda olduðu gibi.
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
    //normalde ismi kaydedip daha sonra uygulamayý kapatýp açtýðýmýzda isim kalsýn istersek bu kod ile kayýt iþlemi yapýyoruz. ama bu projede isim kaydý yapmýyacaðýz
    //[System.Serializable]
    //public class SaveData // buraya kaydetmek istediðimiz verileri giriyoruz.
    //{
    //    public string playerName;
    //}
    //public void SaveName()
    //{
    //    SaveData data = new SaveData();
    //    data.playerName = playerName; // burada savedatanýn içindeki playername ile inputfield daki yazdýklarýmýz ile eþliyoruz.

    //    string json = JsonUtility.ToJson(data); // burada json adýnda bir Jsonutility oluþturup data adýný verdiðimiz savedata classýný içine ekliyoruz. json yerine baþka biþey de isimlendireviliriz.
    //    File.WriteAllText(Application.persistentDataPath + "savefile.json", json); //burada dosya oluþturuyoruz. bu sayede oyundan çýksak bile son bilgilerimiz kaydedilecek.
    //                                                                               //Application.persistentDataPath + "savefile.json" nereye kayýt yapýalcaðý json ile yazýlacak þeyin ismini yazýyoruz.
    //}
    public void GameStart ()
    {
        SceneManager.LoadScene(0);
    }
    public void Exit()
    {
        //MainManager.Instance.SaveData(); burda çýkarken ismi kaydetsin diye kod yazdýk ama kayýt yapmýyacaðýmýz için bunu kullanmýyoruz.
#if UNITY_EDITOR //burda seçenekli exit koyduk. eðer unity de uygulamayý açarsak ayrý kod. eðer deðilse kodu baþka yazdýk. buradaki ayrým þu, normal if koysak bilgisayar yine onu kontrol ediyor. burda ise kotrol etmiyor.
        EditorApplication.ExitPlaymode();
#else
        Aplication.Quit();
#endif
    }
}
