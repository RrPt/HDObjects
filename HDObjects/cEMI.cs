﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using EIBDef;

namespace HomeData
{


    public class cEMI
    {   // Definition der Positionen
        const int msgCode = 0;
        const int adInfoLen=1;
        const int Control1 =2;
        const int control2=3;
        const int sourceAdrH=4;
        const int sourceAdrL=5;
        const int DestAdrH=6;
        const int DestAdrL=7;
        const int dataLen=8;
        const int PosAPDU=9;
        const int PosAPCI = 10;

        // Daten
        EIB_Adress m_source = new EIB_Adress(0);    // physik. Absenderaddr (wird vom Gateway überschrieben)
        EIB_Adress m_destination = null;            // Zieladr
        byte[] m_value = null;                      // Dateninhalt des Telegramms (uninterpretiert)
        DateTime m_ReceiveTime = DateTime.MinValue; // Zeit des anlegens
        int m_DataLen=0;                            // Datenlänge
        APCI_Typ m_APCI = APCI_Typ.unnown;          // APCI-Typ


        /// <summary>
        /// Konstruktor für leeres Telegramm
        /// </summary>
        public cEMI()
        {
            m_value = new byte[15];
        }


        /// <summary>
        /// Konstruktor aus Byte Array
        /// </summary>
        /// <param name="array"></param>
        public cEMI (byte[] array)
        {
            m_ReceiveTime = DateTime.Now;

            try
            {
                // Quelle eintragen
                m_source = new EIB_Adress((array[sourceAdrH] << 8) + array[sourceAdrL], EIB_Adress_Typ.PhysAdr);
                // Ziel eintragen
                if ((array[control2] & 0x80) == 0x80)
                    m_destination = new EIB_Adress((array[DestAdrH] << 8) + array[DestAdrL], EIB_Adress_Typ.GroupAdr);
                else
                    m_destination = new EIB_Adress((array[DestAdrH] << 8) + array[DestAdrL], EIB_Adress_Typ.PhysAdr);
                // Datenlänge
                m_DataLen = array[dataLen]&0x0f;

                // APCI bestimmen
                m_APCI = (APCI_Typ)((array[10] >> 6) & 0x03);

                // Daten kopieren
                m_value = new byte[m_DataLen];
                Array.Copy(array, 10, m_value, 0, m_DataLen);

                for (ushort i = 0; i < m_DataLen; i++)
                {
                    m_value[i] = array[i + 10];
                }
                // APCI-Flag aus den Daten ausblenden
                if (m_DataLen > 0) m_value[0] = (byte)((byte)m_value[0] & (byte)0x3F);

            }
            catch (Exception e)
            {
                throw new Exception("Fehler in cEMI-Konstruktor aus Array",e);
            }

            return ;
        }


        /// <summary>
        /// Konstruktor for EIS1 (bool)
        /// </summary>
        /// <param name="EIB Destination Adress"></param>
        /// <param name="bool"></param>
        public cEMI(EIB_Adress eIB_Adress, bool flag)
        {
            this.m_destination = eIB_Adress;
            m_APCI = APCI_Typ.Send;
            Eis1 = flag;
        }


        public cEMI(EIB_Telegramm tele)
        {
            this.m_destination = tele.DestAdr;
            this.m_APCI = tele.apci;
            this.m_source = tele.SourceAdr;
            m_value = tele.RawData;
            m_DataLen = tele.DataLen;
        }

        /// <summary>
        /// Konstruktoe für EIS2 (Dim)
        /// </summary>
        /// <param name="eIB_Adress"></param>
        /// <param name="flag"></param>
        public cEMI(EIB_Adress eIB_Adress, byte val)
        {
            this.m_destination = eIB_Adress;
            m_APCI = APCI_Typ.Send;
            Eis2 = val;
        }


        /// <summary>
        /// Konstruktor for EIS3 (Zeit)
        /// </summary>
        /// <param name="EIB Destination Adress"></param>
        /// <param name="bool"></param>
        public cEMI(EIB_Adress eIB_Adress, DateTime time)
        {
            this.m_destination = eIB_Adress;
            m_APCI = APCI_Typ.Send;
            Eis3 = time;
        }

