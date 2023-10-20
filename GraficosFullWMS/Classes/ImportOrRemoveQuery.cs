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

                    //Verifica se Existe a Tabela GER_LOGADOS, e as triggers.
                    const string verifyTable = @"
select count(1) from user_tables u
where upper(u.TABLE_NAME) = 'GER_LOGADOS'";

                    OracleCommand commandTable = new OracleCommand
                    {
                        CommandText = verifyTable,
                        Connection = connection,
                        CommandType = CommandType.Text,
                    };

                    using (OracleDataReader reader = commandTable.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int contador = reader.GetInt32(0);

                            if (contador.Equals(0))
                            {
                                Cursor.Current = Cursors.WaitCursor;

                                const string createTable = @"
create table GER_LOGADOS
(
  dthr     DATE default sysdate,
  tipo     CHAR(1),
  empresa  NUMBER(10),
  codigo   NUMBER(10),
  id_login NUMBER(10),
  logado   NUMBER(10),
  total    NUMBER(10)
)
";
                                OracleCommand createCommand = new OracleCommand
                                {
                                    CommandText = createTable,
                                    Connection = connection,
                                    CommandType = CommandType.Text,
                                };
                                createCommand.ExecuteNonQuery();

                                //Cria as Triggers

                                const string triggerColaboradores = @"
create or replace trigger trg_wms_colaboradores_logados
   before insert on wms_colaboradores_logados
   for each row
declare
   v_aux   number;
   v_total number;
begin
   /* Procura na tabela se na mesma data de login houve um ou mais logins */

   select count(1) + 1
     into v_aux
     from wms_colaboradores_logados c
    where c.empr_codemp = :new.empr_codemp
      and c.dthr_saida is null;

   select count(1) + 1 + v_aux
     into v_total
     from ger_usuarios_logados l
    where l.empresa = :new.empr_codemp
      and l.dthr_saida is null;

   insert into ger_logados
      (tipo,
       empresa,
       codigo,
       id_login,
       logado,
       total)
   values
      ('C',
       :new.empr_codemp,
       :new.colab_cod_colab,
       :new.colog_id,
       v_aux,
       v_total);

   -- exception
   --    when others then
   --       null;
end;
";

                                OracleCommand createTrigger1 = new OracleCommand
                                {
                                    CommandText = triggerColaboradores,
                                    Connection = connection,
                                    CommandType = CommandType.Text,
                                };
                                createTrigger1.ExecuteNonQuery();

                                const string triggerUsuarios = @"
create or replace trigger trg_ger_usuarios_logados
   before insert on ger_usuarios_logados
   for each row
declare
   v_aux   number;
   v_total number;
begin
   /* Procura na tabela se na mesma data de login houve um ou mais logins */
   select count(1) + 1
     into v_aux
     from ger_usuarios_logados l
    where l.empresa = :new.empresa
      and l.dthr_saida is null;

   select count(1) + 1
     into v_aux
     from wms_colaboradores_logados c
    where c.empr_codemp = :new.empresa
      and c.dthr_saida is null;

   insert into ger_logados
      (tipo,
       empresa,
       codigo,
       id_login,
       logado,
       total)
   values
      ('U',
       :new.empresa,
       :new.ger_usuario_id,
       :new.ger_usuariologado_id,
       v_aux,
       v_total);

   -- exception
   --    when others then
   --       null;
end;
";

                                OracleCommand createTrigger2 = new OracleCommand
                                {
                                    CommandText = triggerUsuarios,
                                    Connection = connection,
                                    CommandType = CommandType.Text,
                                };
                                createTrigger2.ExecuteNonQuery();
                            }
                        }
                    }

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

                    //Verifica se a Package pkg_wms_full_lic existe na Base Conectada.
                    const string procedureVerify = "select count(1) from user_objects o where upper(o.object_type) = 'PACKAGE' and upper(o.object_name) = 'PKG_WMS_FULL_LIC'";
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
                                DialogResult result = MessageBox.Show("Importante - A package pkg_wms_full_lic já existe, deseja executar mesmo assim?", "Importante", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                if (result.Equals(DialogResult.Yes))
                                    contador = 0;
                                else
                                    contador = 1;
                            }

                            if (contador < 1)
                            {
                                Cursor.Current = Cursors.WaitCursor;

                                const string stringPkgHead = @"
create or replace package pkg_wms_full_lic is

   ----------------------
   -- Versão 22.12.001
   ----------------------

   /*------------------------------------------------------------------------*/
   /* prc_aud_ger_logados                                                    */
   /*------------------------------------------------------------------------*/

   procedure prc_aud_ger_logados(p_tipo in varchar2);

   /*------------------------------------------------------------------------*/
   /* prc_fullwms_licencas                                                   */
   /*------------------------------------------------------------------------*/

   procedure prc_fullwms_licencas(p_tipo        in number,
                                  p_data_inicio in varchar2,
                                  p_data_fim    in varchar2,
                                  p_codemp      in number default null,
                                  p_retorno     out sys_refcursor);

end pkg_wms_full_lic;
";

                                OracleCommand commandPKGHead = new OracleCommand
                                {
                                    CommandText = stringPkgHead,
                                    Connection = connection,
                                    CommandType = CommandType.Text
                                };
                                commandPKGHead.ExecuteNonQuery();

                                const string stringPkgBody = @"
create or replace package body pkg_wms_full_lic is

   ----------------------
   -- Versão 22.12.001
   ----------------------

   /*------------------------------------------------------------------------*/
   /* prc_seleciona_almox_cd                                                 */
   /*------------------------------------------------------------------------*/

   procedure prc_aud_ger_logados(p_tipo in varchar2) as
   
   begin
   
      if p_tipo = 'U' then
      
         for c in (select l.dthr as dthr,
                          'U' as tipo,
                          l.empresa as empresa,
                          l.ger_usuario_id as codigo,
                          l.ger_usuariologado_id as id_login,
                          fnc_usu_log3('U', '', l.dthr) as usuarios_logados,
                          fnc_usu_log3('T', '', l.dthr) as total
                     from ger_usuarios_logados l
                    where not exists (select 1
                             from ger_logados gl
                            where gl.id_login = l.ger_usuariologado_id)
                      and l.dthr >= sysdate - 55)
         loop
            insert into ger_logados
               (dthr,
                tipo,
                empresa,
                codigo,
                id_login,
                logado,
                total)
            values
               (c.dthr,
                c.tipo,
                c.empresa,
                c.codigo,
                c.id_login,
                c.usuarios_logados,
                c.total);
            commit;
         end loop;
      
      elsif p_tipo = 'C' then
      
         for c in (select c.dthr_ent as dthr,
                          'C' as tipo,
                          c.empr_codemp as empresa,
                          c.colab_cod_colab as codigo,
                          c.colog_id as id_login,
                          fnc_usu_log3('C', '', c.dthr_ent) as colaboradores_logados,
                          fnc_usu_log3('T', '', c.dthr_ent) as total
                     from wms_colaboradores_logados c
                    where not exists (select 1
                             from ger_logados gl
                            where gl.id_login = c.colog_id)
                      and c.dthr_ent >= sysdate - 55)
         loop
            insert into ger_logados
               (dthr,
                tipo,
                empresa,
                codigo,
                id_login,
                logado,
                total)
            values
               (c.dthr,
                c.tipo,
                c.empresa,
                c.codigo,
                c.id_login,
                c.colaboradores_logados,
                c.total);
            commit;
         end loop;
      
      elsif p_tipo = 'T' then
      
         for c in (select l.dthr as dthr,
                          'U' as tipo,
                          l.empresa as empresa,
                          l.ger_usuario_id as codigo,
                          l.ger_usuariologado_id as id_login,
                          fnc_usu_log3('U', '', l.dthr) as usuarios_logados,
                          fnc_usu_log3('T', '', l.dthr) as total
                     from ger_usuarios_logados l
                    where not exists (select 1
                             from ger_logados gl
                            where gl.id_login = l.ger_usuariologado_id)
                      and l.dthr >= sysdate - 55)
         loop
            insert into ger_logados
               (dthr,
                tipo,
                empresa,
                codigo,
                id_login,
                logado,
                total)
            values
               (c.dthr,
                c.tipo,
                c.empresa,
                c.codigo,
                c.id_login,
                c.usuarios_logados,
                c.total);
            commit;
         end loop;
      
         for c in (select c.dthr_ent as dthr,
                          'C' as tipo,
                          c.empr_codemp as empresa,
                          c.colab_cod_colab as codigo,
                          c.colog_id as id_login,
                          fnc_usu_log3('C', '', c.dthr_ent) as colaboradores_logados,
                          fnc_usu_log3('T', '', c.dthr_ent) as total
                     from wms_colaboradores_logados c
                    where not exists (select 1
                             from ger_logados gl
                            where gl.id_login = c.colog_id)
                      and c.dthr_ent >= sysdate - 55)
         loop
            insert into ger_logados
               (dthr,
                tipo,
                empresa,
                codigo,
                id_login,
                logado,
                total)
            values
               (c.dthr,
                c.tipo,
                c.empresa,
                c.codigo,
                c.id_login,
                c.colaboradores_logados,
                c.total);
            commit;
         end loop;
      
      end if;
   
   end;

   /*------------------------------------------------------------------------*/
   /* prc_fullwms_licencas                                                   */
   /*------------------------------------------------------------------------*/

   procedure prc_fullwms_licencas(p_tipo        in number,
                                  p_data_inicio in varchar2,
                                  p_data_fim    in varchar2,
                                  p_codemp      in number default null,
                                  p_retorno     out sys_refcursor) is
   
      v_data_inicio date := to_date(p_data_inicio, 'DD/MM/YYYY');
      v_data_fim    date := to_date(p_data_fim, 'DD/MM/YYYY');
   
   begin
   
      if p_tipo = 1 then
      
         open p_retorno for
            select gl.dthr   as data,
                   gl.logado as logados
              from ger_logados gl
             where (gl.dthr >= v_data_inicio)
               and gl.dthr < v_data_fim + 1
               and (p_codemp is null or gl.empresa = p_codemp)
               and gl.tipo = 'U'
             order by dthr asc;
      
      elsif p_tipo = 2 then
      
         open p_retorno for
         
            select gl.dthr   as data,
                   gl.logado as logados
              from ger_logados gl
             where (gl.dthr >= v_data_inicio)
               and gl.dthr < v_data_fim + 1
               and (p_codemp is null or gl.empresa = p_codemp)
               and gl.tipo = 'C'
             order by dthr asc;
      
      elsif p_tipo = 3 then
      
         open p_retorno for
         
            select gl.dthr as data,
                   gl.empresa,
                   case
                      when gl.tipo = 'U' then
                       gl.logado
                      else
                       0
                   end as usuarios,
                   case
                      when gl.tipo = 'C' then
                       gl.logado
                      else
                       0
                   end as colaboradores,
                   gl.total
              from ger_logados gl
             where (gl.dthr >= v_data_inicio)
               and gl.dthr < v_data_fim + 1
               and (p_codemp is null or gl.empresa = p_codemp)
             order by 1;
      
      elsif p_tipo = 4 then
      
         open p_retorno for
         
            select data_entrada,
                   sum(max_usuarios) as max_usuarios,
                   sum(max_colabs) as max_colabs,
                   sum(max_total) as max_total
              from (select data_entrada,
                           to_char(max(usuarios)) as max_usuarios,
                           to_char(max(colabs_logados)) as max_colabs,
                           to_char(max(total)) as max_total
                      from (select trunc(l.dthr) as data_entrada,
                                   case
                                      when l.tipo = 'U' then
                                       l.logado
                                      else
                                       0
                                   end as usuarios,
                                   case
                                      when l.tipo = 'C' then
                                       l.logado
                                      else
                                       0
                                   end as colabs_logados,
                                   l.total as total
                              from ger_logados l
                             where l.dthr >= v_data_inicio
                               and l.dthr < v_data_fim + 1)
                     group by data_entrada
                    
                    union all
                    
                    select v_data_inicio + level - 1 as data_entrada,
                           '0' as max_usuarios,
                           '0' as max_colabs,
                           '0' as max_total
                      from dual
                    connect by level <= (v_data_fim - v_data_inicio + 1))
             group by data_entrada
             order by data_entrada;
      
      end if;
   
   end;

end pkg_wms_full_lic;
";

                                OracleCommand commandPKGBody = new OracleCommand
                                {
                                    CommandText = stringPkgBody,
                                    Connection = connection,
                                    CommandType = CommandType.Text
                                };
                                commandPKGBody.ExecuteNonQuery();
                                MessageBox.Show("A package pkg_wms_full_lic foi gerada com sucesso!", "Importate!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    const string procedureVerify = "select count(1) from user_objects o where upper(o.object_type) = 'PACKAGE' and upper(o.object_name) = 'PKG_WMS_FULL_LIC'";
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
                                MessageBox.Show("Não existe a package PKG_WMS_FULL_LIC nesta base.", "Importante!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            else
                            {
                                DialogResult result = MessageBox.Show("Você tem certeza que deseja remover a package PKG_WMS_FULL_LIC nesta base?", "Importante!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                if (result.Equals(DialogResult.Yes))
                                {
                                    const string removeProcedure = "drop package PKG_WMS_FULL_LIC";
                                    OracleCommand deleteCommand = new OracleCommand
                                    {
                                        CommandText = removeProcedure,
                                        Connection = connection,
                                        CommandType = CommandType.Text
                                    };
                                    deleteCommand.ExecuteNonQuery();
                                    MessageBox.Show("Package PKG_WMS_FULL_LIC removida com sucesso desta base.", "Importante!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
