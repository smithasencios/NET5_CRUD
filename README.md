# CRUD de producto
Este proyecto usa Postgress, Redis, Elasticsearch, NetCore, Docker

## Pasos para ejecutarlo localmente
Ubicarse en la raiz del proyecto y ejecutar el siguiente comando:

```
docker-compose up
```
Invocar la siguiente URL (GET), debe devolver HTTP Status code 200

```
http://localhost:5103/health
```

## Documentacion de los endpoints

```
http://localhost:5103/swagger
```

## Endpoints

### POST Crear Producto

URL 
```
http://localhost:5103/product
```
Body
```json
{
    "description": "esta es la nueva 5",
    "typeId": 1,
    "StateId": 1,
    "Stock": 4244,
    "Price": 42
}
```

### PUT update Producto
URL 
```
http://localhost:5103/product/1
```
Body
```json
{
    "description": "esta es la nueva 5",
    "Stock": 4244,
    "Price": 42
}
```

### GET Producto
URL 
```
http://localhost:5103/product/1
```
