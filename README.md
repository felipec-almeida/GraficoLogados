# Projeto - Gráfico FullWMS

> Esta aplicação possui o objetivo de gerar um gráfico com informações detalhadas sobre os logins durante um determinado período determinado pelo usuário. O foco principal da empresa é o WMS (Warehouse Management System), com o objetivo de auxiliar clientes, automatizar e facilitar o gerenciamento de seus estoques.
> 
> Trata-se de uma aplicação desenvolvida em *C#* que se comunica com o Banco de Dados *Oracle (PLSQL)* do Cliente. A aplicação importa as *Functions e Procedures* necessárias para seu funcionamento. Ambas procuram os dados nas tabelas *ger_usuarios_logados e wms_colaboradores_logados*, obtendo ass informações necessárias para gerar o gráfico corretamente. Após serem retornados os dados da base oracle selecionada para o C#, irá ser disponibilizada a opção para gerar os gráficos, além de outras funções úteis, como *Salvar Strings de Conexões em um Arquivo .JSON*, *Importar automaticamente as Procedures e Funcitons necessárias para a geração do Gráfico*, *Conexão automática com o banco de dados através da própria aplicação*, *Junção de Dados de Bases distintas*, *Quatro tipos diferentes para geração de Gráficos*, *DataGrid com informações detalhadas dos logados* e outras validações e funcionalidades importantes para o funcionamento da aplicação.

# Funcionalidades
## Interface Inicial

![image](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/6169b2d9-078c-4b4a-8eae-dc642436bf4b)

## Conexão ao Banco de Dados
> A conexão com o banco de dados é feita a partir dessa tela, ressalta-se apenas a compatibilidade com o *PL-SQL*. No formulário, ele irá pedir todas as informações necessárias para conseguir se conectar ao banco. Se todos os dados forem fornecidos corretamente, o formulário irá tentar se conectar ao banco de dados, informando que conseguiu se conectar na base informada. É possível: *Selecionar Conexões Salvas, Salvar Conexões, Duplicar / Remover Conexões, Atualizar / Salvar uma nova Conexão.*

![image](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/f4f3334d-cb5e-4d32-8047-96b1f0874462)

### Salvar Conexões
> Esta funcionalidade permite o usuário salvar suas conexões sem precisar ter de colocar os dados toda vez que entrar na aplicação. Ela fica salva num arquivo *.JSON (stringConnection.json)* que guarda todas as conexões salvas até o momento. Junto com essa funcionalidade, é possível atualizar os dados de conexão que foram salvas no arquivo .JSON, além de remove-los caso seja necessário.

**Exemplo de Conexão Salva no Arquivo .JSON:**
> *[{
    "nomeConexao": "",
    "server": "",
    "porta": "",
    "dataBase": "",
    "usuario": "",
    "senha": ""
  }]*

### Importar ou Remover *Procedures e Functions*
> Trata-se de uma funcionalidade que irá disponibilizar ao usuário a opção de importar as procedures e funcitions na base selecionada (serve também como atualização caso já exista na base), ou removê-la caso desejar. Esta funcionalidade também cria os índices necessários nas tabelas.

#### Importar *Procedures e Functions*
![Capturar](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/643d060a-3e72-4af8-a8cf-4d443c637be0)
![Capturar](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/bcd1e505-d3e1-4e9e-89d5-5c992b2cea63)
![Capturar](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/6a4ea502-591a-4a88-9605-fb514f719e91)

#### Remover *Procedures e Functions*
![Capturar](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/0536ac9a-9f62-4fd9-bb85-91a424a277ba)
![Capturar](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/5ee7d863-8b2a-408d-810f-d4b28f058202)

### Gerar Gráfico
> A funcionalidade principal da aplicação. Recebe todos os parâmetros fornecidos pelo Base do Banco de Dados conectado, gerando então um gráfico no qual mostra o total de logados durante uma determinada data, tal qual é especificada pelo usuário.
 
![image](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/46aa7cd2-89ee-4b70-b2fb-bd363053366f)

#### Tipos de Retorno
> São possíveis 4 tipos de retorno para a geração dos Gráficos. Após o gráfico ser gerado, ao passar o mouse sobre uma parte do mesmo, irá mostrar informações relevantes, como por exemplo a data e a quantidade de logados nesta data. Quando pressionado em um dado específico gerado pelo gráfico, irá mostrar uma nova tela, detalhando os usuários logados naquela data.

1. O Tipo 1 retorna o total de **Usuários** logados durante a data fornecida.
2. O Tipo 2 retorna o total de **Colaboradores** logados durante a data fornecida.
3. O Tipo 3 retorna a quantidade total de Logados durante a data fornecida pelo usuário, unificando os dois valores em um só Gráfico.
4. O Tipo 4 retorna o total de **Usuários e Colaboradores** logados durante a data fornecida, criando um gráfico de colunas mostrando a quantidade de *Usuários e Colaboradores Logados, além de uma linha que traça a quantidade total dentre os dois, unificando-os.
5. O Tipo 5 possibilita a junção de dados dentre bases distintas, obtendo todos os valores para em seguida gerar um gráfico do *Tipo 4*.

#### Tipo 1
![image](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/b9ac717b-e430-44af-a368-8ba0755bbe86)
![image](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/89d80101-7767-495f-8b87-d708881c114e)


#### Tipo 2
![image](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/78cf2bea-a83f-47e0-bb56-a24d72cdec44)
![image](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/8fdfd54e-e91d-4de5-9770-78266264ba0e)


#### Tipo 3
![image](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/4f061c94-d750-41a6-9f9a-253bbf38e82c)
![image](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/4b0cbddb-d772-415c-b033-df8eb858d56b)

**Tipo 'C': Colaborador | 'W': Web**

#### Tipo 4
![image](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/efe02a94-d7bb-45bd-bc62-f8d5ac15f3dd)

#### Tipo 5
Quando for gerado o gráfico do tipo 4, irá perguntar ao usuário se gostaria de juntar os dados com outra base, para em seguida mostrar a tela para selecionar as bases que deseja unificar os dados.

![image](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/6f9fc6a3-6dff-43b2-9ffc-267edb4dc8a5)
![image](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/9d6821f8-0364-4a57-930b-ada6ece0d583)

Após selecionar as bases desejadas, basta clicar em *'Finalizar'*, que irá começar a gerar o gráfico com os dados unificados.

![image](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/1bda03d3-21a5-4b83-ace4-1b4fbc78c610)

