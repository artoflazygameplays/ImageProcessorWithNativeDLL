const { env } = require('process');

const target = "https://localhost:7250";


const PROXY_CONFIG = [
  {
    context: ["/api/image"],
    target,
    secure: false
  }
];

module.exports = PROXY_CONFIG;
