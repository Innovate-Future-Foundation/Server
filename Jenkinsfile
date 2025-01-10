pipeline {
    agent any

    environment {
        DOCKER_BUILDKIT = '1'
        COMPOSE_PROJECT_NAME = "${env.JOB_NAME}-${env.BUILD_ID}"
        DOCKER_CACHE_TTL = '24h'
        HEALTH_CHECK_RETRIES = '5'
        HEALTH_CHECK_INTERVAL = '5'
        DB_INIT_TIMEOUT = '30'
        EC2_HOST = credentials('EC2_HOST')
        API_URL = "http://${EC2_HOST}:5091"
        PGADMIN_URL = "http://${EC2_HOST}:5050"
    }

    options {
        timestamps()
        timeout(time: 30, unit: 'MINUTES')
        disableConcurrentBuilds()
    }

    stages {
        stage('Checkout') {
            steps {
                cleanWs()
                checkout scm
                sh '''
                    git log -1
                    git status
                '''
            }
        }

        stage('Pre-build Cleanup') {
            steps {
                script {
                    sh '''
                        echo "Starting cleanup process..."
                        docker compose down || true
                        docker system prune -f --filter "until=${DOCKER_CACHE_TTL}" || true
                        docker volume prune -f || true
                        docker network prune -f || true
                    '''
                }
            }
        }

        stage('Build and Test') {
            steps {
                script {
                    try {
                        sh '''
                            docker compose build \
                                --build-arg BUILDKIT_INLINE_CACHE=1 \
                                --build-arg CACHE_DATE="$(date)" \
                                base api
                            
                            docker compose images
                        '''
                    } catch (Exception e) {
                        error "Build failed: ${e.message}"
                    }
                }
            }
        }

        stage('Database Setup') {
            steps {
                script {
                    try {
                        sh '''
                            docker compose up -d postgres
                            
                            echo "Waiting for database to be ready..."
                            COUNTER=0
                            until docker compose exec postgres pg_isready -h localhost || [ $COUNTER -eq $DB_INIT_TIMEOUT ]; do
                                COUNTER=$((COUNTER+1))
                                echo "Attempt $COUNTER/$DB_INIT_TIMEOUT: Database not ready..."
                                sleep 2
                            done

                            if [ $COUNTER -eq $DB_INIT_TIMEOUT ]; then
                                echo "ERROR: Database failed to initialize"
                                docker compose logs postgres
                                exit 1
                            fi
                        '''
                    } catch (Exception e) {
                        error "Database setup failed: ${e.message}"
                    }
                }
            }
        }
    }

    post {
        always {
            cleanWs()
        }
        failure {
            script {
                sh '''
                    echo "=== Failure Analysis ==="
                    docker compose ps
                    docker compose logs
                '''
            }
        }
    }
}
