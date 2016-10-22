using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using HomeData;

namespace EIBDef
{
    public enum EIB_Adress_Typ  {PhysAdr,GroupAdr};
    public enum APCI_Typ { Request, Answer, Send, unnown };
    public enum EIS_Typ { unknown, EIS1, EIS2, EIS3, EIS4, EIS5, EIS6, EIS7, EIS8, EIS9, EIS10, EIS11, EIS14 };


    /// <summary>
    /// Namen der EIB-Adressen
    /// </summary>
   public enum EIB_Adress_Name
    {
        /////////////////////////////////
        // Hauptgruppe 0 = Fensterkontakte   //
        /////////////////////////////////
        // Mittelgruppe 0 = Untergeschoss
        GA_F_HAR = 0x0001,
        GA_F_AZ = 0x0002,
        GA_F_KE = 0x0003,
        GA_F_V = 0x0004,
        GA_E_HAR = 0x0006,
        GA_E_V1 = 0x0007,
        GA_E_V2 = 0x0008,
        GA_E_AZ = 0x0009,
        GA_AZ_RES2 = 0x000A,
        GA_AZ_RES3 = 0x000B,

        // Mittelgruppe 1 = Erdgeschoss
        GA_F_WZV = 0x0101,
        GA_F_WZH = 0x0102,
        GA_F_KU = 0x0103,
        GA_F_D = 0x0104,
        GA_F_WC = 0x0105,
        GA_T_Haustuer = 0x0106,
        GA_T_Schloss = 0x0107,
        GA_E_WZ1 = 0x0108,
        GA_E_WZ2 = 0x0109,
        GA_E_WZ3 = 0x010A,
        GA_Z_BEW_TER = 0x010B,
        GA_E_WZ5 = 0x010C,

        // Mittelgruppe 2 = Obergeschoss
        GA_F_BAD = 0x0201,
        GA_F_SA = 0x0202,
        GA_F_PA = 0x0203,
        GA_F_SZ = 0x0204,
        GA_E_PA1 = 0x0205,
        GA_E_PA2 = 0x0206,
        GA_E_SA1 = 0x0207,
        GA_E_SA2 = 0x0208,
        // Mittelgruppe 3..7 undefiniert

        /////////////////////////////////
        // Hauptgruppe 1 = Rolladen    //
        /////////////////////////////////
        // Mittelgruppe 0 = Untergeschoss
        GA_R_AZ = 0x0801,
        GA_R_AZ_S = 0x080B,
        GA_R_AZ_G = 0x0815,
        // Mittelgruppe 1 = Erdgeschoss
        GA_R_WZH_A = 0x0901,
        GA_R_WZV_A = 0x0902,
        GA_R_KU_A = 0x0903,
        GA_R_MA_A = 0x0904,
        GA_R_ALLE_A = 0x0905,
        GA_R_WZH_S = 0x090B,
        GA_R_WZV_S = 0x090C,
        GA_R_KU_S = 0x090D,
        GA_R_MA_S = 0x090E,
        GA_R_ALLE_S = 0x090F,
        GA_R_WZH_G = 0x0915,
        GA_R_WZV_G = 0x0916,
        GA_R_KU_G = 0x0917,
        GA_R_MA_G = 0x0918,
        // Mittelgruppe 2 = Obergeschoss
        // Mittelgruppe 3..7 undefiniert

        /////////////////////////////////
        // Hauptgruppe 2 = Strom+Licht //
        /////////////////////////////////
        // Mittelgruppe 0 = Untergeschoss
        GA_Dim3_K1_Schalten = 0x1029,  // 
        GA_Dim3_K2_Schalten = 0x102A,  // 
        GA_Dim3_K1_Dimmen = 0x102B,  // 
        GA_Dim3_K2_Dimmen = 0x102C,  // 
        GA_Dim3_K1_Helligkeit = 0x102D,  // 
        GA_Dim3_K2_Helligkeit = 0x102E,  // 
        GA_Dim3_K1_Rueckmeldung = 0x102F,  // 
        GA_Dim3_K2_Rueckmeldung = 0x1030,  // 
        GA_Dim3_K1_Wertrueckmeldung = 0x1031,  // 
        GA_Dim3_K2_Wertrueckmeldung = 0x1032,  // 
        GA_Dim3_K1_Ueberlast = 0x1033,  // 
        GA_Dim3_K2_Ueberlast = 0x1034,  // 
        GA_Dim3_K1_Spannungsausfall = 0x1035,  // 
        GA_Dim3_K2_Spannungsausfall = 0x1036,  // 



