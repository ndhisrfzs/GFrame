using GFrame;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PoolCodeGenerator {
    [MenuItem("Assets/GFrame/Create PoolCode")]
    static public void CreatePoolCode()
    {
        m_Instance.CreateCode(Selection.activeObject);
    }

    private void CreateCode(Object go)
    {
        if(go != null && go.GetType() == typeof(DefaultAsset))
        {
            m_SelectObject = go;

            m_ScriptGeneratePath = IOUtil.CreateDirIfNotExists(UnityEngine.Application.dataPath + "/" + ConfigUtil.LoadConfig<Setting>().PoolScriptGeneratePath);
            CreatePrefabPoolCode();

            AssetDatabase.Refresh();
        }
    }

    private void CreatePrefabPoolCode()
    {
        if (m_SelectObject != null)
        {
            string name = m_SelectObject.name + "Pool";
            string path = AssetDatabase.GetAssetPath(m_SelectObject);

            string strFilePath = string.Format("{0}/{1}.cs", m_ScriptGeneratePath, name);

            StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);
            StringBuilder strBuilder = new StringBuilder();
            string[] files = Directory.GetFiles(path, "*.prefab", SearchOption.AllDirectories);
            strBuilder.AppendLine("using UnityEngine;")
                .AppendLine("using UnityEngine.UI;")
                .AppendLine()
                .AppendLine("namespace GFrame")
                .AppendLine("{")
                .Append("\t").AppendFormat("public class {0} : SpawnPool<{1}>", name, name).AppendLine()
                .Append("\t").AppendLine("{")
                .Append("\t\t").AppendFormat("public {0}()", name).AppendLine()
                .Append("\t\t").AppendLine("{");
            for (int i = 0; i < files.Length; i++)
            {
                string prefabName = Path.GetFileNameWithoutExtension(files[i]);
                strBuilder.Append("\t\t\t").AppendFormat("m_Pools.Add(\"{0}\", new {1}Pool());", prefabName, prefabName).AppendLine();
            }
            strBuilder.Append("\t\t").AppendLine("}")
                .Append("\t").AppendLine("}");

            for (int i = 0; i < files.Length; i++)
            {
                string prefabName = Path.GetFileNameWithoutExtension(files[i]);
                strBuilder.Append("\t").AppendFormat("public class {0}Pool : PrefabPool<{1}>", prefabName, prefabName).AppendLine()
                    .Append("\t").AppendLine("{")
                    .Append("\t\t").AppendLine("public override string PrefabPath()")
                    .Append("\t\t").AppendLine("{")
                    .Append("\t\t\t").AppendFormat("return \"{0}\";", IOUtil.GetResourcePath(files[i])).AppendLine()
                    .Append("\t\t").AppendLine("}")
                    .Append("\t").AppendLine("}");

                m_DicNameToFullName = new Dictionary<string, string>();
                m_DicNameToTrans = new Dictionary<string, Transform>();

                GameObject clone = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(files[i])) as GameObject;
                if(clone == null)
                {
                    continue;
                }

                FindAllMarkTrans(clone.transform, clone.transform, "");
                CreateUIComponentsCode(strBuilder, prefabName);
                Object.DestroyImmediate(clone);
            }
            strBuilder.AppendLine("}");

            sw.Write(strBuilder);
            sw.Flush();
            sw.Close();
        }
    }

    private void FindAllMarkTrans(Transform rootTrans, Transform curTrans, string transFullName)
    {
        for (int i = 0; i < curTrans.childCount; i++)
        {
            Transform childTrans = curTrans.GetChild(i);
            GFrame.UIMark uiMark = childTrans.GetComponent<GFrame.UIMark>();
            if (uiMark != null)
            {
                if (!m_DicNameToTrans.ContainsKey(childTrans.name))
                {
                    m_DicNameToTrans.Add(childTrans.name, childTrans);
                    m_DicNameToFullName.Add(childTrans.name, transFullName + childTrans.name);
                }
                else
                {
                    Debug.LogError("Repeat Key:" + childTrans.name);
                }
            }

            FindAllMarkTrans(rootTrans, childTrans, transFullName + childTrans.name + "/");
        }
    }

    private void CreateUIComponentsCode(StringBuilder strBuilder, string strDlg)
    {
        strBuilder.Append("\t").AppendFormat("public class {0} : UIComponents", strDlg).AppendLine()
            .Append("\t").AppendLine("{")
            .Append("\t\t").AppendLine("public override void InitUIComponents()")
            .Append("\t\t").AppendLine("{");
        foreach (KeyValuePair<string, Transform> p in m_DicNameToTrans)
        {
            string strUIType = GetUIType(p.Value);
            strBuilder.Append("\t\t\t").AppendFormat("{0}_{1} = transform.Find(\"{2}\").GetComponent<{3}>();\r\n",
                p.Key, strUIType, m_DicNameToFullName[p.Key], strUIType);
        }
        strBuilder.Append("\t\t").AppendLine("}")
        .Append("\t\t").AppendLine("public override void Clear()")
        .Append("\t\t").AppendLine("{");
        foreach (KeyValuePair<string, Transform> p in m_DicNameToTrans)
        {
            string strUIType = GetUIType(p.Value);
            strBuilder.Append("\t\t\t").AppendFormat("{0}_{1} = null;", p.Key, strUIType).AppendLine();
        }
        strBuilder.Append("\t\t").AppendLine("}");

        foreach (KeyValuePair<string, Transform> p in m_DicNameToTrans)
        {
            string strUIType = GetUIType(p.Value);
            strBuilder.AppendFormat("\t\tpublic {0} {1}_{2};", strUIType, p.Key, strUIType).AppendLine();
        }

        strBuilder.Append("\t").AppendLine("}");
    } 

    private string GetUIType(Transform trans)
    {
        if (null != trans.GetComponent<Text>())
            return "Text";
        else if (null != trans.GetComponent<InputField>())
            return "InputField";
        else if (null != trans.GetComponent<Button>())
            return "Button";
        else if (null != trans.GetComponent<Image>())
            return "Image";
        else if (null != trans.GetComponent<RawImage>())
            return "RawImage";
        else if (null != trans.GetComponent<Toggle>())
            return "Toggle";

        else if (null != trans.GetComponent<Slider>())
            return "Slider";
        else if (null != trans.GetComponent<Scrollbar>())
            return "Scrollbar";
        else
            return "Transform";
    }

    Object m_SelectObject = null;
    Dictionary<string, string> m_DicNameToFullName = null;
    Dictionary<string, Transform> m_DicNameToTrans = null;
    static PoolCodeGenerator m_Instance = new PoolCodeGenerator();
    string m_ScriptGeneratePath = null;
}
