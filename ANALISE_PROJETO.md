# Análise técnica do projeto PromoRoboJob

## Visão geral
O repositório está organizado em uma arquitetura em camadas, com boa separação entre **domínio**, **contratos**, **aplicação**, **infraestrutura** e **processos de execução** (API/Worker/MockServer).

Fluxo principal atual:
1. O `OfertaWorker` executa em loop e percorre categorias configuradas.
2. Para cada categoria ativa, dispara o caso de uso `BuscarEEnviarPromocoesUseCase`.
3. O caso de uso busca ofertas via `IOfertaProvider`, valida desconto mínimo, evita duplicidade via `IOfertaRepository`, gera mensagem e envia via `INotificador`.

## Pontos fortes
- **Boa base arquitetural (Clean-ish):** entidades e interfaces independentes de infraestrutura.
- **Extensibilidade por abstrações:** provider/notificador/encurtador/repositório podem ser trocados por DI.
- **Observabilidade inicial:** logs em pontos importantes no worker e no notificador de WhatsApp.
- **Isolamento para testes locais:** `MockServer` e `TestOfertaProvider` ajudam no desenvolvimento sem depender imediatamente de serviços externos.

## Riscos e oportunidades de melhoria

### 1) Configuração e segredos
- Há valores sensíveis em `appsettings.json` (ex.: recipients e identificadores). Recomenda-se migrar para variáveis de ambiente/secret manager.
- Incluir `appsettings.Development.json` (não versionado) e placeholders seguros no repositório.

### 2) Confiabilidade de envio
- O caso de uso marca como enviada após iterar notificadores; se um canal falhar e outro funcionar, a oferta fica marcada mesmo com falha parcial.
- Sugerido: estratégia explícita de sucesso (ex.: marcar somente se ao menos 1 canal entregou) + retry/backoff por canal.

### 3) Repositório de deduplicação
- `OfertaRepository` em memória resolve MVP, mas não sobrevive reinício.
- Próximo passo: persistência em banco (SQLite/PostgreSQL/Redis) com chave idempotente estável por oferta.

### 4) Qualidade de domínio e validação
- `ValidadorPromocao` existe, mas não está integrado diretamente no caso de uso.
- Recomenda-se centralizar validação em um pipeline único para evitar divergência de regra no futuro.

### 5) Provider real ainda incompleto
- `TinyUrlEncurtador` está stubado retornando URL original.
- A integração com provider real (Mercado Livre) parece preparada via DI, mas atualmente `TestOfertaProvider` está fixo.

### 6) Bugs e dívida técnica rápida
- `OfertaWorker.cs` tem `using System;` duplicado.
- `GeradorMensagemPromocional` mostra caracteres estranhos no prefixo (provável encoding no emoji).
- README está vazio/incompleto (apenas “Add”), impactando onboarding.

## Prioridades sugeridas (ordem prática)
1. **Higiene de configuração e segredos** (baixo esforço, alto impacto).
2. **Persistência de idempotência** + política de sucesso de envio.
3. **Conectar `ValidadorPromocao` ao caso de uso**.
4. **Implementar encurtador real e feature toggle por provider**.
5. **Melhorar README com setup local, arquitetura e troubleshooting**.

## Maturidade atual
Projeto com estrutura promissora para automação de promoções, já com esqueleto sólido de modularização. Está em fase de MVP técnico, com foco em consolidar resiliência operacional e padronização de configuração para evoluir para produção.