        // Mittelgruppe 1 = Erdgeschoss
        GA_L_TE = 0x1101,
        GA_S_TE = 0x1102,
        GA_Z_WASSER_V = 0x1103,
        GA_Z_KLINGEL1 = 0x1104,
        GA_Z_KLINGEL2 = 0x1105,
        GA_Z_KLINGEL3 = 0x1106,
        GA_Klingelbel = 0x1107,
        GA_L_TE_F = 0x1115,
        GA_S_TE_F = 0x1116,
        GA_Z_WASSER_F = 0x1117,
        GA_Z_KLINGEL_F = 0x1118,


        GA_L_WZH_E = 0x111F,  // 
        GA_L_WZV_E = 0x1120,  // 
        GA_L_WZH_D = 0x1121,  // 
        GA_L_WZV_D = 0x1122,  // 
        GA_L_WZH_H = 0x1123,  // 
        GA_L_WZV_H = 0x1124,  // 
        GA_L_WZH_R = 0x1125,  // 
        GA_L_WZV_R = 0x1126,  // 
        GA_L_WZH_W = 0x1127,  // 
        GA_L_WZV_W = 0x1128,  // 


        GA_L_WZ_LZ1 = 0x1151,
        GA_L_WZ_LZ2 = 0x1152,

        // Mittelgruppe 2 = Obergeschoss
        GA_L_SZ_BETT = 0x1201,  // 
        GA_L_SZ_SCHRANK = 0x1202,  // 
        GA_L_SZ_BETT_R = 0x1203,  // 
        GA_L_SZ_SCHRANK_R = 0x1204,  // 

        GA_Dim2_K1_Schalten = 0x1229,  // 
        GA_Dim2_K2_Schalten = 0x122A,  // 
        GA_Dim2_K1_Dimmen = 0x122B,  // 
        GA_Dim2_K2_Dimmen = 0x122C,  // 
        GA_Dim2_K1_Helligkeit = 0x122D,  // 
        GA_Dim2_K2_Helligkeit = 0x122E,  // 
        GA_Dim2_K1_Rueckmeldung = 0x122F,  // 
        GA_Dim2_K2_Rueckmeldung = 0x1230,  // 
        GA_Dim2_K1_Wertrueckmeldung = 0x1231,  // 
        GA_Dim2_K2_Wertrueckmeldung = 0x1232,  // 
        GA_Dim2_K1_Ueberlast = 0x1233,  // 
        GA_Dim2_K2_Ueberlast = 0x1234,  // 
        GA_Dim2_K1_Spannungsausfall = 0x1235,  // 
        GA_Dim2_K2_Spannungsausfall = 0x1236,  // 



        // Mittelgruppe 3 = Relais
        GA_Z_Tueroeffner = 0x1301,
        GA_L_Eingang = 0x1302,
        GA_Z_Luefter = 0x1303,
        GA_Z_WASSER_H = 0x1304,
        GA_Z_HEBEFIX = 0x1305,
        GA_E_RELAIS6 = 0x1306,
        GA_Z_Zirkulationspumpe = 0x1307,
        GA_S_TUER = 0x1308,

        // Mittelgruppe 4 = Taster
        GA_L_K2_Schalter = 0x1401,
        //GA_Z_EING1_2 = 0x1402,
        //GA_Z_EING1_3 = 0x1403,
        //GA_Z_EING1_4 = 0x1404,
        //GA_Z_EING1_5 = 0x1405,
        //GA_Z_EING1_6 = 0x1406,
        //GA_Z_EING1_7 = 0x1407,
        //GA_Z_EING1_8 = 0x1408,

        // Mittelgruppe 4..6 undefiniert
        // Mittelgruppe 7 Allgemein

