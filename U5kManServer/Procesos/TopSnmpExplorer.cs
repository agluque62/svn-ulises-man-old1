﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Net;

using System.Threading;
using System.Threading.Tasks;
using U5kBaseDatos;
using Utilities;

using Lextm.SharpSnmpLib;
using NucleoGeneric;

namespace U5kManServer
{
    /// <summary>
    /// Parámetros y Eventos en el Operador.
    /// </summary>
    enum eTopPar
    {
        None = 0,
        Top,
        EstadoTop,
        EstadoJacksEjecutivo,
        EstadoJacksAyudante,
        EstadoAltavozRadio,
        EstadoAltavozLC,
        EstadoPanel,
        EstadoLan1,
        EstadoLan2,
        EstadoSync,
        SwVersion,
        EstadoPtt,
        SeleccionPaginaRadio,
        LlamadaSaliente,
        LlamadaEntrante,
        LlamadaEstablecida,
        LlamadaFinaliza,
        FacilidadTelefonia,
        Briefing,
        Replay,
        EstadoAltavozHF,
        EstadoCableGrabacion
    };
    /// <summary>
    /// 
    /// </summary>
    class TopSnmpExplorer : NucleoGeneric.NGThread
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        static string GetOid(string id, string _default)
        {
            foreach (string s in Properties.u5kManServer.Default.TopOids)
            {
                string[] p = s.Split(':');
                if (p.Count() == 2 && p[0] == id)
                    return p[1];
            }
            return _default;
        }
#if OIDS_V0
        // OIDS de Operador.
        static public Dictionary<eTopPar, string> _OidPos = new Dictionary<eTopPar, string>()
        {
            // {eTopPar.Top,GetOid("Top",".1.1.1000.0")},                                           // INT
            {eTopPar.EstadoTop,GetOid("EstadoTop",".1.1.1000.0")},                                  // INT
            {eTopPar.EstadoAltavozRadio,GetOid("EstadoAltavozRadio",".1.1.1000.1.2.0")},            // INT
            {eTopPar.EstadoAltavozLC,GetOid("EstadoAltavozLC",".1.1.1000.1.2.1")},                  // INT
            {eTopPar.EstadoJacksEjecutivo,GetOid("EstadoJacksEjecutivo",".1.1.1000.1.3.0")},        // INT
            {eTopPar.EstadoJacksAyudante,GetOid("EstadoJacksAyudante",".1.1.1000.1.3.1")},          // INT
            {eTopPar.EstadoPtt,GetOid ("EstadoPtt",".1.1.1000.2")},                                 // STRING
            {eTopPar.EstadoPanel,GetOid ("EstadoPanel",".1.1.1000.1.4")},                           // INT
            {eTopPar.EstadoLan1,GetOid ("EstadoLan1",".1.1.1000.3.1")},                             // INT
            {eTopPar.EstadoLan2,GetOid ("EstadoLan2",".1.1.1000.3.2")},                             // INT
            {eTopPar.SeleccionPaginaRadio, GetOid("SeleccionPaginaRadio",".1.1.1000.6")},           // STRING
            {eTopPar.LlamadaSaliente, GetOid("LlamadaSaliente",".1.1.1000.7")},                     // STRING
            {eTopPar.LlamadaEntrante, GetOid("LlamadaEntrante",".1.1.1000.9")},                     // STRING
            {eTopPar.LlamadaEstablecida, GetOid("LlamadaEstablecida",".1.1.1000.11")},              // STRING
            {eTopPar.LlamadaFinaliza, GetOid("LlamadaFinaliza",".1.1.1000.10")},                    // STRING
            {eTopPar.FacilidadTelefonia, GetOid("FacilidadTelefonia",".1.1.1000.8")},               // STRING
            {eTopPar.Briefing,GetOid("Briefing",".1.1.1000.12")},                                   // STRING
            {eTopPar.Replay,GetOid("Replay",".1.1.1000.13")},                                       // STRING
            {eTopPar.EstadoAltavozHF, ".1.1.1000.1.2.2"},                                           // INT
            {eTopPar.EstadoCableGrabacion, ".1.1.1000.1.2.3"},                                      // INT
        };

