const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://ucare-tgt.ddns.net:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:1900';

const PROXY_CONFIG = [
  {
    context: [
      "/api",
      "/monitor_hub",
    ],
    target: target,
    secure: false,
    ws: true
  }
]

module.exports = PROXY_CONFIG;
