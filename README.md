# Event Driven Architecture

This project will demonstrate buliding an application using an event driven architecture.

For an overview of the concept, please read the blog article [here](https://medium.com/trimble-maps-engineering-blog/event-driven-architecture-3763c8f1ed77).

## Prerequisites

1. .NET Core 2.2 or higher
2. RabbitMQ (https://www.rabbitmq.com/)

## Installing RabbitMQ

## What it does

This project consists of two solutions:  a publisher API and a consumer. We'll first look at the publisher.

The publisher is a simple .NET Core Web API project. It has a single POST endpoint that takes in a GPS position object. When the API receives the object, it places it into a queue.

The consumer is a .NET Core console application that reads from a queue. Once it gets a message from the queue, it does something interesting with it. You can run mulitple consumers at once. The RabbitMQ server will distribute the messages to any consumers that are registered with it in a round-robin fashion. Once a consumer sends an acknowledgement back to the RabbitMQ server, the server knows to no longer send that message anywhere. If the server doesn't receive an ack, the message gets put back in the queue and delivered to another consumer.


## How to run the project(s)

Open the solution and hit F5.

