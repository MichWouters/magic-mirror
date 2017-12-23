using System;
using System.Collections.Generic;
using System.Text;

namespace MagicMirror.DataAccess.Entities.Entities
{
    public class Compliment
    {
        private int complimentId;

        public int ComplimentId
        {
            get { return complimentId; }
            set { complimentId = value; }
        }

        private string _value;

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}