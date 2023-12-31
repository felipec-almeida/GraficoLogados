﻿using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraficosFullWMS.Classes
{
    public class ConnectionToDB
    {
        public string ConnectionString { get; }
        public string DataInicio { get; }
        public string DataFim { get; }
        public string Tipo { get; }
        public int EmprCodemp { get; }
        public bool IsAdded { get; }

        private readonly List<DateTime> DataHora;
        private readonly List<double> Logados;
        private readonly List<double> Colaboradores;
        private readonly List<double> TotalLogados;
        private readonly List<double> JuntaGraficosLogados;
        private readonly List<double> JuntaGraficosColaboradores;
        private readonly List<double> JuntaGraficosTotalLogados;
        public List<ConnectionsDB> ConnectionsDB;

        public ConnectionToDB(string connectionString, string dataInicio, string dataFim, string tipo, int emprCodemp, List<DateTime> dataHora, List<double> logados, List<double> colaboradores, List<double> totalLogados)
        {
            this.ConnectionString = connectionString;
            this.DataInicio = dataInicio;
            this.DataFim = dataFim;
            this.Tipo = tipo;
            this.EmprCodemp = emprCodemp;
            this.DataHora = dataHora;
            this.Logados = logados;
            this.Colaboradores = colaboradores;
            this.TotalLogados = totalLogados;
        }

        public ConnectionToDB(string connectionString, string dataInicio, string dataFim, string tipo, int emprCodemp, List<DateTime> dataHora, List<double> logados, List<double> colaboradores, List<double> totalLogados, List<double> juntaLogados, List<double> juntaColaboradores, List<double> juntaTotalLogados, List<ConnectionsDB> connectionsDBs, bool isAdded)
        {
            this.ConnectionString = connectionString;
            this.DataInicio = dataInicio;
            this.DataFim = dataFim;
            this.Tipo = tipo;
            this.EmprCodemp = emprCodemp;
            this.DataHora = dataHora;
            this.Logados = logados;
            this.Colaboradores = colaboradores;
            this.TotalLogados = totalLogados;
            this.JuntaGraficosLogados = juntaLogados;
            this.JuntaGraficosColaboradores = juntaColaboradores;
            this.JuntaGraficosTotalLogados = juntaTotalLogados;
            this.ConnectionsDB = connectionsDBs;
            this.IsAdded = isAdded;
        }

        public async Task Connect()
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(ConnectionString))
                {
                    Cursor.Current = Cursors.WaitCursor;

                    if (Tipo.Equals("1 - Usuários Logados"))
                    {
                        connection.Open();

                        OracleCommand command = new OracleCommand
                        {
                            CommandText = "pkg_wms_full_lic.prc_fullwms_licencas",
                            Connection = connection,
                            CommandType = CommandType.StoredProcedure,
                            Parameters =
                            {
                                new OracleParameter("p_tipo", OracleDbType.BinaryFloat, ParameterDirection.Input) { Value = 1},
                                new OracleParameter("p_data_inicio", OracleDbType.Varchar2, ParameterDirection.Input) { Value = DataInicio},
                                new OracleParameter("p_data_fim", OracleDbType.Varchar2, ParameterDirection.Input) { Value = DataFim}
                            }
                        };

                        if (EmprCodemp.Equals(0))
                        {
                            command.Parameters.Add("p_codemp", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = null;
                        }
                        else
                        {
                            command.Parameters.Add("p_codemp", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = EmprCodemp;
                        }

                        OracleParameter cursorParameter = new OracleParameter
                        {
                            ParameterName = "cursorParameter",
                            OracleDbType = OracleDbType.RefCursor,
                            Direction = ParameterDirection.Output
                        };

                        command.Parameters.Add(cursorParameter);
                        command.ExecuteNonQuery();

                        using (OracleDataReader reader = ((OracleRefCursor)cursorParameter.Value).GetDataReader())
                        {
                            DataHora.Clear();
                            Logados.Clear();

                            while (reader.Read())
                            {
                                DateTime colunaTotal = reader.GetDateTime(0);
                                int colunaUsu = reader.GetInt32(1);
                                DataHora.Add(colunaTotal);
                                Logados.Add(colunaUsu);
                            }
                        }

                        connection.Close();
                    }
                    else if (Tipo.Equals("2 - Colaboradores Logados"))
                    {
                        connection.Open();
                        OracleCommand command = new OracleCommand
                        {
                            CommandText = "pkg_wms_full_lic.prc_fullwms_licencas",
                            Connection = connection,
                            CommandType = CommandType.StoredProcedure,
                            Parameters =
                            {
                                new OracleParameter("p_tipo", OracleDbType.BinaryFloat, ParameterDirection.Input) { Value = 2},
                                new OracleParameter("p_data_inicio", OracleDbType.Varchar2, ParameterDirection.Input) { Value = DataInicio},
                                new OracleParameter("p_data_fim", OracleDbType.Varchar2, ParameterDirection.Input) { Value = DataFim}
                            }
                        };

                        if (EmprCodemp.Equals(0))
                        {
                            command.Parameters.Add("p_codemp", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = null;
                        }
                        else
                        {
                            command.Parameters.Add("p_codemp", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = EmprCodemp;
                        }

                        OracleParameter cursorParameter = new OracleParameter
                        {
                            ParameterName = "cursorParameter",
                            OracleDbType = OracleDbType.RefCursor,
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(cursorParameter);
                        command.ExecuteNonQuery();

                        using (OracleDataReader reader = ((OracleRefCursor)cursorParameter.Value).GetDataReader())
                        {
                            DataHora.Clear();
                            Colaboradores.Clear();

                            while (reader.Read())
                            {
                                DateTime colunaData = reader.GetDateTime(0);
                                int colunaColab = reader.GetInt32(1);
                                DataHora.Add(colunaData);
                                Colaboradores.Add(colunaColab);
                            }
                        }

                        connection.Close();
                    }
                    else if (Tipo.Equals("3 - Total Logados"))
                    {
                        connection.Open();
                        OracleCommand command = new OracleCommand
                        {
                            CommandText = "pkg_wms_full_lic.prc_fullwms_licencas",
                            Connection = connection,
                            CommandType = CommandType.StoredProcedure,
                            Parameters =
                            {
                                new OracleParameter("p_tipo", OracleDbType.BinaryFloat, ParameterDirection.Input) { Value = 3},
                                new OracleParameter("p_data_inicio", OracleDbType.Varchar2, ParameterDirection.Input) { Value = DataInicio},
                                new OracleParameter("p_data_fim", OracleDbType.Varchar2, ParameterDirection.Input) { Value = DataFim}
                            }
                        };

                        if (EmprCodemp.Equals(0))
                        {
                            command.Parameters.Add("p_codemp", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = null;
                        }
                        else
                        {
                            command.Parameters.Add("p_codemp", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = EmprCodemp;
                        }

                        OracleParameter cursorParameter = new OracleParameter
                        {
                            ParameterName = "cursorParameter",
                            OracleDbType = OracleDbType.RefCursor,
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(cursorParameter);
                        command.ExecuteNonQuery();

                        using (OracleDataReader reader = ((OracleRefCursor)cursorParameter.Value).GetDataReader())
                        {
                            DataHora.Clear();
                            TotalLogados.Clear();

                            while (reader.Read())
                            {
                                DateTime colunaData = reader.GetDateTime(0);
                                int colunaTotal = reader.GetInt32(4);
                                DataHora.Add(colunaData);
                                TotalLogados.Add((colunaTotal));
                            }
                        }

                        connection.Close();
                    }
                    else if (Tipo.Equals("4 - Usuários/Colaboradores"))
                    {
                        connection.Open();
                        OracleCommand command = new OracleCommand
                        {
                            CommandText = "pkg_wms_full_lic.prc_fullwms_licencas",
                            Connection = connection,
                            CommandType = CommandType.StoredProcedure,
                            Parameters =
                            {
                                new OracleParameter("p_tipo", OracleDbType.BinaryFloat, ParameterDirection.Input) { Value = 4},
                                new OracleParameter("p_data_inicio", OracleDbType.Varchar2, ParameterDirection.Input) { Value = DataInicio},
                                new OracleParameter("p_data_fim", OracleDbType.Varchar2, ParameterDirection.Input) { Value = DataFim}
                            }
                        };

                        if (EmprCodemp.Equals(0))
                        {
                            command.Parameters.Add("p_codemp", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = null;
                        }
                        else
                        {
                            command.Parameters.Add("p_codemp", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = EmprCodemp;
                        }

                        OracleParameter cursorParameter = new OracleParameter
                        {
                            ParameterName = "cursorParameter",
                            OracleDbType = OracleDbType.RefCursor,
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(cursorParameter);
                        command.ExecuteNonQuery();

                        using (OracleDataReader reader = ((OracleRefCursor)cursorParameter.Value).GetDataReader())
                        {
                            DataHora.Clear();
                            Logados.Clear();
                            Colaboradores.Clear();
                            TotalLogados.Clear();

                            while (reader.Read())
                            {
                                DateTime colunaData = reader.GetDateTime(0);
                                double colunaUsu = Convert.ToDouble(reader.GetString(1));
                                double colunaColab = Convert.ToDouble(reader.GetString(2));
                                double colunaTotal = Convert.ToDouble(reader.GetString(3));
                                DataHora.Add(colunaData);
                                Logados.Add(colunaUsu);
                                Colaboradores.Add(colunaColab);
                                TotalLogados.Add(colunaTotal);
                            }
                        }

                        connection.Close();
                    }
                    else if (Tipo.Equals("5 - Junta Gráficos"))
                    {
                        connection.Open();
                        OracleCommand command = new OracleCommand
                        {
                            CommandText = "pkg_wms_full_lic.prc_fullwms_licencas",
                            Connection = connection,
                            CommandType = CommandType.StoredProcedure,
                            Parameters =
                            {
                                new OracleParameter("p_tipo", OracleDbType.BinaryFloat, ParameterDirection.Input) { Value = 4},
                                new OracleParameter("p_data_inicio", OracleDbType.Varchar2, ParameterDirection.Input) { Value = DataInicio},
                                new OracleParameter("p_data_fim", OracleDbType.Varchar2, ParameterDirection.Input) { Value = DataFim}
                            }
                        };

                        if (EmprCodemp.Equals(0))
                        {
                            command.Parameters.Add("p_codemp", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = null;
                        }
                        else
                        {
                            command.Parameters.Add("p_codemp", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = EmprCodemp;
                        }

                        OracleParameter cursorParameter = new OracleParameter
                        {
                            ParameterName = "cursorParameter",
                            OracleDbType = OracleDbType.RefCursor,
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(cursorParameter);
                        command.ExecuteNonQuery();

                        using (OracleDataReader reader = ((OracleRefCursor)cursorParameter.Value).GetDataReader())
                        {
                            DataHora.Clear();
                            var temp = new ConnectionsDB();
                            while (reader.Read())
                            {
                                DateTime colunaData = reader.GetDateTime(0);
                                double colunaUsu = Convert.ToDouble(reader.GetString(1));
                                double colunaColab = Convert.ToDouble(reader.GetString(2));
                                double colunaTotal = Convert.ToDouble(reader.GetString(3));
                                temp.DataHoraTemp.Add(colunaData);
                                temp.LogadosTemp.Add(colunaUsu);
                                temp.ColaboradoresTemp.Add(colunaColab);
                                temp.TotalLogadosTemp.Add(colunaTotal);
                            }

                            if (IsAdded.Equals(true))
                                DataHora.AddRange(temp.DataHoraTemp);

                            ConnectionsDB.Add(temp);
                            if (IsAdded.Equals(true))
                            {
                                try
                                {
                                    for (int i = 1; i <= ConnectionsDB.Count; i++)
                                    {
                                        if (!JuntaGraficosLogados.Any())
                                        {
                                            JuntaGraficosLogados.AddRange(ConnectionsDB[i - 1].LogadosTemp);
                                        }
                                        else
                                        {
                                            if (JuntaGraficosLogados.Count.Equals(ConnectionsDB[i - 1].LogadosTemp.Count))
                                            {
                                                for (int j = 0; j < JuntaGraficosLogados.Count; j++)
                                                {
                                                    JuntaGraficosLogados[j] += ConnectionsDB[i - 1].LogadosTemp[j];
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (ArgumentOutOfRangeException)
                                {
                                    throw new ArgumentOutOfRangeException("Erro, índice de usuários fora do limite.");
                                }

                                try
                                {
                                    for (int i = 1; i <= ConnectionsDB.Count; i++)
                                    {
                                        if (!JuntaGraficosColaboradores.Any())
                                        {
                                            JuntaGraficosColaboradores.AddRange(ConnectionsDB[i - 1].ColaboradoresTemp);
                                        }
                                        else
                                        {
                                            if (JuntaGraficosColaboradores.Count.Equals(ConnectionsDB[i - 1].ColaboradoresTemp.Count))
                                            {
                                                for (int j = 0; j < JuntaGraficosColaboradores.Count; j++)
                                                {
                                                    JuntaGraficosColaboradores[j] += ConnectionsDB[i - 1].ColaboradoresTemp[j];
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (ArgumentOutOfRangeException)
                                {
                                    throw new ArgumentOutOfRangeException("Erro, índice de usuários fora do limite.");
                                }

                                try
                                {
                                    for (int i = 1; i <= ConnectionsDB.Count; i++)
                                    {
                                        if (!JuntaGraficosTotalLogados.Any())
                                        {
                                            JuntaGraficosTotalLogados.AddRange(ConnectionsDB[i - 1].TotalLogadosTemp);
                                        }
                                        else
                                        {
                                            if (JuntaGraficosTotalLogados.Count.Equals(ConnectionsDB[i - 1].TotalLogadosTemp.Count))
                                            {
                                                for (int j = 0; j < JuntaGraficosTotalLogados.Count; j++)
                                                {
                                                    JuntaGraficosTotalLogados[j] += ConnectionsDB[i - 1].TotalLogadosTemp[j];
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (ArgumentOutOfRangeException)
                                {
                                    throw new ArgumentOutOfRangeException("Erro, índice de usuários fora do limite.");
                                }
                            }
                        }

                        connection.Close();
                    }

                    await Task.Delay(15);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}
