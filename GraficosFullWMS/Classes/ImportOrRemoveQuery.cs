using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Windows.Forms;

namespace GraficosFullWMS.Classes
{
    public class ImportOrRemoveQuery
    {
        public string ConnectionString { get; }

        public ImportOrRemoveQuery(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public void ImportQuery()
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(this.ConnectionString))
                {
                    connection.Open();
                    int countTemp = 0;

                    // Verifica se existe os Índices necessários para a execução da Procedure.
                    const string verifyIndex = @"select count(1) as resultado
                                          from user_indexes i
                                         where i.INDEX_NAME = 'IX_WMS_COLABO_LOGADOS_DATAS'";
                    OracleCommand commandIndex = new OracleCommand
                    {
                        CommandText = verifyIndex,
                        Connection = connection,
                        CommandType = CommandType.Text,
                    };

                    using (OracleDataReader reader = commandIndex.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int contador = reader.GetInt32(0);

                            if (contador.Equals(0))
                            {
                                Cursor.Current = Cursors.WaitCursor;
                                countTemp++;
                                // MessageBox.Show("Criando os Índices necessários para execução da aplicação...", "Criando os Índices", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                const string createIndex1 = "create index IX_WMS_COLABO_LOGADOS_DATAS on WMS_COLABORADORES_LOGADOS (dthr_ent, dthr_saida)";
                                OracleCommand createCommand1 = new OracleCommand
                                {
                                    CommandText = createIndex1,
                                    Connection = connection,
                                    CommandType = CommandType.Text,
                                };
                                createCommand1.ExecuteNonQuery();
                            }
                        }
                    }

                    // Segunda Verificação
                    const string verifyIndex2 = @"select count(1) as resultado
                                            from user_indexes i
                                            where i.INDEX_NAME = 'IX_GER_USUARIOS_LOGADOS_DATAS'";
                    OracleCommand commandIndex2 = new OracleCommand
                    {
                        CommandText = verifyIndex2,
                        Connection = connection,
                        CommandType = CommandType.Text,
                    };

                    using (OracleDataReader reader2 = commandIndex2.ExecuteReader())
                    {
                        if (reader2.Read())
                        {
                            int contador = reader2.GetInt32(0);

                            if (contador.Equals(0))
                            {
                                Cursor.Current = Cursors.WaitCursor;
                                countTemp++;
                                // MessageBox.Show("Criando os Índices necessários para execução da aplicação...", "Criando os Índices", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                const string createIndex2 = "create index IX_GER_USUARIOS_LOGADOS_DATAS on GER_USUARIOS_LOGADOS (dthr, dthr_saida)";
                                OracleCommand createCommand2 = new OracleCommand
                                {
                                    CommandText = createIndex2,
                                    Connection = connection,
                                    CommandType = CommandType.Text,
                                };
                                createCommand2.ExecuteNonQuery();
                            }
                        }
                    }