        // Lista Variables del POLLING
        static List<Variable> _vList = new List<Variable>()
                        {
                            new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoTop])),
                            new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoPanel])),
                            new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoJacksEjecutivo])),
                            new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoJacksAyudante])),
                            new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoAltavozRadio])),
                            new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoAltavozLC])),
                            new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoAltavozHF])),
                            new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoCableGrabacion])),
                            new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoLan1])),
                            new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoLan2])),
                        };

#else
        static List<Variable> _vList = new List<Variable>();
        static public Dictionary<eTopPar, string> _OidPos = new Dictionary<eTopPar, string>();
        static void InitStaticTables()
        {
            if (_OidPos.Count > 0 || _vList.Count > 0)
                return;

            // OidPos
            if (Properties.u5kManServer.Default.TopOidsShemaVersion == 0)
            {
                /** OID's Antiguas */
                _OidPos.Add(eTopPar.EstadoTop, GetOid("EstadoTop", ".1.1.1000.0"));                                 // INT
                _OidPos.Add(eTopPar.EstadoAltavozRadio, GetOid("EstadoAltavozRadio", ".1.1.1000.1.2.0"));            // INT
                _OidPos.Add(eTopPar.EstadoAltavozLC, GetOid("EstadoAltavozLC", ".1.1.1000.1.2.1"));                  // INT
                _OidPos.Add(eTopPar.EstadoJacksEjecutivo, GetOid("EstadoJacksEjecutivo", ".1.1.1000.1.3.0"));        // INT
                _OidPos.Add(eTopPar.EstadoJacksAyudante, GetOid("EstadoJacksAyudante", ".1.1.1000.1.3.1"));          // INT
                _OidPos.Add(eTopPar.EstadoPtt, GetOid("EstadoPtt", ".1.1.1000.2"));                                 // STRING
                _OidPos.Add(eTopPar.EstadoPanel, GetOid("EstadoPanel", ".1.1.1000.1.4"));                           // INT
                _OidPos.Add(eTopPar.EstadoLan1, GetOid("EstadoLan1", ".1.1.1000.3.1"));                             // INT
                _OidPos.Add(eTopPar.EstadoLan2, GetOid("EstadoLan2", ".1.1.1000.3.2"));                             // INT
                _OidPos.Add(eTopPar.SeleccionPaginaRadio, GetOid("SeleccionPaginaRadio", ".1.1.1000.6"));           // STRING
                _OidPos.Add(eTopPar.LlamadaSaliente, GetOid("LlamadaSaliente", ".1.1.1000.7"));                     // STRING
                _OidPos.Add(eTopPar.LlamadaEntrante, GetOid("LlamadaEntrante", ".1.1.1000.9"));                     // STRING
                _OidPos.Add(eTopPar.LlamadaEstablecida, GetOid("LlamadaEstablecida", ".1.1.1000.11"));              // STRING
                _OidPos.Add(eTopPar.LlamadaFinaliza, GetOid("LlamadaFinaliza", ".1.1.1000.10"));                    // STRING
                _OidPos.Add(eTopPar.FacilidadTelefonia, GetOid("FacilidadTelefonia", ".1.1.1000.8"));               // STRING
                _OidPos.Add(eTopPar.Briefing, GetOid("Briefing", ".1.1.1000.12"));                                   // STRING
                _OidPos.Add(eTopPar.Replay, GetOid("Replay", ".1.1.1000.13"));                                       // STRING
                _OidPos.Add(eTopPar.EstadoAltavozHF, ".1.1.1000.1.2.2");                                           // INT
                _OidPos.Add(eTopPar.EstadoCableGrabacion, ".1.1.1000.1.2.3");                                      // INT
            }
            else
            {
                /** OID's Nuevas */
                _OidPos.Add(eTopPar.EstadoTop, ".1.3.6.1.4.1.7916.8.2.2.1.0");                           // INT
                _OidPos.Add(eTopPar.EstadoAltavozRadio, ".1.3.6.1.4.1.7916.8.2.2.2.0");                           // INT
                _OidPos.Add(eTopPar.EstadoAltavozLC, ".1.3.6.1.4.1.7916.8.2.2.3.0");                           // INT
                _OidPos.Add(eTopPar.EstadoAltavozHF, ".1.3.6.1.4.1.7916.8.2.2.4.0");                           // INT
                _OidPos.Add(eTopPar.EstadoCableGrabacion, ".1.3.6.1.4.1.7916.8.2.2.5.0");                           // INT
                _OidPos.Add(eTopPar.EstadoJacksEjecutivo, ".1.3.6.1.4.1.7916.8.2.2.6.0");                           // INT
                _OidPos.Add(eTopPar.EstadoJacksAyudante, ".1.3.6.1.4.1.7916.8.2.2.7.0");                           // INT
                _OidPos.Add(eTopPar.EstadoPanel, ".1.3.6.1.4.1.7916.8.2.2.8.0");                           // INT
                _OidPos.Add(eTopPar.EstadoLan1, ".1.3.6.1.4.1.7916.8.2.2.9.0");                           // INT
                _OidPos.Add(eTopPar.EstadoLan2, ".1.3.6.1.4.1.7916.8.2.2.10.0");                          // INT
                _OidPos.Add(eTopPar.EstadoSync, ".1.3.6.1.4.1.7916.8.2.2.11.0");                          // STRING
                _OidPos.Add(eTopPar.SwVersion, ".1.3.6.1.4.1.7916.8.2.2.12.0");                          // STRING
                _OidPos.Add(eTopPar.EstadoPtt, ".1.3.6.1.4.1.7916.8.2.3.1.0");                           // STRING
                _OidPos.Add(eTopPar.SeleccionPaginaRadio, ".1.3.6.1.4.1.7916.8.2.3.2.0");                           // STRING
                _OidPos.Add(eTopPar.LlamadaSaliente, ".1.3.6.1.4.1.7916.8.2.3.3.0");                           // STRING
                _OidPos.Add(eTopPar.FacilidadTelefonia, ".1.3.6.1.4.1.7916.8.2.3.4.0");                           // STRING
                _OidPos.Add(eTopPar.LlamadaEntrante, ".1.3.6.1.4.1.7916.8.2.3.5.0");                           // STRING
                _OidPos.Add(eTopPar.LlamadaFinaliza, ".1.3.6.1.4.1.7916.8.2.3.6.0");                           // STRING
                _OidPos.Add(eTopPar.LlamadaEstablecida, ".1.3.6.1.4.1.7916.8.2.3.7.0");                            // STRING
                _OidPos.Add(eTopPar.Briefing, ".1.3.6.1.4.1.7916.8.2.3.8.0");                            // STRING
                _OidPos.Add(eTopPar.Replay, ".1.3.6.1.4.1.7916.8.2.3.9.0");                            // STRING
            }

            // Lista Variables del POLLING
            _vList.Add(new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoTop])));
            _vList.Add(new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoPanel])));
            _vList.Add(new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoJacksEjecutivo])));
            _vList.Add(new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoJacksAyudante])));
            _vList.Add(new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoAltavozRadio])));
            _vList.Add(new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoAltavozLC])));
            _vList.Add(new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoAltavozHF])));
            _vList.Add(new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoCableGrabacion])));
            _vList.Add(new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoLan1])));
            _vList.Add(new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoLan2])));
            _vList.Add(new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoSync])));
            _vList.Add(new Variable(new ObjectIdentifier(_OidPos[eTopPar.SwVersion])));
        }