        /////////////////////////////////
        // Hauptgruppe 3 = Infomelder  //
        /////////////////////////////////
        // Mittelgruppe 0 = Untergeschoss
        // Mittelgruppe 1 = Erdgeschoss
        // Mittelgruppe 2 = Obergeschoss
        // Mittelgruppe 3 = Sonstiges
        GA_L_SZ_LED_GE = 0x1B01,
        GA_L_SZ_LED_BL = 0x1B02,
        GA_L_SZ_LED_RT = 0x1B03,
        GA_L_SZ_LED_GN = 0x1B04,
        // Mittelgruppe 4..7 undefiniert

        /////////////////////////////////
        // Hauptgruppe 4 = Sonstiges   //
        /////////////////////////////////
        GA_Z_CControl_Start = 0x2000,
        GA_Z_ZEIT = 0x2001,
        GA_Z_WASSERALARM = 0x2002,
        GA_Z_HELLIGKEITS_Schwelle = 0x2005,
        GA_Z_BEW_TUER = 0x2006,
        GA_Z_PIEPER = 0x2007,
        GA_Z_SUMMER = 0x2008,
        GA_Z_TEMP_GS = 0x2009,
        GA_Z_PIEPER_BEF = 0x200B,

        GA_E_DI1 = 0x2102,
        GA_E_DI2 = 0x2103,
        GA_E_DI3 = 0x2104,

        GA_I_FBTEST1 = 0x2401,
        GA_I_FBTEST2 = 0x2402,
        GA_I_FBTEST3 = 0x2403,
        GA_I_FBTEST4 = 0x2404,
        GA_I_FBTEST5 = 0x2405,
        GA_I_FBTEST6 = 0x2406,
        GA_I_FBTEST7 = 0x2407,
        GA_I_FBTEST8 = 0x2408,

        ///////////////////////////////////
        // Hauptgruppe 5 =  Messungen //
        ///////////////////////////////////
        // Mittelgruppe 0 = Leistung
        GA_P_ZAEHLER = 0x2800,
        GA_P_HERD1 = 0x2801,
        GA_P_HERD2 = 0x2802,
        GA_P_HERD3 = 0x2803,
        GA_P_SPM = 0x2804,
        GA_P_KUECHE = 0x2805,
        GA_P_TREPPE = 0x2806,
        GA_P_DIELE = 0x2807,
        GA_P_PV = 0x2808,
        GA_P_TREPPE_UG = 0x2809,
        GA_P_SZ = 0x280A,
        GA_P_WZ = 0x280B,
        GA_P_BAD = 0x280C,
        GA_P_FLUR_OG = 0x280D,
        GA_P_PASCAL = 0x280E,
        GA_P_SANDRA = 0x280F,
        GA_P_AZ = 0x2810,
        GA_P_KE = 0x2811,
        GA_P_HAR = 0x2812,
        GA_P_HEIZUNG = 0x2813,
        GA_P_WM = 0x2814,
        GA_P_HAR_STECKDOSEN = 0x2815,
        GA_P_ROLLADEN = 0x2816,
        GA_P_EIB = 0x2817,
        GA_P_KUEHLGERAETE = 0x2818,

        GA_P_H2O_ZAEHLERSTAND = 0x2864,
        GA_P_SET_H2O_ZAEHLERSTAND = 0x2865,

        // Mittelgruppe 1 = Umweltsensoren
        GA_Z_Aussentemp = 0x2901,
        GA_Z_Helligkeit = 0x2902,


        // Mittelgruppe 2 = Sensoren
        GA_Z_WM_Schalter = 0x2A00,
        GA_Z_PV_Error = 0x2A01,
        GA_Z_PV_ok = 0x2A02,
        GA_Z_PV_R1 = 0x2A0B,
        GA_Z_PV_R2 = 0x2A0C,
        GA_Z_PV_R3 = 0x2A0D,
        GA_Z_PV_R4 = 0x2A0E,

        // Mittelgruppe 3 = Temperaturen
        GA_Z_TEMP1 = 0x2B01,
        GA_Z_TEMP2 = 0x2B02,
        GA_Z_TEMP3 = 0x2B03,
        GA_Z_TEMP4 = 0x2B04,
        GA_Z_TEMP5 = 0x2B05,
        GA_Z_TEMP6 = 0x2B06,


        /////////////////////////////////
        // Hauptgruppe 6 = ??? //
        /////////////////////////////////

