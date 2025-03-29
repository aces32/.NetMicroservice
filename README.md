# .NET Microservices with gRPC, RabbitMQ & Kubernetes

This project is a hands-on microservices architecture built with ASP.NET Core, inspired by [Les Jackson's YouTube course](https://www.youtube.com/watch?v=DgVjEo3OGBI). It demonstrates key concepts including:

- Service-to-service communication via **gRPC**
- **Asynchronous messaging** with **RabbitMQ**
- Deployment with **Docker** and **Kubernetes**
- Clean architecture and separation of concerns
- Environment-based configuration and service discovery

## Tech Stack

- ASP.NET Core (.NET 8)
- gRPC
- RabbitMQ
- Docker & Docker Compose
- Kubernetes (K8s)
- MongoDB & PostgreSQL
- Entity Framework Core

## Services

- `PlatformService` – Exposes REST + gRPC APIs and sends events
- `CommandService` – Subscribes to events from RabbitMQ and persists data

## Getting Started

To run the project locally:

```bash
docker-compose up --build
