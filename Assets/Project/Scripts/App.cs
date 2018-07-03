using HHW.Service;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace GFrame
{
    public class App : MonoBehaviour
    {
        public TextAsset KP;
        IEnumerator Start()
        {
            //Config.Instance.KeyWithLogin = CalcBytes.ConvertToUlongs(KP.bytes);
            //Config.Instance.KeyWithGame = CalcBytes.ConvertToUlongs2(KP.bytes);
            //NetworkManager.Instance.Add(new LoginClient(keyWithLogin));
            //NetworkManager.Instance.Add(new GameClient(keyWithGame));
            SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

            EventSystem.Add(DLLType.Model, typeof(App).Assembly);

            Game.Scene.AddComponent<OpcodeTypeComponent>();
            Game.Scene.AddComponent<NetOuterComponent>();

            UIManager.Instance.SetResolution(1136, 640);
            UIManager.Instance.SetMatchOnWidthOrHeight(0);

            UIManager.Instance.OpenUI<UILoginBg>(UILevel.Bg);
            UIManager.Instance.OpenUI<UILoginDialog>(UILevel.Common);
            yield return null;
        }

        private void Update()
        {
            OneThreadSynchronizationContext.Instance.Update();
            EventSystem.Update();
        }
    }
}
