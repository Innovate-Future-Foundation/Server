#!/bin/bash

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
cd "$SCRIPT_DIR/.."  

source .env

case "$1" in
    "backup")
        mkdir -p db/backups
        timestamp=$(date +%Y%m%d_%H%M%S)
        docker exec container-postgres pg_dump -U "$DB_USER" "$DB_NAME" > "db/backups/backup_${timestamp}.sql"
        ;;
    "restore")
        if [ -z "$2" ]; then
            echo "Please provide backup file name from db/backups/"
            exit 1
        fi
        docker exec -i container-postgres psql -U "$DB_USER" "$DB_NAME" < "db/backups/$2"
        ;;
    *)
        echo "Usage: $0 {backup|restore <filename>}"
        exit 1
        ;;
esac