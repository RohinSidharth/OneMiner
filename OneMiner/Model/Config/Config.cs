﻿using OneMiner.Core.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace OneMiner.Model.Config
{
    class Config
    {
        public DB Data{get;set;}
        Hashtable m_AlgoHash = new Hashtable();
        Hashtable m_MinerHash = new Hashtable();
        Hashtable m_MinerNameHash = new Hashtable();

        private ConfigFileManager m_config_manager = new ConfigFileManager();
        private void InitData()
        {
            Data=new DB();
        }
        private void PopulateData()
        {
            foreach (MinerAlgo item in Data.MinerAlgos)
            {
                m_AlgoHash[item.Name] = item;
            }
            foreach (Miner item in Data.Miners)
            {
                m_MinerHash[item.Id] = item;
                m_MinerNameHash[item.Name] = item;
            }

        }
        public Config()
        {
            try
            {
                InitData();                
                DB d = (DB)new JavaScriptSerializer().Deserialize(m_config_manager.Data, typeof(DB));
                if (d != null)
                {
                    Data = d;
                    PopulateData();
                }

            }
            catch (Exception ex)
            {
            }
        }
        public void Save()
        {
            try
            {
                string data=new JavaScriptSerializer().Serialize(Data);
                m_config_manager.Data = data;
                m_config_manager.WriteData();

            }
            catch (Exception ex)
            {
            }
        }

        public void AddAlgorithms(List<IHashAlgorithm> algorithms)
        {
            try
            {
                if (Data.AddAlgorithms(algorithms))
                    Save();
            }
            catch (Exception e)
            {
            }

        }        
        public void AddMinerProgram(IMinerProgram program)
        {
            try
            {
                if (Data.AddMinerProgram(program))
                    Save();
            }
            catch (Exception e)
            {
            }

        }
        public void AddMiner(IMiner miner)
        {
            try
            {
                if (Data.AddMiner(miner))
                    Save();
            }
            catch (Exception e)
            {
            }

        }
        public bool IsMinernameUnique(string name)
        {
            try
            {
                object item = m_MinerNameHash[name];
                if (item == null)
                    return true;
                return false;
            }
            catch (Exception e)
            {
                return true;
            }

        }
        public string GenerateUniqueID()
        {
            Random rnd = new Random();
            bool unique = false;
            string name = "";
            while (!unique)
            {
                int num = rnd.Next(1, 100);
                name = "Miner" + num.ToString();
                object item = m_MinerNameHash[name];
                if (item == null)
                    unique = true;
            }
            return name;
        }

    }
}
