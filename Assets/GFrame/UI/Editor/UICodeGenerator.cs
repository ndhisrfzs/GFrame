using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace GFrame
{
    public class UICodeGenerator
    {
        [MenuItem("Assets/GFrame/Create UICode")]
        static public void CreateUICode()
        {
            //string path = AssetDatabase.GetAssetPath(Selection.activeGameObject);
            m_Instance.CreateCode(Selection.activeGameObject);
        }

        private void CreateCode(GameObject go)
        {
            if (go != null && go.name.StartsWith("UI"))
            {
                m_SelectGameObject = go;

                PrefabType prefabType = PrefabUtility.GetPrefabType(go);
                if (prefabType != PrefabType.Prefab)
                {
                    return;
                }
                GameObject clone = PrefabUtility.InstantiatePrefab(go) as GameObject;
                if (clone == null)
                {
                    return;
                }

                m_DicNameToFullName = new Dictionary<string, string>();
                m_DicNameToTrans = new Dictionary<string, Transform>();
                m_ScriptGeneratePath = UIEditorPathConfig.ScriptGeneratorPath;

                FindAllMarkTrans(clone.transform, clone.transform, "");
                CreateUIBehaviorCode();
                CreateUIComponentsCode();
                //CreateUIFactory();

                AssetDatabase.Refresh();
                GameObject.DestroyImmediate(clone);
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

        private void CreateUIBehaviorCode()
        {
            if (m_SelectGameObject != null)
            {
                string strDlg = m_SelectGameObject.name;
                string strFilePath = string.Format("{0}/{1}.cs", m_ScriptGeneratePath, strDlg);
                if(File.Exists(strFilePath) == false)
                {
                    StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);
                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.AppendLine("using GFrame;")
                        .AppendLine()
                        .AppendFormat("public class {0} : UIBehaviour", strDlg)
                        .AppendLine()
                        .AppendLine("{")
                        .Append("\t").AppendLine("protected override void InitUI()")
                        .Append("\t").AppendLine("{")
                        .Append("\t\t").AppendFormat("mUIComponents = m_IComponents as {0}Components;", strDlg).AppendLine()
                        .Append("\t").AppendLine("}")
                        .AppendLine()
                        .Append("\t").AppendLine("protected override void RegisterUIEvent()")
                        .Append("\t").AppendLine("{")
                        .Append("\t").AppendLine("}")
                        .AppendLine()
                        .Append("\t").AppendLine("protected override void OnShow()")
                        .Append("\t").AppendLine("{")
                        .Append("\t\t").AppendLine("base.OnShow();")
                        .Append("\t").AppendLine("}")
                        .AppendLine()
                        .Append("\t").AppendLine("protected override void OnHide()")
                        .Append("\t").AppendLine("{")
                        .Append("\t\t").AppendLine("base.OnHide();")
                        .Append("\t").AppendLine("}")
                        .AppendLine()
                        .Append("\t").AppendLine("protected override void OnClose()")
                        .Append("\t").AppendLine("{")
                        .Append("\t\t").AppendLine("base.OnClose();")
                        .Append("\t").AppendLine("}")
                        .AppendLine()
                        .Append("\t").AppendLine("protected override void DestoryUI()")
                        .Append("\t").AppendLine("{")
                        .Append("\t\t").AppendLine("base.DestoryUI();")
                        .Append("\t").AppendLine("}")
                        .AppendLine()
                        .Append("\t").AppendLine("void ShowLog(string content)")
                        .Append("\t").AppendLine("{")
                        .Append("\t\t").AppendFormat("UnityEngine.Debug.Log(\"[{0}:]\" + content);", strDlg).AppendLine()
                        .Append("\t").AppendLine("}")
                        .AppendLine()
                        .Append("\t").AppendFormat("{0}Components mUIComponents = null;", strDlg).AppendLine()
                        .AppendLine("}");

                    sw.Write(strBuilder);
                    sw.Flush();
                    sw.Close();
                }
            }
        }

        private void CreateUIComponentsCode()
        {
            if (m_SelectGameObject != null)
            {
                string strDlg = m_SelectGameObject.name;
                string strFilePath = string.Format("{0}/{1}.cs", m_ScriptGeneratePath, strDlg + "Components");

                StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.AppendLine("using GFrame;")
                    .AppendLine("using UnityEngine;")
                    .AppendLine("using UnityEngine.UI;")
                    .AppendLine()
                    .AppendFormat("public class {0}Components : IUIComponents", strDlg)
                    .AppendLine()
                    .AppendLine("{")
                    .Append("\t").AppendLine("public void InitUIComponents()")
                    .Append("\t").AppendLine("{");
                    foreach (KeyValuePair<string, Transform> p in m_DicNameToTrans)
                    {
                        string strUIType = GetUIType(p.Value);
                        strBuilder.Append("\t\t").AppendFormat("{0}_{1} = UIManager.Instance.Get<{2}>(\"{3}\").GetComponent<{4}>();\r\n",
                            p.Key, strUIType, strDlg, m_DicNameToFullName[p.Key], strUIType);
                    }
                strBuilder.Append("\t").AppendLine("}")
                .AppendLine()
                .Append("\t").AppendLine("public void Clear()")
                .Append("\t").AppendLine("{");
                 foreach (KeyValuePair<string, Transform> p in m_DicNameToTrans)
                {
                    string strUIType = GetUIType(p.Value);
                    strBuilder.Append("\t\t").AppendFormat("{0}_{1} = null;", p.Key, strUIType).AppendLine();
                }
                strBuilder.Append("\t").AppendLine("}")
                .AppendLine();

                foreach (KeyValuePair<string, Transform> p in m_DicNameToTrans)
                {
                    string strUIType = GetUIType(p.Value);
                    strBuilder.AppendFormat("\tpublic {0} {1}_{2};", strUIType, p.Key, strUIType).AppendLine();
                }

                strBuilder.AppendLine("}");

                sw.Write(strBuilder);
                sw.Flush();
                sw.Close();
            }
        }

        private void CreateUIFactory()
        {
            if (m_SelectGameObject != null)
            {
                string strFilePath = string.Format("{0}/{1}.cs", m_ScriptGeneratePath, "UIFactory");

                StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.AppendLine("namespace GFrame")
                    .AppendLine("{")
                    .Append("\t").AppendLine("public partial class UIFactory")
                    .Append("\t").AppendLine("{")
                    .Append("\t\t").AppendLine("public override IUIComponents CreateUIComponentsByUIName(string uiName)")
                    .Append("\t\t").AppendLine("{")
                    .Append("\t\t\t").AppendLine("IUIComponents components = null;")
                    .Append("\t\t\t").AppendLine("switch (uiName)")
                    .Append("\t\t\t").AppendLine("{");

                string path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(m_SelectGameObject));
                string[] files = Directory.GetFiles(path, "UI*.prefab", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    strBuilder.Append("\t\t\t\t").AppendFormat("case \"{0}\":", Path.GetFileNameWithoutExtension(files[i])).AppendLine()
                        .Append("\t\t\t\t\t").AppendFormat("components = new {0}Components();", Path.GetFileNameWithoutExtension(files[i])).AppendLine()
                        .Append("\t\t\t\t\t").AppendLine("break;");
                }

                strBuilder.Append("\t\t\t").AppendLine("}")
                    .Append("\t\t\t").AppendLine("return components;")
                    .Append("\t\t").AppendLine("}")
                    .Append("\t").AppendLine("}")
                    .AppendLine("}");

                sw.Write(strBuilder);
                sw.Flush();
                sw.Close();
            }
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

        GameObject m_SelectGameObject = null;
        Dictionary<string, string> m_DicNameToFullName = null;
        Dictionary<string, Transform> m_DicNameToTrans = null;
        static UICodeGenerator m_Instance = new UICodeGenerator();
        string m_ScriptGeneratePath = null;
    }
}