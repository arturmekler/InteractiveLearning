# interactive-learning/Dockerfile
FROM node:18-alpine AS development

WORKDIR /app

# Kopiuj pliki package
COPY package*.json ./

# Zainstaluj zależności
RUN npm ci

# Kopiuj kod źródłowy
COPY . .

EXPOSE 3000

# Ustaw zmienną środowiskową dla development
ENV NODE_ENV=development

# Uruchom aplikację
CMD ["npm", "start"]