                    if (countTemp >= 1)
                        MessageBox.Show("Os Índices necessários foram criados com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Verifica a Existência da função fnc_usu_log
                    const string procedureVerify2 = "select count(1) from user_procedures o where upper(o.object_type) = 'FUNCTION' and upper(o.object_name) = 'FNC_USU_LOG3'";
                    OracleCommand commandVerify2 = new OracleCommand
                    {
                        CommandText = procedureVerify2,
                        Connection = connection,
                        CommandType = CommandType.Text,
                    };

                    using (OracleDataReader reader2 = commandVerify2.ExecuteReader())
                    {
                        if (reader2.Read())
                        {
                            int contador2 = reader2.GetInt32(0);

                            if (contador2.Equals(1))
                            {
                                DialogResult result2 = MessageBox.Show("Importante - A function fnc_usu_log3 já existe, deseja executar mesmo assim?", "Importante", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                if (result2.Equals(DialogResult.Yes))
                                {
                                    contador2 = 0;
                                }
                                else
                                {
                                    contador2 = 1;
                                }
                            }

                            if (contador2 < 1)
                            {
                                Cursor.Current = Cursors.WaitCursor;
                                const string fncString = @"
create or replace function fnc_usu_log3(p_tipo   in char,
                                        p_codemp in number,
                                        p_data   in date) return number is
   v_aux number := 0;
begin
   if p_tipo in ('U', 'T') then
      select count(1) + v_aux
        into v_aux
        from ger_usuarios_logados l
       where (p_codemp is null or l.empresa = p_codemp)
         and p_data between l.dthr and nvl(l.dthr_saida - 1 / 24 / 60 / 60, sysdate + 1);
   end if;
   if p_tipo in ('C', 'T') then
      select count(1) + v_aux
        into v_aux
        from wms_colaboradores_logados c
       where (p_codemp is null or c.empr_codemp = p_codemp)
         and p_data between c.dthr_ent and nvl(c.dthr_saida - 1 / 24 / 60 / 60, sysdate + 1);
   end if;
   return v_aux;
exception
   when no_data_found then
      return 0;
end;
";

                                OracleCommand commandFNC = new OracleCommand
                                {
                                    CommandText = fncString,
                                    Connection = connection,
                                    CommandType = CommandType.Text,
                                };
                                commandFNC.ExecuteNonQuery();
                                MessageBox.Show("A função fnc_usu_log3 foi gerada com sucesso!", "Importate!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }

                    //Verifica se a Procedure prc_full_wms_licencas existe na Base Conectada.
                    const string procedureVerify = "select count(1) from user_procedures o where upper(o.object_type) = 'PROCEDURE' and upper(o.object_name) = 'PRC_FULLWMS_LICENCAS'";
                    OracleCommand commandVerify = new OracleCommand
                    {
                        CommandText = procedureVerify,
                        Connection = connection,
                        CommandType = CommandType.Text,
                    };

                    using (OracleDataReader reader = commandVerify.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int contador = reader.GetInt32(0);

                            if (contador.Equals(1))
                            {
                                DialogResult result = MessageBox.Show("Importante - A procedure prc_fullwms_licencas já existe, deseja executar mesmo assim?", "Importante", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                if (result.Equals(DialogResult.Yes))
                                    contador = 0;
                                else
                                    contador = 1;
                            }

                            if (contador < 1)
                            {
                                Cursor.Current = Cursors.WaitCursor;
                                const string prcString = @"
create or replace procedure prc_fullwms_licencas(p_tipo        in number,
                                                 p_data_inicio in varchar2,
                                                 p_data_fim    in varchar2,
                                                 p_codemp      in number default null,
                                                 p_retorno     out sys_refcursor) is

   v_data_inicio date := to_date(p_data_inicio, 'DD/MM/YYYY');
   v_data_fim    date := to_date(p_data_fim, 'DD/MM/YYYY');

begin

   if p_tipo = 1 then
   
      open p_retorno for
         select l.dthr as data_entrada,
                l.dthr_saida as data_saida,
                l.empresa as empresa,
                l.ger_usuario_id as usuario_id,
                fnc_usu_log3('U', p_codemp, l.dthr) as usuarios_logados
           from ger_usuarios_logados l
          where l.dthr >= v_data_inicio
            and (p_codemp is null or p_codemp = l.empresa)
            and (l.dthr_saida is null or l.dthr < v_data_fim + 1)
          order by l.dthr asc;
   
   elsif p_tipo = 2 then
   
      open p_retorno for
      
         select c.dthr_ent as data_entrada,
                c.dthr_saida as data_saida,
                c.empr_codemp as empresa,
                c.colab_cod_colab as colab_id,
                fnc_usu_log3('C', p_codemp, c.dthr_ent) as colaboradores_logados
           from wms_colaboradores_logados c
          where c.dthr_ent >= v_data_inicio
            and (p_codemp is null or p_codemp = c.empr_codemp)
            and (c.dthr_saida is null or c.dthr_ent < v_data_fim + 1)
          order by c.dthr_ent;
   
   elsif p_tipo = 3 then
   
      open p_retorno for
      
         select x.*,
                to_char(max(x.total) over(partition by trunc(x.data_entrada))) || ' / ' ||
                to_char(max(x.total) over()) as max_diario
           from (select l.dthr as data_entrada,
                        l.dthr_saida as data_saida,
                        l.empresa as empresa,
                        fnc_usu_log3('U', p_codemp, l.dthr) as usuarios_logados,
                        0 as colabs_logados,
                        fnc_usu_log3('T', p_codemp, l.dthr) as total
                   from ger_usuarios_logados l
                  where l.dthr >= v_data_inicio
                    and (p_codemp is null or p_codemp = l.empresa)
                    and (l.dthr_saida is null or l.dthr < v_data_fim + 1)
                 union all
                 select c.dthr_ent,
                        c.dthr_saida,
                        c.empr_codemp,
                        0 as usuarios_logados,
                        fnc_usu_log3('C', p_codemp, c.dthr_ent) as colaboradores_logados,
                        fnc_usu_log3('T', p_codemp, c.dthr_ent) as total
                   from wms_colaboradores_logados c
                  where c.dthr_ent >= v_data_inicio
                    and (p_codemp is null or p_codemp = c.empr_codemp)
                    and (c.dthr_saida is null or c.dthr_ent < v_data_fim + 1)) x
          order by data_entrada asc;
   
   elsif p_tipo = 4 then
      open p_retorno for
         select data_entrada,
                sum(max_usuarios) as max_usuarios,
                sum(max_colabs) as max_colabs
           from (select data_entrada,
                        to_char(max(usuarios)) as max_usuarios,
                        to_char(max(colabs_logados)) as max_colabs
                   from (select trunc(l.dthr) as data_entrada,
                                fnc_usu_log3('U', p_codemp, l.dthr) as usuarios,
                                0 as colabs_logados
                           from ger_usuarios_logados l
                          where l.dthr >= v_data_inicio
                            and l.dthr < v_data_fim + 1
                         union all
                         select trunc(c.dthr_ent),
                                0 as usuarios_logados,
                                fnc_usu_log3('C', p_codemp, c.dthr_ent) as colaboradores_logados
                           from wms_colaboradores_logados c
                          where c.dthr_ent >= v_data_inicio
                            and c.dthr_ent < v_data_fim + 1)
                  group by data_entrada
                 
                 union all
                 
                 select v_data_inicio + level - 1 as data_entrada,
                        '0' as max_usuarios,
                        '0' as max_colabs
                   from dual
                 connect by level <= (v_data_fim - v_data_inicio + 1))
          group by data_entrada
          order by data_entrada;
   end if;

end;
";

                                OracleCommand commandPRC = new OracleCommand
                                {
                                    CommandText = prcString,
                                    Connection = connection,
                                    CommandType = CommandType.Text
                                };
                                commandPRC.ExecuteNonQuery();
                                MessageBox.Show("A função prc_fullwms_licencas foi gerada com sucesso!", "Importate!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Houve um erro ao gerar a Query, tente novamente! {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        public void RemoveQuery()
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(this.ConnectionString))
                {
                    connection.Open();
                    const string procedureVerify = "select count(1) from user_procedures o where upper(o.object_type) = 'PROCEDURE' and upper(o.object_name) = 'PRC_FULLWMS_LICENCAS'";
                    OracleCommand commandVerify = new OracleCommand
                    {
                        CommandText = procedureVerify,
                        Connection = connection,
                        CommandType = CommandType.Text
                    };

                    using (OracleDataReader reader = commandVerify.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int contador = reader.GetInt32(0);
                            if (contador == 0)
                            {
                                MessageBox.Show("Não existe a procedure PRC_FULLWMS_LICENCAS nesta base.", "Importante!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            else
                            {
                                DialogResult result = MessageBox.Show("Você tem certeza que deseja remover a procedure PRC_FULLWMS_LICENCAS nesta base?", "Importante!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                if (result.Equals(DialogResult.Yes))
                                {
                                    const string removeProcedure = "drop procedure PRC_FULLWMS_LICENCAS";
                                    OracleCommand deleteCommand = new OracleCommand
                                    {
                                        CommandText = removeProcedure,
                                        Connection = connection,
                                        CommandType = CommandType.Text
                                    };
                                    deleteCommand.ExecuteNonQuery();
                                    MessageBox.Show("Procedure PRC_FULLWMS_LICENCAS removida com sucesso desta base.", "Importante!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }

                                const string procedureVerify2 = "select count(1) from user_procedures o where upper(o.object_type) = 'FUNCTION' and upper(o.object_name) = 'FNC_USU_LOG3'";
                                OracleCommand commandVerify2 = new OracleCommand
                                {
                                    CommandText = procedureVerify2,
                                    Connection = connection,
                                    CommandType = CommandType.Text
                                };
                                using (OracleDataReader reader2 = commandVerify2.ExecuteReader())
                                {
                                    if (reader2.Read())
                                    {
                                        int contador2 = reader2.GetInt32(0);

                                        if (contador2 == 0)
                                        {
                                            MessageBox.Show("Não existe a procedure FNC_USU_LOG3 nesta base.", "Importante!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return;
                                        }
                                        else
                                        {
                                            DialogResult result2 = MessageBox.Show("Você tem certeza que deseja remover a procedure FNC_USU_LOG3 nesta base?", "Importante!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                            if (result2.Equals(DialogResult.Yes))
                                            {
                                                const string removeProcedure = "drop function FNC_USU_LOG3";
                                                OracleCommand deleteCommand = new OracleCommand
                                                {
                                                    CommandText = removeProcedure,
                                                    Connection = connection,
                                                    CommandType = CommandType.Text
                                                };
                                                deleteCommand.ExecuteNonQuery();
                                                MessageBox.Show("Procedure FNC_USU_LOG3 removida com sucesso desta base.", "Importante!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Houve um erro ao remover a Query, tente novamente! {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
