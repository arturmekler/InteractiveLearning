#!/bin/bash

echo "Budowanie i uruchamianie aplikacji w Docker..."

# Utwórz katalog dla certyfikatów HTTPS jeśli nie istnieje
mkdir -p ~/.aspnet/https

# Sprawdź czy certyfikat istnieje
if [ ! -f ~/.aspnet/https/aspnetapp.pfx ]; then
    echo "Tworzenie certyfikatu HTTPS..."
    
    # Najpierw wyczyść stare certyfikaty
    dotnet dev-certs https --clean
    
    # Wygeneruj i eksportuj nowy certyfikat
    dotnet dev-certs https -ep ~/.aspnet/https/aspnetapp.pfx -p YourPassword123
    
    # Ufaj certyfikatowi
    dotnet dev-certs https --trust
fi

# Uruchom z HTTPS
docker compose -f docker-compose.yml up --build