        /////////////////////////////////
        // Hauptgruppe 7 = Debugging   //
        /////////////////////////////////
        // Mittelgruppe 0 - 6
        // Mittelgruppe 7 = Dimmer
        GA_Z_RESTART_DIMMER = 0x6f01


    };


    ///<summary >
    ///Definiert eine EIB-Bus Adresse
    ///</summary>
    public class EIB_Adress : IComparable 
    {
        private const int MAX_ADR = 0xFFFF;                         // max. Adr.
        private const int MIN_ADR = 0x0;                            // min. Adr.
        private ushort m_Adr = 0;                                    // Adresse
        private EIB_Adress_Typ m_Typ  = EIB_Adress_Typ.GroupAdr;    // Art der Adresse

        // Konstruktor Gruppenadr. aus Integer anlegen
        public EIB_Adress(int GA)
        {
            if (GA > MAX_ADR) throw new Exception("Gruppenadresse zu gross");
            if (GA < MIN_ADR) throw new Exception("Gruppenadresse darf nicht negativ sein");
            m_Adr = (ushort)GA;
            m_Typ  = EIB_Adress_Typ.GroupAdr;
        }

        // Konstruktor Gruppenadr. oder Physik. Adr aus Integer anlegen
        public EIB_Adress(int Adr,EIB_Adress_Typ Typ)
        {
            if (Adr > MAX_ADR) throw new Exception("zu grosse Adresse");
            if (Adr < MIN_ADR) throw new Exception("Adresse darf nicht negativ sein");
            m_Adr = (ushort)Adr;
            m_Typ = Typ;
        }

        // Konstruktor Gruppenadr. aus Teiladressen erzeugen
        public EIB_Adress(ushort HG, ushort MG, ushort UG)
        {
            Set_GA(HG, MG, UG);
        }

        public EIB_Adress(string EibAdrString)
        {
            EIB_Adress adr = EIB_Adress.Parse(EibAdrString);
            m_Adr = adr.m_Adr;
            m_Typ = adr.m_Typ;
        }

        public EIB_Adress(EIB_Adress_Name name)
        {
            int GA = (int)name;
            if (GA > MAX_ADR) throw new Exception("Gruppenadresse zu gross");
            if (GA < MIN_ADR) throw new Exception("Gruppenadresse darf nicht negativ sein");
            m_Adr = (ushort)GA;
            m_Typ = EIB_Adress_Typ.GroupAdr;
        }

        public static EIB_Adress Parse(string EibAdrString)
        {
            Exception ex = new EndOfStreamException("Falsches Format der EIB-Adresse");
            EIB_Adress e = null;
            char[] delimiterChars = { '/','.' };

            String[] parts = EibAdrString.Split(delimiterChars);
            if (parts.Length != 3) throw ex;
            ushort a = ushort.Parse(parts[0]);
            ushort b = ushort.Parse(parts[1]);
            ushort c = ushort.Parse(parts[2]);

            if (EibAdrString.Contains("/"))
            {   // Gruppenadr
                e = new EIB_Adress(a, b, c);
            }
            else if (EibAdrString.Contains("."))
            {   // Physik. Adr
                e = new EIB_Adress(0, EIB_Adress_Typ.PhysAdr);
                e.Set_PA(a, b, c);
            }
            return e;
        }


        // MSB der Adr abfragen
        public byte MSB
        {
            get
            {
                return (byte)(m_Adr >> 8);
            }
        }

        // LSB der Adr abfragen
        public byte LSB
        {
            get
            {
                return (byte)(m_Adr & 0xFF);
            }
        }
        

        // Adresstyp abfragen
        public EIB_Adress_Typ Typ
        {
            get { return m_Typ; }
        }


        // Gruppenadr als ushort abfragen/setzen
        private ushort GA
        {
            get
            {
                if (m_Typ != EIB_Adress_Typ.GroupAdr) throw new Exception("keine Gruppenadresse");
                return m_Adr;
            }
            set
            {
                m_Adr = value;
                m_Typ  = EIB_Adress_Typ.GroupAdr;
            }
        }