#endif
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entrada"></param>
        /// <returns></returns>
        static public std Bool2std(bool entrada)
        {
            return entrada ? std.Ok : std.NoInfo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entrada"></param>
        /// <returns></returns>
        static public bool std2Bool(std entrada)
        {
            return entrada == std.Ok ? true : false;
        }
        /// <summary>
        /// 
        /// </summary>
        // public event GenerarHistorico hist;
        static public event ChangeStatusDelegate CambiaEstado;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pdata"></param>
        public TopSnmpExplorer(ChangeStatusDelegate _CambiaEstado)
        {
            Name = "TopSnmpExplorer";
            CambiaEstado = _CambiaEstado;
#if OIDS_V0
#else
            InitStaticTables();
#endif
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void Run()
        {
            U5kGenericos.TraceCurrentThread(this.GetType().Name);

            Decimal interval = Properties.u5kManServer.Default.SpvInterval;     // Tiempo de Polling,
            Decimal threadTimeout = 2 * interval / 3;                           // Tiempo de proceso individual.
            Decimal poolTimeout = 3 * interval / 4;                             // Tiempo máximo del Pool de Procesos.

            var taskControl = new PollingHelper();

            //U5kGenericos.SetCurrentCulture();
            LogInfo<TopSnmpExplorer>("Arrancado...");

            using (timer = new TaskTimer(TimeSpan.FromMilliseconds((double)interval), this.Cancel))
            {
                // Procesos.
                while (IsRunning())
                {
                    if (U5kManService._Master == true)
                    {
#if POOL_METHOD_0
                        List<stdPos> localpos = null;   // new List<stdPos>();
                        try
                        {
                            TimeMeasurement tm = new TimeMeasurement("Top Explorer");

                            GlobalServices.GetWriteAccess((data) => localpos = data.STDTOPS.Select(pos => new stdPos(pos)).ToList());

                            // Arranco los Procesos... 
                            List<Task> task = new List<Task>();
                            localpos?.ForEach((pos) =>
                            {
                                task.Add(
                                    BackgroundTaskFactory.StartNew(pos.name, () =>
                                    {
                                        U5kGenericos.TraceCurrentThread(this.GetType().Name + " " + pos.name);
                                        ExploraTop(pos);
                                    },
                                    (id, excep) =>
                                    {
                                        LogException<TopSnmpExplorer>("Supervisando Pasarela " + pos.name, excep);
                                    }, TimeSpan.FromMilliseconds((double)threadTimeout))
                                    );
                            });
                            // Espero a que acaben todos.
                            Task.WaitAll(task.ToArray(), TimeSpan.FromMilliseconds((double)poolTimeout));
                        }
                        catch (Exception x)
                        {
                            if (x is ThreadAbortException)
                            {
                                Thread.ResetAbort();
                                break;
                            }
                            else if (x is AggregateException)
                            {
                                foreach (var excep in (x as AggregateException).InnerExceptions)
                                {
                                    LogTrace<TopSnmpExplorer>($"Excepcion {excep.Message}");
                                }
                            }
                            else
                            {
                                LogException<TopSnmpExplorer>("Supervisando Pasarelas ", x);
                            }
                        }
                        if (localpos != null)
                        {
                            GlobalServices.GetWriteAccess((data) => data.POSDIC = localpos.Select(p => p).ToDictionary(p => p.name, p => p));
                        }
                        tm.StopAndPrint((msg) => { LogTrace<TopSnmpExplorer>(msg); });
#else
                        GlobalServices.GetWriteAccess((gdata) =>
                        {
                            // limpiar pollingControl con los Puestos que puedan desaparecer de la configuracion.
                            taskControl.DeleteNotPresent(gdata.STDTOPS.Select(p => p.name).ToList());

                            // Relleno los datos...
                            gdata.STDTOPS.ForEach(psto =>
                            {
                                if (taskControl.IsTaskActive(psto.name) == false)
                                {
                                    var newPsto = new stdPos(psto);
                                    var task = Task.Factory.StartNew(() =>
                                    {
                                        try
                                        {
                                            LogTrace<TopSnmpExplorer>($"Exploracion Puesto {newPsto.name} iniciada.");

                                            ExploraTop(newPsto);
#if DEBUG1
                                            // Para asegurar un tiempo de ejecucion.
                                            Task.Delay(TimeSpan.FromMilliseconds(500)).Wait();
#endif
                                            /// Copio los datos obtenidos a la tabla...
                                            GlobalServices.GetWriteAccess((gdata1) =>
                                            {
                                                if (gdata1.POSDIC.ContainsKey(newPsto.name))
                                                {
                                                    /** 20200813. Solo actualiza el estado si no se ha cambiado en medio la configuracion */
                                                    if (gdata1.POSDIC[newPsto.name].Equals(newPsto))
                                                    {
                                                        gdata1.POSDIC[newPsto.name].CopyFrom(newPsto);
                                                    }
                                                    else
                                                    {
                                                        LogWarn<GwExplorer>($"Exploracion {newPsto.name}. Resultado Exploracion ignorado. El Puesto ha cambiado de Configuracion.");
                                                    }
                                                }
                                                else
                                                {
                                                    LogWarn<GwExplorer>($"Exploracion {newPsto.name}. Resultado Exploracion ignorado. El puesto ha sido eliminado.");
                                                }
                                            });
                                        }
                                        catch (Exception x)
                                        {
                                            LogException<TopSnmpExplorer>("Supervisando Puesto " + newPsto.name, x);
                                        }
                                        finally
                                        {
                                            LogTrace<TopSnmpExplorer>($"Exploracion Puesto {newPsto.name} finalizada.");
                                        }
                                    }, TaskCreationOptions.LongRunning);
                                    taskControl.SetTask(psto.name, task);
                                }
                                else
                                {
                                    // todo. Algun tipo de supervision si nunca vuelve...
                                    LogWarn<TopSnmpExplorer>($"Exploracion de Puesto {psto.name} no finalizada en Tiempo ...");
                                }
                            });
                        });
#if DEBUG1
                        /** Para simular Sectorizaciones con cambios 'problematicos' */
                        tm.FromCreation(TimeSpan.FromMinutes(1), () =>
                        {
                            GlobalServices.GetWriteAccess((gdata) =>
                            {
                                //gdata.POSDIC.Remove("PICT01");
                                gdata.POSDIC["PICT01"] = new stdPos() { name = "PICT01", ip = "127.0.0.1" };
                            });
                        });
#endif

#endif
                    }
                    GoToSleepInTimer();
                }
            }

            Dispose();
            LogInfo<TopSnmpExplorer>("Finalizado...");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pos"></param>
        /// <param name="estado"></param>
        static public void EstadoPosicionSet(string name, stdPos pos, std estado)
        {
            //std stdg_old = pos.stdGlobal();
            pos.stdpos = CambiaEstado(pos.stdpos, estado, 0,
                                      estado == std.Ok ? eIncidencias.ITO_ENTRADA : eIncidencias.ITO_CAIDA,
                                      eTiposInci.TEH_TOP, pos.name);
            pos.stdg = pos.StdGlobal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pos"></param>
        /// <param name="estado"></param>
        static public void PosicionEjecutivoSet(string name, stdPos pos, std estado)
        {
            pos.jack_exe = CambiaEstado(pos.jack_exe, estado, 0,
                                pos.jack_exe == std.Ok ? eIncidencias.ITO_DESCONEXION_JACK_EJECUTIVO : eIncidencias.ITO_CONEXION_JACK_EJECUTIVO,
                                eTiposInci.TEH_TOP, pos.name, pos.name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pos"></param>
        /// <param name="estado"></param>
        static public void PosicionAyudanteSet(string name, stdPos pos, std estado)
        {
            pos.jack_ayu = CambiaEstado(pos.jack_ayu, estado, 0,
                                pos.jack_ayu == std.Ok ? eIncidencias.ITO_DESCONEXION_JACK_AYUDANTE : eIncidencias.ITO_CONEXION_JACK_AYUDANTE,
                                eTiposInci.TEH_TOP, pos.name, pos.name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pos"></param>
        /// <param name="estado"></param>
        static public void PosicionALRSet(string name, stdPos pos, std estado)
        {
            pos.alt_r = CambiaEstado(pos.alt_r, estado, 0,
                                pos.alt_r == std.Ok ? eIncidencias.ITO_DESCONEXION_ALTAVOZ : eIncidencias.ITO_CONEXION_ALTAVOZ,
                                eTiposInci.TEH_TOP, pos.name, "Radio", pos.name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pos"></param>
        /// <param name="estado"></param>
        static public void PosicionALTSet(string name, stdPos pos, std estado)
        {
            pos.alt_t = CambiaEstado(pos.alt_t, estado, 0,
                                pos.alt_t == std.Ok ? eIncidencias.ITO_DESCONEXION_ALTAVOZ : eIncidencias.ITO_CONEXION_ALTAVOZ,
                                eTiposInci.TEH_TOP, pos.name, "LC", pos.name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pos"></param>
        /// <param name="estado"></param>
        static public void PosicionALHFSet(string name, stdPos pos, std estado)
        {
            pos.alt_hf = CambiaEstado(pos.alt_hf, estado, 0,
                                pos.alt_hf == std.Ok ? eIncidencias.ITO_DESCONEXION_ALTAVOZ : eIncidencias.ITO_CONEXION_ALTAVOZ,
                                eTiposInci.TEH_TOP, pos.name, "HF", pos.name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pos"></param>
        /// <param name="estado"></param>
        static public void PosicionGRBCABSet(string name, stdPos pos, std estado)
        {
            pos.rec_w = CambiaEstado(pos.rec_w, estado, 0,
                                pos.rec_w == std.Ok ? eIncidencias.ITO_DESCONEXION_CABLE_GRABACION : eIncidencias.ITO_CONEXION_CABLE_GRABACION,
                                eTiposInci.TEH_TOP, pos.name, "", pos.name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pos"></param>
        /// <param name="estado"></param>
        static public void PosicionPanelSet(string name, stdPos pos, std estado)
        {
            pos.panel = CambiaEstado(pos.panel, estado, 0,
                                pos.panel == std.Ok ? eIncidencias.ITO_PANEL_SBY : eIncidencias.ITO_PANEL_OPERACION,
                                eTiposInci.TEH_TOP, pos.name, pos.name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pos"></param>
        /// <param name="estado"></param>
        static public void PosicionLan1Set(string name, stdPos pos, std estado)
        {
            bool generaevento = (pos.lan1 != estado);
            pos.lan1 = CambiaEstado(pos.lan1, estado, 0, eIncidencias.IGNORE,
                                eTiposInci.TEH_TOP, pos.name, pos.name);
            if (generaevento)
                GeneraEventoLan(pos);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pos"></param>
        /// <param name="estado"></param>
        static public void PosicionLan2Set(string name, stdPos pos, std estado)
        {
            bool generaevento = (pos.lan2 != estado);
            pos.lan2 = CambiaEstado(pos.lan2, estado, 0, eIncidencias.IGNORE,
                                eTiposInci.TEH_TOP, pos.name, pos.name);
            if (generaevento)
                GeneraEventoLan(pos);
        }

        static public void PosicionSyncStatusSet(string name, stdPos pos, string status)
        {
            pos.NtpInfo.Actualize(pos.name, status);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        static void GeneraEventoLan(stdPos pos)
        {
            RecordEvent<TopSnmpExplorer>(DateTime.Now, eIncidencias.ITO_ESTADO_LAN, eTiposInci.TEH_TOP, pos.name,
                Params(pos.lan1 == std.Ok ? "ON" : "OFF", pos.lan2 == std.Ok ? "ON" : "OFF"));
        }

        protected void ExploraTop(object obj)
        {
            stdPos pos = (stdPos)obj;
            if (pos.IsPollingTime() == true)
            {
                LogTrace<TopSnmpExplorer>($"{pos.name}. POLLING executed");
                SnmpTopPolling(pos, (res) =>
                {
                    if (pos.ProcessResult(res) == true)
                    {
                        if (!res)
                        {
                            EstadoPosicionSet(pos.name, pos, std.NoInfo);
                            pos.Reset();                
                            LogTrace<TopSnmpExplorer>($"{pos.name}. Fail.");
                        }
                        else
                        {
                            LogTrace<TopSnmpExplorer>($"{pos.name}. Ok.");
                        }
                    }
                    else
                    {
                        LogWarn<TopSnmpExplorer>($"{pos.name}. Fail Ignored.");
                    }
                });
            }
            else
            {
                LogTrace<TopSnmpExplorer>($"{pos.name}. POLLING Skipped");
            }
        }
        protected void SnmpTopPolling(stdPos pos, Action<bool> response)
        {
            try
            {
                IList<Variable> onlinedata=null;
#if DEBUG
                if (DebuggingHelper.Simulating)
                {
                    var SimulatedTop = new DebuggingHelper.SimulatedTop(pos.name);
                    SimulatedTop.SnmpPing((isok, data) =>
                    {
                        if (!isok)
                            throw new Exception($"Simulated Top {pos.name}. No responde...");
                        onlinedata = new List<Variable>
                        {
                            new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoTop]), new Integer32(data.stdpos)),
                            new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoPanel]), new Integer32(data.panel)),
                            new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoJacksEjecutivo]), new Integer32(data.jack_exe)),
                            new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoJacksAyudante]), new Integer32(data.jack_ayu)),
                            new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoAltavozRadio]), new Integer32(data.alt_r)),
                            new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoAltavozLC]), new Integer32(data.alt_t)),
                            new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoAltavozHF]), new Integer32(data.alt_hf)),
                            new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoCableGrabacion]), new Integer32(data.rec_w)),
                            new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoLan1]), new Integer32(data.lan1)),
                            new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoLan2]), new Integer32(data.lan2)),
                            new Variable(new ObjectIdentifier(_OidPos[eTopPar.EstadoSync]), new OctetString(data.ntp??"")),
                            //new Variable(new ObjectIdentifier(_OidPos[eTopPar.SwVersion]), new OctetString(data.stdpos))
                        };
                    });
                }
                else
