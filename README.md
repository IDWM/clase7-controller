# Clase 7 - Product API

API REST de productos con arquitectura de 3 capas:

- Controller
- Service
- Repository

## Configuración inicial

1. Crear archivo de configuración desde el ejemplo:

```bash
cp appsettings.Example.json appsettings.json
```

1. Restaurar paquetes:

```bash
dotnet restore
```

## Ejecución

```bash
dotnet run
```

## Ruta base

`/api/v1/product`

## Endpoints

- `GET /api/v1/product?page=1&pageSize=10`
- `GET /api/v1/product/{id}`
- `POST /api/v1/product`
- `PATCH /api/v1/product/{id}`
- `DELETE /api/v1/product/{id}`

## Swagger

Disponible en:

- `/swagger`

## MiniProfiler

Disponible en:

- `/profiler/results-index`