        // Physik. Adr als ushort abfragen/setzen
        private ushort PA
        {
            get
            {
                if (m_Typ != EIB_Adress_Typ.PhysAdr) throw new Exception("keine physikalische Adresse");
                return m_Adr;
            }
            set
            {
                m_Adr = value;
                m_Typ = EIB_Adress_Typ.PhysAdr;
            }
        }

        // Adr als ushort abfragen
        public ushort Adr
        {
            get
            {
                return m_Adr;
            }
        }

        // Hauptgruppe abfragen
        public ushort Hauptgruppe
        {
            get
            {
                if (m_Typ == EIB_Adress_Typ.GroupAdr)
                    return (ushort)((m_Adr & 0xF800) >> 11);
                else
                {
                    throw new Exception("keine Gruppenadresse");
                }
            }
        }

        // Mittelgruppe abfragen
        public ushort Mittelgruppe
        {
            get
            {
                if (m_Typ == EIB_Adress_Typ.GroupAdr)
                    return (ushort)((m_Adr & 0x700) >> 8);
                else
                    throw new Exception("keine Gruppenadresse");
            }
        }

        // Untergruppe abfragen
        public ushort Untergruppe
        {
            get
            {
                if (m_Typ == EIB_Adress_Typ.GroupAdr)
                    return (ushort)(m_Adr & 0xFF);
                else
                    throw new Exception("keine Gruppenadresse");
            }
        }

        // Bereich abfragen
        public ushort Bereich
        {
            get
            {
                if (m_Typ == EIB_Adress_Typ.PhysAdr)
                    return (ushort)((m_Adr & 0xF000) >> 12);
                else
                    throw new Exception("keine physikalische Adresse");
            }
        }

        // Linie abfragen
        public ushort Linie
        {
            get
            {
                if (m_Typ == EIB_Adress_Typ.PhysAdr)
                    return (ushort)((m_Adr & 0xF00) >> 8);
                else
                    throw new Exception("keine physikalische Adresse");
            }
        }

        // Teilnehmer abfragen
        public ushort Teilnehmer
        {
            get
            {
                if (m_Typ == EIB_Adress_Typ.PhysAdr)
                    return (ushort)(m_Adr & 0xFF);
                else
                    throw new Exception("keine physikalische Adresse");
            }
        }


        // Gruppenadr. setzen
        private ushort Set_GA(ushort HG, ushort MG, ushort UG)
        {
            if (UG > 255) throw new Exception("Untergruppe zu gross");
            if (MG > 7) throw new Exception("Untergruppe zu gross");
            if (HG > 31) throw new Exception("Untergruppe zu gross");

            m_Adr = (ushort)((HG << 11) + (MG << 8) + UG);
            m_Typ = EIB_Adress_Typ.GroupAdr;
            return m_Adr;
        }

        // Physikadr. setzen
        private ushort Set_PA(ushort Bereich, ushort Linie, ushort Teilnehmer)
        {
            if (Teilnehmer > 255) throw new Exception("Teilnehmer zu gross");
            if (Linie > 15) throw new Exception("Linie zu gross");
            if (Bereich > 15) throw new Exception("Bereich zu gross");

            m_Adr = (ushort)(( Bereich << 12) + (Linie << 8) + Teilnehmer);
            m_Typ = EIB_Adress_Typ.PhysAdr;
            return m_Adr;
        }


        public String GetName()
        {
            EIB_Adress_Name en = (EIB_Adress_Name)m_Adr;
            return en.ToString();
        }

        // Ausgabe als String
        public override string ToString()
        {
            if (m_Typ == EIB_Adress_Typ.GroupAdr)
            {
                return Hauptgruppe + "/" + Mittelgruppe + "/" + Untergruppe;
            }
            else
            {
                return Bereich + "." + Linie + "." + Teilnehmer;
            }
        }


        // Vergleich zweier Adr.
        public bool Equals(EIB_Adress obj)
        {
            if (obj==null) return false;
            if (m_Typ != obj.m_Typ) return false;
            return m_Adr == obj.m_Adr;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            EIB_Adress otherAdr = obj as EIB_Adress;

            if (m_Typ != otherAdr.m_Typ) return 1;

            if (otherAdr != null)
                return this.m_Adr.CompareTo(otherAdr.m_Adr);
            else
                throw new ArgumentException("Object is not a EIB_Adress"); ;
        }

    }


