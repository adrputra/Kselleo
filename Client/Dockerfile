# Use Node.js for building the client
FROM node:14 AS build
WORKDIR /app

# Copy and install dependencies
COPY Client/package.json Client/package-lock.json ./
RUN npm install

# Copy source code and build
COPY Client/. ./
RUN npm run build

# Use Nginx to serve the client
FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
