import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  // rewrites() - это функция, которая позволяет перенаправлять запросы с одного URL на другой. В данном случае, все запросы, начинающиеся с "/api/", будут перенаправлены на "http://localhost:8080/". Это полезно для разработки, когда фронтенд и бэкенд работают на разных портах.
  async rewrites() {
    return [
      {
        source: "/api/:path*",
        destination: "http://localhost:8080/:path*",
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