    ///<summary>
    ///Definiert ein EIB-Bus Telegramm
    ///</summary>
    public class EIB_Telegramm
    {
        private const int EIB_Phys_Source_Adr = 0x1164; // 1.1.100

        private EIB_Adress m_source;                    // physik. Absenderaddr
        private EIB_Adress m_destination;               // Zieladr
        private byte[] m_value ;                        // Inhalt des Telegramme (uninterpretiert)
        private DateTime m_ReceiveTime ;                // Zeit des anlegens
        private int m_DataLen;                          // Datenlänge
        private APCI_Typ m_APCI;                        // APCI-Typ

        // Konstruktor aus empfangenen Rohdaten
        public EIB_Telegramm(byte [] raw)
        {
            m_ReceiveTime = DateTime.Now;
//xxx            if (raw.Length < 10) throw new Exception("Rohdaten zu kurz");
 
            // Quelle eintragen
            m_source = new EIB_Adress((raw[1]<<8)+raw[2],EIB_Adress_Typ.PhysAdr);

            // Ziel eintragen
            //if ((raw[5]&0x80 ) == 0x80)
                m_destination = new EIB_Adress((raw[3] << 8) + raw[4], EIB_Adress_Typ.GroupAdr);
            //else
            //    m_destination = new EIB_Adress((raw[3] << 8) + raw[4], EIB_Adress_Typ.PhysAdr);

            // Datenlänge bestimmen
            m_DataLen = raw[5] & 0x0F;

            // APCI bestimmen
            m_APCI = (APCI_Typ)((raw[7]>>6) & 0x03);

            // Daten kopieren
            m_value = new byte[m_DataLen];
            for (ushort i = 0; i < m_DataLen; i++)
            {
                m_value[i] = raw[i + 7];
            }
            // APCI-Flag aus den Daten ausblenden
            if (m_DataLen>0) m_value[0] =(byte)( (byte)m_value[0] & (byte)0x3F);
        }

 


        // Konstruktor für EIS1 Telegramm: bool
        public EIB_Telegramm(EIB_Adress DestinationAdr,bool value, APCI_Typ apci )
        {
            m_ReceiveTime = DateTime.Now;
            m_source = new EIB_Adress(EIB_Phys_Source_Adr, EIB_Adress_Typ.PhysAdr);
            m_destination = DestinationAdr;
            m_APCI = apci;
            m_DataLen = 1;
            m_value = new byte[m_DataLen];
            if (value) m_value[0] = (byte)1;
            else m_value[0] = (byte)0;
        }


        // Konstruktor für EIS3 Telegramm: Zeit
        public EIB_Telegramm(EIB_Adress DestinationAdr, DateTime time, APCI_Typ apci)
        {
            m_ReceiveTime = DateTime.Now;
            m_source = new EIB_Adress(EIB_Phys_Source_Adr, EIB_Adress_Typ.PhysAdr);
            m_destination = DestinationAdr;
            m_APCI = apci;
            Eis3 = time;
        }



        // Konstruktor für EIS5: float
        public EIB_Telegramm(EIB_Adress DestinationAdr, float value, APCI_Typ apci)
        {
            m_ReceiveTime = DateTime.Now;
            m_source = new EIB_Adress(EIB_Phys_Source_Adr, EIB_Adress_Typ.PhysAdr);
            m_destination = DestinationAdr;
            m_APCI = apci;
            Eis5 = value;
        }

        // Konstruktor für EIS11: uint
        public EIB_Telegramm(EIB_Adress DestinationAdr, uint value, APCI_Typ apci)
        {
            m_ReceiveTime = DateTime.Now;
            m_source = new EIB_Adress(EIB_Phys_Source_Adr, EIB_Adress_Typ.PhysAdr);
            m_destination = DestinationAdr;
            m_APCI = apci;
            Eis11 = value;
        }


        public EIB_Telegramm(HDKnx hdKnx)
        {
            m_ReceiveTime = hdKnx.time;
            m_source = hdKnx.emi.sourceAdr;// new EIB_Adress(EIB_Phys_Source_Adr, EIB_Adress_Typ.PhysAdr);
            m_destination = hdKnx.destAdr;
            m_APCI = hdKnx.emi.APCI;

            m_value = hdKnx.rawValue;
            m_DataLen = hdKnx.emi.DataLen;
           
        }


