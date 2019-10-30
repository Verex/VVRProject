using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class RotationsLoadedEvent : UnityEvent<List<Vector3>> { }

public class IORotationStreamer : MonoBehaviour
{
    enum RotationAxis { X, Y, Z }

    [Tooltip("File path relative to OS application data folder."), SerializeField]
    string relativeFilePath;
    [SerializeField] int rotationCount;
    [SerializeField] RotationAxis sortingAxis;

    public RotationsLoadedEvent OnRotationsLoaded;

    void Start()
    {
        List<Vector3> rotations;

        // Get relevant paths.
        string applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            rotationsFilePath = applicationDataPath + System.IO.Path.DirectorySeparatorChar + relativeFilePath.Replace('/', System.IO.Path.DirectorySeparatorChar);

        if (File.Exists(rotationsFilePath))
        {
            rotations = new List<Vector3>();

            try
            {
                string line;

                // Open stream with rotations data file.
                StreamReader sr = new StreamReader(rotationsFilePath);

                // Read all lines and parse rotations into list.
                while ((line = sr.ReadLine()) != null)
                {
                    string[] components = line.Split(',');

                    if (components.Length == 3)
                    {
                        rotations.Add(new Vector3(float.Parse(components[0]), float.Parse(components[1]), float.Parse(components[2])));
                    }
                }

                sr.Close();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            // Note: Could sort rotations here instead...
        }
        else
        {
            rotations = GetRandomRotations(rotationCount);

            try
            {
                // Create directory for rotation data.
                Directory.CreateDirectory(Path.GetDirectoryName(rotationsFilePath));

                // Create new file and open streamwriter.
                StreamWriter sw = new StreamWriter(File.Create(rotationsFilePath));

                // Write each rotation to file.
                foreach (Vector3 rotation in rotations)
                {
                    sw.WriteLine(rotation.x + "," + rotation.y + "," + rotation.z);
                }

                sw.Close();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        /*
            Sort rotations based on selected axis.

            Note: This was originally going to be executed ONLY after
            loading from file (based off instructions). Not sure if
            we're supposed to sort after creating the file AND after
            loading it.
        */
        rotations.Sort((Vector3 x, Vector3 y) =>
        {
            return x[(int)sortingAxis].CompareTo(y[(int)sortingAxis]);
        });

        OnRotationsLoaded.Invoke(rotations);
    }

    public static List<Vector3> GetRandomRotations(int count)
    {
        List<Vector3> rotations = new List<Vector3>();

        // Add random rotations to list. 
        for (int i = 0; i < count; i++)
        {
            rotations.Add(UnityEngine.Random.rotationUniform.eulerAngles);
        }

        return rotations;
    }
}
