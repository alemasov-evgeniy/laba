using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Калькулятор_калорий
{
    class Product
    {
        public string name;
        public float kall;
        public float belki;
        public float zhiry;
        public float uglevody;
        public int ves;

        public Product(string name, float kall, float belki, float zhiry, float uglevody)
        {
            this.name = name;
            this.kall = kall;
            this.belki = belki;
            this.zhiry = zhiry;
            this.uglevody = uglevody;
        }

        public float getKolvoElementovNaVes(int ves,float elNa100gr)
        {
            return elNa100gr / 100 * ves;
        }

        public float getKall()
        {
            return getKolvoElementovNaVes(ves, kall);
        }

        override public string ToString()
        {
            return name;
        }
    }
}
