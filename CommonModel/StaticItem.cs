using System;
using System.Collections.Generic;
using System.Text;

namespace CommonModel
{
    public class StaticItem
    {
        private string _DataValue;
        private string _TextValue;

        public string DataValue
        {
            get { return _DataValue; }
            set { _DataValue = value; }
        }
        public string TextValue
        {
            get { return _TextValue; }
            set { _TextValue = value; }
        }
    }
}
