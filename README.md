# What is this?

**ScyCasino** is a demo project showcasing the operation of distributed systems in real-time gaming environments.

The project highlights the identification flow (enrollment/registration) using the OIDC protocol, real-time communication for room and game state management via **WebSockets** and **SignalR**, and audio/video streaming in a serverless format using **WebRTC**. It also demonstrates inter-service transactions with compensation through **Routing Slips**.

The project is designed following principles of **Domain-Driven Design, Clean Architecture, CQRS**, and more.

<video src="https://github.com/Scylin232/ScyCasino/blob/master/Demonstration/demonstration.mp4" width="300" />

# Motivation

1. To demonstrate real-time dependent communication (SignalR) in distributed systems.
2. To showcase the use of [Routing Slip in MassTransit](https://masstransit.io/documentation/concepts/routing-slips).
3. To explore integrating a new identity server solution: [Authentik](https://github.com/goauthentik/authentik/).
4. To conduct field testing for [ScyScaff](https://github.com/Scylin232/scyscaff).

# Architecture

![Architecture](/Demonstration/architecture.jpg)

The initial template was scaffolded using [Scylin's Scaffolder](https://github.com/Scylin232/scyscaff).

# How to Launch

1. Create a `.env` file with the required values:
```bash
echo "PG_PASS=$(openssl rand -base64 36 | tr -d '\n')" >> .env
echo "AUTHENTIK_SECRET_KEY=$(openssl rand -base64 60 | tr -d '\n')" >> .env
```
2. Run the following command:
```bash
docker-compose up -d
```
3. Wait for **Authentik** to complete all migrations.
4. Set up an admin profile at:  
   ```http://localhost:9000/if/flow/initial-setup/```
5. Assign the profile the roles **"ScyCasino Member/Administrator"**  
   - Alternatively, register a new profile via the **Frontend**.
6. Access the **Frontend** at:  
   ```http://localhost:4200/```
7. Log in to your profile.

# Attention

This repository is distributed under the [CC BY-NC 4.0](https://creativecommons.org/licenses/by-nc/4.0/deed.en) license, which prohibits any form of commercial use. It was created purely for demonstration purposes and fun.