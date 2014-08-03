using System;
using System.Collections.Generic;
using System.Text;
using HomeData;

namespace Knx
{
    class EIS14 : HDKnx
    {

        public EIS14()
        {
            eisTyp = EIBDef.EIS_Typ.EIS14;
        }

        public EIS14(cEMI emi)
            : base(emi)
        {
            eisTyp = EIBDef.EIS_Typ.EIS14;
            value = emi.Eis14;
        }

        public byte value { get; set; }

        public override void SetValue(cEMI emi)
        {
            base.SetValue(emi);
            value = emi.Eis14;
        }


        public void SetValue(Byte b)
        {
            rawValue = new byte[2];
            rawValue[0] = 0;
            rawValue[1] = b;
        }


        public override String ToString()
        {
            String erg = base.ToString() + "  EIS14 = " + value.ToString();
            return erg;
        }

    }
}
