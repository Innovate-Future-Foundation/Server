pipeline {
    agent { label 'worker-node-inff-server' }
    environment {
        ENV_FILE = credentials('env-file') // 
    }
    stages {
        stage('Prepare Environment') {
            steps {
                script {
                    // 将 ENV_FILE 写入 .env 文件并加载
                    sh '''
                    echo "$ENV_FILE" > .env
                    export $(cat .env | xargs)
                    '''
                }
            }
        }
        stage('Build API Docker Image') {
            steps {
                script {
                    // Build backend Docker image
                    sh '''
                    docker build -t backend-service:latest -f Dockerfile.be .
                    '''
                }
            }
        }
        stage('Run Database Container') {
            steps {
                script {
                    // Start Postgres database
                    sh '''
                    docker run -d --name container-postgres \
                        -e POSTGRES_USER=$DB_USER \
                        -e POSTGRES_PASSWORD=$DB_PASS \
                        -e POSTGRES_DB=$DB_NAME \
                        postgres:17.2
                    '''
                }
            }
        }
        stage('Run Migration') {
            steps {
                script {
                    // Build and run migration container
                    sh '''
                    docker build -t migration-service:latest -f Dockerfile.migration .
                    docker run --rm --name migration-service \
                        --env DBConnection=Host=$DB_HOST;Port=$DB_PORT;Database=$DB_NAME;Username=$DB_USER;Password=$DB_PASS; \
                        --env ASPNETCORE_ENVIRONMENT=$DEP_ENV \
                        migration-service:latest
                    '''
                }
            }
        }
        stage('Run API Service') {
            steps {
                script {
                    // Start backend API service
                    sh '''
                    docker run -d --name backend-service \
                        -p 5091:5091 \
                        --env DBConnection=Host=$DB_HOST;Port=$DB_PORT;Database=$DB_NAME;Username=$DB_USER;Password=$DB_PASS; \
                        --env JWTConfig__SecretKey=$JWT_SECRET \
                        --env ASPNETCORE_ENVIRONMENT=$DEP_ENV \
                        --env ASPNETCORE_URLS=http://+:5091/ \
                        backend-service:latest
                    '''
                }
            }
        }
        stage('Run pgAdmin') {
            steps {
                script {
                    // Start pgAdmin
                    sh '''
                    docker run -d --name container-pgadmin \
                        -p 5050:80 \
                        -e PGADMIN_DEFAULT_EMAIL=$PG_USER \
                        -e PGADMIN_DEFAULT_PASSWORD=$PG_PASS \
                        dpage/pgadmin4
                    '''
                }
            }
        }
    }
    post {
        always {
            echo "Pipeline execution complete."
        }
        success {
            echo "Pipeline executed successfully."
        }
        failure {
            echo "Pipeline failed. Cleaning up..."
            // Clean up all related containers and images
            sh '''
            docker rm -f backend-service container-postgres container-pgadmin migration-service || true
            docker rmi backend-service:latest migration-service:latest || true
            '''
        }
    }
}
