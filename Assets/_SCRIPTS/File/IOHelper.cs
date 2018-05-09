﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;

public static class IOHelper<T> {

    public static void SerializeObject(T objectToSave, string filePath)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        Stream outStream = new FileStream(
            filePath,
            FileMode.Create,
            FileAccess.Write,
            FileShare.None);
        formatter.Serialize(outStream, objectToSave);

        outStream.Close();
    }

    public static T DeSerializeObject(string filePath)
    {
        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            Stream inStream = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read);
            T obj = (T)formatter.Deserialize(inStream);

            inStream.Close();
            return obj;
        }
        else
            throw new FileNotFoundException();
    }
}
