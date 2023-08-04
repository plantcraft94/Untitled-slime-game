using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class Save_load_system
{
    public static void Save(int data)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/savefile.dat";
        FileStream stream= new FileStream(path, FileMode.Create);      

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static int Load()
    {

        string path = Application.persistentDataPath + "/savefile.dat";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            int data = (int)formatter.Deserialize(stream);
            stream.Close();

            return data;
        }
        else
        {
            return 0;
        }
    }
}