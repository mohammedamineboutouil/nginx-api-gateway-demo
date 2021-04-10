FROM node:12 as base
RUN npm i @angular/cli -g

FROM base as build
WORKDIR /src
COPY ["app/package.json", "package.json"]
RUN npm i
COPY app .
RUN ng build --prod --base-href /app/

FROM nginx:alpine
COPY nginx.d /etc/nginx
COPY ["app/src/favicon.ico", "/etc/nginx/html/favicon.ico"]
COPY --from=build /src/dist/app /usr/share/nginx/html/app