#endif
                onlinedata = new SnmpClient().Get(VersionCode.V2,
                    new IPEndPoint(IPAddress.Parse(pos.ip), pos.snmpport),
                    new OctetString("public"), _vList,
                    pos.SnmpTimeout, pos.SnmpReintentos);

                ProcessPos(pos, onlinedata);
                response(true);
            }
            catch (Exception x)
            {
                LogException<TopSnmpExplorer>(String.Format("{0}({1})", pos.name, pos.ip), x);
                response(false);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="result"></param>
        protected void ProcessPos(stdPos pos, IList<Variable> result)
        {
            // lock (pos)
            foreach (Variable varItem in result)
            {
                int val;
                int.TryParse(varItem.Data.ToString(), out val);
                //string oid = varItem.Id.ToString();
                //eTopPar par = (eTopPar)_OidPos.Where(p => p.Value == oid).First().Key;
                var BdtItem = _OidPos.Where(p => p.Value.EndsWith(varItem.Id.ToString())).ToList();
                if (BdtItem.Count > 0)
                {
                    try
                    {
                        switch (BdtItem[0].Key)
                        {
                            case eTopPar.EstadoTop:
                                EstadoPosicionSet(pos.name, pos, val == 1 ? std.Ok : std.NoInfo);
                                break;
                            case eTopPar.EstadoPanel:
                                PosicionPanelSet(pos.name, pos, val == 1 ? std.Ok : std.NoInfo);
                                break;
                            case eTopPar.EstadoJacksEjecutivo:
                                PosicionEjecutivoSet(pos.name, pos, val == 1 ? std.Ok : std.NoInfo);
                                break;
                            case eTopPar.EstadoJacksAyudante:
                                PosicionAyudanteSet(pos.name, pos, val == 1 ? std.Ok : std.NoInfo);
                                break;
                            case eTopPar.EstadoAltavozRadio:
                                PosicionALRSet(pos.name, pos, val == 1 ? std.Ok : std.NoInfo);
                                break;
                            case eTopPar.EstadoAltavozLC:
                                PosicionALTSet(pos.name, pos, val == 1 ? std.Ok : std.NoInfo);
                                break;
                            case eTopPar.EstadoLan1:
                                PosicionLan1Set(pos.name, pos, val == 1 ? std.Ok : val == 2 ? std.Error : std.NoInfo);
                                break;
                            case eTopPar.EstadoLan2:
                                PosicionLan2Set(pos.name, pos, val == 1 ? std.Ok : val == 2 ? std.Error : std.NoInfo);
                                break;
                            case eTopPar.EstadoSync:
                                PosicionSyncStatusSet(pos.name, pos, varItem.Data.ToString());
                                break;
                            case eTopPar.SwVersion:
                                pos.sw_version = varItem.Data.ToString();
                                break;
                            case eTopPar.EstadoAltavozHF:
                                PosicionALHFSet(pos.name, pos, val == 1 ? std.Ok : std.NoInfo);
                                break;
                            case eTopPar.EstadoCableGrabacion:
                                PosicionGRBCABSet(pos.name, pos, val == 1 ? std.Ok : std.NoInfo);
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception x)
                    {
                        LogException<TopSnmpExplorer>(pos.name, x);
                    }
                }
                else
                {
                    // Error OID no encontrado...
                    LogWarn<TopSnmpExplorer>(String.Format("TOP OID [{0}] No encontrado", varItem.Id.ToString()));
                }

            }
        }
    }
}