        /// <summary>
        /// Konstruktor for EIS3/EIS4 (Datum oder Zeit)
        /// </summary>
        /// <param name="EIB Destination Adress"></param>
        /// <param name="bool"></param>
        public cEMI(EIB_Adress eIB_Adress, DateTime time, bool IsDatum)
        {
            this.m_destination = eIB_Adress;
            m_APCI = APCI_Typ.Send;
            if (IsDatum) Eis4 = time;
            else Eis3 = time;
        }


        /// <summary>
        /// Konstruktor for EIS5 (float)
        /// </summary>
        /// <param name="EIB Destination Adress"></param>
        /// <param name="Float"></param>
        public cEMI(EIB_Adress eIB_Adress, float value)
        {
            this.m_destination = eIB_Adress;
            m_APCI = APCI_Typ.Send;
            Eis5 = (float)value;
        }


        /// <summary>
        /// Konstruktor for EIS11 (long)
        /// </summary>
        /// <param name="EIB Destination Adress"></param>
        /// <param name="value"></param>
        public cEMI(EIB_Adress eIB_Adress, uint value)
        {
            this.m_destination = eIB_Adress;
            m_APCI = APCI_Typ.Send;
            Eis11 = value;
        }


        /// <summary>
        /// Liefert die daten als Byte-Array
        /// </summary>
        /// <returns></returns>
        public byte[] GetTelegramm()
        {

            int size = m_DataLen+10;

            byte[] tmp = new byte[size];
            tmp[0] = 0x11;
            tmp[1] = 0x00;
            tmp[2] = 0xBC;
            tmp[3] = 0xE0;
            tmp[4] = m_source.MSB;
            tmp[5] = m_source.LSB;
            tmp[6] = m_destination.MSB;
            tmp[7] = m_destination.LSB;
            tmp[8] = (byte)m_DataLen;
            tmp[9] = 0x00;
            for (int i = 0; i < m_DataLen; i++)
            {
                tmp[10+i] = m_value[i];
            }


            // APCI setzenbestimmen
            //m_APCI = (APCI_Typ)((array[10] >> 6) & 0x03);
            tmp[PosAPCI] = (byte)(((tmp[PosAPCI])&0x3F) | (int)m_APCI<<6);

            return tmp;
        }



      

        // Ausgabe als String
        public override string ToString()
        {
            String erg = m_ReceiveTime + " [" + m_source.ToString().PadLeft(9) + "-->" + m_destination.ToString().PadRight(7) + " (" + m_APCI.ToString().PadLeft(7) + ")]: ";
            erg = erg + DataToString();
            switch (m_DataLen)
            {
                case 1: erg = erg + "  EIS1=" + Eis1.ToString().PadRight(7) + "  EIS2=" + Eis2.ToString().PadRight(7);
                    break;
                case 2: erg = erg + "  EIS14=" + Eis14.ToString().PadRight(7);
                    break;
                case 3: erg = erg + "  EIS5=" + Eis5.ToString().PadRight(7);
                    break;
                case 4: erg = erg + "  EIS3=" + Eis3.ToString("H:m:s").PadRight(7);
                    erg = erg + "  EIS4=" + Eis4.ToString("d").PadRight(7);
                    break;
                case 5: erg = erg + "  EIS11=" + Eis11.ToString().PadRight(6);
                    break;
                default:
                    break;
            }

            return erg;
        }


        // Ausgabe der Rohdaten als String
        public String DataToString()
        {
            String erg = "";
            for (ushort i = 0; i < m_DataLen; i++)
            {
                erg += m_value[i].ToString("X2") + " ";
            }
            return erg;
        }


        /// <summary>
        /// Abfrage der Quelle 
        /// </summary>
         public EIB_Adress sourceAdr
        {
            get
            {
                return m_source;
            }
            //set
            //{
            //}
        }

         /// <summary>
         /// Abfrage des Ziels 
         /// </summary>
         public EIB_Adress destinationAdr
         {
             get
             {
                 return m_destination;
             }
             //set
             //{
             //}
         }


         /// <summary>
         /// Abfrage des Ziels 
         /// </summary>
         public DateTime receiveTime
         {
             get
             {
                 return m_ReceiveTime;
             }
             //set
             //{
             //}
         }



