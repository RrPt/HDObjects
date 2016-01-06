using System;
using System.Collections.Generic;
using System.Text;

namespace HomeData
{
    public class HDObject
    {
        public String name { get; set; }
        public DateTime time { get; set; }
        public String unit { get; set; }
        //private HdAdr sourceAdr;
        //private HdAdr destAdr;

        //public HdAdr SourceAdr
        //{
        //    get { return sourceAdr; }
        //    set { sourceAdr = value; }
        //}

        //public HdAdr DestAdr
        //{
        //    get { return destAdr; }
        //    set { destAdr = value; }
        //}
        



        public override String ToString()
        {
            String erg = time + ": " + name ;
            return erg;

        }

    }
}
