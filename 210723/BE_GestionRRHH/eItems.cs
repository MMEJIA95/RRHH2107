using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_GestionRRHH
{
    public class eItems
    {
        private object _data;
        private string _value;
        private string _key;

        public string Key { get => _key; set => _key = value; }
        public string Value { get => _value; set => _value = value; }
        public object Data { get => _data; set => _data = value; }
    }
}
