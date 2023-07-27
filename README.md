# Projeto - Gráfico de Logados

> Este foi um projeto no qual desenvolvi durante meu expediente de trabalho como estagiário, onde havia a necessidade de ter o conhecimento de quantos usuários estariam logados simultâneamente numa data específica, a fim de conseguir ter um controle do uso destes usuários ao software provido pela empresa na qual estou atualmente.
>
> O foco principal da empresa é o WMS (Warehouse Management System), com o objetivo de auxiliar clientes, automatizar e facilitar o gerenciamento de seus estoques.
>
> O projeto em si, tive uma ajuda dos meus colegas de trabalho, que me ajudaram a resolver alguns problemas e me deram suporte e sugestões de melhorias à aplicação que desenvolvi.
>
> Trata-se de uma aplicação desenvolvida em *C#* que se comunica com o banco de Dados *Oracle (PLSQL)* do Cliente. A aplicação importa as *Procedures e Functions* necessárias para seu funcionamento. Após importar, a aplicação executa-os e recebe os parâmetros obtidos por duas tabelas chamadas *ger_usuarios_logados e wms_colaboradores_logados*, utilizados por estas *Procedures e Functions*, criando assim um relatório com informações mais detalhadas do login do usuário, como a *Data de Entrada e Data de Saída* que um usuário em específico se logou, o seu *ID*, o *Máximo de Logados Diários* e dentre outros.
>
> A segunda parte desta aplicação, consiste em obter todos os valores gerados no banco oracle, e serem retornados para o C#, no qual irá disponibilizar a opção para gerar os gráficos, além de outras funções úteis, como *Salvar Strings de Conexões em um Arquivo .JSON*, *Importar automaticamente as Procedures e Funcitons necessárias para a geração do Gráfico*, *Conexão automática com o banco de dados através da própria aplicação* e algumas outras validações e funcionalidades importantes para o funcionamento da aplicação em si.

# Funcionalidades
> O projeto que desenvolvi, foi focado principalmente no Code Behind, logo, sua estética não é a das melhores pois, esta foi a minha primeira interação com o WindowsForms do C#.

## Interface Inicial

![Capturar](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/9db8cfc8-a413-403b-bf40-d6f0a634a3c4)

## Conexão ao Banco de Dados
> A conexão com o banco de dados, por ora, não possui suporte a outros tipos de banco de dados, como por exemplo *MySQL, SQLServer, PostgreSQL* e dentre outros. Pretendo implementar essa funcionalidade no futuro, mas por enquanto, apenas há compatibilidade com o *PLSQL*. No formulário, ele irá pedir todas as informações necessárias para conseguir se conectar ao banco. Se todos os dados forem fornecidos corretamente, o formulário irá tentar se conectar ao banco de dados, informando que conseguiu se conectar na base informada.

![Capturar](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/92567569-d09e-4cf4-b86a-90eb31c3f1bf)
![Capturar](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/fbcf9e72-b6ff-4245-a500-bdfe240e4387)

### Salvar Conexões
> Esta funcionalidade permite o usuário salvar suas conexões sem precisar ter de colocar os dados toda vez que entrar na aplicação. Ela fica salva num arquivo *.JSON (stringConnection.json)* que guarda todas as conexões salvas até o momento. Junto com essa funcionalidade, é possível atualizar os dados de conexão que foram salvas no arquivo .JSON, além de remove-los caso seja necessário.

**Exemplo de Conexão Salva:**
> *[
  {
    "server": "123.45.678.910",
    "porta": "1521",
    "dataBase": "baseExemplo",
    "usuario": "admin",
    "senha": "admin"
  }]*

### Importar ou Remover *Procedures e Functions*
> Trata-se de uma funcionalidade que irá disponibilizar ao usuário a opção de importar as procedures e funcitions na base selecionada (serve também como atualização caso já exista na base), ou removê-la caso desejar.

#### Importar *Procedures e Functions*
![Capturar](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/643d060a-3e72-4af8-a8cf-4d443c637be0)
![Capturar](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/bcd1e505-d3e1-4e9e-89d5-5c992b2cea63)
![Capturar](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/6a4ea502-591a-4a88-9605-fb514f719e91)

#### Remover *Procedures e Functions*
![Capturar](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/0536ac9a-9f62-4fd9-bb85-91a424a277ba)
![Capturar](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/5ee7d863-8b2a-408d-810f-d4b28f058202)

### Gerar Gráfico
> A funcionalidade principal da aplicação. Recebe todos os parâmetros fornecidos pelo Base do Banco de Dados conectado, gerando então um gráfico no qual mostra o total de logados durante uma determinada data, tal qual é especificada pelo usuário.
 
![Capturar](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/1ef87ab6-c29c-4360-9924-c13abf87c9f6)

#### Tipos de Retorno
> São possíveis 3 tipos de retorno para a geração dos Gráficos. Após o gráfico ser gerado, ao passar o mouse sobre uma parte do mesmo, irá mostrar informações relevantes, como por exemplo a data e a quantidade de logados nesta data.
1. O Tipo 1 retorna o total de **Usuários** logados durante a data fornecida.
2. O Tipo 2 retorna o total de **Colaboradores** logados durante a data fornecida.
3. O Tipo Três retorna o total de **Usuários e Colaboradores** logados durante a data fornecida, unificando os dois valores em um só Gráfico. Ressalta-se que este Tipo em específico pode demorar brevemente para ser executado, dependendo da quantidade de **Usuários e/ou Colaboradores** logados durante a data Fornecida.

#### Tipo 1
![Capturar](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/3f7f6cfc-6687-4498-8398-2c50827b7b2e)

#### Tipo 2
![Capturar](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/d26eccee-65e1-489b-b440-1873ccd28c57)

#### Tipo 3
![Capturar](https://github.com/felipec-almeida/GraficoLogados/assets/122905385/e5755e62-6538-4ead-b503-ee72e3950b3b)
