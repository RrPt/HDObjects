﻿using System;
using System.Collections.Generic;
using System.Text;
using EIBDef;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using Knx;
using HDObjects;

namespace HomeData
{
    class EisConvert<T> where T : HDKnx, new()
    {
        internal static T Make()
        {
            T obj = new T();
            return obj;

        }
    }


    [Serializable]
    public class XmlEibItem
    {
        public String name = "nix";
        public String EibAdress = "0/0/0";
        public String EisName = null;
        public String Typ = "Leistung";
        public String unit = "?";
    }

    [Serializable]
    [XmlRoot("EibItemList")]
    public class XmlEibItemList
    {
        [XmlElement(ElementName = "EibItem")]
        public List<XmlEibItem> list = new List<XmlEibItem>();

        public XmlEibItemList ()
	        {
	        }
    }


    public class HDKnxHandler
    {

        static SortedList<EIB_Adress,HDKnx> hdKnxObjList = new SortedList<EIB_Adress,HDKnx>();


        public static void WriteParametersToFile(string XmlFileName)
        {
            XmlEibItemList list = new XmlEibItemList();
            foreach (HDKnx hdKnx in hdKnxObjList.Values)
            {
                XmlEibItem p = new XmlEibItem();
                p.name = hdKnx.name;
                p.EisName = hdKnx.GetType().ToString();
                //@todo xxx p.Typ = hdKnx.GetType();
                p.unit = "W";
                p.EibAdress = hdKnx.destAdr.ToString();
                list.list.Add(p);
            }

            try
            {
                XmlSerializer mySerializer = new XmlSerializer(typeof(XmlEibItemList));
                StreamWriter myWriter = new StreamWriter(XmlFileName);
                mySerializer.Serialize(myWriter, list);
                myWriter.Close();
            }
            catch (Exception eX)
            {
                HDDebug.WriteLine(eX.ToString());
            }

        }


        public static void ReadParametersFromFile(string XmlFileName)
        {
            XmlEibItemList list; 
            FileStream fs = null;
            try
            {
                XmlSerializer mySerializer = new XmlSerializer(typeof(XmlEibItemList));
                fs = new FileStream(XmlFileName, FileMode.Open);
                if (fs == null) return ;
                XmlReader reader = new XmlTextReader(fs);
                list = (XmlEibItemList)mySerializer.Deserialize(reader);

                foreach (XmlEibItem i in list.list)
                {
                    Type ttt = Type.GetType(i.EisName);
                    HDKnx o = (HDKnx)Activator.CreateInstance(ttt);

                    o.name =  i.name;
                    o.destAdr = new EIB_Adress(i.EibAdress);
                    //o.sourceAdr = new EIB_Adress(0, EIB_Adress_Typ.PhysAdr);
                    o.unit = i.unit;

                    if (!hdKnxObjList.ContainsKey(o.destAdr))
                    {
                        hdKnxObjList.Add(o.destAdr, o);
                    }
                }

                return ;
            }
            catch (FileNotFoundException fnfeX)
            {
                list = null;
                HDDebug.WriteLine(fnfeX.ToString());
                return ;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }




        public static HDKnx GetObject(cEMI emi)
        {
            // Suchen des passenden Objektes
            if (hdKnxObjList.ContainsKey(emi.destinationAdr))
            {
                return hdKnxObjList[emi.destinationAdr];
            }
            // Wenn keines gefunden, dann eines anlegen
            HDKnx hdKnx = null;
            switch (emi.DataLen)
            {
                case 1:
                    hdKnx = new EIS1(emi);
                    break;
                case 2:
                    hdKnx = new EIS14(emi);
                    break;
                case 3:
                    hdKnx = new EIS5(emi);
                    break;
                case 4:
                    hdKnx = new EIS3(emi);
                    break;
                case 5:
                    hdKnx = new EIS11(emi);
                    break;
                default:
                    hdKnx = null;
                    break;
            }
            hdKnxObjList.Add(emi.destinationAdr, hdKnx);
            return hdKnx;

        }


        public static void Load()
        {
            HDKnxHandler.ReadParametersFromFile("KnxClient.xml");
        }

    }
}
