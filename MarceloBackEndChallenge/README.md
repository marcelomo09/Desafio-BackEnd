# Utilização e conferencia das soluções para os desafios
## Inicializando
- Para usar o sistema primerio execute o comando do docker-compose para assim criar o container relicionado ao projeto para poder usar o MongoDB e RabbitMQ,
  também vale ressaltar que a aplicação utiliza Swagger que já encontra-se configurada no Program.cs
    - Comando para criar o container do docker-compose "docker-compose up -d"
    - No browser acesse o endereço http://localhost:5271/swagger/ para ter acesso as funcionalidades do sistema
        - Usuário admin previamente cadastrado "userName = admin" - "password = a123"
        - Ao entrar na página você terá que realizar a autenticação para usar as funcionalidades, o sistema valida autorizações tanto para admins quanto entregadores
        - Ocorrendo sucesso na autenticação copie o token que virá no campo message do response para o campo value do botão Authorize do Swagger

## Cadastrando usuários
- O usuário admin vem previamente cadastrado, as senhas são criptografadas por chaves SHA-256 e os unicos usuários com permissão de acesso as funções são do grupo admin
    - Para cadastrar novos usuários utilize o metodo CreateUser da seção User

## Soluções do Desafio
Todas as funcionalidades citadas aqui seram acessadas pelos métodos disponiveis no Swagger

- Eu como usuário admin quero cadastrar uma nova moto.
    - Acesse o método CreateMotorcycle da seção Motorcycle para incluír novas motos.

- Eu como usuário admin quero consultar as motos existentes na plataforma e conseguir filtrar pela placa.
    - Acesse o método GetMotorcycleByPlate da seção Motorcycle para fazer a consulta da moto pela placa.

- Eu como usuário admin quero modificar uma moto alterando apenas sua placa que foi cadastrado indevidamente.
    - Para realizar a alteração da placa utilize o método UpdateMotorcyclePlate da seção Motorcycle.

- Eu como usuário admin quero remover uma moto que foi cadastrado incorretamente, desde que não tenha registro de locações.
    - Para excluír uma moto acesse utilize o método DeleteMotorcycle da seção Motorcycle.

- Eu como usuário entregador quero me cadastrar na plataforma para alugar motos.
    - Para se cadastrar como entregador use o método CreateDeliveryman da seção Deliveryman.

- Eu como entregador quero enviar a foto de minha cnh para atualizar meu cadastro.
    - Para realizar essa requisição utilize o método UpdateImageCNHDeliveryman da seção Deliveryman.

- Eu como entregador quero alugar uma moto por um período.
    - Para realizar essa requisição utilize o método CreateMotorcycleRental da seção MotorcycleRental

- Eu como entregador quero informar a data que irei devolver a moto e consultar o valor total da locação.
    - Para realizar essa consulta utilize o método SimulationOfMotorcycleRentalValues da seção MotorcycleRental, o retorno será uma mensagem mostrando o valor da devolução da moto

- Eu como admin quero cadastrar um pedido na plataforma e disponibilizar para os entregadores aptos efetuarem a entrega.
    - Para realizar essa rotina utilize o método CreateRequestRace da seção RequestRace

- Eu como entregador quero aceitar um pedido.
    - Para o entregador aceitar um pedido utilize o método AcceptRequestRace da seção RequestRace, os dados do campo idRequestRace podem ser obtidos na GetAllRequestRace que pertence a mesma seção RequestRace

 - Eu como entregador quero efetuar a entrega do pedido.
    - Para o entregador entregar um pedido utilize o método DeliverRequestRace da seção RequestRace, os dados do campo idRequestRace podem ser obtidos na GetAllRequestRace que pertence a mesma seção RequestRace

- Eu como admin quero consultar todos entregadoeres que foram notificados de um pedido.
    - Para visualizar todos os entregadores que receberam notificação de novos pedido utilize o método GetAllNotificattionsDeliveryDrivers da seção RequestRace

## Considerações finais
Para efeito de conhecimento foi utilizado o Trello para gerenciamento de tarefas.
