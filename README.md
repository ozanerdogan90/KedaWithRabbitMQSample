# Keda with RabbitMq Example 

> .Net Core 3.1, RabbitMq, MediatR, Keda

## Description
A simple docker container that will receive messages from a RabbitMQ queue and scale via KEDA.  The reciever will receive a single message at a time (per instance), and sleep for 1 second to simulate performing work.  When adding a massive amount of queue messages, KEDA will drive the container to scale out according to the event source (RabbitMQ).

More about Event driven scaling : [Keda](https://github.com/kedacore/keda#getting-started)

## Features
##### Framework
- .Net Core


## Requirements
- .Net Core >= 3.1
- Helm
- Kubernetes cluster


## Setup

This setup will go through creating a RabbitMQ queue on the cluster and deploying this consumer with the `ScaledObject` to scale via KEDA.  If you already have RabbitMQ you can use your existing queues.

First you should clone the project:

```cli
git clone https://github.com/ozanerdogan90/KedaWithRabbitMQSample.git
cd KedaWithRabbitMQSample
```

### Creating a RabbitMQ queue

#### [Install Helm](https://helm.sh/docs/using_helm/)

```cli
cd deploy
run rabbitmq-install.sh
```

⚠️ Be sure to wait until the deployment has completed before continuing.  

```cli
kubectl get po

NAME         READY   STATUS    RESTARTS   AGE
rabbitmq-0   1/1     Running   0          3m3s
```
 
### Installing Keda to Kubernetes cluster

```cli
cd deploy
run keda-install.sh
``` 

### Deploying a RabbitMQ consumer and producer

```cli
cd deploy
run run.sh
``` 

### Creating load

#### [Install Hey](https://github.com/rakyll/hey)

```cli
hey -n 500 -c 500 -m POST -T "application/json"  -H "Content-Type: application/json"  "http://localhost:5000/orders" -d ""
``` 

this will create 500 items in RabbitMq

#### Validate the deployment scales

```cli
kubectl get deploy -w
```
