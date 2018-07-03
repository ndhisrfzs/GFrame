using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GFrame
{
    public abstract class UIComponents : BaseMonoBehaviour, IUIComponents
    {
        public abstract void Clear();
        public abstract void InitUIComponents();
    }
}
