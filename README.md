# Event Driven Architecture

This project will demonstrate building an application using an event driven architecture.

For an overview of the concept, please read the blog article [here](https://medium.com/trimble-maps-engineering-blog/event-driven-architecture-3763c8f1ed77).

## Prerequisites

1. .NET Core 2.2 or higher
2. RabbitMQ (https://www.rabbitmq.com/)

## What it does

This project consists of two solutions:  a publisher API and a consumer. We'll first look at the publisher.

The publisher is a simple .NET Core Web API project. It has a single POST endpoint that takes in a GPS position object. When the API receives the object, it places it into a queue.

The consumer is a .NET Core console application that reads from a queue. Once it gets a message from the queue, it does something interesting with it. You can run multiple consumers at once. The RabbitMQ server will distribute the messages to any consumers that are registered with it in a round-robin fashion. Once a consumer sends an acknowledgement back to the RabbitMQ server, the server knows to no longer send that message anywhere. If the server doesn't receive an ack, the message gets put back in the queue and delivered to another consumer.

## Installing RabbitMQ

These steps will install RabbitMQ on your local computer. Of course, you'll want to run it on a server for real development. I suggest you follow [the instructions on RabbitMQ's](https://www.rabbitmq.com/install-windows.html) site, but here is the CliffNotes version.

1. Download and install [Erlang](https://www.erlang.org/downloads).
2. Download and install RabbitMQ.
3. Enable the RabbitMQ management UI.
    1. Navigate to `C:\Program Files\RabbitMQ Server\rabbitmq_server-3.8.2\sbin`
    2. Enable the management plugin by executing
    ```rabbitmq-plugins enable rabbitmq_management```
    3. Restart your RabbitMQ server by running
    ```rabbitmq-server -detached```
    4. Navigate to `http://localhost:15672/` and authenticate with the username/password `guest/guest`.
    5. Success!

4. Create a user for this demo.
    1. Navigate to `C:\Program Files\RabbitMQ Server\rabbitmq_server-3.8.2\sbin`
    2. Execute the following commands to create a user `meetup` and give it access to any queue on the server.

    ```bash
    rabbitmqctl add_user meetup demo
    rabbitmqctl set_permissions -p / meetup ".*" ".*" ".*"
    ```

5. That's it! We don't have to create any queues because our app will check for the queue and create one on demand if it doesn't exist.

## How to run the project(s)

Open the solution and hit F5.

## Testing

Included in this repo is a [Postman](https://www.getpostman.com/) collection with a sample input.