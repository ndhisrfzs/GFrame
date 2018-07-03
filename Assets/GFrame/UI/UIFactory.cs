using System;

namespace GFrame
{
    public partial class UIFactory : Singleton<UIFactory>
    {
        private UIFactory()
        {
        }

        public virtual IUIComponents CreateUIComponentsByUIName(string uiName)
        {
            //throw new Exception("UIFactory need override CreateUIComponentsByUIName function");
            Type type = Type.GetType(uiName + "Components", true, true);
            var obj = Activator.CreateInstance(type);
            return obj as IUIComponents;
        }

        public IUIComponents CreateUIComponents(string uiName)
        {
            return CreateUIComponentsByUIName(uiName);
        }
    }
}
