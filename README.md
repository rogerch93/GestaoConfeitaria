🍰 GestãoConfeitaria API com IA

API REST desenvolvida em .NET com autenticação via JWT e integração com Inteligência Artificial para geração de indicadores financeiros inteligentes.

O projeto simula um cenário real de negócio (gestão de confeitaria), permitindo controle de:

Gastos

Materiais

Vendas

Indicadores financeiros com análise automatizada por IA

🚀 Objetivo do Projeto

Aplicar na prática conceitos de:

Arquitetura de APIs REST

Autenticação e autorização com JWT

Boas práticas de desenvolvimento

Organização em camadas

Integração com IA para geração de insights estratégicos

Evolução contínua baseada em estudos de Cloud, IA e DevOps

🛠️ Tecnologias Utilizadas

.NET Web API

C#

Entity Framework Core

SQL Server

JWT (Json Web Token)

Integração com serviço de IA

Swagger (documentação da API)

🔐 Autenticação
Endpoint: /auth

Responsável por:

Login do usuário

Geração de token JWT

Proteção dos demais endpoints via [Authorize]

Após autenticação, o token deve ser enviado no header:

Authorization: Bearer {seu_token}
📌 Endpoints Disponíveis
💰 Gastos

GET /gastos
Retorna todos os registros de gastos.

POST /gastos
Insere um novo lançamento financeiro.

📦 Materiais

GET /materiais
Retorna os materiais cadastrados.

POST /materiais
Insere um novo material.

🛒 Vendas

GET /vendas
Retorna todas as vendas registradas.

POST /vendas
Insere uma nova venda.

📊 Indicadores (IA)

GET /indicadores?dataInicio=yyyy-MM-dd&dataFim=yyyy-MM-dd

Este é o principal diferencial da aplicação.

Esse endpoint:

Consolida dados de Gastos, Materiais e Vendas

Envia os dados para a IA

Gera uma análise financeira baseada no período informado

A IA entrega:

📈 Análise financeira detalhada

💡 Sugestões de redução de custos

⚙️ Recomendações para melhoria de eficiência operacional

📊 Insights estratégicos baseados em dados reais

🧱 Estrutura do Projeto

O projeto segue uma organização baseada em boas práticas:

Controllers → Camada de entrada (HTTP)

Services → Regras de negócio

Repositories → Acesso a dados

Models → Entidades

Configurações → JWT, banco de dados e serviços externos

🔎 Diferenciais Técnicos

Autenticação segura via JWT

Separação de responsabilidades

Integração com IA para geração de insights

Estrutura preparada para escalabilidade

Foco em boas práticas e arquitetura limpa

▶️ Como Executar o Projeto

Clonar o repositório:

git clone https://github.com/rogerch93/GestaoConfeitaria

Configurar a connection string no appsettings.json

Executar as migrations (se aplicável)

dotnet ef database update

Rodar a aplicação:

dotnet run

Acessar o Swagger:

https://localhost:{porta}/swagger
📈 Roadmap (Evoluções Futuras)

Implementação de Clean Architecture

Dockerização da aplicação (Ausente ainda)

👨‍💻 Autor

Rogério Chaves
Desenvolvedor Back-end C#

GitHub:
https://github.com/rogerch93

LinkedIn:
https://linkedin.com/in/rogerio-chaves-5437871a6
