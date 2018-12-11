﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using Utilities;

using U5kBaseDatos;

namespace U5kManServer
{

    /// <summary>
    /// 
    /// </summary>
    // public enum eTiposInci { TEH_TOP = 0, TEH_TIFX = 1, TEH_EXTERNO_RADIO, TEH_EXTERNO_TELEFONIA, TEH_SISTEMA, TEH_RADIOHF, TEH_RADIOMN, TEH_RECORDER }

    /// <summary>
    /// 
    /// </summary>
    public class HistThread : NucleoGeneric.NGThread
    {
        /** 20170802. Para que puedan acceder todos... */
        public static HistThread hproc = null;

        /// <summary>
        /// 
        /// </summary>
        bool _init = false;
        private static U5kBdtService _bdt = null;

        /// <summary>
        /// 
        /// </summary>
        Queue<U5kIncidencia> _incidencias = new Queue<U5kIncidencia>();        //
        static List<U5kIncidenciaDescr> _inciDescr = new List<U5kIncidenciaDescr>();

        // DateTime _hoy = DateTime.MinValue;
        DateTime _hoy = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        public HistThread()
        {
            Name = "HistThread";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inci"></param>
        StoreFilter _inciFilter = new StoreFilter();
        public void AddInci(DateTime fecha, int scv, eIncidencias inci, int thw, string idhw, params object[] parametros)
        {
            lock (_incidencias)
            {
                string desc;
                try
                {
                    if (_inciDescr.Where(rr => rr.id == (int)inci).Count() > 0)
                    {
                        /** */
                        int npar = parametros.Length;
                        Array.Resize(ref parametros, 8);
                        for (int i = npar; i < 8; i++)
                            parametros[i] = "--";

                        desc = string.Format(_inciDescr.Where(rr => rr.id == (int)inci).First().strformat, parametros);

                        U5kIncidencia tinci = new U5kIncidencia()
                        {
                            fecha = fecha,
                            id = (int)inci,
                            tipo = thw,
                            idhw = idhw,
                            sistema = Properties.u5kManServer.Default.stringSistema,
                            desc = desc,
                            scv = scv,
                            reconocida = DateTime.MinValue
                        };

                        if (_inciFilter.StoreFilterPass(tinci) == true)
                        {
                            _incidencias.Enqueue(tinci);
                        }
                    }
                }
                catch (Exception x)
                {
                    LogException<HistThread>("", x);
                    throw x;
                }
            }
        }
        /// <summary>
        /// 20180827. Para que se pueda volver a recargar las incidencias...
        /// </summary>
        public void Reset()
        {
            lock (_incidencias)
            {
                _init = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Run()
        {
            U5kGenericos.TraceCurrentThread(this.GetType().Name);

            LogInfo<HistThread>("HistThread, arrancado...");
            using (timer = new TaskTimer(new TimeSpan(0, 0, 0, 0, 100), this.Cancel))
            {
                // Procesos.
                while (IsRunning())
                {
                    try
                    {
                        if (_init == false)
                            _init = Init();
                        else
                        {

                            lock (_incidencias)
                            {
                                if (U5kManService._Master == true)
                                {
                                    SupervisaCambioDeDia();
                                }
                                
                                if (_incidencias.Count > 0)
                                {
                                    U5kIncidencia inci = _incidencias.Dequeue();
                                    StoreInci(inci);
                                }
                            }
                        }
                    }
                    catch (Exception x)
                    {
                        if (x is ThreadAbortException)
                        {
                            Thread.ResetAbort();
                            LogDebug<HistThread>("HistThread. Abortando...");
                            break;
                        }
                        else
                        {
                            LogException<HistThread>("", x);
                        }
                    }
                    GoToSleepInTimer();
                }
            }
            /** Almacenar las Incidencias Pendientes */
            foreach (U5kIncidencia inci in _incidencias)
            {
                try
                {
                    StoreInci(inci);
                }
                catch (Exception x)
                {
                    LogException<HistThread>("", x);
                }
            }
            Dispose();
            LogInfo<HistThread>("Finalizado...");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool Init()
        {
            Properties.u5kManServer cfg = Properties.u5kManServer.Default;
            bool ret = false;

            lock (_incidencias)
            {
                if (Properties.u5kManServer.Default.TipoBdt == 1)
                    _bdt = new U5kBdtService(Thread.CurrentThread.CurrentUICulture, eBdt.bdtSqlite, cfg.SQLitePath);
                else
                    _bdt = new U5kBdtService(Thread.CurrentThread.CurrentUICulture, eBdt.bdtMySql, cfg.MySqlServer, cfg.MySqlUser, cfg.MySqlPwd);

                _inciDescr = _bdt.ListaDeIncidencias(Properties.u5kManServer.Default.FiltroIncidencias);
                ret = true;
            }
            LogDebug<HistThread>("Leidas Incidencias....");
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inci"></param>
        /// <returns></returns>
        string strInci(U5kIncidencia inci)
        {
            return string.Format("{0}: T:{1}, S:{2}-{3}, id:{4}, {5}--{6}", inci.fecha, inci.tipo, inci.sistema, inci.scv, inci.id, inci.idhw, inci.desc);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inci"></param>
        /// <returns></returns>
        string strInciNoFecha(U5kIncidencia inci)
        {
            //return string.Format("({2}) {0}> {1}", inci.idhw, inci.desc, inci.id);
            return string.Format("{0}>: {1}", inci.idhw, inci.desc, inci.id);
        }
        string strDbgInci(U5kIncidencia inci)
        {
            return string.Format("[{0:D4}]. {1,-8} > {2}", inci.id, inci.idhw, inci.desc);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strInci"></param>
        /// <returns></returns>
        public static string[] strInci2Descr(string strInci)
        {
            return strInci.Split(new string[] { ">: " }, StringSplitOptions.None);
        }
        /// <summary>
        /// 
        /// </summary>
        void SupervisaCambioDeDia()
        {
            if (_hoy.Day != DateTime.Today.Day)
            {
                _hoy = DateTime.Today;

                _bdt.SupervisaTablaIncidencia(U5kManService.cfgSettings/* Properties.u5kManServer.Default*/.DiasEnHistorico);

                /** Genero Incidencia Cambio de Dia */
                U5kIncidencia tinci = new U5kIncidencia()
                {
                    fecha = DateTime.Now + new TimeSpan(0, 1, 0),             // Añado un minuto para evitar problemas de filtro en 00:00:00
                    id = (int)eIncidencias.IGRL_CAMBIO_DE_DIA,
                    tipo = (int)eTiposInci.TEH_SISTEMA,
                    idhw = "SPV",
                    sistema = Properties.u5kManServer.Default.stringSistema,
                    desc = string.Format(_inciDescr.Where(rr => rr.id == (int)eIncidencias.IGRL_CAMBIO_DE_DIA).First().strformat),
                    scv = 0,
                    reconocida = DateTime.MinValue
                };
                _incidencias.Enqueue(tinci);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inci"></param>
        void StoreInci(U5kIncidencia inci)
        {
#if DEBUG_1
            TestDBClose();
#endif
            if (U5kManService._Master == true)
            {
                bool _alarma = _inciDescr.Where(e => e.id == inci.id).First().alarm;
                LogDebug<HistThread>(strDbgInci(inci)/*strInci(inci)*/);

                // Inserto en la BDT
                if (U5kManService.cfgSettings/*Properties.u5kManServer.Default*/.GenerarHistoricos == true)
                {
                    _bdt.InsertaIncidencia(_alarma, inci);
                    if (_alarma)
                        U5kManService.AddInci(inci.fecha, strInciNoFecha(inci));
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        class StoreFilter
        {
            class StoreFilteData
            {
                public DateTime timestamp { get; set; }
                public Int32 repeats { get; set; }
            }
            public StoreFilter()
            {
            }
            //private  bool ToStore(U5kIncidencia inci)
            //{
            //    /** Control de tipo de evento */
            //    if (StoreFilterPass(inci) == false)
            //    {
            //        return false;
            //    }

            //    /** Control de repetición */
            //    string key = String.Format("{0}_{1}_{2}", inci.id, inci.idhw, inci.desc);
            //    bool bStore = true;
            //    DateTime now = DateTime.Now;
            //    Int32 repeats = 1;

            //    if (_control.ContainsKey(key))
            //    {
            //        TimeSpan elapsed = now - _control[key].timestamp;
            //        bStore = elapsed > TimeSpan.FromSeconds(30);            // Solo se gestiona el almacenamiento si no se ha repetido en 30 segundos...
            //        repeats = bStore ? 1 : _control[key].repeats + 1;
            //    }
            //    /** Aviso para evento que se está repitiendo */
            //    if (repeats == 20)
            //    {
            //    }

            //    _control[key] = new StoreFilteData() { timestamp = now, repeats = repeats };

            //    /** Limpiar los eventos que no se repiten... */
            //    CleanOld(now);
            //    return bStore;
            //}

            /// <summary>
            /// Filtros de Incidencias.
            /// </summary>
            /// <param name="inci"></param>
            /// <returns></returns>
            public bool StoreFilterPass(U5kIncidencia inci)
            {
                if (U5kManService.cfgSettings/* Properties.u5kManServer.Default*/.Historico_PttSqhOnBdt == false)
                {
                    if (inci.id == (int)eIncidencias.ITO_PTT)      // PTT de Puesto.
                        return false;
                    if (inci.id == (int)eIncidencias.IGW_EVENTO)    // Evento de Pasarela.
                    {
                        if (inci.desc.ToLower().Contains("ptt") ||
                            inci.desc.ToLower().Contains("sqh"))
                            return false;
                    }
                }
                return true;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="now"></param>
            void CleanOld(DateTime now)
            {
                TimeSpan elapsed = now - lastClean;
                if (elapsed > TimeSpan.FromMinutes(30))
                {
                    var keysForClean = (from item in _control
                                        where now - item.Value.timestamp > TimeSpan.FromMinutes(5)
                                        select item.Key).ToList();

                    keysForClean.ForEach((key) => _control.Remove(key));
                    lastClean = now;
                }
            }

            Dictionary<string, StoreFilteData> _control = new Dictionary<string, StoreFilteData>();
            DateTime lastClean = DateTime.Now;
        }

#if DEBUG
        static int tcount = 0;
        void TestDBClose()
        {
            tcount++;
            if ((tcount % 10) == 0)
            {
                _bdt.dbClose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        static Random rnd = new Random();
        static List<System.Tuple<int, int, eTiposInci>> grpInci = new List<System.Tuple<int, int, eTiposInci>>()
        {
            new System.Tuple<int, int, eTiposInci>(1,50,eTiposInci.TEH_RADIOHF),
            new System.Tuple<int, int, eTiposInci>(51,1000,eTiposInci.TEH_SISTEMA),
            new System.Tuple<int, int, eTiposInci>(1001,2000,eTiposInci.TEH_TOP),
            new System.Tuple<int, int, eTiposInci>(2001,3000,eTiposInci.TEH_TIFX),
            new System.Tuple<int, int, eTiposInci>(3001,3049,eTiposInci.TEH_EXTERNO_RADIO),
            new System.Tuple<int, int, eTiposInci>(3005,3200,eTiposInci.TEH_RADIOMN)
        };
        public static void TestAddInci(int grpIndex = 0, bool alarmas = false)
        {
            System.Tuple<int, int, eTiposInci> grp = grpInci[grpIndex];
            List<U5kIncidenciaDescr> lista = _inciDescr.Where(i => (i.id >= grp.Item1 && i.id < grp.Item2) && (alarmas == false || (alarmas == true && i.alarm == true))).ToList();
            int iIndex = rnd.Next(0, lista.Count);
            object[] parametros = {
                "TEST " + grp.Item3.ToString() + " PAR-01",
                "TEST " + grp.Item3.ToString() + " PAR-02",
                "TEST " + grp.Item3.ToString() + " PAR-03",
                "TEST " + grp.Item3.ToString() + " PAR-04"
            };
            U5kManService._main.GeneraIncidencia(0, (U5kBaseDatos.eIncidencias)lista[iIndex].id, grp.Item3, "TEST", parametros);
        }

#endif

    }
}
