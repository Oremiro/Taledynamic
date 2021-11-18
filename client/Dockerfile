FROM node:latest as build-stage
WORKDIR /app
ENV VITE_API_BASEURL=http://localhost:5000
COPY package*.json ./
RUN npm install
COPY ./ .
RUN npm run build

FROM nginx as production-stage
RUN mkdir /app
COPY --from=build-stage /app/dist /app
COPY nginx.conf /etc/nginx/nginx.conf