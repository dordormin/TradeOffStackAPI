# Security notes — TradeOffStackAPI

## Secrets handling (since `security/hardening-2026-10-11`)

- **No secrets are committed to this repository.** `appsettings.json` contains
  only placeholders for `CloudflareR2:AccessKey` / `SecretKey` and `Jwt:Key`.
- All production secrets are injected via **environment variables** (see
  `.env.example`, never commit `.env`):
  | Variable | Maps to | Required |
  |---|---|---|
  | `POSTGRES_PASSWORD` | DB password | yes (fail-fast) |
  | `JWT_SECRET_KEY` | `Jwt:Key` (min 32 chars) | yes (fail-fast) |
  | `R2_ACCESS_KEY` / `R2_SECRET_KEY` | `CloudflareR2:*` | yes (fail-fast) |
  | `SEED_ADMIN_PASSWORD` / `SEED_TESTER_PASSWORD` | initial accounts (min 12 chars) | no — random one-time password generated & logged once |
- The application **refuses to start** if `Jwt:Key` is missing, shorter than
  32 characters, or still the placeholder value.
- `docker-compose.prod.yml` uses `${VAR:?message}` so a missing secret fails
  the deploy loudly instead of silently falling back to a known default.

## ⚠️ Required manual action — revoke the exposed R2 keys

The Cloudflare R2 keys previously committed in `appsettings.json` were visible
to anyone with read access to this repository. They **must be revoked** in the
Cloudflare dashboard (R2 → Manage R2 API Tokens → delete the token), then
fresh keys issued and provided via `R2_ACCESS_KEY` / `R2_SECRET_KEY`.

## Authentication

- JWT Bearer, validated with issuer/audience/lifetime/signature (`ClockSkew = 0`).
- Browsers authenticate via the **HttpOnly `tos_auth` cookie** (`Secure`,
  `SameSite=Lax`) issued by `POST /api/auth/login`; `POST /api/auth/logout`
  clears it. The `Authorization: Bearer` header remains supported as a fallback
  for non-browser clients and plain-HTTP development.
- CORS is restricted to `https://tradeoffstack.dordorapps.tech` with
  `AllowCredentials` (required for the cookie flow).
- Seed accounts (`admin@` / `tester@`) are created **only on a fresh database**;
  existing deployments keep their current passwords — rotate them manually.

## Remaining hardening roadmap

- [ ] Content-Security-Policy on the frontend (after auditing script sources)
- [ ] Refresh-token rotation (currently 60-min access tokens only)
- [ ] Magic-bytes validation + size limit on uploads (extension check only today)
- [ ] Restrict `ForwardedHeaders` trusted proxies to the Caddy network
- [ ] Dependency vulnerability scan in CI (`dotnet list package --vulnerable`)
