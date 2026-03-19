# Deployment Notes

## Local container run

```bash
docker compose up --build
```

App URL:

```text
http://localhost:8080
```

## Environment overrides

The app reads the connection string from configuration. In Docker, it is overridden with:

```text
ConnectionStrings__DefaultConnection=Data Source=/app/data/intellicasepro.db
```

That keeps the SQLite file outside the published app folder and makes it easy to persist via a volume mount.

## Suggested next hosting targets

- Azure App Service
- Render
- Railway
- Docker-capable VPS
