# Codend

## Run application

### Docker run

```bash
docker run -p 8080:80 --name codend codend/codend:latest
```

Visit http://127.0.0.1:8080 in your browser.

### Docker Compose

**Clone repository**

```bash
git clone https://github.com/CodendDev/Codend
cd codend
```

**Start the application**

```bash
docker compose up
```

**Access the application**

Visit http://127.0.0.1:8080 in your browser.

### Docker configuration files

* [nginx config](nginx/nginx.conf)
* [environmental variables](.env)