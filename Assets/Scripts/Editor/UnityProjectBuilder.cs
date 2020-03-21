using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnityProjectBuilder
{
    static string[] GetBuildScenes()
    {
        List<string> names = new List<string>();

        foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
        {
            if (e == null)
                continue;

            if (e.enabled)
                names.Add(e.path);
        }
        return names.ToArray();
    }

    /// <summary>
    /// 此方法是从jienkins上接受  数据的 方法
    /// </summary>
    [MenuItem("Tool/APKBuild")]
    static void CommandLineBuild()
    {
        try
        {
            Debug.Log("Command line build\n------------------\n------------------");
            string[] scenes = GetBuildScenes();
            //string path = @"E:\Unity游戏包\Android\消消乐游戏";//这里的路径是打包的路径， 定义
            string path = GetJenkinsParameter("BuildPath");
            //path = Application.persistentDataPath + "/Game";
            Debug.Log(path);
            for (int i = 0; i < scenes.Length; ++i)
            {
                Debug.Log(string.Format("Scene[{0}]: \"{1}\"", i, scenes[i]));
            }
            // ProjectPackageEditor.BuildByJenkins(GetJenkinsParameter("Platform"), GetJenkinsParameter("AppID"), GetJenkinsParameter("Version"), GetJenkinsParameter("IPAddress"));
            Debug.Log("Starting Build!");
            Debug.Log(GetJenkinsParameter("Platform"));

            string platform = GetJenkinsParameter("Platform");
            //platform = "Android";
            if (platform == "Android")
            {
                BuildPipeline.BuildPlayer(scenes, path + ".apk", BuildTarget.Android, BuildOptions.None);
            }
            else if (platform == "IOS")
            {
                //BuildPipeline.BuildPlayer(scenes, path, BuildTarget.iOS, BuildOptions.AcceptExternalModificationsToPlayer);
            }
            else if (platform == "Window64")
            {
                BuildPipeline.BuildPlayer(scenes, path + ".exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
            }
        }
        catch (Exception err)
        {
            Console.WriteLine("方法F中捕捉到：" + err.Message);
            throw; //重新抛出当前正在由catch块处理的异常err
        }
        finally
        {
            Debug.Log("---------->  I am copying!   <--------------");
        }
    }


    /// <summary>
    ///解释jekins 传输的参数
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    static string GetJenkinsParameter(string name)
    {
        /*下面是解析这个字符串，用空格分隔，每一个arg是空格分隔后的字符串，然后继续解析出Platform参数和BuildPath参数，
 注意：$BuildPath是我们Jenkins上定义的参数，它会传入一个字符串路径
 -quit -batchmode -projectPath D:\下载文件\游戏蛮牛源码\一个消消乐工程 -executeMethod UnityProjectBuilder.CommandLineBuild -logFile JenkinsBuildUnity.log Platform-$Platform BuildPath-$BuildPath
 */
        foreach (string arg in Environment.GetCommandLineArgs())
        {
            Debug.Log("arg:" + arg);
            if (arg.StartsWith(name))
            {
                return arg.Split("-"[0])[1];
            }
        }
        return null;
    }
}