        // Quelladr abfragen
        public EIB_Adress SourceAdr
        {
            get
            {
                return m_source;
            }
        }


        // Zieladresse abfragen
        public EIB_Adress DestAdr
        {
            get
            {
                return m_destination;
            }
        }


        // Datenlänge abfragen
        public int DataLen
        {
            get
            {
                return m_DataLen;
            }
        }

        public byte[] RawData
        {
            get
            {
                return m_value;
            }
        }


        // APCI abfragen
        public byte apci_byte
        {
            get
            {
                return (byte)(((byte)m_APCI)<<6);
            }
        }

        // APCI abfragen
        public APCI_Typ apci
        {
            get
            {
                return m_APCI;
            }
        }


        // Rohdaten abfragen
        public byte[] value
        {
            get
            {
                return m_value;
            }
        }


        // Empfangszeit abfragen und setzen
        public DateTime ReceiveTime
        {
            get
            {
                return m_ReceiveTime;
            }
            set
            {
                m_ReceiveTime = value;
            }
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
                return m_value[0]==1;
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
                    erg = new DateTime(2000, 1,1, 0, 0, 0); 
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
                if (m_DataLen != 3) return 0.0f;         // keine EIS5
                int inv = m_value[1] * 256 + m_value[2];
                short e, m;

                e = (short)((m_value[1] >> 3) & 0x0f);
                m = (short)(((m_value[1] & 0x07) + ((m_value[1] >> 4) & 0x8)) * 256 + m_value[2]);
                if ((m_value[1]&0x80)==0x80) m = (short)(m - 0x1000);

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

        // Abfrage der Daten in  EIS6-Darstellung (Datum -> DateTime)
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


        // Abfrage der Daten in allen EIS-Darstellung
        public float Wert
        {
            get
            {
                if (m_DataLen == 1) return (float)(Eis1 ? 1.0 : 0.0);
                if (m_DataLen == 2) return (float)Eis6;
                if (m_DataLen == 3) return (float)Eis5;
                if (m_DataLen == 4) return (float)Eis3.TimeOfDay.TotalSeconds;
                if (m_DataLen == 5) return (float)Eis11;
                return -1;
            }
        }



        // Umwandlung der EIS11-Darstellung in eine uint und umgekehrt
        public uint Eis11
        {
            get
            {
                if (m_DataLen != 5) return 0;         // keine EIS11
                return (uint)(m_value[1] * (1 << 24) + m_value[2] * (1 << 16) + m_value[3] * (1 << 8) + m_value[4]);
            }
            set
            {
                m_DataLen = 5;
                m_value = new byte[m_DataLen];

                m_value[0] = 0;
                m_value[1] = (byte)((value >> 24) & 0xFF);
                m_value[2] = (byte)((value >> 16) & 0xFF);
                m_value[3] = (byte)((value >> 8) & 0xFF);
                m_value[4] = (byte)((value) & 0xFF);

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

        // Ausgabe der Rohdaten als String
        private String DataToRaw()
        {
            String erg = "";
            for (ushort i = 0; i < m_DataLen; i++)
            {
                erg += m_value[i].ToString() + " ";
            }
            return erg;
        }

        // Ausgabe als String
        public override string ToString()
        {
            String erg = m_ReceiveTime + " [" + m_source.ToString().PadLeft(9) + "-->" + m_destination.ToString().PadRight(7) + " (" + m_APCI.ToString().PadLeft(7) + ")]: ";
            switch (m_DataLen)
            {
                case 1:  erg= erg + "  EIS1=" + Eis1.ToString().PadRight(7);
                         break;
                case 4:  erg= erg + "  EIS3=" + Eis3.ToString("H:m:s").PadRight(7);
                         erg= erg + "  EIS4=" + Eis4.ToString("d").PadRight(7);
                         break;
                case 5: erg = erg + "  EIS11=" + Eis11.ToString().PadRight(6);
                         break;
                case 3: erg = erg + "  EIS5=" + Eis5.ToString().PadRight(7);
                         break;
                default:
                         break;
            }
            erg = erg + DataToRaw();
            return erg;
        }

        
    }


}  // namespace EIBDef
