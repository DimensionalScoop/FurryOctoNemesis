using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScoopFramework.Drawing;

namespace ScoopFramework.Interface
{
    class Console
    {

        #region Public
        public void Write(string text){}
        
        public void Write(Sprite icon){}

        public void WriteLine(string text)
        {
            Write(text+"\n");
        }
        
        #endregion
    }
}
