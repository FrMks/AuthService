import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  // В Docker backend доступен по имени сервиса authservice, а при локальном
  // запуске frontend без Docker — через localhost.
  async rewrites() {
    return [
      {
        source: "/api/:path*",
        destination: `${process.env.BACKEND_URL ?? "http://localhost:8080"}/:path*`,
      },
    ];
  },

  cacheComponents: true,
  partialPrefetching: true,
  turbopack: {
    rules: {
      "*.css": {
        loaders: ["@tailwindcss/turbopack"],
        as: "*.css",
      },
    },
  },
};

export default nextConfig;