        // Abfrage der Daten in EIS1-Darstellung (bool)
        public bool Eis1
        {
            get
            {
                if (m_DataLen != 1)
                {  // keine EIS1
                    //throw new Exception("Kein EIS1-Datenformat");
                    //L.err("Kein EIS1-Datenformat");
                    return false;
                }
                return m_value[0] == 1;
            }
            set
            {
                m_DataLen = 1;
                m_value = new byte[m_DataLen];
                if (value)
                    m_value[0] = (byte)( m_value[0] | ((byte)0x01));
                else
                    m_value[0] = (byte)( m_value[0] & (byte)0xf0);
            }
        }


        // Abfrage der Daten in EIS2-Darstellung (bool)
        public byte Eis2
        {
            get
            {
                if (m_DataLen != 1)
                {  // keine EIS2
                    //throw new Exception("Kein EIS1-Datenformat");
                    //L.err("Kein EIS1-Datenformat");
                    return 0;
                }
                return m_value[0] ;
            }
            set
            {
                m_DataLen = 1;
                m_value = new byte[m_DataLen];
                m_value[0] = value ;
            }
        }


        // Abfrage der Daten in  EIS3-Darstellung (Zeit -> DateTime)
        public DateTime Eis3
        {
            get
            {
                if (m_DataLen != 4)
                {   // keine EIS3
                    throw new Exception("Kein EIS3-Datenformat");
                }
                if (m_value[1] > 24) m_value[1] = 0;
                if (m_value[2] > 59) m_value[2] = 0;
                if (m_value[3] > 59) m_value[3] = 0;
                DateTime erg = new DateTime(2000, 1, 1, m_value[1] & 0x1F, m_value[2], m_value[3]);
                return erg;
            }
            set
            {
                m_DataLen = 4;
                m_value = new byte[4];
                m_value[0] = 0;
                m_value[1] = (byte)value.Hour;
                m_value[2] = (byte)value.Minute;
                m_value[3] = (byte)value.Second;
            }
        }

        // Abfrage der Daten in  EIS4-Darstellung (Datum -> DateTime)
        public DateTime Eis4
        {
            get
            {
                DateTime erg;
                if (m_DataLen != 4)
                {   // keine EIS4
                    throw new Exception("Kein EIS4-Datenformat");
                }

                try
                {
                    //if (m_value[2] > 12) m_value[2] = 1;
                    //if (m_value[1] > 31) m_value[1] = 1;
                    //if (m_value[2] < 1) m_value[2] = 1;
                    //if (m_value[1] < 1) m_value[1] = 1;
                    erg = new DateTime(m_value[3] + 2000, m_value[2], m_value[1], 0, 0, 0);

                }
                catch (Exception)
                {
                    erg = new DateTime(2000, 1, 1, 0, 0, 0);
                }
                return erg;
            }
            set
            {
                m_DataLen = 4;
                m_value = new byte[4];
                m_value[0] = 0;
                m_value[1] = (byte)value.Day;
                m_value[2] = (byte)value.Month;
                m_value[3] = (byte)(value.Year-2000);
            }
        }




        // Umwandlung der EIS5-Darstellung in eine Floatzahl und umgekehrt
        public float Eis5
        {
            get
            {
//                if (m_DataLen != 3) return 0.0f;         // keine EIS5
                if (m_DataLen != 3)
                {   // keine EIS5
                    throw new Exception("Kein EIS5-Datenformat");
                }
                int inv = m_value[1] * 256 + m_value[2];
                short e, m;

                e = (short)((m_value[1] >> 3) & 0x0f);
                m = (short)(((m_value[1] & 0x07) + ((m_value[1] >> 4) & 0x8)) * 256 + m_value[2]);
                if ((m_value[1] & 0x80) == 0x80) m = (short)(m - 0x1000);

                return (0.01f * m * (1 << e));
            }
            set
            {
                short e, m;
                float rnd;

                if (value < 0) rnd = -0.5f;
                else rnd = 0.5f;

                if (Math.Abs(value) < 20.0f)
                {  // kein exponent
                    e = 0;
                    m = (short)(value * 100 + rnd);
                }
                else
                {  // mit exponent
                    e = (short)(Math.Floor(1 + Math.Log(Math.Abs(value / 20.0)) / Math.Log(2.0)));
                    if (e > 15) e = 15;
                    m = (short)(100.0f * value / (1 << e) + rnd);
                    if (m > 2047) m = 2047;
                    if (m < -2048) m = -2048;
                }

                m_DataLen = 3;
                m_value = new byte[m_DataLen];

                short ival = (short)((e << 11) + (m & 0x7ff) + ((m & 0x800) << 4));
                m_value[0] = 0;
                m_value[1] = (byte)(ival >> 8);
                m_value[2] = (byte)(ival & 0xFF);

            }

        }

