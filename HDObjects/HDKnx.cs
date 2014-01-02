using System;
using System.Collections.Generic;
using System.Text;
using EIBDef;

namespace Knx
{
    public class HDKnx : HDObject
    {
        private EIB_Adress m_destAdr;
        protected EIS_Typ eisTyp = EIS_Typ.unknown;
        private cEMI emi;

        public HDKnx()
        {

        }

        public HDKnx(cEMI emi)
        {
            this.emi = emi;
            //m_sourceAdr = emi.sourceAdr;
            m_destAdr = emi.destinationAdr;
            time = emi.receiveTime;
            name = "auto_" + time.ToShortTimeString();
            rawValue = emi.GetRawData();
        }

        /// <summary>
        /// Zieladresse
        /// </summary>
        public EIB_Adress destAdr
        {
            get { return m_destAdr; }
            set { m_destAdr = value; }
        }


        public byte[] rawValue { get; set; }

        // Ausgabe der Rohdaten als String
        private String DataToString()
        {
            String erg = "";
            for (ushort i = 0; i < rawValue.Length; i++)
            {
                erg += rawValue[i].ToString() + " ";
            }
            return erg;
        }


        public override String ToString()
        {
            String erg = time +": " + name + " [" + m_destAdr.ToString().PadRight(7) + "] " + DataToString();
            return erg;

        }

        /// <summary>
        /// Setzt den RAW Wert aus einem Telegramm
        /// </summary>
        /// <param name="emi"></param>
        public virtual void SetValue(cEMI emi)
        {
            time = emi.receiveTime;
            rawValue = emi.GetRawData();
        }

        /// <summary>
        /// Setzt den RAW Wert 
        /// </summary>
        /// <param name="rawData">Rohdaten als Byte Array</param>
        public virtual void SetValue(byte[] rawData)
        {
            time = emi.receiveTime;
            rawValue = rawData;
        }


    }
}