        // Umwandlung der Daten in  EIS6-Darstellung (Byte)
        public byte Eis6
        {
            get
            {
                byte erg;
                if (m_DataLen != 2)
                {   // keine EIS6
                    throw new Exception("Kein EIS6-Datenformat");
                }

                try
                {
                    erg = m_value[1];

                }
                catch (Exception)
                {
                    erg = 0;
                }
                return erg;
            }
            set
            {
                m_DataLen = 2;
                m_value = new byte[2];
                m_value[0] = 0;
                m_value[1] = value;
            }

        }



        // Umwandlung der EIS11-Darstellung in eine uint und umgekehrt
        public uint Eis11
        {
            get
            {
                //if (m_DataLen != 5) return 0;         // keine EIS11
                if (m_DataLen != 5)
                {   // keine EIS11
                    throw new Exception("Kein EIS11-Datenformat");
                }

                return (uint)(m_value[1] * (1 << 24) + m_value[2] * (1 << 16) + m_value[3] * (1 << 8) + m_value[4]);
            }
            set
            {
                m_DataLen = 5;
                m_value = new byte[m_DataLen];
                m_value[0] = 0;
                m_value[1] = (byte)(value>>24);
                m_value[2] = (byte)((value >> 16)&0xFF);
                m_value[3] = (byte)((value >>  8) & 0xFF);
                m_value[4] = (byte)((value ) & 0xFF);

            }

        }


        // Abfrage der Daten in EIS14-Darstellung (byte)
        public byte Eis14
        {
            get
            {
                if (m_DataLen != 2)
                {  // keine EIS14
                    //throw new Exception("Kein EIS14-Datenformat");
                    //L.err("Kein EIS14-Datenformat");
                    return m_value[1];
                }
                return m_value[1];
            }
            set
            {
                m_DataLen = 2;
                m_value = new byte[m_DataLen];
                m_value[0] = 0;
                m_value[1] = value;
            }
        }


        public int DataLen 
        {
            get
            {
                return m_DataLen;
            }
            //set; 
        }



        public APCI_Typ APCI
        {
            get 
            {
                return m_APCI;
            }
            set
            {
                m_APCI = value;
            }
        }


        // Pruft ob das Telegramm dem EIS-Typ entsprechen kann
        public bool IsEisTyp(EIS_Typ type)
        {
            switch (type)
            {
                case EIS_Typ.EIS1:
                    return m_DataLen == 1;
                case EIS_Typ.EIS2:
                    return m_DataLen == 1;
                case EIS_Typ.EIS3:
                    return m_DataLen == 4;
                case EIS_Typ.EIS4:
                    return m_DataLen == 4;
                case EIS_Typ.EIS5:
                    return m_DataLen == 3;
                case EIS_Typ.EIS11:
                    return m_DataLen == 5;
                case EIS_Typ.EIS14:
                    return m_DataLen == 2;
                default:
                    return false;
            }
        }

        


        internal byte[] GetRawData()
        {
            return m_value;
        }


        public override bool Equals(System.Object obj)
        {
            try
            {
                // If parameter is null return false.
                if (obj == null)
                {
                    return false;
                }

                // If parameter cannot be cast to Point return false.
                cEMI p = obj as cEMI;
                if ((System.Object)p == null)
                {
                    return false;
                }

                // Return true if the fields match:
                //Console.WriteLine("Vergleiche: \r\n{0} == \r\n{1}", this, p);
                if (!p.m_source.Equals(m_source)) return false;
                if (!p.m_destination.Equals(m_destination)) return false;
                if (p.DataLen != DataLen) return false;
                if (p.m_APCI != m_APCI) return false;

                for (int i = 0; i < m_value.Length; i++)
                {
                    //Console.WriteLine("Data[{0}]= {1} == {2}", i, m_value[i], p.m_value[i]);
                    if (p.m_value[i] != m_value[i]) return false;
                }

                TimeSpan zeitDiff = p.m_ReceiveTime - receiveTime;
                if (Math.Abs(zeitDiff.TotalMilliseconds) > 500) return false;

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public override int GetHashCode()
        {
            return m_value.GetHashCode();
        }



    }

}
